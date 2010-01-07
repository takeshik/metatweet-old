// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
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
using Microsoft.Scripting.Hosting;
using XSpect.Hooking;
using XSpect.MetaTweet.Properties;
using XSpect.MetaTweet.Objects;
using System.Diagnostics;
using XSpect.MetaTweet.Modules;
using Achiral;
using Achiral.Extension;
using XSpect.Configuration;
using XSpect.Extension;
using XSpect.Reflection;

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
        /// MetaTweet システム全体の設定を管理するオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// MetaTweet システム全体の設定を管理するオブジェクト。
        /// </value>
        public XmlConfiguration GlobalConfiguration
        {
            get;
            private set;
        }

        /// <summary>
        /// MetaTweet サーバの設定を管理するオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// MetaTweet サーバの設定を管理するオブジェクト。
        /// </value>
        public XmlConfiguration Configuration
        {
            get;
            private set;
        }

        /// <summary>
        /// MetaTweet システムの特別なディレクトリを取得するためのオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// MetaTweet システムの特別なディレクトリを取得するためのオブジェクト。
        /// </value>
        public DirectoryStructure Directories
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
        public ActionHook<ServerCore> InitializeHook
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
        public ActionHook<ServerCore> StartHook
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
        public ActionHook<ServerCore> StopHook
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
        public ActionHook<ServerCore> TerminateHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="Request{T}(Request)"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="Request{T}(Request)"/> のフック リスト。
        /// </value>
        public FuncHook<ServerCore, Request, Type, Object> RequestHook
        {
            get;
            private set;
        }

        /// <summary>
        /// 外部のコードを実行する際に与える既定のパラメータを取得します。
        /// </summary>
        /// <value>外部のコードを実行する際に与える既定のパラメータ。</value>
        public IDictionary<String, Object> DefaultArgumentDictionary
        {
            get
            {
                return Create.Dictionary(
                    Make.Array("host", "args"),
                    new Object[]
                    {
                        this,
                        this.Parameters,
                    }
                );
            }
        }

        /// <summary>
        /// <see cref="ServerCore"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public ServerCore()
        {
            this.InitializeHook = new ActionHook<ServerCore>(this._Initialize);
            this.StartHook = new ActionHook<ServerCore>(this.StartServants);
            this.StopHook = new ActionHook<ServerCore>(this.AbortServants);
            this.TerminateHook = new ActionHook<ServerCore>(this._Terminate);
            this.RequestHook = new FuncHook<ServerCore, Request, Type, Object>(this._Request);
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
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// <see cref="ServerCore"/> によって使用されているアンマネージ リソースを解放し、オプションでマネージ リソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 <c>true</c>、破棄されない場合は <c>false</c>。</param>
        private void Dispose(Boolean disposing)
        {
            this.TerminateHook.Execute();
            this.Directories.RuntimeDirectory.File("MetaTweetServer.pid").Delete();
            this.Directories.RuntimeDirectory.File("MetaTweetServer.svcid").Delete();
            this._disposed = true;
        }

        private void _Terminate()
        {
            this.ModuleManager.Dispose();
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
        /// <param name="arguments">サーバ ホストからサーバ オブジェクトに渡す引数のリスト。</param>
        public void Initialize(IDictionary<String, String> arguments)
        {
            this.CheckIfDisposed();
            this.Parameters = arguments;
            
            if (this.Parameters.Contains(Create.KeyValuePair("debug", "true")))
            {
                Debugger.Launch();
            }
            if (this.Parameters.ContainsKey("wait"))
            {
                Thread.Sleep(new TimeSpan(0, 0, Int32.Parse(this.Parameters["wait"])));
            }

            this.GlobalConfiguration = XmlConfiguration.Load(this.Parameters["config"]);

            this.Configuration = XmlConfiguration.Load(
                this.GlobalConfiguration.ConfigurationFile.Directory.File("MetaTweetServer.conf.xml")
            );

            this.Directories = new DirectoryStructure(this.GlobalConfiguration.ResolveChild("directories"));

            this.Log = Loggable.Initialize<ServerCore>(this.Directories.ConfigDirectory.File("log4net.config"));

            if (this.Directories.RuntimeDirectory.File("MetaTweetServer.pid").Exists)
            {
                this.Log.Warn(Resources.ServerRuntimeFileRemains);
            }
            this.Directories.RuntimeDirectory.File("MetaTweetServer.pid").WriteAllText(arguments[".pid"]);
            this.Directories.RuntimeDirectory.File("MetaTweetServer.svcid").WriteAllText(arguments[".svc_id"]);

            this.InitializeDefaultLogHooks();

            this.InitializeHook.Execute();

            this.Directories.BaseDirectoryWatcher.EnableRaisingEvents = true;
            this.Directories.ConfigDirectoryWatcher.EnableRaisingEvents = true;
            this.Directories.TempDirectoryWatcher.EnableRaisingEvents = true;
        }

        private void _Initialize()
        {
            this.Directories.TempDirectory.GetFiles().ForEach(f => f.Delete());

            this.ModuleManager = new ModuleManager(this, this.Directories.ConfigDirectory.File("ModuleManager.conf.xml"));

            FileInfo initFile = this.Configuration.ResolveValue<String>("initializerPath")
                .Do(s => s.IsNullOrEmpty()
                    ? null
                    : this.Directories.ConfigDirectory.File(s)
                );
            if (initFile != null)
            {
                this.ModuleManager.Execute<Object>(initFile, this.DefaultArgumentDictionary);
            }
            else
            {
                Initializer.Initialize(this, this.Parameters);
            }
        }

        private void InitializeDefaultLogHooks()
        {
            this.InitializeHook.Before.Add(self => self.Log.WarnFormat(
                Resources.ServerInitializing,
                ThisAssembly.EntireVersion,
                ThisAssembly.Branch,
                ThisAssembly.EntireCommitId,
                ThisAssembly.EntireCommitterName,
                ThisAssembly.EntireCommitterEmail,
                ThisAssembly.EntireCommittedAt,
                Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                Environment.OSVersion.ToString(),
                Thread.CurrentThread.CurrentCulture.ToString()
                    .If(s => s.IsNullOrEmpty(), "invaliant")
            ));
            this.InitializeHook.Succeeded.Add(self => self.Log.Info(Resources.ServerInitialized));
            this.StartHook.Before.Add(self => self.Log.Info(Resources.ServerStarting));
            this.StartHook.Succeeded.Add(self => self.Log.Info(Resources.ServerStarted));
            this.StopHook.Before.Add(self => self.Log.Info(Resources.ServerStopping));
            this.StopHook.Succeeded.Add(self => self.Log.Info(Resources.ServerStopped));
            this.TerminateHook.Before.Add(self => self.Log.Info(Resources.ServerTerminating));
            this.TerminateHook.Succeeded.Add(self => self.Log.Info(Resources.ServerTerminated));
            this.RequestHook.Before.Add((self, req, type) => self.Log.Info(String.Format(
                Resources.ServerRequestExecuting,
                req.ToString()
            )));
            this.RequestHook.Succeeded.Add((self, req, type, ret) => self.Log.Info(String.Format(
                Resources.ServerRequestExecuted,
                req.ToString()
            )));
        }

        /// <summary>
        /// サーバおよびサーバント オブジェクトを開始します。
        /// </summary>
        public void Start()
        {
            this.CheckIfDisposed();
            this.StartHook.Execute();
        }

        /// <summary>
        /// サーバおよびサーバント オブジェクトを停止します。
        /// </summary>
        public void Stop()
        {
            this.CheckIfDisposed();
            this.StopHook.Execute();
        }

        /// <summary>
        /// サーバおよびサーバント オブジェクトを安全に停止します。
        /// </summary>
        public void StopGracefully()
        {
            this.CheckIfDisposed();
            this.StopHook.Execute();
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
            return (T) this.Request(request, typeof(T));
        }

        /// <summary>
        /// サーバ オブジェクトに対し要求を発行します。
        /// </summary>
        /// <param name="request">発行する要求。</param>
        /// <param name="outputType">要求の結果の型。</param>
        /// <returns>要求の結果のデータ。</returns>
        /// <see cref="T:Request"/>
        public Object Request(Request request, Type outputType)
        {
            this.CheckIfDisposed();
            return this.RequestHook.Execute(request, outputType);
        }

        private Object _Request(Request request, Type outputType)
        {
            Int32 index = 0;
            IEnumerable<StorageObject> results = null;

            foreach (Request req in request)
            {
                StorageModule storageModule = this.ModuleManager.GetModule<StorageModule>(req.StorageName);

                if (index == 0) // Invoking InputFlowModule
                {
                    InputFlowModule flowModule = this.ModuleManager.GetModule<InputFlowModule>(req.FlowName);
                    results = flowModule.Input(
                        req.Selector,
                        storageModule,
                        req.Arguments
                    );
                }
                else if (index != request.Count() - 1) // Invoking FilterFlowModule
                {
                    FilterFlowModule flowModule = this.ModuleManager.GetModule<FilterFlowModule>(req.FlowName);

                    flowModule.Filter(
                        req.Selector,
                        results,
                        storageModule,
                        req.Arguments
                    );
                }
                else // Invoking OutputFlowModule (End of flow)
                {
                    OutputFlowModule flowModule = this.ModuleManager.GetModule<OutputFlowModule>(req.FlowName);

                    return flowModule.Output(
                        req.Selector,
                        results,
                        storageModule,
                        req.Arguments,
                        outputType
                    );
                }

                ++index;
            }
            // Whether the process is not finished:
            return typeof(IEnumerable<StorageObject>).IsAssignableFrom(outputType)
                ? results
                : null;
        }
    }
}