using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Hnx8.ReadJEnc;

namespace Uindies.Library
{
	/// <summary>
	/// [作成者 fj]
	/// テキストファイルの読み書きを扱います。
	/// </summary>
	public class FileText
	{
		/// <summary>
		/// utf-8 のエンコードワード
		/// </summary>
		public const string Utf8 = "utf-8";
		/// <summary>
		/// shift_jis のエンコードワード
		/// </summary>
		public const string ShiftJis = "shift_jis";
		
		/// <summary>
		/// エンコード文字列
		/// </summary>
		public static string	EncodingWord = "utf-8";
		/// <summary>
		/// 直前のエラー内容
		/// </summary>
		public static Exception Exception = null;
		
		/// <summary>
		/// テキストファイルを読み込みます。ほとんどのエンコードを自動的に読みわけます。
		/// </summary>
		/// <param name="filename">読み込むファイル</param>
		/// <returns>読み込んだ文字列</returns>
		public static string Read(string filename)
		{
			string			s  = null;
			
			if (FileIO.Exists(filename) == false)
			{
				return null;
			}
			
			try
			{
				System.IO.FileInfo file = new FileInfo(filename);

				using (FileReader reader = new FileReader(file))
				{
					//デフォルト文字コードで判別
					reader.ReadJEnc = ReadJEnc.JP;
					//判別結果の文字コードは、Readメソッドの戻り値で把握できます
					CharCode c = reader.Read(file);
					//戻り値の型からファイルの大まかな種類が判定できます、
					string type =
						(c is CharCode.Text ? "Text:"
						: c is FileType.Bin ? "Binary:"
						: c is FileType.Image ? "Image:"
						: "");
					//戻り値のNameプロパティから文字コード名を取得できます
					string name = c.Name;
					
					if (name.ToLower().IndexOf("utf-8") >= 0)
					{
						EncodingWord = FileText.Utf8;
					}
					else
					if (name.ToLower().IndexOf("shiftjis") >= 0)
					{
						EncodingWord = FileText.ShiftJis;
					}
					else
					{
						// Unknown
						EncodingWord = name;
					}
					
					//戻り値のGetEncoding()メソッドで、エンコーディングを取得できます
					System.Text.Encoding enc = c.GetEncoding();
					//実際に読み出したテキストは、Textプロパティから取得できます
					//（非テキストファイルの場合は、nullが設定されます）
					s = reader.Text;
				}
			}
			catch(Exception ex)
			{
				ErrLog.WriteException(ex);
				
				Exception = ex;
				s	 = null;
			}
			
			return s;
		}
		
		/// <summary>
		/// テキストファイルを書き出します。
		/// </summary>
		/// <param name="filename">書き出すファイル</param>
		/// <param name="buff">書き出す文字列バッファ</param>
		/// <param name="encword">エンコードワード。System.Text.Encoding.GetEncodings()が返す値のどれか</param>
		/// <returns>true..成功, false..失敗</returns>
		public static bool Write(string filename, string buff, string encword)
		{
			List<EncodingInfo>	encs = new List<EncodingInfo>(Encoding.GetEncodings());
			EncodingInfo		enc	 = encs.Find( (en) => encword == en.Name );
			string				encname = FileText.Utf8;
			
			if (enc != null)
			{
				encname = enc.Name;
			}
			
			StreamWriter	sw = null;
			bool			ret = true;
			
			try
			{
				// ファイルを上書きし、Shift JISで書き込む
				sw = new StreamWriter(
					filename,
					false,
					System.Text.Encoding.GetEncoding(encname));
				
				sw.Write(buff);
			}
			catch(Exception ex)
			{
				ErrLog.WriteException(ex);
				
				Exception = ex;
				ret	 = false;
			}
			finally
			{
				// 閉じる
				if (sw != null)
				{
					sw.Close();
				}
			}
			
			return ret;
		}
		
		/// <summary>
		/// テキストファイルを書き出します。
		/// </summary>
		/// <param name="filename">書き出すファイル</param>
		/// <param name="buff">書き出す文字列バッファ</param>
		/// <returns>true..成功, false..失敗</returns>
		public static bool Write(string filename, string buff)
		{
			return Write(filename, buff, EncodingWord);
		}
	}
}
