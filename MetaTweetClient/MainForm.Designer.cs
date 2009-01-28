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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.informationTabControl = new System.Windows.Forms.TabControl();
            this.modeLineLabel = new System.Windows.Forms.Label();
            this.propertyTabPage = new System.Windows.Forms.TabPage();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.listView1 = new System.Windows.Forms.ListView();
            this.metaXLabel = new System.Windows.Forms.Label();
            this.miniBufferTextBox = new System.Windows.Forms.TextBox();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.horizontalSplicContainer.Panel1.SuspendLayout();
            this.horizontalSplicContainer.Panel2.SuspendLayout();
            this.horizontalSplicContainer.SuspendLayout();
            this.verticalSplitContainer.Panel1.SuspendLayout();
            this.verticalSplitContainer.Panel2.SuspendLayout();
            this.verticalSplitContainer.SuspendLayout();
            this.postsTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.informationTabControl.SuspendLayout();
            this.propertyTabPage.SuspendLayout();
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
            this.postsTabControl.Controls.Add(this.tabPage1);
            this.postsTabControl.HotTrack = true;
            this.postsTabControl.Name = "postsTabControl";
            this.postsTabControl.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.listView1);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // informationTabControl
            // 
            resources.ApplyResources(this.informationTabControl, "informationTabControl");
            this.informationTabControl.Controls.Add(this.propertyTabPage);
            this.informationTabControl.HotTrack = true;
            this.informationTabControl.Name = "informationTabControl";
            this.informationTabControl.SelectedIndex = 0;
            // 
            // modeLineLabel
            // 
            this.modeLineLabel.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.modeLineLabel, "modeLineLabel");
            this.modeLineLabel.ForeColor = System.Drawing.Color.White;
            this.modeLineLabel.Name = "modeLineLabel";
            // 
            // propertyTabPage
            // 
            this.propertyTabPage.Controls.Add(this.propertyGrid1);
            resources.ApplyResources(this.propertyTabPage, "propertyTabPage");
            this.propertyTabPage.Name = "propertyTabPage";
            this.propertyTabPage.UseVisualStyleBackColor = true;
            // 
            // propertyGrid1
            // 
            resources.ApplyResources(this.propertyGrid1, "propertyGrid1");
            this.propertyGrid1.Name = "propertyGrid1";
            // 
            // listView1
            // 
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView1.CheckBoxes = true;
            resources.ApplyResources(this.listView1, "listView1");
            this.listView1.GridLines = true;
            this.listView1.Name = "listView1";
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // metaXLabel
            // 
            resources.ApplyResources(this.metaXLabel, "metaXLabel");
            this.metaXLabel.Name = "metaXLabel";
            // 
            // miniBufferTextBox
            // 
            resources.ApplyResources(this.miniBufferTextBox, "miniBufferTextBox");
            this.miniBufferTextBox.Name = "miniBufferTextBox";
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStripContainer);
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
            this.tabPage1.ResumeLayout(false);
            this.informationTabControl.ResumeLayout(false);
            this.propertyTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer;
        private System.Windows.Forms.SplitContainer horizontalSplicContainer;
        private System.Windows.Forms.SplitContainer verticalSplitContainer;
        private System.Windows.Forms.TabControl postsTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl informationTabControl;
        private System.Windows.Forms.Label modeLineLabel;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TabPage propertyTabPage;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.TextBox miniBufferTextBox;
        private System.Windows.Forms.Label metaXLabel;
    }
}

