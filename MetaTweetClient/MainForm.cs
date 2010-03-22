using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
                    if (this.Client.TestConnection() != null)
                    {
                        this.Invoke(Lambda.New(() => 
                        {
                            Boolean succeeded = this.RefreshListView(this.Client.FetchData());
                            if (this.tabControl.SelectedTab == this.storedTabPage)
                            {
                                this.messageToolStripStatusLabel.Text = succeeded
                                    ? "Querying was done (" + this.viewDataGridView.Rows.Count.If(i => i < 1, i => i + " object).", i => i + " objects).")
                                    : "Querying was failed.";
                            }
                        }));
                    }
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
            this.messageToolStripStatusLabel.Text = "Connecting to MetaTweet server: " + this.Client.ConfigurationObject.ServerAddress;
        }

        private Boolean RefreshListView(IEnumerable<Activity> addition)
        {
            // NOTE: Argument "addition" is currently ignored
            return (this.queryTextBox.Text.IsNullOrEmpty()
                ? this.Client.DataCache
                : this.Client.ExecuteQuery(this.queryTextBox.Text)
            ).If(
                l => l != null,
                l => l
                    .OrderByDescending(a => a)
                    .Select(a => Make.Array(
                        a.Timestamp.ToLocalTime().ToString("s"),
                        a.Account["ScreenName"].Value,
                        a.Category,
                        a.Value,
                        a.Account["Name"].Value,
                        a.UserAgent
                    ))
                    .Let(_ => this.viewDataGridView.Rows.Clear())
                    .Let(_ => this.viewDataGridView.SuspendLayout())
                    .Let(_ => _.ForEach(a => this.viewDataGridView.Rows.Add(a)))
                    .True(),
                l => false
            ).Let(
                _ => this.viewDataGridView.ResumeLayout(),
                _ => this.refreshToolStripStatusLabel.Text = "Last updated: " + DateTime.Now.ToString("s")
            );
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();
            new Thread(() => this.Client.Connect()).Let(t => t.Start()).Join();
            String response = this.Client.TestConnection();
            if (response == null)
            {
                this.messageToolStripStatusLabel.Text = "Connection failed. Continue to connect...";
            }
            else
            {
                this.messageToolStripStatusLabel.Text = "Connection succeeded: " + response;
                this.RefreshListView(this.Client.FetchData());
            }
            this._timer.Start();
        }
    }
}
