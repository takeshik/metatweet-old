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
    /// アカウントと、アカウントがお気に入りとしてマークしているアクティビティとの関係を表します。
    /// </summary>
    /// <remarks>
    /// このクラスはアカウントとアクティビティとの関係表の単一の行要素を表現し、その集合により多対多の関係を構成します。
    /// </remarks>
    [Serializable()]
    public partial class FavorElement
        : StorageObject<StorageDataSet.FavorMapDataTable, IFavorMapRow, StorageDataSet.FavorMapRow>,
          IComparable<FavorElement>,
          IEquatable<FavorElement>
    {
        private InternalRow _row;

        /// <summary>
        /// データセット内に存在する、この関係の親オブジェクトのシーケンスを取得します。
        /// </summary>
        /// <value>データセット内に存在する、この関係の親オブジェクトのシーケンス。</value>
        public override IEnumerable<StorageObject> Parents
        {
            get
            {
                return new StorageObject[]
                {
                    this.Account,
                    this.FavoringActivity,
                };
            }
        }

        /// <summary>
        /// データセット内に存在する、この関係の子オブジェクトのシーケンスを取得します。
        /// </summary>
        /// <value>データセット内に存在する、この関係の子オブジェクトのシーケンス。</value>
        public override IEnumerable<StorageObject> Children
        {
            get
            {
                return Enumerable.Empty<StorageObject>();
            }
        }

        /// <summary>
        /// このオブジェクトが現在参照している列を取得します。
        /// </summary>
        /// <value>このオブジェクトが現在参照している列。</value>
        public override IFavorMapRow Row
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
        /// データセット内に存在する、お気に入りとしてマークしている主体であるアカウントを取得または設定します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、お気に入りとしてマークしている主体であるアカウント。
        /// </value>
        public Account Account
        {
            get
            {
                this.GuardIfDisconnected();
                return this.Storage.GetAccount(this.UnderlyingDataRow.AccountsRow);
            }
            set
            {
                this.GuardIfDisconnected();
                this.UnderlyingDataRow.AccountsRow = value.UnderlyingDataRow;
            }

        }

        /// <summary>
        /// データセット内に存在する、アカウントがお気に入りとしてマークしているアクティビティを取得または設定します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、アカウントがお気に入りとしてマークしているアクティビティ。
        /// </value>
        public Activity FavoringActivity
        {
            get
            {
                this.GuardIfDisconnected();
                return this.Storage.GetActivity(this.UnderlyingDataRow.ActivitiesRowParent);
            }
            set
            {
                this.GuardIfDisconnected();
                this.UnderlyingDataRow.ActivitiesRowParent = value.UnderlyingDataRow;
            }
        }

        /// <summary>
        /// 2 つの関係が等しいかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較する関係。</param>
        /// <param name="right">比較される関係。</param>
        /// <returns><paramref name="left"/> と <paramref name="right"/> が等しい場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator ==(FavorElement left, FavorElement right)
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
        /// 2 つの関係が等しくないかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較する関係。</param>
        /// <param name="right">比較される関係。</param>
        /// <returns><paramref name="left"/> と <paramref name="right"/> が等しくない場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator !=(FavorElement left, FavorElement right)
        {
            return !(left == right);
        }

        /// <summary>
        /// 一方の関係が、他方の関係より前に位置するかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較する関係。</param>
        /// <param name="right">比較される関係。</param>
        /// <returns><paramref name="left"/> が <paramref name="right"/> より前に位置する場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator <(FavorElement left, FavorElement right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// 一方の関係が、他方の関係より後ろに位置するかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較する関係。</param>
        /// <param name="right">比較される関係。</param>
        /// <returns><paramref name="left"/> が <paramref name="right"/> より後ろに位置する場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator >(FavorElement left, FavorElement right)
        {
            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// 一方の関係が、他方の関係と等しいか、または前に位置するかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較する関係。</param>
        /// <param name="right">比較される関係。</param>
        /// <returns><paramref name="left"/> が <paramref name="right"/> と等しい、または前に位置する場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator <=(FavorElement left, FavorElement right)
        {
            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// 一方の関係が、他方の関係と等しいか、または後ろに位置するかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較する関係。</param>
        /// <param name="right">比較される関係。</param>
        /// <returns><paramref name="left"/> が <paramref name="right"/> と等しい、または後ろに位置する場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator >=(FavorElement left, FavorElement right)
        {
            return left.CompareTo(right) >= 0;
        }

        /// <summary>
        /// <see cref="FavorElement"/> の新しいインスタンスを初期化します。
        /// </summary>
        public FavorElement()
        {
            this._row = new InternalRow();
        }

        /// <summary>
        /// この関係と、指定した別の関係が同一かどうかを判断します。
        /// </summary>
        /// <param name="obj">この関係と比較する関係。</param>
        /// <returns>
        /// <paramref name="obj"/> パラメータの値がこの関係と同じ場合は <c>true</c>。それ以外の場合は <c>false</c>。 
        /// </returns>
        public override Boolean Equals(Object obj)
        {
            return obj is FavorElement && this.Equals(obj as FavorElement);
        }

        /// <summary>
        /// この関係のハッシュ コードを返します。 
        /// </summary>
        /// <returns>32 ビット符号付き整数ハッシュ コード。 </returns>
        public override Int32 GetHashCode()
        {
            return unchecked(((((
                this.Row.AccountId.GetHashCode() * 397) ^
                this.Row.FavoringAccountId.GetHashCode() * 397) ^
                this.Row.FavoringTimestamp.GetHashCode() * 397) ^
                this.Row.FavoringCategory.GetHashCode() * 397) ^
                this.Row.FavoringSubindex
            );
        }

        /// <summary>
        /// この関係を表す <see cref="T:System.String"/> を返します。
        /// </summary>
        /// <returns>この関係を表す <see cref="T:System.String"/>。</returns>
        public override String ToString()
        {
            return this.IsConnected
                ? String.Format(
                      "Fav* [{0}]: [{1}]",
                      this.Account,
                      this.FavoringActivity
                  )
                : String.Format(
                      "Fav {0}: {1} @ {2}: {3}({4})",
                      this.Row.AccountId.ToString("b"),
                      this.Row.FavoringAccountId,
                      this.Row.FavoringTimestamp,
                      this.Row.FavoringCategory,
                      this.Row.FavoringSubindex
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
            if (!(other is FavorElement))
            {
                throw new ArgumentException("other");
            }
            return this.CompareTo(other as FavorElement);
        }

        /// <summary>
        /// この関係の親オブジェクトのシーケンスを取得します。
        /// </summary>
        /// <returns>この関係の親オブジェクトのシーケンス。</returns>
        public override IEnumerable<StorageObject> GetParents()
        {
            return new StorageObject[]
            {
                this.GetAccount(),
                this.GetFavoringActivity(),
            };
        }

        /// <summary>
        /// このオブジェクトの子オブジェクトのシーケンスを取得します。
        /// </summary>
        /// <returns>このオブジェクトの子オブジェクトのシーケンス。</returns>
        public override IEnumerable<StorageObject> GetChildren()
        {
            return Enumerable.Empty<StorageObject>();
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
                this._row.FavoringAccountId = this.UnderlyingDataRow.FavoringAccountId;
                this._row.FavoringTimestamp = this.UnderlyingDataRow.FavoringTimestamp;
                this._row.FavoringCategory = this.UnderlyingDataRow.FavoringCategory;
                this._row.FavoringSubindex = this.UnderlyingDataRow.FavoringSubindex;
                this.EndInit();
            }
            else
            {
                if (this._row.IsAccountIdModified)
                {
                    this.UnderlyingDataRow.AccountId = this._row.AccountId;
                }
                if (this._row.IsFavoringAccountIdModified)
                {
                    this.UnderlyingDataRow.FavoringAccountId = this._row.FavoringAccountId;
                }
                if (this._row.IsFavoringTimestampModified)
                {
                    this.UnderlyingDataRow.FavoringTimestamp = this._row.FavoringTimestamp;
                }
                if (this._row.IsFavoringCategoryModified)
                {
                    this.UnderlyingDataRow.FavoringCategory = this._row.FavoringCategory;
                }
                if (this._row.IsFavoringSubindexModified)
                {
                    this.UnderlyingDataRow.FavoringSubindex = this._row.FavoringSubindex;
                }
                this._row = null;
            }
        }

        /// <summary>
        /// この関係を別の関係と比較します。
        /// </summary>
        /// <param name="other">この関係と比較する関係。</param>
        /// <returns>
        /// 比較対象アカウントの相対順序を示す 32 ビット符号付き整数。戻り値の意味は次のとおりです。
        /// 値
        /// 意味
        /// 0 より小さい値
        /// この関係が <paramref name="other"/> パラメータより前に序列されるべきであることを意味します。
        /// 0
        /// この関係が <paramref name="other"/> と等しいことを意味します。
        /// 0 より大きい値
        /// この関係が <paramref name="other"/> パラメータより後に序列されるべきであることを意味します。
        /// </returns>
        public Int32 CompareTo(FavorElement other)
        {
            Int32 ret;
            if ((ret = this.Row.AccountId.CompareTo(other.Row.AccountId)) != 0)
            {
                return ret;
            }
            if ((ret = this.Row.FavoringAccountId.CompareTo(other.Row.FavoringAccountId)) != 0)
            {
                return ret;
            }
            else if ((ret = this.Row.FavoringTimestamp.CompareTo(other.Row.FavoringTimestamp)) != 0)
            {
                return ret;
            }
            else if ((ret = this.Row.FavoringCategory.CompareTo(other.Row.FavoringCategory)) != 0)
            {
                return ret;
            }
            else
            {
                return this.Row.FavoringSubindex.CompareTo(other.Row.FavoringSubindex);
            }
        }

        /// <summary>
        /// この関係と、指定した別の関係が同一かどうかを判断します。
        /// </summary>
        /// <param name="other">この関係と比較する関係。</param>
        /// <returns>
        /// <paramref name="other"/> パラメータの主キーの値がこの関係と同じ場合は <c>true</c>。それ以外の場合は <c>false</c>。 
        /// </returns>
        public Boolean Equals(FavorElement other)
        {
            return this.CompareTo(other) == 0;
        }

        /// <summary>
        /// この関係と、指定した別の関係が同一のデータソースを参照し、かつ、同一の値を持つかどうかを判断します。
        /// </summary>
        /// <param name="other">この関係と比較する関係。</param>
        /// <returns><paramref name="other"/> パラメータの主キーの値がこの関係と同じで、なおかつ <see cref="Storage"/> も同じ場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public Boolean ExactlyEquals(Account other)
        {
            return this.Storage == other.Storage && this.Equals(other);
        }

        /// <summary>
        /// お気に入りとしてマークしている主体であるアカウントを取得します。
        /// </summary>
        /// <returns>
        /// お気に入りとしてマークしている主体であるアカウント。
        /// </returns>
        public Account GetAccount()
        {
            this.GuardIfDisconnected();
            this.Storage.LoadAccountsDataTable(this.UnderlyingDataRow.AccountId);
            return this.Account;
        }

        /// <summary>
        /// アカウントがお気に入りとしてマークしているアクティビティを取得します。
        /// </summary>
        /// <value>
        /// アカウントがお気に入りとしてマークしているアクティビティ。
        /// </value>
        public Activity GetFavoringActivity()
        {
            this.GuardIfDisconnected();
            this.Storage.LoadActivitiesDataTable(
                this.UnderlyingDataRow.FavoringAccountId,
                this.UnderlyingDataRow.FavoringTimestamp,
                this.UnderlyingDataRow.FavoringCategory,
                this.UnderlyingDataRow.FavoringSubindex
            );
            return this.FavoringActivity;
        }
    }
}