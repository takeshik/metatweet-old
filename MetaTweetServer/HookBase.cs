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
using System.Collections.Generic;

namespace XSpect.MetaTweet
{
    /// <summary>
    /// フック リストの抽象基本クラスを提供します。
    /// </summary>
    /// <typeparam name="T"><see cref="Before"/> フックおよび <see cref="After"/> フックのデリゲートの型。</typeparam>
    /// <typeparam name="TEx"><see cref="Failed"/> フックのデリゲートの型。</typeparam>
    public abstract class HookBase<T, TEx>
        : Object
    {
        /// <summary>
        /// コードが実行される前に呼び出されるフック コードのリストを取得します。
        /// </summary>
        /// <value>
        /// コードが実行される前に呼び出されるフック コードのリスト。
        /// </value>
        public IList<T> Before
        {
            get;
            protected set;
        }

        /// <summary>
        /// コードが正常に実行された後に呼び出されるフック コードのリストを取得します。
        /// </summary>
        /// <value>
        /// コードが正常に実行された後に呼び出されるフック コードのリスト。
        /// </value>
        public IList<T> After
        {
            get;
            protected set;
        }

        /// <summary>
        /// コードの実行中に例外が発生した後に呼び出されるフック コードのリストを取得します。
        /// </summary>
        /// <value>
        /// コードの実行中に例外が発生した後に呼び出されるフック コードのリスト。
        /// </value>
        public IList<TEx> Failed
        {
            get;
            protected set;
        }
    }
}