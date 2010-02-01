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
using System.Windows.Forms;
using System.Linq;
using Achiral;
using Achiral.Extension;
using XSpect.Extension;

namespace XSpect.MetaTweet.Clients.Mint
{
    public sealed class KeyInputManager
        : Object
    {
        private readonly LinkedList<Keys> _keyBuffer;

        public ClientCore Parent
        {
            get;
            private set;
        }

        public IDictionary<Tuple<String, Keys[]>, Tuple<String, IDictionary<String, String>>> Keybinds
        {
            get;
            private set;
        }
        
        public Control CurrentContext
        {
            get;
            private set;
        }

        public IEnumerable<Keys> KeyBuffer
        {
            get
            {
                return this._keyBuffer;
            }
        }

        public IEnumerable<KeyValuePair<Keys[], Tuple<String, IDictionary<String, String>>>> Candidates
        {
            get
            {
                return this.Keybinds.Where(e =>
                    (e.Key.Item1 == this.CurrentContext.Name || e.Key.Item1 == null) &&
                    e.Key.Item2.Take(this.KeyBuffer.Count()).SequenceEqual(this.KeyBuffer)
                ).Select(e => Create.KeyValuePair(e.Key.Item2, e.Value));
            }
        }

        public Boolean IsDetermined
        {
            get
            {
                return this.Candidates.Count() == 1
                    && this.Candidates.Single().Key.SequenceEqual(this.KeyBuffer);
            }
        }

        public event EventHandler<KeyInputEventArgs> KeyInputContinuing;

        public KeyInputManager(ClientCore parent)
        {
            this._keyBuffer = new LinkedList<Keys>();
            this.Parent = parent;
            this.Keybinds = new Dictionary<Tuple<String, Keys[]>, Tuple<String, IDictionary<String, String>>>();
            this.ResetKeyInput();
        }

        public void AddKeybind(String contextName, IEnumerable<Keys> keys, String functionName, IDictionary<String, String> parameters)
        {
            this.Keybinds.Add(Make.Tuple(contextName, keys.ToArray()), Make.Tuple(functionName, parameters));
        }

        public void RemoveKeybind(String contextName, IEnumerable<Keys> keys)
        {
            this.Keybinds.Remove(Make.Tuple(contextName, keys.ToArray()));
        }

        public void AddListener(params Control[] controls)
        {
            controls.ForEach(c => c.KeyDown += control_KeyDown);
        }

        public void RemoveListener(params Control[] controls)
        {
            controls.ForEach(c => c.KeyDown -= control_KeyDown);
        }

        private void control_KeyDown(Object sender, KeyEventArgs e)
        {
            Control s = sender as Control;
            if (this.CurrentContext != s)
            {
                this.CurrentContext = s;
                this._keyBuffer.Clear();
            }
            this._keyBuffer.AddLast(e.KeyData);
            IEnumerable<KeyValuePair<Keys[], Tuple<String, IDictionary<String, String>>>> c = this.Candidates;
            if (!c.Any())
            {
                this.ResetKeyInput();
            }
            else if (this.IsDetermined)
            {
                c.Single().Value.Let(_ => this.Parent.Functions[_.Item1](this.Parent, _.Item2));
                this.ResetKeyInput();
            }
            else if (this.KeyInputContinuing != null)
            {
                this.KeyInputContinuing(this, new KeyInputEventArgs(this));
            }
        }

        public void ResetKeyInput()
        {
            this.CurrentContext = this.Parent.MainForm;
            this._keyBuffer.Clear();
        }
    }
}