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
        public IEnumerable<Account> GetAccounts()
        {
            return this.GetAccounts(null);
        }

        public abstract IEnumerable<Account> GetAccounts(
            Nullable<Guid> accountId
        );
        #endregion

        #region FollowMap
        public FollowMap GetFollowMap()
        {
            return this.GetFollowMap(null, null);
        }

        public abstract FollowMap GetFollowMap(
            Account account
        );

        public abstract FollowMap GetFollowMap(
            Account account,
            Account followingAccount
        );
        #endregion
        
        #region Activities
        public IEnumerable<Activity> GetActivities()
        {
            return this.GetActivities(null, null, null);
        }

        public abstract IEnumerable<Activity> GetActivities(
            Account account,
            Nullable<DateTime> timestamp,
            String category
        );
        #endregion
        
        #region TagMap
        public TagMap GetTagMap()
        {
            return this.GetTagMap(null, null);
        }

        public abstract TagMap GetTagMap(
            Activity activity,
            String tag
        );
        #endregion

        #region Posts
        public IEnumerable<Post> GetPosts()
        {
            return this.GetPosts(null, null);
        }

        public abstract IEnumerable<Post> GetPosts(
            Account account,
            String postId
        );
        #endregion

        #region ReplyMap
        public ReplyMap GetReplyMap()
        {
            return this.GetReplyMap(null, null);
        }

        public abstract ReplyMap GetReplyMap(
            Post post
        );

        public abstract ReplyMap GetReplyMap(
            Post post,
            Post inReplyToPost
        );
        #endregion
    }
}