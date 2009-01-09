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
    public class Post
        : StorageObject<StorageDataSet.PostsDataTable, StorageDataSet.PostsRow>,
          IComparable<Post>
    {
        private Activity _activity;

        private String _postId;

        private Nullable<DateTime> _timestamp;
        
        private String _text;
        
        private String _source;
        
        private Nullable<Int32> _favoriteCount;
        
        private Nullable<Boolean> _isFavorited;
        
        private Nullable<Boolean> _isRestricted;

        private ICollection<ReplyElement> _replyMap;

        public Activity Activity
        {
            get
            {
                return this._activity ?? (this._activity = this.Storage.GetActivity(
                    this.UnderlyingDataRow.ActivitiesRowParent
                ));
            }
            set
            {
                this.UnderlyingDataRow.AccountId = value.Account.AccountId;
                this._activity = value;
            }
        }
        
        public String PostId
        {
            get
            {
                return this._postId ?? (this._postId = this.UnderlyingDataRow.PostId);
            }
            set
            {
                this.UnderlyingDataRow.PostId = value;
                this._postId = value;
            }
        }

        public DateTime Timestamp
        {
            get
            {
                if (!this._timestamp.HasValue)
                {
                    this._timestamp = this.UnderlyingDataRow.Timestamp;
                }
                return this._timestamp.Value;
            }
            set
            {
                this.UnderlyingDataRow.Timestamp = value;
                this._timestamp = value;
            }
        }

        public String Text
        {
            get
            {
                return this._text ?? (this._text = this.UnderlyingDataRow.Text);
            }
            set
            {
                this.UnderlyingDataRow.Text = value;
                this._text = value;
            }
        }

        public String Source
        {
            get
            {
                return this._source ?? (this._source = this.UnderlyingDataRow.Source);
            }
            set
            {
                this.UnderlyingDataRow.Source = value;
                this._source = value;
            }
        }

        public Nullable<Int32> FavoriteCount
        {
            get
            {
                if (this.UnderlyingDataRow.IsFavoriteCountNull())
                {
                    return null;
                }
                else if (!this._favoriteCount.HasValue)
                {
                    this._favoriteCount = this.UnderlyingDataRow.FavoriteCount;
                }
                return this._favoriteCount;
            }
            set
            {
                if (value == null)
                {
                    this.UnderlyingDataRow.SetFavoriteCountNull();
                }
                else
                {
                    this._favoriteCount = value;
                }
            }
        }

        public Boolean IsFavorited
        {
            get
            {
                if (!this._isFavorited.HasValue)
                {
                    this._isFavorited = this.UnderlyingDataRow.IsFavorited;
                }
                return this._isFavorited.Value;
            }
            set
            {
                this.UnderlyingDataRow.IsFavorited = value;
                this._isFavorited = value;
            }
        }

        public Boolean IsRestricted
        {
            get
            {
                if (!this._isRestricted.HasValue)
                {
                    this._isRestricted = this.UnderlyingDataRow.IsRestricted;
                }
                return _isRestricted.Value;
            }
            set
            {
                this.UnderlyingDataRow.IsRestricted = value;
                this._isRestricted = value;
            }
        }

        public ICollection<ReplyElement> ReplyMap
        {
            get
            {
                return this._replyMap ?? (this._replyMap = this.Storage.GetReplyElements(
                    row => (
                           row.AccountId == this.Activity.Account.AccountId
                        && row.PostId == this.PostId
                    ) || (
                           row.InReplyToAccountId == this.Activity.Account.AccountId
                        && row.InReplyToPostId == this.PostId
                    )
                ).ToList());
            }
        }

        public IEnumerable<Post> Replying
        {
            get
            {
                return this.ReplyMap.Where(e => e.Post == this).Select(e => e.InReplyToPost);
            }
        }

        public IEnumerable<Post> Replies
        {
            get
            {
                return this.ReplyMap.Where(e => e.InReplyToPost == this).Select(e => e.Post);
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

        public override Boolean Equals(Object obj)
        {
            Post other = obj as Post;
            return this.Activity == other.Activity && this.PostId == other.PostId;
        }

        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        public override String ToString()
        {
            return string.Format(
                "#({0}): \"{1}\"{2}",
                this.PostId,
                this.Text,
                this.IsFavorited ? " (*)" : String.Empty
            );
        }

        protected override void UpdateImpl()
        {
            this.Storage.Update(this.UnderlyingDataRow);
        }
    }
}