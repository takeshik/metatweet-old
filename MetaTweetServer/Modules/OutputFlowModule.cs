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
using XSpect.Extension;
using XSpect.Hooking;
using XSpect.MetaTweet.Objects;

namespace XSpect.MetaTweet.Modules
{
    /// <summary>
    /// 出力フロー モジュールの抽象基本クラスを提供します。
    /// </summary>
    /// <remarks>
    /// 出力フロー モジュールとは、ストレージ オブジェクトを入力とし、任意の型および形式への最終的なフローの結果を出力する、パイプラインの末端に位置するフロー モジュールを指します。
    /// </remarks>
    public abstract class OutputFlowModule
        : FlowModule
    {
        /// <summary>
        /// <see cref="Output"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="Output"/> のフック リスト。
        /// </value>
        public FuncHook<OutputFlowModule, String, Object, StorageModule, IDictionary<String, String>, Type, Object> OutputHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="OutputFlowModule"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        protected OutputFlowModule()
        {
            this.OutputHook = new FuncHook<OutputFlowModule, String, Object, StorageModule, IDictionary<String, String>, Type, Object>(this._Output);
        }

        /// <summary>
        /// 出力処理を行います。
        /// </summary>
        /// <typeparam name="TOutput">出力されるデータの型。</typeparam>
        /// <param name="selector">モジュールに対し照合のために提示するセレクタ文字列。</param>
        /// <param name="source">フィルタ処理の入力として与えるストレージ オブジェクトのシーケンス。</param>
        /// <param name="storage">ストレージ オブジェクトの入出力先として使用するストレージ。</param>
        /// <param name="arguments">フィルタ処理の引数のリスト。</param>
        /// <returns>フロー処理の最終的な結果となる出力。</returns>
        public TOutput Output<TOutput>(String selector, Object source, StorageModule storage, IDictionary<String, String> arguments)
        {
            return (TOutput) this.Output(selector, source, storage, arguments, typeof(TOutput));
        }

        /// <summary>
        /// 出力処理を行います。
        /// </summary>
        /// <param name="selector">モジュールに対し照合のために提示するセレクタ文字列。</param>
        /// <param name="source">フィルタ処理の入力として与えるストレージ オブジェクトのシーケンス。</param>
        /// <param name="storage">ストレージ オブジェクトの入出力先として使用するストレージ。</param>
        /// <param name="arguments">フィルタ処理の引数のリスト。</param>
        /// <param name="outputType">出力されるデータの型。</param>
        /// <returns>フロー処理の最終的な結果となる出力。</returns>
        public Object Output(String selector, Object source, StorageModule storage, IDictionary<String, String> arguments, Type outputType)
        {
            this.CheckIfDisposed();
            return this.OutputHook.Execute(selector, source, storage, arguments, outputType);
        }

        private Object _Output(String selector, Object source, StorageModule storage, IDictionary<String, String> arguments, Type outputType)
        {
            String param;
            return this.GetFlowInterface(selector, outputType, out param).Invoke<Object>(
                this,
                source,
                storage,
                param,
                arguments
            );
        }

        /// <summary>
        /// 非同期の出力処理を開始します。
        /// </summary>
        /// <typeparam name="TOutput">出力されるデータの型。</typeparam>
        /// <param name="selector">モジュールに対し照合のために提示するセレクタ文字列。</param>
        /// <param name="source">フィルタ処理の入力として与えるストレージ オブジェクトのシーケンス。</param>
        /// <param name="storage">ストレージ オブジェクトの入出力先として使用するストレージ。</param>
        /// <param name="arguments">フィルタ処理の引数のリスト。</param>
        /// <param name="callback">出力処理完了時に呼び出されるオプションの非同期コールバック。</param>
        /// <param name="state">この特定の非同期出力処理要求を他の要求と区別するために使用するユーザー指定のオブジェクト。</param>
        /// <returns>非同期のフィルタ処理を表す <see cref="System.IAsyncResult"/>。まだ保留状態の場合もあります。</returns>
        public IAsyncResult BeginOutput<TOutput>(
            String selector,
            Object source,
            StorageModule storage,
            IDictionary<String, String> arguments,
            AsyncCallback callback,
            Object state
        )
        {
            return new Func<String, Object, StorageModule, IDictionary<String, String>, TOutput>(this.Output<TOutput>).BeginInvoke(
                selector,
                source,
                storage,
                arguments,
                callback,
                state
            );
        }

        /// <summary>
        /// 保留中の非同期出力処理が完了するまで待機します。
        /// </summary>
        /// <typeparam name="TOutput">出力されるデータの型。</typeparam>
        /// <param name="asyncResult">終了させる保留状態の非同期リクエストへの参照。</param>
        /// <returns>フロー処理の最終的な結果となる出力。</returns>
        public TOutput EndOutput<TOutput>(IAsyncResult asyncResult)
        {
            return asyncResult.GetAsyncDelegate<Func<String, Object, IDictionary<String, String>, TOutput>>()
                .EndInvoke(asyncResult);
        }
    }
}