namespace XSpect.MetaTweet.Clients.Mint.Panes
{
    partial class PropertyPane
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertyPane));
            this.contextComboBox = new System.Windows.Forms.ComboBox();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // contextComboBox
            // 
            resources.ApplyResources(this.contextComboBox, "contextComboBox");
            this.contextComboBox.FormattingEnabled = true;
            this.contextComboBox.Name = "contextComboBox";
            // 
            // propertyGrid
            // 
            resources.ApplyResources(this.propertyGrid, "propertyGrid");
            this.propertyGrid.Name = "propertyGrid";
            // 
            // PropertyPane
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.contextComboBox);
            this.Name = "PropertyPane";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox contextComboBox;
        private System.Windows.Forms.PropertyGrid propertyGrid;
    }
}