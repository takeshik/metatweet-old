// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008 Takeshi KIRIYA, XSpect Project <takeshik@xspect.org>
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
using System.ComponentModel;

namespace XSpect.MetaTweet
{
    public sealed class StorageDataSetUnit
        : Component,
          ICloneable
    {
        private StorageDataSet.AccountsDataTable _accounts;

        private StorageDataSet.ActivitiesDataTable _activities;

        private StorageDataSet.FollowMapDataTable _followMap;

        private StorageDataSet.PicturesDataTable _pictures;

        private StorageDataSet.PostsDataTable _posts;

        private StorageDataSet.ReplyMapDataTable _replyMap;

        private Exception _exception;

        public StorageDataSet.AccountsDataTable Accounts
        {
            get
            {
                return this._accounts;
            }
            set
            {
                this._accounts = value ?? new StorageDataSet.AccountsDataTable();
            }
        }

        public StorageDataSet.ActivitiesDataTable Activities
        {
            get
            {
                return this._activities;
            }
            set
            {
                this._activities = value ?? new StorageDataSet.ActivitiesDataTable();
            }
        }

        public StorageDataSet.FollowMapDataTable FollowMap
        {
            get
            {
                return this._followMap;
            }
            set
            {
                this._followMap = value ?? new StorageDataSet.FollowMapDataTable();
            }
        }

        public StorageDataSet.PicturesDataTable Pictures
        {
            get
            {
                return this._pictures;
            }
            set
            {
                this._pictures = value ?? new StorageDataSet.PicturesDataTable();
            }
        }

        public StorageDataSet.PostsDataTable Posts
        {
            get
            {
                return this._posts;
            }
            set
            {
                this._posts = value ?? new StorageDataSet.PostsDataTable();
            }
        }

        public StorageDataSet.ReplyMapDataTable ReplyMap
        {
            get
            {
                return this._replyMap;
            }
            set
            {
                this._replyMap = value ?? new StorageDataSet.ReplyMapDataTable();
            }
        }

        public Exception Exception
        {
            get
            {
                return this._exception;
            }
            set
            {
                this._exception = value;
            }
        }

        public Boolean IsSucceeded
        {
            get
            {
                return this._exception == null;
            }
        }

        public StorageDataSetUnit()
        {
        }

        public StorageDataSetUnit(
            StorageDataSet.AccountsDataTable accounts,
            StorageDataSet.ActivitiesDataTable activities,
            StorageDataSet.FollowMapDataTable followMap,
            StorageDataSet.PicturesDataTable pictures,
            StorageDataSet.PostsDataTable posts,
            StorageDataSet.ReplyMapDataTable replyMap
        )
        {
            this._accounts = accounts ?? new StorageDataSet.AccountsDataTable();
            this._activities = activities ?? new StorageDataSet.ActivitiesDataTable();
            this._followMap = followMap ?? new StorageDataSet.FollowMapDataTable();
            this._pictures = pictures ?? new StorageDataSet.PicturesDataTable();
            this._posts = posts ?? new StorageDataSet.PostsDataTable();
            this._replyMap = replyMap ?? new StorageDataSet.ReplyMapDataTable();
        }

        public static StorageDataSetUnit operator +(StorageDataSetUnit self, StorageDataSetUnit other)
        {
            StorageDataSetUnit unit = self.Clone();
            unit._accounts.Merge(other._accounts);
            unit._activities.Merge(other._activities);
            unit._followMap.Merge(other._followMap);
            unit._pictures.Merge(other._pictures);
            unit._posts.Merge(other._posts);
            unit._replyMap.Merge(other._replyMap);
            return unit;
        }

        public static StorageDataSetUnit operator +(StorageDataSetUnit self, StorageDataSet.AccountsDataTable accounts)
        {
            StorageDataSetUnit unit = self.Clone();
            unit._accounts.Merge(accounts);
            return unit;
        }

        public static StorageDataSetUnit operator +(StorageDataSetUnit self, StorageDataSet.ActivitiesDataTable activities)
        {
            StorageDataSetUnit unit = self.Clone();
            unit._activities.Merge(activities);
            return unit;
        }

        public static StorageDataSetUnit operator +(StorageDataSetUnit self, StorageDataSet.FollowMapDataTable followMap)
        {
            StorageDataSetUnit unit = self.Clone();
            unit._followMap.Merge(followMap);
            return unit;
        }

        public static StorageDataSetUnit operator +(StorageDataSetUnit self, StorageDataSet.PicturesDataTable pictures)
        {
            StorageDataSetUnit unit = self.Clone();
            unit._pictures.Merge(pictures);
            return unit;
        }

        public static StorageDataSetUnit operator +(StorageDataSetUnit self, StorageDataSet.PostsDataTable posts)
        {
            StorageDataSetUnit unit = self.Clone();
            unit._posts.Merge(posts);
            return unit;
        }

        public static StorageDataSetUnit operator +(StorageDataSetUnit self, StorageDataSet.ReplyMapDataTable replyMap)
        {
            StorageDataSetUnit unit = self.Clone();
            unit._replyMap.Merge(replyMap);
            return unit;
        }

        Object ICloneable.Clone()
        {
            return this.Clone();
        }

        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                if (this._accounts != null)
                {
                    this._accounts.Dispose();
                }
                if (this._activities != null)
                {
                    this._activities.Dispose();
                }
                if (this._followMap != null)
                {
                    this._followMap.Dispose();
                }
                if (this._pictures != null)
                {
                    this._pictures.Dispose();
                }
                if (this._posts != null)
                {
                    this._posts.Dispose();
                }
                if (this._replyMap != null)
                {
                    this._replyMap.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        public StorageDataSetUnit Clone()
        {
            return new StorageDataSetUnit(
                this._accounts.Clone() as StorageDataSet.AccountsDataTable,
                this._activities.Clone() as StorageDataSet.ActivitiesDataTable,
                this._followMap.Clone() as StorageDataSet.FollowMapDataTable,
                this._pictures.Clone() as StorageDataSet.PicturesDataTable,
                this._posts.Clone() as StorageDataSet.PostsDataTable,
                this._replyMap.Clone() as StorageDataSet.ReplyMapDataTable
            );
        }
    }
}