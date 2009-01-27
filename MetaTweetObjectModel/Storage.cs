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

        public void Initialize(IDictionary<String, String> args)
        {
            if (args.ContainsKey("connection"))
            {
                this.Initialize(args["connection"]);
            }
        }

        public abstract void Initialize(String connectionString);

        public abstract void Connect();

        public abstract void Disconnect();

        public void Dispose()
        {
            this.Disconnect();
        }

        #region Accounts
        public abstract Int32 FillAccountsBy(String query, params Object[] args);

        public IEnumerable<Account> GetAccounts()
        {
            return this.GetAccounts(row => true);
        }

        public IEnumerable<Account> GetAccounts(Func<StorageDataSet.AccountsRow, Boolean> predicate)
        {
            return this.GetAccounts(this.UnderlyingDataSet.Accounts.Where(predicate));
        }

        public IEnumerable<Account> GetAccounts(IEnumerable<StorageDataSet.AccountsRow> rows)
        {
            return rows.Select(row => this.GetAccount(row));
        }

        public Account GetAccount(StorageDataSet.AccountsRow row)
        {
            return new Account(row)
            {
                Storage = this,
            };
        }

        public Account NewAccount(Guid accountId, String realm)
        {
            Account account = new Account(accountId, realm)
            {
                Storage = this,
            };
            return account;
        }
        #endregion

        #region Activities
        public abstract Int32 FillActivitiesBy(String query, params Object[] args);

        public IEnumerable<Activity> GetActivities()
        {
            return this.GetActivities(row => true);
        }

        public IEnumerable<Activity> GetActivities(Func<StorageDataSet.ActivitiesRow, Boolean> predicate)
        {
            return this.GetActivities(this.UnderlyingDataSet.Activities.Where(predicate));
        }

        public IEnumerable<Activity> GetActivities(IEnumerable<StorageDataSet.ActivitiesRow> rows)
        {
            return rows.Select(row => this.GetActivity(row));
        }

        public Activity GetActivity(StorageDataSet.ActivitiesRow row)
        {
            return new Activity(row)
            {
                Storage = this,
            };
        }

        public Activity NewActivity(
            Account account,
            DateTime timestamp,
            String category
        )
        {
            Activity activity = new Activity(account, timestamp, category)
            {
                Storage = this,
            };
            return activity;
        }
        #endregion

        #region FavorMap
        public abstract Int32 FillFavorMapBy(String query, params Object[] args);

        public IEnumerable<FavorElement> GetFavorElements()
        {
            return this.GetFavorElements(row => true);
        }

        public IEnumerable<FavorElement> GetFavorElements(Func<StorageDataSet.FavorMapRow, Boolean> predicate)
        {
            return this.GetFavorElements(this.UnderlyingDataSet.FavorMap.Where(predicate));
        }

        public IEnumerable<FavorElement> GetFavorElements(IEnumerable<StorageDataSet.FavorMapRow> rows)
        {
            return rows.Select(row => this.GetFavorElement(row));
        }

        public FavorElement GetFavorElement(StorageDataSet.FavorMapRow row)
        {
            return new FavorElement(row)
            {
                Storage = this,
            };
        }

        public FavorElement NewFavorElement(
            Account account,
            Activity favoringActivity
        )
        {
            FavorElement element = new FavorElement(account, favoringActivity)
            {
                Storage = this,
            };
            return element;
        }
        #endregion

        #region FollowMap
        public abstract Int32 FillFollowMapBy(String query, params Object[] args);

        public IEnumerable<FollowElement> GetFollowElements()
        {
            return this.GetFollowElements(row => true);
        }

        public IEnumerable<FollowElement> GetFollowElements(Func<StorageDataSet.FollowMapRow, Boolean> predicate)
        {
            return this.GetFollowElements(this.UnderlyingDataSet.FollowMap.Where(predicate));
        }

        public IEnumerable<FollowElement> GetFollowElements(IEnumerable<StorageDataSet.FollowMapRow> rows)
        {
            return rows.Select(row => this.GetFollowElement(row));
        }

        public FollowElement GetFollowElement(StorageDataSet.FollowMapRow row)
        {
            return new FollowElement(row)
            {
                Storage = this,
            };
        }

        public FollowElement NewFollowElement(
            Account account,
            Account followingAccount
        )
        {
            FollowElement element = new FollowElement(account, followingAccount)
            {
                Storage = this,
            };
            return element;
        }
        #endregion

        #region Posts
        public abstract Int32 FillPostsBy(String query, params Object[] args);

        public IEnumerable<Post> GetPosts()
        {
            return this.GetPosts(row => true);
        }

        public IEnumerable<Post> GetPosts(Func<StorageDataSet.PostsRow, Boolean> predicate)
        {
            return this.GetPosts(this.UnderlyingDataSet.Posts.Where(predicate));
        }

        public IEnumerable<Post> GetPosts(IEnumerable<StorageDataSet.PostsRow> rows)
        {
            return rows.Select(row => this.GetPost(row));
        }

        public Post GetPost(StorageDataSet.PostsRow row)
        {
            return new Post(row)
            {
                Storage = this,
            };
        }

        public Post NewPost(
            Activity activity
        )
        {
            // TODO: Check the property setting
            Post post = new Post(activity)
            {
                Storage = this,
            };
            return post;
        }
        #endregion

        #region ReplyMap
        public abstract Int32 FillReplyMapBy(String query, params Object[] args);

        public IEnumerable<ReplyElement> GetReplyElements()
        {
            return this.GetReplyElements(row => true);
        }

        public IEnumerable<ReplyElement> GetReplyElements(Func<StorageDataSet.ReplyMapRow, Boolean> predicate)
        {
            return this.GetReplyElements(this.UnderlyingDataSet.ReplyMap.Where(predicate));
        }

        public IEnumerable<ReplyElement> GetReplyElements(IEnumerable<StorageDataSet.ReplyMapRow> rows)
        {
            return rows.Select(row => this.GetReplyElement(row));
        }

        public ReplyElement GetReplyElement(StorageDataSet.ReplyMapRow row)
        {
            return new ReplyElement(row)
            {
                Storage = this,
            };
        }

        public ReplyElement NewReplyElement(
            Post post,
            Post inReplyToPost
        )
        {
            ReplyElement element = new ReplyElement(post, inReplyToPost)
            {
                Storage = this,
            };
            return element;
        }
        #endregion

        #region TagMap
        public abstract Int32 FillTagMapBy(String query, params Object[] args);

        public IEnumerable<TagElement> GetTagElements()
        {
            return this.GetTagElements(row => true);
        }

        public IEnumerable<TagElement> GetTagElements(Func<StorageDataSet.TagMapRow, Boolean> predicate)
        {
            return this.GetTagElements(this.UnderlyingDataSet.TagMap.Where(predicate));
        }

        public IEnumerable<TagElement> GetTagElements(IEnumerable<StorageDataSet.TagMapRow> rows)
        {
            return rows.Select(row => this.GetTagElement(row));
        }

        public TagElement GetTagElement(StorageDataSet.TagMapRow row)
        {
            return new TagElement(row)
            {
                Storage = this,
            };
        }

        public TagElement NewTagElement(
            Activity activity,
            String tag
        )
        {
            TagElement element = new TagElement(activity, tag)
            {
                Storage = this,
            };
            return element;
        }
        #endregion

        public abstract void Update();

        public virtual void Merge(Storage destination)
        {
            this.UnderlyingDataSet.Merge(destination.UnderlyingDataSet);
        }
    }
}