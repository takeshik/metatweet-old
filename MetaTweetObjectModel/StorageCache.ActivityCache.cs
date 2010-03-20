// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetObjectModel
 *   Object model and Storage interface for MetaTweet and other systems
 *   Part of MetaTweet
 * Copyright © 2008-2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace XSpect.MetaTweet.Objects
{
    partial class StorageCache
    {
        /// <summary>
        /// <see cref="Activity"/> に対するデータのキャッシュ機能を提供します。
        /// </summary>
        [Serializable()]
        public class ActivityCache
            : MarshalByRefObject,
              ISerializable,
              ICollection<Activity>
        {
            private readonly Dictionary<KeyValuePair<Guid, String>, Activity> _dictionary;

            #region Interface Implementations

            /// <summary>
            /// コレクションを反復処理する列挙子を返します。
            /// </summary>
            /// <returns>
            /// コレクションを反復処理するために使用できる <see cref="T:System.Collections.Generic.IEnumerator`1"/>。
            /// </returns>
            /// <filterpriority>1</filterpriority>
            public IEnumerator<Activity> GetEnumerator()
            {
                return this._dictionary.Values.GetEnumerator();
            }

            /// <summary>
            /// コレクションを反復処理する列挙子を返します。
            /// </summary>
            /// <returns>
            /// コレクションを反復処理するために使用できる <see cref="T:System.Collections.IEnumerator"/> オブジェクト。
            /// </returns>
            /// <filterpriority>2</filterpriority>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            /// <summary>
            /// <see cref="T:System.Collections.Generic.ICollection`1"/> に項目を追加します。
            /// </summary>
            /// <param name="item"><see cref="T:System.Collections.Generic.ICollection`1"/> に追加するオブジェクト。</param><exception cref="T:System.NotSupportedException"><see cref="T:System.Collections.Generic.ICollection`1"/> は読み取り専用です。</exception>
            public void Add(Activity item)
            {
                this._dictionary.Add(GetKeyForItem(item), item);
            }

            /// <summary>
            /// <see cref="T:System.Collections.Generic.ICollection`1"/> からすべての項目を削除します。
            /// </summary>
            /// <exception cref="T:System.NotSupportedException"><see cref="T:System.Collections.Generic.ICollection`1"/> は読み取り専用です。 </exception>
            public void Clear()
            {
                this._dictionary.Clear();
            }

            /// <summary>
            /// <see cref="T:System.Collections.Generic.ICollection`1"/> に特定の値が格納されているかどうかを判断します。
            /// </summary>
            /// <returns>
            /// <paramref name="item"/> が <see cref="T:System.Collections.Generic.ICollection`1"/> に存在する場合は true。それ以外の場合は false。
            /// </returns>
            /// <param name="item"><see cref="T:System.Collections.Generic.ICollection`1"/> 内で検索するオブジェクト。</param>
            public Boolean Contains(Activity item)
            {
                return this._dictionary.ContainsValue(item);
            }

            /// <summary>
            /// <see cref="T:System.Collections.Generic.ICollection`1"/> の要素を <see cref="T:System.Array"/> にコピーします。<see cref="T:System.Array"/> の特定のインデックスからコピーが開始されます。
            /// </summary>
            /// <param name="array"><see cref="T:System.Collections.Generic.ICollection`1"/> から要素がコピーされる 1 次元の <see cref="T:System.Array"/>。<see cref="T:System.Array"/> には、0 から始まるインデックス番号が必要です。</param><param name="arrayIndex">コピーの開始位置となる、<paramref name="array"/> の 0 から始まるインデックス番号。</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> が null です。</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> が 0 未満です。</exception><exception cref="T:System.ArgumentException"><paramref name="array"/> が多次元です。または<paramref name="arrayIndex"/> が array の長さ以上です。またはコピー元の <see cref="T:System.Collections.Generic.ICollection`1"/> の要素数が、<paramref name="arrayIndex"/> からコピー先の <paramref name="array"/> の末尾までに格納できる数を超えています。または型 <paramref name="T"/> をコピー先の <paramref name="array"/> の型に自動的にキャストすることはできません。</exception>
            public void CopyTo(Activity[] array, Int32 arrayIndex)
            {
                this._dictionary.Values.CopyTo(array, arrayIndex);
            }

            /// <summary>
            /// <see cref="T:System.Collections.Generic.ICollection`1"/> 内で最初に見つかった特定のオブジェクトを削除します。
            /// </summary>
            /// <returns>
            /// <paramref name="item"/> が <see cref="T:System.Collections.Generic.ICollection`1"/> から正常に削除された場合は true。それ以外の場合は false。このメソッドは、<paramref name="item"/> が元の <see cref="T:System.Collections.Generic.ICollection`1"/> に見つからない場合にも false を返します。
            /// </returns>
            /// <param name="item"><see cref="T:System.Collections.Generic.ICollection`1"/> から削除するオブジェクト。</param><exception cref="T:System.NotSupportedException"><see cref="T:System.Collections.Generic.ICollection`1"/> は読み取り専用です。</exception>
            public Boolean Remove(Activity item)
            {
                return this._dictionary.Remove(GetKeyForItem(item));
            }

            /// <summary>
            /// <see cref="T:System.Collections.Generic.ICollection`1"/> に格納されている要素の数を取得します。
            /// </summary>
            /// <returns>
            /// <see cref="T:System.Collections.Generic.ICollection`1"/> に格納されている要素の数。
            /// </returns>
            public Int32 Count
            {
                get
                {
                    return this._dictionary.Count;
                }
            }

            /// <summary>
            /// <see cref="T:System.Collections.Generic.ICollection`1"/> が読み取り専用かどうかを示す値を取得します。
            /// </summary>
            /// <returns>
            /// <see cref="T:System.Collections.Generic.ICollection`1"/> が読み取り専用の場合は true。それ以外の場合は false。
            /// </returns>
            public Boolean IsReadOnly
            {
                get
                {
                    return (this._dictionary as ICollection<KeyValuePair<KeyValuePair<Guid, String>, Activity>>)
                        .IsReadOnly;
                }
            }

            /// <summary>
            /// <see cref="T:System.Runtime.Serialization.SerializationInfo"/> に、オブジェクトをシリアル化するために必要なデータを設定します。
            /// </summary>
            /// <param name="info">データを読み込む先の <see cref="T:System.Runtime.Serialization.SerializationInfo"/>。</param><param name="context">このシリアル化のシリアル化先 (<see cref="T:System.Runtime.Serialization.StreamingContext"/> を参照)。</param><exception cref="T:System.Security.SecurityException">呼び出し元に、必要なアクセス許可がありません。</exception>
            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("dictionary", this._dictionary, typeof(Dictionary<KeyValuePair<Guid, String>, Activity>));
            }

            #endregion

            #region Members for Compatibility with KeyedCollection<TKey, TItem>

            public Activity this[KeyValuePair<Guid, String> key]
            {
                get
                {
                    return this._dictionary[key];
                }
            }

            public Boolean Contains(KeyValuePair<Guid, String> key)
            {
                return this._dictionary.ContainsKey(key);
            }

            public Boolean Remove(KeyValuePair<Guid, String> key)
            {
                return this._dictionary.Remove(key);
            }

            #endregion

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
                this._dictionary = new Dictionary<KeyValuePair<Guid, String>, Activity>();
            }

            protected ActivityCache(SerializationInfo info, StreamingContext context)
            {
                this._dictionary = (Dictionary<KeyValuePair<Guid, String>, Activity>)
                    info.GetValue("dictionary", typeof(Dictionary<KeyValuePair<Guid, String>, Activity>));
            }

            /// <summary>
            /// 指定したアクティビティからキーを抽出します。
            /// </summary>
            /// <param name="item">キーの抽出元アクティビティ。</param>
            /// <returns>指定したアクティビティのキー。</returns>
            private static KeyValuePair<Guid, String> GetKeyForItem(Activity item)
            {
                return item == null
                    ? default(KeyValuePair<Guid, String>)
                    : new KeyValuePair<Guid, String>(item.AccountId, item.Category);
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
                if (activity.CompareTo(latest) > 0)
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
                    IEnumerable<Activity> activities = this.Cache.Storage.GetActivities(
                        accountId,
                        null,
                        category,
                        null
                    );
                    Activity latest;
                    if (activities.Any() &&
                        (latest = activities.OrderByDescending(a => a).FirstOrDefault()) != null
                    )
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