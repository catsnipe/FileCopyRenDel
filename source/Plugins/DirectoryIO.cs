using System;
using System.IO;

namespace Uindies.Library
{
	/// <summary>
	/// [作成者 fj]
	/// Directory に関する命令セットです。
	/// </summary>
	public class DirectoryIO
	{
		/// <summary>
		/// ディレクトリが存在するか調べます
		/// </summary>
		public static bool Exists(string filename)
		{
			return Directory.Exists(filename);
		}
		
		/// <summary>
		/// ディレクトリを作成します
		/// </summary>
		public static bool Make(string filename)
		{
			try
			{
				Directory.CreateDirectory(filename);
			}
			catch(Exception ex)
			{
				ErrLog.WriteException(ex);
				return false;
			}
			
			return true;
		}
		
		/// <summary>
		/// ディレクトリを削除します。サブディレクトリ以下もがっつり削除
		/// 配下に読み取り専用があったらエラー
		/// </summary>
		public static bool Delete(string filename)
		{
			try
			{
				if (Directory.Exists(filename) == true)
				{
					Directory.Delete(filename, true);
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
		/// ディレクトリ移動
		/// 移動先は存在してなく、移動元と同じドライブにあること
		/// 移動先は移動元のサブディレクトリではダメ
		/// 移動元が存在していなければダメ
		/// 属性も受け継がれる
		/// </summary>
		/// <param name="src">移動元</param>
		/// <param name="dst">移動先</param>
		public static bool Move(string src, string dst)
		{
			// 存在するので移動できない
			if (Exists(dst) == true)
			{
				return false;
			}
			
			try
			{
				Directory.Move(src, dst);
			}
			catch(Exception ex)
			{
				ErrLog.WriteException(ex);
				return false;
			}
			
			return true;
		}
		
		/// <summary>
		/// ディレクトリコピー
		/// </summary>
		/// <param name="src">コピーするディレクトリ</param>
		/// <param name="dst">コピー先のディレクトリ</param>
		public static bool Copy(string src, string dst)
		{
			try
			{
				// コピー先のディレクトリがないときは作る
				if (!Directory.Exists(dst))
				{
					Directory.CreateDirectory(dst);
				
					// 属性もコピー
					File.SetAttributes(dst, File.GetAttributes(src));
				}
			
				// コピー先のディレクトリ名の末尾に"\"をつける
				if (dst[dst.Length - 1] != Path.DirectorySeparatorChar)
				{
					dst = dst + Path.DirectorySeparatorChar;
				}
			
				// コピー元のディレクトリにあるファイルをコピー
				string[] files = Directory.GetFiles(src);
				foreach (string file in files)
				{
					File.Copy(file, dst + Path.GetFileName(file), true);
				}
			
				// コピー元のディレクトリにあるディレクトリについて、
				// 再帰的に呼び出す
				string[] dirs = Directory.GetDirectories(src);
				foreach (string dir in dirs)
				{
					Copy(dir, dst + Path.GetFileName(dir));
				}
			}
			catch (Exception ex)
			{
				ErrLog.WriteException(ex);
				return false;
			}
			
			return true;
		}
		
		/// <summary>
		/// 指定したフォルダ以下のファイルを全て取得します。
		/// </summary>
		/// <param name="dir">フォルダ</param>
		/// <param name="opt">サーチオプション</param>
		public static string[] GetFiles(string dir, SearchOption opt)
		{
			string[]	files;
			
			try
			{
				files = System.IO.Directory.GetFiles(dir, "*", opt);
			}
			catch (Exception ex)
			{
				ErrLog.WriteException(ex);
				
				return null;
			}
			
			return files;
		}
		
		/// <summary>
		/// 指定したフォルダ以下のファイルを全て取得します。
		/// </summary>
		/// <param name="dir">フォルダ</param>
		public static string[] GetFiles(string dir)
		{
			return GetFiles(dir, SearchOption.AllDirectories);
		}
		
		/// <summary>
		/// 指定したフォルダ以下のサブフォルダを全て取得します。
		/// </summary>
		/// <param name="dir">フォルダ</param>
		/// <param name="opt">サーチオプション</param>
		/// <returns>検索したディレクトリ配列</returns>
		public static string[] GetDirectories(string dir, SearchOption opt)
		{
			string[]	directories;
			
			try
			{
				directories = System.IO.Directory.GetDirectories(dir, "*", opt);
			}
			catch (Exception ex)
			{
				ErrLog.WriteException(ex);
				
				return null;
			}
			
			return directories;
		}
		
		/// <summary>
		/// 指定したフォルダ以下のサブフォルダを全て取得します。
		/// </summary>
		/// <param name="dir">フォルダ</param>
		public static string[] GetDirectories(string dir)
		{
			return GetDirectories(dir, SearchOption.AllDirectories);
		}
		
		/// <summary>
		/// カレントフォルダを取得します。
		/// </summary>
		public static string GetCurrentDirectory()
		{
			string	dir;
			
			try
			{
				dir = Directory.GetCurrentDirectory();
			}
			catch (Exception ex)
			{
				ErrLog.WriteException(ex);
				dir = null;
			}
			
			return dir;
		}
		
		/// <summary>
		/// カレントフォルダを設定します。
		/// </summary>
		/// <param name="dir">カレントフォルダパス</param>
		public static void SetCurrentDirectory(string dir)
		{
			try
			{
				Directory.SetCurrentDirectory(dir);
			}
			catch (Exception ex)
			{
				ErrLog.WriteException(ex);
			}
			
			return;
		}
		
		/// <summary>
		/// 作成日時を取得します
		/// </summary>
		public static DateTime GetCreationTime(string filename)
		{
			DateTime	dt = DateTime.MinValue;
			
			try
			{
				dt = Directory.GetCreationTime(filename);
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
				Directory.SetCreationTime(filename, dt);
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
				dt = Directory.GetLastWriteTime(filename);
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
				Directory.SetLastWriteTime(filename, dt);
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
				dt = Directory.GetLastAccessTime(filename);
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
				Directory.SetLastAccessTime(filename, dt);
			}
			catch(Exception ex)
			{
				ErrLog.WriteException(ex);
				return false;
			}
			
			return true;
		}
	}
}
