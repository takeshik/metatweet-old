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
        : StorageObject<StorageDataSet.ReplyMapDataTable, StorageDataSet.ReplyMapRow>
    {
        /// <summary>
        /// この関係のデータのバックエンドとなるデータ行の主キーのシーケンスを取得します。
        /// </summary>
        /// <value>この関係のデータのバックエンドとなるデータ行の主キーのシーケンス。</value>
        public override IEnumerable<Object> PrimaryKeys
        {
            get
            {
                return this.GetPrimaryKeyCollection();
            }
        }
        
        /// <summary>
        /// 返信している主体であるポストを取得または設定します。
        /// </summary>
        /// <value>
        /// 返信している主体であるポスト。
        /// </value>
        public Post Post
        {
            get
            {
                this.Storage.LoadPostsDataTable(
                    this.UnderlyingDataRow.AccountId,
                    this.UnderlyingDataRow.PostId,
                    null,
                    null
                );
                return this.PostInDataSet;
            }
            set
            {
                this.UnderlyingDataRow.PostsRowParentByFK_Posts_ReplyMap = value.UnderlyingDataRow;
            }
        }

        /// <summary>
        /// データセット内に存在する、返信している主体であるポストを取得または設定します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、返信している主体であるポスト。
        /// </value>
        public Post PostInDataSet
        {
            get
            {
                return this.Storage.GetPost(this.UnderlyingDataRow.PostsRowParentByFK_Posts_ReplyMap);
            }
        }

        /// <summary>
        /// ポストの返信元のポストを取得または設定します。
        /// </summary>
        /// <value>
        /// ポストの返信元のポスト。
        /// </value>
        public Post InReplyToPost
        {
            get
            {
                this.Storage.LoadPostsDataTable(
                    null,
                    null,
                    this.UnderlyingDataRow.InReplyToAccountId,
                    this.UnderlyingDataRow.InReplyToPostId
                );
                return this.InReplyToPostInDataSet;
            }
            set
            {
                this.UnderlyingDataRow.PostsRowParentByFK_PostsInReplyTo_ReplyMap = value.UnderlyingDataRow;
            }
        }

        /// <summary>
        /// データセット内に存在する、ポストの返信元のポストを取得または設定します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、ポストの返信元のポスト。
        /// </value>
        public Post InReplyToPostInDataSet
        {
            get
            {
                return this.Storage.GetPost(this.UnderlyingDataRow.PostsRowParentByFK_PostsInReplyTo_ReplyMap);
            }
        }

        /// <summary>
        /// <see cref="ReplyElement"/> の新しいインスタンスを初期化します。
        /// </summary>
        public ReplyElement()
        {
        }

        /// <summary>
        /// <see cref="ReplyElement"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="row">関係が参照するデータ列。</param>
        public ReplyElement(StorageDataSet.ReplyMapRow row)
        {
            this.UnderlyingDataRow = row;
        }

        /// <summary>
        /// この関係を表す <see cref="T:System.String"/> を返します。
        /// </summary>
        /// <returns>
        /// この関係を表す <see cref="T:System.String"/>。
        /// </returns>
        public override String ToString()
        {
            return String.Format("{0} => {1}", this.Post.ToString(), this.InReplyToPost.ToString());
        }

        /// <summary>
        /// この関係のデータのバックエンドとなるデータ行の主キーのシーケンスを表すオブジェクトを取得します。
        /// </summary>
        /// <returns>この関係のデータのバックエンドとなるデータ行の主キーのシーケンスを表すオブジェクト。</returns>
        public PrimaryKeyCollection GetPrimaryKeyCollection()
        {
            return new PrimaryKeyCollection(this);
        }

        /// <summary>
        /// この関係を別のストレージへコピーします。
        /// </summary>
        /// <param name="destination">コピー先の <see cref="Storage"/>。</param>
        /// <returns>コピーされた関係。</returns>
        public ReplyElement Copy(Storage destination)
        {
            return destination.NewReplyElement(
                this.Post.Copy(destination),
                this.InReplyToPost.Copy(destination)
            );
        }
    }
}