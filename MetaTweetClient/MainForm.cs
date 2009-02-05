// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetClient
 *   Bandled GUI client for MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetClient.
 * 
 * This program is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation; either version 3 of the License, or (at your
 * option) any later version.
 * 
 * This program is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
 * or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public
 * License for more details. 
 * 
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>,
 * or write to the Free Software Foundation, Inc., 51 Franklin Street,
 * Fifth Floor, Boston, MA 02110-1301, USA.
 */

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
using XSpect.MetaTweet.ObjectModel;
using XSpect.MetaTweet.Modules;

namespace XSpect.MetaTweet.Clients
{
    public partial class MainForm
        : Form
    {
        private DateTime _since = DateTime.MinValue;

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
                if (this.metaXLabel.Width == 0)
                {
                    if (this.miniBufferTextBox.Text != "")
                    {
                        this._client.Update(this.miniBufferTextBox.Text);
                        this.miniBufferTextBox.Clear();
                    }
                    List<Post> posts = this._client.GetFriendsTimeLine(_since);
                    this.timeLineListView.BeginUpdate();
                    foreach (Post post in posts)
                    {
                        ListViewItem item = new ListViewItem(new String[]
                        {
                            post.Timestamp.ToLocalTime().ToString("s").Replace("T", " "),
                            post.Activity.Account["ScreenName"],
                            post.Text,
                            post.Source,
                        });
                        item.Tag = post;
                        this.timeLineListView.Items.Add(item);
                    }
                    this.timeLineListView.EndUpdate();
                    this._since = DateTime.Now.ToUniversalTime();
                }
                else
                {
                }
                var s = this._client.Host.ModuleManager.GetModule<StorageModule>("sqlite");
                this.modeLineLabel.Text = String.Format("Acc: {0} Act: {1} Pst: {2} Flw: {3} Fav: {4} Tag: {5} Rep: {6} / last = {7}",
                    s.UnderlyingDataSet.Accounts.Count,
                    s.UnderlyingDataSet.Activities.Count,
                    s.UnderlyingDataSet.Posts.Count,
                    s.UnderlyingDataSet.FollowMap.Count,
                    s.UnderlyingDataSet.FavorMap.Count,
                    s.UnderlyingDataSet.TagMap.Count,
                    s.UnderlyingDataSet.ReplyMap.Count,
                    _since.ToString("R")
                );
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this._client.Connect();
        }

        private void timeLineListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.timeLineListView.SelectedItems.Count == 1)
            {
                Post post = this.timeLineListView.SelectedItems[0].Tag as Post;
                this.propertyGrid.SelectedObject = new
                {
                    Id = post.Activity.Account["Id"],
                    Name = post.Activity.Account["Name"],
                    ScreenName = post.Activity.Account["ScreenName"],
                    Location = post.Activity.Account["Location"],
                    Description = post.Activity.Account["Description"],
                    FollowersCount = post.Activity.Account["FollowersCount"],
                };
            }
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            this.userIdListBox.BeginUpdate();
            this.userIdListBox.Items.Clear();
            this.userIdListBox.Items.AddRange(this._client.Host.ModuleManager.GetModule<StorageModule>("sqlite")
                .GetAccounts().Select(a => a["ScreenName"]).OrderBy(s => s).ToArray());
            this.userIdListBox.EndUpdate();
        }

        private void miniBufferTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
