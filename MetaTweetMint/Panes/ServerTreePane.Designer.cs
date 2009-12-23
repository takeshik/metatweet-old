namespace XSpect.MetaTweet.Clients.Mint.Panes
{
    partial class ServerTreePane
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerTreePane));
            this.serversTreeToolStrip = new System.Windows.Forms.ToolStrip();
            this.connectButton = new System.Windows.Forms.ToolStripButton();
            this.serversTreeView = new System.Windows.Forms.TreeView();
            this.serversTreeToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // serversTreeToolStrip
            // 
            this.serversTreeToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectButton});
            resources.ApplyResources(this.serversTreeToolStrip, "serversTreeToolStrip");
            this.serversTreeToolStrip.Name = "serversTreeToolStrip";
            // 
            // connectButton
            // 
            resources.ApplyResources(this.connectButton, "connectButton");
            this.connectButton.Name = "connectButton";
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // serversTreeView
            // 
            resources.ApplyResources(this.serversTreeView, "serversTreeView");
            this.serversTreeView.Name = "serversTreeView";
            // 
            // ServerTreePane
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.serversTreeView);
            this.Controls.Add(this.serversTreeToolStrip);
            this.Name = "ServerTreePane";
            this.serversTreeToolStrip.ResumeLayout(false);
            this.serversTreeToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip serversTreeToolStrip;
        private System.Windows.Forms.TreeView serversTreeView;
        private System.Windows.Forms.ToolStripButton connectButton;
    }
}