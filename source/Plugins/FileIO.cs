using System;
using System.IO;

namespace Uindies.Library
{
	/// <summary>
	/// [作成者 fj]
	/// File に関する命令セットです。
	/// </summary>
	public class FileIO
	{
		/// <summary>
		/// ファイルが存在するか調べます
		/// </summary>
		public static bool Exists(string filename)
		{
			return File.Exists(filename);
		}
		
		/// <summary>
		/// ファイルを削除します
		/// </summary>
		public static bool Delete(string filename)
		{
			try
			{
				if (Exists(filename) == true)
				{
					File.Delete(filename);
				}
			}
			catch(Exception ex)
			{
				ErrLog.WriteException(ex);
				return false;
			}
			
			return true;
		}
		
		/// <summary>
		/// ファイルを移動します
		/// 最終更新日付はそのままの値を保持します
		/// </summary>
		/// <param name="src">移動元</param>
		/// <param name="dst">移動先</param>
		public static bool Move(string src, string dst)
		{
			// 存在しないので移動できない
			if (Exists(src) == false)
			{
//				ErrLog.WriteLine("移動元が存在しません。");
				return false;
			}
			
			// 存在するので移動できない
			if (Exists(dst) == true)
			{
//				ErrLog.WriteLine("移動先が既に存在しています。");
				return false;
			}
			
			try
			{
				DateTime	dt;
				
				// 最終更新日付をそのままで移動する
				dt = GetLastWriteTime(src);
				File.Move(src, dst);
				SetLastWriteTime(dst, dt);
			}
			catch(Exception ex)
			{
				ErrLog.WriteException(ex);
				return false;
			}
			
			return true;
		}
		
		/// <summary>
		/// ファイルをコピーします
		/// 最終更新日付はそのままの値を保持します
		/// </summary>
		/// <param name="src">コピー元</param>
		/// <param name="dst">コピー先</param>
		public static bool Copy(string src, string dst)
		{
			// 存在しないのでコピーできない
			if (Exists(src) == false)
			{
				ErrLog.WriteLine("コピー元が存在しません。");
				return false;
			}
			
			try
			{
				DateTime	dt;
				
				// 最終更新日付をそのままでコピーする
				dt = GetLastWriteTime(src);
				File.Copy(src, dst, true);
				
				// （勝手に）読み取り属性を設定している場合は解除
				FileIO.ResetFileAttributes(dst, FileAttributes.ReadOnly);
				
				SetLastWriteTime(dst, dt);
			}
			catch(Exception ex)
			{
				ErrLog.WriteException(ex);
				return false;
			}
			
			return true;
		}
		
		/// <summary>
		/// 作成日時を取得します
		/// </summary>
		public static DateTime GetCreationTime(string filename)
		{
			DateTime	dt = DateTime.MinValue;
			
			try
			{
				dt = File.GetCreationTime(filename);
			}
			catch(Exception ex)
			{
				ErrLog.WriteException(ex);
			}
			
			return dt;
		}
		
		/// <summary>
		/// 作成日時を設定します
		/// </summary>
		public static bool SetCreationTime(string filename, DateTime dt)
		{
			try
			{
				File.SetCreationTime(filename, dt);
			}
			catch(Exception ex)
			{
				ErrLog.WriteException(ex);
				return false;
			}
			
			return true;
		}
		
		/// <summary>
		/// 更新日時を取得します
		/// </summary>
		public static DateTime GetLastWriteTime(string filename)
		{
			DateTime	dt = DateTime.MinValue;
			
			try
			{
				dt = File.GetLastWriteTime(filename);
			}
			catch(Exception ex)
			{
				ErrLog.WriteException(ex);
			}
			
			return dt;
		}
		
		/// <summary>
		/// 更新日時を設定します
		/// </summary>
		public static bool SetLastWriteTime(string filename, DateTime dt)
		{
			try
			{
				File.SetLastWriteTime(filename, dt);
			}
			catch(Exception ex)
			{
				ErrLog.WriteException(ex);
				return false;
			}
			
			return true;
		}
		
		/// <summary>
		/// アクセス日時を取得します
		/// </summary>
		public static DateTime GetLastAccessTime(string filename)
		{
			DateTime	dt = DateTime.MinValue;
			
			try
			{
				dt = File.GetLastAccessTime(filename);
			}
			catch(Exception ex)
			{
				ErrLog.WriteException(ex);
			}
			
			return dt;
		}
		
		/// <summary>
		/// アクセス日時を設定します
		/// </summary>
		public static bool SetLastAccessTime(string filename, DateTime dt)
		{
			try
			{
				File.SetLastAccessTime(filename, dt);
			}
			catch(Exception ex)
			{
				ErrLog.WriteException(ex);
				return false;
			}
			
			return true;
		}
		
		/// <summary>
		/// ファイル属性を取得します
		/// </summary>
		public static FileAttributes GetFileAttributes(string filename)
		{
			FileAttributes fattr = 0;
			
			try
			{
				fattr= File.GetAttributes(filename);
			}
			catch(Exception ex)
			{
				ErrLog.WriteException(ex);
			}
			
			return fattr;
		}
		
		/// <summary>
		/// ファイル属性を設定します
		/// </summary>
		public static bool SetFileAttributes(string filename, FileAttributes attr)
		{
			FileAttributes fattr;
			
			fattr = GetFileAttributes(filename);
			if (fattr != 0)
			{
				try
				{
					File.SetAttributes(filename, fattr | attr);	
				}
				catch(Exception ex)
				{
					ErrLog.WriteException(ex);
					return false;
				}
			}
			
			return true;
		}
		
		/// <summary>
		/// ファイル属性を解除します
		/// </summary>
		public static bool ResetFileAttributes(string filename, FileAttributes attr)
		{
			FileAttributes fattr;
			
			fattr = GetFileAttributes(filename);
			if (fattr != 0)
			{
				try
				{
					File.SetAttributes(filename, fattr & ~attr);	
				}
				catch(Exception ex)
				{
					ErrLog.WriteException(ex);
					return false;
				}
			}
			
			return true;
		}
	}
}
