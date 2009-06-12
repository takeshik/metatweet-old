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

namespace XSpect.MetaTweet
{
    partial class StorageDataSet
    {
        partial class AccountsRow
            : IAccountsRow
        {
            /// <summary>
            /// 行の全ての値のリストを取得します。
            /// </summary>
            /// <value>行の全ての値のリスト。</value>
            public IList<Object> Items
            {
                get
                {
                    return this.ItemArray;
                }
            }

            /// <summary>
            /// 行における主キーとなる値のリストを取得します。
            /// </summary>
            /// <value>行における主キーの値のリスト。</value>
            public IList<Object> PrimaryKeys
            {
                get
                {
                    return new Object[]
                    {
                        this.AccountId,
                    };
                }
            }

            /// <summary>
            /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </returns>
            public override String ToString()
            {
                return String.Format(
                    "{{Acc* {0}: {1}}}",
                    this.AccountId.ToString("d"),
                    this.Realm
                );
            }
        }

        partial class ActivitiesRow
            : IActivitiesRow
        {
            /// <summary>
            /// 行の全ての値のリストを取得します。
            /// </summary>
            /// <value>行の全ての値のリスト。</value>
            public IList<Object> Items
            {
                get
                {
                    return this.ItemArray;
                }
            }

            /// <summary>
            /// 行における主キーとなる値のリストを取得します。
            /// </summary>
            /// <value>行における主キーの値のリスト。</value>
            public IList<Object> PrimaryKeys
            {
                get
                {
                    return new Object[]
                    {
                        this.AccountId,
                        this.Timestamp,
                        this.Category,
                        this.Subindex,
                    };
                }
            }

            /// <summary>
            /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </returns>
            public override String ToString()
            {
                return String.Format(
                    "{{Act* {0}, {1}, {2}, {3}: {4}, {5}}}",
                    this.AccountId.ToString("d"),
                    this.Timestamp.ToString("s"),
                    this.Category,
                    this.Subindex,
                    this.IsValueNull() ? "(null)" : this.Value,
                    this.IsDataNull() ? "(null)" : "(" + this.Data.Length + ")"
                );
            }
        }

        partial class FavorMapRow
            : IFavorMapRow
        {
            /// <summary>
            /// 行の全ての値のリストを取得します。
            /// </summary>
            /// <value>行の全ての値のリスト。</value>
            public IList<Object> Items
            {
                get
                {
                    return this.ItemArray;
                }
            }

            /// <summary>
            /// 行における主キーとなる値のリストを取得します。
            /// </summary>
            /// <value>行における主キーの値のリスト。</value>
            public IList<Object> PrimaryKeys
            {
                get
                {
                    return this.Items;
                }
            }

            /// <summary>
            /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </returns>
            public override String ToString()
            {
                return String.Format(
                    "{{Fav* {0}, {1}, {2}, {3}, {4}}}",
                    this.AccountId.ToString("d"),
                    this.FavoringAccountId.ToString("d"),
                    this.FavoringTimestamp.ToString("s"),
                    this.FavoringCategory,
                    this.FavoringSubindex
                );
            }
        }

        partial class FollowMapRow
            : IFollowMapRow
        {
            /// <summary>
            /// 行の全ての値のリストを取得します。
            /// </summary>
            /// <value>行の全ての値のリスト。</value>
            public IList<Object> Items
            {
                get
                {
                    return this.ItemArray;
                }
            }

            /// <summary>
            /// 行における主キーとなる値のリストを取得します。
            /// </summary>
            /// <value>行における主キーの値のリスト。</value>
            public IList<Object> PrimaryKeys
            {
                get
                {
                    return this.Items;
                }
            }

            /// <summary>
            /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </returns>
            public override String ToString()
            {
                return String.Format(
                    "{{Flw* {0}, {1}}}",
                    this.AccountId.ToString("d"),
                    this.FollowingAccountId.ToString("d")
                );
            }
        }

        partial class PostsRow
            : IPostsRow
        {
            /// <summary>
            /// 行の全ての値のリストを取得します。
            /// </summary>
            /// <value>行の全ての値のリスト。</value>
            public IList<Object> Items
            {
                get
                {
                    return this.ItemArray;
                }
            }

            /// <summary>
            /// 行における主キーとなる値のリストを取得します。
            /// </summary>
            /// <value>行における主キーの値のリスト。</value>
            public IList<Object> PrimaryKeys
            {
                get
                {
                    return new Object[]
                    {
                        this.AccountId,
                        this.PostId,
                    };
                }
            }

            /// <summary>
            /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </returns>
            public override String ToString()
            {
                return String.Format(
                    "{{Pst* {0}, {1}: {2}, {3}}}",
                    this.AccountId.ToString("d"),
                    this.PostId,
                    this.IsTextNull() ? "(null)" : this.Text,
                    this.IsSourceNull() ? "(null)" : this.Source
                );
            }
        }

        partial class ReplyMapRow
            : IReplyMapRow
        {
            /// <summary>
            /// 行の全ての値のリストを取得します。
            /// </summary>
            /// <value>行の全ての値のリスト。</value>
            public IList<Object> Items
            {
                get
                {
                    return this.ItemArray;
                }
            }

            /// <summary>
            /// 行における主キーとなる値のリストを取得します。
            /// </summary>
            /// <value>行における主キーの値のリスト。</value>
            public IList<Object> PrimaryKeys
            {
                get
                {
                    return this.Items;
                }
            }

            /// <summary>
            /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </returns>
            public override String ToString()
            {
                return String.Format(
                    "{{Rep* {0}, {1}, {2}, {3}}}",
                    this.AccountId.ToString("d"),
                    this.PostId,
                    this.InReplyToAccountId.ToString("d"),
                    this.InReplyToPostId
                );
            }
        }

        partial class TagMapRow
            : ITagMapRow
        {
            /// <summary>
            /// 行の全ての値のリストを取得します。
            /// </summary>
            /// <value>行の全ての値のリスト。</value>
            public IList<Object> Items
            {
                get
                {
                    return this.ItemArray;
                }
            }

            /// <summary>
            /// 行における主キーとなる値のリストを取得します。
            /// </summary>
            /// <value>行における主キーの値のリスト。</value>
            public IList<Object> PrimaryKeys
            {
                get
                {
                    return this.Items;
                }
            }

            /// <summary>
            /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </returns>
            public override String ToString()
            {
                return String.Format(
                    "{{Tag* {0}, {1}, {2}, {3}, {4}}}",
                    this.AccountId.ToString("d"),
                    this.Timestamp.ToString("s"),
                    this.Category,
                    this.Subindex,
                    this.Tag
                );
            }
        }
    }
}