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
          IDisposable,
          ILoggable
    {
        private readonly Mutex _mutex;

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
        /// 設定ファイルが配置されているディレクトリを取得します。
        /// </summary>
        /// <value>
        /// 設定ファイルが配置されているディレクトリ。
        /// </value>
        /// <remarks>
        /// 指定されているディレクトリが存在しない場合、新規に作成されます。
        /// </remarks>
        public DirectoryInfo ConfigDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// 一時ファイルが生成されるディレクトリを取得します。
        /// </summary>
        /// <value>
        /// 一時ファイルが生成されるディレクトリ。
        /// </value>
        /// <remarks>
        /// 指定されているディレクトリが存在しない場合、新規に作成されます。
        /// </remarks>
        public DirectoryInfo TempDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="RootDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="RootDirectory"/> を監視するコンポーネント。
        /// </value>
        public FileSystemWatcher RootDirectoryWatcher
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="ConfigDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="ConfigDirectory"/> を監視するコンポーネント。
        /// </value>
        public FileSystemWatcher ConfigDirectoryWatcher
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="TempDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="TempDirectory"/> を監視するコンポーネント。
        /// </value>
        public FileSystemWatcher TempDirectoryWatcher
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
        /// イベントを記録するログ ライタを取得します。
        /// </summary>
        /// <value>
        /// イベントを記録するログ ライタ。
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
            this._mutex = new Mutex();
            this.RootDirectory = new FileInfo(Assembly.GetEntryAssembly().Location).Directory;
            this.RootDirectoryWatcher = new FileSystemWatcher(this.RootDirectory.FullName);
            this.Log = LogManager.GetLogger(typeof(ServerCore));
            this.InitializeHook = new Hook<ServerCore>();
            this.StartHook = new Hook<ServerCore>();
            this.StopHook = new Hook<ServerCore>();
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
                self._mutex.Close();
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
                Debugger.Break();
            }
            if (this.Parameters.ContainsKey("wait"))
            {
                Thread.Sleep(new TimeSpan(0, 0, Int32.Parse(this.Parameters["wait"])));
            }

            this.InitializeDefaultLogHooks();

            this.InitializeHook.Execute(self =>
            {
                this.Configuration = this.Parameters.ContainsKey("config")
                    ? XmlConfiguration.Load(this.Parameters["config"])
                    // TODO: app.config - read key
                    : new XmlConfiguration();

                this.ConfigDirectory = this.RootDirectory.CreateSubdirectory(this.Configuration.GetValueOrDefault("configDirectory", "conf"));
                this.TempDirectory = this.RootDirectory.CreateSubdirectory(this.Configuration.GetValueOrDefault("tempDirectory", "temp"));
                this.ConfigDirectoryWatcher = new FileSystemWatcher(this.ConfigDirectory.FullName);
                this.TempDirectoryWatcher = new FileSystemWatcher(this.TempDirectory.FullName);
                
                this.ModuleManager = new ModuleManager(
                    this,
                    this.RootDirectory.CreateSubdirectory(this.Configuration.GetValueOrDefault("moduleDirectory", "module"))
                );

                this.TempDirectory.GetFileSystemInfos().ForEach(fso => fso.Delete());
                
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

            this.RootDirectoryWatcher.EnableRaisingEvents = true;
            this.ConfigDirectoryWatcher.EnableRaisingEvents = true;
            this.TempDirectoryWatcher.EnableRaisingEvents = true;
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
        /// <param name="request">発行する要求。</param>
        /// <returns>要求の結果のデータ。</returns>
        /// <see cref="T:Request"/>
        public T Request<T>(Request request)
        {
            try
            {
                this._mutex.WaitOne();
                Int32 index = 0;
                IEnumerable<StorageObject> results = null;

                foreach (Request req in request)
                {
                    StorageModule storageModule = this.ModuleManager.GetModule<StorageModule>(req.StorageName);

                    if (index == 0) // Invoking InputFlowModule
                    {
                        results = this.ModuleManager.GetModule<InputFlowModule>(req.FlowName).Input(
                            req.Selector,
                            storageModule,
                            req.Arguments
                        );
                    }
                    else if (index != request.Count() - 1) // Invoking FilterFlowModule
                    {
                        this.ModuleManager.GetModule<FilterFlowModule>(req.FlowName).Filter(
                            req.Selector,
                            results,
                            storageModule,
                            req.Arguments
                        );
                    }
                    else // Invoking OutputFlowModule
                    {
                        return this.ModuleManager.GetModule<OutputFlowModule>(req.FlowName).Output<T>(
                            req.Selector,
                            results,
                            storageModule,
                            req.Arguments
                        );
                    }

                    storageModule.Update();
                    ++index;
                }
                // Throws when not returned yet (it means Output module is not invoked.)
                throw new ArgumentException("uri");
            }
            finally
            {
                this._mutex.ReleaseMutex();
            }
        }
    }
}