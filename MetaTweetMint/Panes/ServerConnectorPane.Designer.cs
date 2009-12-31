namespace XSpect.MetaTweet.Clients.Mint.Panes
{
    partial class ServerConnectorPane
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerConnectorPane));
            this.tab = new System.Windows.Forms.TabControl();
            this.configurationPage = new System.Windows.Forms.TabPage();
            this.scriptPage = new System.Windows.Forms.TabPage();
            this.topPanel = new System.Windows.Forms.Panel();
            this.languageLabel = new System.Windows.Forms.Label();
            this.languageComboBox = new System.Windows.Forms.ComboBox();
            this.scriptTextBox = new Sgry.Azuki.Windows.AzukiControl();
            this.tab.SuspendLayout();
            this.scriptPage.SuspendLayout();
            this.topPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab
            // 
            this.tab.Controls.Add(this.configurationPage);
            this.tab.Controls.Add(this.scriptPage);
            resources.ApplyResources(this.tab, "tab");
            this.tab.Name = "tab";
            this.tab.SelectedIndex = 0;
            // 
            // configurationPage
            // 
            resources.ApplyResources(this.configurationPage, "configurationPage");
            this.configurationPage.Name = "configurationPage";
            this.configurationPage.UseVisualStyleBackColor = true;
            // 
            // scriptPage
            // 
            this.scriptPage.Controls.Add(this.scriptTextBox);
            this.scriptPage.Controls.Add(this.topPanel);
            resources.ApplyResources(this.scriptPage, "scriptPage");
            this.scriptPage.Name = "scriptPage";
            this.scriptPage.UseVisualStyleBackColor = true;
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.languageComboBox);
            this.topPanel.Controls.Add(this.languageLabel);
            resources.ApplyResources(this.topPanel, "topPanel");
            this.topPanel.Name = "topPanel";
            // 
            // languageLabel
            // 
            resources.ApplyResources(this.languageLabel, "languageLabel");
            this.languageLabel.Name = "languageLabel";
            // 
            // languageComboBox
            // 
            resources.ApplyResources(this.languageComboBox, "languageComboBox");
            this.languageComboBox.FormattingEnabled = true;
            this.languageComboBox.Name = "languageComboBox";
            // 
            // scriptTextBox
            // 
            this.scriptTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            resources.ApplyResources(this.scriptTextBox, "scriptTextBox");
            this.scriptTextBox.DrawingOption = ((Sgry.Azuki.DrawingOption) ((((((Sgry.Azuki.DrawingOption.DrawsFullWidthSpace | Sgry.Azuki.DrawingOption.DrawsTab)
                        | Sgry.Azuki.DrawingOption.DrawsEol)
                        | Sgry.Azuki.DrawingOption.HighlightCurrentLine)
                        | Sgry.Azuki.DrawingOption.ShowsLineNumber)
                        | Sgry.Azuki.DrawingOption.ShowsDirtBar)));
            this.scriptTextBox.FirstVisibleLine = 0;
            this.scriptTextBox.Name = "scriptTextBox";
            this.scriptTextBox.TabWidth = 8;
            this.scriptTextBox.ViewWidth = 4129;
            // 
            // ServerConnectorPane
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tab);
            this.Name = "ServerConnectorPane";
            this.tab.ResumeLayout(false);
            this.scriptPage.ResumeLayout(false);
            this.topPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tab;
        private System.Windows.Forms.TabPage configurationPage;
        private System.Windows.Forms.TabPage scriptPage;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.ComboBox languageComboBox;
        private System.Windows.Forms.Label languageLabel;
        private Sgry.Azuki.Windows.AzukiControl scriptTextBox;
    }
}