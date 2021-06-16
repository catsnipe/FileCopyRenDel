using System;
using System.IO;

namespace Uindies.Library
{
	/// <summary>
	/// [�쐬�� fj]
	/// File �Ɋւ��閽�߃Z�b�g�ł��B
	/// </summary>
	public class FileIO
	{
		/// <summary>
		/// �t�@�C�������݂��邩���ׂ܂�
		/// </summary>
		public static bool Exists(string filename)
		{
			return File.Exists(filename);
		}
		
		/// <summary>
		/// �t�@�C�����폜���܂�
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
		/// �t�@�C�����ړ����܂�
		/// �ŏI�X�V���t�͂��̂܂܂̒l��ێ����܂�
		/// </summary>
		/// <param name="src">�ړ���</param>
		/// <param name="dst">�ړ���</param>
		public static bool Move(string src, string dst)
		{
			// ���݂��Ȃ��̂ňړ��ł��Ȃ�
			if (Exists(src) == false)
			{
//				ErrLog.WriteLine("�ړ��������݂��܂���B");
				return false;
			}
			
			// ���݂���̂ňړ��ł��Ȃ�
			if (Exists(dst) == true)
			{
//				ErrLog.WriteLine("�ړ��悪���ɑ��݂��Ă��܂��B");
				return false;
			}
			
			try
			{
				DateTime	dt;
				
				// �ŏI�X�V���t�����̂܂܂ňړ�����
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
		/// �t�@�C�����R�s�[���܂�
		/// �ŏI�X�V���t�͂��̂܂܂̒l��ێ����܂�
		/// </summary>
		/// <param name="src">�R�s�[��</param>
		/// <param name="dst">�R�s�[��</param>
		public static bool Copy(string src, string dst)
		{
			// ���݂��Ȃ��̂ŃR�s�[�ł��Ȃ�
			if (Exists(src) == false)
			{
				ErrLog.WriteLine("�R�s�[�������݂��܂���B");
				return false;
			}
			
			try
			{
				DateTime	dt;
				
				// �ŏI�X�V���t�����̂܂܂ŃR�s�[����
				dt = GetLastWriteTime(src);
				File.Copy(src, dst, true);
				
				// �i����Ɂj�ǂݎ�葮����ݒ肵�Ă���ꍇ�͉���
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
		/// �쐬�������擾���܂�
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
		/// �쐬������ݒ肵�܂�
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
		/// �X�V�������擾���܂�
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
		/// �X�V������ݒ肵�܂�
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
		/// �A�N�Z�X�������擾���܂�
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
		/// �A�N�Z�X������ݒ肵�܂�
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
		/// �t�@�C���������擾���܂�
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
		/// �t�@�C��������ݒ肵�܂�
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
		/// �t�@�C���������������܂�
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
