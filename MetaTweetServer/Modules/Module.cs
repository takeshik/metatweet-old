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
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Remoting;
using XSpect.Extension;
using XSpect.Hooking;

namespace XSpect.MetaTweet.Modules
{
    [Serializable()]
    public abstract class Module
        : MarshalByRefObject,
          IModule
    {
        private Boolean _disposed;

        /// <summary>
        /// このモジュールがホストされているサーバ オブジェクトを取得します。
        /// </summary>
        /// <value>このモジュールがホストされているサーバ オブジェクト。</value>
        public ServerCore Host
        {
            get;
            private set;
        }

        /// <summary>
        /// このモジュールが生成されたドメインを取得します。
        /// </summary>
        /// <value>
        /// このモジュールが生成されたドメイン。
        /// </value>
        public ModuleDomain Domain
        {
            get;
            private set;
        }

        /// <summary>
        /// このモジュールに設定された名前を取得します。
        /// </summary>
        /// <value>このモジュールに設定された名前。</value>
        public String Name
        {
            get;
            private set;
        }

        /// <summary>
        /// このモジュールに渡されたオプションのリストを取得します。
        /// </summary>
        /// <value>
        /// このモジュールに渡されたオプションのリスト。
        /// </value>
        public IList<String> Options
        {
            get;
            private set;
        }

        /// <summary>
        /// このモジュールの設定を保持するオブジェクトを取得します。
        /// </summary>
        /// <value>このモジュールの設定を保持するオブジェクト。</value>
        public dynamic Configuration
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="Initialize()"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="Initialize()"/> のフック リスト。
        /// </value>
        public ActionHook<IModule> InitializeHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="Configure(FileInfo)"/> のフック リストを取得します。
        /// </summary>
        /// <value><see cref="Configure(FileInfo)"/> のフック リスト。</value>
        public ActionHook<IModule, FileInfo> ConfigureHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="Dispose()"/> のフック リストを取得します。
        /// </summary>
        /// <value><see cref="Dispose()"/> のフック リスト。</value>
        public ActionHook<IModule> DisposeHook
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
        public virtual Log Log
        {
            get
            {
                return GetLogImpl(this);
            }
        }

        /// <summary>
        /// <see cref="Module"/> の新しいインスタンスを初期化します。
        /// </summary>
        protected Module()
        {
            this.InitializeHook = new ActionHook<IModule>(this.InitializeImpl);
            this.ConfigureHook = new ActionHook<IModule, FileInfo>(this.ConfigureImpl);
            this.DisposeHook = new ActionHook<IModule>(this._Dispose);
        }

        /// <summary>
        /// <see cref="Module"/> がガベージ コレクションによってクリアされる前に、アンマネージ リソースを解放し、その他のクリーンアップ操作を実行します。
        /// </summary>
        ~Module()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// このモジュールを表す文字列を返します。
        /// </summary>
        /// <returns>このモジュールを表す文字列。</returns>
        public override String ToString()
        {
            return ToStringImpl(this);
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
        /// <see cref="FlowModule"/> によって使用されているすべてのリソースを解放します。
        /// </summary>
        public void Dispose()
        {
            this.DisposeHook.Execute();
        }

        private void _Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// <see cref="FlowModule"/> によって使用されているアンマネージ リソースを解放し、オプションでマネージ リソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 <c>true</c>、破棄されない場合は <c>false</c>。</param>
        protected virtual void Dispose(Boolean disposing)
        {
            this._disposed = true;
        }

        /// <summary>
        /// オブジェクトが破棄されているかどうかを確認し、破棄されている場合は例外を送出します。
        /// </summary>
        protected void CheckIfDisposed()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }

        /// <summary>
        /// このモジュールをサーバ オブジェクトに登録します。
        /// </summary>
        /// <param name="domain">登録されるモジュール ドメイン。</param>
        /// <param name="name">モジュールに設定する名前。</param>
        /// <param name="options">モジュールに渡されたオプションのリスト。</param>
        public virtual void Register(ModuleDomain domain, String name, IList<String> options)
        {
            this.Domain = domain;
            this.Host = domain.Parent.Parent;
            this.Name = name;
            this.Options = options;
        }

        /// <summary>
        /// このモジュールを初期化します。
        /// </summary>
        /// <remarks>
        /// このメソッドはモジュールの寿命中、複数回呼び出される可能性があります。
        /// </remarks>
        public void Initialize()
        {
            this.InitializeHook.Execute();
        }

        /// <summary>
        /// このモジュールの設定を行います。
        /// </summary>
        /// <param name="configFile">設定ファイル。</param>
        public void Configure(FileInfo configFile)
        {
            this.ConfigureHook.Execute(configFile);
        }

        /// <summary>
        /// 派生クラスで実装された場合、実際の設定処理を行います。
        /// </summary>
        /// <param name="configFile">設定ファイル。</param>
        protected virtual void ConfigureImpl(FileInfo configFile)
        {
            this.Configuration = this.Host.ModuleManager.Execute(configFile, self => this, host => this.Host);
        }

        /// <summary>
        /// リモート オブジェクトとの通信に使用するプロキシの生成に必要な情報をすべて格納しているオブジェクトを作成します。
        /// </summary>
        /// <returns>プロキシを生成するのに必要な情報。</returns>
        public ObjRef CreateObjRef()
        {
            return this.Domain.DoCallback(() => this.CreateObjRef(this.GetType()));
        }

        /// <summary>
        /// 派生クラスで実装された場合、実際の初期化処理を行います。
        /// </summary>
        protected virtual void InitializeImpl()
        {
        }

        #region Implementations

        internal static Log GetLogImpl(IModule module)
        {
            return module.Host.LogManager[module.Options.SingleOrDefault(s => s.StartsWith("log=")).If(
                s => s != null, s => s.Substring(4 /* "log=" */), _ => "module"
            )];
        }

        internal static String ToStringImpl(IModule module)
        {
            return module.GetType().Name + "-" + module.Name;
        }

        #endregion
    }
}