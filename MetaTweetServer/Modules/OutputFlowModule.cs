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
using System.Collections.Generic;
using System.Linq;
using Achiral;
using XSpect.Extension;
using XSpect.MetaTweet.Properties;

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
        /// 出力処理を行います。
        /// </summary>
        /// <typeparam name="TOutput">出力されるデータの型。</typeparam>
        /// <param name="selector">モジュールに対し照合のために提示するセレクタ文字列。</param>
        /// <param name="input">フィルタ処理の入力として与えるストレージ オブジェクトのシーケンス。</param>
        /// <param name="storage">ストレージ オブジェクトの入出力先として使用するストレージ。</param>
        /// <param name="arguments">フィルタ処理の引数のリスト。</param>
        /// <param name="additionalData">処理結果の補足情報。このパラメータは初期化せずに渡されます。</param>
        /// <returns>フロー処理の最終的な結果となる出力。</returns>
        public TOutput Output<TOutput>(String selector, Object input, StorageModule storage, IDictionary<String, String> arguments, out IDictionary<String, Object> additionalData)
        {
            return (TOutput) this.Output(selector, input, storage, arguments, typeof(TOutput), out additionalData);
        }

        /// <summary>
        /// 出力処理を行います。
        /// </summary>
        /// <param name="selector">モジュールに対し照合のために提示するセレクタ文字列。</param>
        /// <param name="input">フィルタ処理の入力として与えるストレージ オブジェクトのシーケンス。</param>
        /// <param name="storage">ストレージ オブジェクトの入出力先として使用するストレージ。</param>
        /// <param name="arguments">フィルタ処理の引数のリスト。</param>
        /// <param name="outputType">出力されるデータの型。</param>
        /// <param name="additionalData">処理結果の補足情報。このパラメータは初期化せずに渡されます。</param>
        /// <returns>フロー処理の最終的な結果となる出力。</returns>
        public Object Output(String selector, Object input, StorageModule storage, IDictionary<String, String> arguments, Type outputType, out IDictionary<String, Object> additionalData)
        {
            this.CheckIfDisposed();
            this.Log.Debug(
                Resources.OutputFlowPerforming,
                this.Name,
                selector,
                input is IEnumerable
                    ? ((IEnumerable) input).Cast<Object>().Count()
                          .If(i => i > 1, i => i + " objects", i => i + " object")
                    : input,
                storage.Name,
                arguments.Inspect().Indent(4),
                outputType != null ? outputType.FullName : "(any)"
            );
            String param;
            Tuple<Object, IDictionary<String, Object>> result = Tuple.Create(this.GetFlowInterface(
                selector,
                input != null ? input.GetType() : null,
                outputType,
                out param
            ).Invoke(
                this,
                input,
                storage,
                param,
                arguments,
                out additionalData
            ), additionalData);
            this.Log.Debug(Resources.OutputFlowPerformed, this.Name);
            return result.Item1;
        }
    }
}