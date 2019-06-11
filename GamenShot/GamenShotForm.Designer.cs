namespace GamenShot
{
    partial class GamenShotForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
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
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.generalTabPage = new System.Windows.Forms.TabPage();
            this.saveFolderPathButton = new System.Windows.Forms.Button();
            this.saveFolderPathTextBox = new System.Windows.Forms.TextBox();
            this.saveFolderPathLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.aboutTabPage = new System.Windows.Forms.TabPage();
            this.nameLabel = new System.Windows.Forms.Label();
            this.licenseLabel = new System.Windows.Forms.Label();
            this.licenseLinkLabel = new System.Windows.Forms.LinkLabel();
            this.nameValueLabel = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.versionValueLabel = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.generalTabPage.SuspendLayout();
            this.panel1.SuspendLayout();
            this.aboutTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(296, 12);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(377, 12);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "キャンセル";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.generalTabPage);
            this.tabControl.Controls.Add(this.aboutTabPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(464, 216);
            this.tabControl.TabIndex = 0;
            // 
            // generalTabPage
            // 
            this.generalTabPage.Controls.Add(this.saveFolderPathButton);
            this.generalTabPage.Controls.Add(this.saveFolderPathTextBox);
            this.generalTabPage.Controls.Add(this.saveFolderPathLabel);
            this.generalTabPage.Location = new System.Drawing.Point(4, 22);
            this.generalTabPage.Name = "generalTabPage";
            this.generalTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.generalTabPage.Size = new System.Drawing.Size(456, 190);
            this.generalTabPage.TabIndex = 0;
            this.generalTabPage.Text = "全般設定";
            this.generalTabPage.UseVisualStyleBackColor = true;
            // 
            // saveFolderPathButton
            // 
            this.saveFolderPathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveFolderPathButton.Location = new System.Drawing.Point(396, 11);
            this.saveFolderPathButton.Name = "saveFolderPathButton";
            this.saveFolderPathButton.Size = new System.Drawing.Size(48, 23);
            this.saveFolderPathButton.TabIndex = 2;
            this.saveFolderPathButton.Text = "参照";
            this.saveFolderPathButton.UseVisualStyleBackColor = true;
            this.saveFolderPathButton.Click += new System.EventHandler(this.SaveFolderPathButton_Click);
            // 
            // saveFolderPathTextBox
            // 
            this.saveFolderPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.saveFolderPathTextBox.Location = new System.Drawing.Point(86, 13);
            this.saveFolderPathTextBox.Name = "saveFolderPathTextBox";
            this.saveFolderPathTextBox.Size = new System.Drawing.Size(304, 19);
            this.saveFolderPathTextBox.TabIndex = 1;
            // 
            // saveFolderPathLabel
            // 
            this.saveFolderPathLabel.AutoSize = true;
            this.saveFolderPathLabel.Location = new System.Drawing.Point(16, 16);
            this.saveFolderPathLabel.Name = "saveFolderPathLabel";
            this.saveFolderPathLabel.Size = new System.Drawing.Size(64, 12);
            this.saveFolderPathLabel.TabIndex = 0;
            this.saveFolderPathLabel.Text = "保存フォルダ";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.okButton);
            this.panel1.Controls.Add(this.cancelButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 216);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(464, 45);
            this.panel1.TabIndex = 1;
            // 
            // aboutTabPage
            // 
            this.aboutTabPage.Controls.Add(this.versionValueLabel);
            this.aboutTabPage.Controls.Add(this.versionLabel);
            this.aboutTabPage.Controls.Add(this.nameValueLabel);
            this.aboutTabPage.Controls.Add(this.licenseLinkLabel);
            this.aboutTabPage.Controls.Add(this.licenseLabel);
            this.aboutTabPage.Controls.Add(this.nameLabel);
            this.aboutTabPage.Location = new System.Drawing.Point(4, 22);
            this.aboutTabPage.Name = "aboutTabPage";
            this.aboutTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.aboutTabPage.Size = new System.Drawing.Size(456, 190);
            this.aboutTabPage.TabIndex = 1;
            this.aboutTabPage.Text = "画面ショットについて";
            this.aboutTabPage.UseVisualStyleBackColor = true;
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(16, 16);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(86, 12);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "アプリケーション名";
            // 
            // licenseLabel
            // 
            this.licenseLabel.AutoSize = true;
            this.licenseLabel.Location = new System.Drawing.Point(16, 127);
            this.licenseLabel.Name = "licenseLabel";
            this.licenseLabel.Size = new System.Drawing.Size(315, 12);
            this.licenseLabel.TabIndex = 2;
            this.licenseLabel.Text = "画面ショットはオープンソースソフトウェアを使用して開発しています。";
            // 
            // licenseLinkLabel
            // 
            this.licenseLinkLabel.AutoSize = true;
            this.licenseLinkLabel.Location = new System.Drawing.Point(16, 150);
            this.licenseLinkLabel.Name = "licenseLinkLabel";
            this.licenseLinkLabel.Size = new System.Drawing.Size(239, 12);
            this.licenseLinkLabel.TabIndex = 3;
            this.licenseLinkLabel.TabStop = true;
            this.licenseLinkLabel.Text = "http://www.apache.org/licenses/LICENSE-2.0";
            // 
            // nameValueLabel
            // 
            this.nameValueLabel.AutoSize = true;
            this.nameValueLabel.Location = new System.Drawing.Point(108, 16);
            this.nameValueLabel.Name = "nameValueLabel";
            this.nameValueLabel.Size = new System.Drawing.Size(60, 12);
            this.nameValueLabel.TabIndex = 4;
            this.nameValueLabel.Text = "画面ショット";
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(16, 41);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(50, 12);
            this.versionLabel.TabIndex = 5;
            this.versionLabel.Text = "バージョン";
            // 
            // versionValueLabel
            // 
            this.versionValueLabel.AutoSize = true;
            this.versionValueLabel.Location = new System.Drawing.Point(108, 41);
            this.versionValueLabel.Name = "versionValueLabel";
            this.versionValueLabel.Size = new System.Drawing.Size(35, 12);
            this.versionValueLabel.TabIndex = 6;
            this.versionValueLabel.Text = "0.0.0.0";
            // 
            // GamenShotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 261);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GamenShotForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "画面ショット";
            this.tabControl.ResumeLayout(false);
            this.generalTabPage.ResumeLayout(false);
            this.generalTabPage.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.aboutTabPage.ResumeLayout(false);
            this.aboutTabPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage generalTabPage;
        private System.Windows.Forms.Button saveFolderPathButton;
        private System.Windows.Forms.TextBox saveFolderPathTextBox;
        private System.Windows.Forms.Label saveFolderPathLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabPage aboutTabPage;
        private System.Windows.Forms.LinkLabel licenseLinkLabel;
        private System.Windows.Forms.Label licenseLabel;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label versionValueLabel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label nameValueLabel;
    }
}

