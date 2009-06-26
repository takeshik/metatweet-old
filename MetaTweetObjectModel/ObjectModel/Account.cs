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
    /// アカウントを表します。
    /// </summary>
    /// <remarks>
    /// <para>アカウントは MetaTweet のデータ構造の頂点に位置する構造で、個々のサービスを利用するユーザを表現します。</para>
    /// <para>アカウントは <see cref="AccountId"/> によって一意に識別されます。</para>
    /// </remarks>
    [Serializable()]
    public partial class Account
        : StorageObject<StorageDataSet.AccountsDataTable, IAccountsRow, StorageDataSet.AccountsRow>,
          IComparable<Account>,
          IEquatable<Account>
    {
        private InternalRow _row;

        /// <summary>
        /// 指定されたカテゴリに属する、<see cref="Storage.Cache"/> およびデータセット内に存在する、このアカウントの最新のアクティビティの値を取得します。
        /// </summary>
        /// <param name="category">検索するカテゴリ。</param>
        /// <returns>指定されたカテゴリに属する、<see cref="Storage.Cache"/> およびデータセット内に存在する、このアカウントの最新のアクティビティの値。</returns>
        /// <remarks>
        /// このプロパティの返す値とは <see cref="Activity.Value"/> です。
        /// </remarks>
        public String this[String category]
        {
            get
            {
                return this.GetActivityInDataSetOf(category).Value;
            }
        }

        /// <summary>
        /// 基準とする日時の時点での、指定されたカテゴリに属する、このアカウントの最新のアクティビティの値を取得します。
        /// </summary>
        /// <param name="category">検索するカテゴリ。</param>
        /// <param name="baseline">検索する基準とする日時。</param>
        /// <returns>基準とする日時の時点での、指定されたカテゴリに属する、このアカウントの最新のアクティビティの値。</returns>
        /// <remarks>
        /// このプロパティの返す値とは <see cref="Activity.Value"/> です。
        /// </remarks>
        public String this[String category, DateTime baseline]
        {
            get
            {
                return this.GetActivityOf(category, baseline).Value;
            }
        }

        /// <summary>
        /// データセット内に存在する、このアカウントの親オブジェクトのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このアカウントの親オブジェクトのシーケンス。
        /// </value>
        public override IEnumerable<StorageObject> Parents
        {
            get
            {
                return Enumerable.Empty<StorageObject>();
            }
        }

        /// <summary>
        /// データセット内に存在する、このアカウントの子オブジェクトのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このアカウントの子オブジェクトのシーケンス。
        /// </value>
        public override IEnumerable<StorageObject> Children
        {
            get
            {
                return this.Activities.Cast<StorageObject>()
                    .Concat(this.FollowersMap.Cast<StorageObject>())
                    .Concat(this.FollowingMap.Cast<StorageObject>())
                    .Concat(this.FavoringMap.Cast<StorageObject>());
            }
        }

        /// <summary>
        /// このオブジェクトが現在参照している列を取得します。
        /// </summary>
        /// <value>このオブジェクトが現在参照している列。</value>
        public override IAccountsRow Row
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
        /// このアカウントを一意に識別するグローバル一意識別子 (GUID) 値を取得または設定します。
        /// </summary>
        /// <value>
        /// このアカウントを一意に識別するグローバル一意識別子 (GUID) 値。
        /// </value>
        public Guid AccountId
        {
            get
            {
                return this.Row.AccountId;
            }
            set
            {
                this.Row.AccountId = value;
            }
        }

        /// <summary>
        /// このアカウントに関連付けられているサービスを表す文字列を取得または設定します。
        /// </summary>
        /// <value>
        /// このアカウントに関連付けられているサービスを表す文字列。
        /// </value>
        /// <remarks>
        /// <para>Realm はアカウントに関連付けられているサービスを識別する要素です。Realm は <see cref="Storage"/> を利用するオブジェクトまたはユーザによって自由に指定されます。通常、Realm はサービスの完全修飾ドメイン名 (FQDN) を逆順に並べた文字列を先頭に配置し、その後に補足的な要素を連結した文字列を指定します (Java のパッケージ命名規約と同じ)。例えば、サービス example.com に関連付けられているアカウントの Realm は <c>com.example</c> が一般に用いられるべきです。</para>
        /// </remarks>
        public String Realm
        {
            get
            {
                return this.Row.Realm;
            }
            set
            {
                this.Row.Realm = value;
            }
        }

        /// <summary>
        /// データセット内に存在する、このアカウントがお気に入りとしてマークしたアクティビティとの関係のシーケンスを取得します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このアカウントがお気に入りとしてマークしたアクティビティとの関係のシーケンス。
        /// </value>
        public IEnumerable<FavorElement> FavoringMap
        {
            get
            {
                this.GuardIfDisconnected();
                return this.Storage.GetFavorElements(this.UnderlyingDataRow.GetFavorMapRows());
            }
        }

        /// <summary>
        /// データセット内に存在する、このアカウントがお気に入りとしてマークしたアクティビティのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このアカウントがお気に入りとしてマークしたアクティビティのシーケンス。
        /// </value>
        public IEnumerable<Activity> Favoring
        {
            get
            {
                return this.FavoringMap.Select(e => e.FavoringActivity);
            }
        }

        /// <summary>
        /// データセット内に存在する、このアカウントがフォローしているアカウントとの関係のシーケンスを取得します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このアカウントがフォローしているアカウントとの関係のシーケンス。
        /// </value>
        public IEnumerable<FollowElement> FollowingMap
        {
            get
            {
                this.GuardIfDisconnected();
                return this.Storage.GetFollowElements(this.UnderlyingDataRow.GetFollowMapRowsByFK_Accounts_FollowMap());
            }
        }

        /// <summary>
        /// データセット内に存在する、このアカウントがフォローしているアカウントのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このアカウントがフォローしているアカウントのシーケンス。
        /// </value>
        public IEnumerable<Account> Following
        {
            get
            {
                return this.FollowingMap.Select(e => e.FollowingAccount);
            }
        }

        /// <summary>
        /// データセット内に存在する、このアカウントがフォローされているアカウントとの関係のシーケンスを取得します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このアカウントがフォローされているアカウントとの関係のシーケンス。
        /// </value>
        public IEnumerable<FollowElement> FollowersMap
        {
            get
            {
                this.GuardIfDisconnected();
                return this.Storage.GetFollowElements(this.UnderlyingDataRow.GetFollowMapRowsByFK_AccountsFollowing_FollowMap());
            }
        }

        /// <summary>
        /// データセット内に存在する、このアカウントがフォローされているアカウントのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このアカウントがフォローされているアカウントのシーケンス。
        /// </value>
        public IEnumerable<Account> Followers
        {
            get
            {
                return this.FollowersMap.Select(e => e.Account);
            }
        }

        /// <summary>
        /// データセット内に存在する、このアカウントのアクティビティのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このアカウントのアクティビティのシーケンス。
        /// </value>
        public IEnumerable<Activity> Activities
        {
            get
            {
                this.GuardIfDisconnected();
                return this.Storage.GetActivities(this.UnderlyingDataRow.GetActivitiesRows());
            }
        }

        /// <summary>
        /// <see cref="Account"/> の新しいインスタンスを初期化します。
        /// </summary>
        public Account()
        {
            this._row = new InternalRow();
        }

        /// <summary>
        /// このアカウントと、指定した別のアカウントが同一かどうかを判断します。
        /// </summary>
        /// <param name="obj">このアカウントと比較するオブジェクト。</param>
        /// <returns>
        /// <paramref name="obj"/> パラメータの値がこのアカウントと同じ場合は <c>true</c>。それ以外の場合は <c>false</c>。 
        /// </returns>
        public override Boolean Equals(Object obj)
        {
            return obj is Account && this.Equals(obj as Account);
        }

        /// <summary>
        /// このアカウントのハッシュ コードを返します。 
        /// </summary>
        /// <returns>32 ビット符号付き整数ハッシュ コード。 </returns>
        public override Int32 GetHashCode()
        {
            return this.Row.AccountId.GetHashCode();
        }

        /// <summary>
        /// このアカウントを表す <see cref="T:System.String"/> を返します。
        /// </summary>
        /// <returns>
        /// このアカウントを表す <see cref="T:System.String"/>。
        /// </returns>
        public override String ToString()
        {
            return String.Format(
                "Acc{0} {1}@{2}",
                this.IsConnected ? "*" : String.Empty,
                this.AccountId.ToString("d"),
                this.Realm
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
            if (!(other is Account))
            {
                throw new ArgumentException("other");
            }
            return this.CompareTo(other as Account);
        }

        /// <summary>
        /// このアカウントの親オブジェクトのシーケンスを取得します。
        /// </summary>
        /// <returns>
        /// このオブジアカウントのェクトの親オブジェクトのシーケンス。
        /// </returns>
        public override IEnumerable<StorageObject> GetParents()
        {
            return Enumerable.Empty<StorageObject>();
        }

        /// <summary>
        /// このアカウントの子オブジェクトのシーケンスを取得します。
        /// </summary>
        /// <returns>
        /// このアカウントの子オブジェクトのシーケンス。
        /// </returns>
        public override IEnumerable<StorageObject> GetChildren()
        {
            return this.GetActivities().Cast<StorageObject>()
                .Concat(this.GetFollowersMap().Cast<StorageObject>())
                .Concat(this.GetFollowingMap().Cast<StorageObject>())
                .Concat(this.GetFavoringMap().Cast<StorageObject>());
        }

        /// <summary>
        /// 2 つのアカウントが等しいかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較するアカウント。</param>
        /// <param name="right">比較されるアカウント。</param>
        /// <returns><paramref name="left"/> と <paramref name="right"/> が等しい場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator ==(Account left, Account right)
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
        /// 2 つのアカウントが等しくないかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較するアカウント。</param>
        /// <param name="right">比較されるアカウント。</param>
        /// <returns><paramref name="left"/> と <paramref name="right"/> が等しくない場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator !=(Account left, Account right)
        {
            return !(left == right);
        }

        /// <summary>
        /// 一方のアカウントが、他方のアカウントより前に位置するかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較するアカウント。</param>
        /// <param name="right">比較されるアカウント。</param>
        /// <returns><paramref name="left"/> が <paramref name="right"/> より前に位置する場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator <(Account left, Account right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// 一方のアカウントが、他方のアカウントより後ろに位置するかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較するアカウント。</param>
        /// <param name="right">比較されるアカウント。</param>
        /// <returns><paramref name="left"/> が <paramref name="right"/> より後ろに位置する場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator >(Account left, Account right)
        {
            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// 一方のアカウントが、他方のアカウントと等しいか、または前に位置するかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較するアカウント。</param>
        /// <param name="right">比較されるアカウント。</param>
        /// <returns><paramref name="left"/> が <paramref name="right"/> と等しい、または前に位置する場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator <=(Account left, Account right)
        {
            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// 一方のアカウントが、他方のアカウントと等しいか、または後ろに位置するかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較するアカウント。</param>
        /// <param name="right">比較されるアカウント。</param>
        /// <returns><paramref name="left"/> が <paramref name="right"/> と等しい、または後ろに位置する場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator >=(Account left, Account right)
        {
            return left.CompareTo(right) >= 0;
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
                this._row = new InternalRow();
                this.BeginInit();
                this._row.AccountId = this.UnderlyingDataRow.AccountId;
                this._row.Realm = this.UnderlyingDataRow.Realm;
                this.EndInit();
            }
            else
            {
                if (this._row.IsAccountIdModified)
                {
                    this.UnderlyingDataRow.AccountId = this._row.AccountId;
                }
                if (this._row.IsAccountIdModified)
                {
                    this.UnderlyingDataRow.Realm = this._row.Realm;
                }
                this._row = null;
            }
        }

        /// <summary>
        /// このアカウントを別のアカウントと比較します。
        /// </summary>
        /// <param name="other">このアカウントと比較するアカウント。</param>
        /// <returns>
        /// 比較対象アカウントの相対順序を示す 32 ビット符号付き整数。戻り値の意味は次のとおりです。
        /// 値
        /// 意味
        /// 0 より小さい値
        /// このアカウントが <paramref name="other"/> パラメータより前に序列されるべきであることを意味します。
        /// 0
        /// このアカウントが <paramref name="other"/> と等しいことを意味します。
        /// 0 より大きい値
        /// このアカウントが <paramref name="other"/> パラメータより後に序列されるべきであることを意味します。
        /// </returns>
        public Int32 CompareTo(Account other)
        {
            return this.Row.AccountId.CompareTo(other.Row.AccountId);
        }

        /// <summary>
        /// このアカウントと、指定した別のアカウントが同一かどうかを判断します。
        /// </summary>
        /// <param name="other">このアカウントと比較するアカウント。</param>
        /// <returns>
        /// <paramref name="other"/> パラメータの主キーの値がこのアカウントと同じ場合は <c>true</c>。それ以外の場合は <c>false</c>。 
        /// </returns>
        public Boolean Equals(Account other)
        {
            return this.CompareTo(other) == 0;
        }

        /// <summary>
        /// このアカウントと、指定した別のアカウントが同一のデータソースを参照し、かつ、同一の値を持つかどうかを判断します。
        /// </summary>
        /// <param name="other">このアカウントと比較するアカウント。</param>
        /// <returns><paramref name="other"/> パラメータの主キーの値がこのアカウントと同じで、なおかつ <see cref="Storage"/> も同じ場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public Boolean ExactlyEquals(Account other)
        {
            return this.Storage == other.Storage && this.Equals(other);
        }

        /// <summary>
        /// このアカウントがお気に入りとしてマークしたアクティビティとの関係のシーケンスを取得します。
        /// </summary>
        /// <returns>
        /// このアカウントがお気に入りとしてマークしたアクティビティとの関係のシーケンス。
        /// </returns>
        public IEnumerable<FavorElement> GetFavoringMap()
        {
            this.GuardIfDisconnected();
            this.Storage.LoadFavorMap(this.AccountId, null, null, null, null);
            return this.FavoringMap;
        }

        /// <summary>
        /// このアカウントがお気に入りとしてマークしたアクティビティのシーケンスを取得します。
        /// </summary>
        /// <returns>
        /// このアカウントがお気に入りとしてマークしたアクティビティのシーケンス。
        /// </returns>
        public IEnumerable<Activity> GetFavoring()
        {
            return this.GetFavoringMap().Select(e => e.FavoringActivity);
        }

        /// <summary>
        /// 指定されたアクティビティをお気に入りの関係として追加します。
        /// </summary>
        /// <param name="activity">お気に入りとしてマークするアクティビティ。</param>
        public void AddFavorite(Activity activity)
        {
            this.GuardIfDisconnected();
            this.Storage.NewFavorElement(this, activity);
        }

        /// <summary>
        /// 指定されたアクティビティへのお気に入りの関係を削除します。
        /// </summary>
        /// <param name="activity">お気に入りのマークを削除するアクティビティ。</param>
        public void RemoveFavorite(Activity activity)
        {
            this.GetFavoringMap().Single(e => e.FavoringActivity == activity).Delete();
        }

        /// <summary>
        /// このアカウントがフォローしているアカウントとの関係のシーケンスを取得します。
        /// </summary>
        /// <returns>
        /// このアカウントがフォローしているアカウントとの関係のシーケンス。
        /// </returns>
        public IEnumerable<FollowElement> GetFollowingMap()
        {
            this.GuardIfDisconnected();
            this.Storage.LoadFollowMap(this.AccountId, null);
            return this.FollowingMap;
        }

        /// <summary>
        /// このアカウントがフォローしているアカウントのシーケンスを取得します。
        /// </summary>
        /// <returns>
        /// このアカウントがフォローしているアカウントのシーケンス。
        /// </returns>
        public IEnumerable<Account> GetFollowing()
        {
            return this.GetFollowingMap().Select(e => e.FollowingAccount);
        }

        /// <summary>
        /// 指定されたアカウントをフォローしている関係として追加します。
        /// </summary>
        /// <param name="account">フォローしている関係として追加するアカウント</param>
        public void AddFollowing(Account account)
        {
            this.GuardIfDisconnected();
            this.Storage.NewFollowElement(this, account);
        }

        /// <summary>
        /// 指定されたアカウントとのフォローしている関係を削除します。
        /// </summary>
        /// <param name="account">フォローしている関係を削除するアカウント。</param>
        public void RemoveFollowing(Account account)
        {
            this.GetFollowingMap().Single(e => e.FollowingAccount == account).Delete();
        }

        /// <summary>
        /// このアカウントがフォローされているアカウントとの関係のシーケンスを取得します。
        /// </summary>
        /// <returns>
        /// このアカウントがフォローされているアカウントとの関係のシーケンス。
        /// </returns>
        public IEnumerable<FollowElement> GetFollowersMap()
        {
            this.GuardIfDisconnected();
            this.Storage.LoadFollowMap(null, this.AccountId);
            return this.FollowersMap;
        }

        /// <summary>
        /// このアカウントがフォローされているアカウントのシーケンスを取得します。
        /// </summary>
        /// <returns>
        /// このアカウントがフォローされているアカウントのシーケンス。
        /// </returns>
        public IEnumerable<Account> GetFollowers()
        {
            return this.GetFollowersMap().Select(e => e.Account);
        }

        /// <summary>
        /// 指定されたアカウントをフォローされている関係として追加します。
        /// </summary>
        /// <param name="account">フォローされている関係として追加するアカウント</param>
        public void AddFollower(Account account)
        {
            this.GuardIfDisconnected();
            this.Storage.NewFollowElement(account, this);
        }

        /// <summary>
        /// 指定されたアカウントからのフォローされている関係を削除します。
        /// </summary>
        /// <param name="account">フォローされている関係を削除されるアカウント。</param>
        public void RemoveFollower(Account account)
        {
            this.GetFollowersMap().Single(e => e.Account == account).Delete();
        }

        /// <summary>
        /// このアカウントのアクティビティのシーケンスを取得します。
        /// </summary>
        /// <returns>
        /// このアカウントのアクティビティのシーケンス。
        /// </returns>
        public IEnumerable<Activity> GetActivities()
        {
            this.GuardIfDisconnected();
            this.Storage.LoadActivities(this.AccountId, null, null, null);
            return this.Activities;
        }

        /// <summary>
        /// このアカウントに新しいアクティビティを追加します。
        /// </summary>
        /// <param name="timestamp"><see cref="Activity.Timestamp"/> の値。</param>
        /// <param name="category"><see cref="Activity.Category"/> の値。</param>
        /// <returns>新しいアクティビティ。</returns>
        public Activity NewActivity(DateTime timestamp, String category)
        {
            this.GuardIfDisconnected();
            return this.Storage.NewActivity(this, timestamp, category);
        }

        /// <summary>
        /// 指定されたカテゴリに属する、このアカウントの最新のアクティビティを取得します。
        /// </summary>
        /// <param name="category">検索するカテゴリ。</param>
        /// <returns>指定されたカテゴリに属する、アカウントの最新のアクティビティ。</returns>
        public Activity GetActivityOf(String category)
        {
            this.GuardIfDisconnected();
            return this.Storage.Cache.Activities.GetLatestActivity(this.Row.AccountId, category);
        }

        /// <summary>
        /// 基準とする日時の時点での、指定されたカテゴリに属する、このアカウントの最新のアクティビティの値を取得します。
        /// </summary>
        /// <param name="category">検索するカテゴリ。</param>
        /// <param name="baseline">検索する基準とする日時。</param>
        /// <returns>基準とする日時の時点での、指定されたカテゴリに属する、このアカウントの最新のアクティビティ。</returns>
        public Activity GetActivityOf(String category, DateTime baseline)
        {
            this.GuardIfDisconnected();
            this.Storage.LoadActivities(String.Format(
                "WHERE [AccountId] == '{0}' AND [Timestamp] < datetime('{1}') AND [Category] == '{2}' ORDER BY [Timestamp] DESC, [Subindex] DESC LIMIT 1",
                this.AccountId.ToString("d"),
                baseline.ToString("s"),
                category
            ));
            return this.GetActivityInDataSetOf(category, baseline);
        }

        /// <summary>
        /// 指定されたカテゴリに属する、<see cref="Storage.Cache"/> およびデータセット内に存在する、このアカウントの最新のアクティビティを取得します。
        /// </summary>
        /// <param name="category">検索するカテゴリ。</param>
        /// <returns>指定されたカテゴリに属する、<see cref="Storage.Cache"/> およびデータセット内に存在する、アカウントの最新のアクティビティ。</returns>
        public Activity GetActivityInDataSetOf(String category)
        {
            this.GuardIfDisconnected();
            return this.Storage.Cache.Activities.GetLatestActivityInDataSet(this.Row.AccountId, category);
        }

        /// <summary>
        /// 基準とする日時の時点での、指定されたカテゴリに属する、データセット内に存在する、このアカウントの最新のアクティビティの値を取得します。
        /// </summary>
        /// <param name="category">検索するカテゴリ。</param>
        /// <param name="baseline">検索する基準とする日時。</param>
        /// <returns>基準とする日時の時点での、指定されたカテゴリに属する、データセット内に存在する、このアカウントの最新のアクティビティ。</returns>
        public Activity GetActivityInDataSetOf(String category, DateTime baseline)
        {
            return this.Activities
                .Where(a => a.Category == category && a.Timestamp <= baseline)
                .OrderByDescending(a => a.Timestamp)
                .ThenByDescending(a => a.Subindex)
                .FirstOrDefault();
        }
    }
}