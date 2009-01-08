// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@xspect.org>
 * All rights reserved.
 * 
 * This file is part of MetaTweetServer.
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
using XSpect.MetaTweet.Properties;
using System.Data;
using XSpect.MetaTweet.ObjectModel;

namespace XSpect.MetaTweet
{
    public abstract class Storage
        : IDisposable
    {
        public abstract void Initialize(String connectionString);

        public abstract void Connect();

        public abstract void Disconnect();

        public abstract void Dispose();

        #region Accounts
        public ICollection<Account> GetAccounts()
        {
            return this.GetAccounts(null);
        }

        public abstract ICollection<Account> GetAccounts(
            Nullable<Guid> accountId
        );
        #endregion

        #region Activities
        public ICollection<Activity> GetActivities()
        {
            // Call GetActivities(Nullable<Guid>, Nullable<DateTime>, String).
            return this.GetActivities(default(Nullable<Guid>), null, null);
        }

        public ICollection<Activity> GetActivities(
            Account account,
            Nullable<DateTime> timestamp,
            String category
        )
        {
            return this.GetActivities(account != null ? account.AccountId : default(Nullable<Guid>), timestamp, category);
        }

        public abstract ICollection<Activity> GetActivities(
            Nullable<Guid> accountId,
            Nullable<DateTime> timestamp,
            String category
        );
        #endregion

        #region FollowMap
        public ICollection<FollowElement> GetFollowElements()
        {
            // Call GetFollowMap(Nullable<Guid>, Nullable<Guid>).
            return this.GetFollowElements(default(Nullable<Guid>), default(Nullable<Guid>));
        }

        public ICollection<FollowElement> GetFollowElements(
            Account account
        )
        {
            if (account != null)
            {
                return this.GetFollowElements(account.AccountId);
            }
            else
            {
                return this.GetFollowElements();
            }
        }

        public abstract ICollection<FollowElement> GetFollowElements(
            Nullable<Guid> accountId
        );

        public ICollection<FollowElement> GetFollowElements(
            Account account,
            Account followingAccount
        )
        {
            Nullable<Guid> accountId = account != null ? account.AccountId : default(Nullable<Guid>);
            Nullable<Guid> followingAccountId = followingAccount != null ? followingAccount.AccountId : default(Nullable<Guid>);
            return this.GetFollowElements(accountId, followingAccountId);
        }

        public abstract ICollection<FollowElement> GetFollowElements(
            Nullable<Guid> accountId,
            Nullable<Guid> followingAccountId
        );
        #endregion

        #region Posts
        public ICollection<Post> GetPosts()
        {
            // Call GetPosts(Nullable<Guid>, String, Nullable<DateTime>).
            return this.GetPosts(default(Nullable<Guid>), null, null);
        }

        public ICollection<Post> GetPosts(
            Account account,
            String postId,
            Nullable<DateTime> timestamp
        )
        {
            return this.GetPosts(account != null ? account.AccountId : default(Nullable<Guid>), postId, timestamp);
        }

        public abstract ICollection<Post> GetPosts(
            Nullable<Guid> accountId,
            String postId,
            Nullable<DateTime> timestamp
        );
        #endregion

        #region ReplyMap
        public ICollection<ReplyElement> GetReplyElements()
        {
            return this.GetReplyElements(null, null, null, null);
        }

        public ICollection<ReplyElement> GetReplyElements(
            Post post
        )
        {
            return this.GetReplyElements(post.Activity.Account.AccountId, post.PostId);
        }

        public abstract ICollection<ReplyElement> GetReplyElements(
            Nullable<Guid> accountId,
            String postId
        );

        public ICollection<ReplyElement> GetReplyElements(
            Post post,
            Post inReplyToPost
        )
        {
            return this.GetReplyElements(
                post.Activity.Account.AccountId,
                post.PostId,
                inReplyToPost.Activity.Account.AccountId,
                inReplyToPost.PostId
            );
        }

        public abstract ICollection<ReplyElement> GetReplyElements(
            Nullable<Guid> accountId,
            String postId,
            Nullable<Guid> inReplyToAccountId,
            String inReplyTopostId
        );
        #endregion

        #region TagMap
        public ICollection<TagElement> GetTagElements()
        {
            return this.GetTagElements(null, null);
        }

        public ICollection<TagElement> GetTagElements(
            Activity activity,
            String tag
        )
        {
            return this.GetTagElements(activity.Account.AccountId, activity.Timestamp, activity.Category, tag);
        }

        public abstract ICollection<TagElement> GetTagElements(
            Nullable<Guid> accountId,
            Nullable<DateTime> timestamp,
            String category,
            String tag
        );
        #endregion

        #region Update
        public abstract void Update(params StorageDataSet.AccountsRow[] rows);

        public abstract void Update(params StorageDataSet.ActivitiesRow[] rows);

        public abstract void Update(params StorageDataSet.FollowMapRow[] rows);

        public abstract void Update(params StorageDataSet.PostsRow[] rows);

        public abstract void Update(params StorageDataSet.ReplyMapRow[] rows);

        public abstract void Update(params StorageDataSet.TagMapRow[] rows);
        #endregion
    }
}