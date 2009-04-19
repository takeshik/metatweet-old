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
    /// ポストを表します。
    /// </summary>
    /// <remarks>
    /// <para>ポストはカテゴリが Post であるアクティビティと一対一で対応する要素で、アカウントが行った投稿を表現します。個々の投稿には文字列の ID を提示する必要があり、その内容は <see cref="XSpect.MetaTweet.ObjectModel.Activity.Value"/> に対応します。</para>
    /// <para>個々のポストは「未取得」という状態をとることができます。ポストが未取得であるとは、ポストの存在のみを確定させ、その内容については保持していない状態を意味します。具体的には <see cref="Text"/> と <see cref="Source"/> の両プロパティが <c>null</c> である状態を指します。未取得のポストはそのポストの存在のみを明らかにする場合に効果的です。例えばあるポストの返信元を探索する際に当該発言を発見できなかった場合、 一時的な処置として未取得のポストを生成し、具体的内容については後に取得する、などの用途が期待されています。これにより、リモート サーバへの負荷の過剰な増大を抑止することができます。</para>
    /// <para>ポストは <see cref="Activity"/> および <see cref="PostId"/> によって一意に識別されます。</para>
    /// </remarks>
    [Serializable()]
    public class Post
        : StorageObject<StorageDataSet.PostsDataTable, StorageDataSet.PostsRow>,
          IComparable<Post>
    {
        /// <summary>
        /// このポストと一対一で対応するアクティビティを取得または設定します。
        /// </summary>
        /// <value>
        /// このポストと一対一で対応するアクティビティ。
        /// </value>
        public Activity Activity
        {
            get
            {
                this.Storage.LoadActivitiesDataTable(
                    this.UnderlyingDataRow.AccountId,
                    null,
                    "Post",
                    null,
                    this.PostId,
                    null
                );
                return this.ActivityInDataSet;
            }
            set
            {
                this.UnderlyingDataRow.ActivitiesRowParent = value.UnderlyingDataRow;
            }
        }

        /// <summary>
        /// データセット内に存在する、このポストと一対一で対応するアクティビティを取得または設定します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このポストと一対一で対応するアクティビティ。
        /// </value>
        public Activity ActivityInDataSet
        {
            get
            {
                return this.Storage.GetActivity(this.UnderlyingDataRow.ActivitiesRowParent);
            }
        }

        /// <summary>
        /// サービス内においてこのポストを一意に識別する文字列を取得または設定します。
        /// </summary>
        /// <value>
        /// サービス内においてこのポストを一意に識別する文字列。
        /// </value>
        public String PostId
        {
            get
            {
                return this.UnderlyingDataRow.PostId;
            }
            set
            {
                this.UnderlyingDataRow.PostId = value;
            }
        }

        /// <summary>
        /// このポストの本文を取得または設定します。
        /// </summary>
        /// <value>
        /// このポストの本文。ポストが未取得の場合は <c>null</c>。
        /// </value>
        public String Text
        {
            get
            {
                return this.UnderlyingDataRow.Text;
            }
            set
            {
                this.UnderlyingDataRow.Text = value;
            }
        }

        /// <summary>
        /// このポストの投稿に使用されたクライアントを表す文字列を取得または設定します。
        /// </summary>
        /// <value>
        /// このポストの投稿に使用されたクライアントを表す文字列。ポストが未取得の場合は <c>null</c>。
        /// </value>
        public String Source
        {
            get
            {
                return this.UnderlyingDataRow.Source;
            }
            set
            {
                this.UnderlyingDataRow.Source = value;
            }
        }

        /// <summary>
        /// このポストの返信元のポストとの関係のシーケンスを取得します。
        /// </summary>
        /// <value>
        /// このポストの返信元のポストとの関係のシーケンス。
        /// </value>
        public IEnumerable<ReplyElement> ReplyingMap
        {
            get
            {
                this.Storage.LoadReplyMapDataTable(this.UnderlyingDataRow.AccountId, this.PostId, null, null);
                return this.ReplyingMapInDataSet;
            }
        }

        /// <summary>
        /// データセット内に存在する、このポストの返信元のポストとの関係のシーケンスを取得します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このポストの返信元のポストとの関係のシーケンス。
        /// </value>
        public IEnumerable<ReplyElement> ReplyingMapInDataSet
        {
            get
            {
                return this.Storage.GetReplyElements(this.UnderlyingDataRow.GetReplyMapRowsByFK_Posts_ReplyMap());
            }
        }

        /// <summary>
        /// このポストの返信元のポストのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// このポストの返信元のポストのシーケンス。
        /// </value>
        public IEnumerable<Post> Replying
        {
            get
            {
                return this.ReplyingMap.Select(e => e.InReplyToPost);
            }
        }

        /// <summary>
        /// データセット内に存在する、このポストの返信元のポストのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このポストの返信元のポストのシーケンス。
        /// </value>
        public IEnumerable<Post> ReplyingInDataSet
        {
            get
            {
                return this.ReplyingMapInDataSet.Select(e => e.InReplyToPost);
            }
        }

        /// <summary>
        /// このポストに対する返信のポストとの関係のシーケンスを取得します。
        /// </summary>
        /// <value>
        /// このポストに対する返信のポストとの関係のシーケンス。
        /// </value>
        public IEnumerable<ReplyElement> RepliesMap
        {
            get
            {
                this.Storage.LoadReplyMapDataTable(null, null, this.UnderlyingDataRow.AccountId, this.PostId);
                return this.RepliesMapInDataSet;
            }
        }

        /// <summary>
        /// データセット内に存在する、このポストに対する返信のポストとの関係のシーケンスを取得します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このポストに対する返信のポストとの関係のシーケンス。
        /// </value>
        public IEnumerable<ReplyElement> RepliesMapInDataSet
        {
            get
            {
                return this.Storage.GetReplyElements(this.UnderlyingDataRow.GetReplyMapRowsByFK_Posts_ReplyMap());
            }
        }

        /// <summary>
        /// このポストに対する返信のポストのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// このポストに対する返信のポストのシーケンス。
        /// </value>
        public IEnumerable<Post> Replies
        {
            get
            {
                return this.RepliesMap.Select(e => e.Post);
            }
        }

        /// <summary>
        /// データセット内に存在する、このポストに対する返信のポストのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このポストに対する返信のポストのシーケンス。
        /// </value>
        public IEnumerable<Post> RepliesInDataSet
        {
            get
            {
                return this.RepliesMapInDataSet.Select(e => e.Post);
            }
        }

        /// <summary>
        /// <see cref="Post"/> の新しいインスタンスを初期化します。
        /// </summary>
        public Post()
        {
        }

        /// <summary>
        /// <see cref="Post"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="row">ポストが参照するデータ列。</param>
        public Post(StorageDataSet.PostsRow row)
        {
            this.UnderlyingDataRow = row;
        }

        /// <summary>
        /// このポストを別のポストと比較します。
        /// </summary>
        /// <param name="other">このアカウントと比較するポスト。</param>
        /// <returns>
        /// 比較対象ポストの相対順序を示す 32 ビット符号付き整数。戻り値の意味は次のとおりです。<br/>
        /// 値<br/>
        /// 意味<br/>
        /// 0 より小さい値<br/>
        /// このポストが <paramref name="other"/> パラメータより前に序列されるべきであることを意味します。<br/>
        /// 0<br/>
        /// このポストが <paramref name="other"/> と等しいことを意味します。<br/>
        /// 0 より大きい値<br/>
        /// このポストが <paramref name="other"/> パラメータより後に序列されるべきであることを意味します。<br/>
        /// </returns>
        public Int32 CompareTo(Post other)
        {
            Int32 result;
            if ((result = this.Activity.CompareTo(other.Activity)) != 0)
            {
                return result;
            }
            else
            {
                return this.PostId.CompareTo((other as Post).PostId);
            }
        }

        /// <summary>
        /// このポストを表す <see cref="T:System.String"/> を返します。
        /// </summary>
        /// <returns>
        /// このポストを表す <see cref="T:System.String"/>。
        /// </returns>
        public override String ToString()
        {
            return string.Format(
                "#({0}): \"{1}\"",
                this.PostId,
                this.Text
            );
        }

        /// <summary>
        /// このオブジェクトの参照するデータ行を削除するようマークします。
        /// </summary>
        public override void Delete()
        {
            base.Delete();
            this.Activity.Delete();
        }

        /// <summary>
        /// このポストを別のストレージへコピーします。
        /// </summary>
        /// <param name="destination">コピー先の <see cref="Storage"/>。</param>
        /// <returns>コピーされたポスト。</returns>
        public Post Copy(Storage destination)
        {
            Post post = destination.NewPost(this.Activity.Copy(destination));
            post.Text = this.Text;
            post.Source = this.Source;
            return post;
        }

        /// <summary>
        /// 指定されたポストをこのポストの返信元の関係として追加します。
        /// </summary>
        /// <param name="post">このポストの返信元の関係として追加するポスト。</param>
        public void AddReplying(Post post)
        {
            this.Storage.NewReplyElement(this, post);
        }

        /// <summary>
        /// 指定されたポストへの返信元の関係を削除します。
        /// </summary>
        /// <param name="post">返信元の関係を削除するポスト。</param>
        public void RemoveReplying(Post post)
        {
            this.ReplyingMap.Single(e => e.InReplyToPost == post).Delete();
        }

        /// <summary>
        /// 指定されたポストをこのポストへの返信の関係として追加します。
        /// </summary>
        /// <param name="post">このポストへの返信の関係として追加するポスト。</param>
        public void AddReply(Post post)
        {
            this.Storage.NewReplyElement(post, this);
        }

        /// <summary>
        /// 指定されたポストからの返信の関係を削除します。
        /// </summary>
        /// <param name="post">返信の関係を削除するポスト。</param>
        public void RemoveReply(Post post)
        {
            this.ReplyingMap.Single(e => e.Post == post).Delete();
        }
    }
}