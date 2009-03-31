// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
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

namespace XSpect.MetaTweet.Modules
{
    /// <summary>
    /// メソッドがフロー インターフェイスであることを示します。このクラスは継承できません。
    /// </summary>
    /// <remarks>
    /// <para>フロー インターフェイスとは、<see cref="FlowModule"/> において、セレクタ照合によって呼び出すことのできるメソッドを指します。</para>
    /// <para>全てのフロー インターフェイスは ID を持ちます。フロー インターフェイスはセレクタおよび出力データの型によって一意に識別されます。セレクタ照合は前方一致によって行われ、最長一致であるものほど高い順位に置かれます。標準では最も高い順位のフロー インターフェイスが選択されます。セレクタにおいて ID に続く文字列はパラメータとして扱われます。例えば、<c>/foo/bar/baz/</c> という ID が <c>/foo/bar/baz/1234</c> というセレクタによって照合された場合、パラメータは <c>1234</c> となります。</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class FlowInterfaceAttribute
        : Attribute
    {
        /// <summary>
        /// このフロー インターフェイスの ID を取得します。
        /// </summary>
        /// <value>
        /// このフロー インターフェイスの ID。
        /// </value>
        public String Id
        {
            get;
            private set;
        }

        /// <summary>
        /// このフロー インターフェイスがアクセスするデータ表を示す値を取得または設定します。
        /// </summary>
        /// <value>
        /// このフロー インターフェイスがアクセスするデータ表を示す値。既定値は <see cref="StorageDataTypes.All"/>。
        /// </value>
        public StorageDataTypes AccessTo
        {
            get;
            set;
        }

        /// <summary>
        /// このフロー インターフェイスに関する概要を取得または設定します。
        /// </summary>
        /// <value>
        /// このフロー インターフェイスに関する概要。
        /// </value>
        public String Summary
        {
            get;
            set;
        }

        /// <summary>
        /// このフロー インターフェイスに関する補足説明を取得または設定します。
        /// </summary>
        /// <value>
        /// このフロー インターフェイスに関する補足説明。
        /// </value>
        public String Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// <see cref="FlowInterfaceAttribute"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="id">フロー インターフェイスの ID。</param>
        public FlowInterfaceAttribute(String id)
        {
            this.AccessTo = StorageDataTypes.All;
            this.Id = id;
        }
    }
}