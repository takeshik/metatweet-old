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
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace XSpect.MetaTweet.Requesting
{
    /// <summary>
    /// ストアド リクエストを表す基底クラスです。
    /// </summary>
    /// <remarks>
    /// ストアド リクエストとは、定義された規則および適用時に渡される引数に基づいて <see cref="Request"/> を返す機構です。
    /// </remarks>
    [XmlInclude(typeof(RequestTemplate))]
    public abstract class StoredRequest
        : MarshalByRefObject
    {
        /// <summary>
        /// ストアド リクエストの名前を取得または設定します。
        /// </summary>
        /// <value>ストアド リクエストの名前。</value>
        public String Name
        {
            get;
            set;
        }

        /// <summary>
        /// ストアド リクエストの説明を取得または設定します。
        /// </summary>
        /// <value>ストアド リクエストの説明。</value>
        public String Description
        {
            get;
            set;
        }

        /// <summary>
        /// 引数の一覧を表す文字列のリストを取得または設定します。
        /// </summary>
        /// <value>引数の一覧を表す文字列のリスト。</value>
        /// <remarks>
        /// <para>引数は一つにつき一つの文字列によって表現され、その書式は以下の通りです:</para>
        /// <c>KEY=VALUE|KEY=VALUE|...</c>
        /// <para>ここで、<c>KEY=VALUE</c> が引数の定義のためのデータの組となります。キー <c>name</c> は必須項目であり、引数の名前を指定します。その他は任意に指定可能です。</para>
        /// </remarks>
        [XmlElement("Parameter")]
        public Collection<String> ParameterPairs
        {
            get;
            set;
        }

        /// <summary>
        /// 引数の一覧を表すディクショナリを取得します。
        /// </summary>
        /// <value>引数の一覧を表すディクショナリ。</value>
        /// <remarks>
        /// <see cref="Parameters"/> プロパティは二重の辞書構造で表現されており、引数の <c>name</c> 値によって、<c>name</c> を含めた引数を定義するデータの組で構成された辞書が取得できます。
        /// </remarks>
        public IDictionary<String, IDictionary<String, String>> Parameters
        {
            get
            {
                return this.ParameterPairs
                    .Select(s => s.Split('|'))
                    .Select(e => (IDictionary<String, String>) e
                        .Select(_ => _.Split('='))
                        .ToDictionary(p => p[0], p => p[1])
                    )
                    .ToDictionary(e => e["name"]);
            }
        }

        /// <summary>
        /// 派生クラスで実装された場合、ストアド リクエストを適用し、<see cref="Request"/> を返します。
        /// </summary>
        /// <param name="arguments">ストアド リクエストに渡す引数。</param>
        /// <returns>派生クラスで実装された場合、適用結果となる <see cref="Request"/>。</returns>
        public abstract Request Apply(IDictionary<String, String> arguments);
    }
}