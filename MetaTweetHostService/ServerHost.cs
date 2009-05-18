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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace XSpect.MetaTweet
{
    [Serializable()]
    public partial class ServerHost
        : ServiceBase
    {
        public const String ServerDllName = "MetaTweetServer.dll";

        private AppDomain _serverDomain;

        private readonly Dictionary<String, String> _arguments;

        private Object _serverObject;

        public ServerHost()
        {
            this._arguments = new Dictionary<String, String>();
            this.InitializeComponent();
        }

        protected override void OnContinue()
        {
            this.StartServer();
        }

        protected override void OnPause()
        {
            this.StopServer();
        }

        protected override void OnStart(String[] args)
        {
            Environment.CurrentDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
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
                this._arguments[match.Groups["key"].Value] = match.Groups["value"].Success
                    ? match.Groups["value"].Value
                    : "true";
            }
            if (this._arguments.ContainsKey("host_debug") && this._arguments["host_debug"] == "true")
            {
                Debugger.Break();
            }
            this._arguments.Add(".pid", Process.GetCurrentProcess().Id.ToString());
            this._arguments.Add(".svc_id", this.ServiceName);
            this.StartServer();
        }

        protected override void OnStop()
        {
            this.StopServerGracefully();
        }

        private TDelegate GetMethod<TDelegate>(String name)
            where TDelegate : class
        {
            return Delegate.CreateDelegate(
                typeof(TDelegate),
                this._serverObject,
                this._serverObject
                    .GetType()
                    .GetMethod(name, BindingFlags.Public | BindingFlags.Instance)
            ) as TDelegate;
        }

        private void StartServer()
        {
            Environment.CurrentDirectory = Path.GetFullPath(this._arguments.ContainsKey("init_base")
                ? this._arguments["init_base"]
                : ".."
            );
            this._serverDomain = AppDomain.CreateDomain(
                ServerDllName,
                AppDomain.CurrentDomain.Evidence,
                new AppDomainSetup()
                {
                    ApplicationBase = Environment.CurrentDirectory,
                    ConfigurationFile = Path.Combine(this._arguments.ContainsKey("init_config")
                        ? this._arguments["init_config"]
                        : "etc", 
                        "MetaTweetServer.config"
                    ),
                    // Suppose Default (if not specified): -init_config=etc/<APP_NAME>.exe.config
                    PrivateBinPath = this._arguments.ContainsKey("init_probe")
                        ? this._arguments["init_probe"]
                        : "lib;sbin",
                    PrivateBinPathProbe = "true",
                    ApplicationName = "MetaTweetServer",
                    LoaderOptimization = LoaderOptimization.MultiDomainHost,
                }
            );

            this._serverDomain.DoCallBack(() =>
            {
                this._serverObject = Assembly.LoadFrom(Path.Combine(
                    this._arguments.ContainsKey("init_probe")
                        ? this._arguments["init_probe"].Split(';').First()
                        : "lib",
                    ServerDllName
                )).CreateInstance("XSpect.MetaTweet.ServerCore");
            });
            this._serverDomain.DoCallBack(() => this.GetMethod<Action<IDictionary<String, String>>>("Initialize")(this._arguments));
            this._serverDomain.DoCallBack(() => this.GetMethod<Action>("Start")());
        }

        private void StopServer()
        {
            this.GetMethod<Action>("Stop")();
            this._serverObject = null;
            AppDomain.Unload(this._serverDomain);
        }

        private void StopServerGracefully()
        {
            this.GetMethod<Action>("StopGracefully")();
            this._serverObject = null;
            AppDomain.Unload(this._serverDomain);
        }
    }
}
