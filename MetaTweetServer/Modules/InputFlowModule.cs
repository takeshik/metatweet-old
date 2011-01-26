// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
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
using System.Collections.Generic;
using System.Linq;
using Achiral;
using XSpect.Extension;
using XSpect.MetaTweet.Objects;
using XSpect.MetaTweet.Properties;
using System.Collections;

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
        /// 入力処理を行います。
        /// </summary>
        /// <param name="selector">モジュールに対し照合のために提示するセレクタ文字列。</param>
        /// <param name="storage">ストレージ オブジェクトの入出力先として使用するストレージ。</param>
        /// <param name="arguments">入力処理の引数のリスト。</param>
        /// <param name="additionalData">処理結果の補足情報。このパラメータは初期化せずに渡されます。</param>
        /// <returns>データ ソースからの入力を基に生成された出力のシーケンス。</returns>
        public Object Input(String selector, StorageSession session, IDictionary<String, String> arguments, out IDictionary<String, Object> additionalData)
        {
            this.CheckIfDisposed();
            this.Log.Debug(
                Resources.InputFlowPerforming,
                this.Name,
                selector,
                session,
                arguments.Inspect().Indent(4)
            );
            String param;
            Tuple<Object, IDictionary<String, Object>> result = Tuple.Create(this.GetFlowInterface(selector, out param).Invoke(
                this,
                null,
                session,
                param,
                arguments,
                out additionalData
            ), additionalData);
            this.Log.Debug(Resources.InputFlowPerformed, this.Name, result.Item1 is IEnumerable
                ? ((IEnumerable) result.Item1).Cast<Object>().Count()
                      .If(i => i > 1, i => i + " objects", i => i + " object")
                : result.Item1
            );
            return result.Item1;
        }
    }
}