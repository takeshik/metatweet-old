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
using XSpect.Extension;

namespace XSpect.MetaTweet
{
    /// <summary>
    /// サーバ オブジェクトを表します。サーバ オブジェクトは、MetaTweet サーバ全体を表すオブジェクトで、他の全てのサーバ構造へのアクセスを提供します。このクラスは継承できません。
    /// </summary>
    public sealed class ServerCore
        : MarshalByRefObject,
          IDisposable
    {
        /// <summary>
        /// MetaTweet サーバのルートディレクトリを取得します。
        /// </summary>
        /// <value>
        /// MetaTweet サーバのルートディレクトリ。
        /// </value>
        public DirectoryInfo RootDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// サーバ オブジェクトの生成時に渡されたパラメータのリストを取得します。
        /// </summary>
        /// <value>
        /// サーバ オブジェクトの生成時に渡されたパラメータのリスト。
        /// </value>
        public IDictionary<String, String> Parameters
        {
            get;
            private set;
        }

        /// <summary>
        /// サーバ オブジェクトの設定を管理するオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// サーバ オブジェクトの設定を管理するオブジェクト。
        /// </value>
        public XmlConfiguration Configuration
        {
            get;
            private set;
        }

        /// <summary>
        /// このサーバ オブジェクトのモジュール マネージャを取得します。
        /// </summary>
        /// <value>
        /// このサーバ オブジェクトのモジュール マネージャ。
        /// </value>
        public ModuleManager ModuleManager
        {
            get;
            private set;
        }

        /// <summary>
        /// このサーバ オブジェクトのイベント ログ ライタを取得します。
        /// </summary>
        /// <value>
        /// このサーバ オブジェクトのイベント ログ ライタ。
        /// </value>
        public ILog Log
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="Initialize"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="Initialize"/> のフック リスト。
        /// </value>
        public Hook<ServerCore> InitializeHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="Start"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="Start"/> のフック リスト。
        /// </value>
        public Hook<ServerCore> StartHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="Stop"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="Stop"/> のフック リスト。
        /// </value>
        public Hook<ServerCore> StopHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="Pause"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="Pause"/> のフック リスト。
        /// </value>
        public Hook<ServerCore> PauseHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="Continue"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="Continue"/> のフック リスト。
        /// </value>
        public Hook<ServerCore> ContinueHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="Dispose"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="Dispose"/> のフック リスト。
        /// </value>
        public Hook<ServerCore> TerminateHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="ServerCore"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public ServerCore()
        {
            this.RootDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;
            this.Log = LogManager.GetLogger(typeof(ServerCore));
            this.InitializeHook = new Hook<ServerCore>();
            this.StartHook = new Hook<ServerCore>();
            this.StopHook = new Hook<ServerCore>();
            this.PauseHook = new Hook<ServerCore>();
            this.ContinueHook = new Hook<ServerCore>();
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

        /// <summary>
        /// <see cref="ServerCore"/> によって使用されているすべてのリソースを解放します。
        /// </summary>
        public void Dispose()
        {
            this.TerminateHook.Execute(self =>
            {
                self.ModuleManager.GetModules().ForEach(m => m.Dispose());
            }, this);
        }

        /// <summary>
        /// サーバ オブジェクトを初期化します。
        /// </summary>
        /// <param name="arguments">サーバ オブジェクトに渡す引数のリスト。</param>
        public void Initialize(IDictionary<String, String> arguments)
        {
            this.Parameters = arguments;
            
            if (this.Parameters.ContainsKey("debug") && this.Parameters["debug"] == "true")
            {
                Debugger.Launch();
                Debugger.Break();
            }

            this.InitializeDefaultLogHooks();

            this.InitializeHook.Execute(self =>
            {
                this.Configuration = this.Parameters.ContainsKey("config")
                    ? XmlConfiguration.Load(this.Parameters["config"])
                    : new XmlConfiguration();
                this.ModuleManager = new ModuleManager(
                    this,
                    this.RootDirectory.CreateSubdirectory(this.Configuration.GetValueOrDefault("moduleDirectory", "module")),
                    this.RootDirectory.CreateSubdirectory(this.Configuration.GetValueOrDefault("cacheDirectory", "cache")),
                    this.RootDirectory.CreateSubdirectory(this.Configuration.GetValueOrDefault("configDirectory", "conf")),
                    this.RootDirectory.CreateSubdirectory(this.Configuration.GetValueOrDefault("tempDirectory", "temp"))
                );
                this.ModuleManager.TempDirectory.GetFileSystemInfos().ForEach(fso => fso.Delete());
                FileInfo initFile = this.RootDirectory.GetFiles("init.*").SingleOrDefault();
                if (initFile != null)
                {
                    this.ModuleManager.Execute(initFile);
                }
                else
                {
                    Initializer.Initialize(this, arguments);
                }
            }, this);
        }

        private void InitializeDefaultLogHooks()
        {
            this.InitializeHook.Before.Add(self => self.Log.WarnFormat(
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
        }

        /// <summary>
        /// サーバおよびサーバント オブジェクトを開始します。
        /// </summary>
        public void Start()
        {
            this.StartHook.Execute(self =>
            {
                self.StartServants();
            }, this);
        }

        /// <summary>
        /// サーバおよびサーバント オブジェクトを停止します。
        /// </summary>
        public void Stop()
        {
            this.StopHook.Execute(self =>
            {
                self.AbortServants();
            }, this);
        }

        /// <summary>
        /// サーバおよびサーバント オブジェクトを安全に停止します。
        /// </summary>
        public void StopGracefully()
        {
            this.StopHook.Execute(self =>
            {
                self.StopServants();
            }, this);
        }

        /// <summary>
        /// サーバント オブジェクトを停止します。
        /// </summary>
        public void Pause()
        {
            this.PauseHook.Execute(self =>
            {
                self.AbortServants();
            }, this);
        }

        /// <summary>
        /// サーバント オブジェクトを安全に停止します。
        /// </summary>
        public void PauseGracefully()
        {
            this.PauseHook.Execute(self =>
            {
                self.StopServants();
            }, this);
        }

        /// <summary>
        /// サーバント オブジェクトを再開します。
        /// </summary>
        public void Continue()
        {
            this.ContinueHook.Execute(self =>
            {
                self.StartServants();
            }, this);
        }

        /// <summary>
        /// 登録されているサーバント オブジェクトをすべて開始させます。
        /// </summary>
        private void StartServants()
        {
            if (this.ModuleManager.GetModules<ServantModule>().Any())
            {
                IEnumerable<IAsyncResult> asyncResults = this.ModuleManager
                    .GetModules<ServantModule>()
                    .Select(s => s.BeginStart(
                        r => (r.AsyncState as ServantModule).EndStart(r), s
                    ));
                WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
            }
        }

        /// <summary>
        /// 登録されているサーバント オブジェクトをすべて停止させます。
        /// </summary>
        private void StopServants()
        {
            if (this.ModuleManager.GetModules<ServantModule>().Any())
            {
                IEnumerable<IAsyncResult> asyncResults = this.ModuleManager
                    .GetModules<ServantModule>()
                    .Select(s => s.BeginStop(
                        r => (r.AsyncState as ServantModule).EndStop(r), s
                    ));
                WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
            }
        }

        /// <summary>
        /// 登録されているサーバント オブジェクトをすべて強制停止させます。
        /// </summary>
        private void AbortServants()
        {
            if (this.ModuleManager.GetModules<ServantModule>().Any())
            {
                IEnumerable<IAsyncResult> asyncResults = this.ModuleManager
                    .GetModules<ServantModule>()
                    .Select(s => s.BeginAbort(
                        r => (r.AsyncState as ServantModule).EndAbort(r), s
                    ));
                WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
            }
        }

        /// <summary>
        /// サーバ オブジェクトに対し要求を発行します。
        /// </summary>
        /// <typeparam name="T">要求の結果の型。</typeparam>
        /// <param name="request">要求の内容を表す文字列。</param>
        /// <returns>要求の結果のデータ。</returns>
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
            String flow = "sys";   // Default Module
            Int32 index = 0;

            // a) .../$storage!module/... -> storage!module/...
            // b) .../$storage!/...       -> storage!/...
            // c) .../!module/...         -> module/...
            // d) .../!/...               -> /...
            foreach (String elem in units)
            {
                String prefixes = elem.Substring(0, elem.IndexOf('/'));
                if (prefixes.Contains("!"))
                {
                    if (!prefixes.EndsWith("!")) // a) Specified Storage and Module
                    {
                        String[] prefixArray = prefixes.Split('!');
                        storage = prefixArray[0];
                        flow = prefixArray[1];
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
                        flow = prefixes;
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

                StorageModule storageModule = this.ModuleManager.GetModule<StorageModule>(storage);

                if (index == 0) // Invoking InputFlowModule
                {
                    results = this.ModuleManager.GetModule<InputFlowModule>(flow).Input(
                        selector,
                        storageModule,
                        argumentDictionary
                    );
                }
                else if (index != units.Count() - 1) // Invoking FilterFlowModule
                {
                    this.ModuleManager.GetModule<FilterFlowModule>(flow).Filter(
                        selector,
                        results,
                        storageModule,
                        argumentDictionary
                    );
                }
                else // Invoking OutputFlowModule
                {
                    return this.ModuleManager.GetModule<OutputFlowModule>(flow).Output<T>(
                        selector,
                        results,
                        storageModule,
                        argumentDictionary
                    );
                }

                storageModule.Update();
                ++index;
            }

            // Throws when not returned yet (it means Output module is not invoked.)
            throw new ArgumentException("uri");
        }
    }
}