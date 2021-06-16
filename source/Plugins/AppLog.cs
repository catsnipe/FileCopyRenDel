using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace Uindies.Library
{
	/// <summary>
	/// [作成者 fj]
	/// アプリケーションのログ出力クラスです。主に開発時に用いられます。
	/// </summary>
	public class AppLog
	{
		/// <summary>
		/// エラーログファイルにも書き出すかどうか取得、または設定します。
		/// </summary>
		public static bool ErrorLogOutput
	{
			get
			{
				return errorLogOutput;
			}
			set
			{
				errorLogOutput = value;
			}
		}
		static bool errorLogOutput = false;
		
		/// <summary>
		/// ログのファイルパスを設定します。
		/// ErrLog と共用です。
		/// </summary>
		public static string LogFilePath
		{
			get
			{
				return ErrLog.ErrorLogFilePath;
			}
			set
			{
				ErrLog.ErrorLogFilePath = value;
			}
		}
		
		/// <summary>
		/// エラーログのファイルサイズを取得、または設定します。
		/// ErrLog と共用です。
		/// Initialize 時、このサイズを超えるファイルは最大の半分のサイズに自動的に調整されます。
		/// </summary>
		public static int LogFileSize
		{
			get
			{
				return ErrLog.ErrorLogFileSize;
			}
			set
			{
				ErrLog.ErrorLogFileSize= value;
			}
		}
		
		/// <summary>
		/// 文字列をエラーログに吐き出します。
		/// </summary>
		/// <param name="msg">メッセージ</param>
		public static void Write(object msg)
		{
			Write(msg.ToString());
		}
		
		/// <summary>
		/// 文字列をエラーログに吐き出します。
		/// </summary>
		/// <param name="msg">メッセージ</param>
		public static void Write(string msg)
		{
			Write(msg, null);
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
			Write(msg + "\r\n", null);
		}
		
		/// <summary>
		/// 文字列をエラーログに吐き出し、改行します。
		/// </summary>
		/// <param name="msg">メッセージ</param>
		/// <param name="prms">{0}などの置き換えパラメータ</param>
		public static void WriteLine(string msg, params object[] prms)
		{
			Write(msg + "\r\n", prms);
		}
		
		/// <summary>
		/// 文字列をエラーログに吐き出します。
		/// </summary>
		/// <param name="msg">メッセージ</param>
		/// <param name="prms">{0}などの置き換えパラメータ</param>
		public static void Write(string msg, params object[] prms)
		{
			string	str;
			
			if (prms == null)
			{
				str = msg;
			}
			else
			{
				str = String.Format(msg, prms);
			}
			
			Debug.Write(str);
			
			if (errorLogOutput == true)
			{
				ErrLog.LogWrite(str, true);
			}
		}
	}
}
