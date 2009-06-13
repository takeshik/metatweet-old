// -*- mode: csharp; encoding: utf-8; -*-
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
using XSpect.MetaTweet.ObjectModel;

namespace XSpect.MetaTweet
{
    /// <summary>
    /// <see cref="StorageDataSet"/> のデータ表、または <see cref="StorageObject"/> の型を表します。
    /// </summary>
    [Flags()]
    public enum StorageDataTypes
        : int
    {
        /// <summary>
        /// どの <see cref="StorageDataSet"/> のデータ表および構成要素、または <see cref="StorageObject"/> を継承する型も示しません。
        /// </summary>
        None = 0x0,
        /// <summary>
        /// <see cref="StorageDataSet.AccountsDataTable"/> および構成要素、または <see cref="Account"/> (略称: Acc) を示します。
        /// </summary>
        Account = 0x1,
        /// <summary>
        /// <see cref="StorageDataSet.ActivitiesDataTable"/> および構成要素、または <see cref="Activity"/> (略称: Act) を示します。
        /// </summary>
        Activity = 0x2,
        /// <summary>
        /// <see cref="StorageDataSet.PostsDataTable"/> および構成要素、または <see cref="Post"/> (略称: Pst) を示します。
        /// </summary>
        Post = 0x4,
        /// <summary>
        /// <see cref="StorageDataSet.FollowMapDataTable"/> および構成要素、または <see cref="FollowElement"/> (略称: Flw) を示します。
        /// </summary>
        Follow = 0x10,
        /// <summary>
        /// <see cref="StorageDataSet.FavorMapDataTable"/> および構成要素、または <see cref="FavorElement"/> (略称: Fav) を示します。
        /// </summary>
        Favor = 0x20,
        /// <summary>
        /// <see cref="StorageDataSet.TagMapDataTable"/> および構成要素、または <see cref="TagElement"/> (略称: Tag) を示します。
        /// </summary>
        Tag = 0x40,
        /// <summary>
        /// <see cref="StorageDataSet.ReplyMapDataTable"/> および構成要素、または <see cref="ReplyElement"/> (略称: Rep) を示します。
        /// </summary>
        Reply = 0x80,
        /// <summary>
        /// 全ての <see cref="StorageDataSet"/> のデータ表および構成要素、または <see cref="StorageObject"/> を継承する全ての型を示します。
        /// </summary>
        All = Account | Activity | Post | Follow | Favor | Tag | Reply,
    }
}