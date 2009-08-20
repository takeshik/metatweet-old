// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
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
    /// ポストと、ポストの返信元のポストとの関係を表します。
    /// </summary>
    /// <remarks>
    /// このクラスは一方のポストと他方のポストとの関係表の単一の行要素を表現し、その集合により多対多の関係を構成します。
    /// </remarks>
    [Serializable()]
    public partial class ReplyElement
        : StorageObject<StorageDataSet.ReplyMapDataTable, IReplyMapRow, StorageDataSet.ReplyMapRow>,
          IComparable<ReplyElement>,
          IEquatable<ReplyElement>
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
                    this.Post,
                    this.InReplyToPost,
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
        public override IReplyMapRow Row
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
        /// データセット内に存在する、返信している主体であるポストを取得または設定します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、返信している主体であるポスト。
        /// </value>
        public Post Post
        {
            get
            {
                this.GuardIfDisconnected();
                return this.Storage.GetPost(this.UnderlyingDataRow.PostsRowParentByFK_Posts_ReplyMap);
            }
            set
            {
                this.GuardIfDisconnected();
                this.UnderlyingDataRow.PostsRowParentByFK_Posts_ReplyMap = value.UnderlyingDataRow;
            }

        }

        /// <summary>
        /// データセット内に存在する、ポストの返信元のポストを取得または設定します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、ポストの返信元のポスト。
        /// </value>
        public Post InReplyToPost
        {
            get
            {
                this.GuardIfDisconnected();
                return this.Storage.GetPost(this.UnderlyingDataRow.PostsRowParentByFK_PostsInReplyTo_ReplyMap);
            }
            set
            {
                this.GuardIfDisconnected();
                this.UnderlyingDataRow.PostsRowParentByFK_PostsInReplyTo_ReplyMap = value.UnderlyingDataRow;
            }
        }

        /// <summary>
        /// 2 つの関係が等しいかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較する関係。</param>
        /// <param name="right">比較される関係。</param>
        /// <returns><paramref name="left"/> と <paramref name="right"/> が等しい場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator ==(ReplyElement left, ReplyElement right)
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
        public static Boolean operator !=(ReplyElement left, ReplyElement right)
        {
            return !(left == right);
        }

        /// <summary>
        /// 一方の関係が、他方の関係より前に位置するかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較する関係。</param>
        /// <param name="right">比較される関係。</param>
        /// <returns><paramref name="left"/> が <paramref name="right"/> より前に位置する場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator <(ReplyElement left, ReplyElement right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// 一方の関係が、他方の関係より後ろに位置するかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較する関係。</param>
        /// <param name="right">比較される関係。</param>
        /// <returns><paramref name="left"/> が <paramref name="right"/> より後ろに位置する場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator >(ReplyElement left, ReplyElement right)
        {
            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// 一方の関係が、他方の関係と等しいか、または前に位置するかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較する関係。</param>
        /// <param name="right">比較される関係。</param>
        /// <returns><paramref name="left"/> が <paramref name="right"/> と等しい、または前に位置する場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator <=(ReplyElement left, ReplyElement right)
        {
            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// 一方の関係が、他方の関係と等しいか、または後ろに位置するかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較する関係。</param>
        /// <param name="right">比較される関係。</param>
        /// <returns><paramref name="left"/> が <paramref name="right"/> と等しい、または後ろに位置する場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator >=(ReplyElement left, ReplyElement right)
        {
            return left.CompareTo(right) >= 0;
        }

        /// <summary>
        /// <see cref="ReplyElement"/> の新しいインスタンスを初期化します。
        /// </summary>
        public ReplyElement()
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
            return obj is ReplyElement && this.Equals(obj as ReplyElement);
        }

        /// <summary>
        /// この関係のハッシュ コードを返します。 
        /// </summary>
        /// <returns>32 ビット符号付き整数ハッシュ コード。 </returns>
        public override Int32 GetHashCode()
        {
            return unchecked((((
                this.Row.AccountId.GetHashCode() * 397) ^
                this.Row.PostId.GetHashCode() * 397) ^
                this.Row.InReplyToAccountId.GetHashCode() * 397) ^
                this.Row.InReplyToPostId.GetHashCode()
            );
        }

        /// <summary>
        /// この関係を表す <see cref="T:System.String"/> を返します。
        /// </summary>
        /// <returns>
        /// この関係を表す <see cref="T:System.String"/>。
        /// </returns>
        public override String ToString()
        {
            return this.IsConnected
                ? String.Format(
                      "Rep* [{0}] => [{1}]",
                      this.Post,
                      this.InReplyToPost
                  )
                : String.Format(
                      "Rep {0} #{1} => {2} #{3}",
                      this.Row.AccountId.ToString("b"),
                      this.Row.PostId,
                      this.Row.InReplyToAccountId.ToString("b"),
                      this.Row.InReplyToPostId
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
            if (!(other is ReplyElement))
            {
                throw new ArgumentException("other");
            }
            return this.CompareTo(other as ReplyElement);
        }

        /// <summary>
        /// この関係の親オブジェクトのシーケンスを取得します。
        /// </summary>
        /// <returns>この関係の親オブジェクトのシーケンス。</returns>
        public override IEnumerable<StorageObject> GetParents()
        {
            return new StorageObject[]
            {
                this.GetPost(),
                this.GetInReplyToPost(),
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
                this._row.PostId = this.UnderlyingDataRow.PostId;
                this._row.InReplyToAccountId = this.UnderlyingDataRow.InReplyToAccountId;
                this._row.InReplyToPostId = this.UnderlyingDataRow.InReplyToPostId;
                this.EndInit();
            }
            else
            {
                if (this._row.IsAccountIdModified)
                {
                    this.UnderlyingDataRow.AccountId = this._row.AccountId;
                }
                if (this._row.IsPostIdModified)
                {
                    this.UnderlyingDataRow.PostId = this._row.PostId;
                }
                if (this._row.IsInReplyToAccountIdModified)
                {
                    this.UnderlyingDataRow.InReplyToAccountId = this._row.InReplyToAccountId;
                }
                if (this._row.IsInReplyToPostIdModified)
                {
                    this.UnderlyingDataRow.InReplyToPostId = this._row.InReplyToPostId;
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
        public Int32 CompareTo(ReplyElement other)
        {
            Int32 ret;
            Int64 x;
            Int64 y;
            if ((ret = this.Row.AccountId.CompareTo(other.Row.AccountId)) != 0)
            {
                return ret;
            }
            else if ((ret = this.Row.InReplyToAccountId.CompareTo(other.Row.InReplyToAccountId)) != 0)
            {
                return ret;
            }
            else if (this.Row.PostId != other.Row.PostId)
            {
                if (Int64.TryParse(this.Row.PostId, out x) && Int64.TryParse(other.Row.PostId, out y))
                {
                    return x.CompareTo(y);
                }
                else
                {
                    return this.Row.PostId.CompareTo(other.Row.PostId);
                }
            }
            else
            {
                if (Int64.TryParse(this.Row.InReplyToPostId, out x) && Int64.TryParse(other.Row.InReplyToPostId, out y))
                {
                    return x.CompareTo(y);
                }
                else
                {
                    return this.Row.InReplyToPostId.CompareTo(this.Row.InReplyToPostId);
                }
            }
        }

        /// <summary>
        /// この関係と、指定した別の関係が同一かどうかを判断します。
        /// </summary>
        /// <param name="other">この関係と比較する関係。</param>
        /// <returns>
        /// <paramref name="other"/> パラメータの主キーの値がこの関係と同じ場合は <c>true</c>。それ以外の場合は <c>false</c>。 
        /// </returns>
        public Boolean Equals(ReplyElement other)
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
        /// 返信している主体であるポストを取得します。
        /// </summary>
        /// <returns>
        /// 返信している主体であるポスト。
        /// </returns>
        public Post GetPost()
        {
            this.GuardIfDisconnected();
            this.Storage.LoadPosts(
                this.UnderlyingDataRow.AccountId,
                this.UnderlyingDataRow.PostId,
                null,
                null
            );
            return this.Post;
        }

        /// <summary>
        /// ポストの返信元のポストを取得します。
        /// </summary>
        /// <returns>
        /// ポストの返信元のポスト。
        /// </returns>
        public Post GetInReplyToPost()
        {
            this.GuardIfDisconnected();
            this.Storage.LoadPosts(
                null,
                null,
                this.UnderlyingDataRow.InReplyToAccountId,
                this.UnderlyingDataRow.InReplyToPostId
            );
            return this.InReplyToPost;
        }
    }
}