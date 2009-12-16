// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetMint
 *   Extensible GUI client for MetaTweet
 *   Part of MetaTweet
 * Copyright © 2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Achiral;
using Achiral.Extension;
using WeifenLuo.WinFormsUI.Docking;
using XSpect.MetaTweet.Clients.Mint.Contents;
using XSpect.MetaTweet.Objects;
using XSpect.Reflection;
using XSpect.Windows.Forms;

namespace XSpect.MetaTweet.Clients.Mint
{
    public partial class MainForm
        : Form
    {
        private readonly List<Keys> _keyBuffer;

        public ClientCore Client
        {
            get;
            private set;
        }

        public Stack<MinibufferLevel> MinibufferStack
        {
            get;
            private set;
        }

        public String MinibufferTitleText
        {
            get
            {
                return this.minibufferTitleLabel.Text;
            }
            set
            {
                this.minibufferTitleLabel.Visible = !value.IsNullOrEmpty();
                this.minibufferTitleLabel.Text = value;
            }
        }

        public String MinibufferText
        {
            get
            {
                return this.minibufferTextBox.Text;
            }
            set
            {
                this.minibufferTextBox.Text = value;
            }
        }

        public String ModeLineText
        {
            get
            {
                return this.modeLineTextBox.Text;
            }
            set
            {
                this.modeLineTextBox.Text = value;
            }
        }

        public String StatusBarText
        {
            get
            {
                return this.mainStatusLabel.Text;
            }
            set
            {
                this.mainStatusLabel.Text = value;
            }
        }

        public DockPanel DockPanel
        {
            get
            {
                return this.dockPanel;
            }
        }

        public MainForm(ClientCore client)
        {
            this.Client = client;
            this._keyBuffer = new List<Keys>();
            this.MinibufferStack = new Stack<MinibufferLevel>();
            InitializeComponent();
            Initialize();
        }

        public void Initialize()
        {
            this.Font = this.Client.Fonts.Default;
            this.minibufferTextBox.Font = this.Client.Fonts.Minibuffer;
            this.minibufferTitleLabel.Font = this.Client.Fonts.MinibufferTitle;
            this.modeLineTextBox.Font = this.Client.Fonts.ModeLine;
            this.mainStatusStrip.Font = this.Client.Fonts.StatusBar;
        }

        public MinibufferLevel StartNewMinibufferLevel(String title)
        {
            return this.StartNewMinibufferLevel(title, null);
        }

        public MinibufferLevel StartNewMinibufferLevel(String title, EventHandler<EventArgs> callback)
        {
            MinibufferLevel level = new MinibufferLevel()
            {
                Title = title,
            };
            this.MinibufferStack.Push(level);
            this.minibufferTextBox.ResetText();
            if (title.IsNullOrEmpty())
            {
                this.MinibufferTitleText = String.Empty;
            }
            else
            {
                this.MinibufferTitleText = title;
            }
            if(callback != null)
            {
                level.LevelEnded += callback;
            }
            return level;
        }

        public Int32 EndMinibufferLevel(Boolean kill)
        {
            if (this.MinibufferStack.Count == 0)
            {
                // TODO: Stub function.
                this.Client.EvaluateFunction(
                    "post",
                    Create.Table("body", minibufferTextBox.Text + " [MetaTweet ." + ThisAssembly.EntireCommitCount + "]"),
                    false
                );
                return 0;
            }
            MinibufferLevel level = this.MinibufferStack.Pop();
            level.Title = this.MinibufferTitleText;
            level.Body = this.MinibufferText;
            if (!kill)
            {
                level.NotifyLevelEnded();
            }
            if (this.MinibufferStack.Count > 0)
            {
                level = this.MinibufferStack.Peek();
                this.MinibufferTitleText = level.Title;
                this.MinibufferText = level.Body;
            }
            else
            {
                this.MinibufferTitleText = String.Empty;
                this.MinibufferText = String.Empty;
            }
            return this.MinibufferStack.Count;
        }

        public Int32 EndMinibufferLevel()
        {
            return this.EndMinibufferLevel(false);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            DockContent content = new StartWindow()
            {
                MdiParent = this,
            };
            content.Show(this.dockPanel);
            content = new ContentTreeWindow(this.Client)
            {
                MdiParent = this,
            };
            content.Show(this.dockPanel);
            content.DockState = DockState.DockLeft;
        }

        private void MainForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            IEnumerable<KeyValuePair<Keys[], String>> functions
                = this.Client.GetBoundFunctions(this._keyBuffer.Concat(Make.Array(e.KeyData)).ToArray());
            switch (functions.Count())
            {
                case 0:
                    break;
                case 1:
                    this.Client.Functions[functions.Single().Value](this.Client, null);
                    return;
                default:
                    this._keyBuffer.Add(e.KeyData);
                    this.StatusBarText = this._keyBuffer.Select(k => k.ToKeyString()).Join(" ");
                    // TODO: Frame Mechanism or candidate popup
                    return;
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void minibufferTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData.ToKeyString() == "C-Return")
            {
                this.EndMinibufferLevel();
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }
    }
}
