﻿using System;

namespace Hnx8.ReadJEnc
{
    /// <summary>
    /// ReadJEnc 文字コード自動判別ライブラリ・処理本体(Rev.20141206)
    /// </summary>
    public class ReadJEnc
    {   ////////////////////////////////////////////////////////////////////////
        // <ReadJEnc.cs> ReadJEnc 文字コード自動判別処理本体(Rev.20141206)
        //  Copyright (C) 2014 hnx8(H.Takahashi)
        //  http://hp.vector.co.jp/authors/VA055804/
        //
        //  Released under the MIT license
        //  http://opensource.org/licenses/mit-license.php
        ////////////////////////////////////////////////////////////////////////

        //言語別文字コード自動判別オブジェクト==================================

        /// <summary>ANSI文字コード自動判別(CP1252)</summary>
        public static ReadJEnc ANSI = new ReadJEnc(CharCode.ANSI, null);
        /// <summary>日本語文字コード自動判別(SJIS/EUCJP)</summary>
        public static ReadJEnc JP = new SJIS();

#if (!JPONLY) //漢字圏テキスト文字コード自動判別（日本語判別以外使用しないなら定義省略可）
        /// <summary>繁体字中国語文字コード自動判別(BIG5TW/EUCTW)</summary>
        public static ReadJEnc TW = new BIG5TW();
        /// <summary>簡体字中国語文字コード自動判別(GB18030/EUCCN)</summary>
        public static ReadJEnc CN = new GB18030();
        /// <summary>韓国語文字コード自動判別(UHCKR/EUCKR)</summary>
        public static ReadJEnc KR = new UHCKR();
#endif

        //文字コード判別メソッド================================================

        /// <summary>バイト配列を全走査し、文字コードを自動判別する</summary>
        /// <param name="Bytes">判定対象のバイト配列</param>
        /// <param name="Length">ファイルサイズ(バイト配列先頭からのデコード対象バイト数)</param>
        /// <param name="Text">out 判別した文字コードにより取り出したテキスト文字列（非テキストならnull）</param>
        /// <returns>文字コード判別結果（非テキストならnull）</returns>
        public CharCode GetEncoding(byte[] Bytes, int Length, out string Text)
        {
            byte b1 = (Length > 0) ? Bytes[0] : (byte)0; //汎用バイトデータ読み取り変数初期化
            byte b2;

            //【1】7bit文字コードの範囲の走査(ASCII判定/非ASCII文字開始位置把握)、およびUTF16N/JISチェック
            int escapeSequenceScore = 0; //JISエスケープシーケンス評価変数
            int escapeCount = 0; //0x1B<ESC>単純カウント変数
            int asciiEndPos = 0; //ループ変数、兼、非ASCII文字を初めて検出した位置
            while (b1 < DEL) //非ASCII文字が出現したらループ脱出：b1にはあらかじめ読み込み済
            {
                if (b1 <= BINARY)
                {   //バイナリ文字検出：先頭２バイトでの検出ならUTF16Nの可能性をチェック、否ならバイナリ確定
                    CharCode ret = (asciiEndPos < 2 ? SeemsUTF16N(Bytes, Length) : null);
                    if (ret != null && (Text = ret.GetString(Bytes, Length)) != null)
                    {   //UTF16Nデコード成功：非テキスト文字混入チェック
                        int i;
                        for (i = -3; i <= BINARY; i++)
                        {   //0xFFFD,0xFFFE,0xFFFF,0～BINARY、DELが混入している場合は非テキストとみなす
                            if (Text.IndexOf((char)i, 0, Text.Length) != -1) { break; }
                        }
                        if (i > BINARY && Text.IndexOf((char)DEL, 0, Text.Length) == -1)
                        {   //■UTF16N確定（非テキスト文字混入なし）
                            return ret;
                        }
                    }
                    Text = null;
                    return null; //■バイナリ確定
                }
                if (b1 == 0x1B)
                {   //エスケープシーケンス判定
                    escapeCount++;
                    b2 = JIS.isEscape(Bytes, Length, asciiEndPos);
                    escapeSequenceScore += b2;
                    asciiEndPos += b2;
                }
                //次の文字へ
                if ((++asciiEndPos) >= Length)
                {   //全文字チェック完了：非ASCII文字未検出、JISもしくはASCII
                    if (escapeSequenceScore > escapeCount)
                    {   //■エスケープシーケンス有効、JIS確定(詳細種別を判定する)
                        return JIS.GetEncoding(Bytes, Length, out Text);
                    }
                    if (JIS.hasSOSI(Bytes, Length))
                    {   //SO,SIによるエスケープを検出した場合は、ISO-2022-KR/半角カナJISの可能性を判定
                        int escPos = Array.IndexOf<byte>(Bytes, 0x1B, 0, Length);
                        if (escPos >= 0 && escPos < Length - 4
                            && Bytes[escPos + 1] == '$'
                            && Bytes[escPos + 2] == ')'
                            && Bytes[escPos + 3] == 'C'
                            && (Text = CharCode.ISOKR.GetString(Bytes, Length)) != null)
                        {   //■KSエスケープシーケンスあり、ISO-2022-KRで確定(半角カナJISではない)
                            return CharCode.ISOKR;
                        }
                        if (escapeCount == 0 && (Text = CharCode.JIS50222.GetString(Bytes, Length)) != null)
                        {   //■エスケープシーケンスなし、半角カナSOSIのみを使用したJISで確定
                            return CharCode.JIS50222;
                        }
                    }
                    //■ASCII確定（ただしデコード失敗時はバイナリ）
                    return ((Text = CharCode.ASCII.GetString(Bytes, Length)) != null) ? CharCode.ASCII : null;
                }
                b1 = Bytes[asciiEndPos];
            }

            //【2】非ASCII文字を含む範囲の走査、CP1252/UTF8/EUCチェック、JIS残チェック
            int cp1252Score = 0; //いずれも、可能性が否定されたらint.MinValueが設定される
            int utfScore = 0;
            int eucScore = (EUC == null ? int.MinValue : 0);
            int sjisScore = (CharCode == null ? int.MinValue : 0);
            bool existsEUC0x8F = false; //EUC補助漢字を見つけたらtrueを設定

            for (int cp1252Pos = asciiEndPos; cp1252Pos < Length; ) //cp1252Posの加算はロジック途中で随時実施
            {
                if (b1 == DEL)
                {   //制御文字0x7F登場なら、ごくわずかなJISの可能性以外全消滅。JISの可能性を消しきれるか判定
                    cp1252Score = int.MinValue;
                    utfScore = int.MinValue;
                    eucScore = int.MinValue;
                    sjisScore = int.MinValue;
                    if (escapeSequenceScore == 0 || (cp1252Pos++) >= Length || (b1 = Bytes[cp1252Pos]) < 0x21 || b1 >= DEL)
                    {   //JISエスケープ未出現 or ファイル末尾で2バイト目なし or 2バイト目が0x21-0x7E範囲外ならJISの可能性も否定
                        Text = null;
                        return null; //■バイナリ確定
                    }
                }
                //CP1252チェック＆0x80以上の文字範囲の把握(notAsciiStartPos～cp1252Pos)。b1読込済
                int notAsciiStart = cp1252Pos;
                switch (cp1252Score)
                {
                    case int.MinValue: //CP1252可能性否定済み、非ASCII文字のスキップのみ実施
                        while (b1 > DEL && (++cp1252Pos) < Length) { b1 = Bytes[cp1252Pos]; }
                        break;
                    default: //CP1252可能性あり、定義外文字混入チェック＆CP1252ポイント加算
                        while (b1 > DEL)
                        {   // 非CP1252チェック用定義(0x2001A002)：未定義の81,8D,8F,90,9Dに対応するビットがON
                            //        FEDC BA98 7654 3210         FEDC BA98 7654 3210
                            //        ---- ---- ---- ----         ---- ---- ---- ----
                            // (0x9#) 0010 0000 0000 0001  (0x8#) 1010 0000 0000 0010
                            if (b1 <= 0x9D && (0x2001A002 & (1u << (b1 % 32))) != 0)
                            {   // CP1252未定義文字を検出、可能性消滅
                                cp1252Score = int.MinValue;
                                goto case int.MinValue; //非ASCII文字スキップへ
                            }
                            if ((++cp1252Pos) >= Length) { break; }
                            b1 = Bytes[cp1252Pos];
                        }
                        //非ASCII文字範囲終了、評価ポイント加算
                        //１バイトのみ出現時（SJISよりもCP1252の可能性が高い）、SJIS漢字1文字目と同評価・SJISカナよりも高評価となるようポイント加算
                        if (cp1252Pos == notAsciiStart + 1) { cp1252Score += 2; }
                        else if (cp1252Pos == notAsciiStart + 2 && (b2 = Bytes[cp1252Pos - 1]) >= 0xC0)
                        {   //２バイトのみ出現時、ダイアクリティカルマーク（発音記号等）つきアルファベットなら配点補正
                            if (b2 == (b2 = Bytes[cp1252Pos - 2])) { cp1252Score += 5; } //同一文字重ねはかなり特徴的(SJISカナより可能性高)
                            else if (b2 >= 0xC0)
                            {   //続きor直前のASCII文字がアルファベットっぽければ、SJISカナより可能性が高くなるよう補正                               
                                if (b1 > 0x40 || (notAsciiStart > 0 && Bytes[notAsciiStart - 1] > 0x40)) { cp1252Score += 5; }
                                else { cp1252Score += 3; } //どちらでもなければ、EUCよりは可能性高とする
                            }
                            else { cp1252Score++; } //否ならば低めの加算とする
                        }
                        else { cp1252Score++; } //いずれにも該当しなければやや低めの加算とする
                        break;
                }
                //notAsciiStartPos～cp1252Pos範囲のUTF8チェック
                if (utfScore >= 0)
                {
                    bool prevIsKanji = false;
                    for (int utfPos = notAsciiStart; utfPos < cp1252Pos; utfPos++)
                    {
                        b1 = Bytes[utfPos];
                        //1バイト目・２バイト目(ともに0x80以上であることは確認済み)をチェック
                        if (b1 < 0xC2 || (++utfPos) >= cp1252Pos || Bytes[utfPos] > 0xBF) { utfScore = int.MinValue; break; } //UTF8可能性消滅
                        else if (b1 < 0xE0)
                        {   //２バイト文字OK（半角文字とみなして評価）
                            if (prevIsKanji == false) { utfScore += 6; } else { utfScore += 2; prevIsKanji = false; }
                        }
                        //3バイト目(0x80以上であることは確認済み)をチェック
                        else if ((++utfPos) >= cp1252Pos || Bytes[utfPos] > 0xBF) { utfScore = int.MinValue; break; } //UTF8可能性消滅
                        else if (b1 < 0xF0)
                        {   //３バイト文字OK（全角文字とみなして評価）
                            if (prevIsKanji == true) { utfScore += 8; } else { utfScore += 4; prevIsKanji = true; }
                        }
                        //4バイト目(0x80以上であることは確認済み)をチェック
                        else if ((++utfPos) >= cp1252Pos || Bytes[utfPos] > 0xBF) { utfScore = int.MinValue; break; } //UTF8可能性消滅
                        else if (b1 < 0xF5)
                        {   //４バイト文字OK（全角文字とみなして評価）
                            if (prevIsKanji == true) { utfScore += 12; } else { utfScore += 6; prevIsKanji = true; }
                        }
                        else { utfScore = int.MinValue; break; } //UTF8可能性消滅(0xF5以降はUTF8未定義)
                    }
                }
                //notAsciiStartPos～cp1252Pos範囲のEUCチェック ※EUCの文字コード範囲はEUC-JP/TW/CN/KRでほぼ共通
                if (eucScore >= 0)
                {   //前の文字との連続性チェック用定数定義
                    const int PREV_KANA = 1; //直前文字は半角カナ
                    const int PREV_ZENKAKU = 2; //直前文字は全角
                    int prevChar = 0; //前の文字はKANAでもZENKAKUでもない
                    for (int eucPos = notAsciiStart; eucPos < cp1252Pos; eucPos++)
                    {   //１バイト目(0xA1-0xFE,0x8E,0x8F)・２バイト目(１バイト目に応じ範囲が異なる)のチェック
                        b1 = Bytes[eucPos];
                        if (b1 == 0xFF || (++eucPos) >= cp1252Pos) { eucScore = int.MinValue; break; } //EUC可能性消滅
                        b2 = Bytes[eucPos];
                        if (b1 >= 0xA1)
                        {   //１バイト目＝全角文字指定、２バイト全角文字チェック
                            if (b2 < 0xA1 || b2 == 0xFF) { eucScore = int.MinValue; break; } //EUC可能性消滅
                            //２バイト文字OK（全角）
                            if (prevChar == PREV_ZENKAKU) { eucScore += 5; } else { eucScore += 2; prevChar = PREV_ZENKAKU; }
                        }
                        else if (b1 == 0x8E)
                        {   //１バイト目＝かな文字(orEUC-TWの４バイト文字)指定。２バイトの半角カナ文字チェック
                            if (b2 < 0xA1 || b2 > 0xDF) { eucScore = int.MinValue; break; } //EUC可能性消滅
                            //検出OK,EUC文字数を加算（半角文字）
                            if (prevChar == PREV_KANA) { eucScore += 6; }
                            //漢字圏テキスト文字コードのうちEUC-TWに限り全角文字相当の扱いとする(0x8E,0xA2-0xB0,0xA1-0xFE,0xA1-0xFEの４バイト文字の判定に流用)
                            else if (EUCTW) { if (prevChar == PREV_ZENKAKU) { eucScore += 6; } else { eucScore += 2; prevChar = PREV_ZENKAKU; } }
                            else { eucScore += 2; prevChar = PREV_KANA; }
                        }
                        else if (b1 == 0x8F
                            && b2 >= 0xA1 && b2 < 0xFF
                            && (++eucPos) < cp1252Pos
                            && (b2 = Bytes[eucPos]) >= 0xA1 && b2 < 0xFF)
                        {   //残る可能性は３バイト文字：検出OKならEUC文字数を加算（全角文字、補助漢字）
                            if (prevChar == PREV_ZENKAKU) { eucScore += 8; } else { eucScore += 3; prevChar = PREV_ZENKAKU; }
                            existsEUC0x8F = true; //※補助漢字有
                        }
                        else { eucScore = int.MinValue; break; } //EUC可能性消滅
                    }
                }

                //ASCII文字範囲の読み飛ばし＆バイナリチェック＆JISチェック、b1に非ASCII文字出現位置のバイト値を格納
                while (cp1252Pos < Length && (b1 = Bytes[cp1252Pos]) < DEL)
                {
                    if (b1 <= BINARY)
                    {   //■バイナリ確定
                        Text = null;
                        return null;
                    }
                    if (b1 == 0x1B)
                    {   //エスケープシーケンス判定
                        escapeCount++;
                        b2 = JIS.isEscape(Bytes, Length, cp1252Pos);
                        escapeSequenceScore += b2;
                        cp1252Pos += b2;
                    }
                    cp1252Pos++;
                }
            }

            //【3】SJISなどの各国語文字コードチェック（非ASCII登場位置からチェック開始:ただしDEL検出時などは可能性なし）
            if (sjisScore != int.MinValue)
            {
                sjisScore = GetEncoding(Bytes, asciiEndPos, Length);
            }

            //【4】ポイントに応じ文字コードを決定（実際にそのエンコーディングで読み出し成功すればOKとみなす）
            if (escapeSequenceScore > escapeCount)
            {   //JISエスケープシーケンス出現
                return JIS.GetEncoding(Bytes, Length, out Text);  //■JIS確定(詳細種別を判定する)
            }
            if (eucScore > 0 && eucScore > sjisScore && eucScore > utfScore)
            {   //EUC可能性高
                if (cp1252Score > eucScore)
                {   //ただしCP1252の可能性が高ければCP1252を先にチェック
                    if ((Text = CharCode.ANSI.GetString(Bytes, Length)) != null) { return CharCode.ANSI; } //■CP1252で読みこみ成功
                }
                if (existsEUC0x8F && (Text = CharCode.EUCH.GetString(Bytes, Length)) != null) { return CharCode.EUCH; }//■EUC補助漢字読みこみ成功
                if ((Text = EUC.GetString(Bytes, Length)) != null) { return EUC; } //■EUCで読みこみ成功
            }
            if (utfScore > 0 && utfScore >= sjisScore)
            {   //UTF可能性高
                if ((Text = CharCode.UTF8N.GetString(Bytes, Length)) != null) { return CharCode.UTF8N; } //■UTF-8Nで読みこみ成功
            }
            if (sjisScore >= 0)
            {   //SJISなどの各国語指定に合致したなら、そのコードでの読み出しを試みる(ただしCP1252の可能性が高ければCP1252を先にチェック)
                if (cp1252Score > sjisScore && (Text = CharCode.ANSI.GetString(Bytes, Length)) != null) { return CharCode.ANSI; } //■CP1252で読みこみ成功
                if ((Text = CharCode.GetString(Bytes, Length)) != null) { return CharCode; } //■指定された文字コード(日本語の場合はSJIS、それ以外の場合はBig5/GB18030/UHC)で読みこみ成功
            }
            if (cp1252Score > 0)
            {   //CP1252の可能性のみ残っているのでチェック
                if ((Text = CharCode.ANSI.GetString(Bytes, Length)) != null) { return CharCode.ANSI; } //■CP1252で読みこみ成功
            }
            //■いずれにも該当しなかった場合は、バイナリファイル扱いとする
            Text = null;
            return null;
        }

        /// <summary>デフォルト文字コードの判定スコア算出（判定開始位置～ファイル末尾までの範囲を対象）</summary>
        /// <param name="Bytes">判定対象のバイト配列</param>
        /// <param name="pos">判定開始位置(非ASCII文字コードが初めて登場した位置)</param>
        /// <param name="Length">ファイルサイズ(バイト配列先頭からのデコード対象バイト数)</param>
        /// <returns>判定スコア算出結果</returns>
        protected virtual int GetEncoding(byte[] Bytes, int pos, int Length)
        {   //デフォルト(ANSI)の場合は常に評価なしとする。オーバーライドで算出ロジックを書き換えること
            return int.MinValue;
        }

        /// <summary>BOMなしUTF16の可能性があるか(先頭文字がASCIIか否かをもとに)判定</summary>
        /// <param name="Bytes">判定対象のバイト配列</param>
        /// <param name="Length">ファイルサイズ(バイト配列先頭からのデコード対象バイト数)</param>
        /// <returns>UTF16Nと思われる場合はその文字コード、否ならnull</returns>
        public static CharCode SeemsUTF16N(byte[] Bytes, int Length)
        {
            if (Length >= 2 && Length % 2 == 0)
            {
                if (Bytes[0] == 0x00)
                {
                    if (Bytes[1] > BINARY && Bytes[1] < DEL && (Length == 2 || Bytes[2] == 0))
                    {   //▲UTF16BigEndianの可能性あり
                        return CharCode.UTF16BE;
                    }
                }
                else if (Bytes[1] == 0x00)
                {
                    if (Bytes[0] > BINARY && Bytes[0] < DEL && (Length == 2 || Bytes[3] == 0))
                    {   //▲UTF16LittleEndianの可能性あり
                        return CharCode.UTF16LE;
                    }
                }
            }
            return null; //UTF16Nの可能性はないと判断
        }

        #region 基本クラス定義--------------------------------------------------
        /// <summary>バイナリと判定するDEL文字コード、兼、ASCII/非ASCIIの境界文字コード</summary>
        const byte DEL = (byte)0x7F;
        /// <summary>非テキストファイルと判定する制御文字コードの最大値</summary>
        const byte BINARY = (byte)0x03; //0x01-0x07位の範囲で調整。0x08(BS)はTeraTerm等ログで出る。0x09(TAB)は普通にテキストで使う。0x03くらいにするのがよい。HNXgrepでは0x03を採用

        /// <summary>自動判別デフォルト文字コード</summary>
        public readonly CharCode CharCode;
        /// <summary>EUCの文字コード</summary>
        public readonly CharCode EUC;
        /// <summary>0x8EのEUC文字を全角文字として判定するか（EUCTWの場合のみtrue）</summary>
        protected bool EUCTW = false;

        /// <summary>基本コンストラクタ</summary>
        /// <param name="Encoding">自動判別デフォルト文字コード</param>
        /// <param name="EUC">EUCの文字コード</param>
        protected ReadJEnc(CharCode Encoding, CharCode EUC)
        {
            this.CharCode = Encoding;
            this.EUC = EUC;
        }
        /// <summary>オブジェクト文字列表現として、自動判別デフォルト文字コードの名前を返す</summary>
        public override string ToString()
        {
            return this.CharCode.Name;
        }
        #endregion

        #region JIS/SJIS判定処理各種--------------------------------------------
        /// <summary>
        /// JIS判定関連処理クラス
        /// </summary>
        private static class JIS
        {
            //※MS版JISでは、ISO/IEC 2022に準じていない0x7F以上のコード(外字や8bitカナなど)が混入してもOKである。
            //  このため、非ASCII文字登場後もエスケープシーケンスのチェックを行う。
            //  CP5022x(CP50220/CP50221/CP50222)の特異なコード体系については、
            //  http://sourceforge.jp/projects/legacy-encoding/lists/archive/talk-ja/2006-March/000002.html も参照

            // また、エスケープシーケンスを伴わない(文字コードSO/SIのみによる)半角カナ切り替えが含まれている場合も、
            // JISとみなしてデコードするほうが妥当であるため、ASCII文字のみ出現の場合にもSOSI有無チェックを行う。

            /// <summary>SO/SI文字出現チェック</summary>
            /// <param name="Bytes">判定対象のバイト配列</param>
            /// <param name="Length">ファイルサイズ(バイト配列先頭からのデコード対象バイト数)</param>
            /// <returns>出現していればtrue、否ならfalse</returns>
            internal static bool hasSOSI(byte[] Bytes, int Length)
            {
                return (Array.IndexOf<byte>(Bytes, 0x0E, 0, Length) >= 0 && Array.IndexOf<byte>(Bytes, 0x0F, 0, Length) >= 0);
            }
            /// <summary>JISエスケープシーケンス妥当性チェック</summary>
            /// <param name="Bytes">判定対象のバイト配列</param>
            /// <param name="Length">ファイルサイズ(バイト配列先頭からのデコード対象バイト数)</param>
            /// <param name="pos">エスケープ(0x1B)出現位置</param>
            /// <returns>エスケープシーケンスとして妥当ならpos加算値、そうでなければゼロ</returns>
            internal static byte isEscape(byte[] Bytes, int Length, int pos)
            {
                if (pos + 2 < Length)
                {   //ESC(0x1B):有効なエスケープシーケンスかチェック
                    switch (Bytes[pos + 1])
                    {   //２バイト目で分岐
                        case 0x24: //ESC$
                            switch (Bytes[pos + 2])
                            {   //３バイト目で分岐
                                case 0x40: //<ESC>$@ : JISエスケープ(78JIS)     - ISO-2022-JP
                                case 0x42: //<ESC>$B : JISエスケープ(83JIS)     - ISO-2022-JP
                                    return 2;
                                case 0x28: //<ESC>$(D: JISエスケープ(90補助漢字)- ISO-2022-JP-1 
                                    //ただしCP5022Xではデコードできないため別途CP20932を用いてデコードすること
                                    if (pos + 3 < Length && Bytes[pos + 3] == 0x44) { return 3; }
                                    break; 
                            }
                            break;
                        case 0x28: //ESC(
                            switch (Bytes[pos + 2])
                            {   //３バイト目で分岐
                                case 0x42: //<ESC>(B : JISエスケープ(ASCII)    - ISO-2022-JP
                                case 0x48: //<ESC>(H : JISエスケープ(swedish)  - (ISO-2022-JP規定外)
                                case 0x49: //<ESC>(I : JISエスケープ(カタカナ) - (ISO-2022-JP規定外 http://tools.ietf.org/html/rfc1468 だがCP5022Xでデコード可能)
                                case 0x4A: //<ESC>(J : JISエスケープ(ローマ字) - ISO-2022-JP
                                    return 2;
                            }
                            break;
                        // 以下、MS版JIS(CP50220/CP50221/CP50222)でデコードできないエスケープシーケンスは、チェックしない。
                        //    <ESC>&@<ESC>$B  : JISエスケープ(90JIS)   - (ISO-2022-JP規定外。90JISで追加された「凜・熙」の２文字は、CP5022xでは更新シーケンス「<ESC>&@」を付けなくてもデコード可能)
                        //    <ESC>$(O : JISエスケープ(2000JIS#1)      - ISO-2022-JP-3
                        //    <ESC>$(P : JISエスケープ(2000JIS#2)      - ISO-2022-JP-3
                        //    <ESC>$(Q : JISエスケープ(2004JIS#1)      - ISO-2022-JP-4
                        // ほか、多言語対応のエスケープシーケンスもチェック外とする。
                        // エスケープシーケンスの一覧／うちMS版JISで有効なものは、
                        // http://ja.wikipedia.org/wiki/ISO/IEC_2022#.E5.BF.9C.E7.94.A8.E4.BE.8B
                        // http://referencesource.microsoft.com/#mscorlib/system/text/iso2022encoding.cs
                        // http://www.wdic.org/w/WDIC/Microsoft%20Windows%20Codepage%20%3A%2050221 なども参照
                    }
                }
                return 0;
            }
            /// <summary>JISと判定された場合の文字コードを確定する</summary>
            /// <param name="Bytes">判定対象のバイト配列</param>
            /// <param name="Length">ファイルサイズ(バイト配列先頭からのデコード対象バイト数)</param>
            /// <param name="Text">out 判別した文字コードにより取り出したテキスト文字列（非テキストならnull）</param>
            /// <returns>カナSO/SI有無やデコード不能箇所有無に応じたJIS文字コード種類</returns>
            internal static CharCode GetEncoding(byte[] Bytes, int Length, out string Text)
            {
                Text = CharCode.JIS.GetString(Bytes, Length);
                if (Text == null)
                {   //デコード不能箇所有、JIS破損ファイルとみなす
                    Text = CharCode.JISNG.GetString(Bytes, Length);
                    return CharCode.JISNG;
                }
                //カナのSO/SI切り替えがあればCP50222、なければCP50221とみなす。
                //(なお、CP50221/CP50222どちらを使ってもデコード結果は同一となる)
                return hasSOSI(Bytes, Length) ? CharCode.JIS50222 : CharCode.JIS;
            }
        }

        /// <summary>
        /// SJIS評価クラス
        /// </summary>
        private class SJIS : ReadJEnc
        {
            internal SJIS() : base(CharCode.SJIS, CharCode.EUC) { }

            protected override int GetEncoding(byte[] Bytes, int pos, int Length)
            {
                int score = 0; //初期値ゼロからReadJEnc評価を始める
                byte b1 = Bytes[pos];
                byte b2;
                while (pos < Length)
                {   //前の文字との連続性チェック用定数定義
                    const int PREV_KANA = 1; //直前文字は半角カナ
                    const int PREV_ZENKAKU = 2; //直前文字は全角
                    int prevChar = 0; //前の文字はKANAでもZENKAKUでもない
                    while (b1 > DEL)
                    {
                        if (b1 >= 0xA1 && b1 <= 0xDF)
                        {   //１バイト半角カナ：OK（連続はEUCやCP1252よりも高配点とする）
                            if (prevChar == PREV_KANA) { score += 3; } else { score += 1; prevChar = PREV_KANA; }
                        }
                        // 非CP932チェック用定数(0x00000061,0xE0009800)：CP932ではデコード不能な未定義文字のビットを１
                        //        FEDC BA98 7654 3210         FEDC BA98 7654 3210
                        //        ---- ---- ---- ----         ---- ---- ---- ----
                        // (0x9#) 0000 0000 0000 0000  (0x8#) 0000 0000 0110 0001 - 80(A0判定でも流用):定義外、85,86:未使用(shift_jis2004などでは使用ありだがCP932ではデコード不能)
                        // (0xF#) 1110 0000 0000 0000  (0xE#) 1001 1000 0000 0000 - FD,FE,FF:定義外、EB,EC,EF:未使用 (F0-F9:外字は許容。HNXgrepなど外字不許容とする場合はビットを立てること)
                        else if (((b1 < 0xE0 ? 0x00000061 : 0xE0009800) & 1u << (b1 % 32)) != 0
                            || (++pos) >= Length
                            || (b2 = Bytes[pos]) < 0x40 || b2 > 0xFC)
                        {   //１バイト目がおかしい(SJIS定義外/未使用) or ２バイト目把握不能 or ２バイト目SJIS定義外
                            return int.MinValue; //可能性消滅
                        }
                        else
                        {   //全角文字数を加算(EUCよりは可能性を低めに見積もっておく)
                            if (prevChar == PREV_ZENKAKU) { score += 4; }
                            else
                            {   //（ただし唐突に0x98以降の第二水準文字が出てきた場合は、UTF-8/EUC/CP1252の可能性が高いのでプラス値なしとする）
                                score += (b1 > 0x98 ? 0 : 2);
                                prevChar = PREV_ZENKAKU;
                            }
                        }
                        //各国語全コード共通：さらに次の文字へ
                        if ((++pos) >= Length) { break; }
                        b1 = Bytes[pos];
                    }
                    //各国語全コード共通：半角文字の範囲を読み飛ばし
                    while (b1 <= DEL && (++pos) < Length) { b1 = Bytes[pos]; }
                }
                return score;
            }
        }
        #endregion

#if (!JPONLY) //漢字圏テキスト文字コード各種（日本語判別以外使用しないなら定義省略可）
        #region 各国語文字コード評価クラス--------------------------------------
        /// <summary>
        /// BIG5TW評価クラス
        /// </summary>
        private class BIG5TW : ReadJEnc
        {
            internal BIG5TW()
                : base(CharCode.BIG5TW, CharCode.EUCTW)
            {   //0x8Eの４バイト文字を全角文字とみなす
                this.EUCTW = true;
            }

            protected override int GetEncoding(byte[] Bytes, int pos, int Length)
            {   //※配点未調整（EUC/UTF8はおそらく正しく識別可能、CP1252は一部誤判別する可能性あり）
                int score = 0; //初期値ゼロからReadJEnc評価を始める
                byte b1 = Bytes[pos];
                byte b2;
                while (pos < Length)
                {   //前の文字との連続性チェック用定数定義
                    const int PREV_ZENKAKU = 2; //直前文字は全角
                    int prevChar = 0; //前の文字はKANAでもZENKAKUでもない
                    while (b1 > DEL)
                    {
                        if (b1 < 0x81 || b1 > 0xF9 || b1 == 0xC7 || b1 == 0xC8
                            || (++pos) >= Length
                            || (b2 = Bytes[pos]) < 0x40 || (b2 < 0xA1 && b2 > 0x7E) || b2 > 0xFE)
                        {   //１バイト目がBig5定義外 or ２バイト目把握不能 or ２バイト目がBig5定義外
                            return int.MinValue; //可能性消滅
                        }
                        else
                        {   //全角文字数を加算(EUCよりは可能性を低めに見積もっておく)
                            if (prevChar == PREV_ZENKAKU) { score += 4; }
                            else
                            {   //ただし唐突に外字・次常用字(第二水準)が出てきた場合は配点を低めにする
                                score += (b1 < 0xA1 || b1 > 0xC8 ? 1 : 2);
                                prevChar = PREV_ZENKAKU;
                            }
                        }
                        //各国語全コード共通：さらに次の文字へ
                        if ((++pos) >= Length) { break; }
                        b1 = Bytes[pos];
                    }
                    //各国語全コード共通：半角文字の範囲を読み飛ばし
                    while (b1 <= DEL && (++pos) < Length) { b1 = Bytes[pos]; }
                }
                return score;
            }
        }

        /// <summary>
        /// GB18030評価クラス
        /// </summary>
        private class GB18030 : ReadJEnc
        {
            internal GB18030() : base(CharCode.GB18030, CharCode.EUCCN) { }

            protected override int GetEncoding(byte[] Bytes, int pos, int Length)
            {   //※配点未調整（EUC/UTF8はおそらく正しく識別可能、CP1252は誤判別の可能性大）
                int score = 0; //初期値ゼロからReadJEnc評価を始める
                byte b1 = Bytes[pos];
                byte b2;
                while (pos < Length)
                {   //前の文字との連続性チェック用定数定義
                    const int PREV_ZENKAKU = 2; //直前文字は全角
                    int prevChar = 0; //前の文字はKANAでもZENKAKUでもない
                    while (b1 > DEL)
                    {
                        if (b1 < 0x81 || b1 > 0xFE || (++pos) >= Length)
                        {   //１バイト目がおかしい(GB定義外) or ２バイト目把握不能
                            return int.MinValue; //可能性消滅
                        }
                        else if ((b2 = Bytes[pos]) >= 0x40 && b2 <= 0xFE)
                        {   //２バイト文字：全角文字数を加算(EUCよりは可能性を低めに見積もっておく)
                            if (prevChar == PREV_ZENKAKU) { score += 4; } else { score += 2; prevChar = PREV_ZENKAKU; }
                        }
                        else if (b2 >= 0x30 && b2 <= 0x39
                            && (++pos) < Length
                            && (b2 = Bytes[pos]) >= 0x81 && b2 <= 0xFE
                            && (++pos) < Length
                            && (b2 = Bytes[pos]) >= 0x30 && b2 <= 0x39)
                        {   //４バイト全角文字：全角文字数を加算（かなりGB18030の決め手になる特徴なので、ポイントを高めに）
                            if (prevChar == PREV_ZENKAKU) { score += 16; } else { score += 8; prevChar = PREV_ZENKAKU; }
                        }
                        else
                        {   //GB18030定義外
                            return int.MinValue; //可能性消滅
                        }
                        //各国語全コード共通：さらに次の文字へ
                        if ((++pos) >= Length) { break; }
                        b1 = Bytes[pos];
                    }
                    //各国語全コード共通：半角文字の範囲を読み飛ばし
                    while (b1 <= DEL && (++pos) < Length) { b1 = Bytes[pos]; }
                }
                return score;
            }
        }

        /// <summary>
        /// UHCKR評価クラス
        /// </summary>
        private class UHCKR : ReadJEnc
        {
            internal UHCKR() : base(CharCode.UHCKR, CharCode.EUCKR) { }

            protected override int GetEncoding(byte[] Bytes, int pos, int Length)
            {   //※配点未調整（EUC/UTF8はおそらく正しく識別可能、CP1252は誤判別の可能性大）
                int score = 0; //初期値ゼロからReadJEnc評価を始める
                byte b1 = Bytes[pos];
                byte b2;
                while (pos < Length)
                {   //前の文字との連続性チェック用定数定義
                    const int PREV_ZENKAKU = 2; //直前文字は全角
                    int prevChar = 0; //前の文字はKANAでもZENKAKUでもない
                    while (b1 > DEL)
                    {
                        if (b1 < 0x81 || b1 > 0xFE
                            || (++pos) >= Length
                            || (b2 = Bytes[pos]) < 0x41 || (b2 < 0x61 && b2 > 0x5A) || (b2 < 0x81 && b2 > 0x7A) || b2 > 0xFE)
                        {   //１バイト目がおかしい(UHC定義外) or ２バイト目把握不能 or ２バイト目がUHC定義外
                            return int.MinValue; //可能性消滅
                        }
                        else
                        {   //全角文字数を加算(EUCよりは可能性を低めに見積もっておく)
                            if (prevChar == PREV_ZENKAKU) { score += 4; } else { score += 2; prevChar = PREV_ZENKAKU; }
                        }
                        //各国語全コード共通：さらに次の文字へ
                        if ((++pos) >= Length) { break; }
                        b1 = Bytes[pos];
                    }
                    //各国語全コード共通：半角文字の範囲を読み飛ばし
                    while (b1 <= DEL && (++pos) < Length) { b1 = Bytes[pos]; }
                }
                return score;
            }
        }
        #endregion
#endif
    }
}
