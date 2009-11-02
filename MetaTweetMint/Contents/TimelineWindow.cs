using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Achiral;
using Achiral.Extension;
using WeifenLuo.WinFormsUI.Docking;
using XSpect.MetaTweet.Modules;
using XSpect.Extension;
using XSpect.MetaTweet.Objects;

namespace XSpect.MetaTweet.Clients.Mint.Contents
{
    public partial class TimelineWindow
        : DockContent
    {
        private readonly System.Timers.Timer _timer;

        public ClientCore Client
        {
            get;
            private set;
        }

        public TimelineWindow(ClientCore client)
        {
            this.Client = client;
            this._timer = new System.Timers.Timer();
            InitializeComponent();
            this.Initialize();
        }

        public void Initialize()
        {
            this._timer.Interval = 5000;
            StorageModule storage = (this.Client
                        .Host
                        .ModuleManager
                        .GetModule<StorageModule>("main"));
            this._timer.Elapsed += (sender, e) =>
                this.timelineListView.Invoke((MethodInvoker) (() =>
                {
                    this.timelineListView.BeginUpdate();
                    this.timelineListView.Items.Clear();
                    storage
                        .GetActivities(null, null, "Post", null)
                        .Select(a =>
                            new ListViewItem(Make.Array(
                                a.Timestamp.ToLocalTime().ToString("s").Replace("T", " "),
                                a.Account["ScreenName", DateTime.MaxValue].Value,
                                a.Value,
                                a.UserAgent //.Substring(a.UserAgent.IndexOf('>')).Do(s => s.Substring(s.IndexOf('<')))
                            ))
                        )
                        .ForEach(i => this.timelineListView.Items.Add(i));
                    this.timelineListView.EndUpdate();
                }));
        }

        private void TimelineWindow_Load(object sender, EventArgs e)
        {
            this.timelineListView.Items.Add(new ListViewItem(Make.Array("a", "b", "c", "d")));
            this._timer.Start();
        }
    }
}
