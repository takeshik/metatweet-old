// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetObjectModel
 *   Object model and Storage interface for MetaTweet and other systems
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@xspect.org>
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
using System.Data;
using System.Linq;
using XSpect.MetaTweet.ObjectModel;

namespace XSpect.MetaTweet
{
    public interface IStorage
        : IDisposable
    {
        StorageDataSet UnderlyingDataSet
        {
            get;
            set;
        }

        void Initialize(String connectionString);

        void Connect();

        void Disconnect();

        #region Accounts
        StorageDataSet.AccountsDataTable GetAccountsDataTable();

        IEnumerable<Account> GetAccounts();

        IEnumerable<Account> GetAccounts(Func<StorageDataSet.AccountsRow, Boolean> predicate);

        IEnumerable<Account> GetAccounts(IEnumerable<StorageDataSet.AccountsRow> rows);

        Account GetAccount(StorageDataSet.AccountsRow row);

        Account NewAccount();
        #endregion

        #region Activities
        StorageDataSet.ActivitiesDataTable GetActivitiesDataTable();

        IEnumerable<Activity> GetActivities();

        IEnumerable<Activity> GetActivities(Func<StorageDataSet.ActivitiesRow, Boolean> predicate);

        IEnumerable<Activity> GetActivities(IEnumerable<StorageDataSet.ActivitiesRow> rows);

        Activity GetActivity(StorageDataSet.ActivitiesRow row);

        Activity NewActivity();
        #endregion

        #region FollowMap
        StorageDataSet.FollowMapDataTable GetFollowMapDataTable();

        IEnumerable<FollowElement> GetFollowElements();

        IEnumerable<FollowElement> GetFollowElements(Func<StorageDataSet.FollowMapRow, Boolean> predicate);

        IEnumerable<FollowElement> GetFollowElements(IEnumerable<StorageDataSet.FollowMapRow> rows);

        FollowElement GetFollowElement(StorageDataSet.FollowMapRow row);

        FollowElement NewFollowElement();
        #endregion

        #region Posts
        StorageDataSet.PostsDataTable GetPostsDataTable();

        IEnumerable<Post> GetPosts();

        IEnumerable<Post> GetPosts(Func<StorageDataSet.PostsRow, Boolean> predicate);

        IEnumerable<Post> GetPosts(IEnumerable<StorageDataSet.PostsRow> rows);

        Post GetPost(StorageDataSet.PostsRow row);

        Post NewPost();
        #endregion

        #region ReplyMap
        StorageDataSet.ReplyMapDataTable GetReplyMapTable();

        IEnumerable<ReplyElement> GetReplyElements();

        IEnumerable<ReplyElement> GetReplyElements(Func<StorageDataSet.ReplyMapRow, Boolean> predicate);

        IEnumerable<ReplyElement> GetReplyElements(IEnumerable<StorageDataSet.ReplyMapRow> rows);

        ReplyElement GetReplyElement(StorageDataSet.ReplyMapRow row);

        ReplyElement NewReplyElement();
        #endregion

        #region TagMap
        StorageDataSet.TagMapDataTable GetTagMapDataTable();

        IEnumerable<TagElement> GetTagElements();

        IEnumerable<TagElement> GetTagElements(Func<StorageDataSet.TagMapRow, Boolean> predicate);

        IEnumerable<TagElement> GetTagElements(IEnumerable<StorageDataSet.TagMapRow> rows);

        TagElement GetTagElement(StorageDataSet.TagMapRow row);

        TagElement NewTagElement();
        #endregion

        void Update();

        void Merge(IStorage destination);
    }
}