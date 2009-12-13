// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetObjectModel
 *   Object model and Storage interface for MetaTweet and other systems
 *   Part of MetaTweet
 * Copyright c 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetObjectModel.
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

namespace XSpect.MetaTweet.Objects
{
    /// <summary>
    /// エンティティ モデルに依存しないアノテーションの基本実装を表します。
    /// </summary>
    public interface IAnnotation
        : IComparable<IAnnotation>,
          IEquatable<IAnnotation>
    {
        /// <summary>
        /// このアノテーションが関連付けられているアカウントの ID を取得または設定します。
        /// </summary>
        /// <value>
        /// このアノテーションが関連付けられているアカウントの ID。
        /// </value>
        Guid AccountId
        {
            get;
            set;
        }

        /// <summary>
        /// このアノテーションの意味となる文字列を取得または設定します。
        /// </summary>
        /// <value>
        /// このアノテーションの意味となる文字列。
        /// </value>
        String Name
        {
            get;
            set;
        }

        /// <summary>
        /// このアノテーションが関連付けられているアカウントを取得または設定します。
        /// </summary>
        /// <value>
        /// このアノテーションが関連付けられているアカウント。
        /// </value>
        IAccount Account
        {
            get;
            set;
        }

        /// <summary>
        /// 指定したアノテーションが、このアノテーションと完全に等しいかどうかを判断します。
        /// </summary>
        /// <param name="other">このアノテーションと比較するアノテーション。</param>
        /// <returns>指定したアノテーションがこのアノテーションと完全に等しい場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        Boolean EqualsExact(IAnnotation other);
    }
}