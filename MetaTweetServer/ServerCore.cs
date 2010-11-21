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
using XSpect.MetaTweet.Properties;
using System.Diagnostics;
using XSpect.MetaTweet.Modules;
using Achiral;
using Achiral.Extension;
using XSpect.Extension;
using XSpect.MetaTweet.Requesting;

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
        /// イベントを記録するログ ライタを取得します。
        /// </summary>
        /// <value>イベントを記録するログ ライタ。</value>
        public Log Log
        {
            get
            {
                return this.LogManager[this.Configuration.Loggers.ServerCore];
            }
        }

        /// <summary>
        /// このサーバ オブジェクトが存在するアプリケーション ドメインを取得します。
        /// </summary>
        public AppDomain MainAppDomain
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
        /// MetaTweet システム全体の設定を保持するオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// MetaTweet システム全体の設定を保持するオブジェクト。
        /// </value>
        public dynamic GlobalConfiguration
        {
            get;
            private set;
        }

        /// <summary>
        /// MetaTweet サーバの設定を保持するオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// MetaTweet サーバの設定を保持するオブジェクト。
        /// </value>
        public dynamic Configuration
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
        /// このサーバ オブジェクトのリクエスト マネージャを取得します。
        /// </summary>
        /// <value>このサーバ オブジェクトのリクエスト マネージャ。</value>
        public RequestManager RequestManager
        {
            get;
            private set;
        }

        /// <summary>
        /// このサーバ オブジェクトのストアド リクエスト マネージャを取得します。
        /// </summary>
        /// <value>このサーバ オブジェクトのストアド リクエスト マネージャ。</value>
        public StoredRequestManager StoredRequestManager
        {
            get;
            private set;
        }

        /// <summary>
        /// このサーバ オブジェクトのログ マネージャを取得します。
        /// </summary>
        /// <value>
        /// このサーバ オブジェクトのログ マネージャ。
        /// </value>
        public LogManager LogManager
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
        /// MetaTweet システムのバージョン情報を表す文字列を取得します。
        /// </summary>
        /// <value>MetaTweet システムのバージョン情報を表す文字列。</value>
        /// <remarks>このプロパティはバージョン情報の取得の他に、クライアントがサーバとの接続の確立を確認するために用いられることが想定されています。</remarks>
        public String Version
        {
            get
            {
                return ThisAssembly.EntireVersionInfo;
            }
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
            if (this.RequestManager != null)
            {
                this.RequestManager.Dispose();
            }
            if (this.ModuleManager != null)
            {
                this.ModuleManager.Dispose();
            }
            this.Directories.RuntimeDirectory.File("MetaTweetServer.pid").Delete();
            this.Directories.RuntimeDirectory.File("MetaTweetServer.svcid").Delete();
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
        /// <param name="arguments">サーバ ホストからサーバ オブジェクトに渡す引数のリスト。</param>
        public void Initialize(IDictionary<String, String> arguments)
        {
            this.CheckIfDisposed();
            Debug.Assert(AppDomain.CurrentDomain.FriendlyName == "MetaTweetServer.dll");
            this.MainAppDomain = AppDomain.CurrentDomain;
            this.Parameters = arguments;
            // Determine *temporary* directory structure.
            this.Directories = new DirectoryStructure(this.Parameters["config"]);
            
            if (this.Parameters.Contains(Create.KeyValuePair("debug", "true")))
            {
                Debugger.Launch();
            }
            if (this.Parameters.ContainsKey("wait"))
            {
                Thread.Sleep(new TimeSpan(0, 0, Int32.Parse(this.Parameters["wait"])));
            }

            this.ModuleManager = new ModuleManager(
                this,
                this.Directories.ConfigDirectory.File("ModuleManager.conf.*"),
                this.Directories.ConfigDirectory.File("scripting.config")
            );

            this.GlobalConfiguration = this.ModuleManager.Execute(this.Directories.ConfigDirectory.File("MetaTweet.conf.*"), self => this);

            // Determine real directory structure from configuration.
            this.Directories = new DirectoryStructure(this.GlobalConfiguration.Directories);

            this.Configuration = this.ModuleManager.Execute(
                this.Directories.ConfigDirectory.File("MetaTweetServer.conf.*"),
                self => this
            );

            this.LogManager = new LogManager(this, this.Directories.ConfigDirectory.File("log4net.config"));

            if (this.Directories.RuntimeDirectory.File("MetaTweetServer.pid").Exists)
            {
                this.Log.Warn(Resources.ServerRuntimeFileRemains);
            }
            this.Directories.RuntimeDirectory.File("MetaTweetServer.pid").WriteAllText(arguments[".pid"]);
            this.Directories.RuntimeDirectory.File("MetaTweetServer.svcid").WriteAllText(arguments[".svc_id"]);

            this.Directories.TempDirectory.GetFiles().ForEach(f => f.Delete());

            this.RequestManager = new RequestManager(this);
            this.StoredRequestManager = new StoredRequestManager(this, this.Directories.ConfigDirectory.File("StoredRequestManager.conf.*"));
            Initializer.Initialize(this, this.Parameters);

            this.Directories.BaseDirectoryWatcher.EnableRaisingEvents = true;
            this.Directories.ConfigDirectoryWatcher.EnableRaisingEvents = true;
            this.Directories.TempDirectoryWatcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// サーバおよびサーバント オブジェクトを開始します。
        /// </summary>
        public void Start()
        {
            this.CheckIfDisposed();
            this.ModuleManager.GetModules<ServantModule>()
                .ForEach(s => s.Start());
        }

        /// <summary>
        /// サーバおよびサーバント オブジェクトを停止します。
        /// </summary>
        public void Stop()
        {
            this.CheckIfDisposed();
            this.ModuleManager.GetModules<ServantModule>()
                .ForEach(s => s.Abort());
        }

        /// <summary>
        /// サーバおよびサーバント オブジェクトを安全に停止します。
        /// </summary>
        public void StopGracefully()
        {
            this.CheckIfDisposed();
            this.ModuleManager.GetModules<ServantModule>()
                .ForEach(s => s.Stop());
        }
    }
}