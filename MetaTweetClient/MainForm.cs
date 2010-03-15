using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
    }
}
