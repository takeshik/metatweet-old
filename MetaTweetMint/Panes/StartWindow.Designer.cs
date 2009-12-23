namespace XSpect.MetaTweet.Clients.Mint.Panes
{
    partial class StartWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartWindow));
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.welcomeTextBox = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize) (this.logoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // logoPictureBox
            // 
            resources.ApplyResources(this.logoPictureBox, "logoPictureBox");
            this.logoPictureBox.Image = global::XSpect.MetaTweet.Clients.Mint.Properties.Resources.MetaTweetLogo;
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.TabStop = false;
            // 
            // welcomeTextBox
            // 
            resources.ApplyResources(this.welcomeTextBox, "welcomeTextBox");
            this.welcomeTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.welcomeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.welcomeTextBox.Name = "welcomeTextBox";
            this.welcomeTextBox.ReadOnly = true;
            this.welcomeTextBox.ShortcutsEnabled = false;
            // 
            // StartWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.welcomeTextBox);
            this.Controls.Add(this.logoPictureBox);
            this.Name = "StartWindow";
            this.Load += new System.EventHandler(this.StartWindow_Load);
            ((System.ComponentModel.ISupportInitialize) (this.logoPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.RichTextBox welcomeTextBox;
    }
}