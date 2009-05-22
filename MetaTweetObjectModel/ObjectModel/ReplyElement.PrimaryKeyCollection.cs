﻿// -*- mode: csharp; encoding: utf-8; -*-
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
    partial class ReplyElement
    {
        /// <summary>
        /// <see cref="ReplyElement"/> のデータのバックエンドとなる行の主キーのシーケンスを表します。このクラスは継承できません。
        /// </summary>
        [Serializable()]
        public sealed class PrimaryKeyCollection
            : Object,
              IEnumerable<Object>,
              IComparable<PrimaryKeyCollection>,
              IEquatable<PrimaryKeyCollection>
        {
            private readonly ReplyElement _element;

            /// <summary>
            /// <see cref="IReplyMapRow.AccountId"/> の値を取得または設定します。
            /// </summary>
            /// <remarks>このプロパティは <see cref="ReplyElement.Post"/> の <see cref="ObjectModel.Post.Activity"/> の <see cref="ObjectModel.Activity.Account"/> の <see cref="ObjectModel.Account.AccountId"/> に対応します。</remarks>
            public Guid AccountId
            {
                get
                {
                    return this._element.Row.AccountId;
                }
                set
                {
                    this._element.Row.AccountId = value;
                }
            }

            /// <summary>
            /// <see cref="IReplyMapRow.PostId"/> の値を取得または設定します。
            /// </summary>
            /// <remarks>このプロパティは <see cref="ReplyElement.Post"/> の <see cref="ObjectModel.Post.PostId"/> に対応します。</remarks>
            public String PostId
            {
                get
                {
                    return this._element.Row.PostId;
                }
                set
                {
                    this._element.Row.PostId = value;
                }
            }

            /// <summary>
            /// <see cref="IReplyMapRow.InReplyToAccountId"/> の値を取得または設定します。
            /// </summary>
            /// <remarks>このプロパティは <see cref="ReplyElement.InReplyToPost"/> の <see cref="ObjectModel.Post.Activity"/> の <see cref="ObjectModel.Activity.Account"/> の <see cref="ObjectModel.Account.AccountId"/> に対応します。</remarks>
            public Guid InReplyToAccountId
            {
                get
                {
                    return this._element.Row.InReplyToAccountId;
                }
                set
                {
                    this._element.Row.InReplyToAccountId = value;
                }
            }

            /// <summary>
            /// <see cref="IReplyMapRow.InReplyToPostId"/> の値を取得または設定します。
            /// </summary>
            /// <remarks>このプロパティは <see cref="ReplyElement.InReplyToPost"/> の <see cref="ObjectModel.Post.PostId"/> に対応します。</remarks>
            public String InReplyToPostId
            {
                get
                {
                    return this._element.Row.InReplyToPostId;
                }
                set
                {
                    this._element.Row.InReplyToPostId = value;
                }
            }

            /// <summary>
            /// <see cref="PrimaryKeyCollection"/> の新しいインスタンスを初期化します。
            /// </summary>
            /// <param name="element">参照する <see cref="ReplyElement"/>。</param>
            public PrimaryKeyCollection(ReplyElement element)
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
                return unchecked((((
                    this.AccountId.GetHashCode() * 397) ^
                    this.PostId.GetHashCode() * 397) ^
                    this.InReplyToAccountId.GetHashCode() * 397) ^
                    this.InReplyToPostId.GetHashCode()
                );
            }
            
            /// <summary>
            /// <see cref="PrimaryKeyCollection"/> を反復処理する列挙子を返します。 
            /// </summary>
            /// <returns>コレクションを反復処理するために使用できる <see cref="IEnumerable{Object}"/>。</returns>
            public IEnumerator<Object> GetEnumerator()
            {
                yield return this.AccountId;
                yield return this.PostId;
                yield return this.InReplyToAccountId;
                yield return this.InReplyToPostId;
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
                Int64 x;
                Int64 y;
                if ((ret = this.AccountId.CompareTo(other.AccountId)) != 0)
                {
                    return ret;
                }
                else if ((ret = this.InReplyToAccountId.CompareTo(other.InReplyToAccountId)) != 0)
                {
                    return ret;
                }
                else if (this.PostId != other.PostId)
                {
                    if (Int64.TryParse(this.PostId, out x) && Int64.TryParse(other.PostId, out y))
                    {
                        return x.CompareTo(y);
                    }
                    else
                    {
                        return this.PostId.CompareTo(other.PostId);
                    }
                }
                else
                {
                    if (Int64.TryParse(this.InReplyToPostId, out x) && Int64.TryParse(other.InReplyToPostId, out y))
                    {
                        return x.CompareTo(y);
                    }
                    else
                    {
                        return this.InReplyToPostId.CompareTo(this.InReplyToPostId);
                    }
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