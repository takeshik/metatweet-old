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

using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// このフロー インターフェイスの入力の型を表すオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// このフロー インターフェイスの入力の型を表すオブジェクト。
        /// </value>
        public Type InputType
        {
            get
            {
                return this.RequiresInput
                    ? this._method.GetParameters().Single(p => p.Name == "input").ParameterType
                    : typeof(void);
            }
        }

        /// <summary>
        /// このフロー インターフェイスの出力の型を表すオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// このフロー インターフェイスの出力の型を表すオブジェクト。
        /// </value>
        public Type OutputType
        {
            get
            {
                return this._method.ReturnType;
            }
        }

        /// <summary>
        /// このフロー インターフェイスが入力を必要とするかどうかを表す値を取得します。
        /// </summary>
        /// <value>
        /// このフロー インターフェイスが入力を必要とする場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </value>
        public Boolean RequiresInput
        {
            get
            {
                return this._method.GetParameters()
                    .Any(p => p.Name == "input");
            }
        }

        /// <summary>
        /// このフロー インターフェイスがストレージセッションを使用するかどうかを表す値を取得します。
        /// </summary>
        /// <value>
        /// このフロー インターフェイスがストレージセッションを使用する場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </value>
        public Boolean RequiresSession
        {
            get
            {
                return this._method.GetParameters()
                    .Any(p => p.Name == "session" && p.ParameterType == typeof(StorageSession));
            }
        }

        /// <summary>
        /// このフロー インターフェイスがパラメータを要求するかどうかを表す値を取得します。
        /// </summary>
        /// <value>
        /// このフロー インターフェイスがパラメータを要求する場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </value>
        public Boolean RequiresParameter
        {
            get
            {
                return this._method.GetParameters()
                    .Any(p => (p.Name == "parameter" || p.Name == "param") && p.ParameterType == typeof(String));
            }
        }

        /// <summary>
        /// このフロー インターフェイスがフロー 引数を要求するかどうかを表す値を取得します。
        /// </summary>
        /// <value>
        /// このフロー インターフェイスがフロー 引数を要求する場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </value>
        public Boolean RequiresArguments
        {
            get
            {
                return this._method.GetParameters()
                    .Any(p => (p.Name == "arguments" || p.Name == "args") && p.ParameterType == typeof(IDictionary<String, String>));
            }
        }

        /// <summary>
        /// このフロー インターフェイスがリクエスト変数を処理するかどうかを表す値を取得します。
        /// </summary>
        /// <value>
        /// このフロー インターフェイスがリクエスト変数を処理する場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </value>
        public Boolean HandlesVariables
        {
            get
            {
                return this._method.GetParameters()
                    .Any(p => (p.Name == "variables" || p.Name == "vars") && p.ParameterType == typeof(IDictionary<String, Object>));
            }
        }

        /// <summary>
        /// このフロー インターフェイスに対してセレクタ照合を行います。
        /// </summary>
        /// <param name="selector">照合を行うセレクタ。</param>
        /// <returns>照合の結果得られたパラメータ。</returns>
        public String GetParameter(String selector)
        {
            return String.IsNullOrEmpty(selector)
                ? ""
                : selector.Substring(this._attribute.Id.Length);
        }

        /// <summary>
        /// フロー インターフェイスを呼び出します。
        /// </summary>
        /// <param name="module">呼び出しに用いるモジュール オブジェクト。</param>
        /// <param name="input">フィルタ処理の入力として与えるストレージ オブジェクトのシーケンス。</param>
        /// <param name="session">ストレージ オブジェクトの入出力先として使用するストレージ セッション。</param>
        /// <param name="parameter">処理のパラメータ。</param>
        /// <param name="arguments">処理の引数のディクショナリ。</param>
        /// <param name="variables">リクエスト間で受け渡される変数のディクショナリ。</param>
        /// <returns>処理の結果。</returns>
        public Object Invoke(
            FlowModule module,
            Object input = null,
            StorageSession session = null,
            String parameter = null,
            IDictionary<String, String> arguments = null,
            IDictionary<String, Object> variables = null
        )
        {
            LinkedList<Object> args = new LinkedList<Object>();
            if(this.RequiresInput)
            {
                args.AddLast(input);
            }
            if(this.RequiresSession)
            {
                args.AddLast(session);
            }
            if(this.RequiresParameter)
            {
                args.AddLast(parameter ?? "");
            }
            if(this.RequiresArguments)
            {
                args.AddLast(arguments ?? new Dictionary<String, String>());
            }
            if(this.HandlesVariables)
            {
                args.AddLast(variables ?? new Dictionary<String, Object>());
            }
            Object output = this._method.Invoke(module, args.ToArray());
            if (session != null)
            {
                session.Update();
            }
            return output;
        }
    }
}