// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetServer.
 * 
 * This library is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation; either version 3 of the License, or (at your
 * option) any later version.
 * 
 * This library is distributed in the hope that it will be useful, but
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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;
using XSpect.MetaTweet.Properties;
using XSpect.Reflection;
using System.Text.RegularExpressions;
using XSpect.MetaTweet.ObjectModel;
using System.Diagnostics;
using XSpect.MetaTweet.Modules;
using Achiral;
using Achiral.Extension;
using XSpect.Configuration;

namespace XSpect.MetaTweet
{
    public sealed class ServerCore
        : MarshalByRefObject,
          IDisposable
    {
        private static readonly DirectoryInfo _rootDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;

        public static DirectoryInfo RootDirectory
        {
            get
            {
                return _rootDirectory;
            }
        }

        public XmlConfiguration Configuration
        {
            get;
            private set;
        }

        public IDictionary<String, String> Parameters
        {
            get;
            private set;
        }

        public ModuleManager ModuleManager
        {
            get;
            private set;
        }

        public ILog Log
        {
            get;
            private set;
        }

        public Hook<ServerCore> InitializeHook
        {
            get;
            private set;
        }

        public Hook<ServerCore> StartHook
        {
            get;
            private set;
        }

        public Hook<ServerCore> StopHook
        {
            get;
            private set;
        }

        public Hook<ServerCore> PauseHook
        {
            get;
            private set;
        }

        public Hook<ServerCore> ContinueHook
        {
            get;
            private set;
        }

        public Hook<ServerCore> WaitToEndHook
        {
            get;
            private set;
        }

        public Hook<ServerCore> TerminateHook
        {
            get;
            private set;
        }

        public ServerCore()
        {
            this.Parameters = new Dictionary<String, String>();
            this.ModuleManager = new ModuleManager(this);
            this.Log = LogManager.GetLogger(typeof(ServerCore));
            this.InitializeHook = new Hook<ServerCore>();
            this.StartHook = new Hook<ServerCore>();
            this.StopHook = new Hook<ServerCore>();
            this.PauseHook = new Hook<ServerCore>();
            this.ContinueHook = new Hook<ServerCore>();
            this.WaitToEndHook = new Hook<ServerCore>();
            this.TerminateHook = new Hook<ServerCore>();
        }

        /// <summary>
        /// 対象のインスタンスの有効期間ポリシーを制御する、有効期間サービス オブジェクトを取得します。
        /// </summary>
        /// <returns>
        /// 対象のインスタンスの有効期間ポリシーを制御するときに使用する、<see cref="T:System.Runtime.Remoting.Lifetime.ILease"/> 型のオブジェクト。存在する場合は、このインスタンスの現在の有効期間サービス オブジェクトです。それ以外の場合は、<see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime"/> プロパティの値に初期化された新しい有効期間サービス オブジェクトです。
        /// </returns>
        /// <exception cref="T:System.Security.SecurityException">直前の呼び出し元に、インフラストラクチャ アクセス許可がありません。</exception>
        /// <PermissionSet>
        /// <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure"/>
        /// </PermissionSet>
        public override Object InitializeLifetimeService()
        {
            return null;
        }

        public void Initialize(IDictionary<String, String> arguments)
        {
            this.Parameters = arguments;

            if (this.Parameters.ContainsKey("debug") && this.Parameters["debug"] == "true")
            {
                Debugger.Launch();
                Debugger.Break();
            }

            this.Configuration = this.Parameters.ContainsKey("config")
                ? XmlConfiguration.Load(this.Parameters["config"])
                : new XmlConfiguration();
            this.InitializeDefaultLogHooks();
            this.ModuleManager.Execute(RootDirectory.GetFiles("init.*").Single());
        }

        private void InitializeDefaultLogHooks()
        {
            this.InitializeHook.Before.Add(self => self.Log.InfoFormat(
                Resources.ServerInitializing,
                Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                Environment.OSVersion.ToString(),
                Thread.CurrentThread.CurrentUICulture.ToString()
            ));
            this.InitializeHook.After.Add(self => self.Log.Info(Resources.ServerInitialized));
            this.StartHook.Before.Add(self => self.Log.Info(Resources.ServerStarting));
            this.StartHook.After.Add(self => self.Log.Info(Resources.ServerStarted));
            this.StopHook.Before.Add(self => self.Log.Info(Resources.ServerStopping));
            this.StopHook.After.Add(self => self.Log.Info(Resources.ServerStopped));
            this.PauseHook.Before.Add(self => self.Log.Info(Resources.ServerPausing));
            this.PauseHook.After.Add(self => self.Log.Info(Resources.ServerPaused));
            this.ContinueHook.Before.Add(self => self.Log.Info(Resources.ServerResuming));
            this.ContinueHook.After.Add(self => self.Log.Info(Resources.ServerResumed));
            this.TerminateHook.Before.Add(self => self.Log.Info(Resources.ServerTerminating));
            this.TerminateHook.After.Add(self => self.Log.Info(Resources.ServerTerminated));
            this.WaitToEndHook.Before.Add(self => self.Log.Info(Resources.ServerWaitingToEnd));
            this.WaitToEndHook.After.Add(self => self.Log.Info(Resources.ServerWaitedToEnd));
        }

        public void Start()
        {
            this.StartHook.Execute(self =>
            {
                if (self.ModuleManager.GetModules<ServantModule>().Any())
                {
                    IEnumerable<IAsyncResult> asyncResults = self.ModuleManager
                        .GetModules<ServantModule>()
                        .Select(l => l.BeginStart(
                            r => (r.AsyncState as ServantModule).EndStart(r), l
                        ));
                    WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
                }
            }, this);
        }

        public void Stop()
        {
            this.StopHook.Execute(self =>
            {
                if (self.ModuleManager.GetModules<ServantModule>().Any())
                {
                    IEnumerable<IAsyncResult> asyncResults = self.ModuleManager
                        .GetModules<ServantModule>()
                        .Select(l => l.BeginAbort(
                            r =>
                            {
                                (r.AsyncState as ServantModule).EndAbort(r);
                                (r.AsyncState as ServantModule).Stop();
                            }, l
                        ));
                    WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
                }
            }, this);
        }

        public void StopGracefully()
        {
            this.StopHook.Execute(self =>
            {
                if (self.ModuleManager.GetModules<ServantModule>().Any())
                {
                    IEnumerable<IAsyncResult> asyncResults = self.ModuleManager
                        .GetModules<ServantModule>()
                        .Select(l => l.BeginWait(
                            r =>
                            {
                                (r.AsyncState as ServantModule).EndWait(r);
                                (r.AsyncState as ServantModule).Stop();
                            }, l
                        ));
                    WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
                }
            }, this);
        }

        public void Pause()
        {
            this.PauseHook.Execute(self =>
            {
                if (self.ModuleManager.GetModules<ServantModule>().Any())
                {
                    IEnumerable<IAsyncResult> asyncResults = self.ModuleManager
                        .GetModules<ServantModule>()
                        .Select(l => l.BeginPause(
                            r => (r.AsyncState as ServantModule).EndPause(r), l
                        ));
                    WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
                }
            }, this);
        }

        public void Continue()
        {
            this.ContinueHook.Execute(self =>
            {
                if (self.ModuleManager.GetModules<ServantModule>().Any())
                {
                    IEnumerable<IAsyncResult> asyncResults = self.ModuleManager
                        .GetModules<ServantModule>()
                        .Select(l => l.BeginContinue(
                            r => (r.AsyncState as ServantModule).EndContinue(r), l
                        ));
                    WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
                }
            }, this);
        }

        public void Dispose()
        {
            this.TerminateHook.Execute(self =>
            {
            }, this);
        }

        public void WaitToEnd()
        {
            this.WaitToEndHook.Execute(self =>
            {
                if (self.ModuleManager.GetModules<ServantModule>().Any())
                {
                    IEnumerable<IAsyncResult> asyncResults = self.ModuleManager
                        .GetModules<ServantModule>()
                        .Select(l => l.BeginWait(
                            r => (r.AsyncState as ServantModule).EndStart(r), l
                        ));
                    WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
                }
            }, this);
        }

        public T Request<T>(String request)
        {
            if (request[request.LastIndexOf('/') + 1] != '.')
            {
                // example.ext?foo=bar -> example?foo=bar/!/.ext
                request = Regex.Replace(request, @"(\.[^?]*)(\?.*)?$", @"$2/!/$1");
            }

            IEnumerable<String> units = Regex.Split(request, "/[!$]").SkipWhile(String.IsNullOrEmpty);
            IEnumerable<StorageObject> results = null;
            String storage = "main"; // Default Storage
            String module = "sys";   // Default Module
            Int32 index = 0;

            // a) .../$storage!module/... -> storage!module/...
            // b) .../$storage!/...       -> storage!/...
            // c) .../!module/...         -> module/...
            // d) .../!/...               -> /...
            foreach(String elem in units)
            {
                String prefixes = elem.Substring(0, elem.IndexOf('/'));
                if (prefixes.Contains("!"))
                {
                    if (!prefixes.EndsWith("!")) // a) Specified Storage and Module
                    {
                        String[] prefixArray = prefixes.Split('!');
                        storage = prefixArray[0];
                        module = prefixArray[1];
                    }
                    else // b) Specified Storage
                    {
                        storage = prefixes.TrimEnd('!');
                        // Module is taken over.
                    }
                }
                else
                {
                    if (prefixes != String.Empty) // c) Specified Module
                    {
                        // Storage is taken over.
                        module = prefixes;
                    }
                    else // d) Specified nothing
                    {
                        // Do nothing; Storage and Module are taken over.
                    }
                }

                String selector;
                Dictionary<String, String> argumentDictionary = new Dictionary<String, String>();
                
                if (elem.Contains("?"))
                {
                    selector = elem.Substring(prefixes.Length, elem.IndexOf('?') - prefixes.Length);
                    String arguments = elem.Substring(prefixes.Length + selector.Length);
                    foreach (String[] pair in arguments
                        .TrimStart('?')
                        .Split('&')
                        .Select(s => s.Split('='))
                    )
                    {
                        argumentDictionary.Add(pair[0], pair[1]);
                    }
                }
                else
                {
                    selector = elem.Substring(prefixes.Length);
                }

                if (index == 0) // Invoking InputFlowModule
                {
                    results = this.ModuleManager.GetModule<InputFlowModule>(module).Input(
                        selector,
                        this.ModuleManager.GetModule<StorageModule>(storage),
                        argumentDictionary
                    );
                }
                else if (index != units.Count() - 1) // Invoking FilterFlowModule
                {
                    this.ModuleManager.GetModule<FilterFlowModule>(module).Filter(
                        selector,
                        results,
                        this.ModuleManager.GetModule<StorageModule>(storage), argumentDictionary
                    );
                }
                else // Invoking OutputFlowModule
                {
                    return this.ModuleManager.GetModule<OutputFlowModule>(module).Output<T>(
                        selector,
                        results,
                        this.ModuleManager.GetModule<StorageModule>(storage),
                        argumentDictionary
                    );
                }

                ++index;
            }

            // Throws when not returned yet (it means Output module is not invoked.)
            throw new ArgumentException("uri");
        }
    }
}