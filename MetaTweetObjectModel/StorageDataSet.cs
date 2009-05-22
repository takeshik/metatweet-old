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

namespace XSpect.MetaTweet
{
    partial class StorageDataSet
    {
        partial class AccountsRow
            : IAccountsRow
        {
            /// <summary>
            /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </returns>
            public override String ToString()
            {
                return String.Format(
                    "{{{0}}}",
                    this.AccountId.ToString("d")
                );
            }
        }

        partial class ActivitiesRow
            : IActivitiesRow
        {
            /// <summary>
            /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </returns>
            public override String ToString()
            {
                return String.Format(
                    "{{{0}, {1}, {2}, {3}}}",
                    this.AccountId.ToString("d"),
                    this.Timestamp.ToString("s"),
                    this.Category,
                    this.Subindex
                );
            }
        }

        partial class FavorMapRow
            : IFavorMapRow
        {
            /// <summary>
            /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </returns>
            public override String ToString()
            {
                return String.Format(
                    "{{{0}, {1}, {2}, {3}, {4}}}",
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
            /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </returns>
            public override String ToString()
            {
                return String.Format(
                    "{{{0}, {1}}}",
                    this.AccountId.ToString("d"),
                    this.FollowingAccountId.ToString("d")
                );
            }
        }

        partial class PostsRow
            : IPostsRow
        {
            /// <summary>
            /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </returns>
            public override String ToString()
            {
                return String.Format(
                    "{{{0}, {1}}}",
                    this.AccountId.ToString("d"),
                    this.PostId
                );
            }
        }

        partial class ReplyMapRow
            : IReplyMapRow
        {
            /// <summary>
            /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </returns>
            public override String ToString()
            {
                return String.Format(
                    "{{{0}, {1}, {2}, {3}}}",
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
            /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </returns>
            public override String ToString()
            {
                return String.Format(
                    "{{{0}, {1}, {2}, {3}, {4}}}",
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