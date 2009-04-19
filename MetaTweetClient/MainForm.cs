// -*- mode: csharp; encoding: utf-8; -*-
// $Id$
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
using XSpect.Extension;
using XSpect.Configuration;
using Achiral.Extension;

namespace XSpect.MetaTweet.Clients
{
    public partial class MainForm
        : Form
    {
        private DateTime _since = DateTime.MinValue;

        private MetaTweetClient _client = new MetaTweetClient();

        private String _textBoxText;

        private readonly XmlConfiguration _config;

        public MainForm()
        {
            InitializeComponent();
            this._config = XmlConfiguration.Load("MetaTweetClient.conf.xml");
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
                        this._client.Host.Request<Object>(new Request(
                            "main",
                            "twitter",
                            "/statuses/update",
                            new Dictionary<String, String>()
                            {
                                {"status", this.miniBufferTextBox.Text + " [MetaTweet]"},
                                {"source", "metatweet"},
                            },
                            new Request("main", "sys", "/.null")
                        ));
                        this.miniBufferTextBox.Clear();
                    }
                    else
                    {
                        List<Post> posts = this._client.Host.Request<IEnumerable<StorageObject>>(new Request(
                            "main",
                            "sys",
                            "/get-posts",
                            new Dictionary<String, String>()
                            {
                                {"count", "1000"},
                            },
                            new Request("main", "sys", "/.obj")
                        )).OfType<Post>().ToList();
                        this.timeLineListView.BeginUpdate();
                        this.timeLineListView.Items.Clear();
                        foreach (Post post in posts)
                        {
                            ListViewItem item = new ListViewItem(new String[]
                            {
                                post.ActivityInDataSet.Timestamp.ToLocalTime().ToString("s").Replace("T", " "),
                                post.ActivityInDataSet.AccountInDataSet.GetActivityInDataSetOf("ScreenName").Value,
                                post.Text,
                                post.Source,
                            })
                            {
                                Tag = post,
                            };
                            this.timeLineListView.Items.Add(item);
                        }
                        this.timeLineListView.EndUpdate();
                        this._since = DateTime.Now.ToUniversalTime();
                    }
                }
                else
                {
                    if(this._config.GetValue<XmlConfiguration>("keybind").ContainsKey(e.ToKeyString()))
                    {
                        this._config
                            .GetValue<XmlConfiguration>("keybind")
                            .GetValue<String[]>(e.ToKeyString())
                            .ForEach(r => this._client.Host.Request<Object>(Request.Parse(r)));
                    }
                }
                var s = this._client.Host.ModuleManager.GetModule<StorageModule>("main");
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
                    Id = post.ActivityInDataSet.AccountInDataSet.GetActivityInDataSetOf("Id").Null(a => a.Value),
                    Name = post.ActivityInDataSet.AccountInDataSet.GetActivityInDataSetOf("Name").Null(a => a.Value),
                    ScreenName = post.ActivityInDataSet.AccountInDataSet.GetActivityInDataSetOf("ScreenName").Null(a => a.Value),
                    Location = post.ActivityInDataSet.AccountInDataSet.GetActivityInDataSetOf("Location").Null(a => a.Value),
                    Description = post.ActivityInDataSet.AccountInDataSet.GetActivityInDataSetOf("Description").Null(a => a.Value),
                    FollowersCount = post.ActivityInDataSet.AccountInDataSet.GetActivityInDataSetOf("FollowersCount").Null(a => a.Value),
                };
            }
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
        }

        private void miniBufferTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
