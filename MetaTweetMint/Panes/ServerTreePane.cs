// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetMint
 *   Extensible GUI client for MetaTweet
 *   Part of MetaTweet
 * Copyright © 2009-2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetMint.
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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Achiral;
using Achiral.Extension;
using XSpect;
using XSpect.Collections;
using XSpect.Extension;
using XSpect.MetaTweet.Clients.Mint.DataModel;

namespace XSpect.MetaTweet.Clients.Mint.Panes
{
    public partial class ServerTreePane
        : DockContent
    {
        public ClientCore Client
        {
            get;
            private set;
        }

        public ServerTreePane(ClientCore client)
        {
            this.Client = client;
            InitializeComponent();
        }

        private void ServerTreePane_Load(object sender, EventArgs e)
        {
            this.InitializeServerConnectorCollection();
        }

        private void InitializeServerConnectorCollection()
        {
            this.Client.Connectors.Let(
                c => c.ItemsAdded += (sender, e) =>
                    e.NewElements
                        .ForEach(t => this.serversTreeView.Nodes.Insert(t.Index, t.Key, t.Key, "ServerConnector", "ServerConnector")
                            .Let(this.InitializeObjectViewCollection)
                        ),
                c => c.ItemsRemoved += (sender, e) => e.OldElements.ForEach(t => this.serversTreeView.Nodes.RemoveAt(t.Index))
            );
        }

        private void InitializeObjectViewCollection(TreeNode node)
        {
            this.Client.Connectors[node.FullPath].Views.Let(
                c => c.ItemsAdded += (sender, e) => e.NewElements
                    .ForEach(t => node.Nodes.Insert(t.Index, t.Key, t.Key, "ObjectView", "ObjectView")
                        .Let(this.InitializeObjectFilterCollection)
                    ),
                c => c.ItemsRemoved += (sender, e) => e.OldElements.ForEach(t => node.Nodes.RemoveAt(t.Index))
            );
        }

        private void InitializeObjectFilterCollection(TreeNode node)
        {
            node.FullPath.Split('\\')
                .Do(e => this.Client.Connectors[e[0]].Views[e[1]].Filters
                    .Do(c => c.Walk((_, p) => _[p].ChildFilters, e.Skip(2)))
                )
                .Let(
                    c => c.ItemsAdded += (sender, e) => e.NewElements
                        .ForEach(t => node.Nodes.Insert(t.Index, t.Key, t.Key, "ObjectFilter", "ObjectFilter")
                            .Let(this.InitializeObjectFilterCollection)
                        ),
                    c => c.ItemsRemoved += (sender, e) => e.OldElements.ForEach(t => node.Nodes.RemoveAt(t.Index))
                );
        }
    }
}
