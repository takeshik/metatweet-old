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
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Achiral;
using Achiral.Extension;
using XSpect.Configuration;
using XSpect.Extension;
using XSpect.MetaTweet.Clients.Mint.DataModel;
using XSpect.MetaTweet.Clients.Mint.Evaluating;
using XSpect.Reflection;
using XSpect.Windows.Forms;

namespace XSpect.MetaTweet.Clients.Mint
{
    public static class Initializer
    {
        private static ClientCore _host;

        public static void Initialize(IDictionary<String, Object> args)
        {
            _host = args["host"] as ClientCore;
            AddFunctions();
            AddKeybinds();
            AddMenus();
        }

        private static void AddFunctions()
        {
            _host.Functions.Add("call-function", new MethodReference((h, a) =>
                h.MainForm.StartNewMinibufferLevel("M-x", (sender, e) =>
                    (sender as MinibufferLevel).Body
                        .Split(Make.Array(Environment.NewLine), StringSplitOptions.RemoveEmptyEntries)
                        .Let(b => h.Functions[b.First()].Evaluate(h, b
                            .Skip(1)
                            .Select(s => s.Split('=').Do(p => Create.KeyValuePair(p[0], p[1])))
                            .ToDictionary())
                        )
                )
            ));
            _host.Functions.Add("exit-minibuffer-level", new MethodReference((h, a) => h.MainForm.EndMinibufferLevel()));
            _host.Functions.Add("kill-minibuffer-level", new MethodReference((h, a) => h.MainForm.EndMinibufferLevel(true)));
            _host.Functions.Add("exit-application", new MethodReference((h, a) => Application.ExitThread()));
        }

        private static void AddKeybinds()
        {
            _host.KeyInputManager.AddKeybind(null, KeyString.GetKeysArray("M-x"), "call-function", null);
            _host.KeyInputManager.AddKeybind(null, KeyString.GetKeysArray("C-Enter"), "exit-minibuffer-level", null);
            _host.KeyInputManager.AddKeybind(null, KeyString.GetKeysArray("C-g"), "kill-minibuffer-level", null);
            _host.KeyInputManager.AddKeybind(null, KeyString.GetKeysArray("C-q a"), "exit-application", null);
        }

        private static void AddMenus()
        {
            _host.MainForm.MenuItems.Add("file", "&File");
            _host.MainForm.MenuItems.Add("file/exit", "E&xit", "exit-application", null);
        }
    }
}
