// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetHostService
 *   Windows Service which hosts MetaTweetServer
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetServer.
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
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace XSpect.MetaTweet
{
    public partial class ServerHost
        : ServiceBase
    {
        private ServerCore _server;

        public ServerHost()
        {
            this._server = new ServerCore();
            InitializeComponent();
        }
        
        public override EventLog EventLog
        {
            get
            {
                // Use ServerCore#Log (log4net.ILog) instead.
                return null;
            }
        }

        protected override void OnContinue()
        {
            this._server.Continue();
        }

        protected override void OnPause()
        {
            this._server.Pause();
        }

        protected override void OnStart(String[] args)
        {
            Dictionary<String, String> argDic = new Dictionary<String, String>();
            Match match;

            foreach (String arg in args)
            {
                if ((match = Regex.Match(arg, "(-(?<key>[a-zA-Z0-9_]*)=(?<value>(\"[^\"]*\")|('[^']*')|(.*)))*")).Success)
                {
                    argDic.Add(match.Groups["key"].Value, match.Groups["value"].Value);
                }
            }

            this._server.Initialize(argDic);
            this._server.Start();
        }

        protected override void OnStop()
        {
            this._server.Stop();
        }
    }
}
