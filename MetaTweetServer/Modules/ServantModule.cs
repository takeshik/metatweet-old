﻿// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
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
using XSpect.Extension;
using XSpect.MetaTweet.Properties;

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
        : Module
    {
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
        /// このサーバント モジュールを開始します。
        /// </summary>
        public void Start()
        {
            if (!this.IsStarted)
            {
                this.CheckIfDisposed();
                this.Log.Info(Resources.ServantStarting, this.Name);
                this.StartImpl();
                this.IsStarted = true;
                this.Log.Info(Resources.ServantStarted, this.Name);
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
                this.Log.Info(Resources.ServantStopping, this.Name);
                this.StopImpl();
                this.IsStarted = false;
                this.Log.Info(Resources.ServantStopped, this.Name);
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
                this.Log.Info(Resources.ServantAborting, this.Name);
                this.AbortImpl();
                this.IsStarted = false;
                this.Log.Info(Resources.ServantAborted, this.Name);
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
