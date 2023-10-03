using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace UIndies.Library
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
            /// <summary></summary>
            ToLower,
            /// <summary></summary>
            ToUpper,
        };
        
        /// <summary></summary>
        public class UserHistory
        {
            /// <summary></summary>
            public string		Name;
            /// <summary></summary>
            public string		Filepath;
            /// <summary></summary>
            public int          FileType;
            /// <summary></summary>
            public eExecMode	Mode;
            /// <summary></summary>
            public string		KeywordBefore;
            /// <summary></summary>
            public string		KeywordAfter;
            /// <summary></summary>
            public bool			IsRegex;
        }
        
        class FileType
        {
            public SearchOption SearchOption;
            public bool         TypeDirectory;
            public string       Description;
        }

        class ExecType
        {
            public string       Description;
        }

        List<FileType[]> fileTypes = new List<FileType[]>()
        {
            {
                new FileType[]
                {
                    new FileType()
                    {
                        SearchOption   = SearchOption.AllDirectories,
                        TypeDirectory  = false,
                        Description    = "ファイルを処理する（サブディレクトリー含む）",
                    }
                }
            },
            {
                new FileType[]
                {
                    new FileType()
                    {
                        SearchOption   = SearchOption.TopDirectoryOnly,
                        TypeDirectory  = false,
                        Description    = "ファイルを処理する",
                    }
                }
            },
            {
                new FileType[]
                {
                    new FileType()
                    {
                        SearchOption   = SearchOption.AllDirectories,
                        TypeDirectory  = true,
                        Description    = "ファイル＋ディレクトリー（サブディレクトリー含む）",
                    },
                    new FileType()
                    {
                        SearchOption   = SearchOption.AllDirectories,
                        TypeDirectory  = false,
                    }
                }
            },
            {
                new FileType[]
                {
                    new FileType()
                    {
                        SearchOption   = SearchOption.TopDirectoryOnly,
                        TypeDirectory  = true,
                        Description    = "ファイル＋ディレクトリー",
                    },
                    new FileType()
                    {
                        SearchOption   = SearchOption.TopDirectoryOnly,
                        TypeDirectory  = false,
                    }
                }
            },
        };


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
            layout.AddControl(txtResult, FormLayoutRule.None, FormLayoutRule.None, FormLayoutRule.LinkBottom, FormLayoutRule.LinkBottom);
            layout.AddControl(sHistory, FormLayoutRule.None, FormLayoutRule.None, FormLayoutRule.None, FormLayoutRule.LinkBottom);
            layout.AddControl(pnlHistory, FormLayoutRule.None, FormLayoutRule.None, FormLayoutRule.None, FormLayoutRule.LinkBottom);
            layout.EndAdd();
            
            cmbFileType.BeginUpdate();
            cmbFileType.Items.Clear();
            foreach (var filetype in fileTypes)
            {
                cmbFileType.Items.Add(filetype[0].Description);
            }
            cmbFileType.EndUpdate();

            this.FormClosing				+= Form1_FormClosing;
            btnExec.Click					+= btnExec_Click;
            btnTest.Click					+= btnTest_Click;
            txtFolder.DragEnter				+= txtDir_DragEnter;
            txtFolder.DragDrop				+= txtDir_DragDrop;
            
            txtFolder.TextChanged			+= content_Changed;
            radCopy.CheckedChanged			+= content_Changed;
            radDelete.CheckedChanged		+= content_Changed;
            radRename.CheckedChanged		+= content_Changed;
            radToLower.CheckedChanged		+= content_Changed;
            radToUpper.CheckedChanged		+= content_Changed;
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
            
            if (radDelete.Checked  == true ||
                radToLower.Checked == true ||
                radToUpper.Checked == true)
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
            if (cmbFileType.SelectedIndex < 0 || cmbFileType.SelectedIndex >= fileTypes.Count)
            {
                MessageBox.Show("対象ファイルが選択されていません。");
                return;
            }

            StringBuilder sb     = new StringBuilder();
            FileType[]    ftypes = fileTypes[cmbFileType.SelectedIndex];
            int           cnt    = 0;

            sb.AppendLine("[match test]");
            sb.AppendLine("");
            
            foreach (var filetype in ftypes)
            {
                SearchOption  opt   = filetype.SearchOption;
                List<string>  files = getFiles(opt, chkRegex.Checked, filetype.TypeDirectory);

                if (filetype.TypeDirectory == true)
                {
                    sb.AppendLine($"[directories]");
                }
                else
                {
                    sb.AppendLine($"[files]");
                }

                txtResult.Text = "";
                for (int i = 0; i < files.Count; i++)
                {
                    string dir      = Path.GetDirectoryName(files[i]);
                    string file     = Path.GetFileName(files[i]);
                    string newfile;
                    
                    newfile = getNewFile(file);

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
            }
            sb.AppendFormat("{0} counts matched.\r\n", cnt);
            
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
            his.FileType         = cmbFileType.SelectedIndex;
            if (radCopy.Checked == true)
            {
                his.Mode		 = eExecMode.Copy;
                his.Name		 = string.Format("[ copy ] \"{0}\" ＞ \"{1}\"", txtBefore.Text, txtAfter.Text);
            }
            else
            if (radDelete.Checked == true)
            {
                his.Mode		 = eExecMode.Delete;
                his.Name		 = string.Format("[delete] \"{0}\"", txtBefore.Text);
            }
            else
            if (radRename.Checked == true)
            {
                his.Mode		 = eExecMode.Rename;
                his.Name		 = string.Format("[rename] \"{0}\" ＞ \"{1}\"", txtBefore.Text, txtAfter.Text);
            }
            else
            if (radToLower.Checked == true)
            {
                his.Mode		 = eExecMode.ToLower;
                his.Name		 = string.Format("[lower ] \"{0}\"", txtBefore.Text);
            }
            else
            if (radToUpper.Checked == true)
            {
                his.Mode		 = eExecMode.ToUpper;
                his.Name		 = string.Format("[upper ] \"{0}\"", txtBefore.Text);
            }
            his.KeywordBefore	 = txtBefore.Text;
            his.KeywordAfter	 = txtAfter.Text;
            his.IsRegex			 = chkRegex.Checked;
            
            return his;
        }
        
        void historySet(UserHistory his)
        {
            txtFolder.Text		      = his.Filepath;
            cmbFileType.SelectedIndex = his.FileType;
            if (his.Mode == eExecMode.Copy)
            {
                radCopy.Checked	 = true;
            }
            else
            if (his.Mode == eExecMode.Delete)
            {
                radDelete.Checked = true;
            }
            else
            if (his.Mode == eExecMode.Rename)
            {
                radRename.Checked = true;
            }
            else
            if (his.Mode == eExecMode.ToLower)
            {
                radToLower.Checked = true;
            }
            else
            if (his.Mode == eExecMode.ToUpper)
            {
                radToUpper.Checked = true;
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
            if (cmbFileType.SelectedIndex < 0 || cmbFileType.SelectedIndex >= fileTypes.Count)
            {
                MessageBox.Show("対象ファイルが選択されていません。");
                return;
            }

            StringBuilder sb     = new StringBuilder();
            FileType[]    ftypes = fileTypes[cmbFileType.SelectedIndex];
            int           cnt    = 0;
            int           ngcnt  = 0;

            if (radCopy.Checked == true)
            {
                sb.AppendLine("[copy]");
            }
            else
            if (radDelete.Checked == true)
            {
                sb.AppendLine("[delete]");
            }
            else
            if (radRename.Checked == true)
            {
                sb.AppendLine("[rename]");
            }
            else
            if (radToLower.Checked == true)
            {
                sb.AppendLine("[to lower]");
            }
            else
            {
                sb.AppendLine("[to upper]");
            }
            sb.AppendLine("");
            
            foreach (var filetype in ftypes)
            {
                SearchOption  opt   = filetype.SearchOption;
                List<string>  files = getFiles(opt, chkRegex.Checked, filetype.TypeDirectory);

                if (filetype.TypeDirectory == true)
                {
                    sb.AppendLine($"[directories]");
                }
                else
                {
                    sb.AppendLine($"[files]");
                }

                txtResult.Text = "";
                for (int i = 0; i < files.Count; i++)
                {
                    string dir = Path.GetDirectoryName(files[i]);
                    string file = Path.GetFileName(files[i]);
                    string newfile;

                    newfile = getNewFile(file);

                    //if (radDelete.Checked == true)
                    //{
                    //    if (chkRegex.Checked == false)
                    //    {
                    //        if (file.IndexOf(txtBefore.Text) < 0)
                    //        {
                    //            continue;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (Regex.IsMatch(file, txtBefore.Text) == false)
                    //        {
                    //            continue;
                    //        }
                    //    }
                    //    newfile = "×";
                    //}
                    //else
                    //if (radCopy.Checked == true || radRename.Checked == true)
                    //{
                    //    newfile = getCopyNewFile(file);
                    //    if (string.IsNullOrEmpty(newfile) == true || newfile == file)
                    //    {
                    //        continue;
                    //    }
                    //}

                    bool success = true;

                    try
                    {
                        if (radDelete.Checked == true)
                        {
                            if (filetype.TypeDirectory == true)
                            {
                                if (Directory.Exists(files[i]) == true)
                                {
                                    Directory.Delete(files[i], true);
                                }
                            }
                            else
                            {
                                if (File.Exists(files[i]) == true)
                                {
                                    File.Delete(files[i]);
                                }
                            }
                        }
                        else
                        if (radCopy.Checked == true)
                        {
                            var newpath = Path.Combine(dir, newfile);

                            if (filetype.TypeDirectory == true)
                            {
                                copyDirectory(files[i], newpath);
                            }
                            else
                            {
                                //if (FileIO.Exists(newpath) == true)
                                //{
                                //    File.Delete(newpath);
                                //}
                                File.Copy(files[i], newpath, true);
                            }
                        }
                        else
                        if (radRename.Checked  == true ||
                            radToLower.Checked == true ||
                            radToUpper.Checked == true)
                        {
                            var newpath = Path.Combine(dir, newfile);

                            if (filetype.TypeDirectory == true)
                            {
                                if (Path.GetFileName(files[i]).ToLower() == newfile.ToLower())
                                {
                                    var guid8    = Guid.NewGuid().ToString().Substring(0, 8);
                                    var temppath = Path.Combine(Path.GetDirectoryName(files[i]), $"rename{guid8}");

                                    Directory.Move(files[i], temppath);
                                    Directory.Move(temppath, newpath);
                                }
                                else
                                {
                                    if (File.Exists(newpath) == true)
                                    {
                                        success = false;
                                        ngcnt++;
                                    }

                                    Directory.Move(files[i], newpath);
                                }
                            }
                            else
                            {
                                if (Path.GetFileName(files[i]).ToLower() == newfile.ToLower())
                                {
                                    var guid8    = Guid.NewGuid().ToString().Substring(0, 8);
                                    var temppath = Path.Combine(Path.GetDirectoryName(files[i]), $"rename{guid8}.tmp");

                                    File.Move(files[i], temppath);
                                    File.Move(temppath, newpath);
                                }
                                else
                                {
                                    if (File.Exists(newpath) == true)
                                    {
                                        success = false;
                                        ngcnt++;
                                    }

                                    File.Move(files[i], newpath);
                                }
                            }
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
            }

            sb.AppendFormat("{0} files replaced.\r\n", cnt);
            sb.AppendFormat("{0} files failed.\r\n", ngcnt);

            txtResult.Text = sb.ToString();
            txtResult.SelectionStart = txtResult.Text.Length;
            txtResult.ScrollToCaret();
            Application.DoEvents();
        }

        /// <summary>
        /// ディレクトリコピー
        /// </summary>
        /// <param name="srcdir">コピー元ディレクトリ</param>
        /// <param name="dstdir">コピー先ディレクトリ</param>
        public static void copyDirectory(string srcdir, string dstdir)
        {
            if (!Directory.Exists(dstdir))
            {
                Directory.CreateDirectory(dstdir);
            }
 
            File.SetAttributes(dstdir, File.GetAttributes(srcdir));
 
            Directory.GetFiles(srcdir).ToList().ForEach
            (
                f => File.Copy(f, Path.Combine(dstdir, Path.GetFileName(f)), true)
            );
 
            Directory.GetDirectories(srcdir).ToList().ForEach
            (
                d => copyDirectory(d, Path.Combine(dstdir, Path.GetFileName(d)))
            );
        }

        List<string> getFiles(SearchOption opt, bool regex, bool directory)
        {
            List<string>	files;

            try
            {
                if (regex == false)
                {
                    if (directory == true)
                    {
                        files = new List<string>(Directory.GetDirectories(txtFolder.Text, "*" + txtBefore.Text + "*", opt));
                    }
                    else
                    {
                        files = new List<string>(Directory.GetFiles(txtFolder.Text, "*" + txtBefore.Text + "*", opt));
                    }
                }
                else
                {
                    if (directory == true)
                    {
                        files = new List<string>(Directory.GetDirectories(txtFolder.Text, "*", opt));
                    }
                    else
                    {
                        files = new List<string>(Directory.GetFiles(txtFolder.Text, "*", opt));
                    }
                }
            }
            catch
            {
                MessageBox.Show(string.Format("無効なファイルパス文字列が含まれています。 '{0}'", txtBefore.Text));
                return null;
            }

            return files;
        }

        string getNewFile(string file)
        {
            string newfile = null;

            if (radDelete.Checked == true)
            {
                if (chkRegex.Checked == false)
                {
                    if (file.IndexOf(txtBefore.Text) < 0)
                    {
                        return null;
                    }
                }
                else
                {
                    if (Regex.IsMatch(file, txtBefore.Text) == false)
                    {
                        return null;
                    }
                }
                newfile = "×";
            }
            else
            if (radCopy.Checked == true || radRename.Checked == true)
            {
                newfile = getCopyNewFile(file);
                if (string.IsNullOrEmpty(newfile) == true || newfile == file)
                {
                    return null;
                }
            }
            else
            if (radToLower.Checked == true)
            {
                newfile = file.ToLower();
            }
            else
            if (radToUpper.Checked == true)
            {
                newfile = file.ToUpper();
            }
            
            return newfile;
        }

        string getCopyNewFile(string file)
        {
            string newfile = null;

            try
            {
                if (string.IsNullOrEmpty(txtBefore.Text) == true)
                {
                    newfile = txtAfter.Text + file;
                }
                else
                {
                    if (chkRegex.Checked == false)
                    {
                        newfile = file.Replace(txtBefore.Text, txtAfter.Text);
                    }
                    else
                    {
                        newfile = Regex.Replace(file, txtBefore.Text, txtAfter.Text);
                    }
                }
            }
            catch
            {
                MessageBox.Show(string.Format("正規表現でエラーが発生しました [{0}]", txtBefore.Text));
            }

            return newfile;
        }

        void regRead()
        {
            txtFolder.Text            = Cast.String(RegCommon.Read("txtFolder"));
            cmbFileType.SelectedIndex = Cast.Int(RegCommon.Read("cmbFileType"));
            radCopy.Checked           = Cast.Bool(RegCommon.Read("radCopy"));
            radRename.Checked         = Cast.Bool(RegCommon.Read("radRename"));
            radDelete.Checked         = Cast.Bool(RegCommon.Read("radDelete"));
            radToLower.Checked        = Cast.Bool(RegCommon.Read("radToLower"));
            radToUpper.Checked        = Cast.Bool(RegCommon.Read("radToUpper"));
            txtBefore.Text            = Cast.String(RegCommon.Read("txtBefore"));
            txtAfter.Text             = Cast.String(RegCommon.Read("txtAfter"));
            chkRegex.Checked          = Cast.Bool(RegCommon.Read("chkRegex"));
            
            string json = Cast.String(RegCommon.Read("history"));
            
            users = JsonConvert.DeserializeObject<List<UserHistory>>(json);
            if (users == null)
            {
                users = new List<UserHistory>();
            }

            refreshHistory();
        }
        
        void regWrite()
        {
            RegCommon.Write("txtFolder", txtFolder.Text);
            RegCommon.Write("cmbFileType", cmbFileType.SelectedIndex);
            RegCommon.Write("radCopy", radCopy.Checked);
            RegCommon.Write("radRename", radRename.Checked);
            RegCommon.Write("radDelete", radDelete.Checked);
            RegCommon.Write("radToLower", radToLower.Checked);
            RegCommon.Write("radToUpper", radToUpper.Checked);
            RegCommon.Write("txtBefore", txtBefore.Text);
            RegCommon.Write("txtAfter", txtAfter.Text);
            RegCommon.Write("chkRegex", chkRegex.Checked);
            
            string json = JsonConvert.SerializeObject(users);
            
            RegCommon.Write("history", json);
        }
        
    }
}
