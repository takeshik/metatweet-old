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
    public partial class Post
        : StorageObject<StorageDataSet.PostsDataTable, IPostsRow, StorageDataSet.PostsRow>,
          IComparable<Post>,
          IEquatable<Post>
    {
        private InternalRow _row;

        /// <summary>
        /// データセット内に存在する、このポストの親オブジェクトのシーケンスを取得します。
        /// </summary>
        /// <value>データセット内に存在する、このポストの親オブジェクトのシーケンス。</value>
        public override IEnumerable<StorageObject> Parents
        {
            get
            {
                return new StorageObject[]
                {
                    this.Activity,
                };
            }
        }

        /// <summary>
        /// データセット内に存在する、このポストの子オブジェクトのシーケンスを取得します。
        /// </summary>
        /// <value>データセット内に存在する、このポストの子オブジェクトのシーケンス。</value>
        public override IEnumerable<StorageObject> Children
        {
            get
            {
                return this.ReplyingMap.Cast<StorageObject>()
                    .Concat(this.RepliesMap.Cast<StorageObject>());
            }
        }

        /// <summary>
        /// このオブジェクトが現在参照している列を取得します。
        /// </summary>
        /// <value>このオブジェクトが現在参照している列。</value>
        public override IPostsRow Row
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
        /// データセット内に存在する、このポストと一対一で対応するアクティビティを取得または設定します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このポストと一対一で対応するアクティビティ。
        /// </value>
        public Activity Activity
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
        /// サービス内においてこのポストを一意に識別する文字列を取得または設定します。
        /// </summary>
        /// <value>
        /// サービス内においてこのポストを一意に識別する文字列。
        /// </value>
        public String PostId
        {
            get
            {
                return this.Row.PostId;
            }
            set
            {
                this.Row.PostId = value;
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
                return this.IsConnected
                    ? (this.UnderlyingDataRow.IsTextNull()
                        ? null
                        : this.UnderlyingDataRow.Text)
                    : this.Row.Text;
            }
            set
            {
                if (value == null && this.IsConnected)
                {
                    this.UnderlyingDataRow.SetTextNull();
                }
                else
                {
                    this.Row.Text = value;
                }
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
                return this.IsConnected
                    ? (this.UnderlyingDataRow.IsSourceNull()
                        ? null
                        : this.UnderlyingDataRow.Source)
                    : this.Row.Source;
            }
            set
            {
                if (value == null && this.IsConnected)
                {
                    this.UnderlyingDataRow.SetSourceNull();
                }
                else
                {
                    this.Row.Source = value;
                }
            }
        }

        /// <summary>
        /// データセット内に存在する、このポストの返信元のポストとの関係のシーケンスを取得します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このポストの返信元のポストとの関係のシーケンス。
        /// </value>
        public IEnumerable<ReplyElement> ReplyingMap
        {
            get
            {
                return this.Storage.GetReplyElements(this.UnderlyingDataRow.GetReplyMapRowsByFK_Posts_ReplyMap());
            }
        }

        /// <summary>
        /// データセット内に存在する、このポストの返信元のポストのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このポストの返信元のポストのシーケンス。
        /// </value>
        public IEnumerable<Post> Replying
        {
            get
            {
                return this.ReplyingMap.Select(e => e.InReplyToPost);
            }
        }

        /// <summary>
        /// データセット内に存在する、このポストに対する返信のポストとの関係のシーケンスを取得します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このポストに対する返信のポストとの関係のシーケンス。
        /// </value>
        public IEnumerable<ReplyElement> RepliesMap
        {
            get
            {
                return this.Storage.GetReplyElements(this.UnderlyingDataRow.GetReplyMapRowsByFK_Posts_ReplyMap());
            }
        }

        /// <summary>
        /// データセット内に存在する、このポストに対する返信のポストのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このポストに対する返信のポストのシーケンス。
        /// </value>
        public IEnumerable<Post> Replies
        {
            get
            {
                return this.RepliesMap.Select(e => e.Post);
            }
        }

        /// <summary>
        /// <see cref="Post"/> の新しいインスタンスを初期化します。
        /// </summary>
        public Post()
        {
            this._row = new InternalRow();
        }

        /// <summary>
        /// 2 つのポストが等しいかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較するポスト。</param>
        /// <param name="right">比較されるポスト。</param>
        /// <returns><paramref name="left"/> と <paramref name="right"/> が等しい場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator ==(Post left, Post right)
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
        /// 2 つのポストが等しくないかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較するポスト。</param>
        /// <param name="right">比較されるポスト。</param>
        /// <returns><paramref name="left"/> と <paramref name="right"/> が等しくない場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator !=(Post left, Post right)
        {
            return !(left == right);
        }

        /// <summary>
        /// 一方のポストが、他方のポストより前に位置するかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較するポスト。</param>
        /// <param name="right">比較されるポスト。</param>
        /// <returns><paramref name="left"/> が <paramref name="right"/> より前に位置する場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator <(Post left, Post right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// 一方のポストが、他方のポストより後ろに位置するかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較するポスト。</param>
        /// <param name="right">比較されるポスト。</param>
        /// <returns><paramref name="left"/> が <paramref name="right"/> より後ろに位置する場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator >(Post left, Post right)
        {
            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// 一方のポストが、他方のポストと等しいか、または前に位置するかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較するポスト。</param>
        /// <param name="right">比較されるポスト。</param>
        /// <returns><paramref name="left"/> が <paramref name="right"/> と等しい、または前に位置する場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator <=(Post left, Post right)
        {
            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// 一方のポストが、他方のポストと等しいか、または後ろに位置するかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較するポスト。</param>
        /// <param name="right">比較されるポスト。</param>
        /// <returns><paramref name="left"/> が <paramref name="right"/> と等しい、または後ろに位置する場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator >=(Post left, Post right)
        {
            return left.CompareTo(right) >= 0;
        }

        /// <summary>
        /// このポストと、指定した別のポストが同一かどうかを判断します。
        /// </summary>
        /// <param name="obj">このポストと比較するポスト。</param>
        /// <returns>
        /// <paramref name="obj"/> パラメータの値がこのポストと同じ場合は <c>true</c>。それ以外の場合は <c>false</c>。 
        /// </returns>
        public override Boolean Equals(Object obj)
        {
            return obj is Post && this.Equals(obj as Post);
        }

        /// <summary>
        /// このポストのハッシュ コードを返します。 
        /// </summary>
        /// <returns>32 ビット符号付き整数ハッシュ コード。 </returns>
        public override Int32 GetHashCode()
        {
            return unchecked((
                this.Row.AccountId.GetHashCode() * 397) ^
                this.Row.PostId.GetHashCode()
            );
        }

        /// <summary>
        /// このポストを表す <see cref="T:System.String"/> を返します。
        /// </summary>
        /// <returns>
        /// このポストを表す <see cref="T:System.String"/>。
        /// </returns>
        public override String ToString()
        {
            return this.IsConnected
                ? String.Format(
                      "Pst* [{0}] #{1}: \"{2}\" : {3}",
                      this.Activity.Account,
                      this.PostId,
                      this.Text ?? "(null)",
                      this.Source ?? "(null)"
                  )
                : String.Format(
                      "Pst {0} #{1}: \"{2}\" : {3}",
                      this.Row.AccountId.ToString("b"),
                      this.PostId,
                      this.Text ?? "(null)",
                      this.Source ?? "(null)"
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
            if (!(other is Post))
            {
                throw new ArgumentException("other");
            }
            return this.CompareTo(other as Post);
        }

        /// <summary>
        /// このポストの親オブジェクトのシーケンスを取得します。
        /// </summary>
        /// <returns>このポストの親オブジェクトのシーケンス。</returns>
        public override IEnumerable<StorageObject> GetParents()
        {
            return new StorageObject[]
            {
                this.GetActivity(),
            };
        }

        /// <summary>
        /// このポストの子オブジェクトのシーケンスを取得します。
        /// </summary>
        /// <returns>このポストの子オブジェクトのシーケンス。</returns>
        public override IEnumerable<StorageObject> GetChildren()
        {
            return this.GetReplyingMap().Cast<StorageObject>()
                .Concat(this.GetRepliesMap().Cast<StorageObject>());
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
                this._row.Text = this.UnderlyingDataRow.IsTextNull() ? null : this.UnderlyingDataRow.Text;
                this._row.Source = this.UnderlyingDataRow.IsSourceNull() ? null : this.UnderlyingDataRow.Source;
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
                if (this._row.IsTextModified)
                {
                    this.UnderlyingDataRow.Text = this._row.Text;
                }
                if (this._row.IsSourceModified)
                {
                    this.UnderlyingDataRow.Source = this._row.Source;
                }
                this._row = null;
            }
        }

        /// <summary>
        /// このポストを別のポストと比較します。
        /// </summary>
        /// <param name="other">このポストと比較するポスト。</param>
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
            Int32 ret;
            if ((ret = this.Row.AccountId.CompareTo(other.Row.AccountId)) != 0)
            {
                return ret;
            }
            else
            {
                // For numerical-order sorting (only if both PostIds are parseable).
                Int64 x;
                Int64 y;
                if (Int64.TryParse(this.Row.PostId, out x) && Int64.TryParse(other.Row.PostId, out y))
                {
                    return x.CompareTo(y);
                }
                else
                {
                    return this.Row.PostId.CompareTo(other.Row.PostId);
                }
            }
        }

        /// <summary>
        /// このポストと、指定した別のポストが同一かどうかを判断します。
        /// </summary>
        /// <param name="other">このポストと比較するポスト。</param>
        /// <returns>
        /// <paramref name="other"/> パラメータの主キーの値がこのポストと同じ場合は <c>true</c>。それ以外の場合は <c>false</c>。 
        /// </returns>
        public Boolean Equals(Post other)
        {
            return this.CompareTo(other) == 0;
        }

        /// <summary>
        /// このポストと、指定した別のポストが同一のデータソースを参照し、かつ、同一の値を持つかどうかを判断します。
        /// </summary>
        /// <param name="other">このポストと比較するポスト。</param>
        /// <returns><paramref name="other"/> パラメータの主キーの値がこのポストと同じで、なおかつ <see cref="Storage"/> も同じ場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public Boolean ExactlyEquals(Account other)
        {
            return this.Storage == other.Storage && this.Equals(other);
        }

        /// <summary>
        /// このポストと一対一で対応するアクティビティを取得します。
        /// </summary>
        /// <returns>
        /// このポストと一対一で対応するアクティビティ。
        /// </returns>
        public Activity GetActivity()
        {
            this.GuardIfDisconnected();
            this.Storage.LoadActivitiesDataTable(
                this.UnderlyingDataRow.AccountId,
                null,
                "Post",
                null,
                this.PostId,
                null
            );
            return this.Activity;
        }

        /// <summary>
        /// このポストの返信元のポストとの関係のシーケンスを取得します。
        /// </summary>
        /// <returns>
        /// このポストの返信元のポストとの関係のシーケンス。
        /// </returns>
        public IEnumerable<ReplyElement> GetReplyingMap()
        {
            this.GuardIfDisconnected();
            this.Storage.LoadReplyMapDataTable(this.UnderlyingDataRow.AccountId, this.PostId, null, null);
            return this.ReplyingMap;
        }

        /// <summary>
        /// このポストの返信元のポストのシーケンスを取得します。
        /// </summary>
        /// <returns>
        /// このポストの返信元のポストのシーケンス。
        /// </returns>
        public IEnumerable<Post> GetReplying()
        {
            return this.GetReplyingMap().Select(e => e.InReplyToPost);
        }

        /// <summary>
        /// 指定されたポストをこのポストの返信元の関係として追加します。
        /// </summary>
        /// <param name="post">このポストの返信元の関係として追加するポスト。</param>
        public void AddReplying(Post post)
        {
            this.GuardIfDisconnected();
            this.Storage.NewReplyElement(this, post);
        }

        /// <summary>
        /// 指定されたポストへの返信元の関係を削除します。
        /// </summary>
        /// <param name="post">返信元の関係を削除するポスト。</param>
        public void RemoveReplying(Post post)
        {
            this.GetReplyingMap().Single(e => e.InReplyToPost == post).Delete();
        }

        /// <summary>
        /// このポストに対する返信のポストとの関係のシーケンスを取得します。
        /// </summary>
        /// <returns>
        /// このポストに対する返信のポストとの関係のシーケンス。
        /// </returns>
        public IEnumerable<ReplyElement> GetRepliesMap()
        {
            this.GuardIfDisconnected();
            this.Storage.LoadReplyMapDataTable(null, null, this.UnderlyingDataRow.AccountId, this.PostId);
            return this.RepliesMap;
        }

        /// <summary>
        /// このポストに対する返信のポストのシーケンスを取得します。
        /// </summary>
        /// <returns>
        /// このポストに対する返信のポストのシーケンス。
        /// </returns>
        public IEnumerable<Post> GetReplies()
        {
            return this.GetRepliesMap().Select(e => e.Post);
        }

        /// <summary>
        /// 指定されたポストをこのポストへの返信の関係として追加します。
        /// </summary>
        /// <param name="post">このポストへの返信の関係として追加するポスト。</param>
        public void AddReply(Post post)
        {
            this.GuardIfDisconnected();
            this.Storage.NewReplyElement(post, this);
        }

        /// <summary>
        /// 指定されたポストからの返信の関係を削除します。
        /// </summary>
        /// <param name="post">返信の関係を削除するポスト。</param>
        public void RemoveReply(Post post)
        {
            this.GetReplyingMap().Single(e => e.Post == post).Delete();
        }
    }
}