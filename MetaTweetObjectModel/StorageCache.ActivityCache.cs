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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using XSpect.MetaTweet.ObjectModel;

namespace XSpect.MetaTweet
{
    partial class StorageCache
    {
        /// <summary>
        /// アクティビティのデータのキャッシュ機能を提供します。
        /// </summary>
        /// <remarks>
        /// このクラスは、<see cref="Account.GetActivityOf(String)"/> の取得処理を高速化するために用意された機構です。
        /// </remarks>
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
                    return this[new KeyValuePair<Guid, String>(accountId, category)];
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
                return new KeyValuePair<Guid, String>(item.PrimaryKeys.AccountId, item.Category);
            }

            /// <summary>
            /// 指定したインデックスの位置に要素を挿入します。
            /// </summary>
            /// <param name="index"><paramref name="item"/> を挿入する位置の、0 から始まるインデックス番号。</param>
            /// <param name="item">挿入するアクティビティ。</param>
            protected override void InsertItem(Int32 index, Activity item)
            {
                KeyValuePair<Guid, String> key = new KeyValuePair<Guid, String>(item.PrimaryKeys.AccountId, item.Category);
                if (!this.Contains(key))
                {
                    base.InsertItem(index, item);
                }
                else if(this[key].CompareTo(item) < 0)
                {
                    this.Remove(key);
                    this.Add(item);
                }
            }

            /// <summary>
            /// 指定したアカウント ID およびカテゴリの、最新のアクティビティを取得します。キャッシュ内にアクティビティが存在しなかった場合は、バックエンドのデータソースからデータを取得し、キャッシュに追加します。
            /// </summary>
            /// <param name="accountId">アカウントを一意に識別するグローバル一意識別子 (GUID) 値。</param>
            /// <param name="category">取得するアクティビティのカテゴリ。</param>
            /// <returns>指定したアカウント ID およびカテゴリの、最新のアクティビティ。</returns>
            public Activity GetLatestActivity(Guid accountId, String category)
            {
                return this.GetLatestActivity(accountId, category, true);
            }

            /// <summary>
            /// 指定したアカウント ID およびカテゴリの、キャッシュおよびデータセット内における最新のアクティビティを取得します。キャッシュ内にアクティビティが存在しなかった場合は、データセットからデータを取得し、キャッシュに追加します。
            /// </summary>
            /// <param name="accountId">アカウントを一意に識別するグローバル一意識別子 (GUID) 値。</param>
            /// <param name="category">取得するアクティビティのカテゴリ。</param>
            /// <returns>指定したアカウント ID およびカテゴリの、キャッシュおよびデータセット内における最新のアクティビティ。</returns>
            public Activity GetLatestActivityInDataSet(Guid accountId, String category)
            {
                return this.GetLatestActivity(accountId, category, false);
            }

            private Activity GetLatestActivity(Guid accountId, String category, Boolean loadFromDataSource)
            {
                KeyValuePair<Guid, String> key = new KeyValuePair<Guid, String>(accountId, category);
                if (this.Contains(key))
                {
                    return this[key];
                }
                else
                {
                    if (loadFromDataSource)
                    {
                        this.Cache.Storage.LoadActivitiesDataTable(String.Format(
                            "WHERE [AccountId] == '{0}' AND [Category] == '{1}' ORDER BY [Timestamp] DESC, [Subindex] DESC LIMIT 1",
                            accountId.ToString("d"),
                            category
                        ));
                    }
                    Activity activity = this.Cache.Storage.GetActivities(accountId, null, category, null)
                        .OrderByDescending(a => a.Timestamp)
                        .ThenByDescending(a => a.Subindex)
                        .FirstOrDefault();
                    if (activity != null)
                    {
                        this.Add(activity);
                        return activity;
                    }
                    else
                    {
                        this.Remove(key);
                        return null;
                    }
                }
            }

            /// <summary>
            /// 指定したキーのアクティビティがキャッシュに含まれているかどうかを確認します。
            /// </summary>
            /// <param name="accountId">アカウントを一意に識別するグローバル一意識別子 (GUID) 値。</param>
            /// <param name="category">取得するアクティビティのカテゴリ。</param>
            /// <returns>指定したキーを持つ要素がキャッシュに格納されている場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
            public Boolean Contains(Guid accountId, String category)
            {
                return this.Contains(new KeyValuePair<Guid, String>(accountId, category));
            }

            /// <summary>
            /// 指定したキーを持つアクティビティをキャッシュから削除します。
            /// </summary>
            /// <param name="accountId">アカウントを一意に識別するグローバル一意識別子 (GUID) 値。</param>
            /// <param name="category">取得するアクティビティのカテゴリ。</param>
            /// <returns>アクティビティがキャッシュから正常に削除された場合は <c>true</c>。それ以外の場合は <c>false</c>。このメソッドは、<paramref name="key"/> がキャッシュ内に見つからない場合にも <c>false</c> を返します。</returns>
            public Boolean Remove(Guid accountId, String category)
            {
                return this.Remove(new KeyValuePair<Guid, String>(accountId, category));
            }
        }
    }
}