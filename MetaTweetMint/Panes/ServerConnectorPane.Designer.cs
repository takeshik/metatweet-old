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
            this.queryBar = new System.Windows.Forms.ComboBox();
            this.resultTab = new System.Windows.Forms.TabControl();
            this.logPage = new System.Windows.Forms.TabPage();
            this.logListView = new System.Windows.Forms.ListView();
            this.resultTab.SuspendLayout();
            this.logPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // queryBar
            // 
            resources.ApplyResources(this.queryBar, "queryBar");
            this.queryBar.FormattingEnabled = true;
            this.queryBar.Name = "queryBar";
            // 
            // resultTab
            // 
            resources.ApplyResources(this.resultTab, "resultTab");
            this.resultTab.Controls.Add(this.logPage);
            this.resultTab.Name = "resultTab";
            this.resultTab.SelectedIndex = 0;
            // 
            // logPage
            // 
            this.logPage.Controls.Add(this.logListView);
            resources.ApplyResources(this.logPage, "logPage");
            this.logPage.Name = "logPage";
            this.logPage.UseVisualStyleBackColor = true;
            // 
            // logListView
            // 
            resources.ApplyResources(this.logListView, "logListView");
            this.logListView.Name = "logListView";
            this.logListView.UseCompatibleStateImageBehavior = false;
            this.logListView.View = System.Windows.Forms.View.Details;
            // 
            // ServerConnectorPane
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.resultTab);
            this.Controls.Add(this.queryBar);
            this.Name = "ServerConnectorPane";
            this.resultTab.ResumeLayout(false);
            this.logPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox queryBar;
        private System.Windows.Forms.TabControl resultTab;
        private System.Windows.Forms.TabPage logPage;
        private System.Windows.Forms.ListView logListView;

    }
}