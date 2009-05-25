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
        : StorageObject<StorageDataSet.FavorMapDataTable, StorageDataSet.FavorMapRow>,
          IComparable<FavorElement>,
          IEquatable<FavorElement>
    {
        [NonSerialized()]
        private readonly PrimaryKeyCollection _primaryKeys;

        private readonly InternalRow _row;

        /// <summary>
        /// この関係のデータのバックエンドとなる行の主キーのシーケンスを取得します。
        /// </summary>
        /// <value>この関係のデータのバックエンドとなる行の主キーのシーケンス。</value>
        public override IList<Object> PrimaryKeyList
        {
            get
            {
                return this.PrimaryKeys.ToList();
            }
        }

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
        public IFavorMapRow Row
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
        /// この関係のデータのバックエンドとなる行の主キーのシーケンスを表すオブジェクトを取得します。
        /// </summary>
        /// <returns>この関係のデータのバックエンドとなる行の主キーのシーケンスを表すオブジェクト。</returns>
        public PrimaryKeyCollection PrimaryKeys
        {
            get
            {
                return this._primaryKeys;
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
                return this.Storage.GetAccount(this.UnderlyingDataRow.AccountsRow);
            }
            set
            {
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
                return this.Storage.GetActivity(this.UnderlyingDataRow.ActivitiesRowParent);
            }
            set
            {
                this.UnderlyingDataRow.ActivitiesRowParent = value.UnderlyingDataRow;
            }
        }

        /// <summary>
        /// <see cref="FavorElement"/> の新しいインスタンスを初期化します。
        /// </summary>
        public FavorElement()
        {
            this._row = new InternalRow();
            this._primaryKeys = new PrimaryKeyCollection(this);
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
            return this.PrimaryKeys.GetHashCode();
        }

        /// <summary>
        /// この関係を表す <see cref="T:System.String"/> を返します。
        /// </summary>
        /// <returns>この関係を表す <see cref="T:System.String"/>。</returns>
        public override String ToString()
        {
            return String.Format("{0}: {1}", this.Account, this.FavoringActivity);
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
            return new PrimaryKeyCollection(this).CompareTo(other.PrimaryKeys);
        }

        /// <summary>
        /// この関係と、指定した別の関係が同一かどうかを判断します。
        /// </summary>
        /// <param name="other">この関係と比較する関係。</param>
        /// <returns>
        /// <paramref name="other"/> パラメータの値がこの関係と同じ場合は <c>true</c>。それ以外の場合は <c>false</c>。 
        /// </returns>
        public Boolean Equals(FavorElement other)
        {
            return this.Storage == other.Storage && this.CompareTo(other) == 0;
        }

        /// <summary>
        /// お気に入りとしてマークしている主体であるアカウントを取得します。
        /// </summary>
        /// <returns>
        /// お気に入りとしてマークしている主体であるアカウント。
        /// </returns>
        public Account GetAccount()
        {
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