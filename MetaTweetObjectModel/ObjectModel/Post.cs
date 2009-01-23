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
    [Serializable()]
    public class Post
        : StorageObject<StorageDataSet.PostsDataTable, StorageDataSet.PostsRow>,
          IComparable<Post>
    {
        public Activity Activity
        {
            get
            {
                return this.Storage.GetActivity(this.UnderlyingDataRow.ActivitiesRowParent);
            }
            set
            {
                this.UnderlyingDataRow.ActivitiesRowParent = value.UnderlyingDataRow;
            }
        }

        public String PostId
        {
            get
            {
                return this.UnderlyingDataRow.PostId;
            }
            set
            {
                this.UnderlyingDataRow.PostId = value;
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

        public String Text
        {
            get
            {
                return this.UnderlyingDataRow.Text;
            }
            set
            {
                this.UnderlyingDataRow.Text = value;
            }
        }

        public String Source
        {
            get
            {
                return this.UnderlyingDataRow.Source;
            }
            set
            {
                this.UnderlyingDataRow.Source = value;
            }
        }

        public IEnumerable<ReplyElement> ReplyingMap
        {
            get
            {
                return this.Storage.GetReplyElements(this.UnderlyingDataRow.GetReplyMapRowsByFK_Posts_ReplyMap());
            }
        }

        public IEnumerable<Post> Replying
        {
            get
            {
                return this.ReplyingMap.Select(e => e.InReplyToPost);
            }
        }

        public IEnumerable<ReplyElement> RepliesMap
        {
            get
            {
                return this.Storage.GetReplyElements(this.UnderlyingDataRow.GetReplyMapRowsByFK_Posts_ReplyMap());
            }
        }

        public IEnumerable<Post> Replies
        {
            get
            {
                return this.RepliesMap.Select(e => e.Post);
            }
        }

        public Int32 CompareTo(Post other)
        {
            Int32 result;
            if ((result = this.Activity.CompareTo(other.Activity)) != 0)
            {
                return result;
            }
            else
            {
                return this.PostId.CompareTo((other as Post).PostId);
            }
        }

        public override String ToString()
        {
            return string.Format(
                "#({0}): \"{1}\"",
                this.PostId,
                this.Text
            );
        }

        public void AddReplying(Post post)
        {
            ReplyElement element = this.Storage.NewReplyElement();
            element.Post = this;
            element.InReplyToPost = post;
            element.Update();
        }

        public void AddReply(Post post)
        {
            ReplyElement element = this.Storage.NewReplyElement();
            element.Post = post;
            element.InReplyToPost = this;
            element.Update();
        }

        public void RemoveReplying(Post post)
        {
            ReplyElement element = this.ReplyingMap.Single(e => e.InReplyToPost == post);
            element.Delete();
            element.Update();
        }

        public void RemoveReply(Post post)
        {
            ReplyElement element = this.ReplyingMap.Single(e => e.Post == post);
            element.Delete();
            element.Update();
        }
    }
}