namespace XSpect.MetaTweet.Clients.Mint.Contents
{
    partial class TimelineWindow
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
            this.timelineListView = new System.Windows.Forms.ListView();
            this.timestampColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.screenNameColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.bodyColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.sourceColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // timelineListView
            // 
            this.timelineListView.CheckBoxes = true;
            this.timelineListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.timestampColumnHeader,
            this.screenNameColumnHeader,
            this.bodyColumnHeader,
            this.sourceColumnHeader});
            this.timelineListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.timelineListView.GridLines = true;
            this.timelineListView.Location = new System.Drawing.Point(0, 0);
            this.timelineListView.Name = "timelineListView";
            this.timelineListView.ShowItemToolTips = true;
            this.timelineListView.Size = new System.Drawing.Size(632, 453);
            this.timelineListView.Sorting = System.Windows.Forms.SortOrder.Descending;
            this.timelineListView.TabIndex = 0;
            this.timelineListView.UseCompatibleStateImageBehavior = false;
            this.timelineListView.View = System.Windows.Forms.View.Details;
            // 
            // timestampColumnHeader
            // 
            this.timestampColumnHeader.Text = "At";
            this.timestampColumnHeader.Width = 85;
            // 
            // screenNameColumnHeader
            // 
            this.screenNameColumnHeader.Text = "By";
            this.screenNameColumnHeader.Width = 100;
            // 
            // bodyColumnHeader
            // 
            this.bodyColumnHeader.Text = "Body";
            this.bodyColumnHeader.Width = 360;
            // 
            // sourceColumnHeader
            // 
            this.sourceColumnHeader.Text = "From";
            this.sourceColumnHeader.Width = 80;
            // 
            // TimelineWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 453);
            this.Controls.Add(this.timelineListView);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (128)));
            this.Name = "TimelineWindow";
            this.Text = "TimelineWindow";
            this.Load += new System.EventHandler(this.TimelineWindow_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView timelineListView;
        private System.Windows.Forms.ColumnHeader timestampColumnHeader;
        private System.Windows.Forms.ColumnHeader screenNameColumnHeader;
        private System.Windows.Forms.ColumnHeader bodyColumnHeader;
        private System.Windows.Forms.ColumnHeader sourceColumnHeader;
    }
}