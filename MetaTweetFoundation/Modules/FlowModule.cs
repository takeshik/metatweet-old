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
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using XSpect.MetaTweet.Properties;
using XSpect.MetaTweet.Objects;

namespace XSpect.MetaTweet.Modules
{
    /// <summary>
    /// フロー モジュールに共通の機能を提供します。
    /// </summary>
    [Serializable()]
    public abstract class FlowModule
        : Module
    {
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

        /// <summary>
        /// フロー処理を行います。
        /// </summary>
        /// <param name="selector">モジュールに対し照合のために提示するセレクタ文字列。</param>
        /// <param name="input">フロー処理の入力として与えるストレージ オブジェクトのシーケンス。</param>
        /// <param name="session">ストレージ オブジェクトの入出力先として使用するストレージ セッション。</param>
        /// <param name="arguments">フロー処理の引数のリスト。</param>
        /// <param name="variables">リクエスト間で受け渡される変数のディクショナリ。</param>
        /// <returns>フロー処理の結果となる出力。</returns>
        public Object Perform(
            String selector,
            Object input = null,
            StorageSession session = null,
            IDictionary<String, String> arguments = null,
            IDictionary<String, Object> variables = null
        )
        {
            this.CheckIfDisposed();
            this.Log.Debug(
                Resources.FlowPerforming,
                this.Name,
                selector,
                input is IEnumerable
                    ? ((IEnumerable) input).Cast<Object>().Count() + " object(s)"
                    : input,
                session,
                Inspect(arguments),
                Inspect(variables)
            );
            String param;
            Object output = this.GetFlowInterface(
                selector,
                input != null ? input.GetType() : null,
                null,
                out param
            ).Invoke(
                this,
                input,
                session,
                param,
                arguments,
                variables
            );
            this.Log.Debug(
                Resources.FlowPerformed,
                this.Name,
                output is IEnumerable
                    ? ((IEnumerable) output).Cast<Object>().Count() + " object(s)"
                    : output,
                Inspect(variables)
            );
            return output;
        }

        private static String Inspect<TValue>(IEnumerable<KeyValuePair<String, TValue>> dictionary)
        {
            return dictionary != null
                ? dictionary.Any()
                      ? String.Join(Environment.NewLine, dictionary.Select(p => "    " + p.ToString()))
                      : "(empty)"
                : "(null)";
        }
    }
}