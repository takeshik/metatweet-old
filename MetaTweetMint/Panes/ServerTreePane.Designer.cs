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
            this.serversTreeView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
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
            this.Name = "ServerTreePane";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView serversTreeView;
    }
}