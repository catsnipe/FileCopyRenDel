using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace Uindies.Library
{
	/// <summary>
	/// [作成者 fj]
	/// エラーログを出力します。
	/// </summary>
	public class ErrLog
	{
		/// <summary>
		/// 初期化されたかどうかを示すフラグ。
		/// </summary>
		static bool		init = false;
		
		/// <summary>
		/// エラーログのファイルサイズを取得、または設定します。
		/// Initialize 時、このサイズを超えるファイルは最大の半分のサイズに自動的に調整されます。
		/// </summary>
		public static int	 ErrorLogFileSize
		{
			get
			{
				return errorLogFileSize;
			}
			set
			{
				// サイズをリミット。
				if (value < 128)
				{
					value = 128;
				}
				if (value > 1024 * 1024)
				{
					value = 1024 * 1024;
				}
				errorLogFileSize = value;
			}
		}
		static int errorLogFileSize = 128 * 1024;
		
		/// <summary>
		/// エラーログファイルパスを取得、または設定します。
		/// </summary>
		public static string ErrorLogFilePath
		{
			get
			{
				return errorLogFilePath;
			}
			set
			{
				if (value == null)
				{
					errorLogFilePath = "errlog.txt";
				}
				else
				{
					errorLogFilePath = value;
				}
			}
		}
		static string errorLogFilePath = "errlog.txt";
		
		/// <summary>
		/// クラスを初期化します。
		/// </summary>
		public static void Initialize()
		{
			FileInfo	fi = new FileInfo(errorLogFilePath);
			
			init = true;
			
			// ErrLogシステム初期化情報をログに残す。
			string s = "▼ [Error Log System] is starting... time = " + DateTime.Now.ToString() + "\r\n";
			
			Debug.Write(s);
			LogWrite(s, true);
			
			// ファイルが最大容量を超えた場合、半分に減らす。
			if (fi.Exists == true && fi.Length > errorLogFileSize)
			{
				s = null;
				
				logRead(ref s);
				if (s != null)
				{
					s = s.Remove(0, s.Length / 2);
					LogWrite(s, false);
					
					s = "▼ Log file is resized half.\r\n";
					Debug.Write(s);
					LogWrite(s, true);
				}
				else
				{
					Debug.WriteLine("▼ ErrLog ファイルを正しくオープンできませんでした。初期化に失敗しました。");
					
					init = false;
				}
			}
		}
		
		/// <summary>
		/// 文字列をエラーログに吐き出し、改行します。
		/// </summary>
		/// <param name="msg">メッセージ</param>
		public static void WriteLine(object msg)
		{
			WriteLine(msg.ToString());
		}
		
		/// <summary>
		/// 文字列をエラーログに吐き出し、改行します。
		/// </summary>
		/// <param name="msg">メッセージ</param>
		public static void WriteLine(string msg)
		{
			string	str = "";
			
			getCallerMethod(ref str); 
			str += msg + "\r\n\r\n";
			
			Debug.Write(str);
			if (Debugger.IsAttached == true)
			{
				System.Media.SystemSounds.Hand.Play();
			}
			
			LogWrite(str, true);
		}
		
		/// <summary>
		/// 文字列をエラーログに吐き出します。
		/// </summary>
		/// <param name="msg">メッセージ</param>
		/// <param name="prms">{0}などの置き換えパラメータ</param>
		public static void WriteLine(string msg, params object[] prms)
		{
			string	str = "";
			
			getCallerMethod(ref str); 
			
			if (prms == null)
			{
				str += msg + "\r\n\r\n";
			}
			else
			{
				str += String.Format(msg, prms) + "\r\n\r\n";
			}
			
			Debug.Write(str);
			if (Debugger.IsAttached == true)
			{
				System.Media.SystemSounds.Hand.Play();
			}
			
			LogWrite(str, true);
		}
		
		/// <summary>
		/// 発生した例外をエラーログに吐き出します。
		/// </summary>
		/// <param name="ex">例外の内容</param>
		public static void WriteException(Exception ex)
		{
			string	str = "";
			bool	enabled = AppLog.ErrorLogOutput;
			
			AppLog.ErrorLogOutput = true;
			
			getCallerMethod(ref str); 
			str += "■■■ Exception";
			AppLog.WriteLine(str);
			AppLog.WriteLine("   Source     = {0}", ex.Source);
			AppLog.WriteLine("   Type       = {0}", ex.GetType().ToString());
			AppLog.WriteLine("   Message    = {0}", ex.Message);
			AppLog.WriteLine("   StackTrace = \r\n{0}", ex.StackTrace);
			AppLog.WriteLine("■■■ Exception");
			AppLog.WriteLine("");
			if (Debugger.IsAttached == true)
			{
				System.Media.SystemSounds.Hand.Play();
			}
			
			// 元に戻す
			AppLog.ErrorLogOutput = enabled;
		}
		
		/// <summary>
		/// エラーログファイルを書き出します。
		/// </summary>
		/// <param name="buff">書き出すバッファ</param>
		/// <param name="append">ファイルの末尾に追加する(true)</param>
		public static void LogWrite(string buff, bool append)
		{
			StreamWriter	sw = null;
			
			try
			{
				if (init == false)
				{
					return;
				}
				
				// ファイルを上書きし、Shift JISで書き込む
				sw = new StreamWriter(
					errorLogFilePath,
					append,
					System.Text.Encoding.GetEncoding("shift-jis"));
				
				sw.Write(buff);
			}
			catch
			{
				Debug.WriteLine("▼ ErrLog を書き出せませんでした。");
			}
			finally
			{
				// 閉じる
				if (sw != null)
				{
					sw.Close();
				}
			}
		}
		
		/// <summary>
		/// エラーログファイルを読込みます。
		/// </summary>
		/// <param name="s">読み込んだ文字列</param>
		static void logRead(ref string s)
		{
			StreamReader	sr = null;
			
			if (File.Exists(errorLogFilePath) == false)
			{
				s = null;
				return;
			}
			
			try
			{
				sr = new StreamReader(
					errorLogFilePath,
					System.Text.Encoding.GetEncoding("shift-jis"));
					
				// 内容をすべて読み込む
				s = sr.ReadToEnd();
			}
			catch
			{
				s = null;
			}
			finally
			{
				// 閉じる
				if (sr != null)
				{
					sr.Close();
				}
			}
		}
		
		/// <summary>
		/// エラーログを呼び出したモジュール、クラス名、メソッド名を文字列化して返します。
		/// </summary>
		/// <param name="s">戻り値</param>
		static void getCallerMethod(ref string s)
		{
			StackFrame	callerFrame = null;
			MethodBase	mBase		= null;
			
			try
			{
				for (int i = 2; ; i++)
				{
					callerFrame	 = new StackFrame(i, true);		// n 個前の呼び元。
																// Release ビルドではメソッドがインライン化され、期待通りの表示にならない可能性があるので安易に使わないこと。
					mBase = callerFrame.GetMethod();
					
					if (mBase.ReflectedType.Name != "ErrLog")
					{
						break;
					}
				}
			}
			catch
			{
				callerFrame = null;
			}
			
			if (callerFrame == null)
			{
				s = String.Format("▼---------- [{0}][Unknown : ???]\r\n", DateTime.Now.ToString());
			}
			else
			{
				// ファイル名、行数は Debug ビルドしか取ることができない。
				s = String.Format("▼---------- [{0}][{1} : {2}] {3}\r\n", DateTime.Now.ToString(), Path.GetFileName(callerFrame.GetFileName()), callerFrame.GetFileLineNumber(), mBase.Name);
			}
		}
	}
}
