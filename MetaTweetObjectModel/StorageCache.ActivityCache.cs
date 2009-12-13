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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace XSpect.MetaTweet.Objects
{
    /// <summary>
    /// <see cref="Activity"/> に対するデータのキャッシュ機能を提供します。 
    /// </summary>
    partial class StorageCache
    {
        /// <summary>
        /// <see cref="Activity"/> に対するデータのキャッシュ機能を提供します。
        /// </summary>
        [Serializable()]
        public class ActivityCache
            : KeyedCollection<KeyValuePair<Guid, String>, Activity>
        {
            /// <summary>
            /// 親となる <see cref="StorageCache"/> を取得します。
            /// </summary>
            /// <value>
            /// 親となる <see cref="StorageCache"/>。
            /// </value>
            public StorageCache Cache
            {
                get;
                private set;
            }

            /// <summary>
            /// 指定したアカウント ID およびカテゴリの、最新のアクティビティを取得します。
            /// </summary>
            /// <param name="accountId">アカウントを一意に識別するグローバル一意識別子 (GUID) 値。</param>
            /// <param name="category">取得するアクティビティのカテゴリ。</param>
            /// <returns>指定したアカウント ID およびカテゴリの、最新のアクティビティ。</returns>
            public Activity this[Guid accountId, String category]
            {
                get
                {
                    KeyValuePair<Guid, String> key = new KeyValuePair<Guid, String>(accountId, category);
                    return this.Contains(key) ? this[key] : null;
                }
            }

            /// <summary>
            /// <see cref="ActivityCache"/> の新しいインスタンスを初期化します。
            /// </summary>
            /// <param name="cache">親となる <see cref="StorageCache"/>。</param>
            public ActivityCache(StorageCache cache)
            {
                this.Cache = cache;
            }

            /// <summary>
            /// 指定したアクティビティからキーを抽出します。
            /// </summary>
            /// <param name="item">キーの抽出元アクティビティ。</param>
            /// <returns>指定したアクティビティのキー。</returns>
            protected override KeyValuePair<Guid, String> GetKeyForItem(Activity item)
            {
                return new KeyValuePair<Guid, String>(item.AccountId, item.Category);
            }

            /// <summary>
            /// 指定したアクティビティをキャッシュに追加します。
            /// </summary>
            /// <param name="activity">キャッシュへの追加を試行するアクティビティ。</param>
            /// <returns>キャッシュにアクティビティが追加された場合は <c>true</c>。それ以外の場合 (すでにより新しいアクティビティがキャッシュに存在している状態) は <c>false</c>。</returns>
            public Boolean Update(Activity activity)
            {
                Activity latest = this[activity.AccountId, activity.Category];
                // There is no problem when latest is null, because null is smaller than
                // any non-null Activity objects.
                if (activity.CompareTo(activity) > 0)
                {
                    this.Remove(latest);
                    this.Add(activity);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// キャッシュからアクティビティを取得します。
            /// </summary>
            /// <param name="accountId">アクティビティを行ったアカウントの ID。</param>
            /// <param name="category">アクティビティのカテゴリ。</param>
            /// <returns>指定したアカウントによる、指定したカテゴリの最新のアクティビティ。キャッシュに存在しなかった場合は <c>null</c>。</returns>
            public Activity GetActivity(Guid accountId, String category)
            {
                KeyValuePair<Guid, String> key = new KeyValuePair<Guid, String>(accountId, category);
                if (!this.Contains(key))
                {
                    Activity latest = this.Cache.Storage.GetActivities(
                        accountId,
                        null,
                        category,
                        null
                    )
                        .OrderByDescending(a => a)
                        .First();
                    if (latest != null)
                    {
                        this.Add(latest);
                    }
                    else
                    {
                        return null;
                    }
                }
                return this[key];
            }
        }
    }
}