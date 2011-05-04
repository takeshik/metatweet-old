// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetInterface
 *   Common interface library to communicate with MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetInterface.
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
using System.Collections.ObjectModel;
using System.Linq;

namespace XSpect.MetaTweet.Modules
{
    /// <summary>
    /// モジュール オブジェクトを生成するための情報を表します。
    /// </summary>
    [Serializable()]
    public sealed class ModuleObjectSetup
        : Object
    {
        private String _key;

        private String _typeName;

        private Collection<String> _options;

        /// <summary>
        /// モジュール オブジェクトに付ける名前を取得または設定します。
        /// </summary>
        /// <value>
        /// モジュール オブジェクトに付ける名前。
        /// </value>
        public String Key
        {
            get
            {
                return this._key;
            }
            set
            {
                this._key = value ?? "";
            }
        }

        /// <summary>
        /// モジュール オブジェクトの型を表す文字列を取得または設定します。
        /// </summary>
        /// <value>
        /// モジュール オブジェクトの型を表す文字列。
        /// </value>
        public String TypeName
        {
            get
            {
                return this._typeName;
            }
            set
            {
                this._typeName = value ?? "";
            }
        }

        /// <summary>
        /// モジュール オブジェクトに渡すオプションのリストを取得または設定します。
        /// </summary>
        public Collection<String> Options
        {
            get
            {
                return this._options;
            }
            set
            {
                this._options = value ?? new Collection<String>();
            }
        }

        /// <summary>
        /// <see cref="ModuleObjectSetup"/> の新しいインスタンスを初期化します。
        /// </summary>
        public ModuleObjectSetup()
        {
            this.Options = new Collection<String>();
        }

        public ModuleObjectSetup(String key, String typeName, params String[] options)
        {
            this.Key = key;
            this.TypeName = typeName;
            this.Options = new Collection<String>(options);
        }

        public Int32 GetOrder()
        {
            String order = this.Options.SingleOrDefault(s => s.StartsWith("order="));
            return order == null ? 0 : Int32.Parse(order.Substring(6 /* "order=" */));
        }
    }
}