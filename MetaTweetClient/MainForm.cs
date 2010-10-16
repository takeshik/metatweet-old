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
using XSpect.Collections;
using XSpect.Extension;
using XSpect.MetaTweet.Requesting;
using XSpect.Windows.Forms;
using XSpect.MetaTweet.Objects;
using T = System.Timers;

namespace XSpect.MetaTweet.Clients.Client
{
    public partial class MainForm : Form
    {
        private readonly T.Timer _timer;

        private HybridDictionary<String, String>.Tuple _selectedTuple;

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
                (sender_, e_) => (e_.PropertyName == "Item[]").Then(() => this.RefreshFilterList());
            this._selectedTuple = this.Client.Filters.Tuples[0];
            this.RefreshFilterList();
            this.configurationPropertyGrid.SelectedObject = this.Client.ConfigurationObject;
            this.messageToolStripStatusLabel.Text = "Connecting to MetaTweet server: " + this.Client.ConfigurationObject.ServerAddress;
        }

        private void RefreshFilterList()
        {
            this.filterListBox.Items.Clear();
            this.filterListBox.Items.AddRange(this.Client.Filters.Keys.ToArray());
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

        private Boolean TryRefreshListView()
        {
            String response = this.Client.TestConnection();
            if (response == null)
            {
                this.messageToolStripStatusLabel.Text = "Connection failed. Continue to connect...";
                return false;
            }
            else
            {
                this.messageToolStripStatusLabel.Text = "Connection succeeded: " + response;
                return this.RefreshListView(this.Client.FetchData());
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();
            new Thread(() => this.Client.Connect()).Let(t => t.Start()).Join();
            this.TryRefreshListView();
            this._timer.Start();
        }


        private void inputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData.ToKeyString() == "C-Return")
            {
                this.Client.Host.RequestManager.Execute<Object>(Request.Parse(
                    String.Format(this.Client.ConfigurationObject.PostingRequest, Request.Escape(this.inputTextBox.Text + " [MetaTweet]"))
                ));
                this.inputTextBox.Clear();
                e.SuppressKeyPress = true;
            }
        }

        private void inputTextBox_TextChanged(object sender, EventArgs e)
        {
            this.messageToolStripStatusLabel.Text = this.inputTextBox.Text.Length
                .If(i => i < 1, i => i + " character", i => i + " characters");
        }

        private void addFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Client.Filters.Add("Filter" + this.Client.Filters.Count, String.Empty);
        }

        private void removeFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Client.Filters.RemoveAt(this.filterListBox.SelectedIndex);
        }

        private void filterNameToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            // TODO: Implement filter renaming with key renaming method
        }

        private void filterListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.filterListBox.SelectedIndex >= 0)
            {
                this._selectedTuple = this.Client.Filters.Tuples[this.filterListBox.SelectedIndex];
                this.viewDataGridView.Rows.Clear();
                this.queryTextBox.Text = this._selectedTuple.Value;
                this.filterListBox.Focus();
            }
        }

        private void filtersContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            (this._selectedTuple.Index >= 0).Let(
                b => this.removeFilterToolStripMenuItem.Enabled = b,
                b => this.filterNameToolStripTextBox.Enabled = b,
                b => b.Then(() =>
                    this.filterNameToolStripTextBox.Text = this._selectedTuple.Key
                )
            );
        }

        private void queryTextBox_TextChanged(object sender, EventArgs e)
        {
            // TODO: Rewrite
            this.Client.Filters[this._selectedTuple.Key] = this.queryTextBox.Text;
        }

        private void filtersContextMenuStrip_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            // FIXME: Why RemoveAt couldn't use?
            this.Client.Filters.Remove(this._selectedTuple.Key);
            this.Client.Filters.Insert(this._selectedTuple.Index, this.filterNameToolStripTextBox.Text, this.queryTextBox.Text);
            this._selectedTuple = this.Client.Filters.Tuples[this._selectedTuple.Index];
        }
    }
}
