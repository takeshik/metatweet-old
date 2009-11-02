namespace XSpect.MetaTweet.Clients.Mint.Contents
{
    partial class ResultTreeWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResultTreeWindow));
            this.resultsTreeToolStrip = new System.Windows.Forms.ToolStrip();
            this.resultsTreeView = new System.Windows.Forms.TreeView();
            this.connectButton = new System.Windows.Forms.ToolStripButton();
            this.resultsTreeToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // resultsTreeToolStrip
            // 
            this.resultsTreeToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectButton});
            resources.ApplyResources(this.resultsTreeToolStrip, "resultsTreeToolStrip");
            this.resultsTreeToolStrip.Name = "resultsTreeToolStrip";
            // 
            // resultsTreeView
            // 
            resources.ApplyResources(this.resultsTreeView, "resultsTreeView");
            this.resultsTreeView.Name = "resultsTreeView";
            // 
            // connectButton
            // 
            resources.ApplyResources(this.connectButton, "connectButton");
            this.connectButton.Name = "connectButton";
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // ResultTreeWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.resultsTreeView);
            this.Controls.Add(this.resultsTreeToolStrip);
            this.Name = "ResultTreeWindow";
            this.resultsTreeToolStrip.ResumeLayout(false);
            this.resultsTreeToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip resultsTreeToolStrip;
        private System.Windows.Forms.TreeView resultsTreeView;
        private System.Windows.Forms.ToolStripButton connectButton;
    }
}