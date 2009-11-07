// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetObjectModel
 *   Object model and Storage interface for MetaTweet and other systems
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
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
    /// エンティティ モデルに依存しないリレーションの基本実装を表します。
    /// </summary>
    public interface IRelation
        : IComparable<IRelation>,
          IEquatable<IRelation>
    {
        /// <summary>
        /// このリレーションが関連付けられているアカウントの ID を取得または設定します。
        /// </summary>
        /// <value>
        /// このリレーションが関連付けられているアカウントの ID。
        /// </value>
        Guid AccountId
        {
            get;
            set;
        }

        /// <summary>
        /// このリレーションの意味となる文字列を取得または設定します。
        /// </summary>
        /// <value>
        /// このリレーションの意味となる文字列。
        /// </value>
        String Name
        {
            get;
            set;
        }

        /// <summary>
        /// このリレーションが関連付けられる先のアカウントの ID を取得または設定します。
        /// </summary>
        /// <value>
        /// このリレーションが関連付けられる先のアカウントの ID。
        /// </value>
        Guid RelatingAccountId
        {
            get;
            set;
        }

        /// <summary>
        /// このリレーションが関連付けられているアカウントを取得または設定します。
        /// </summary>
        /// <value>
        /// このリレーションが関連付けられているアカウント。
        /// </value>
        IAccount Account
        {
            get;
            set;
        }

        /// <summary>
        /// このリレーションが関連付けられる先のアカウントを取得または設定します。
        /// </summary>
        /// <value>
        /// このリレーションが関連付けられる先のアカウント。
        /// </value>
        IAccount RelatingAccount
        {
            get;
            set;
        }

        Boolean EqualsExact(IRelation other);
    }
}