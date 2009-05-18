// -*- mode: csharp; encoding: utf-8; -*-
// $Id$
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
using log4net.Config;
using XSpect.MetaTweet.Properties;
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
    [Serializable()]
    public sealed class ServerCore
        : MarshalByRefObject,
          IDisposable,
          ILoggable
    {
        private Boolean _disposed;

        /// <summary>
        /// MetaTweet サーバのベースディレクトリを取得します。
        /// </summary>
        /// <value>
        /// MetaTweet サーバのベースディレクトリ。
        /// </value>
        public DirectoryInfo BaseDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// システムの、またはシステム全体で使用される実行ファイルが配置されているディレクトリを取得します。
        /// </summary>
        /// <value>
        /// システムの、またはシステム全体で使用される実行ファイルが配置されているディレクトリ。
        /// </value>
        /// <remarks>
        /// 指定されているディレクトリが存在しない場合、新規に作成されます。
        /// </remarks>
        public DirectoryInfo BinaryDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// キャッシュ ファイルが生成されるディレクトリを取得します。
        /// </summary>
        /// <value>
        /// キャッシュ ファイルが生成されるディレクトリ。
        /// </value>
        /// <remarks>
        /// 指定されているディレクトリが存在しない場合、新規に作成されます。
        /// </remarks>
        public DirectoryInfo CacheDirectory
        {
            get;
            set;
        }

        /// <summary>
        /// システムの、またはシステム全体で使用される設定ファイルが配置されているディレクトリを取得します。
        /// </summary>
        /// <value>
        /// システムの、またはシステム全体で使用される設定ファイルが配置されているディレクトリ。
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
        /// システムの、またはシステム全体で使用されるライブラリが配置されているディレクトリを取得します。
        /// </summary>
        /// <value>
        /// システムの、またはシステム全体で使用されるライブラリが配置されているディレクトリ。
        /// </value>
        /// <remarks>
        /// 指定されているディレクトリが存在しない場合、新規に作成されます。
        /// </remarks>
        public DirectoryInfo LibraryDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// ログ ファイルが生成されるディレクトリを取得します。
        /// </summary>
        /// <value>
        /// ログ ファイルが生成されるディレクトリ。
        /// </value>
        /// <remarks>
        /// 指定されているディレクトリが存在しない場合、新規に作成されます。
        /// </remarks>
        public DirectoryInfo LogDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// モジュールおよびモジュールの使用する各種ファイルが配置されているディレクトリを取得します。
        /// </summary>
        /// <value>
        /// モジュールおよびモジュールの使用する各種ファイルが配置されているディレクトリ。
        /// </value>
        /// <remarks>
        /// 指定されているディレクトリが存在しない場合、新規に作成されます。
        /// </remarks>
        public DirectoryInfo ModuleDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// システムの状態を記録するファイルが生成されるディレクトリを取得します。
        /// </summary>
        /// <value>
        /// システムの状態を記録するファイルが生成されるディレクトリ。
        /// </value>
        /// <remarks>
        /// 指定されているディレクトリが存在しない場合、新規に作成されます。
        /// </remarks>
        public DirectoryInfo RuntimeDirectory
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
        /// <see cref="BaseDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="BaseDirectory"/> を監視するコンポーネント。
        /// </value>
        public FileSystemWatcher BaseDirectoryWatcher
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="BinaryDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="BinaryDirectory"/> を監視するコンポーネント。
        /// </value>
        public FileSystemWatcher BinaryDirectoryWatcher
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="CacheDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="CacheDirectory"/> を監視するコンポーネント。
        /// </value>
        public FileSystemWatcher CacheDirectoryWatcher
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
        /// <see cref="LibraryDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="LibraryDirectory"/> を監視するコンポーネント。
        /// </value>
        public FileSystemWatcher LibraryDirectoryWatcher
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="LogDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="LogDirectory"/> を監視するコンポーネント。
        /// </value>
        public FileSystemWatcher LogDirectoryWatcher
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="ModuleDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="ModuleDirectory"/> を監視するコンポーネント。
        /// </value>
        public FileSystemWatcher ModuleDirectoryWatcher
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="RuntimeDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="RuntimeDirectory"/> を監視するコンポーネント。
        /// </value>
        public FileSystemWatcher RuntimeDirectoryWatcher
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
        /// <see cref="Dispose(Boolean)"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="Dispose(Boolean)"/> のフック リスト。
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
        /// <see cref="Storage"/> によって使用されているすべてのリソースを解放します。
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// <see cref="Storage"/> によって使用されているアンマネージ リソースを解放し、オプションでマネージ リソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 <c>true</c>、破棄されない場合は <c>false</c>。</param>
        private void Dispose(Boolean disposing)
        {
            this.TerminateHook.Execute(
                self => self.ModuleManager.GetModules().ForEach(m => m.Dispose()),
                this
            );
            this._disposed = true;
        }

        /// <summary>
        /// オブジェクトが破棄されているかどうかを確認し、破棄されている場合は例外を送出します。
        /// </summary>
        private void CheckIfDisposed()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }

        /// <summary>
        /// サーバ オブジェクトを初期化します。
        /// </summary>
        /// <param name="arguments">サーバ オブジェクトに渡す引数のリスト。</param>
        public void Initialize(IDictionary<String, String> arguments)
        {
            this.CheckIfDisposed();
            this.Parameters = arguments;
            
            if (this.Parameters.ContainsKey("debug") && this.Parameters["debug"] == "true")
            {
                Debugger.Break();
            }
            if (this.Parameters.ContainsKey("wait"))
            {
                Thread.Sleep(new TimeSpan(0, 0, Int32.Parse(this.Parameters["wait"])));
            }

            this.Configuration = this.Parameters.ContainsKey("config")
                ? XmlConfiguration.Load(this.Parameters["config"])
                : new XmlConfiguration();

            this.BaseDirectory = new DirectoryInfo(Environment.CurrentDirectory);
            this.BinaryDirectory = this.BaseDirectory.CreateSubdirectory(
                this.Configuration.GetChild("directories").GetValueOrDefault("binary", "bin")
            );
            this.CacheDirectory = this.BaseDirectory.CreateSubdirectory(
                this.Configuration.GetChild("directories").GetValueOrDefault("cache", "var/cache")
            );
            this.ConfigDirectory = this.BaseDirectory.CreateSubdirectory(
                this.Configuration.GetChild("directories").GetValueOrDefault("config", "etc")
            );
            this.LibraryDirectory = this.BaseDirectory.CreateSubdirectory(
                this.Configuration.GetChild("directories").GetValueOrDefault("library", "lib")
            );
            this.LogDirectory = this.BaseDirectory.CreateSubdirectory(
                this.Configuration.GetChild("directories").GetValueOrDefault("log", "var/log")
            );
            this.ModuleDirectory = this.BaseDirectory.CreateSubdirectory(
                this.Configuration.GetChild("directories").GetValueOrDefault("module", "libexec")
            );
            this.RuntimeDirectory = this.BaseDirectory.CreateSubdirectory(
                this.Configuration.GetChild("directories").GetValueOrDefault("runtime", "var/run")
            );
            this.TempDirectory = this.BaseDirectory.CreateSubdirectory(
                this.Configuration.GetChild("directories").GetValueOrDefault("temp", "tmp")
            );

            XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(this.ConfigDirectory.FullName, "log4net.config")));

            this.BaseDirectoryWatcher = new FileSystemWatcher(this.BaseDirectory.FullName);
            this.BinaryDirectoryWatcher = new FileSystemWatcher(this.BinaryDirectory.FullName);
            this.CacheDirectoryWatcher = new FileSystemWatcher(this.CacheDirectory.FullName);
            this.ConfigDirectoryWatcher = new FileSystemWatcher(this.ConfigDirectory.FullName);
            this.LibraryDirectoryWatcher = new FileSystemWatcher(this.LibraryDirectory.FullName);
            this.LogDirectoryWatcher = new FileSystemWatcher(this.LogDirectory.FullName);
            this.ModuleDirectoryWatcher = new FileSystemWatcher(this.ModuleDirectory.FullName);
            this.RuntimeDirectoryWatcher = new FileSystemWatcher(this.RuntimeDirectory.FullName);
            this.TempDirectoryWatcher = new FileSystemWatcher(this.TempDirectory.FullName);

            this.InitializeDefaultLogHooks();

            this.InitializeHook.Execute(self =>
            {
                this.ModuleManager = new ModuleManager(this);

                this.TempDirectory.GetFileSystemInfos().ForEach(fso => fso.Delete());
                
                FileInfo initFile = this.ConfigDirectory.GetFiles("init.*").SingleOrDefault();
                if (initFile != null)
                {
                    this.ModuleManager.Execute(initFile);
                }
                else
                {
                    Initializer.Initialize(this, arguments);
                }
            }, this);

            this.BaseDirectoryWatcher.EnableRaisingEvents = true;
            this.ConfigDirectoryWatcher.EnableRaisingEvents = true;
            this.TempDirectoryWatcher.EnableRaisingEvents = true;

            if (this.Configuration.ConfigurationFile == null)
            {
                this.Configuration.Save(Path.Combine(this.ConfigDirectory.FullName, "MetaTweetServer.conf.xml"));
            }
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
            this.CheckIfDisposed();
            this.StartHook.Execute(self => self.StartServants(), this);
        }

        /// <summary>
        /// サーバおよびサーバント オブジェクトを停止します。
        /// </summary>
        public void Stop()
        {
            this.CheckIfDisposed();
            this.StopHook.Execute(self => self.AbortServants(), this);
        }

        /// <summary>
        /// サーバおよびサーバント オブジェクトを安全に停止します。
        /// </summary>
        public void StopGracefully()
        {
            this.CheckIfDisposed();
            this.StopHook.Execute(self => self.StopServants(), this);
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
            this.CheckIfDisposed();
            Int32 index = 0;
            IEnumerable<StorageObject> results = null;

            foreach (Request req in request)
            {
                StorageModule storageModule = this.ModuleManager.GetModule<StorageModule>(req.StorageName);

                if (index == 0) // Invoking InputFlowModule
                {
                    InputFlowModule flowModule = this.ModuleManager.GetModule<InputFlowModule>(req.FlowName);

                    if (req.Selector.StartsWith("@")) // Getting scalar value (End of flow)
                    {
                        return flowModule.GetScalar<T>(
                            req.Selector,
                            storageModule,
                            req.Arguments
                        );
                    }
                    else
                    {
                        results = flowModule.Input(
                            req.Selector,
                            storageModule,
                            req.Arguments
                        );
                    }
                }
                else if (index != request.Count() - 1) // Invoking FilterFlowModule
                {
                    FilterFlowModule flowModule = this.ModuleManager.GetModule<FilterFlowModule>(req.FlowName);

                    if (req.Selector.StartsWith("@")) // Getting scalar value (End of flow)
                    {
                        return flowModule.GetScalar<T>(
                            req.Selector,
                            storageModule,
                            req.Arguments
                        );
                    }
                    else
                    {
                        flowModule.Filter(
                            req.Selector,
                            results,
                            storageModule,
                            req.Arguments
                        );
                    }
                }
                else // Invoking OutputFlowModule (End of flow)
                {
                    OutputFlowModule flowModule = this.ModuleManager.GetModule<OutputFlowModule>(req.FlowName);

                    if (req.Selector.StartsWith("@")) // Getting scalar value (End of flow)
                    {
                        return flowModule.GetScalar<T>(
                            req.Selector,
                            storageModule,
                            req.Arguments
                        );
                    }
                    else
                    {
                        return flowModule.Output<T>(
                            req.Selector,
                            results,
                            storageModule,
                            req.Arguments
                        );
                    }
                }

                ++index;
            }
            // Whether the process is not finished:
            if (typeof(T) == typeof(IEnumerable<StorageObject>))
            {
                // Same as /!sys/.obj
                return (T) results;
            }
            else
            {
                // Same as /!sys/.null
                return default(T);
            }
        }
    }
}