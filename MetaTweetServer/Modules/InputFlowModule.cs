// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
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
using XSpect.Extension;
using XSpect.MetaTweet.ObjectModel;

namespace XSpect.MetaTweet.Modules
{
    /// <summary>
    /// 入力フロー モジュールの抽象基本クラスを提供します。
    /// </summary>
    /// <remarks>
    /// 入力フロー モジュールとは、外部のデータ ソースからストレージ オブジェクトを生成し出力する、パイプラインの先端に位置するフロー モジュールを指します。
    /// </remarks>
    public abstract class InputFlowModule
        : FlowModule
    {
        /// <summary>
        /// <see cref="Input"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="Input"/> のフック リスト。
        /// </value>
        public Hook<InputFlowModule, String, StorageModule, IDictionary<String, String>> InputHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="InputFlowModule"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        protected InputFlowModule()
        {
            this.InputHook = new Hook<InputFlowModule, String, StorageModule, IDictionary<String, String>>();
        }

        /// <summary>
        /// 入力処理を行います。
        /// </summary>
        /// <param name="selector">モジュールに対し照合のために提示するセレクタ文字列。</param>
        /// <param name="storage">ストレージ オブジェクトの入出力先として使用するストレージ。</param>
        /// <param name="arguments">入力処理の引数のリスト。</param>
        /// <returns>データ ソースからの入力を基に生成された出力のシーケンス。</returns>
        public IEnumerable<StorageObject> Input(String selector, StorageModule storage, IDictionary<String, String> arguments)
        {
            this.CheckIfDisposed();
            return this.InputHook.Execute<IEnumerable<StorageObject>>((self, selector_, storage_, arguments_) =>
            {
                String param;
                return this.GetFlowInterface(selector_, out param).Invoke<IEnumerable<StorageObject>>(
                    self,
                    null,
                    storage_,
                    param,
                    arguments_
                );
            }, this, selector, storage, arguments);
        }

        /// <summary>
        /// 非同期の入力処理を開始します。
        /// </summary>
        /// <param name="selector">モジュールに対し照合のために提示するセレクタ文字列。</param>
        /// <param name="storage">ストレージ オブジェクトの出力先として使用するストレージ。</param>
        /// <param name="arguments">入力処理の引数のリスト。</param>
        /// <param name="callback">入力処理完了時に呼び出されるオプションの非同期コールバック。</param>
        /// <param name="state">この特定の非同期フィルタ処理要求を他の要求と区別するために使用するユーザー指定のオブジェクト。</param>
        /// <returns>データ ソースからの入力を基に生成された出力のシーケンス。</returns>
        public IAsyncResult BeginInput(
            String selector,
            StorageModule storage,
            IDictionary<String, String> arguments,
            AsyncCallback callback,
            Object state
        )
        {
            return new Func<String, StorageModule, IDictionary<String, String>, IEnumerable<StorageObject>>(this.Input).BeginInvoke(
                selector,
                storage,
                arguments,
                callback,
                state
            );
        }

        /// <summary>
        /// 保留中の非同期入力処理が完了するまで待機します。
        /// </summary>
        /// <param name="asyncResult">終了させる保留状態の非同期リクエストへの参照。</param>
        /// <returns>データ ソースからの入力を基に生成された出力のシーケンス。</returns>
        public IEnumerable<StorageObject> EndInput(IAsyncResult asyncResult)
        {
            return asyncResult.GetAsyncDelegate<Func<String, IDictionary<String, String>, IEnumerable<StorageObject>>>()
                .EndInvoke(asyncResult);
        }
    }
}