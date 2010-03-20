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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.storedTabPage = new System.Windows.Forms.TabPage();
            this.filterViewSplitContainer = new System.Windows.Forms.SplitContainer();
            this.filterListBox = new System.Windows.Forms.ListBox();
            this.queryViewSplitContainer = new System.Windows.Forms.SplitContainer();
            this.queryTextBox = new System.Windows.Forms.TextBox();
            this.viewDataGridView = new System.Windows.Forms.DataGridView();
            this.timestampDataGridViewRow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.screenNameDataGridViewRow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.categoryDataGridViewRow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueDataGridViewRow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewRow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userAgentDataGridViewRow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.immediateTabPage = new System.Windows.Forms.TabPage();
            this.immediateViewTabControl = new System.Windows.Forms.TabControl();
            this.rawTabPage = new System.Windows.Forms.TabPage();
            this.immediateWebBrowser = new System.Windows.Forms.WebBrowser();
            this.tableTabPage = new System.Windows.Forms.TabPage();
            this.immediateViewDataGridView = new System.Windows.Forms.DataGridView();
            this.requestTextBox = new System.Windows.Forms.TextBox();
            this.configurationTabPage = new System.Windows.Forms.TabPage();
            this.configurationPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.inputTextBox = new System.Windows.Forms.TextBox();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.horizontalSplitContainer.Panel1.SuspendLayout();
            this.horizontalSplitContainer.Panel2.SuspendLayout();
            this.horizontalSplitContainer.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.storedTabPage.SuspendLayout();
            this.filterViewSplitContainer.Panel1.SuspendLayout();
            this.filterViewSplitContainer.Panel2.SuspendLayout();
            this.filterViewSplitContainer.SuspendLayout();
            this.queryViewSplitContainer.Panel1.SuspendLayout();
            this.queryViewSplitContainer.Panel2.SuspendLayout();
            this.queryViewSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.viewDataGridView)).BeginInit();
            this.immediateTabPage.SuspendLayout();
            this.immediateViewTabControl.SuspendLayout();
            this.rawTabPage.SuspendLayout();
            this.tableTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.immediateViewDataGridView)).BeginInit();
            this.configurationTabPage.SuspendLayout();
            this.mainMenuStrip.SuspendLayout();
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
            this.storedTabPage.Controls.Add(this.filterViewSplitContainer);
            resources.ApplyResources(this.storedTabPage, "storedTabPage");
            this.storedTabPage.Name = "storedTabPage";
            this.storedTabPage.UseVisualStyleBackColor = true;
            // 
            // filterViewSplitContainer
            // 
            resources.ApplyResources(this.filterViewSplitContainer, "filterViewSplitContainer");
            this.filterViewSplitContainer.Name = "filterViewSplitContainer";
            // 
            // filterViewSplitContainer.Panel1
            // 
            this.filterViewSplitContainer.Panel1.Controls.Add(this.filterListBox);
            // 
            // filterViewSplitContainer.Panel2
            // 
            this.filterViewSplitContainer.Panel2.Controls.Add(this.queryViewSplitContainer);
            // 
            // filterListBox
            // 
            resources.ApplyResources(this.filterListBox, "filterListBox");
            this.filterListBox.FormattingEnabled = true;
            this.filterListBox.Name = "filterListBox";
            // 
            // queryViewSplitContainer
            // 
            resources.ApplyResources(this.queryViewSplitContainer, "queryViewSplitContainer");
            this.queryViewSplitContainer.Name = "queryViewSplitContainer";
            // 
            // queryViewSplitContainer.Panel1
            // 
            this.queryViewSplitContainer.Panel1.Controls.Add(this.queryTextBox);
            // 
            // queryViewSplitContainer.Panel2
            // 
            this.queryViewSplitContainer.Panel2.Controls.Add(this.viewDataGridView);
            // 
            // queryTextBox
            // 
            resources.ApplyResources(this.queryTextBox, "queryTextBox");
            this.queryTextBox.Name = "queryTextBox";
            // 
            // viewDataGridView
            // 
            this.viewDataGridView.AllowUserToAddRows = false;
            this.viewDataGridView.AllowUserToDeleteRows = false;
            this.viewDataGridView.AllowUserToOrderColumns = true;
            this.viewDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.viewDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.timestampDataGridViewRow,
            this.screenNameDataGridViewRow,
            this.categoryDataGridViewRow,
            this.valueDataGridViewRow,
            this.nameDataGridViewRow,
            this.userAgentDataGridViewRow});
            resources.ApplyResources(this.viewDataGridView, "viewDataGridView");
            this.viewDataGridView.Name = "viewDataGridView";
            this.viewDataGridView.ReadOnly = true;
            this.viewDataGridView.RowHeadersVisible = false;
            this.viewDataGridView.RowTemplate.Height = 21;
            // 
            // timestampDataGridViewRow
            // 
            resources.ApplyResources(this.timestampDataGridViewRow, "timestampDataGridViewRow");
            this.timestampDataGridViewRow.Name = "timestampDataGridViewRow";
            this.timestampDataGridViewRow.ReadOnly = true;
            // 
            // screenNameDataGridViewRow
            // 
            resources.ApplyResources(this.screenNameDataGridViewRow, "screenNameDataGridViewRow");
            this.screenNameDataGridViewRow.Name = "screenNameDataGridViewRow";
            this.screenNameDataGridViewRow.ReadOnly = true;
            // 
            // categoryDataGridViewRow
            // 
            resources.ApplyResources(this.categoryDataGridViewRow, "categoryDataGridViewRow");
            this.categoryDataGridViewRow.Name = "categoryDataGridViewRow";
            this.categoryDataGridViewRow.ReadOnly = true;
            // 
            // valueDataGridViewRow
            // 
            resources.ApplyResources(this.valueDataGridViewRow, "valueDataGridViewRow");
            this.valueDataGridViewRow.Name = "valueDataGridViewRow";
            this.valueDataGridViewRow.ReadOnly = true;
            // 
            // nameDataGridViewRow
            // 
            resources.ApplyResources(this.nameDataGridViewRow, "nameDataGridViewRow");
            this.nameDataGridViewRow.Name = "nameDataGridViewRow";
            this.nameDataGridViewRow.ReadOnly = true;
            // 
            // userAgentDataGridViewRow
            // 
            resources.ApplyResources(this.userAgentDataGridViewRow, "userAgentDataGridViewRow");
            this.userAgentDataGridViewRow.Name = "userAgentDataGridViewRow";
            this.userAgentDataGridViewRow.ReadOnly = true;
            // 
            // immediateTabPage
            // 
            this.immediateTabPage.Controls.Add(this.immediateViewTabControl);
            this.immediateTabPage.Controls.Add(this.requestTextBox);
            resources.ApplyResources(this.immediateTabPage, "immediateTabPage");
            this.immediateTabPage.Name = "immediateTabPage";
            this.immediateTabPage.UseVisualStyleBackColor = true;
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
            this.rawTabPage.Controls.Add(this.immediateWebBrowser);
            resources.ApplyResources(this.rawTabPage, "rawTabPage");
            this.rawTabPage.Name = "rawTabPage";
            this.rawTabPage.UseVisualStyleBackColor = true;
            // 
            // immediateWebBrowser
            // 
            resources.ApplyResources(this.immediateWebBrowser, "immediateWebBrowser");
            this.immediateWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.immediateWebBrowser.Name = "immediateWebBrowser";
            // 
            // tableTabPage
            // 
            this.tableTabPage.Controls.Add(this.immediateViewDataGridView);
            resources.ApplyResources(this.tableTabPage, "tableTabPage");
            this.tableTabPage.Name = "tableTabPage";
            this.tableTabPage.UseVisualStyleBackColor = true;
            // 
            // immediateViewDataGridView
            // 
            this.immediateViewDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.immediateViewDataGridView, "immediateViewDataGridView");
            this.immediateViewDataGridView.Name = "immediateViewDataGridView";
            this.immediateViewDataGridView.RowTemplate.Height = 21;
            // 
            // requestTextBox
            // 
            resources.ApplyResources(this.requestTextBox, "requestTextBox");
            this.requestTextBox.Name = "requestTextBox";
            // 
            // configurationTabPage
            // 
            this.configurationTabPage.Controls.Add(this.configurationPropertyGrid);
            resources.ApplyResources(this.configurationTabPage, "configurationTabPage");
            this.configurationTabPage.Name = "configurationTabPage";
            this.configurationTabPage.UseVisualStyleBackColor = true;
            // 
            // configurationPropertyGrid
            // 
            resources.ApplyResources(this.configurationPropertyGrid, "configurationPropertyGrid");
            this.configurationPropertyGrid.Name = "configurationPropertyGrid";
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
            this.tabControl.ResumeLayout(false);
            this.storedTabPage.ResumeLayout(false);
            this.filterViewSplitContainer.Panel1.ResumeLayout(false);
            this.filterViewSplitContainer.Panel2.ResumeLayout(false);
            this.filterViewSplitContainer.ResumeLayout(false);
            this.queryViewSplitContainer.Panel1.ResumeLayout(false);
            this.queryViewSplitContainer.Panel1.PerformLayout();
            this.queryViewSplitContainer.Panel2.ResumeLayout(false);
            this.queryViewSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.viewDataGridView)).EndInit();
            this.immediateTabPage.ResumeLayout(false);
            this.immediateTabPage.PerformLayout();
            this.immediateViewTabControl.ResumeLayout(false);
            this.rawTabPage.ResumeLayout(false);
            this.tableTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.immediateViewDataGridView)).EndInit();
            this.configurationTabPage.ResumeLayout(false);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
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
        private System.Windows.Forms.SplitContainer filterViewSplitContainer;
        private System.Windows.Forms.ListBox filterListBox;
        private System.Windows.Forms.WebBrowser immediateWebBrowser;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.PropertyGrid configurationPropertyGrid;
        private System.Windows.Forms.DataGridView immediateViewDataGridView;
        private System.Windows.Forms.SplitContainer queryViewSplitContainer;
        private System.Windows.Forms.TextBox queryTextBox;
        private System.Windows.Forms.DataGridView viewDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn timestampDataGridViewRow;
        private System.Windows.Forms.DataGridViewTextBoxColumn screenNameDataGridViewRow;
        private System.Windows.Forms.DataGridViewTextBoxColumn categoryDataGridViewRow;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueDataGridViewRow;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewRow;
        private System.Windows.Forms.DataGridViewTextBoxColumn userAgentDataGridViewRow;

    }
}