// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetHostService
 *   Windows Service which hosts MetaTweetServer
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
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
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Diagnostics;

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

        public dynamic ServerObject
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
                    ApplicationBase = this._applicationBase
                        ?? (this._applicationBase = Path.GetFullPath(this.Arguments["init_base"])),
                    PrivateBinPath = this.Arguments["init_probe"],
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
            try
            {
                AppDomain.Unload(this._serverDomain);
            }
            catch (CannotUnloadAppDomainException ex)
            {
                // TODO: handle the exception or fix the problem
                Debug.Fail("Caught CannotUnloadAppDomainException", ex.Message);
            }
        }

        public void StopServerGracefully()
        {
            this._serverDomain.DoCallBack(this._StopServerGracefully);
            try
            {
                AppDomain.Unload(this._serverDomain);
            }
            catch (CannotUnloadAppDomainException ex)
            {
                // TODO: handle the exception or fix the problem
                Debug.Fail("Caught CannotUnloadAppDomainException", ex.Message);
            }
        }

        private void _StartServer()
        {
            String cultureString;
            Thread.CurrentThread.CurrentCulture = this.Arguments.ContainsKey("culture")
                ? String.IsNullOrEmpty(cultureString = this.Arguments["culture"])
                      ? Thread.CurrentThread.CurrentCulture
                      : cultureString == "invaliant"
                            ? CultureInfo.InvariantCulture
                            : CultureInfo.GetCultureInfo(cultureString)
                : CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            this.ServerObject = Assembly.LoadFrom(Path.Combine(
                this.Arguments.ContainsKey("init_probe")
                    ? this.Arguments["init_probe"].Split(';').First()
                    : "lib",
                ServerDllName
            )).CreateInstance("XSpect.MetaTweet.ServerCore");
            this.ServerObject.Initialize(this.Arguments);
            this.ServerObject.Start();
        }

        private void _StopServer()
        {
            this.ServerObject.Stop();
            this.ServerObject.Dispose();
            this.ServerObject = null;
        }

        private void _StopServerGracefully()
        {
            this.ServerObject.StopGracefully();
            this.ServerObject.Dispose();
            this.ServerObject = null;
        }
    }
}