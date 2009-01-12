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
using System.Linq;
using XSpect.MetaTweet.ObjectModel;

namespace XSpect.MetaTweet
{
    public abstract class Storage
        : IDisposable
    {
        private StorageDataSet _underlyingDataSet;

        public StorageDataSet UnderlyingDataSet
        {
            get
            {
                return this._underlyingDataSet ?? (this._underlyingDataSet = new StorageDataSet());
            }
            set
            {
                // Suppress re-setting.
                if (this._underlyingDataSet != null)
                {
                    // TODO: Exception string resource
                    throw new InvalidOperationException();
                }
                this._underlyingDataSet = value;
            }
        }

        public abstract void Initialize(String connectionString);

        public abstract void Connect();

        public abstract void Disconnect();

        public abstract void Dispose();

        #region Accounts
        public abstract StorageDataSet.AccountsDataTable GetAccountsDataTable();

        public IEnumerable<Account> GetAccounts()
        {
            return this.GetAccounts(row => true);
        }

        public IEnumerable<Account> GetAccounts(Func<StorageDataSet.AccountsRow, Boolean> predicate)
        {
            return this.GetAccountsDataTable().Where(predicate).Select(row => this.GetAccount(row));
        }

        public IEnumerable<Account> GetAccounts(IEnumerable<StorageDataSet.AccountsRow> rows)
        {
            return rows.Select(row => this.GetAccount(row));
        }

        public Account GetAccount(StorageDataSet.AccountsRow row)
        {
            Account account = this.NewAccount();
            account.UnderlyingDataRow = row;
            return account;
        }

        public Account NewAccount()
        {
            return new Account()
            {
                Storage = this,
            };
        }
        #endregion

        #region Activities
        public abstract StorageDataSet.ActivitiesDataTable GetActivitiesDataTable();

        public IEnumerable<Activity> GetActivities()
        {
            return this.GetActivities(row => true);
        }

        public IEnumerable<Activity> GetActivities(Func<StorageDataSet.ActivitiesRow, Boolean> predicate)
        {
            return this.GetActivitiesDataTable().Where(predicate).Select(row => this.GetActivity(row));
        }

        public IEnumerable<Activity> GetActivities(IEnumerable<StorageDataSet.ActivitiesRow> rows)
        {
            return rows.Select(row => this.GetActivity(row));
        }

        public Activity GetActivity(StorageDataSet.ActivitiesRow row)
        {
            Activity activity = this.NewActivity();
            activity.UnderlyingDataRow = row;
            return activity;
        }

        public Activity NewActivity()
        {
            return new Activity()
            {
                Storage = this,
            };
        }
        #endregion

        #region FollowMap
        public abstract StorageDataSet.FollowMapDataTable GetFollowMapDataTable();

        public IEnumerable<FollowElement> GetFollowElements()
        {
            return this.GetFollowElements(row => true);
        }

        public IEnumerable<FollowElement> GetFollowElements(Func<StorageDataSet.FollowMapRow, Boolean> predicate)
        {
            return this.GetFollowMapDataTable().Where(predicate).Select(row => this.GetFollowElement(row));
        }

        public IEnumerable<FollowElement> GetFollowElements(IEnumerable<StorageDataSet.FollowMapRow> rows)
        {
            return rows.Select(row => this.GetFollowElement(row));
        }

        public FollowElement GetFollowElement(StorageDataSet.FollowMapRow row)
        {
            FollowElement element = this.NewFollowElement();
            element.UnderlyingDataRow = row;
            return element;
        }

        public FollowElement NewFollowElement()
        {
            return new FollowElement()
            {
                Storage = this,
            };
        }
        #endregion

        #region Posts
        public abstract StorageDataSet.PostsDataTable GetPostsDataTable();

        public IEnumerable<Post> GetPosts()
        {
            return this.GetPosts(row => true);
        }

        public IEnumerable<Post> GetPosts(Func<StorageDataSet.PostsRow, Boolean> predicate)
        {
            return this.GetPostsDataTable().Where(predicate).Select(row => this.GetPost(row));
        }

        public IEnumerable<Post> GetPosts(IEnumerable<StorageDataSet.PostsRow> rows)
        {
            return rows.Select(row => this.GetPost(row));
        }

        public Post GetPost(StorageDataSet.PostsRow row)
        {
            Post post = this.NewPost();
            post.UnderlyingDataRow = row;
            return post;
        }

        public Post NewPost()
        {
            return new Post()
            {
                Storage = this,
            };
        }
        #endregion

        #region ReplyMap
        public abstract StorageDataSet.ReplyMapDataTable GetReplyMapTable();

        public IEnumerable<ReplyElement> GetReplyElements()
        {
            return this.GetReplyElements(row => true);
        }

        public IEnumerable<ReplyElement> GetReplyElements(Func<StorageDataSet.ReplyMapRow, Boolean> predicate)
        {
            return this.GetReplyMapTable().Where(predicate).Select(row => this.GetReplyElement(row));
        }

        public IEnumerable<ReplyElement> GetReplyElements(IEnumerable<StorageDataSet.ReplyMapRow> rows)
        {
            return rows.Select(row => this.GetReplyElement(row));
        }

        public ReplyElement GetReplyElement(StorageDataSet.ReplyMapRow row)
        {
            ReplyElement element = this.NewReplyElement();
            element.UnderlyingDataRow = row;
            return element;
        }

        public ReplyElement NewReplyElement()
        {
            return new ReplyElement()
            {
                Storage = this,
            };
        }
        #endregion

        #region TagMap
        public abstract StorageDataSet.TagMapDataTable GetTagMapDataTable();

        public IEnumerable<TagElement> GetTagElements()
        {
            return this.GetTagElements(row => true);
        }

        public IEnumerable<TagElement> GetTagElements(Func<StorageDataSet.TagMapRow, Boolean> predicate)
        {
            return this.GetTagMapDataTable().Where(predicate).Select(row => this.GetTagElement(row));
        }

        public IEnumerable<TagElement> GetTagElements(IEnumerable<StorageDataSet.TagMapRow> rows)
        {
            return rows.Select(row => this.GetTagElement(row));
        }

        public TagElement GetTagElement(StorageDataSet.TagMapRow row)
        {
            TagElement element = this.NewTagElement();
            element.UnderlyingDataRow = row;
            return element;
        }

        public TagElement NewTagElement()
        {
            return new TagElement()
            {
                Storage = this,
            };
        }
        #endregion

        public abstract void Update();
    }
}