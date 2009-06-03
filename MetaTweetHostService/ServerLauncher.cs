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
using System.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace XSpect.MetaTweet
{
    [Serializable()]
    public sealed class ServerLauncher
        : Object
    {
        public const String ServerDllName = "MetaTweetServer.dll";

        private static readonly ServerLauncher _instance = new ServerLauncher();

        private AppDomain _serverDomain;

        private String _applicationBase;

        public static ServerLauncher Instance
        {
            get
            {
                return _instance;
            }
        }

        public IDictionary<String, String> Arguments
        {
            get;
            set;
        }

        public Object ServerObject
        {
            get
            {
                return this._serverDomain.GetData("server");
            }
            set
            {
                this._serverDomain.SetData("server", value);
            }
        }

        public ServerLauncher()
        {
            this.Arguments = ConfigurationManager.AppSettings.AllKeys
                .ToDictionary(k => k, k => ConfigurationManager.AppSettings[k]);

        }

        public void StartServer()
        {
            this._serverDomain = AppDomain.CreateDomain(
                ServerDllName,
                AppDomain.CurrentDomain.Evidence,
                new AppDomainSetup()
                {
                    ApplicationBase = this._applicationBase ?? (this._applicationBase
                        = Path.GetFullPath(this.Arguments.ContainsKey("init_base")
                            ? this.Arguments["init_base"]
                            : ".."
                        )
                    ),
                    ConfigurationFile = Path.Combine(this.Arguments.ContainsKey("init_config")
                        ? this.Arguments["init_config"]
                        : "etc",
                        "MetaTweetServer.config"
                    ),
                    PrivateBinPath = this.Arguments.ContainsKey("init_probe")
                        ? this.Arguments["init_probe"]
                        : "lib;sbin",
                    PrivateBinPathProbe = "true",
                    ApplicationName = "MetaTweetServer",
                    LoaderOptimization = LoaderOptimization.MultiDomainHost,
                }
            );

            Environment.CurrentDirectory = this._serverDomain.BaseDirectory;
            this._serverDomain.DoCallBack(this._StartServer);
        }

        public void StopServer()
        {
            this._serverDomain.DoCallBack(this._StopServer);
            AppDomain.Unload(this._serverDomain);
        }

        public void StopServerGracefully()
        {
            this._serverDomain.DoCallBack(this._StopServerGracefully);
            AppDomain.Unload(this._serverDomain);
        }

        private TDelegate GetMethod<TDelegate>(String name)
            where TDelegate : class
        {
            return Delegate.CreateDelegate(
                typeof(TDelegate),
                this.ServerObject,
                this.ServerObject
                    .GetType()
                    .GetMethod(name, BindingFlags.Public | BindingFlags.Instance)
            ) as TDelegate;
        }

        private void _StartServer()
        {
            this.ServerObject = Assembly.LoadFrom(Path.Combine(
                this.Arguments.ContainsKey("init_probe")
                    ? this.Arguments["init_probe"].Split(';').First()
                    : "lib",
                ServerDllName
            )).CreateInstance("XSpect.MetaTweet.ServerCore");
            this.GetMethod<Action<IDictionary<String, String>>>("Initialize")(this.Arguments);
            this.GetMethod<Action>("Start")();
        }

        private void _StopServer()
        {
            this.GetMethod<Action>("Stop")();
            this.GetMethod<Action>("Dispose")();
            this.ServerObject = null;
        }

        private void _StopServerGracefully()
        {
            this.GetMethod<Action>("StopGracefully")();
            this.GetMethod<Action>("Dispose")();
            this.ServerObject = null;
        }
    }
}