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

using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using Achiral;
using XSpect.Extension;
using XSpect.MetaTweet.Objects;

namespace XSpect.MetaTweet.Modules
{
    /// <summary>
    /// フロー インターフェイスに関する情報を提供します。
    /// </summary>
    public class FlowInterfaceInfo
    {
        private readonly MethodInfo _method;

        private readonly FlowInterfaceAttribute _attribute;

        /// <summary>
        /// <see cref="FlowInterfaceInfo"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="method">フロー インターフェイスとして指定されたメソッド。</param>
        /// <param name="attribute">フロー インターフェイスであることを示すカスタム属性。</param>
        public FlowInterfaceInfo(MethodInfo method, FlowInterfaceAttribute attribute)
        {
            this._method = method;
            this._attribute = attribute;
        }

        /// <summary>
        /// このフロー インターフェイスの ID を取得します。
        /// </summary>
        /// <value>
        /// このフロー インターフェイスの ID。
        /// </value>
        public String Id
        {
            get
            {
                return this._attribute.Id;
            }
        }

        /// <summary>
        /// このフロー インターフェイスが書き込み操作を行うデータ表を示す値を取得します。
        /// </summary>
        /// <value>
        /// このフロー インターフェイスが書き込み操作を行うデータ表を示す値。
        /// </value>
        public StorageObjectTypes WriteTo
        {
            get
            {
                return this._attribute.WriteTo;
            }
        }

        /// <summary>
        /// このフロー インターフェイスに関する概要を取得します。
        /// </summary>
        /// <value>
        /// このフロー インターフェイスに関する概要。
        /// </value>
        public String Summary
        {
            get
            {
                return this._attribute.Summary;
            }
        }

        /// <summary>
        /// このフロー インターフェイスに関する補足説明を取得します。
        /// </summary>
        /// <value>
        /// このフロー インターフェイスに関する補足説明。
        /// </value>
        public String Remarks
        {
            get
            {
                return this._attribute.Remarks;
            }
        }

        public Type InputType
        {
            get
            {
                return this._method.GetParameters()
                    .SingleOrDefault(p => p.Name == "input")
                    .Null(p => p.ParameterType);
            }
        }

        /// <summary>
        /// このフロー インターフェイスの返すデータの型を表すオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// このフロー インターフェイスの返すデータの型を表すオブジェクト。
        /// </value>
        public Type OutputType
        {
            get
            {
                return this._method.ReturnType;
            }
        }

        /// <summary>
        /// このフロー インターフェイスに対してセレクタ照合を行います。
        /// </summary>
        /// <param name="selector">照合を行うセレクタ。</param>
        /// <returns>照合の結果得られたパラメータ。</returns>
        public String GetParameter(String selector)
        {
            return selector == "/"
                ? String.Empty
                : selector.Substring(this._attribute.Id.Length + (this._attribute.Id.EndsWith("/") ? 1 : 0));
        }

        /// <summary>
        /// フロー インターフェイスを呼び出します。
        /// </summary>
        /// <param name="module">呼び出しに用いるモジュール オブジェクト。</param>
        /// <param name="input">フィルタ処理の入力として与えるストレージ オブジェクトのシーケンス。</param>
        /// <param name="storage">ストレージ オブジェクトの入出力先として使用するストレージ。</param>
        /// <param name="parameter">処理のパラメータ。</param>
        /// <param name="arguments">処理の引数のリスト。</param>
        /// <param name="additionalData">処理の結果に付随して返される追加のデータ。このパラメータは初期化せずに渡されます。</param>
        /// <returns>処理の結果。</returns>
        public Object Invoke(
            FlowModule module,
            Object input,
            StorageModule storage,
            String parameter,
            IDictionary<String, String> arguments,
            out IDictionary<String, Object> additionalData
        )
        {
            storage.Wait(this.WriteTo);
            IDictionary<String, Object> data = null;
            Object result = this._method.GetParameters().Do(ps =>
                ((IEnumerable<Object>) Make.Array<Object>(storage, parameter, arguments))
                    .If(
                        a => ps.First().Name == "input",
                        a => Make.Sequence(input).Concat(a)
                    )
                    .If(
                        a => ps.Last().ParameterType == typeof(IDictionary<String, Object>),
                        a => a.Concat(Make.Sequence<Object>(new Dictionary<String, Object>()))
                    )
                    .Do(a => this._method.Invoke(module, a.ToArray())
                        .Let(_ => data = ps.Last().ParameterType == typeof(IDictionary<String, Object>)
                            ? (IDictionary<String, Object>) a.Last()
                            : null
                        )
                    )
            );
            additionalData = data;
            // There is no reason to update if WriteTo is None since
            // the flow was not accessed to any tables.
            if (this.WriteTo != StorageObjectTypes.None)
            {
                // WriteTo was already tested. Escape from double-checking.
                storage.Release(this.WriteTo);
                storage.TryUpdate();
            }
            return result;
        }
    }
}