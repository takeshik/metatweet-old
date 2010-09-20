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
using System.Runtime.Remoting;
using XSpect.Configuration;
using XSpect.Extension;
using log4net;
using XSpect.Hooking;

namespace XSpect.MetaTweet.Modules
{
    /// <summary>
    /// サーバント モジュールの抽象基本クラスを提供します。
    /// </summary>
    /// <remarks>
    /// サーバント モジュールとは、開始および停止状態を遷移させる機能を持つモジュールです。サーバ オブジェクトの状態遷移に連動して、または独立して操作されます。
    /// </remarks>
    [Serializable()]
    public abstract class ServantModule
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

        public ModuleDomain Domain
        {
            get;
            private set;
        }

        /// <summary>
        /// このモジュールに設定された名前を取得します。
        /// </summary>
        /// <value>このモジュールに設定された名前を取得します。</value>
        public String Name
        {
            get;
            private set;
        }

        public IList<String> Options
        {
            get;
            private set;
        }

        /// <summary>
        /// このモジュールの設定を管理するオブジェクトを取得します。
        /// </summary>
        /// <value>このモジュールの設定を管理するオブジェクト。</value>
        public XmlConfiguration Configuration
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
        public Log Log
        {
            get
            {
                return this.Host.Log;
            }
        }

        /// <summary>
        /// このサーバント モジュールが開始状態にあるかどうかを表す値を取得します。
        /// </summary>
        /// <value>このサーバント モジュールが開始状態にある場合は <c>true</c>。それ以外の場合は <c>false</c>。</value>
        public Boolean IsStarted
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
        /// <see cref="Configure(XmlConfiguration)"/> のフック リストを取得します。
        /// </summary>
        /// <value><see cref="Configure(XmlConfiguration)"/> のフック リスト。</value>
        public ActionHook<IModule, XmlConfiguration> ConfigureHook
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
        /// <see cref="Start"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="Start"/> のフック リスト。
        /// </value>
        public ActionHook<ServantModule> StartHook
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
        public ActionHook<ServantModule> StopHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="Abort"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="Abort"/> のフック リスト。
        /// </value>
        public ActionHook<ServantModule> AbortHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="ServantModule"/> の新しいインスタンスを初期化します。
        /// </summary>
        protected ServantModule()
        {
            this.InitializeHook = new ActionHook<IModule>(this.InitializeImpl);
            this.ConfigureHook = new ActionHook<IModule, XmlConfiguration>(c => this.ConfigureImpl());
            this.DisposeHook = new ActionHook<IModule>(this._Dispose);
            this.StartHook = new ActionHook<ServantModule>(this.StartImpl);
            this.StopHook = new ActionHook<ServantModule>(this.StopImpl);
            this.AbortHook = new ActionHook<ServantModule>(this.AbortImpl);
        }

        /// <summary>
        /// <see cref="ServantModule"/> がガベージ コレクションによってクリアされる前に、アンマネージ リソースを解放し、その他のクリーンアップ操作を実行します。
        /// </summary>
        ~ServantModule()
        {
            this.Dispose(false);
        }

        public override String ToString()
        {
            return this.GetType().Name + "-" + this.Name;
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
        /// このモジュールをサーバ オブジェクトに登録します。
        /// </summary>
        /// <param name="host">登録されるサーバ オブジェクト。</param>
        /// <param name="name">モジュールに設定する名前。</param>
        /// <param name="configuration">モジュールが参照する設定。</param>
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
        /// 派生クラスで実装された場合、実際の初期化処理を行います。
        /// </summary>
        protected virtual void InitializeImpl()
        {
        }

        public void Configure(XmlConfiguration configuration)
        {
            this.Configuration = configuration;
            this.ConfigureHook.Execute(configuration);
        }

        protected virtual void ConfigureImpl()
        {
        }

        /// <summary>
        ///<see cref="ServantModule"/> によって使用されているすべてのリソースを解放します。
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
        /// <see cref="ServantModule"/> によって使用されているアンマネージ リソースを解放し、オプションでマネージ リソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 <c>true</c>、破棄されない場合は <c>false</c>。</param>
        protected virtual void Dispose(Boolean disposing)
        {
            // TODO: Ensure to stop servants?
            this._disposed = true;
        }

        public ObjRef CreateObjRef()
        {
            return this.Domain.DoCallback(() => this.CreateObjRef(this.GetType()));
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
        /// このサーバント モジュールを開始します。
        /// </summary>
        public void Start()
        {
            if (!this.IsStarted)
            {
                this.CheckIfDisposed();
                this.StartHook.Execute();
                this.IsStarted = true;
            }
        }

        /// <summary>
        /// 派生クラスで実装された場合、実際の開始処理を行います。
        /// </summary>
        protected abstract void StartImpl();

        /// <summary>
        /// このサーバント モジュールを停止します。
        /// </summary>
        public void Stop()
        {
            if (this.IsStarted)
            {
                this.CheckIfDisposed();
                this.StopHook.Execute();
                this.IsStarted = false;
            }
        }

        /// <summary>
        /// 派生クラスで実装された場合、実際の停止処理を行います。
        /// </summary>
        protected abstract void StopImpl();

        /// <summary>
        /// このサーバント モジュールを強制的に停止します。
        /// </summary>
        public void Abort()
        {
            if (this.IsStarted)
            {
                this.CheckIfDisposed();
                this.AbortHook.Execute();
                this.IsStarted = false;
            }
        }

        /// <summary>
        /// 派生クラスで実装された場合、実際の強制停止処理を行います。
        /// </summary>
        /// <remarks>
        /// 既定では、<see cref="StopImpl"/> を呼び出します。つまり、<see cref="Stop"/> と同じコードを実行します。
        /// </remarks>
        protected virtual void AbortImpl()
        {
            this.StopImpl();
        }

        /// <summary>
        /// 非同期の開始処理を開始します。
        /// </summary>
        /// <param name="callback">開始処理完了時に呼び出されるオプションの非同期コールバック。</param>
        /// <param name="state">この特定の非同期開始処理要求を他の要求と区別するために使用するユーザー指定のオブジェクト。</param>
        /// <returns>非同期の開始処理を表す <see cref="System.IAsyncResult"/>。まだ保留状態の場合もあります。</returns>
        public IAsyncResult BeginStart(AsyncCallback callback, Object state)
        {
            return new Action(this.Start).BeginInvoke(callback, state);
        }

        /// <summary>
        /// 保留中の非同期開始処理が完了するまで待機します。
        /// </summary>
        /// <param name="asyncResult">終了させる保留状態の非同期リクエストへの参照。</param>
        public void EndStart(IAsyncResult asyncResult)
        {
            asyncResult.GetAsyncDelegate<Action>().EndInvoke(asyncResult);
        }

        /// <summary>
        /// 非同期の停止処理を開始します。
        /// </summary>
        /// <param name="callback">停止処理完了時に呼び出されるオプションの非同期コールバック。</param>
        /// <param name="state">この特定の非同期停止処理要求を他の要求と区別するために使用するユーザー指定のオブジェクト。</param>
        /// <returns>非同期の停止処理を表す <see cref="System.IAsyncResult"/>。まだ保留状態の場合もあります。</returns>
        public IAsyncResult BeginStop(AsyncCallback callback, Object state)
        {
            return new Action(this.Stop).BeginInvoke(callback, state);
        }

        /// <summary>
        /// 保留中の非同期停止処理が完了するまで待機します。
        /// </summary>
        /// <param name="asyncResult">終了させる保留状態の非同期リクエストへの参照。</param>
        public void EndStop(IAsyncResult asyncResult)
        {
            asyncResult.GetAsyncDelegate<Action>().EndInvoke(asyncResult);
        }

        /// <summary>
        /// 非同期の強制停止処理を開始します。
        /// </summary>
        /// <param name="callback">強制停止処理完了時に呼び出されるオプションの非同期コールバック。</param>
        /// <param name="state">この特定の非同期強制停止処理要求を他の要求と区別するために使用するユーザー指定のオブジェクト。</param>
        /// <returns>非同期の強制停止処理を表す <see cref="System.IAsyncResult"/>。まだ保留状態の場合もあります。</returns>
        public IAsyncResult BeginAbort(AsyncCallback callback, Object state)
        {
            return new Action(this.Abort).BeginInvoke(callback, state);
        }

        /// <summary>
        /// 保留中の非同期強制停止処理が完了するまで待機します。
        /// </summary>
        /// <param name="asyncResult">終了させる保留状態の非同期リクエストへの参照。</param>
        public void EndAbort(IAsyncResult asyncResult)
        {
            asyncResult.GetAsyncDelegate<Action>().EndInvoke(asyncResult);
        }
    }
}
