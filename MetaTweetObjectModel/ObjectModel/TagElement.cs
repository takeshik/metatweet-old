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
    /// アクティビティと、アクティビティに付与されている文字列との関係を表します。
    /// </summary>
    /// <remarks>
    /// このクラスはアクティビティと文字列との関係表の単一の行要素を表現し、その集合により多対多の関係を構成します。
    /// </remarks>
    [Serializable()]
    public partial class TagElement
        : StorageObject<StorageDataSet.TagMapDataTable, StorageDataSet.TagMapRow>,
          IComparable<TagElement>,
          IEquatable<TagElement>
    {
        private readonly PrimaryKeyCollection _primaryKeys;

        /// <summary>
        /// この関係のデータのバックエンドとなるデータ行の主キーのシーケンスを取得します。
        /// </summary>
        /// <value>この関係のデータのバックエンドとなるデータ行の主キーのシーケンス。</value>
        public override IList<Object> PrimaryKeyList
        {
            get
            {
                return this.PrimaryKeys.ToList();
            }
        }

        /// <summary>
        /// この関係のデータのバックエンドとなるデータ行の主キーのシーケンスを表すオブジェクトを取得します。
        /// </summary>
        /// <returns>この関係のデータのバックエンドとなるデータ行の主キーのシーケンスを表すオブジェクト。</returns>
        public PrimaryKeyCollection PrimaryKeys
        {
            get
            {
                return this._primaryKeys;
            }
        }
        
        /// <summary>
        /// データセット内に存在する、タグを付与されている主体であるアクティビティを取得または設定します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、タグを付与されている主体であるアクティビティ。
        /// </value>
        public Activity Activity
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
        /// アクティビティに対し付与されているタグの文字列を取得または設定します。
        /// </summary>
        /// <value>
        /// アクティビティに対し付与されているタグの文字列。
        /// </value>
        public String Tag
        {
            get
            {
                return this.UnderlyingDataRow.Tag;
            }
            set
            {
                this.UnderlyingDataRow.Tag = value;
            }
        }

        /// <summary>
        /// <see cref="TagElement"/> の新しいインスタンスを初期化します。
        /// </summary>
        public TagElement()
        {
            this._primaryKeys = new PrimaryKeyCollection(this);
        }

        /// <summary>
        /// <see cref="TagElement"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="row">関係が参照するデータ列。</param>
        public TagElement(StorageDataSet.TagMapRow row)
            : this()
        {
            this.UnderlyingDataRow = row;
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
            return obj is TagElement && this.Equals(obj as TagElement);
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
            return String.Format("{0}: {1}", this.Activity, this.Tag);
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
        public Int32 CompareTo(TagElement other)
        {
            return new PrimaryKeyCollection(this).CompareTo(new PrimaryKeyCollection(other));
        }

        /// <summary>
        /// この関係と、指定した別の関係が同一かどうかを判断します。
        /// </summary>
        /// <param name="other">この関係と比較する関係。</param>
        /// <returns>
        /// <paramref name="other"/> パラメータの値がこの関係と同じ場合は <c>true</c>。それ以外の場合は <c>false</c>。 
        /// </returns>
        public Boolean Equals(TagElement other)
        {
            return this.Storage == other.Storage && this.CompareTo(other) == 0;
        }

        /// <summary>
        /// この関係を別のストレージへコピーします。
        /// </summary>
        /// <param name="destination">コピー先の <see cref="Storage"/>。</param>
        /// <returns>コピーされた関係。</returns>
        public TagElement Copy(Storage destination)
        {
            return destination.NewTagElement(
                this.GetActivity().Copy(destination),
                this.Tag
            );
        }

        /// <summary>
        /// タグを付与されている主体であるアクティビティを取得します。
        /// </summary>
        /// <returns>
        /// タグを付与されている主体であるアクティビティ。
        /// </returns>
        public Activity GetActivity()
        {
            this.Storage.LoadActivitiesDataTable(
                this.UnderlyingDataRow.AccountId,
                this.UnderlyingDataRow.Timestamp,
                this.UnderlyingDataRow.Category,
                this.UnderlyingDataRow.Subindex
            );
            return this.Activity;
        }
    }
}