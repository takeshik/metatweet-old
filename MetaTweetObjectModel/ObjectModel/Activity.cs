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
using System.Linq;

namespace XSpect.MetaTweet.ObjectModel
{
    /// <summary>
    /// Represents activity.
    /// </summary>
    /// <remarks>
    /// <see cref="Activity"/> is the unit of <see cref="Account"/>'s actions, includes changing
    /// informations, posting, etc. <see cref="Activity"/> is distinguished by
    /// <see cref="Account"/>, <see cref="Timestamp"/>, and <see cref="Category"/>. And each
    /// <see cref="Activity"/> can have <see cref="String"/> value and/or <see cref="Byte[]"/>,
    /// and <see cref="TagElement"/> collection.
    /// </remarks>
    [Serializable()]
    public class Activity
        : StorageObject<StorageDataSet.ActivitiesDataTable, StorageDataSet.ActivitiesRow>,
          IComparable<Activity>
    {
        /// <summary>
        /// Gets or sets the parent <see cref="Account"/> of the <see cref="Activity"/>.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the timestamp when the action was raised or notified.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the name of the <see cref="Activity"/>'s category.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the value of the <see cref="Activity"/>.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the data of the <see cref="Activity"/>.
        /// </summary>
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

        /// <summary>
        /// Gets the collection of <see cref="TagElement"/> which is tagged with this
        /// <see cref="Activity"/>.
        /// </summary>
        public IEnumerable<TagElement> TagMap
        {
            get
            {
                return this.Storage.GetTagElements(this.UnderlyingDataRow.GetTagMapRows());
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="String"/> which is tagged with this
        /// <see cref="Activity"/>.
        /// </summary>
        public IEnumerable<String> Tags
        {
            get
            {
                return this.TagMap.Select(e => e.Tag);
            }
        }

        /// <summary>
        /// Compares the <see cref="Activity"/> with another <see cref="Activity"/>.
        /// </summary>
        /// <param name="other">Comparing <see cref="Activity"/>.</param>
        /// <returns>
        /// A 32-bit signed value.
        /// Negative value indicates the <see cref="Activity"/> is less than <paramref name="other"/>.
        /// Zero indicates the <see cref="Activity"/> is equal to <paramref name="other"/>.
        /// Positive value indeicates the <see cref="Activity"/> is greater than <paramref name="other"/>.
        /// </returns>
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

        /// <summary>
        /// Returuns formatted <see cref="String"/> for the <see cref="Activity"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> containing formatted data of the <see cref="Activity"/>.
        /// </returns>
        public override String ToString()
        {
            return String.Format(
                "{0}: {1} = \"{2}\"",
                this.Timestamp.ToString("s"),
                this.Category,
                this.Value != null ? this.Value : "(null)"
            );
        }

        /// <summary>
        /// Adds tag to the <see cref="Activity"/>.
        /// </summary>
        /// <param name="tag">Adding tag.</param>
        public void AddTag(String tag)
        {
            TagElement element = this.Storage.NewTagElement();
            element.Activity = this;
            element.Tag = tag;
            element.Update();
        }

        /// <summary>
        /// Removes tag in the <see cref="Activity"/>.
        /// </summary>
        /// <param name="tag">Removing tag.</param>
        public void RemoveTag(String tag)
        {
            TagElement element = this.TagMap.Where(e => e.Tag == tag).Single();
            element.Delete();
            element.Update();
        }

        /// <summary>
        /// Gets the <see cref="Post"/> object which is related with this <see cref="Activity"/>.
        /// </summary>
        /// <returns>
        /// <see cref="Post"/> object whose <see cref="PostId"/> is <see cref="Value"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// <see cref="Category"/> of the <see cref="Activity"/> is not <c>"Post"</c>.
        /// </exception>
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

        /// <summary>
        /// Creates and gets new <see cref="Post"/> whose parent is the <see cref="Activity"/> and
        /// the ID is <see cref="Value"/>.
        /// </summary>
        /// <returns>
        /// New <see cref="Post"/> whose parent is the <see cref="Activity"/> and the ID is
        /// <see cref="Value"/>.
        /// </returns>
        public Post NewPost()
        {
            Post post = this.Storage.NewPost();
            post.UnderlyingDataRow.ActivitiesRowParent = this.UnderlyingDataRow;
            return post;
        }
    }
}