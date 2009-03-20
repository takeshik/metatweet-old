﻿// -*- mode: csharp; encoding: utf-8; -*-
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
using System.Runtime.Remoting.Messaging;
using System.Collections.Generic;
using XSpect.Configuration;
using log4net;

namespace XSpect.MetaTweet.Modules
{
    /// <summary>
    /// サーバント モジュールの抽象基本クラスを提供します。
    /// </summary>
    /// <remarks>
    /// サーバント モジュールとは、開始、停止、一時停止、および再開などの状態を遷移させる機能を持つモジュールです。サーバ オブジェクトの状態遷移に連動して、または独立して操作されます。
    /// </remarks>
    public abstract class ServantModule
        : MarshalByRefObject,
          IModule
    {
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
        /// このモジュールに設定された名前を取得します。
        /// </summary>
        /// <value>このモジュールに設定された名前を取得します。</value>
        public String Name
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
        public ILog Log
        {
            get
            {
                return this.Host.Log;
            }
        }

        /// <summary>
        /// <see cref="Start"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="Start"/> のフック リスト。
        /// </value>
        public Hook<ServantModule> StartHook
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
        public Hook<ServantModule> StopHook
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
        public Hook<ServantModule> AbortHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="ServantModule"/> の新しいインスタンスを初期化します。
        /// </summary>
        public ServantModule()
        {
            this.StartHook = new Hook<ServantModule>();
            this.StopHook = new Hook<ServantModule>();
            this.AbortHook = new Hook<ServantModule>();
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
        public virtual void Register(ServerCore host, String name)
        {
            this.Host = host;
            this.Name = name;
        }

        /// <summary>
        /// このモジュールに設定を適用し、初期化を行います。
        /// </summary>
        /// <param name="configuration">適用する設定。</param>
        public void Initialize(XmlConfiguration configuration)
        {
            this.Configuration = configuration;
            this.Initialize();
        }

        /// <summary>
        /// このモジュールを初期化します。
        /// </summary>
        /// <remarks>
        /// このメソッドはモジュールの寿命中、複数回呼び出される可能性があります。
        /// </remarks>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// <see cref="ServantModule"/> によって使用されているすべてのリソースを解放します。
        /// </summary>
        public virtual void Dispose()
        {
            this.Abort();
        }

        /// <summary>
        /// このサーバント モジュールを開始します。
        /// </summary>
        public void Start()
        {
            this.StartHook.Execute(self =>
            {
                self.StartImpl();
            }, this);
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
            this.StopHook.Execute(self =>
            {
                self.StopImpl();
            }, this);
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
            this.AbortHook.Execute(self =>
            {
                self.AbortImpl();
            }, this);
        }

        /// <summary>
        /// 派生クラスで実装された場合、実際の強制停止処理を行います。
        /// </summary>
        /// <remarks>
        /// 規定では、<see cref="StopImpl"/> を呼び出します。つまり、<see cref="Stop"/> と同じコードを実行します。
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
            ((asyncResult as AsyncResult).AsyncDelegate as Action).EndInvoke(asyncResult);
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
            ((asyncResult as AsyncResult).AsyncDelegate as Action).EndInvoke(asyncResult);
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
            ((asyncResult as AsyncResult).AsyncDelegate as Action).EndInvoke(asyncResult);
        }
    }
}
