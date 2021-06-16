using System;
using System.IO;

namespace Uindies.Library
{
	/// <summary>
	/// [�쐬�� fj]
	/// �o�C�i���t�@�C���̓ǂݏ������s���܂��B
	/// </summary>
	public class FileBinary
	{
		/// <summary>
		/// �o�C�i���t�@�C����ǂݍ��݂܂��B
		/// </summary>
		/// <param name="filename">�t�@�C����</param>
		/// <param name="size">�t�@�C���T�C�Y</param>
		/// <returns>�ǂݍ��܂ꂽ�o�C�g�f�[�^</returns>
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
				
				// ���e�����ׂēǂݍ���
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
				// ����
				if (br != null)
				{
					br.Close();
				}
			}
			
			return b;
		}
		
		/// <summary>
		/// �o�C�i���t�@�C����ǂݍ��݂܂��B
		/// </summary>
		/// <param name="filename">�t�@�C����</param>
		/// <returns>�ǂݍ��܂ꂽ�o�C�g�f�[�^</returns>
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
				
				// ���e�����ׂēǂݍ���
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
				// ����
				if (br != null)
				{
					br.Close();
				}
			}
			
			return b;
		}
		
		/// <summary>
		/// �o�C�i���t�@�C����ǂݍ��݂܂��B
		/// </summary>
		/// <param name="filename">�t�@�C����</param>
		/// <param name="size">�ǂݍ��ރT�C�Y</param>
		/// <returns>�ǂݍ��܂ꂽ�o�C�g�f�[�^</returns>
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
				// ���e�����ׂēǂݍ���
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
				// ����
				if (br != null)
				{
					br.Close();
				}
			}
			
			return b;
		}
		
		/// <summary>
		/// �o�C�i���t�@�C���������o���܂��B
		/// </summary>
		/// <param name="filename">�t�@�C����</param>
		/// <param name="data">�������ރo�C�g�f�[�^</param>
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
				// ����
				if (bw != null)
				{
					bw.Close();
				}
			}
			
			return ret;
		}
		
		/// <summary>
		/// �o�C�i���t�@�C���������o���܂��B
		/// </summary>
		/// <param name="filename">�t�@�C����</param>
		/// <param name="data">�������ރo�C�g�f�[�^</param>
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
				// ����
				if (bw != null)
				{
					bw.Close();
				}
			}
			
			return ret;
		}
	}
}
