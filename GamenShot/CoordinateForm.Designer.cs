namespace GamenShot
{
    partial class CoordinateForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(128, 128);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.MouseEnter += new System.EventHandler(this.PictureBox_MouseEnter);
            this.pictureBox.MouseLeave += new System.EventHandler(this.PictureBox_MouseLeave);
            // 
            // CoordinateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(128, 128);
            this.Controls.Add(this.pictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(144, 167);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(144, 167);
            this.Name = "CoordinateForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "X:**** Y:****";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CoordinateForm_FormClosing);
            this.Load += new System.EventHandler(this.CoordinateForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CoordinateForm_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CoordinateForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CoordinateForm_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
    }
}