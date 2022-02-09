using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using UIndies.Library;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace App
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Form1 : Form
    {
        FormLayout	layout;
        
        /// <summary></summary>
        public enum eExecMode
        {
            /// <summary></summary>
            Copy,
            /// <summary></summary>
            Rename,
            /// <summary></summary>
            Delete,
        };
        
        /// <summary></summary>
        public class UserHistory
        {
            /// <summary></summary>
            public string		Name;
            /// <summary></summary>
            public string		Filepath;
            /// <summary></summary>
            public bool			ExecSubDirectory;
            /// <summary></summary>
            public eExecMode	Mode;
            /// <summary></summary>
            public string		KeywordBefore;
            /// <summary></summary>
            public string		KeywordAfter;
            /// <summary></summary>
            public bool			IsRegex;
        }
        
        List<UserHistory>		users;
        
        /// <summary>
        /// 
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            
            RegCommon.Init(@"config\filecrd\");
            
            users = new List<UserHistory>();
            
            layout = new FormLayout(this);
            layout.AddControl(txtResult, FormLayoutRule.None, FormLayoutRule.None, FormLayoutRule.None, FormLayoutRule.LinkBottom);
            layout.AddControl(sHistory, FormLayoutRule.None, FormLayoutRule.None, FormLayoutRule.None, FormLayoutRule.LinkBottom);
            layout.AddControl(pnlHistory, FormLayoutRule.None, FormLayoutRule.None, FormLayoutRule.None, FormLayoutRule.LinkBottom);
            layout.EndAdd();
            
            this.FormClosing				+= Form1_FormClosing;
            btnExec.Click					+= btnExec_Click;
            btnTest.Click					+= btnTest_Click;
            txtFolder.DragEnter				+= txtDir_DragEnter;
            txtFolder.DragDrop				+= txtDir_DragDrop;
            
            txtFolder.TextChanged			+= content_Changed;
            chkSubDir.CheckedChanged		+= content_Changed;
            radCopy.CheckedChanged			+= content_Changed;
            radRename.CheckedChanged		+= content_Changed;
            radDelete.CheckedChanged		+= content_Changed;
            txtBefore.TextChanged			+= content_Changed;
            txtAfter.TextChanged			+= content_Changed;
            chkRegex.CheckedChanged			+= content_Changed;
            sHistory.SelectedIndexChanged	+= sHistory_SelectedIndexChanged;
            sUp.Click						+= sUp_Click;
            sDown.Click						+= sDown_Click;
            sDelete.Click					+= sDelete_Click;
            
            regRead();
        }
        
        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            regWrite();
            
            RegCommon.Exit();
        }
        
        /// <summary>
        /// 少しでも変更があったら「確認」から。
        /// </summary>
        void content_Changed(object sender, EventArgs e)
        {
            txtResult.Text	= "";
            btnExec.Enabled	= false;
            
            if (radDelete.Checked == true)
            {
                txtAfter.Text = "";
                txtAfter.Enabled = false;
            }
            else
            {
                txtAfter.Enabled = true;
            }
        }
        
        /// <summary>
        /// アーカイバオフセットファイルのドロップ受付
        /// </summary>
        void txtDir_DragEnter(object sender, DragEventArgs e)
        {
            // コントロール内にドラッグされたとき実行される
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                // ファイル以外は受け付けない
                e.Effect = DragDropEffects.None;
            }
        }
        
        /// <summary>
        /// アーカイバオフセットファイルのドロップ
        /// </summary>
        void txtDir_DragDrop(object sender, DragEventArgs e)
        {
            Control		ctl = (Control)sender;
            string[]	filename = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            
            if (filename.Length > 0)
            {
                ctl.Text = filename[0];
            }
        }
        
        void btnTest_Click(object sender, EventArgs e)
        {
            regWrite();
            
            if (Directory.Exists(txtFolder.Text) == false)
            {
                MessageBox.Show("指定されたフォルダはありません。");
                return;
            }
            
            SearchOption opt	= chkSubDir.Checked == true ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            
            string[]	 files	= null;
            int			 cnt	= 0;
            
            if (chkRegex.Checked == false)
            {
                try
                {
                    files = Directory.GetFiles(txtFolder.Text, "*" + txtBefore.Text + "*", opt);
                }
                catch
                {
                    MessageBox.Show(string.Format("無効なファイルパス文字列が含まれています。 '{0}'", txtBefore.Text));
                    return;
                }
            }
            else
            {
                files = Directory.GetFiles(txtFolder.Text, "*", opt);
            }
            
            StringBuilder	sb = new StringBuilder();
            sb.AppendLine("[match test]");
            sb.AppendLine("");
            
            txtResult.Text = "";
            for (int i = 0; i < files.Length; i++)
            {
                string dir		= Path.GetDirectoryName(files[i]);
                string file		= Path.GetFileName(files[i]);
                string newfile;
                
                if (radDelete.Checked == true)
                {
                    if (chkRegex.Checked == false)
                    {
                        if (file.IndexOf(txtBefore.Text) < 0)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (Regex.IsMatch(file, txtBefore.Text) == false)
                        {
                            continue;
                        }
                    }
                    newfile = "×";
                }
                else
                {
                    if (chkRegex.Checked == false)
                    {
                        if (txtBefore.Text.Length == 0)
                        {
                            newfile = txtAfter.Text + file;
                        }
                        else
                        {
                            newfile = file.Replace(txtBefore.Text, txtAfter.Text);
                        }
                    }
                    else
                    {
                        try
                        {
                            newfile = Regex.Replace(file, txtBefore.Text, txtAfter.Text);
                            if (file == newfile)
                            {
                                continue;
                            }
                        }
                        catch
                        {
                            MessageBox.Show(string.Format("正規表現でエラーが発生しました [{0}]", txtBefore.Text));
                            break;
                        }
                    }
                }
                sb.AppendFormat(
                    "{0} -> {1}\r\n",
                    file,
                    newfile
                );
                cnt++;
                
                if ((i % 200) == 0)
                {
                    txtResult.Text = sb.ToString();
                    txtResult.SelectionStart = txtResult.Text.Length;
                    txtResult.ScrollToCaret();
                    Application.DoEvents();
                }
            }
            sb.AppendLine("");
            sb.AppendFormat("{0} files matched.\r\n", cnt);
            
            txtResult.Text = sb.ToString();
            txtResult.SelectionStart = txtResult.Text.Length;
            txtResult.ScrollToCaret();
            Application.DoEvents();
            
            btnExec.Enabled = true;
        }
        
        /// <summary>
        /// 
        /// </summary>
        void sHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = sHistory.SelectedIndex;
            if (index < 0 || index >= users.Count)
            {
                return;
            }
            
            historySet(users[index]);
        }
        
        void sUp_Click(object sender, EventArgs e)
        {
            int index = sHistory.SelectedIndex;
            if (index < 0 || index >= users.Count)
            {
                return;
            }
            if (index == 0)
            {
                return;
            }
            
            UserHistory his = users[index];
            
            usersDelete(index);
            usersAdd(index-1, his);
            sHistory.SelectedIndex = index-1;
        }
        
        void sDown_Click(object sender, EventArgs e)
        {
            int index = sHistory.SelectedIndex;
            if (index < 0 || index >= users.Count)
            {
                return;
            }
            if (index == users.Count-1)
            {
                return;
            }
            
            UserHistory his = users[index];
            
            usersDelete(index);
            usersAdd(index+1, his);
            sHistory.SelectedIndex = index+1;
        }
        
        void sDelete_Click(object sender, EventArgs e)
        {
            usersDelete(sHistory.SelectedIndex);
        }
        
        UserHistory historyGet()
        {
            UserHistory his = new UserHistory();
            
            his.Filepath		 = txtFolder.Text;
            his.ExecSubDirectory = chkSubDir.Checked;
            if (radCopy.Checked == true)
            {
                his.Mode		 = eExecMode.Copy;
                his.Name		 = string.Format("[ copy ] \"{0}\" ＞ \"{1}\"", txtBefore.Text, txtAfter.Text);
            }
            else
            if (radRename.Checked == true)
            {
                his.Mode		 = eExecMode.Rename;
                his.Name		 = string.Format("[rename] \"{0}\" ＞ \"{1}\"", txtBefore.Text, txtAfter.Text);
            }
            else
            {
                his.Mode		 = eExecMode.Delete;
                his.Name		 = string.Format("[delete] \"{0}\"", txtBefore.Text);
            }
            his.KeywordBefore	 = txtBefore.Text;
            his.KeywordAfter	 = txtAfter.Text;
            his.IsRegex			 = chkRegex.Checked;
            
            return his;
        }
        
        void historySet(UserHistory his)
        {
            txtFolder.Text		 = his.Filepath;
            chkSubDir.Checked	 = his.ExecSubDirectory;
            if (his.Mode == eExecMode.Copy)
            {
                radCopy.Checked	 = true;
            }
            else
            if (his.Mode == eExecMode.Rename)
            {
                radRename.Checked = true;
            }
            else
            {
                radDelete.Checked = true;
            }
            txtBefore.Text		 = his.KeywordBefore;
            txtAfter.Text		 = his.KeywordAfter;
            chkRegex.Checked	 = his.IsRegex;
        }
        
        void refreshHistory()
        {
            sHistory.BeginUpdate();
            sHistory.Items.Clear();
            if (users != null)
            {
                for (int i = 0; i < users.Count; i++)
                {
                    sHistory.Items.Add(users[i].Name);
                }
            }
            sHistory.EndUpdate();
        }
        
        void usersAdd(int index, UserHistory his)
        {
            UserHistory samehis = users.Find(
                (UserHistory chk) => chk.Name == his.Name
            );
            
            if (samehis == null)
            {
                if (index < 0)
                {
                    users.Add(his);
                }
                else
                {
                    users.Insert(index, his);
                }
            }
            refreshHistory();
        }
        
        void usersDelete(int index)
        {
            if (index < 0 || index >= users.Count)
            {
                return;
            }
            
            users.RemoveAt(index);
            refreshHistory();
        }
        
        void btnExec_Click(object sender, EventArgs e)
        {
            regWrite();
            
            UserHistory his = historyGet();
            usersAdd(-1, his);
            
            refreshHistory();
            
            if (Directory.Exists(txtFolder.Text) == false)
            {
                MessageBox.Show("指定されたフォルダはありません。");
                return;
            }
            
            SearchOption opt	= chkSubDir.Checked == true ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            
            List<string>	files;
            int				cnt	  = 0;
            int				ngcnt = 0;
            
            if (chkRegex.Checked == false)
            {
                files = new List<string>(Directory.GetFiles(txtFolder.Text, "*" + txtBefore.Text + "*", opt));
            }
            else
            {
                files = new List<string>(Directory.GetFiles(txtFolder.Text, "*", opt));
            }
            
            StringBuilder	sb = new StringBuilder();
            
            if (radCopy.Checked == true)
            {
                sb.AppendLine("[copy]");
            }
            else
            if (radRename.Checked == true)
            {
                sb.AppendLine("[rename]");
            }
            else
            {
                sb.AppendLine("[delete]");
            }
            sb.AppendLine("");
            
            txtResult.Text = "";
            for (int i = 0; i < files.Count; i++)
            {
                string dir		= Path.GetDirectoryName(files[i]);
                string file		= Path.GetFileName(files[i]);
                string newfile;
                
                if (radDelete.Checked == true)
                {
                    if (chkRegex.Checked == false)
                    {
                        if (file.IndexOf(txtBefore.Text) < 0)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (Regex.IsMatch(file, txtBefore.Text) == false)
                        {
                            continue;
                        }
                    }
                    newfile = "×";
                }
                else
                {
                    if (chkRegex.Checked == false)
                    {
                        newfile = file.Replace(txtBefore.Text, txtAfter.Text);
                    }
                    else
                    {
                        try
                        {
                            newfile = Regex.Replace(file, txtBefore.Text, txtAfter.Text);
                            if (file == newfile)
                            {
                                continue;
                            }
                        }
                        catch
                        {
                            MessageBox.Show(string.Format("正規表現でエラーが発生しました [{0}]", txtBefore.Text));
                            break;
                        }
                    }
                }
                
                bool	success = true;
                
                try
                {
                    if (radDelete.Checked == true)
                    {
                        if (File.Exists(files[i]) == true)
                        {
                            File.Delete(files[i]);
                        }
                    }
                    else
                    if (radCopy.Checked == true)
                    {
                        if (File.Exists(dir + @"\" + newfile) == true)
                        {
                            File.Delete(dir + @"\" + newfile);
                        }
                        File.Copy(files[i], dir + @"\" + newfile);
                    }
                    else
                    if (radRename.Checked == true)
                    {
                        if (File.Exists(dir + @"\" + newfile) == true)
                        {
                            File.Delete(dir + @"\" + newfile);
                        }
                        File.Move(files[i], dir + @"\" + newfile);
                    }
                }
                catch
                {
                    success = false;
                    ngcnt++;
                }
                
                sb.AppendFormat(
                    "{0}{1} -> {2}\r\n",
                    success == false ? "×" : "○",
                    file,
                    newfile
                );
                cnt++;
                
                if ((i % 200) == 0)
                {
                    txtResult.Text = sb.ToString();
                    txtResult.SelectionStart = txtResult.Text.Length;
                    txtResult.ScrollToCaret();
                    Application.DoEvents();
                }
            }
            sb.AppendLine("");
            sb.AppendFormat("{0} files replaced.\r\n", cnt);
            sb.AppendFormat("{0} files failed.\r\n", ngcnt);
            
            txtResult.Text = sb.ToString();
            txtResult.SelectionStart = txtResult.Text.Length;
            txtResult.ScrollToCaret();
            Application.DoEvents();
        }
        
        void regRead()
        {
            txtFolder.Text		= Cast.String(RegCommon.Read("txtFolder"));
            chkSubDir.Checked	= Cast.Bool(RegCommon.Read("chkSubDir"));
            radCopy.Checked		= Cast.Bool(RegCommon.Read("radCopy"));
            radRename.Checked	= Cast.Bool(RegCommon.Read("radRename"));
            radDelete.Checked	= Cast.Bool(RegCommon.Read("radDelete"));
            txtBefore.Text		= Cast.String(RegCommon.Read("txtBefore"));
            txtAfter.Text		= Cast.String(RegCommon.Read("txtAfter"));
            chkRegex.Checked	= Cast.Bool(RegCommon.Read("chkRegex"));
            
            string json = Cast.String(RegCommon.Read("history"));
            
            users = JsonConvert.DeserializeObject<List<UserHistory>>(json);
            refreshHistory();
        }
        
        void regWrite()
        {
            RegCommon.Write("txtFolder", txtFolder.Text);
            RegCommon.Write("chkSubDir", chkSubDir.Checked);
            RegCommon.Write("radCopy", radCopy.Checked);
            RegCommon.Write("radRename", radRename.Checked);
            RegCommon.Write("radDelete", radDelete.Checked);
            RegCommon.Write("txtBefore", txtBefore.Text);
            RegCommon.Write("txtAfter", txtAfter.Text);
            RegCommon.Write("chkRegex", chkRegex.Checked);
            
            string json = JsonConvert.SerializeObject(users);
            
            RegCommon.Write("history", json);
        }
        
    }
}
