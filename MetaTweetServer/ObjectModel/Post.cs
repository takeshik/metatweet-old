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
        : Activity
    {
        private String _postId;
        
        private String _text;
        
        private String _source;
        
        private Int32 _favoriteCount;
        
        private Boolean _isRead;
        
        private Boolean _isFavorited;
        
        private Boolean _isReply;
        
        private Boolean _isRestricted;

        private ReplyMap _replyMap;
        
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

        public IEnumerable<Post> Replying
        {
            get
            {
                return this._replyMap.GetReplying(this);
            }
        }

        public IEnumerable<Post> Replies
        {
            get
            {
                return this._replyMap.GetReplies(this);
            }
        }

        public ReplyMap ReplyMap
        {
            set
            {
                this._replyMap = value;
            }
        }

        public override Int32 CompareTo(Activity other)
        {
            if (other is Post && this.Account.Realm == other.Account.Realm)
            {
                return this._postId.CompareTo((other as Post)._postId);
            }
            else
            {
                return base.CompareTo(other);
            }
        }

        public override Boolean Equals(Object obj)
        {
            return obj is Post
                && base.Equals(obj)
                && this._postId == (obj as Post)._postId;
        }
    }
}