using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace XSpect.MetaTweet.Clients
{
    public partial class MainForm
        : Form
    {
        private MetaTweetClient _client = new MetaTweetClient();

        private String _textBoxText;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_KeyDown(Object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.G)
            {
                if (this.metaXLabel.Width > 0)
                {
                    this.metaXLabel.Width = 0;
                    this.miniBufferTextBox.Text = this._textBoxText;
                }
                this._textBoxText = String.Empty;
                e.SuppressKeyPress = false;
                e.Handled = true;
            }
            else if (e.Alt && e.KeyCode == Keys.X)
            {
                this._textBoxText = this.miniBufferTextBox.Text;
                this.miniBufferTextBox.Clear();
                this.metaXLabel.Width = 35;
                e.SuppressKeyPress = false;
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.Enter)
            {
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this._client.Connect();
            var s = this._client.Host.GetStorage("sqlite");
            var t = this._client.Host.GetInput("twitter") as TwitterApiInput;
            var o = s.GetPosts().Where(p => p.Activity.Account["IsRestricted"] == false.ToString()).Select(p => new {Id = p.Activity.Account["ScreenName"], p.Timestamp, p.Text, p.Source});
        }
    }
}
