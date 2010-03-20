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
using XSpect.Extension;
using XSpect.MetaTweet.Objects;
using T = System.Timers;

namespace XSpect.MetaTweet.Clients.Client
{
    public partial class MainForm : Form
    {
        private readonly T.Timer _timer;

        public ClientCore Client
        {
            get;
            private set;
        }

        public MainForm(ClientCore client)
        {
            this.Client = client;
            InitializeComponent();
            this._timer = new System.Timers.Timer(this.Client.ConfigurationObject.RequestInterval)
                .Let(t => t.Elapsed += (sender, e) =>
                {
                    this._timer.Stop();
                    this.Invoke((MethodInvoker) (() => this.RefreshListView(this.Client.FetchData())));
                    this._timer.Start();
                });
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Client.Filters.PropertyChanged +=
                (sender_, e_) => (e_.PropertyName == "Item[]").Then(() => this.filterListBox.Items.Let(
                    _ => _.Clear(),
                    _ => _.AddRange(this.Client.Filters.Keys.ToArray())
                ));
            this.configurationPropertyGrid.SelectedObject = this.Client.ConfigurationObject;
            this.Client.Connect();
            this.RefreshListView(this.Client.FetchData());
            this._timer.Start();
        }

        private void RefreshListView(IEnumerable<Activity> addition)
        {
            // NOTE: Argument "addition" is currently ignored
            this.viewDataGridView.Rows.Clear();
            (this.queryTextBox.Text.IsNullOrEmpty()
                ? this.Client.DataCache
                : this.Client.ExecuteQuery(this.queryTextBox.Text)
            )
                .OrderBy(a => a)
                .Select(a => Make.Array(
                    a.Timestamp.ToLocalTime().ToString("s"),
                    a.Account["ScreenName"].Value,
                    a.Category,
                    a.Value,
                    a.Account["Name"].Value,
                    a.UserAgent
                ))
                .Let(_ => this.viewDataGridView.SuspendLayout())
                .ForEach(a => this.viewDataGridView.Rows.Add(a));
            this.viewDataGridView.ResumeLayout();
        }
    }
}
