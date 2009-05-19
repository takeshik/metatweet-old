// -*- mode: csharp; encoding: utf-8; -*-
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetHostService
 *   Windows Service which hosts MetaTweetServer
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetHostService.
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
using System.Diagnostics;
using System.ServiceProcess;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace XSpect.MetaTweet
{
    public partial class ServerHost
        : ServiceBase
    {
        public ServerLauncher Launcher
        {
            get
            {
                return ServerLauncher.Instance;
            }
        }

        public ServerHost()
        {
            this.InitializeComponent();
        }

        protected override void OnContinue()
        {
            this.Launcher.StartServer();
        }

        protected override void OnPause()
        {
            this.Launcher.StopServerGracefully();
        }

        protected override void OnStart(String[] args)
        {
            Environment.CurrentDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            // TODO: Replace to App.config
            if (File.Exists("MetaTweetServer.args"))
            {
                args = File.ReadAllLines("MetaTweetServer.args")
                    .Where(l => !(l.StartsWith("#") || String.IsNullOrEmpty(l)))
                    .Concat(args)
                    .ToArray();
            }
            foreach (Match match in args
                .Select(s => Regex.Match(s, "(-(?<key>[a-zA-Z0-9_]*)(=(?<value>(\"[^\"]*\")|('[^']*')|(.*)))?)*"))
                .Where(m => m.Success)
            )
            {
                this.Launcher.Arguments[match.Groups["key"].Value] = match.Groups["value"].Success
                    ? match.Groups["value"].Value
                    : "true";
            }
            if (this.Launcher.Arguments.ContainsKey("host_debug") && this.Launcher.Arguments["host_debug"] == "true")
            {
                Debugger.Break();
            }
            this.Launcher.Arguments.Add(".pid", Process.GetCurrentProcess().Id.ToString());
            this.Launcher.Arguments.Add(".svc_id", this.ServiceName);
            this.Launcher.StartServer();
        }

        protected override void OnStop()
        {
            this.Launcher.StopServerGracefully();
        }
    }
}
