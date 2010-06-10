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
using Achiral;
using XSpect.Extension;
using XSpect.Hooking;
using XSpect.MetaTweet.Objects;

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
        public FuncHook<InputFlowModule, String, StorageModule, IDictionary<String, String>, Tuple<Object, IDictionary<String, Object>>> InputHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="InputFlowModule"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        protected InputFlowModule()
        {
            this.InputHook = new FuncHook<InputFlowModule, String, StorageModule, IDictionary<String, String>, Tuple<Object, IDictionary<String, Object>>>(this._Input);
        }

        /// <summary>
        /// 入力処理を行います。
        /// </summary>
        /// <param name="selector">モジュールに対し照合のために提示するセレクタ文字列。</param>
        /// <param name="storage">ストレージ オブジェクトの入出力先として使用するストレージ。</param>
        /// <param name="arguments">入力処理の引数のリスト。</param>
        /// <returns>データ ソースからの入力を基に生成された出力のシーケンス。</returns>
        public Object Input(String selector, StorageModule storage, IDictionary<String, String> arguments, out IDictionary<String, Object> additionalData)
        {
            this.CheckIfDisposed();
            Tuple<Object, IDictionary<String, Object>> result = this.InputHook.Execute(selector, storage, arguments);
            additionalData = result.Item2;
            return result.Item1;
        }

        private Tuple<Object, IDictionary<String, Object>> _Input(String selector, StorageModule storage, IDictionary<String, String> arguments)
        {
            String param;
            IDictionary<String, Object> additionalData;
            return Make.Tuple(this.GetFlowInterface(selector, out param).Invoke(
                this,
                null,
                storage,
                param,
                arguments,
                out additionalData
            ), additionalData);
        }
    }
}