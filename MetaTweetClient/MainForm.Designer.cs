namespace XSpect.MetaTweet.Clients
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.horizontalSplicContainer = new System.Windows.Forms.SplitContainer();
            this.verticalSplitContainer = new System.Windows.Forms.SplitContainer();
            this.postsTabControl = new System.Windows.Forms.TabControl();
            this.postTabTemplate = new System.Windows.Forms.TabPage();
            this.timeLineListView = new System.Windows.Forms.ListView();
            this.atcolumnHeader = new System.Windows.Forms.ColumnHeader();
            this.byColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.bodyColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.fromColumnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.informationTabControl = new System.Windows.Forms.TabControl();
            this.propertyTabPage = new System.Windows.Forms.TabPage();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.logTagPage = new System.Windows.Forms.TabPage();
            this.userIdListBox = new System.Windows.Forms.ListBox();
            this.miniBufferTextBox = new System.Windows.Forms.TextBox();
            this.metaXLabel = new System.Windows.Forms.Label();
            this.modeLineLabel = new System.Windows.Forms.Label();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.horizontalSplicContainer.Panel1.SuspendLayout();
            this.horizontalSplicContainer.Panel2.SuspendLayout();
            this.horizontalSplicContainer.SuspendLayout();
            this.verticalSplitContainer.Panel1.SuspendLayout();
            this.verticalSplitContainer.Panel2.SuspendLayout();
            this.verticalSplitContainer.SuspendLayout();
            this.postsTabControl.SuspendLayout();
            this.postTabTemplate.SuspendLayout();
            this.informationTabControl.SuspendLayout();
            this.propertyTabPage.SuspendLayout();
            this.logTagPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.ContentPanel
            // 
            this.toolStripContainer.ContentPanel.Controls.Add(this.horizontalSplicContainer);
            resources.ApplyResources(this.toolStripContainer.ContentPanel, "toolStripContainer.ContentPanel");
            resources.ApplyResources(this.toolStripContainer, "toolStripContainer");
            this.toolStripContainer.Name = "toolStripContainer";
            // 
            // horizontalSplicContainer
            // 
            resources.ApplyResources(this.horizontalSplicContainer, "horizontalSplicContainer");
            this.horizontalSplicContainer.Name = "horizontalSplicContainer";
            // 
            // horizontalSplicContainer.Panel1
            // 
            this.horizontalSplicContainer.Panel1.Controls.Add(this.verticalSplitContainer);
            // 
            // horizontalSplicContainer.Panel2
            // 
            this.horizontalSplicContainer.Panel2.Controls.Add(this.miniBufferTextBox);
            this.horizontalSplicContainer.Panel2.Controls.Add(this.metaXLabel);
            this.horizontalSplicContainer.Panel2.Controls.Add(this.modeLineLabel);
            // 
            // verticalSplitContainer
            // 
            resources.ApplyResources(this.verticalSplitContainer, "verticalSplitContainer");
            this.verticalSplitContainer.Name = "verticalSplitContainer";
            // 
            // verticalSplitContainer.Panel1
            // 
            this.verticalSplitContainer.Panel1.Controls.Add(this.postsTabControl);
            // 
            // verticalSplitContainer.Panel2
            // 
            this.verticalSplitContainer.Panel2.Controls.Add(this.informationTabControl);
            // 
            // postsTabControl
            // 
            resources.ApplyResources(this.postsTabControl, "postsTabControl");
            this.postsTabControl.Controls.Add(this.postTabTemplate);
            this.postsTabControl.HotTrack = true;
            this.postsTabControl.Name = "postsTabControl";
            this.postsTabControl.SelectedIndex = 0;
            // 
            // postTabTemplate
            // 
            this.postTabTemplate.Controls.Add(this.timeLineListView);
            resources.ApplyResources(this.postTabTemplate, "postTabTemplate");
            this.postTabTemplate.Name = "postTabTemplate";
            this.postTabTemplate.UseVisualStyleBackColor = true;
            // 
            // timeLineListView
            // 
            this.timeLineListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.timeLineListView.CheckBoxes = true;
            this.timeLineListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.atcolumnHeader,
            this.byColumnHeader,
            this.bodyColumnHeader,
            this.fromColumnHeader1});
            resources.ApplyResources(this.timeLineListView, "timeLineListView");
            this.timeLineListView.FullRowSelect = true;
            this.timeLineListView.GridLines = true;
            this.timeLineListView.Name = "timeLineListView";
            this.timeLineListView.Sorting = System.Windows.Forms.SortOrder.Descending;
            this.timeLineListView.UseCompatibleStateImageBehavior = false;
            this.timeLineListView.View = System.Windows.Forms.View.Details;
            this.timeLineListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.timeLineListView_MouseClick);
            // 
            // atcolumnHeader
            // 
            resources.ApplyResources(this.atcolumnHeader, "atcolumnHeader");
            // 
            // byColumnHeader
            // 
            resources.ApplyResources(this.byColumnHeader, "byColumnHeader");
            // 
            // bodyColumnHeader
            // 
            resources.ApplyResources(this.bodyColumnHeader, "bodyColumnHeader");
            // 
            // fromColumnHeader1
            // 
            resources.ApplyResources(this.fromColumnHeader1, "fromColumnHeader1");
            // 
            // informationTabControl
            // 
            resources.ApplyResources(this.informationTabControl, "informationTabControl");
            this.informationTabControl.Controls.Add(this.propertyTabPage);
            this.informationTabControl.Controls.Add(this.logTagPage);
            this.informationTabControl.HotTrack = true;
            this.informationTabControl.Name = "informationTabControl";
            this.informationTabControl.SelectedIndex = 0;
            // 
            // propertyTabPage
            // 
            this.propertyTabPage.Controls.Add(this.propertyGrid);
            resources.ApplyResources(this.propertyTabPage, "propertyTabPage");
            this.propertyTabPage.Name = "propertyTabPage";
            this.propertyTabPage.UseVisualStyleBackColor = true;
            // 
            // propertyGrid
            // 
            resources.ApplyResources(this.propertyGrid, "propertyGrid");
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            // 
            // logTagPage
            // 
            this.logTagPage.Controls.Add(this.userIdListBox);
            resources.ApplyResources(this.logTagPage, "logTagPage");
            this.logTagPage.Name = "logTagPage";
            this.logTagPage.UseVisualStyleBackColor = true;
            // 
            // userIdListBox
            // 
            resources.ApplyResources(this.userIdListBox, "userIdListBox");
            this.userIdListBox.FormattingEnabled = true;
            this.userIdListBox.Name = "userIdListBox";
            // 
            // miniBufferTextBox
            // 
            resources.ApplyResources(this.miniBufferTextBox, "miniBufferTextBox");
            this.miniBufferTextBox.Name = "miniBufferTextBox";
            this.miniBufferTextBox.TextChanged += new System.EventHandler(this.miniBufferTextBox_TextChanged);
            // 
            // metaXLabel
            // 
            resources.ApplyResources(this.metaXLabel, "metaXLabel");
            this.metaXLabel.Name = "metaXLabel";
            // 
            // modeLineLabel
            // 
            this.modeLineLabel.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.modeLineLabel, "modeLineLabel");
            this.modeLineLabel.ForeColor = System.Drawing.Color.White;
            this.modeLineLabel.Name = "modeLineLabel";
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStripContainer);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            this.horizontalSplicContainer.Panel1.ResumeLayout(false);
            this.horizontalSplicContainer.Panel2.ResumeLayout(false);
            this.horizontalSplicContainer.Panel2.PerformLayout();
            this.horizontalSplicContainer.ResumeLayout(false);
            this.verticalSplitContainer.Panel1.ResumeLayout(false);
            this.verticalSplitContainer.Panel2.ResumeLayout(false);
            this.verticalSplitContainer.ResumeLayout(false);
            this.postsTabControl.ResumeLayout(false);
            this.postTabTemplate.ResumeLayout(false);
            this.informationTabControl.ResumeLayout(false);
            this.propertyTabPage.ResumeLayout(false);
            this.logTagPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer;
        private System.Windows.Forms.SplitContainer horizontalSplicContainer;
        private System.Windows.Forms.SplitContainer verticalSplitContainer;
        private System.Windows.Forms.TabControl postsTabControl;
        private System.Windows.Forms.TabPage postTabTemplate;
        private System.Windows.Forms.TabControl informationTabControl;
        private System.Windows.Forms.Label modeLineLabel;
        private System.Windows.Forms.ListView timeLineListView;
        private System.Windows.Forms.TabPage propertyTabPage;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.TextBox miniBufferTextBox;
        private System.Windows.Forms.Label metaXLabel;
        private System.Windows.Forms.ColumnHeader byColumnHeader;
        private System.Windows.Forms.ColumnHeader bodyColumnHeader;
        private System.Windows.Forms.ColumnHeader fromColumnHeader1;
        private System.Windows.Forms.ColumnHeader atcolumnHeader;
        private System.Windows.Forms.TabPage logTagPage;
        private System.Windows.Forms.ListBox userIdListBox;
    }
}

