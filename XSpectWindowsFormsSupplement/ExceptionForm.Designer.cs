namespace XSpect.Windows.Forms
{
	partial class ExceptionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExceptionForm));
            this.errorPictureBox = new System.Windows.Forms.PictureBox();
            this.informationTextBox = new System.Windows.Forms.TextBox();
            this.exitButton = new System.Windows.Forms.Button();
            this.pleaseLabel = new System.Windows.Forms.Label();
            this.btsLinkLabel = new System.Windows.Forms.LinkLabel();
            this.debugButton = new System.Windows.Forms.Button();
            this.messageTextBox = new System.Windows.Forms.TextBox();
            this.exceptionTextBox = new System.Windows.Forms.TextBox();
            this.warningLabel = new System.Windows.Forms.Label();
            this.warningPictureBox = new System.Windows.Forms.PictureBox();
            this.continueButton = new System.Windows.Forms.Button();
            this.abortButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize) (this.errorPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.warningPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // errorPictureBox
            // 
            this.errorPictureBox.AccessibleDescription = null;
            this.errorPictureBox.AccessibleName = null;
            resources.ApplyResources(this.errorPictureBox, "errorPictureBox");
            this.errorPictureBox.BackgroundImage = null;
            this.errorPictureBox.Font = null;
            this.errorPictureBox.Image = global::XSpect.Windows.Forms.Properties.Resources.Error;
            this.errorPictureBox.ImageLocation = null;
            this.errorPictureBox.Name = "errorPictureBox";
            this.errorPictureBox.TabStop = false;
            // 
            // informationTextBox
            // 
            this.informationTextBox.AccessibleDescription = null;
            this.informationTextBox.AccessibleName = null;
            resources.ApplyResources(this.informationTextBox, "informationTextBox");
            this.informationTextBox.BackgroundImage = null;
            this.informationTextBox.Font = null;
            this.informationTextBox.Name = "informationTextBox";
            this.informationTextBox.ReadOnly = true;
            // 
            // exitButton
            // 
            this.exitButton.AccessibleDescription = null;
            this.exitButton.AccessibleName = null;
            resources.ApplyResources(this.exitButton, "exitButton");
            this.exitButton.BackgroundImage = null;
            this.exitButton.Font = null;
            this.exitButton.Name = "exitButton";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // pleaseLabel
            // 
            this.pleaseLabel.AccessibleDescription = null;
            this.pleaseLabel.AccessibleName = null;
            resources.ApplyResources(this.pleaseLabel, "pleaseLabel");
            this.pleaseLabel.Font = null;
            this.pleaseLabel.Name = "pleaseLabel";
            // 
            // btsLinkLabel
            // 
            this.btsLinkLabel.AccessibleDescription = null;
            this.btsLinkLabel.AccessibleName = null;
            resources.ApplyResources(this.btsLinkLabel, "btsLinkLabel");
            this.btsLinkLabel.Font = null;
            this.btsLinkLabel.Name = "btsLinkLabel";
            this.btsLinkLabel.TabStop = true;
            this.btsLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btsLinkLabel_LinkClicked);
            // 
            // debugButton
            // 
            this.debugButton.AccessibleDescription = null;
            this.debugButton.AccessibleName = null;
            resources.ApplyResources(this.debugButton, "debugButton");
            this.debugButton.BackgroundImage = null;
            this.debugButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.debugButton.Font = null;
            this.debugButton.Name = "debugButton";
            this.debugButton.UseVisualStyleBackColor = true;
            this.debugButton.Click += new System.EventHandler(this.debugButton_Click);
            // 
            // messageTextBox
            // 
            this.messageTextBox.AccessibleDescription = null;
            this.messageTextBox.AccessibleName = null;
            resources.ApplyResources(this.messageTextBox, "messageTextBox");
            this.messageTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.messageTextBox.BackgroundImage = null;
            this.messageTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.ReadOnly = true;
            // 
            // exceptionTextBox
            // 
            this.exceptionTextBox.AccessibleDescription = null;
            this.exceptionTextBox.AccessibleName = null;
            resources.ApplyResources(this.exceptionTextBox, "exceptionTextBox");
            this.exceptionTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.exceptionTextBox.BackgroundImage = null;
            this.exceptionTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.exceptionTextBox.Name = "exceptionTextBox";
            this.exceptionTextBox.ReadOnly = true;
            // 
            // warningLabel
            // 
            this.warningLabel.AccessibleDescription = null;
            this.warningLabel.AccessibleName = null;
            resources.ApplyResources(this.warningLabel, "warningLabel");
            this.warningLabel.ForeColor = System.Drawing.Color.Red;
            this.warningLabel.Name = "warningLabel";
            // 
            // warningPictureBox
            // 
            this.warningPictureBox.AccessibleDescription = null;
            this.warningPictureBox.AccessibleName = null;
            resources.ApplyResources(this.warningPictureBox, "warningPictureBox");
            this.warningPictureBox.BackgroundImage = null;
            this.warningPictureBox.Font = null;
            this.warningPictureBox.Image = global::XSpect.Windows.Forms.Properties.Resources.Warning;
            this.warningPictureBox.ImageLocation = null;
            this.warningPictureBox.Name = "warningPictureBox";
            this.warningPictureBox.TabStop = false;
            // 
            // continueButton
            // 
            this.continueButton.AccessibleDescription = null;
            this.continueButton.AccessibleName = null;
            resources.ApplyResources(this.continueButton, "continueButton");
            this.continueButton.BackgroundImage = null;
            this.continueButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.continueButton.Font = null;
            this.continueButton.Name = "continueButton";
            this.continueButton.UseVisualStyleBackColor = true;
            this.continueButton.Click += new System.EventHandler(this.continueButton_Click);
            // 
            // abortButton
            // 
            this.abortButton.AccessibleDescription = null;
            this.abortButton.AccessibleName = null;
            resources.ApplyResources(this.abortButton, "abortButton");
            this.abortButton.BackgroundImage = null;
            this.abortButton.Font = null;
            this.abortButton.Name = "abortButton";
            this.abortButton.UseVisualStyleBackColor = true;
            this.abortButton.Click += new System.EventHandler(this.abortButton_Click);
            // 
            // ExceptionForm
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.abortButton);
            this.Controls.Add(this.continueButton);
            this.Controls.Add(this.exceptionTextBox);
            this.Controls.Add(this.messageTextBox);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.informationTextBox);
            this.Controls.Add(this.pleaseLabel);
            this.Controls.Add(this.errorPictureBox);
            this.Controls.Add(this.warningPictureBox);
            this.Controls.Add(this.btsLinkLabel);
            this.Controls.Add(this.debugButton);
            this.Controls.Add(this.warningLabel);
            this.Font = null;
            this.Name = "ExceptionForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ExceptionForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize) (this.errorPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.warningPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox errorPictureBox;
        private System.Windows.Forms.TextBox informationTextBox;
		private System.Windows.Forms.Button exitButton;
		private System.Windows.Forms.Label pleaseLabel;
		private System.Windows.Forms.LinkLabel btsLinkLabel;
		private System.Windows.Forms.Button debugButton;
		private System.Windows.Forms.TextBox messageTextBox;
		private System.Windows.Forms.TextBox exceptionTextBox;
		private System.Windows.Forms.Label warningLabel;
		private System.Windows.Forms.PictureBox warningPictureBox;
        private System.Windows.Forms.Button continueButton;
        private System.Windows.Forms.Button abortButton;
	}
}