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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerTreePane));
            this.serversTreeView = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // serversTreeView
            // 
            resources.ApplyResources(this.serversTreeView, "serversTreeView");
            this.serversTreeView.ImageList = this.imageList;
            this.serversTreeView.Name = "serversTreeView";
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer) (resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "ServerConnector");
            this.imageList.Images.SetKeyName(1, "ObjectView");
            this.imageList.Images.SetKeyName(2, "ObjectFilter");
            // 
            // ServerTreePane
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.serversTreeView);
            this.Name = "ServerTreePane";
            this.Load += new System.EventHandler(this.ServerTreePane_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView serversTreeView;
        private System.Windows.Forms.ImageList imageList;
    }
}