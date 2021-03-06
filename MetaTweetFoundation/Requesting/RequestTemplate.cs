﻿// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetFoundation
 *   Common library to access MetaTweet platform
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetFoundation.
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
using System.Text.RegularExpressions;

namespace XSpect.MetaTweet.Requesting
{
    /// <summary>
    /// リクエスト文字列のテンプレートに引数の値を展開することによって <see cref="Request"/> を得るストアド リクエストの実装を提供します。
    /// </summary>
    /// <remarks>
    /// <see cref="RequestTemplate"/> オブジェクトの定義および使用方法については <see cref="Template"/> プロパティを参照してください。
    /// </remarks>
    [Serializable()]
    public class RequestTemplate
        : StoredRequest
    {
        /// <summary>
        /// <see cref="Request"/> を生成するためのリクエスト文字列のテンプレートを取得または設定します。
        /// </summary>
        /// <value><see cref="Request"/> を生成するためのリクエスト文字列のテンプレート。</value>
        /// <remarks>
        /// <para>この文字列において、<c>$(KEY)</c> と指定された部分は、<see cref="Apply"/> で渡された引数のディクショナリにおける、キー <c>KEY</c> の値の文字列に置換されます。</para>
        /// <para>置換はリクエスト文字列の各部分ごと (ストレージ名、フロー名、セレクタ、引数の各キーおよび値ごと) に行われます。</para>
        /// </remarks>
        public String Template
        {
            get;
            set;
        }

        /// <summary>
        /// <see cref="RequestTemplate"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="name">ストアド リクエストの名前。</param>
        /// <param name="description">ストアド リクエストの説明。</param>
        /// <param name="template">ストアド リクエストの処理内容を表すテンプレート文字列。</param>
        /// <param name="parameters">ストアド リクエストの引数のリスト。</param>
        public RequestTemplate(String name, String description, String template, params String[] parameters)
        {
            this.Name = name;
            this.Description = description;
            this.Template = template;
            this.ParameterPairs = new Collection<String>(parameters);
        }

        /// <summary>
        /// ストアド リクエストを適用し、<see cref="Request"/> を返します。
        /// </summary>
        /// <param name="arguments">ストアド リクエストに渡す引数。</param>
        /// <returns>
        /// 適用結果となる <see cref="Request"/>。
        /// </returns>
        public override Request Apply(IDictionary<String, String> arguments)
        {
            return this.Template.StartsWith("/")
                ? this.Replace(Request.Parse(this.Template), arguments)
                : Request.Parse(this.Replace(this.Template, arguments));
        }

        private Request Replace(Request request, IDictionary<String, String> arguments)
        {
            return new Request(request.Fragments.Select(_ => this.Replace(_, arguments)));
        }

        private Fragment Replace(Fragment fragment, IDictionary<String, String> arguments)
        {
            switch (fragment.Type)
            {
                case FragmentType.Flow:
                    FlowFragment f = (FlowFragment) fragment;
                    return new FlowFragment(
                        this.Replace(f.Variables, arguments),
                        this.Replace(f.FlowName, arguments),
                        this.Replace(f.Selector, arguments),
                        this.Replace(f.Arguments, arguments)
                    );
                case FragmentType.Code:
                    CodeFragment c = (CodeFragment) fragment;
                    return new CodeFragment(
                        this.Replace(c.Variables, arguments),
                        this.Replace(c.Code, arguments)
                    );
                case FragmentType.Scope:
                    ScopeFragment s = (ScopeFragment) fragment;
                    return new ScopeFragment(
                        this.Replace(s.Variables, arguments),
                        s.Fragments.Select(_ => this.Replace(_, arguments))
                    );
                default: // case FragmentType.Operator
                    OperatorFragment o = (OperatorFragment) fragment;
                    return new OperatorFragment(
                        this.Replace(o.Variables, arguments),
                        this.Replace(o.Name, arguments)
                    );
            }
        }

        private IDictionary<String, String> Replace(IEnumerable<KeyValuePair<string, string>> dictionary, IDictionary<String, String> arguments)
        {
            return dictionary
                .Select(p => new KeyValuePair<String, String>(
                    this.Replace(p.Key, arguments),
                    this.Replace(p.Value, arguments)
                ))
                .ToDictionary(p => p.Key, p => p.Value);
        }

        private String Replace(String str, IDictionary<String, String> arguments)
        {
            return Regex.Replace(str, @"\$\((\w+)\)", m => arguments[m.Groups[1].Value]);
        }
    }
}
