using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Hnx8.ReadJEnc;

namespace Uindies.Library
{
	/// <summary>
	/// [�쐬�� fj]
	/// �e�L�X�g�t�@�C���̓ǂݏ����������܂��B
	/// </summary>
	public class FileText
	{
		/// <summary>
		/// utf-8 �̃G���R�[�h���[�h
		/// </summary>
		public const string Utf8 = "utf-8";
		/// <summary>
		/// shift_jis �̃G���R�[�h���[�h
		/// </summary>
		public const string ShiftJis = "shift_jis";
		
		/// <summary>
		/// �G���R�[�h������
		/// </summary>
		public static string	EncodingWord = "utf-8";
		/// <summary>
		/// ���O�̃G���[���e
		/// </summary>
		public static Exception Exception = null;
		
		/// <summary>
		/// �e�L�X�g�t�@�C����ǂݍ��݂܂��B�قƂ�ǂ̃G���R�[�h�������I�ɓǂ݂킯�܂��B
		/// </summary>
		/// <param name="filename">�ǂݍ��ރt�@�C��</param>
		/// <returns>�ǂݍ��񂾕�����</returns>
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
					//�f�t�H���g�����R�[�h�Ŕ���
					reader.ReadJEnc = ReadJEnc.JP;
					//���ʌ��ʂ̕����R�[�h�́ARead���\�b�h�̖߂�l�Ŕc���ł��܂�
					CharCode c = reader.Read(file);
					//�߂�l�̌^����t�@�C���̑�܂��Ȏ�ނ�����ł��܂��A
					string type =
						(c is CharCode.Text ? "Text:"
						: c is FileType.Bin ? "Binary:"
						: c is FileType.Image ? "Image:"
						: "");
					//�߂�l��Name�v���p�e�B���當���R�[�h�����擾�ł��܂�
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
					
					//�߂�l��GetEncoding()���\�b�h�ŁA�G���R�[�f�B���O���擾�ł��܂�
					System.Text.Encoding enc = c.GetEncoding();
					//���ۂɓǂݏo�����e�L�X�g�́AText�v���p�e�B����擾�ł��܂�
					//�i��e�L�X�g�t�@�C���̏ꍇ�́Anull���ݒ肳��܂��j
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
		/// �e�L�X�g�t�@�C���������o���܂��B
		/// </summary>
		/// <param name="filename">�����o���t�@�C��</param>
		/// <param name="buff">�����o��������o�b�t�@</param>
		/// <param name="encword">�G���R�[�h���[�h�BSystem.Text.Encoding.GetEncodings()���Ԃ��l�̂ǂꂩ</param>
		/// <returns>true..����, false..���s</returns>
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
				// �t�@�C�����㏑�����AShift JIS�ŏ�������
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
				// ����
				if (sw != null)
				{
					sw.Close();
				}
			}
			
			return ret;
		}
		
		/// <summary>
		/// �e�L�X�g�t�@�C���������o���܂��B
		/// </summary>
		/// <param name="filename">�����o���t�@�C��</param>
		/// <param name="buff">�����o��������o�b�t�@</param>
		/// <returns>true..����, false..���s</returns>
		public static bool Write(string filename, string buff)
		{
			return Write(filename, buff, EncodingWord);
		}
	}
}
