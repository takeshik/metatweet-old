// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetHostService
 *   Windows Service which hosts MetaTweetServer
 *   Part of MetaTweet
 * Copyright © 2008-2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
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
            foreach (Match match in args
                .TakeWhile(s => s != "--")
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
                Debugger.Launch();
            }
            this.Launcher.Arguments[".pid"] = Process.GetCurrentProcess().Id.ToString();
            this.Launcher.Arguments[".svc_id"] = this.ServiceName;
            this.Launcher.StartServer();
        }

        protected override void OnStop()
        {
            this.Launcher.StopServerGracefully();
        }
    }
}
