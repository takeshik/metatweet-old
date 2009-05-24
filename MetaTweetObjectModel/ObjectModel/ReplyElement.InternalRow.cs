// -*- mode: csharp; encoding: utf-8; -*-
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetObjectModel
 *   Object model and Storage interface for MetaTweet and other systems
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
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

namespace XSpect.MetaTweet.ObjectModel
{
    partial class ReplyElement
    {
        private sealed class InternalRow
            : IReplyMapRow
        {
            private Guid _accountId;

            private String _postId;

            private Guid _inReplyToAccountId;

            private String _inReplyToPostId;

            [NonSerialized()]
            private Boolean _isAccountIdModified;

            [NonSerialized()]
            private Boolean _isPostIdModified;

            [NonSerialized()]
            private Boolean _isInReplyToAccountIdModified;

            [NonSerialized()]
            private Boolean _isInReplyToPostIdModified;

            public Guid AccountId
            {
                get
                {
                    return this._accountId;
                }
                set
                {
                    this._accountId = value;
                    this.IsAccountIdModified = true;
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
                    this.IsPostIdModified = true;
                }
            }

            public Guid InReplyToAccountId
            {
                get
                {
                    return this._inReplyToAccountId;
                }
                set
                {
                    this._inReplyToAccountId = value;
                    this.IsInReplyToAccountIdModified = true;
                }
            }

            public String InReplyToPostId
            {
                get
                {
                    return this._inReplyToPostId;
                }
                set
                {
                    this._inReplyToPostId = value;
                    this.IsInReplyToPostIdModified = true;
                }
            }

            public Boolean IsAccountIdModified
            {
                get
                {
                    return this._isAccountIdModified;
                }
                set
                {
                    this._isAccountIdModified = value;
                }
            }

            public Boolean IsPostIdModified
            {
                get
                {
                    return this._isPostIdModified;
                }
                set
                {
                    this._isPostIdModified = value;
                }
            }

            public Boolean IsInReplyToAccountIdModified
            {
                get
                {
                    return this._isInReplyToAccountIdModified;
                }
                set
                {
                    this._isInReplyToAccountIdModified = value;
                }
            }

            public Boolean IsInReplyToPostIdModified
            {
                get
                {
                    return this._isInReplyToPostIdModified;
                }
                set
                {
                    this._isInReplyToPostIdModified = value;
                }
            }
        }
    }
}