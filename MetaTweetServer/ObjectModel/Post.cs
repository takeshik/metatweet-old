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
using System.Collections.Generic;

namespace XSpect.MetaTweet.ObjectModel
{
    [Serializable()]
    public class Post
        : StorageObject<StorageDataSet.PostsRow>,
          IComparable<Post>
    {
        private Activity _activity;

        private String _postId;
        
        private String _text;
        
        private String _source;
        
        private Int32 _favoriteCount;
        
        private Boolean _isRead;
        
        private Boolean _isFavorited;
        
        private Boolean _isReply;

        private Boolean _isRestricted;

        private ReplyMap _replyMap;

        public Activity Activity
        {
            get
            {
                return this._activity;
            }
            set
            {
                this._activity = value;
            }
        }
        
        public String PostId
        {
            get
            {
                return this._postId;
            }
            set
            {
                this._postId = value;
            }
        }

        public String Text
        {
            get
            {
                return this._text;
            }
            set
            {
                this._text = value;
            }
        }

        public String Source
        {
            get
            {
                return this._source;
            }
            set
            {
                this._source = value;
            }
        }

        public Int32 FavoriteCount
        {
            get
            {
                return this._favoriteCount;
            }
            set
            {
                this._favoriteCount = value;
            }
        }

        public Boolean IsRead
        {
            get
            {
                return this._isRead;
            }
            set
            {
                this._isRead = value;
            }
        }

        public Boolean IsFavorited
        {
            get
            {
                return this._isFavorited;
            }
            set
            {
                this._isFavorited = value;
            }
        }

        public Boolean IsReply
        {
            get
            {
                return this._isReply;
            }
            set
            {
                this._isReply = value;
            }
        }

        public Boolean IsRestricted
        {
            get
            {
                return _isRestricted;
            }
            set
            {
                _isRestricted = value;
            }
        }

        public ReplyMap ReplyMap
        {
            get
            {
                return this._replyMap;
            }
            set
            {
                this._replyMap = value;
            }
        }

        public IEnumerable<Post> Replies
        {
            get
            {
                return this._replyMap.GetReplies(this);
            }
        }

        public IEnumerable<Post> Replying
        {
            get
            {
                return this._replyMap.GetReplying(this);
            }
        }

        public Int32 CompareTo(Post other)
        {
            Int32 result;
            if ((result = this._activity.CompareTo(other._activity)) != 0)
            {
                return result;
            }
            else
            {
                return this._postId.CompareTo((other as Post)._postId);
            }
        }

        public override Boolean Equals(Object obj)
        {
            Post other = obj as Post;
            return this._activity == other.Activity && this._postId == other._postId;
        }

        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}