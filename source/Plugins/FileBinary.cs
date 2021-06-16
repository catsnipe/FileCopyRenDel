using System;
using System.IO;

namespace Uindies.Library
{
	/// <summary>
	/// [作成者 fj]
	/// バイナリファイルの読み書きを行います。
	/// </summary>
	public class FileBinary
	{
		/// <summary>
		/// バイナリファイルを読み込みます。
		/// </summary>
		/// <param name="filename">ファイル名</param>
		/// <param name="size">ファイルサイズ</param>
		/// <returns>読み込まれたバイトデータ</returns>
		public static byte[] Read(string filename, int size)
		{
			BinaryReader	br = null;
			byte[]			b;
			
			if (FileIO.Exists(filename) == false)
			{
				return null;
			}
			
			try
			{
				br = new BinaryReader(File.OpenRead(filename));
				
				// 内容をすべて読み込む
				b  = new byte[size];
				br.Read(b, 0, size);
			}
			catch(Exception ex)
			{
				ErrLog.WriteException(ex);
				
				b = null;
			}
			finally
			{
				// 閉じる
				if (br != null)
				{
					br.Close();
				}
			}
			
			return b;
		}
		
		/// <summary>
		/// バイナリファイルを読み込みます。
		/// </summary>
		/// <param name="filename">ファイル名</param>
		/// <returns>読み込まれたバイトデータ</returns>
		public static byte[] Read(string filename)
		{
			BinaryReader	br = null;
			byte[]			b;
			
			if (FileIO.Exists(filename) == false)
			{
				return null;
			}
			
			try
			{
				br = new BinaryReader(File.OpenRead(filename));
				
				// 内容をすべて読み込む
				b  = new byte[(int)br.BaseStream.Length];
				br.Read(b, 0, (int)br.BaseStream.Length);
			}
			catch(Exception ex)
			{
				ErrLog.WriteException(ex);
				
				b = null;
			}
			finally
			{
				// 閉じる
				if (br != null)
				{
					br.Close();
				}
			}
			
			return b;
		}
		
		/// <summary>
		/// バイナリファイルを読み込みます。
		/// </summary>
		/// <param name="filename">ファイル名</param>
		/// <param name="size">読み込むサイズ</param>
		/// <returns>読み込まれたバイトデータ</returns>
		public static byte[] ReadSizable(string filename, int size)
		{
			BinaryReader	br = null;
			byte[]			b;
			
			if (FileIO.Exists(filename) == false)
			{
				return null;
			}
			
			try
			{
				br = new BinaryReader(File.OpenRead(filename));
				
				if (size > (int)br.BaseStream.Length)
				{
					size = (int)br.BaseStream.Length;
				}
				// 内容をすべて読み込む
				b  = new byte[size];
				br.Read(b, 0, size);
			}
			catch(Exception ex)
			{
				ErrLog.WriteException(ex);
				
				b = null;
			}
			finally
			{
				// 閉じる
				if (br != null)
				{
					br.Close();
				}
			}
			
			return b;
		}
		
		/// <summary>
		/// バイナリファイルを書き出します。
		/// </summary>
		/// <param name="filename">ファイル名</param>
		/// <param name="data">書き込むバイトデータ</param>
		public static bool Write(string filename, byte[] data)
		{
			BinaryWriter bw  = null;
			bool		 ret = true;
			try
			{
				bw = new BinaryWriter(File.Open(filename, FileMode.Create, FileAccess.Write));
				
				bw.Write(data);
			}
			catch(Exception ex)
			{
				ErrLog.WriteException(ex);
				
				ret = false;
			}
			finally
			{
				// 閉じる
				if (bw != null)
				{
					bw.Close();
				}
			}
			
			return ret;
		}
		
		/// <summary>
		/// バイナリファイルを書き出します。
		/// </summary>
		/// <param name="filename">ファイル名</param>
		/// <param name="data">書き込むバイトデータ</param>
		public static bool Append(string filename, byte[] data)
		{
			BinaryWriter bw  = null;
			bool		 ret = true;
			try
			{
				bw = new BinaryWriter(File.Open(filename, FileMode.Append, FileAccess.Write));
				
				bw.Write(data);
			}
			catch(Exception ex)
			{
				ErrLog.WriteException(ex);
				
				ret = false;
			}
			finally
			{
				// 閉じる
				if (bw != null)
				{
					bw.Close();
				}
			}
			
			return ret;
		}
	}
}
