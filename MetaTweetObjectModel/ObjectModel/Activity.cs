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
using System.Linq;

namespace XSpect.MetaTweet.ObjectModel
{
    /// <summary>
    /// アクティビティを表します。
    /// </summary>
    /// <remarks>
    /// <para>アクティビティはアカウントの行動を表現します。行動には、名前などを含むユーザ情報の変更および発言の投稿を含みます。個々のアクティビティは行動が行われた日時、行動の種別を表す文字列、サブインデックス (同一日付種別内での行動を一意に識別する数値)、文字列およびバイト列の値によって構成されます。</para>
    /// <para>アクティビティは <see cref="Account"/>、<see cref="Timestamp"/>、<see cref="Category"/> および <see cref="Subindex"/> によって一意に識別されます。</para>
    /// </remarks>
    [Serializable()]
    public partial class Activity
        : StorageObject<StorageDataSet.ActivitiesDataTable, StorageDataSet.ActivitiesRow>,
          IComparable<Activity>,
          IEquatable<Activity>
    {
        [NonSerialized()]
        private readonly PrimaryKeyCollection _primaryKeys;

        private readonly InternalRow _row;

        /// <summary>
        /// このアクティビティのデータのバックエンドとなる行の主キーのシーケンスを取得します。
        /// </summary>
        /// <value>このアクティビティのデータのバックエンドとなる行の主キーのシーケンス。</value>
        public override IList<Object> PrimaryKeyList
        {
            get
            {
                return this.PrimaryKeys.ToList();
            }
        }

        /// <summary>
        /// データセット内に存在する、このアクティビティの親オブジェクトのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このアクティビティの親オブジェクトのシーケンス。
        /// </value>
        public override IEnumerable<StorageObject> Parents
        {
            get
            {
                return new StorageObject[]
                {
                    this.Account,
                };
            }
        }

        /// <summary>
        /// データセット内に存在する、このアクティビティの子オブジェクトのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このアクティビティの子オブジェクトのシーケンス。
        /// </value>
        public override IEnumerable<StorageObject> Children
        {
            get
            {
                Post post = this.Post;
                return (post == null
                    ? Enumerable.Empty<StorageObject>()
                    : new StorageObject[]
                      {
                          post,
                      }
                )
                    .Concat(this.FavorersMap.Cast<StorageObject>())
                    .Concat(this.TagMap.Cast<StorageObject>());
            }
        }

        /// <summary>
        /// このオブジェクトが現在参照している列を取得します。
        /// </summary>
        /// <value>このオブジェクトが現在参照している列。</value>
        public IActivitiesRow Row
        {
            get
            {
                if (this.IsConnected)
                {
                    return this.UnderlyingDataRow;
                }
                else
                {
                    return this._row;
                }
            }
        }

        /// <summary>
        /// このアクティビティのデータのバックエンドとなる行の主キーのシーケンスを表すオブジェクトを取得します。
        /// </summary>
        /// <returns>このアクティビティのデータのバックエンドとなる行の主キーのシーケンスを表すオブジェクト。</returns>
        public PrimaryKeyCollection PrimaryKeys
        {
            get
            {
                return this._primaryKeys;
            }
        }

        /// <summary>
        /// データセット内に存在する、このアクティビティの主体であるアカウントを取得または設定します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このアクティビティの主体であるアカウント。
        /// </value>
        public Account Account
        {
            get
            {
                return this.Storage.GetAccount(this.UnderlyingDataRow.AccountsRow);
            }
            set
            {
                this.UnderlyingDataRow.AccountsRow = value.UnderlyingDataRow;
            }
        }
        
        /// <summary>
        /// このアクティビティの行われた日時を取得または設定します。
        /// </summary>
        /// <value>
        /// このアクティビティの行われた日時。
        /// </value>
        /// <remarks>
        /// 日時は協定世界時 (UTC) として表されます。
        /// </remarks>
        public DateTime Timestamp
        {
            get
            {
                return this.UnderlyingDataRow.Timestamp;
            }
            set
            {
                // TODO: this.IsStored && ... ?
                if (this.Storage.Cache.Activies.Contains(this) && value < this.Timestamp)
                {
                    this.Storage.Cache.Activies.Remove(this);
                }
                this.UnderlyingDataRow.Timestamp = value;
            }
        }

        /// <summary>
        /// このアクティビティの種別を表す文字列を取得または設定します。
        /// </summary>
        /// <value>
        /// このアクティビティの種別を表す文字列。
        /// </value>
        /// <remarks>
        /// どのような文字列を種別として使用するかに関しては、はインスタンスを操作する側が自由に決定できます。
        /// </remarks>
        public String Category
        {
            get
            {
                return this.UnderlyingDataRow.Category;
            }
            set
            {
                this.UnderlyingDataRow.Category = value;
            }
        }

        /// <summary>
        /// このアクティビティのサブインデックスを取得または設定します。
        /// </summary>
        /// <value>
        /// このアクティビティのサブインデックス。
        /// </value>
        /// <remarks>
        /// <para>サブインデックスとは、<see cref="Account"/>、<see cref="Timestamp"/>、<see cref="Category"/> が同一のアクティビティをそれぞれ一意に識別するための整数値を指します。</para>
        /// <para>サブインデックスは 0 から開始します。<see cref="Account"/>、<see cref="Timestamp"/>、および <see cref="Category"/> が同一な他のアクティビティが存在しない場合、そのアクティビティのサブインデックスは常に 0 です。同一な他のアクティビティが追加された場合、追加される順にインクリメントされたサブインデックスが設定されます。</para>
        /// </remarks>
        public Int32 Subindex
        {
            get
            {
                return this.UnderlyingDataRow.Subindex;
            }
            set
            {
                this.UnderlyingDataRow.Subindex = value;
            }
        }

        /// <summary>
        /// このアクティビティに関連付けられている文字列の値を取得または設定します。
        /// </summary>
        /// <value>
        /// このアクティビティに関連付けられている文字列の値。存在しない場合は <c>null</c>。
        /// </value>
        public String Value
        {
            get
            {
                return this.UnderlyingDataRow.IsValueNull()
                    ? null
                    : this.UnderlyingDataRow.Value;
            }
            set
            {
                if (value != null)
                {
                    this.UnderlyingDataRow.Value = value;
                }
                else
                {
                    this.UnderlyingDataRow.SetValueNull();
                }
            }
        }

        /// <summary>
        /// このアクティビティに関連付けられているバイト列の値を取得または設定します。
        /// </summary>
        /// <value>
        /// このアクティビティに関連付けられているバイト列の値。存在しない場合は <c>null</c>。
        /// </value>
        public Byte[] Data
        {
            get
            {
                return this.UnderlyingDataRow.IsDataNull()
                    ? null
                    : this.UnderlyingDataRow.Data;
            }
            set
            {
                if (value != null)
                {
                    this.UnderlyingDataRow.Data = value;
                }
                else
                {
                    this.UnderlyingDataRow.SetDataNull();
                }
            }
        }

        /// <summary>
        /// このアクティビティと一対一で関連付けられるポストを取得します。
        /// </summary>
        /// <returns>このアクティビティと一対一で関連付けられるポスト。存在しない場合、または <see cref="Category"/> が <c>Post</c> 以外の場合は <c>null</c>。</returns>
        /// <remarks>
        /// カテゴリが Post であるアクティビティはポストと関連付けられます。このメソッドはカテゴリが Post であるメソッドにおいて関連付けられた <see cref="Post"/> を取得し、または存在しない場合新しく作成し、それを返します。カテゴリが Post 以外の場合は例外 <see cref="InvalidOperationException"/> がスローされます。
        /// </remarks>
        /// <seealso cref="Post"/>
        public Post Post
        {
            get
            {
                return this.Category != "Post"
                    ? null
                    : this.Storage.GetPost(
                          this.UnderlyingDataRow.GetPostsRows().SingleOrDefault()
                      );
            }
        }

        /// <summary>
        /// データセット内に存在する、このアクティビティをお気に入りとしているアカウントとの関係のシーケンスを取得します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このアクティビティをお気に入りとしているアカウントとの関係のシーケンス。
        /// </value>
        public IEnumerable<FavorElement> FavorersMap
        {
            get
            {
                return this.Storage.GetFavorElements(this.UnderlyingDataRow.GetFavorMapRows());
            }
        }

        /// <summary>
        /// データセット内に存在する、このアクティビティをお気に入りとしているアカウントのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このアクティビティをお気に入りとしているアカウントのシーケンス。
        /// </value>
        public IEnumerable<Account> Favorers
        {
            get
            {
                return this.FavorersMap.Select(e => e.Account);
            }
        }

        /// <summary>
        /// データセット内に存在する、このアクティビティに付与されているタグとの関係のシーケンスを取得します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このアクティビティに付与されているタグとの関係のシーケンス。
        /// </value>
        public IEnumerable<TagElement> TagMap
        {
            get
            {
                return this.Storage.GetTagElements(this.UnderlyingDataRow.GetTagMapRows());
            }
        }

        /// <summary>
        /// データセット内に存在する、このアクティビティに付与されているタグのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このアクティビティに付与されているタグのシーケンス。
        /// </value>
        public IEnumerable<String> Tags
        {
            get
            {
                return this.TagMap.Select(e => e.Tag);
            }
        }

        /// <summary>
        /// 2 つのアクティビティが等しいかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較するアクティビティ。</param>
        /// <param name="right">比較されるアクティビティ。</param>
        /// <returns><paramref name="left"/> と <paramref name="right"/> が等しい場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator ==(Activity left, Activity right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }
            else if ((Object) left == null || (Object) right == null)
            {
                return false;
            }
            else
            {
                return left.Equals(right);
            }
        }

        /// <summary>
        /// 2 つのアクティビティが等しくないかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較するアクティビティ。</param>
        /// <param name="right">比較されるアクティビティ。</param>
        /// <returns><paramref name="left"/> と <paramref name="right"/> が等しくない場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator !=(Activity left, Activity right)
        {
            return !(left == right);
        }

        /// <summary>
        /// 一方のアクティビティが、他方のアクティビティより前に位置するかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較するアクティビティ。</param>
        /// <param name="right">比較されるアクティビティ。</param>
        /// <returns><paramref name="left"/> が <paramref name="right"/> より前に位置する場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator <(Activity left, Activity right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// 一方のアクティビティが、他方のアクティビティより後ろに位置するかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較するアクティビティ。</param>
        /// <param name="right">比較されるアクティビティ。</param>
        /// <returns><paramref name="left"/> が <paramref name="right"/> より後ろに位置する場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator >(Activity left, Activity right)
        {
            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// 一方のアクティビティが、他方のアクティビティと等しいか、または前に位置するかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較するアクティビティ。</param>
        /// <param name="right">比較されるアクティビティ。</param>
        /// <returns><paramref name="left"/> が <paramref name="right"/> と等しい、または前に位置する場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator <=(Activity left, Activity right)
        {
            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// 一方のアクティビティが、他方のアクティビティと等しいか、または後ろに位置するかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較するアクティビティ。</param>
        /// <param name="right">比較されるアクティビティ。</param>
        /// <returns><paramref name="left"/> が <paramref name="right"/> と等しい、または後ろに位置する場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator >=(Activity left, Activity right)
        {
            return left.CompareTo(right) >= 0;
        }

        /// <summary>
        /// <see cref="Activity"/> の新しいインスタンスを初期化します。
        /// </summary>
        public Activity()
        {
            this._row = new InternalRow();
            this._primaryKeys = new PrimaryKeyCollection(this);
        }

        /// <summary>
        /// このアクティビティと、指定した別のアカウントが同一かどうかを判断します。
        /// </summary>
        /// <param name="obj">このアクティビティと比較するオブジェクト。</param>
        /// <returns>
        /// <paramref name="obj"/> パラメータの値がこのアクティビティと同じ場合は <c>true</c>。それ以外の場合は <c>false</c>。 
        /// </returns>
        public override Boolean Equals(Object obj)
        {
            return obj is Activity && this.Equals(obj as Activity);
        }

        /// <summary>
        /// このアクティビティのハッシュ コードを返します。 
        /// </summary>
        /// <returns>32 ビット符号付き整数ハッシュ コード。 </returns>
        public override Int32 GetHashCode()
        {
            return this.PrimaryKeys.GetHashCode();
        }

        /// <summary>
        /// このアクティビティを表す <see cref="T:System.String"/> を返します。
        /// </summary>
        /// <returns>
        /// このアクティビティを表す <see cref="T:System.String"/>。
        /// </returns>
        public override String ToString()
        {
            return String.Format(
                "{0}: {1} = \"{2}\"",
                this.Timestamp.ToString("s"),
                this.Category,
                this.Value ?? "(null)"
            );
        }

        /// <summary>
        /// 現在のオブジェクトを同じ型の別のオブジェクトと比較します。
        /// </summary>
        /// <param name="other">このオブジェクトと比較するオブジェクト。</param>
        /// <returns>
        /// 比較対象オブジェクトの相対順序を示す 32 ビット符号付き整数。戻り値の意味は次のとおりです。
        /// 値
        /// 意味
        /// 0 より小さい値
        /// このオブジェクトが <paramref name="other"/> パラメータより小さいことを意味します。
        /// 0
        /// このオブジェクトが <paramref name="other"/> と等しいことを意味します。
        /// 0 より大きい値
        /// このオブジェクトが <paramref name="other"/> よりも大きいことを意味します。
        /// </returns>
        public override Int32 CompareTo(StorageObject other)
        {
            if (!(other is Activity))
            {
                throw new ArgumentException("other");
            }
            return this.CompareTo(other as Activity);
        }

        /// <summary>
        /// このアクティビティの親オブジェクトのシーケンスを取得します。
        /// </summary>
        /// <returns>
        /// このアクティビティの親オブジェクトのシーケンス。
        /// </returns>
        public override IEnumerable<StorageObject> GetParents()
        {
            return new StorageObject[]
            {
                this.GetAccount(),
            };
        }

        /// <summary>
        /// このアクティビティの子オブジェクトのシーケンスを取得します。
        /// </summary>
        /// <returns>
        /// このアクティビティの子オブジェクトのシーケンス。
        /// </returns>
        public override IEnumerable<StorageObject> GetChildren()
        {
            return (this.Category != "Post"
                ? Enumerable.Empty<StorageObject>()
                : new StorageObject[]
                  {
                      this.GetPost(),
                  }
            )
                .Concat(this.GetFavorersMap().Cast<StorageObject>())
                .Concat(this.GetTagMap().Cast<StorageObject>());
        }

        /// <summary>
        /// 初期化の開始を通知するシグナルをオブジェクトに送信します。
        /// </summary>
        public override void BeginInit()
        {
            this._row.BeginInit();
        }

        /// <summary>
        /// 初期化の完了を通知するシグナルをオブジェクトに送信します。
        /// </summary>
        public override void EndInit()
        {
            this._row.EndInit();
        }

        /// <summary>
        /// このオブジェクトが現在参照している列の内容で、このオブジェクトが他に参照する可能性のある列の内容を上書きします。
        /// </summary>
        protected override void Synchronize()
        {
            if (this.IsConnected)
            {
                this.BeginInit();
                this._row.AccountId = this.UnderlyingDataRow.AccountId;
                this._row.Timestamp = this.UnderlyingDataRow.Timestamp;
                this._row.Category = this.UnderlyingDataRow.Category;
                this._row.Subindex = this.UnderlyingDataRow.Subindex;
                this._row.Value = this.UnderlyingDataRow.IsValueNull() ? null : this.UnderlyingDataRow.Value;
                this._row.Data = this.UnderlyingDataRow.IsDataNull() ? null : this.UnderlyingDataRow.Data;
                this.EndInit();
            }
            else
            {
                if (this._row.IsAccountIdModified)
                {
                    this.UnderlyingDataRow.AccountId = this._row.AccountId;
                }
                if (this._row.IsTimestampModified)
                {
                    this.UnderlyingDataRow.Timestamp = this._row.Timestamp;
                }
                if (this._row.IsCategoryModified)
                {
                    this.UnderlyingDataRow.Category = this._row.Category;
                }
                if (this._row.IsSubindexModified)
                {
                    this.UnderlyingDataRow.Subindex = this._row.Subindex;
                }
                if (this._row.IsValueModified)
                {
                    this.UnderlyingDataRow.Value = this._row.Value;
                }
                if (this._row.IsDataModified)
                {
                    this.UnderlyingDataRow.Data = this._row.Data;
                }
            }
        }

        /// <summary>
        /// このアクティビティを別のアクティビティと比較します。
        /// </summary>
        /// <param name="other">このアクティビティと比較するアクティビティ。</param>
        /// <returns>
        /// 比較対象アクティビティの相対順序を示す 32 ビット符号付き整数。戻り値の意味は次のとおりです。<br/>
        /// 値<br/>
        /// 意味<br/>
        /// 0 より小さい値<br/>
        /// このアクティビティが <paramref name="other"/> パラメータより前に序列されるべきであることを意味します。<br/>
        /// 0<br/>
        /// このアクティビティが <paramref name="other"/> と等しいことを意味します。<br/>
        /// 0 より大きい値<br/>
        /// このアクティビティが <paramref name="other"/> パラメータより後に序列されるべきであることを意味します。<br/>
        /// </returns>
        public virtual Int32 CompareTo(Activity other)
        {
            return new PrimaryKeyCollection(this).CompareTo(other.PrimaryKeys);
        }

        /// <summary>
        /// このアクティビティと、指定した別のアクティビティが同一かどうかを判断します。
        /// </summary>
        /// <param name="other">このアクティビティと比較するアクティビティ。</param>
        /// <returns>
        /// <paramref name="other"/> パラメータの主キーの値がこのアクティビティと同じ場合は <c>true</c>。それ以外の場合は <c>false</c>。 
        /// </returns>
        public Boolean Equals(Activity other)
        {
            return this.CompareTo(other) == 0;
        }

        /// <summary>
        /// このアクティビティと、指定した別のアクティビティが同一のデータソースを参照し、かつ、同一の値を持つかどうかを判断します。
        /// </summary>
        /// <param name="other">このアクティビティと比較するアクティビティ。</param>
        /// <returns><paramref name="other"/> パラメータの主キーの値がこのアクティビティと同じで、なおかつ <see cref="Storage"/> も同じ場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public Boolean ExactlyEquals(Account other)
        {
            return this.Storage == other.Storage && this.Equals(other);
        }

        /// <summary>
        /// <see cref="Subindex"/> 値を再設定します。
        /// </summary>
        /// <returns>再設定された <see cref="Subindex"/> の値。</returns>
        public Int32 FixSubindex()
        {
            this.Storage.LoadActivitiesDataTable(
                this.PrimaryKeys.AccountId, this.Timestamp, this.Category, null
            );
            this.Subindex = this.Storage.GetActivities(
                this.PrimaryKeys.AccountId, this.Timestamp, this.Category, null
            ).Count();
            return this.Subindex;
        }

        /// <summary>
        /// このアクティビティの主体であるアカウントを取得します。
        /// </summary>
        /// <returns>
        /// このアクティビティの主体であるアカウント。
        /// </returns>
        public Account GetAccount()
        {
            this.Storage.LoadAccountsDataTable(this.UnderlyingDataRow.AccountId);
            return this.Account;
        }

        /// <summary>
        /// このアクティビティをお気に入りとしているアカウントとの関係のシーケンスを取得します。
        /// </summary>
        /// <returns>
        /// このアクティビティをお気に入りとしているアカウントとの関係のシーケンス。
        /// </returns>
        public IEnumerable<FavorElement> GetFavorersMap()
        {
            this.Storage.LoadFavorMapDataTable(null, this.UnderlyingDataRow.AccountId, this.Timestamp, this.Category, this.Subindex);
            return this.FavorersMap;
        }

        /// <summary>
        /// このアクティビティをお気に入りとしているアカウントのシーケンスを取得します。
        /// </summary>
        /// <returns>
        /// このアクティビティをお気に入りとしているアカウントのシーケンス。
        /// </returns>
        public IEnumerable<Account> GetFavorers()
        {
            return this.GetFavorersMap().Select(e => e.Account);
        }

        /// <summary>
        /// 指定されたアカウントをこのアクティビティをお気に入りとしている関係として追加します。
        /// </summary>
        /// <param name="account">お気に入りとしてマークしている関係として追加するアカウント</param>
        public void AddFavorer(Account account)
        {
            this.Storage.NewFavorElement(account, this);
        }

        /// <summary>
        /// 指定されたアカウントとのお気に入りとしている関係を削除します。
        /// </summary>
        /// <param name="account">お気に入りとしている関係を削除するアカウント</param>
        public void RemoveFavorer(Account account)
        {
            this.GetFavorersMap().Single(e => e.Account == account).Delete();
        }

        /// <summary>
        /// このアクティビティに付与されているタグとの関係のシーケンスを取得します。
        /// </summary>
        /// <returns>
        /// このアクティビティに付与されているタグとの関係のシーケンス。
        /// </returns>
        public IEnumerable<TagElement> GetTagMap()
        {
            this.Storage.LoadTagMapDataTable(this.UnderlyingDataRow.AccountId, this.Timestamp, this.Category, this.Subindex, null);
            return this.TagMap;
        }

        /// <summary>
        /// このアクティビティに付与されているタグのシーケンスを取得します。
        /// </summary>
        /// <returns>
        /// このアクティビティに付与されているタグのシーケンス。
        /// </returns>
        public IEnumerable<String> GetTags()
        {
            return this.GetTagMap().Select(e => e.Tag);
        }

        /// <summary>
        /// 指定された文字列をこのアクティビティにタグとして付与します。
        /// </summary>
        /// <param name="tag">タグとして付与する文字列。</param>
        public void AddTag(String tag)
        {
            this.Storage.NewTagElement(this, tag);
        }

        /// <summary>
        /// 指定された文字列のタグをこのアクティビティから剥奪します。
        /// </summary>
        /// <param name="tag">剥奪するタグの文字列。</param>
        public void RemoveTag(String tag)
        {
            this.GetTagMap().Single(e => e.Tag == tag).Delete();
        }

        /// <summary>
        /// このアクティビティと一対一で関連付けられるポストを取得します。存在しない場合は、新たに作成されます。
        /// </summary>
        /// <returns>このアクティビティと一対一で関連付けられるポスト。存在しなかった場合は、新たに作成されたポスト。</returns>
        /// <exception cref="InvalidOperationException">
        /// <see cref="Category"/> が Post ではありません。
        /// </exception>
        /// <remarks>
        /// カテゴリが Post であるアクティビティはポストと関連付けられます。このメソッドはカテゴリが Post であるメソッドにおいて関連付けられた <see cref="Post"/> を取得し、または存在しない場合新しく作成し、それを返します。カテゴリが Post 以外の場合は例外 <see cref="InvalidOperationException"/> がスローされます。
        /// </remarks>
        /// <seealso cref="Post"/>
        public Post GetPost()
        {
            if (this.Category != "Post")
            {
                throw new InvalidOperationException("This activity's category is not \"Post\".");
            }
            this.Storage.LoadPostsDataTable(this.UnderlyingDataRow.AccountId, this.Value);
            return this.Post ?? this.Storage.NewPost(this);
        }
    }
}