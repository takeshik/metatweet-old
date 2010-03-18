namespace XSpect.MetaTweet.Clients.Client
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.horizontalSplitContainer = new System.Windows.Forms.SplitContainer();
            this.inputTextBox = new System.Windows.Forms.TextBox();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.storedTabPage = new System.Windows.Forms.TabPage();
            this.immediateTabPage = new System.Windows.Forms.TabPage();
            this.configurationTabPage = new System.Windows.Forms.TabPage();
            this.requestTextBox = new System.Windows.Forms.TextBox();
            this.immediateViewTabControl = new System.Windows.Forms.TabControl();
            this.rawTabPage = new System.Windows.Forms.TabPage();
            this.tableTabPage = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.filterListBox = new System.Windows.Forms.ListBox();
            this.expressionTextBox = new System.Windows.Forms.TextBox();
            this.storedListView = new System.Windows.Forms.ListView();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.immediateListView = new System.Windows.Forms.ListView();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.horizontalSplitContainer.Panel1.SuspendLayout();
            this.horizontalSplitContainer.Panel2.SuspendLayout();
            this.horizontalSplitContainer.SuspendLayout();
            this.mainMenuStrip.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.storedTabPage.SuspendLayout();
            this.immediateTabPage.SuspendLayout();
            this.immediateViewTabControl.SuspendLayout();
            this.rawTabPage.SuspendLayout();
            this.tableTabPage.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.BottomToolStripPanel
            // 
            this.toolStripContainer.BottomToolStripPanel.Controls.Add(this.statusStrip);
            // 
            // toolStripContainer.ContentPanel
            // 
            this.toolStripContainer.ContentPanel.Controls.Add(this.horizontalSplitContainer);
            resources.ApplyResources(this.toolStripContainer.ContentPanel, "toolStripContainer.ContentPanel");
            resources.ApplyResources(this.toolStripContainer, "toolStripContainer");
            this.toolStripContainer.Name = "toolStripContainer";
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.mainMenuStrip);
            // 
            // statusStrip
            // 
            resources.ApplyResources(this.statusStrip, "statusStrip");
            this.statusStrip.Name = "statusStrip";
            // 
            // horizontalSplitContainer
            // 
            resources.ApplyResources(this.horizontalSplitContainer, "horizontalSplitContainer");
            this.horizontalSplitContainer.Name = "horizontalSplitContainer";
            // 
            // horizontalSplitContainer.Panel1
            // 
            this.horizontalSplitContainer.Panel1.Controls.Add(this.tabControl);
            // 
            // horizontalSplitContainer.Panel2
            // 
            this.horizontalSplitContainer.Panel2.Controls.Add(this.inputTextBox);
            // 
            // inputTextBox
            // 
            resources.ApplyResources(this.inputTextBox, "inputTextBox");
            this.inputTextBox.Name = "inputTextBox";
            // 
            // mainMenuStrip
            // 
            resources.ApplyResources(this.mainMenuStrip, "mainMenuStrip");
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.mainMenuStrip.Name = "mainMenuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // tabControl
            // 
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Controls.Add(this.storedTabPage);
            this.tabControl.Controls.Add(this.immediateTabPage);
            this.tabControl.Controls.Add(this.configurationTabPage);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            // 
            // storedTabPage
            // 
            this.storedTabPage.Controls.Add(this.splitContainer1);
            resources.ApplyResources(this.storedTabPage, "storedTabPage");
            this.storedTabPage.Name = "storedTabPage";
            this.storedTabPage.UseVisualStyleBackColor = true;
            // 
            // immediateTabPage
            // 
            this.immediateTabPage.Controls.Add(this.immediateViewTabControl);
            this.immediateTabPage.Controls.Add(this.requestTextBox);
            resources.ApplyResources(this.immediateTabPage, "immediateTabPage");
            this.immediateTabPage.Name = "immediateTabPage";
            this.immediateTabPage.UseVisualStyleBackColor = true;
            // 
            // configurationTabPage
            // 
            resources.ApplyResources(this.configurationTabPage, "configurationTabPage");
            this.configurationTabPage.Name = "configurationTabPage";
            this.configurationTabPage.UseVisualStyleBackColor = true;
            // 
            // requestTextBox
            // 
            resources.ApplyResources(this.requestTextBox, "requestTextBox");
            this.requestTextBox.Name = "requestTextBox";
            // 
            // immediateViewTabControl
            // 
            resources.ApplyResources(this.immediateViewTabControl, "immediateViewTabControl");
            this.immediateViewTabControl.Controls.Add(this.rawTabPage);
            this.immediateViewTabControl.Controls.Add(this.tableTabPage);
            this.immediateViewTabControl.Multiline = true;
            this.immediateViewTabControl.Name = "immediateViewTabControl";
            this.immediateViewTabControl.SelectedIndex = 0;
            // 
            // rawTabPage
            // 
            this.rawTabPage.Controls.Add(this.webBrowser);
            resources.ApplyResources(this.rawTabPage, "rawTabPage");
            this.rawTabPage.Name = "rawTabPage";
            this.rawTabPage.UseVisualStyleBackColor = true;
            // 
            // tableTabPage
            // 
            this.tableTabPage.Controls.Add(this.immediateListView);
            resources.ApplyResources(this.tableTabPage, "tableTabPage");
            this.tableTabPage.Name = "tableTabPage";
            this.tableTabPage.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.filterListBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.storedListView);
            this.splitContainer1.Panel2.Controls.Add(this.expressionTextBox);
            // 
            // filterListBox
            // 
            resources.ApplyResources(this.filterListBox, "filterListBox");
            this.filterListBox.FormattingEnabled = true;
            this.filterListBox.Name = "filterListBox";
            // 
            // expressionTextBox
            // 
            resources.ApplyResources(this.expressionTextBox, "expressionTextBox");
            this.expressionTextBox.Name = "expressionTextBox";
            // 
            // storedListView
            // 
            resources.ApplyResources(this.storedListView, "storedListView");
            this.storedListView.Name = "storedListView";
            this.storedListView.UseCompatibleStateImageBehavior = false;
            // 
            // webBrowser
            // 
            resources.ApplyResources(this.webBrowser, "webBrowser");
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            // 
            // immediateListView
            // 
            resources.ApplyResources(this.immediateListView, "immediateListView");
            this.immediateListView.Name = "immediateListView";
            this.immediateListView.UseCompatibleStateImageBehavior = false;
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStripContainer);
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.PerformLayout();
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            this.horizontalSplitContainer.Panel1.ResumeLayout(false);
            this.horizontalSplitContainer.Panel2.ResumeLayout(false);
            this.horizontalSplitContainer.Panel2.PerformLayout();
            this.horizontalSplitContainer.ResumeLayout(false);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.storedTabPage.ResumeLayout(false);
            this.immediateTabPage.ResumeLayout(false);
            this.immediateTabPage.PerformLayout();
            this.immediateViewTabControl.ResumeLayout(false);
            this.rawTabPage.ResumeLayout(false);
            this.tableTabPage.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.SplitContainer horizontalSplitContainer;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.TextBox inputTextBox;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage storedTabPage;
        private System.Windows.Forms.TabPage immediateTabPage;
        private System.Windows.Forms.TabPage configurationTabPage;
        private System.Windows.Forms.TabControl immediateViewTabControl;
        private System.Windows.Forms.TabPage rawTabPage;
        private System.Windows.Forms.TabPage tableTabPage;
        private System.Windows.Forms.TextBox requestTextBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox filterListBox;
        private System.Windows.Forms.ListView storedListView;
        private System.Windows.Forms.TextBox expressionTextBox;
        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.ListView immediateListView;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;

    }
}