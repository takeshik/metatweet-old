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
using System.Collections;
using System.Collections.Generic;

namespace XSpect.MetaTweet.ObjectModel
{
    partial class TagElement
    {
        /// <summary>
        /// <see cref="TagElement"/> のデータのバックエンドとなるデータ行の主キーのシーケンスを表します。このクラスは継承できません。
        /// </summary>
        [Serializable()]
        public sealed class PrimaryKeyCollection
            : Object,
              IEnumerable<Object>,
              IComparable<PrimaryKeyCollection>,
              IEquatable<PrimaryKeyCollection>
        {
            private readonly TagElement _element;

            /// <summary>
            /// <see cref="StorageDataSet.TagMapRow.AccountId"/> の値を取得または設定します。
            /// </summary>
            /// <remarks>このプロパティは <see cref="TagElement.Activity"/> の <see cref="ObjectModel.Activity.Account"/> の <see cref="ObjectModel.Account.AccountId"/> に対応します。</remarks>
            public Guid AccountId
            {
                get
                {
                    return this._element.UnderlyingDataRow.AccountId;
                    ;
                }
                set
                {
                    this._element.UnderlyingDataRow.AccountId = value;
                }
            }

            /// <summary>
            /// <see cref="StorageDataSet.TagMapRow.Timestamp"/> の値を取得または設定します。
            /// </summary>
            /// <remarks>このプロパティは <see cref="TagElement.Activity"/> の <see cref="ObjectModel.Activity.Timestamp"/> に対応します。</remarks>
            public DateTime Timestamp
            {
                get
                {
                    return this._element.UnderlyingDataRow.Timestamp;
                    ;
                }
                set
                {
                    this._element.UnderlyingDataRow.Timestamp = value;
                }
            }

            /// <summary>
            /// <see cref="StorageDataSet.TagMapRow.Category"/> の値を取得または設定します。
            /// </summary>
            /// <remarks>このプロパティは <see cref="TagElement.Activity"/> の <see cref="ObjectModel.Activity.Category"/> に対応します。</remarks>
            public String Category
            {
                get
                {
                    return this._element.UnderlyingDataRow.Category;
                    ;
                }
                set
                {
                    this._element.UnderlyingDataRow.Category = value;
                }
            }

            /// <summary>
            /// <see cref="StorageDataSet.TagMapRow.Subindex"/> の値を取得または設定します。
            /// </summary>
            /// <remarks>このプロパティは <see cref="TagElement.Activity"/> の <see cref="ObjectModel.Activity.Subindex"/> に対応します。</remarks>
            public Int32 Subindex
            {
                get
                {
                    return this._element.UnderlyingDataRow.Subindex;
                }
                set
                {
                    this._element.UnderlyingDataRow.Subindex = value;
                }
            }


            /// <summary>
            /// <see cref="StorageDataSet.TagMapRow.Tag"/> の値を取得または設定します。
            /// </summary>
            /// <remarks>このプロパティは <see cref="TagElement.Tag"/> に対応します。</remarks>
            public String Tag
            {
                get
                {
                    return this._element.UnderlyingDataRow.Tag;
                }
                set
                {
                    this._element.UnderlyingDataRow.Tag = value;
                }
            }

            /// <summary>
            /// <see cref="PrimaryKeyCollection"/> の新しいインスタンスを初期化します。
            /// </summary>
            /// <param name="element">参照する <see cref="TagElement"/>。</param>
            public PrimaryKeyCollection(TagElement element)
            {
                this._element = element;
            }

            /// <summary>
            /// この主キーのシーケンスと、指定した別の主キーのシーケンスが同一かどうかを判断します。
            /// </summary>
            /// <param name="obj">この主キーのシーケンスと比較するオブジェクト。</param>
            /// <returns>
            /// <paramref name="obj"/> パラメータの値がこの主キーのシーケンスと同じ場合は <c>true</c>。それ以外の場合は <c>false</c>。 
            /// </returns>
            public override Boolean Equals(Object obj)
            {
                return obj is PrimaryKeyCollection && this.Equals(obj as PrimaryKeyCollection);
            }

            /// <summary>
            /// この主キーのシーケンスのハッシュ コードを返します。 
            /// </summary>
            /// <returns>32 ビット符号付き整数ハッシュ コード。 </returns>
            public override Int32 GetHashCode()
            {
                return unchecked(((((
                    this.AccountId.GetHashCode() * 397) ^
                    this.Timestamp.GetHashCode() * 397) ^
                    this.Category.GetHashCode() * 397) ^
                    this.Subindex * 397) ^
                    this.Tag.GetHashCode()
                );
            }
            
            /// <summary>
            /// <see cref="PrimaryKeyCollection"/> を反復処理する列挙子を返します。 
            /// </summary>
            /// <returns>コレクションを反復処理するために使用できる <see cref="IEnumerable{Object}"/>。</returns>
            public IEnumerator<Object> GetEnumerator()
            {
                yield return this.AccountId;
                yield return this.Timestamp;
                yield return this.Category;
                yield return this.Subindex;
                yield return this.Tag;
            }

            /// <summary>
            /// <see cref="PrimaryKeyCollection"/> を反復処理する列挙子を返します。 
            /// </summary>
            /// <returns>コレクションを反復処理するために使用できる <see cref="IEnumerable"/>。</returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            /// <summary>
            /// この主キーのシーケンスを別の主キーのシーケンスと比較します。
            /// </summary>
            /// <param name="other">この主キーのシーケンスと比較する主キーのシーケンス。</param>
            /// <returns>
            /// 比較対象ポストの相対順序を示す 32 ビット符号付き整数。戻り値の意味は次のとおりです。<br/>
            /// 値<br/>
            /// 意味<br/>
            /// 0 より小さい値<br/>
            /// この主キーのシーケンスが <paramref name="other"/> パラメータより前に序列されるべきであることを意味します。<br/>
            /// 0<br/>
            /// この主キーのシーケンスが <paramref name="other"/> と等しいことを意味します。<br/>
            /// 0 より大きい値<br/>
            /// この主キーのシーケンスが <paramref name="other"/> パラメータより後に序列されるべきであることを意味します。<br/>
            /// </returns>
            public Int32 CompareTo(PrimaryKeyCollection other)
            {
                Int32 ret;
                if ((ret = this.AccountId.CompareTo(other.AccountId)) != 0)
                {
                    return ret;
                }
                else if ((ret = this.Timestamp.CompareTo(other.Timestamp)) != 0)
                {
                    return ret;
                }
                else if ((ret = this.Category.CompareTo(other.Category)) != 0)
                {
                    return ret;
                }
                else if ((ret = this.Subindex.CompareTo(other.Subindex)) != 0)
                {
                    return ret;
                }
                else
                {
                    return this.Tag.CompareTo(other.Tag);
                }
            }

            /// <summary>
            /// この主キーのシーケンスと、指定した別の主キーのシーケンスが同一かどうかを判断します。
            /// </summary>
            /// <param name="other">この主キーのシーケンスと比較する主キーのシーケンス。</param>
            /// <returns>
            /// <paramref name="other"/> パラメータの値がこの主キーのシーケンスと同じ場合は <c>true</c>。それ以外の場合は <c>false</c>。 
            /// </returns>
            public Boolean Equals(PrimaryKeyCollection other)
            {
                return this.CompareTo(other) == 0;
            }
        }
    }
}