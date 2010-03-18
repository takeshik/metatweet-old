using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XSpect.Extension;

namespace XSpect.MetaTweet.Clients.Client
{
    public partial class MainForm : Form
    {
        public ClientCore Client
        {
            get;
            private set;
        }

        public MainForm(ClientCore client)
        {
            this.Client = client;
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Client.Filters.PropertyChanged +=
                (sender_, e_) => (e_.PropertyName == "Item[]").Then(() => this.filterListBox.Items.Let(
                    _ => _.Clear(),
                    _ => _.AddRange(this.Client.Filters.Keys.ToArray())
                ));
        }
    }
}
