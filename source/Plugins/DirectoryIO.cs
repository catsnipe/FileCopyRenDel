using System;
using System.IO;

namespace Uindies.Library
{
	/// <summary>
	/// [�쐬�� fj]
	/// Directory �Ɋւ��閽�߃Z�b�g�ł��B
	/// </summary>
	public class DirectoryIO
	{
		/// <summary>
		/// �f�B���N�g�������݂��邩���ׂ܂�
		/// </summary>
		public static bool Exists(string filename)
		{
			return Directory.Exists(filename);
		}
		
		/// <summary>
		/// �f�B���N�g�����쐬���܂�
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
		/// �f�B���N�g�����폜���܂��B�T�u�f�B���N�g���ȉ���������폜
		/// �z���ɓǂݎ���p����������G���[
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
		/// �f�B���N�g���ړ�
		/// �ړ���͑��݂��ĂȂ��A�ړ����Ɠ����h���C�u�ɂ��邱��
		/// �ړ���͈ړ����̃T�u�f�B���N�g���ł̓_��
		/// �ړ��������݂��Ă��Ȃ���΃_��
		/// �������󂯌p�����
		/// </summary>
		/// <param name="src">�ړ���</param>
		/// <param name="dst">�ړ���</param>
		public static bool Move(string src, string dst)
		{
			// ���݂���̂ňړ��ł��Ȃ�
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
		/// �f�B���N�g���R�s�[
		/// </summary>
		/// <param name="src">�R�s�[����f�B���N�g��</param>
		/// <param name="dst">�R�s�[��̃f�B���N�g��</param>
		public static bool Copy(string src, string dst)
		{
			try
			{
				// �R�s�[��̃f�B���N�g�����Ȃ��Ƃ��͍��
				if (!Directory.Exists(dst))
				{
					Directory.CreateDirectory(dst);
				
					// �������R�s�[
					File.SetAttributes(dst, File.GetAttributes(src));
				}
			
				// �R�s�[��̃f�B���N�g�����̖�����"\"������
				if (dst[dst.Length - 1] != Path.DirectorySeparatorChar)
				{
					dst = dst + Path.DirectorySeparatorChar;
				}
			
				// �R�s�[���̃f�B���N�g���ɂ���t�@�C�����R�s�[
				string[] files = Directory.GetFiles(src);
				foreach (string file in files)
				{
					File.Copy(file, dst + Path.GetFileName(file), true);
				}
			
				// �R�s�[���̃f�B���N�g���ɂ���f�B���N�g���ɂ��āA
				// �ċA�I�ɌĂяo��
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
		/// �w�肵���t�H���_�ȉ��̃t�@�C����S�Ď擾���܂��B
		/// </summary>
		/// <param name="dir">�t�H���_</param>
		/// <param name="opt">�T�[�`�I�v�V����</param>
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
		/// �w�肵���t�H���_�ȉ��̃t�@�C����S�Ď擾���܂��B
		/// </summary>
		/// <param name="dir">�t�H���_</param>
		public static string[] GetFiles(string dir)
		{
			return GetFiles(dir, SearchOption.AllDirectories);
		}
		
		/// <summary>
		/// �w�肵���t�H���_�ȉ��̃T�u�t�H���_��S�Ď擾���܂��B
		/// </summary>
		/// <param name="dir">�t�H���_</param>
		/// <param name="opt">�T�[�`�I�v�V����</param>
		/// <returns>���������f�B���N�g���z��</returns>
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
		/// �w�肵���t�H���_�ȉ��̃T�u�t�H���_��S�Ď擾���܂��B
		/// </summary>
		/// <param name="dir">�t�H���_</param>
		public static string[] GetDirectories(string dir)
		{
			return GetDirectories(dir, SearchOption.AllDirectories);
		}
		
		/// <summary>
		/// �J�����g�t�H���_���擾���܂��B
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
		/// �J�����g�t�H���_��ݒ肵�܂��B
		/// </summary>
		/// <param name="dir">�J�����g�t�H���_�p�X</param>
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
		/// �쐬�������擾���܂�
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
		/// �쐬������ݒ肵�܂�
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
		/// �X�V�������擾���܂�
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
		/// �X�V������ݒ肵�܂�
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
		/// �A�N�Z�X�������擾���܂�
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
		/// �A�N�Z�X������ݒ肵�܂�
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
