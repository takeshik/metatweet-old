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
using System.Linq;

namespace XSpect.MetaTweet.ObjectModel
{
    [Serializable()]
    public class Activity
        : StorageObject<StorageDataSet.ActivitiesDataTable, StorageDataSet.ActivitiesRow>,
          IComparable<Activity>
    {
        public Account Account
        {
            get
            {
                return this.Storage.GetAccount(this.UnderlyingDataRow.AccountsRow);
            }
            set
            {
                this.UnderlyingDataRow.AccountsRow = value.UnderlyingDataRow;
            }
        }

        public DateTime Timestamp
        {
            get
            {
                return this.UnderlyingDataRow.Timestamp;
            }
            set
            {
                this.UnderlyingDataRow.Timestamp = value;
            }
        }

        public String Category
        {
            get
            {
                return this.UnderlyingDataRow.Category;
            }
            set
            {
                this.UnderlyingDataRow.Category = value;
            }
        }

        public String Value
        {
            get
            {
                return this.UnderlyingDataRow.IsValueNull()
                    ? null
                    : this.UnderlyingDataRow.Value;
            }
            set
            {
                if (value != null)
                {
                    this.UnderlyingDataRow.Value = value;
                }
                else
                {
                    this.UnderlyingDataRow.SetValueNull();
                }
            }
        }

        public Byte[] Data
        {
            get
            {
                return this.UnderlyingDataRow.IsDataNull()
                    ? null
                    : this.UnderlyingDataRow.Data;
            }
            set
            {
                if (value != null)
                {
                    this.UnderlyingDataRow.Data = value;
                }
                else
                {
                    this.UnderlyingDataRow.SetDataNull();
                }
            }
        }

        public IEnumerable<TagElement> TagMap
        {
            get
            {
                return this.Storage.GetTagElements(this.UnderlyingDataRow.GetTagMapRows());
            }
        }

        public IEnumerable<String> Tags
        {
            get
            {
                return this.TagMap.Select(e => e.Tag);
            }
        }

        internal Activity()
        {
        }

        public virtual Int32 CompareTo(Activity other)
        {
            Int32 result;
            if ((result = this.Timestamp.CompareTo(other.Timestamp)) != 0)
            {
                return result;
            }
            else if ((result = this.Account.CompareTo(other.Account)) != 0)
            {
                return result;
            }
            else
            {
                return this.Category.CompareTo(other.Category);
            }
        }

        public override String ToString()
        {
            return String.Format(
                "{0}: {1} = \"{2}\"",
                this.Timestamp.ToString("s"),
                this.Category,
                this.Value != null ? this.Value : "(null)"
            );
        }

        public void AddTag(String tag)
        {
            TagElement element = this.Storage.NewTagElement();
            element.Activity = this;
            element.Tag = tag;
            element.Update();
        }

        public void RemoveTag(String tag)
        {
            TagElement element = this.TagMap.Where(e => e.Tag == tag).Single();
            element.Delete();
            element.Update();
        }

        public Post ToPost()
        {
            if (this.Category != "Post")
            {
                // TODO: exception string resource
                throw new InvalidOperationException();
            }
            StorageDataSet.PostsRow row = this.UnderlyingDataRow.GetPostsRows().SingleOrDefault();
            if (row != null)
            {
                return this.Storage.GetPost(row);
            }
            else
            {
                return this.NewPost();
            }
        }

        public Post NewPost()
        {
            Post post = this.Storage.NewPost();
            post.UnderlyingDataRow.ActivitiesRowParent = this.UnderlyingDataRow;
            return post;
        }
    }
}