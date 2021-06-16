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
	/// [作成者 fj]
	/// ComponentRegistry.RegCommon の概要の説明です。
	/// </summary>
	public class RegCommon
	{
		const	string	RegFilename		= "prop.xml";
		const	string	NullValueString = "#null";
		
		static	string	reg_tree;
		
		#region ***** Property *****
		/// <summary>
		/// レジストリに書き込むスタートポジション
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
		/// RegCommonをDictionary化して格納するためのバッファ
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
		/// レジストリに書き込むポジションツリーを設定します。
		/// </summary>
		/// <param name="tree">書き込むポジションツリー</param>
		public static void Init(string tree)
		{
			string path = Application.StartupPath;
//MessageBox.Show(string.Format("startuppath:{0}", path));
			
			// デバッガ使用の場合
			if (Debugger.IsAttached == true)
			{
				// _tools\config になるよう強制する
				for ( ; path != null; )
				{
					if (DirectoryIO.Exists(path + @"\" + tree) == true)
					{
						break;
					}
					path = Path.GetDirectoryName(path);
				}
				
				// なかったので仕方なく戻す
				if (path == null)
				{
					path = Application.StartupPath;
				}
			}
			path += @"\";
			
			// ディレクトリがなければ作成
//MessageBox.Show(string.Format("search:{0}", path + tree));
			if (DirectoryIO.Exists(path + tree) == false)
			{
				DirectoryIO.Make(path + tree);
			}
			
			// パス名末尾の \ 確認。なければ追加
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
					//DataContractSerializerオブジェクトを作成
					DataContractSerializer serializer = new DataContractSerializer(typeof(RegData));
					//読み込むファイルを開く
					XmlReader xr = XmlReader.Create(reg_tree);
					//XMLファイルから読み込み、逆シリアル化する
					regdata = (RegData)serializer.ReadObject(xr);
					//ファイルを閉じる
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
		/// 終了。ファイル書き出し
		/// </summary>
		public static void Exit()
		{
			fileWrite();
		}
		
		/// <summary>
		/// レジストリのTree情報を準備します
		/// </summary>
		/// <param name="field">レジストリ名</param>
		static RegistryKey PrepareRegKey(string field)
		{
			string		tree;
			
			if (reg_tree == null)
			{
				ErrLog.WriteLine("キーの位置が指定されていません.");
				return null;
			}
			
			// サブツリーがあれば勝手にバラす
			if (field.IndexOf(@"\") >= 0)
			{
				tree  = @"\" + field.Substring(0, field.LastIndexOf(@"\"));
				field = field.Remove(0, field.LastIndexOf(@"\")+1);
			}
			else
			{
				tree = "";
			}
			
			//■ レジストリへの書き込み
			//（「HKEY_LOCAL_MACHINE\SOFTWARE\???」に書き込む）
			// キーを開く
			try
			{
				return Registry.LocalMachine.CreateSubKey(@"SOFTWARE\" + reg_tree + tree);
			}
			catch
			{
				ErrLog.WriteLine("キーのアクセスに失敗しました.");
				return null;
			}
		}
		
		/// <summary>
		/// レジストリに書き込みます
		/// </summary>
		/// <param name="field">レジストリ名</param>
		/// <param name="value">値</param>
		public static void Write(string field, object value)
		{
			if (regdata == null)
			{
				ErrLog.WriteLine("初期化されていません.");
				return;
			}
			
			regdata.Set(field, value);
			// デバッグ時はデバッグ終了で書き込まれないことが多いので、随時（処理は重い）
			if (Debugger.IsAttached == true)
			{
				fileWrite();
			}
		}
		
		/// <summary>
		/// レジストリの値を読み取ります
		/// </summary>
		/// <param name="field">レジストリ名</param>
		/// <returns>レジストリ値</returns>
		public static object Read(string field)
		{
			if (regdata == null)
			{
				ErrLog.WriteLine("初期化されていません.");
				return null;
			}
			
			return regdata.Get(field);
		}
		
		/// <summary>
		/// レジストリの値を読み取ります
		/// </summary>
		/// <param name="field">レジストリ名</param>
		/// <returns>レジストリ値</returns>
		public static string ReadString(string field)
		{
			if (regdata == null)
			{
				ErrLog.WriteLine("初期化されていません.");
				return null;
			}
			
			return Cast.String(regdata.Get(field));
		}
		
		/// <summary>
		/// レジストリを削除します
		/// </summary>
		/// <param name="field">レジストリ名</param>
		public static void Delete(string field)
		{
			if (regdata == null)
			{
				ErrLog.WriteLine("初期化されていません.");
				return;
			}
			
			regdata.Delete(field);
			// デバッグ時はデバッグ終了で書き込まれないことが多いので、随時（処理は重い）
			if (Debugger.IsAttached == true)
			{
				fileWrite();
			}
		}
		
		/// <summary>
		/// レジストリをツリーごと削除します
		/// </summary>
		/// <param name="field">ツリー名</param>
		public static void DeleteTree(string field)
		{
			if (regdata == null)
			{
				ErrLog.WriteLine("初期化されていません.");
				return;
			}
			
			regdata.DeleteTree(field);
			// デバッグ時はデバッグ終了で書き込まれないことが多いので、随時（処理は重い）
			if (Debugger.IsAttached == true)
			{
				fileWrite();
			}
		}
		
		/// <summary>
		/// ファイル書き出し
		/// </summary>
		static void fileWrite()
		{
			DataContractSerializer serializer = new DataContractSerializer(typeof(RegData));
			// 出力設定
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.OmitXmlDeclaration = true;
			settings.IndentChars = "\t";
			settings.NewLineChars = "\n";
			settings.Encoding = new System.Text.UTF8Encoding(false);
			
			XmlWriter xw = XmlWriter.Create(reg_tree, settings);
			//シリアル化し、XMLファイルに保存する
			serializer.WriteObject(xw, regdata);
			//ファイルを閉じる
			xw.Close();
		}
		
	}
}
