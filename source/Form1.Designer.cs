﻿namespace App
{
	partial class Form1
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBefore = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAfter = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnExec = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chkSubDir = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.radCopy = new System.Windows.Forms.RadioButton();
            this.radRename = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnTest = new System.Windows.Forms.Button();
            this.chkRegex = new System.Windows.Forms.CheckBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.radDelete = new System.Windows.Forms.RadioButton();
            this.sHistory = new System.Windows.Forms.ListBox();
            this.pnlHistory = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.sDelete = new System.Windows.Forms.Button();
            this.sUp = new System.Windows.Forms.Button();
            this.sDown = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtFolder
            // 
            this.txtFolder.AllowDrop = true;
            this.txtFolder.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtFolder.Location = new System.Drawing.Point(44, 52);
            this.txtFolder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(537, 30);
            this.txtFolder.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.ForeColor = System.Drawing.Color.SteelBlue;
            this.label1.Location = new System.Drawing.Point(44, 26);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "変更フォルダ";
            // 
            // txtBefore
            // 
            this.txtBefore.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBefore.Location = new System.Drawing.Point(44, 256);
            this.txtBefore.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBefore.Name = "txtBefore";
            this.txtBefore.Size = new System.Drawing.Size(237, 30);
            this.txtBefore.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.ForeColor = System.Drawing.Color.SteelBlue;
            this.label2.Location = new System.Drawing.Point(40, 230);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "キーワード(変更前)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.ForeColor = System.Drawing.Color.SteelBlue;
            this.label3.Location = new System.Drawing.Point(340, 230);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "キーワード(変更後)";
            // 
            // txtAfter
            // 
            this.txtAfter.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtAfter.Location = new System.Drawing.Point(344, 256);
            this.txtAfter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtAfter.Name = "txtAfter";
            this.txtAfter.Size = new System.Drawing.Size(237, 30);
            this.txtAfter.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(301, 260);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 23);
            this.label4.TabIndex = 6;
            this.label4.Text = "＞";
            // 
            // btnExec
            // 
            this.btnExec.Enabled = false;
            this.btnExec.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnExec.Location = new System.Drawing.Point(183, 346);
            this.btnExec.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExec.Name = "btnExec";
            this.btnExec.Size = new System.Drawing.Size(100, 31);
            this.btnExec.TabIndex = 10;
            this.btnExec.Text = "実行";
            this.btnExec.UseVisualStyleBackColor = true;
            // 
            // txtResult
            // 
            this.txtResult.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtResult.Location = new System.Drawing.Point(617, 52);
            this.txtResult.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(411, 572);
            this.txtResult.TabIndex = 15;
            this.txtResult.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.ForeColor = System.Drawing.Color.SteelBlue;
            this.label5.Location = new System.Drawing.Point(615, 26);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 23);
            this.label5.TabIndex = 9;
            this.label5.Text = "確認ログ";
            // 
            // chkSubDir
            // 
            this.chkSubDir.AutoSize = true;
            this.chkSubDir.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkSubDir.Location = new System.Drawing.Point(47, 91);
            this.chkSubDir.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkSubDir.Name = "chkSubDir";
            this.chkSubDir.Size = new System.Drawing.Size(227, 27);
            this.chkSubDir.TabIndex = 2;
            this.chkSubDir.Text = "サブディレクトリも処理する";
            this.chkSubDir.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label6.ForeColor = System.Drawing.Color.SteelBlue;
            this.label6.Location = new System.Drawing.Point(44, 151);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 23);
            this.label6.TabIndex = 11;
            this.label6.Text = "処理方法";
            // 
            // radCopy
            // 
            this.radCopy.AutoSize = true;
            this.radCopy.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.radCopy.Location = new System.Drawing.Point(47, 178);
            this.radCopy.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radCopy.Name = "radCopy";
            this.radCopy.Size = new System.Drawing.Size(76, 27);
            this.radCopy.TabIndex = 3;
            this.radCopy.TabStop = true;
            this.radCopy.Text = "コピー";
            this.radCopy.UseVisualStyleBackColor = true;
            // 
            // radRename
            // 
            this.radRename.AutoSize = true;
            this.radRename.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.radRename.Location = new System.Drawing.Point(145, 178);
            this.radRename.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radRename.Name = "radRename";
            this.radRename.Size = new System.Drawing.Size(91, 27);
            this.radRename.TabIndex = 4;
            this.radRename.TabStop = true;
            this.radRename.Text = "名前変更";
            this.radRename.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.panel1.Location = new System.Drawing.Point(20, 26);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 92);
            this.panel1.TabIndex = 14;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Silver;
            this.panel2.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.panel2.Location = new System.Drawing.Point(20, 151);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 54);
            this.panel2.TabIndex = 15;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Silver;
            this.panel3.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.panel3.Location = new System.Drawing.Point(20, 230);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(5, 92);
            this.panel3.TabIndex = 16;
            // 
            // btnTest
            // 
            this.btnTest.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnTest.Location = new System.Drawing.Point(44, 346);
            this.btnTest.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(100, 31);
            this.btnTest.TabIndex = 9;
            this.btnTest.Text = "確認";
            this.btnTest.UseVisualStyleBackColor = true;
            // 
            // chkRegex
            // 
            this.chkRegex.AutoSize = true;
            this.chkRegex.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkRegex.Location = new System.Drawing.Point(47, 295);
            this.chkRegex.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkRegex.Name = "chkRegex";
            this.chkRegex.Size = new System.Drawing.Size(122, 27);
            this.chkRegex.TabIndex = 8;
            this.chkRegex.Text = "Regexを使用";
            this.chkRegex.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Silver;
            this.panel4.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.panel4.Location = new System.Drawing.Point(20, 346);
            this.panel4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(5, 31);
            this.panel4.TabIndex = 19;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label7.Location = new System.Drawing.Point(152, 350);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(25, 23);
            this.label7.TabIndex = 20;
            this.label7.Text = "＞";
            // 
            // radDelete
            // 
            this.radDelete.AutoSize = true;
            this.radDelete.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.radDelete.Location = new System.Drawing.Point(259, 178);
            this.radDelete.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radDelete.Name = "radDelete";
            this.radDelete.Size = new System.Drawing.Size(91, 27);
            this.radDelete.TabIndex = 5;
            this.radDelete.TabStop = true;
            this.radDelete.Text = "デリート";
            this.radDelete.UseVisualStyleBackColor = true;
            // 
            // sHistory
            // 
            this.sHistory.Font = new System.Drawing.Font("ＭＳ ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.sHistory.FormattingEnabled = true;
            this.sHistory.ItemHeight = 16;
            this.sHistory.Location = new System.Drawing.Point(44, 455);
            this.sHistory.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.sHistory.Name = "sHistory";
            this.sHistory.Size = new System.Drawing.Size(537, 164);
            this.sHistory.TabIndex = 14;
            // 
            // pnlHistory
            // 
            this.pnlHistory.BackColor = System.Drawing.Color.DodgerBlue;
            this.pnlHistory.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.pnlHistory.Location = new System.Drawing.Point(20, 425);
            this.pnlHistory.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlHistory.Name = "pnlHistory";
            this.pnlHistory.Size = new System.Drawing.Size(5, 200);
            this.pnlHistory.TabIndex = 23;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label8.ForeColor = System.Drawing.Color.SteelBlue;
            this.label8.Location = new System.Drawing.Point(44, 425);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 23);
            this.label8.TabIndex = 24;
            this.label8.Text = "履歴";
            // 
            // sDelete
            // 
            this.sDelete.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.sDelete.Location = new System.Drawing.Point(550, 422);
            this.sDelete.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.sDelete.Name = "sDelete";
            this.sDelete.Size = new System.Drawing.Size(31, 29);
            this.sDelete.TabIndex = 13;
            this.sDelete.TabStop = false;
            this.sDelete.Text = "×";
            this.sDelete.UseVisualStyleBackColor = true;
            // 
            // sUp
            // 
            this.sUp.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.sUp.Location = new System.Drawing.Point(472, 422);
            this.sUp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.sUp.Name = "sUp";
            this.sUp.Size = new System.Drawing.Size(31, 29);
            this.sUp.TabIndex = 11;
            this.sUp.TabStop = false;
            this.sUp.Text = "↑";
            this.sUp.UseVisualStyleBackColor = true;
            // 
            // sDown
            // 
            this.sDown.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.sDown.Location = new System.Drawing.Point(511, 422);
            this.sDown.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.sDown.Name = "sDown";
            this.sDown.Size = new System.Drawing.Size(31, 29);
            this.sDown.TabIndex = 12;
            this.sDown.TabStop = false;
            this.sDown.Text = "↓";
            this.sDown.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1043, 640);
            this.Controls.Add(this.sDown);
            this.Controls.Add(this.sUp);
            this.Controls.Add(this.sDelete);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.pnlHistory);
            this.Controls.Add(this.sHistory);
            this.Controls.Add(this.radDelete);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.chkRegex);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.radRename);
            this.Controls.Add(this.radCopy);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.chkSubDir);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.btnExec);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtAfter);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtBefore);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFolder);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximumSize = new System.Drawing.Size(1061, 1238);
            this.MinimumSize = new System.Drawing.Size(1061, 613);
            this.Name = "Form1";
            this.Text = "ファイル　コピー／名前変更／削除";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtFolder;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtBefore;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtAfter;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnExec;
		private System.Windows.Forms.TextBox txtResult;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox chkSubDir;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.RadioButton radCopy;
		private System.Windows.Forms.RadioButton radRename;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Button btnTest;
		private System.Windows.Forms.CheckBox chkRegex;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.RadioButton radDelete;
		private System.Windows.Forms.ListBox sHistory;
		private System.Windows.Forms.Panel pnlHistory;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Button sDelete;
		private System.Windows.Forms.Button sUp;
		private System.Windows.Forms.Button sDown;
	}
}

