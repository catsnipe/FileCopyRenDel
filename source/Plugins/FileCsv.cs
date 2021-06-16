using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    #region ICsvEnclosureToken
    /// <summary>
    /// CSV フォーマットの囲い込み記号のインターフェイスです。
    /// </summary>
    internal interface ICsvEnclosureToken
    {
        #region Property
        /// <summary>
        /// 囲い込みの開始を表す文字列を取得します。
        /// </summary>
        /// <value>囲い込みの開始を表す文字列</value>
        string BeginToken {get;}

        /// <summary>
        /// 囲い込みの終了を表す文字列を取得します。
        /// </summary>
        /// <value>囲い込みの終了を表す文字列</value>
        string EndToken {get;}
        #endregion
    }
    #endregion

    #region CsvEnclosureToken
    /// <summary>
    /// CSV フォーマットの囲い込み記号を表します。
    /// </summary>
    public sealed class CsvEnclosureToken : ICsvEnclosureToken
    {
        #region Private Field
        /// <summary>
        /// 囲い込みの開始を表す文字列を保持します。
        /// </summary>
        private readonly string _beginToken;

        /// <summary>
        /// 囲い込みの終了を表す文字列を保持します。
        /// </summary>
        private readonly string _endToken;

        /// <summary>
        /// エスケープを表す文字列を保持します。
        /// </summary>
        private readonly string _escape;
        #endregion

        #region Public Property
        /// <summary>
        /// エスケープを表す文字列を取得します。
        /// </summary>
        /// <value>エスケープを表す文字列</value>
        public string Escape { get {return this._escape;} }
        #endregion

        #region ICsvEnclosureToken
        /// <summary>
        /// 囲い込みの開始を表す文字列を取得します。
        /// </summary>
        /// <value>囲い込みの開始を表す文字列</value>
        public string BeginToken { get {return this._beginToken;} }

        /// <summary>
        /// 囲い込みの終了を表す文字列を取得します。
        /// </summary>
        /// <value>囲い込みの終了を表す文字列</value>
        public string EndToken { get {return this._endToken;} }
        #endregion

        #region Constructor
        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="token">囲い込みを表す文字列</param>
        /// <exception cref="System.ArgumentNullException">null が設定された場合に送出します。</exception>
        /// <exception cref="System.ArgumentException">不正な値が設定された場合に送出します。</exception>
        public CsvEnclosureToken(string token) : this(token, token, token) {}

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="token">囲い込みを表す文字列</param>
        /// <param name="escape">エスケープを表す文字列</param>
        /// <exception cref="System.ArgumentNullException">null が設定された場合に送出します。</exception>
        /// <exception cref="System.ArgumentException">不正な値が設定された場合に送出します。</exception>
        public CsvEnclosureToken(string token, string escape) : this(token, token, escape) {}

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="beginToken">囲い込みの開始を表す文字列</param>
        /// <param name="endToken">囲い込みの終了を表す文字列</param>
        /// <param name="escape">エスケープを表す文字列</param>
        /// <exception cref="System.ArgumentNullException">null が設定された場合に送出します。</exception>
        /// <exception cref="System.ArgumentException">不正な値が設定された場合に送出します。</exception>
        public CsvEnclosureToken(string beginToken, string endToken, string escape)
        {
            if (beginToken == null)
                throw new System.ArgumentNullException("null は設定できません。", "beginToken");

            if (beginToken.Length == 0)
                throw new System.ArgumentException("長さが0の文字列は設定できません。", "beginToken");

            if (endToken == null)
                throw new System.ArgumentNullException("null は設定できません。", "endToken");

            if (endToken.Length == 0)
                throw new System.ArgumentException("長さが0の文字列は設定できません。", "endToken");

            if (escape == null)
                throw new System.ArgumentNullException("null は設定できません。", "escape");

            if (escape.Length == 0)
                throw new System.ArgumentException("長さが0の文字列は設定できません。", "escape");

            this._beginToken = beginToken;
            this._endToken = endToken;
            this._escape = escape;
        }
        #endregion
    }
    #endregion

    #region CsvMultiLineCommentToken
    /// <summary>
    /// CSV フォーマットの複数行 コメント記号を表します。
    /// </summary>
    public sealed class CsvMultiLineCommentToken : ICsvEnclosureToken
    {
        #region Private Field
        /// <summary>
        /// 複数行 コメントの開始を表す文字列を保持します。
        /// </summary>
        private readonly string _beginToken;

        /// <summary>
        /// 複数行 コメントの終了を表す文字列を保持します。
        /// </summary>
        private readonly string _endToken;
        #endregion

        #region ICsvEnclosureToken
        /// <summary>
        /// 複数行 コメントの開始を表す文字列を取得します。
        /// </summary>
        /// <value>複数行 コメントの開始を表す文字列</value>
        public string BeginToken { get {return this._beginToken;} }

        /// <summary>
        /// 複数行 コメントの終了を表す文字列を取得します。
        /// </summary>
        /// <value>複数行 コメントの終了を表す文字列</value>
        public string EndToken { get {return this._endToken;} }
        #endregion

        #region Constructor
        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="token">複数行 コメントを表す文字列</param>
        /// <exception cref="System.ArgumentNullException">null が設定された場合に送出します。</exception>
        /// <exception cref="System.ArgumentException">不正な値が設定された場合に送出します。</exception>
        public CsvMultiLineCommentToken(string token) : this(token, token) {}

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="beginToken">複数行 コメントの開始を表す文字列</param>
        /// <param name="endToken">複数行 コメントの終了を表す文字列</param>
        /// <exception cref="System.ArgumentNullException">null が設定された場合に送出します。</exception>
        /// <exception cref="System.ArgumentException">不正な値が設定された場合に送出します。</exception>
        public CsvMultiLineCommentToken(string beginToken, string endToken)
        {
            if (beginToken == null)
                throw new System.ArgumentNullException("null は設定できません。", "beginToken");

            if (beginToken.Length == 0)
                throw new System.ArgumentException("長さが0の文字列は設定できません。", "beginToken");

            if (endToken == null)
                throw new System.ArgumentNullException("null は設定できません。", "endToken");

            if (endToken.Length == 0)
                throw new System.ArgumentException("長さが0の文字列は設定できません。", "endToken");

            this._beginToken = beginToken;
            this._endToken = endToken;
        }
        #endregion
    }
    #endregion

    #region CsvReader
    /// <summary>
    /// CSV フォーマットのストリームを読み取ります。
    /// </summary>
    public unsafe sealed class CsvReader : System.IO.StreamReader
    {
        #region Private Field
        /// <summary>
        /// フィールドの区切り記号を表す複数の文字列を保持します。
        /// </summary>
        private string[] _delimiterTokens;

        /// <summary>
        /// フィールドの囲い込みの開始と終了を表す複数の文字列を保持します。
        /// </summary>
        private CsvEnclosureToken[] _enclosureTokens;

        /// <summary>
        /// 単一行 コメントの開始を表す複数の文字列を保持します。
        /// </summary>
        private string[] _singleLineCommentTokens;

        /// <summary>
        /// 複数行 コメントの開始と終了を表す複数の文字列を保持します。
        /// </summary>
        private CsvMultiLineCommentToken[] _multiLineCommentTokens;
        #endregion

        #region Public Property
        /// <summary>
        /// フィールルドの区切り記号を表す複数の文字列を取得、または設定します。
        /// </summary>
        /// <value>フィールルドの区切り記号を表す複数の文字列</value>
        /// <exception cref="System.ArgumentNullException">null が設定された場合に送出します。</exception>
        /// <exception cref="System.ArgumentException">不正な値が設定された場合に送出します。</exception>
        public string[] DelimiterTokens
        {
            get {return this._delimiterTokens;}
            set
            {
                if (value == null)
                    throw new System.ArgumentNullException("null は設定できません。", "value");

                if (value.Length == 0)
                    throw new System.ArgumentException("要素が0の配列は設定できません。", "value");

                this._delimiterTokens = value;
            }
        }

        /// <summary>
        /// フィールドの囲い込みの開始と終了を表す複数の文字列を取得、または設定します。
        /// </summary>
        /// <value>フィールドの囲い込みの開始と終了を表す複数の文字列</value>
        public CsvEnclosureToken[] EnclosureTokens
        {
            get {return this._enclosureTokens;}
            set
            {
                this.EnclosureEnabled = (value != null && value.Length != 0);
                this._enclosureTokens = value;
            }
        }

        /// <summary>
        /// 単一行 コメントの開始を表す複数の文字列を取得、または設定します。
        /// </summary>
        /// <value>単一行 コメントの開始を表す複数の文字列</value>
        public string[] SingleLineCommentTokens
        {
            get {return this._singleLineCommentTokens;}
            set
            {
                this.SingleLineCommentEnabled = (value != null && value.Length != 0);
                this._singleLineCommentTokens = value;
            }
        }

        /// <summary>
        /// 複数行 コメントの開始と終了を表す複数の文字列を取得、または設定します。
        /// </summary>
        /// <value>複数行 コメントの開始と終了を表す複数の文字列</value>
        public CsvMultiLineCommentToken[] MultiLineCommentTokens       
        {
            get {return this._multiLineCommentTokens;}
            set
            {
                this.MultiLineCommentEnabled = (value != null && value.Length != 0);
                this._multiLineCommentTokens = value;
            }
        }

        /// <summary>
        /// 行頭に配置されたコメントのみを有効にするかどうかを示す値を取得、または設定します。
        /// </summary>
        /// <value>行頭に配置されたコメントのみを有効にするかどうかを示す値</value>
        public bool CommentTokenIsLineHeadOnly {get; set;}

        /// <summary>
        /// 空白の行を有効なフィールドとして読み込むかどうかを示す値を取得、または設定します。
        /// </summary>
        /// <value>空白の行を有効なフィールドとして読み込むかどうかを示す値</value>
        public bool ReadEmptyLine {get; set;}

        /// <summary>
        /// エスケープ文字が有効であるかどうかを示す値を取得、または設定します。
        /// </summary>
        /// <value>エスケープ文字が有効であるかどうかを示す値</value>
        public bool Escape {get; set;}

        /// <summary>
        /// 現在の行番号を取得します。
        /// </summary>
        /// <value>現在の行番号</value>
        public int LineNumber {get; private set;}
        #endregion

        #region Private Property
        /// <summary>
        /// フィールドの囲い込み記号が有効であるかどうかを示す値を取得します。
        /// </summary>
        private bool EnclosureEnabled {get; set;}

        /// <summary>
        /// 単一行 コメントが有効であるかどうかを示す値を取得します。
        /// </summary>
        /// <value>単一行 コメントが有効であるかどうかを示す値</value>
        private bool SingleLineCommentEnabled {get; set;}

        /// <summary>
        /// 複数行 コメントが有効であるかどうかを示す値を取得します。
        /// </summary>
        /// <value>複数行 コメントが有効であるかどうかを示す値</value>
        private bool MultiLineCommentEnabled {get; set;}
        #endregion

        #region Constructor
        /// <summary>
        /// 指定したストリーム用の CsvReader クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="stream">読み込まれるストリーム。</param>
        /// <exception cref="System.ArgumentException">stream が読み取りをサポートしていません。</exception>
        /// <exception cref="System.ArgumentNullException">stream が null です。</exception>
        public CsvReader(System.IO.Stream stream) 
                : base(stream)
        {
            this.Initialize();
        }

        /// <summary>
        /// 指定したファイル名用の CsvReader クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="path">読み込まれる完全なファイルパス。</param>
        /// <exception cref="System.ArgumentException">path が空の文字列 ("") です。</exception>
        /// <exception cref="System.ArgumentNullException">path が null です。</exception>
        /// <exception cref="System.IO.FileNotFoundException">stream が読み取りをサポートしていません。</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">stream が null です。</exception>
        /// <exception cref="System.IO.IOException">path に、ファイル名、ディレクトリ名、
        /// またはボリューム ラベルとしては不正または無効な構文が含まれています。</exception>
        public CsvReader(string path)
                : base(path)
        {
            this.Initialize();
        }

        /// <summary>
        /// バイト順マーク検出オプションを設定して、
        /// 指定したストリーム用の CsvReader クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="stream">読み込まれるストリーム。</param>
        /// <param name="detectEncodingFromByteOrderMarks">
        /// ファイルの先頭にあるバイト順序マークを検索するかどうかを示します。</param>
        /// <exception cref="System.ArgumentException">stream が読み取りをサポートしていません。</exception>
        /// <exception cref="System.ArgumentNullException">stream が null です。</exception>
        public CsvReader(System.IO.Stream stream, bool detectEncodingFromByteOrderMarks)
                : base(stream, detectEncodingFromByteOrderMarks)
        {
            this.Initialize();
        }

        /// <summary>
        /// 文字エンコーディングを設定して、
        /// 指定したストリーム用の CsvReader クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="stream">読み込まれるストリーム。</param>
        /// <param name="encoding">使用する文字エンコーディング。</param>
        /// <exception cref="System.ArgumentException">stream が読み取りをサポートしていません。</exception>
        /// <exception cref="System.ArgumentNullException">stream が null です。</exception>
        public CsvReader(System.IO.Stream stream, System.Text.Encoding encoding)
                : base(stream, encoding)
        {
            this.Initialize();
        }

        /// <summary>
        /// 指定したバイト順マーク検出オプションを使用して、
        /// 指定したファイル名用の CsvReader クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="path">読み込まれる完全なファイルパス。</param>
        /// <param name="detectEncodingFromByteOrderMarks">
        /// ファイルの先頭にあるバイト順序マークを検索するかどうかを示します。</param>
        /// <exception cref="System.ArgumentException">path が空の文字列 ("") です。</exception>
        /// <exception cref="System.ArgumentNullException">path が null です。</exception>
        /// <exception cref="System.IO.FileNotFoundException">stream が読み取りをサポートしていません。</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">stream が null です。</exception>
        /// <exception cref="System.IO.IOException">path に、ファイル名、ディレクトリ名、
        /// またはボリューム ラベルとしては不正または無効な構文が含まれています。</exception>
        public CsvReader(string path, bool detectEncodingFromByteOrderMarks)
                : base(path, detectEncodingFromByteOrderMarks)
        {
            this.Initialize();
        }

        /// <summary>
        /// 文字エンコーディングを設定して、
        /// 指定したファイル名用の CsvReader クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="path">読み込まれる完全なファイルパス。</param>
        /// <param name="encoding">使用する文字エンコーディング。</param>
        /// <exception cref="System.ArgumentException">path が空の文字列 ("") です。</exception>
        /// <exception cref="System.ArgumentNullException">path が null です。</exception>
        /// <exception cref="System.IO.FileNotFoundException">stream が読み取りをサポートしていません。</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">stream が null です。</exception>
        /// <exception cref="System.NotSupportedException">path に、ファイル名、ディレクトリ名、
        /// またはボリューム ラベルとしては不正または無効な構文が含まれています。</exception>
        public CsvReader(string path, System.Text.Encoding encoding)
                : base(path, encoding)
        {
            this.Initialize();
        }

        /// <summary>
        /// 文字エンコーディングとバイト順マーク検出オプションを設定して、
        /// 指定したストリーム用の CsvReader クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="stream">読み込まれるストリーム。</param>
        /// <param name="encoding">使用する文字エンコーディング。</param>
        /// <param name="detectEncodingFromByteOrderMarks">
        /// ファイルの先頭にあるバイト順序マークを検索するかどうかを示します。</param>
        /// <exception cref="System.ArgumentException">stream が読み取りをサポートしていません。</exception>
        /// <exception cref="System.ArgumentNullException">stream が null です。</exception>
        public CsvReader(System.IO.Stream stream, System.Text.Encoding encoding, bool detectEncodingFromByteOrderMarks)
                : base(stream, encoding, detectEncodingFromByteOrderMarks)
        {
            this.Initialize();
        }

        /// <summary>
        /// 文字エンコーディングとバイト順マーク検出オプションを設定して、
        /// 指定したファイル名用の CsvReader クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="path">読み込まれる完全なファイルパス。</param>
        /// <param name="encoding">使用する文字エンコーディング。</param>
        /// <param name="detectEncodingFromByteOrderMarks">
        /// ファイルの先頭にあるバイト順序マークを検索するかどうかを示します。</param>
        /// <exception cref="System.ArgumentException">path が空の文字列 ("") です。</exception>
        /// <exception cref="System.ArgumentNullException">path が null です。</exception>
        /// <exception cref="System.IO.FileNotFoundException">stream が読み取りをサポートしていません。</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">stream が null です。</exception>
        /// <exception cref="System.NotSupportedException">path に、ファイル名、ディレクトリ名、
        /// またはボリューム ラベルとしては不正または無効な構文が含まれています。</exception>
        public CsvReader(string path, System.Text.Encoding encoding, bool detectEncodingFromByteOrderMarks)
                : base(path, encoding, detectEncodingFromByteOrderMarks)
        {
            this.Initialize();
        }

        /// <summary>
        /// 文字エンコーディング、バイト順マーク検出オプション、
        /// およびバッファー サイズを設定して、指定したストリーム用の CsvReader
        /// クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="stream">読み込まれるストリーム。</param>
        /// <param name="encoding">使用する文字エンコーディング。</param>
        /// <param name="detectEncodingFromByteOrderMarks">
        /// ファイルの先頭にあるバイト順序マークを検索するかどうかを示します。</param>
        /// <param name="bufferSize">最小バッファー サイズ。</param>
        /// <exception cref="System.ArgumentException">stream が読み取りをサポートしていません。</exception>
        /// <exception cref="System.ArgumentNullException">stream が null です。</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">bufferSize が 0 以下です。</exception>
        public CsvReader(System.IO.Stream stream, System.Text.Encoding encoding, 
            bool detectEncodingFromByteOrderMarks, int bufferSize)
                : base(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize)
        {
            this.Initialize();
        }

        /// <summary>
        /// 文字エンコーディング、バイト順マーク検出オプション、
        /// およびバッファー サイズを設定して、指定したファイル名の CsvReader
        /// クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="path">読み込まれる完全なファイルパス。</param>
        /// <param name="encoding">使用する文字エンコーディング。</param>
        /// <param name="detectEncodingFromByteOrderMarks">
        /// ファイルの先頭にあるバイト順序マークを検索するかどうかを示します。</param>
        /// <param name="bufferSize">最小バッファサイズ。単位は、16 ビット文字数です。</param>
        /// <exception cref="System.ArgumentException">path が空の文字列 ("") です。</exception>
        /// <exception cref="System.ArgumentNullException">path が null です。</exception>
        /// <exception cref="System.IO.FileNotFoundException">stream が読み取りをサポートしていません。</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">stream が null です。</exception>
        /// <exception cref="System.NotSupportedException">path に、ファイル名、ディレクトリ名、
        /// またはボリューム ラベルとしては不正または無効な構文が含まれています。</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">bufferSize が 0 以下です。</exception>
        public CsvReader(string path, System.Text.Encoding encoding, 
            bool detectEncodingFromByteOrderMarks, int bufferSize)
                : base(path, encoding, detectEncodingFromByteOrderMarks, bufferSize)
        {
            this.Initialize();
        }
        #endregion

        #region Public Setter
        /// <summary>
        /// フィールドの区切り記号を設定します。
        /// </summary>
        /// <param name="tokens">記号</param>
        public void SetDelimiterTokens(params string[] tokens)
        {
            this.DelimiterTokens = tokens;
        }

        /// <summary>
        /// フィールドの囲い込みの開始と終了を表す記号を設定します。
        /// </summary>
        /// <param name="token">囲い込みを表す文字列</param>
        public void SetEnclosureTokens(string token)
        {
            this.SetEnclosureTokens(token, token);
        }

        /// <summary>
        /// フィールドの囲い込みの開始と終了を表す記号を設定します。
        /// </summary>
        /// <param name="token">囲い込みを表す文字列</param>
        /// <param name="escape">エスケープを表す文字列</param>
        public void SetEnclosureTokens(string token, string escape) 
        {
            this.SetEnclosureTokens(token, token, escape);
        }

        /// <summary>
        /// フィールドの囲い込みの開始と終了を表す記号を設定します。
        /// </summary>
        /// <param name="beginToken">囲い込みの開始を表す文字列</param>
        /// <param name="endToken">囲い込みの終了を表す文字列</param>
        /// <param name="escape">エスケープを表す文字列</param>
        public void SetEnclosureTokens(string beginToken, string endToken, string escape)
        {
            this.EnclosureTokens = new CsvEnclosureToken[] {new CsvEnclosureToken(beginToken, endToken, escape)};
        }

        /// <summary>
        /// フィールドの囲い込みの開始と終了を表す記号を設定します。
        /// </summary>
        /// <param name="tokens">記号</param>
        public void SetEnclosureTokens(params CsvEnclosureToken[] tokens)
        {
            this.EnclosureTokens = tokens;
        }

        /// <summary>
        /// 単一行 コメントの開始を表す記号を設定します。
        /// </summary>
        /// <param name="tokens">記号</param>
        public void SetSingleLineCommentTokens(params string[] tokens)
        {
            this.SingleLineCommentTokens = tokens;
        }

        /// <summary>
        /// 複数行 コメントの開始と終了を表す記号を設定します。
        /// </summary>
        /// <param name="token">複数行 コメントの開始と終了を表す文字列</param>
        public void SetMultiLineCommentTokens(string token)
        {
            this.SetMultiLineCommentTokens(token, token);
        }

        /// <summary>
        /// 複数行 コメントの開始と終了を表す記号を設定します。
        /// </summary>
        /// <param name="beginToken">複数行 コメントの開始を表す文字列</param>
        /// <param name="endToken">複数行 コメントの終了を表す文字列</param>
        public void SetMultiLineCommentTokens(string beginToken, string endToken)
        {
            this.MultiLineCommentTokens 
                = new CsvMultiLineCommentToken[] {new CsvMultiLineCommentToken(beginToken, endToken)};
        }

        /// <summary>
        /// 複数行 コメントの開始と終了を表す記号を設定します。
        /// </summary>
        /// <param name="tokens">記号</param>
        public void SetMultiLineCommentTokens(params CsvMultiLineCommentToken[] tokens)
        {
            this.MultiLineCommentTokens = tokens;
        }
        #endregion

        #region Public Method
        /// <summary>
        /// 現在のストリームから、ストリームの終端までの全てのフィールドを読み取ります。
        /// </summary>
        /// <returns>各要素がフィールドを表す１次元の文字列の配列の配列</returns>
        /// <exception cref="CsvSyntaxAnalysisException">文を解析できない場合に送出します。</exception>
        public string[][] ReadAllRecords()
        {
            return this.ReadAllRecords(0);
        }

        /// <summary>
        /// 現在のストリームから、ストリームの終端までの全てのフィールドを読み取ります。
        /// </summary>
        /// <param name="column">レコードあたりのフィールドの数</param>
        /// <returns>各要素がフィールドを表す１次元の文字列の配列の配列</returns>
        /// <exception cref="CsvSyntaxAnalysisException">文を解析できない場合に送出します。</exception>
        public string[][] ReadAllRecords(int column)
        {
            var records = new System.Collections.Generic.List<string[]>();
            string[] record;
            while (!this.EndOfStream)
                if ((record = this.ReadFields(column)) != null)
                    records.Add(record);
            return records.ToArray();
        }

        /// <summary>
        /// 現在のストリームから、一連のフィールドを読み取ります。
        /// </summary>
        /// <returns>
        /// 各要素がフィールドを表す１次元の文字列の配列。有効なデータがない場合はnull。
        /// </returns>
        /// <exception cref="CsvSyntaxAnalysisException">文を解析できない場合に送出します。</exception>
        public string[] ReadFields()
        {
            return this.ReadFields(0);
        }

        /// <summary>
        /// 現在のストリームから、一連のフィールドを読み取ります。
        /// </summary>
        /// <param name="column">フィールドの数</param>
        /// <returns>
        /// 各要素がフィールドを表す１次元の文字列の配列。有効なデータがない場合はnull。
        /// </returns>
        /// <exception cref="CsvSyntaxAnalysisException">文を解析できない場合に送出します。</exception>
        public string[] ReadFields(int column)
        {
            return this.ReadFields(column, 0);
        }

        /// <summary>
        /// 現在のストリームから、一連のフィールドを読み取ります。
        /// </summary>
        /// <param name="column">フィールドの数</param>
        /// <param name="fieldMaxLength">フィールドの最大文字数</param>
        /// <returns>
        /// 各要素がフィールドを表す１次元の文字列の配列。有効なデータがない場合はnull。
        /// </returns>
        /// <exception cref="CsvSyntaxAnalysisException">文を解析できない場合に送出します。</exception>
        public unsafe string[] ReadFields(int column, int fieldMaxLength)
        {
            var fields = new System.Collections.Generic.List<string>(column);
            var field = new System.Text.StringBuilder(fieldMaxLength);

            CsvEnclosureToken currentEnclosureToken = null;
            CsvMultiLineCommentToken currentMultiLineCommentToken = null;

            bool lineHead = true;
            bool fieldRead = false;
            bool enclosedFieldRead = false;
            bool singleLineCommented = false;
            bool multiLineCommented = false;
            bool endOfRecord = false;
            while (!endOfRecord && !this.EndOfStream)
            {
                fixed (char* f = this.ReadToEndOfRecord())
                {
                    char* s = f;
                    while (*s != '\0')
                    {
                        if (enclosedFieldRead)
                        {
                            if (this.Escape && CsvReader.Match(s, currentEnclosureToken.Escape))
                            {
                                s += currentEnclosureToken.Escape.Length;
                                if (CsvReader.Match(s, currentEnclosureToken.EndToken))
                                    field.Append(*s++);
                                else if (CsvReader.Match(s, currentEnclosureToken.Escape))
                                {
                                    s += currentEnclosureToken.Escape.Length;
                                    field.Append(currentEnclosureToken.Escape);
                                }
                                else
                                {
                                    if (currentEnclosureToken.Escape == currentEnclosureToken.EndToken)
                                        enclosedFieldRead = false;
                                    else
                                        field.Append(*s++);
                                }
                            }
                            else if (CsvReader.Match(s, currentEnclosureToken.EndToken))
                            {
                                s += currentEnclosureToken.EndToken.Length;
                                enclosedFieldRead = false;
                            }
                            else
                                field.Append(*s++);
                        }
                        else if (singleLineCommented)
                        {
                            if (*s++ == '\r')
                                singleLineCommented = false;

                            if (*s++ == '\n')
                                singleLineCommented = false;
                        }
                        else if (multiLineCommented)
                        {
                            if (CsvReader.Match(s++, currentMultiLineCommentToken.EndToken))
                            {
                                s += (currentMultiLineCommentToken.EndToken.Length - 1);
                                multiLineCommented = false;
                            }
                        }
                        else
                        {
                            string sContains;
                            ICsvEnclosureToken iContains = null;
                            if (!fieldRead && this.EnclosureEnabled && 
                                    CsvReader.MatchEnclosureBeginToken(s, out iContains, this.EnclosureTokens))
                            {
                                s += iContains.BeginToken.Length;
                                currentEnclosureToken = (iContains as CsvEnclosureToken);
                                field.Length = 0;
                                enclosedFieldRead = true;
                            }
                            else if (CsvReader.Match(s, out sContains, this.DelimiterTokens))
                            {
                                s += sContains.Length;
                                fields.Add(field.ToString());
                                field.Length = 0;
                                currentEnclosureToken = null;
                                currentMultiLineCommentToken = null;
                                fieldRead = false;
                            }
                            else if (*s == '\r')
                            {
                                ++s;
                                if (*s == '\n')
                                    ++s;
                                endOfRecord = true;
                                break;
                            }
                            else if (*s == '\n')
                            {
                                ++s;
                                endOfRecord = true;
                                break;
                            }
                            else if (this.SingleLineCommentEnabled && 
                              ((this.CommentTokenIsLineHeadOnly && lineHead) || (!this.CommentTokenIsLineHeadOnly)) &&
                                    CsvReader.Match(s, out sContains, this.SingleLineCommentTokens))
                            {
                                s += sContains.Length;
                                singleLineCommented = true;
                                endOfRecord = true;
                                if (!this.ReadEmptyLine && fields.Count == 0)
                                    return this.ReadFields(column, fieldMaxLength);
                            }
                            else if (this.MultiLineCommentEnabled && 
                              ((this.CommentTokenIsLineHeadOnly && lineHead) || (!this.CommentTokenIsLineHeadOnly)) &&
                                    CsvReader.MatchEnclosureBeginToken(s, out iContains, this.MultiLineCommentTokens))
                            {
                                s += iContains.BeginToken.Length;
                                currentMultiLineCommentToken = (iContains as CsvMultiLineCommentToken);
                                multiLineCommented = true;
                            }
                            else if (!fieldRead && *s != '\0' && char.IsWhiteSpace(*s))
                            {
                                if (currentEnclosureToken == null)
                                    field.Append(*s);
                                ++s;
                            }
                            else if (currentEnclosureToken == null)
                            {
                                field.Append(*s++);
                                fieldRead = true;
                            }
                            else
                                throw new CsvSyntaxAnalysisException(this.LineNumber, (int)(s - f));
                            lineHead = false;
                        }
                    }
                }
            }            
            if (this.EndOfStream && field.Length == 0)
                return (fields.Count == 0) ? null : fields.ToArray();
            else
            {
                fields.Add(field.ToString());
                while (fields.Count < column)
                    fields.Add(string.Empty);
                return fields.ToArray();
            }
        }
        #endregion

        #region Private Method
        /// <summary>
        /// 現在のストリームから、レコードの終端と思しき文字が現れるまでを読み取ります。
        /// </summary>
        /// <returns>文字列</returns>
        private string ReadToEndOfRecord()
        {
            int i;
            char c;
            var buffer = new System.Text.StringBuilder();
            while ((i = this.Read()) != -1)
            {
                buffer.Append( (c = System.Convert.ToChar(i)) );
                if (c == '\r')
                {
                    if (!this.ReadEmptyLine && buffer.Length == 1)
                        return this.ReadToEndOfRecord();

                    if (this.Peek() == '\n')
                    {
                        this.Read();
                        buffer.Append('\n');
                    }
                    ++this.LineNumber;
                    break;
                }
                else if (c == '\n')
                {
                    if (!this.ReadEmptyLine && buffer.Length == 1)
                        return this.ReadToEndOfRecord();
                    ++this.LineNumber;
                    break;
                }
            }
            return buffer.ToString();
        }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        private void Initialize()
        {
            this.SetDelimiterTokens(",");
            this.SetEnclosureTokens("\"");
            this.SingleLineCommentTokens = null;
            this.MultiLineCommentTokens = null;
            this.CommentTokenIsLineHeadOnly = true;
            this.ReadEmptyLine = false;
            this.Escape = true;
            this.LineNumber = 0;
        }
        #endregion

        #region Private Static Method
        /// <summary>
        /// 現在の文字列を指すポインターの先に、
        /// 対象のいずれかの ICsvEnclosureToken の開始文字が含まれているかどうかを検査します。
        /// </summary>
        /// <param name="s">文字列</param>
        /// <param name="contains">一致した ICsvEnclosureToken</param>
        /// <param name="tokens">検索する ICsvEnclosureToken</param>
        /// <returns>ICsvEnclosureToken の開始文字が含まれているかどうかを示す値</returns>
        private unsafe static bool MatchEnclosureBeginToken(
                char* s, out ICsvEnclosureToken contains, params ICsvEnclosureToken[] tokens)
        {
            foreach (var t in tokens)
            {
                if (CsvReader.Match(s, t.BeginToken))
                {
                    contains = t;
                    return true;
                }
            }
            contains = null;
            return false;
        }

        /// <summary>
        /// 現在の文字列を指すポインターの先に、対象の文字列のいずれかが含まれているかどうかを検査します。
        /// </summary>
        /// <param name="s">文字列</param>
        /// <param name="contains">一致した文字列</param>
        /// <param name="tokens">検索する文字列</param>
        /// <returns>文字列が含まれているかどうかを示す値</returns>
        private unsafe static bool Match(char* s, out string contains, params string[] tokens)
        {
            foreach (var t in tokens)
            {
                if (CsvReader.Match(s, t))
                {
                    contains = t;
                    return true;
                }
            }
            contains = null;
            return false;
        }

        /// <summary>
        /// 現在の文字列を指すポインターの先に、対象の文字列が含まれているかどうかを検査します。
        /// </summary>
        /// <param name="s">文字列</param>
        /// <param name="token">検索する文字列</param>
        /// <returns>文字列が含まれているかどうかを示す値</returns>
        private unsafe static bool Match(char* s, string token)
        {
            fixed (char* t = token)
                return CsvReader.Match(s, t);
        }

        /// <summary>
        /// 現在の文字列を指すポインターの先に、対象の文字列が含まれているかどうかを検査します。
        /// </summary>
        /// <param name="s">文字列</param>
        /// <param name="t">検索する文字列</param>
        /// <returns>文字列が含まれているかどうかを示す値</returns>
        private unsafe static bool Match(char* s, char* t)
        {
            while (*t != '\0')
                if (*s++ != *t++)
                    return false;
            return true;
        }
        #endregion
    }
    #endregion

    #region CsvCsvSyntaxAnalysisException
    /// <summary>
    /// CSV フォーマットの構文解析でエラーが発生した事を示す例外です。
    /// </summary>
    public sealed class CsvSyntaxAnalysisException : System.Exception
    {
        #region Private Field
        /// <summary>
        /// 行番号を保持します。
        /// </summary>
        private readonly int _lineNumber;

        /// <summary>
        /// 位置を保持します。
        /// </summary>
        private readonly int _index;
        #endregion

        #region Public Property
        /// <summary>
        /// エラーが発生した行番号を取得します。
        /// </summary>
        /// <value>行番号</value>
        public int LineNumber { get {return this._lineNumber;} }

        /// <summary>
        /// エラーが発生した位置を取得します。
        /// </summary>
        /// <value>位置</value>
        public int Index { get {return this._index;} }
        #endregion

        #region Constructor
        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        public CsvSyntaxAnalysisException() : base("不正な構文が検出されました。") {}

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="line">行番号</param>
        /// <param name="index">位置</param>
        public CsvSyntaxAnalysisException(int line, int index) 
            : this(string.Format("行 {0}  位置 {1} を解析できません。", line, index), line, index) {}

        /// <summary>
        /// 指定したエラー メッセージを使用して、新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="message">例外の原因を説明するエラー メッセージ。</param>
        /// <param name="line">行番号</param>
        /// <param name="index">位置</param>
        public CsvSyntaxAnalysisException(string message, int line, int index) : base(message) 
        {
            this._lineNumber = line;
            this._index = index;
        }

        /// <summary>
        /// 指定したエラー メッセージと、この例外の原因である内部例外への参照を使用して、
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="message">例外の原因を説明するエラー メッセージ。</param>
        /// <param name="line">行番号</param>
        /// <param name="index">位置</param>
        /// <param name="innerException">
        /// 現在の例外の原因である例外。内部例外が指定されていない場合は、null 参照</param>
        public CsvSyntaxAnalysisException(string message, int line, int index, System.Exception innerException) 
                : base(message, innerException) 
        {
            this._lineNumber = line;
            this._index = index;
        }

        /// <summary>
        /// シリアル化したデータを使用して、新しいインスタンスを初期化します。 
        /// </summary>
        /// <param name="info">
        /// スローされている例外に関するシリアル化済みオブジェクト データを保持している SerializationInfo。
        /// </param>
        /// <param name="context">転送元または転送先に関するコンテキスト情報を含んでいる StreamingContext。</param>
        public CsvSyntaxAnalysisException(
                System.Runtime.Serialization.SerializationInfo info, 
                System.Runtime.Serialization.StreamingContext context) 
            : base(info, context) {}
        #endregion
    }
    #endregion
}
