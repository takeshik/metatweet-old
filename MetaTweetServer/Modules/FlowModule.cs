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
using System.Linq;
using System.Net;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.Remoting;
using XSpect.Extension;
using XSpect.Hooking;
using XSpect.MetaTweet.Objects;
using log4net;

namespace XSpect.MetaTweet.Modules
{
    /// <summary>
    /// フロー モジュールに共通の機能を提供します。
    /// </summary>
    /// <remarks>
    /// <para>フロー モジュールとは、このクラスを継承する型、即ち <see cref="InputFlowModule"/>、<see cref="FilterFlowModule"/> および <see cref="OutputFlowModule"/> を指します。このクラスは、これらフロー モジュールに共通の操作を実装し、提供します。</para>
    /// <para>全てのフロー モジュール (<see cref="FlowModule"/> を継承する全てのモジュール) はスカラ値を返すことができます。スカラ値は任意の型の値で、<see cref="FlowInterfaceAttribute.Id"/> が <c>@</c> で始まるインターフェイスで返すことができます。スカラ値の取得は入力を取ることができず、また、スカラ値の型に関わらず取得した時点でフロー パイプラインは終了します。</para>
    /// </remarks>
    [Serializable()]
    public abstract class FlowModule
        : Module
    {
        /// <summary>
        /// ストレージに対し提示する、サービスを表す文字列 (Realm) を取得または設定します。
        /// </summary>
        /// <value>
        /// ストレージに対し提示する、サービスを表す文字列 (Realm)。
        /// </value>
        /// <remarks>
        /// <para>このプロパティの値は <see cref="XSpect.MetaTweet.Objects.Account.Realm"/> に対応します。</para>
        /// </remarks>
        public String Realm
        {
            get
            {
                return this.Configuration.Realm;
            }
        }

        /// <summary>
        /// 入出力の型およびセレクタ文字列を指定して、このモジュールに定義されているフロー インターフェイスを検索します。
        /// </summary>
        /// <param name="selector">フロー インターフェイスに対し照合を行うセレクタ文字列。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="inputType">フロー インターフェイスの入力の型。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="outputType">フロー インターフェイスの出力の型。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>指定した条件に合致するフロー インターフェイスと、セレクタ照合で得られたパラメータの組のシーケンス。</returns>
        public IEnumerable<KeyValuePair<FlowInterfaceInfo, String>> GetFlowInterfaces(String selector, Type inputType, Type outputType)
        {
            this.CheckIfDisposed();
            return this.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .SelectMany(
                    m => m.GetCustomAttributes(typeof(FlowInterfaceAttribute), true)
                        .Cast<FlowInterfaceAttribute>()
                        .Select(a => new FlowInterfaceInfo(m, a))
                )
                .Where(ii =>
                    (selector == null || selector.StartsWith(ii.Id)) &&
                    (inputType == null || ii.InputType.IsAssignableFrom(inputType)) &&
                    (outputType == null || ii.OutputType.IsAssignableFrom(outputType))
                )
                .Select(ii =>
                    new KeyValuePair<FlowInterfaceInfo, String>(ii, ii.GetParameter(selector))
                )
                .OrderBy(p => p.Value.Length);
        }

        /// <summary>
        /// このモジュールに定義されているフロー インターフェイスを検索します。
        /// </summary>
        /// <returns>全てのフロー インターフェイスのシーケンス。</returns>
        public IEnumerable<KeyValuePair<FlowInterfaceInfo, String>> GetFlowInterfaces()
        {
            return this.GetFlowInterfaces(null, null, null);
        }

        /// <summary>
        /// 入出力の型およびセレクタ文字列を指定して、このモジュールに定義されているフロー インターフェイスを検索します。
        /// </summary>
        /// <typeparam name="TInput">フロー インターフェイスの入力の型。</typeparam>
        /// <typeparam name="TOutput">フロー インターフェイスの出力の型。</typeparam>
        /// <param name="selector">フロー インターフェイスに対し照合を行うセレクタ文字列。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>指定した条件に合致するフロー インターフェイスと、セレクタ照合で得られたパラメータの組のシーケンス。</returns>
        public IEnumerable<KeyValuePair<FlowInterfaceInfo, String>> GetFlowInterfaces<TInput, TOutput>(String selector)
        {
            return this.GetFlowInterfaces(selector, typeof(TInput), typeof(TOutput));
        }

        /// <summary>
        /// セレクタ文字列を指定して、このモジュールに定義されているフロー インターフェイスを検索します。
        /// </summary>
        /// <param name="selector">フロー インターフェイスに対し照合を行うセレクタ文字列。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>指定した条件に合致するフロー インターフェイスと、セレクタ照合で得られたパラメータの組のシーケンス。</returns>
        public IEnumerable<KeyValuePair<FlowInterfaceInfo, String>> GetFlowInterfaces(String selector)
        {
            return this.GetFlowInterfaces(selector, null, null);
        }

        /// <summary>
        /// 入出力の型およびセレクタ文字列を指定して、このモジュールに定義されているフロー インターフェイスを取得します。
        /// </summary>
        /// <param name="selector">フロー インターフェイスに対し照合を行うセレクタ文字列。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="inputType">フロー インターフェイスの入力の型。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="outputType">フロー インターフェイスの出力の型。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="parameter">セレクタ照合で得られたパラメータ。このパラメータは初期化せずに渡されます。</param>
        /// <returns>指定した条件に合致する中で、最も適合するフロー インターフェイス。</returns>
        public FlowInterfaceInfo GetFlowInterface(String selector, Type inputType, Type outputType, out String parameter)
        {
            KeyValuePair<FlowInterfaceInfo, String> selected
                = this.GetFlowInterfaces(selector, inputType, outputType).First();
            parameter = selected.Value;
            return selected.Key;
        }

        /// <summary>
        /// 入出力の型およびセレクタ文字列を指定して、このモジュールに定義されているフロー インターフェイスを取得します。
        /// </summary>
        /// <typeparam name="TInput">フロー インターフェイスの入力の型。</typeparam>
        /// <typeparam name="TOutput">フロー インターフェイスの出力の型。</typeparam>
        /// <param name="selector">フロー インターフェイスに対し照合を行うセレクタ文字列。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="parameter">セレクタ照合で得られたパラメータ。このパラメータは初期化せずに渡されます。</param>
        /// <returns>指定した条件に合致する中で、最も適合するフロー インターフェイス。</returns>
        public FlowInterfaceInfo GetFlowInterface<TInput, TOutput>(String selector, out String parameter)
        {
            return this.GetFlowInterface(selector, typeof(TInput), typeof(TOutput), out parameter);
        }

        /// <summary>
        /// セレクタ文字列を指定して、このモジュールに定義されているフロー インターフェイスを取得します。
        /// </summary>
        /// <param name="selector">フロー インターフェイスに対し照合を行うセレクタ文字列。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="parameter">セレクタ照合で得られたパラメータ。このパラメータは初期化せずに渡されます。</param>
        /// <returns>指定した条件に合致する中で、最も適合するフロー インターフェイス。</returns>
        public FlowInterfaceInfo GetFlowInterface(String selector, out String parameter)
        {
            return this.GetFlowInterface(selector, null, null, out parameter);
        }
    }
}