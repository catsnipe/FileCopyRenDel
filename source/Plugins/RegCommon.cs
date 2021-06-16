using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;

namespace Uindies.Library
{
	/// <summary>
	/// [�쐬�� fj]
	/// ComponentRegistry.RegCommon �̊T�v�̐����ł��B
	/// </summary>
	public class RegCommon
	{
		const	string	RegFilename		= "prop.xml";
		const	string	NullValueString = "#null";
		
		static	string	reg_tree;
		
		#region ***** Property *****
		/// <summary>
		/// ���W�X�g���ɏ������ރX�^�[�g�|�W�V����
		/// </summary>
		public static string	RegTree
		{
			get
			{
				return reg_tree;
			}
		}
		#endregion
		
		/// <summary>
		/// RegCommon��Dictionary�����Ċi�[���邽�߂̃o�b�t�@
		/// </summary>
		[DataContract]
		class RegData
		{
			[DataMember]
			Dictionary<string, string>	keys;
			
			/// <summary>
			/// 
			/// </summary>
			public RegData()
			{
				keys = new Dictionary<string, string>();
			}
			
			/// <summary>
			/// 
			/// </summary>
			public void Set(string key, object val)
			{
				if (val == null)
				{
					val = NullValueString;
				}
				
				if (keys.ContainsKey(key) == true)
				{
					keys[key] = val.ToString();
				}
				else
				{
					keys.Add(key, val.ToString());
				}
			}
			
			/// <summary>
			/// 
			/// </summary>
			public string Get(string key)
			{
				if (keys.ContainsKey(key) == true)
				{
					if (keys[key] == NullValueString)
					{
						return null;
					}
					
					return keys[key];
				}
				else
				{
					return null;
				}
			}
			
			/// <summary>
			/// 
			/// </summary>
			public void Delete(string key)
			{
				if (keys.ContainsKey(key) == true)
				{
					keys.Remove(key);
				}
			}
			
			/// <summary>
			/// 
			/// </summary>
			public void DeleteTree(string key)
			{
				List<string>	dellist = new List<string>();
				
				foreach (KeyValuePair<string, string>pair in keys)
				{
					if (pair.Key.IndexOf(key) == 0)
					{
						dellist.Add(pair.Key);
					}
				}
				
				foreach (string del in dellist)
				{
					keys.Remove(del);
				}
			}
			
			/// <summary>
			/// 
			/// </summary>
			public bool ContainsKey(string key)
			{
				return keys.ContainsKey(key);
			}
		}
		static RegData regdata;
		
		/// <summary>
		/// ���W�X�g���ɏ������ރ|�W�V�����c���[��ݒ肵�܂��B
		/// </summary>
		/// <param name="tree">�������ރ|�W�V�����c���[</param>
		public static void Init(string tree)
		{
			string path = Application.StartupPath;
//MessageBox.Show(string.Format("startuppath:{0}", path));
			
			// �f�o�b�K�g�p�̏ꍇ
			if (Debugger.IsAttached == true)
			{
				// _tools\config �ɂȂ�悤��������
				for ( ; path != null; )
				{
					if (DirectoryIO.Exists(path + @"\" + tree) == true)
					{
						break;
					}
					path = Path.GetDirectoryName(path);
				}
				
				// �Ȃ������̂Ŏd���Ȃ��߂�
				if (path == null)
				{
					path = Application.StartupPath;
				}
			}
			path += @"\";
			
			// �f�B���N�g�����Ȃ���΍쐬
//MessageBox.Show(string.Format("search:{0}", path + tree));
			if (DirectoryIO.Exists(path + tree) == false)
			{
				DirectoryIO.Make(path + tree);
			}
			
			// �p�X�������� \ �m�F�B�Ȃ���Βǉ�
			if (tree[tree.Length-1] != '\\')
			{
				tree += @"\";
			}
			tree = path + tree + RegFilename;
			
			reg_tree = tree;
			
			try
			{
				if (FileIO.Exists(reg_tree) == true)
				{
					//DataContractSerializer�I�u�W�F�N�g���쐬
					DataContractSerializer serializer = new DataContractSerializer(typeof(RegData));
					//�ǂݍ��ރt�@�C�����J��
					XmlReader xr = XmlReader.Create(reg_tree);
					//XML�t�@�C������ǂݍ��݁A�t�V���A��������
					regdata = (RegData)serializer.ReadObject(xr);
					//�t�@�C�������
					xr.Close();
				}
				else
				{
					regdata = new RegData();
				}
			}
			catch
			{
				regdata = new RegData();
			}
		}
		
		/// <summary>
		/// �I���B�t�@�C�������o��
		/// </summary>
		public static void Exit()
		{
			fileWrite();
		}
		
		/// <summary>
		/// ���W�X�g����Tree�����������܂�
		/// </summary>
		/// <param name="field">���W�X�g����</param>
		static RegistryKey PrepareRegKey(string field)
		{
			string		tree;
			
			if (reg_tree == null)
			{
				ErrLog.WriteLine("�L�[�̈ʒu���w�肳��Ă��܂���.");
				return null;
			}
			
			// �T�u�c���[������Ώ���Ƀo����
			if (field.IndexOf(@"\") >= 0)
			{
				tree  = @"\" + field.Substring(0, field.LastIndexOf(@"\"));
				field = field.Remove(0, field.LastIndexOf(@"\")+1);
			}
			else
			{
				tree = "";
			}
			
			//�� ���W�X�g���ւ̏�������
			//�i�uHKEY_LOCAL_MACHINE\SOFTWARE\???�v�ɏ������ށj
			// �L�[���J��
			try
			{
				return Registry.LocalMachine.CreateSubKey(@"SOFTWARE\" + reg_tree + tree);
			}
			catch
			{
				ErrLog.WriteLine("�L�[�̃A�N�Z�X�Ɏ��s���܂���.");
				return null;
			}
		}
		
		/// <summary>
		/// ���W�X�g���ɏ������݂܂�
		/// </summary>
		/// <param name="field">���W�X�g����</param>
		/// <param name="value">�l</param>
		public static void Write(string field, object value)
		{
			if (regdata == null)
			{
				ErrLog.WriteLine("����������Ă��܂���.");
				return;
			}
			
			regdata.Set(field, value);
			// �f�o�b�O���̓f�o�b�O�I���ŏ������܂�Ȃ����Ƃ������̂ŁA�����i�����͏d���j
			if (Debugger.IsAttached == true)
			{
				fileWrite();
			}
		}
		
		/// <summary>
		/// ���W�X�g���̒l��ǂݎ��܂�
		/// </summary>
		/// <param name="field">���W�X�g����</param>
		/// <returns>���W�X�g���l</returns>
		public static object Read(string field)
		{
			if (regdata == null)
			{
				ErrLog.WriteLine("����������Ă��܂���.");
				return null;
			}
			
			return regdata.Get(field);
		}
		
		/// <summary>
		/// ���W�X�g���̒l��ǂݎ��܂�
		/// </summary>
		/// <param name="field">���W�X�g����</param>
		/// <returns>���W�X�g���l</returns>
		public static string ReadString(string field)
		{
			if (regdata == null)
			{
				ErrLog.WriteLine("����������Ă��܂���.");
				return null;
			}
			
			return Cast.String(regdata.Get(field));
		}
		
		/// <summary>
		/// ���W�X�g�����폜���܂�
		/// </summary>
		/// <param name="field">���W�X�g����</param>
		public static void Delete(string field)
		{
			if (regdata == null)
			{
				ErrLog.WriteLine("����������Ă��܂���.");
				return;
			}
			
			regdata.Delete(field);
			// �f�o�b�O���̓f�o�b�O�I���ŏ������܂�Ȃ����Ƃ������̂ŁA�����i�����͏d���j
			if (Debugger.IsAttached == true)
			{
				fileWrite();
			}
		}
		
		/// <summary>
		/// ���W�X�g�����c���[���ƍ폜���܂�
		/// </summary>
		/// <param name="field">�c���[��</param>
		public static void DeleteTree(string field)
		{
			if (regdata == null)
			{
				ErrLog.WriteLine("����������Ă��܂���.");
				return;
			}
			
			regdata.DeleteTree(field);
			// �f�o�b�O���̓f�o�b�O�I���ŏ������܂�Ȃ����Ƃ������̂ŁA�����i�����͏d���j
			if (Debugger.IsAttached == true)
			{
				fileWrite();
			}
		}
		
		/// <summary>
		/// �t�@�C�������o��
		/// </summary>
		static void fileWrite()
		{
			DataContractSerializer serializer = new DataContractSerializer(typeof(RegData));
			// �o�͐ݒ�
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.OmitXmlDeclaration = true;
			settings.IndentChars = "\t";
			settings.NewLineChars = "\n";
			settings.Encoding = new System.Text.UTF8Encoding(false);
			
			XmlWriter xw = XmlWriter.Create(reg_tree, settings);
			//�V���A�������AXML�t�@�C���ɕۑ�����
			serializer.WriteObject(xw, regdata);
			//�t�@�C�������
			xw.Close();
		}
		
	}
}
