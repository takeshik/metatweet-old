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
using System.ComponentModel;

namespace XSpect.MetaTweet.ObjectModel
{
    partial class Post
    {
        private sealed class InternalRow
            : IPostsRow,
              ISupportInitialize
        {
            [NonSerialized()]
            private Boolean _isInitializing;

            private Guid _accountId;

            private String _postId;

            private String _text;

            private String _source;

            [NonSerialized()]
            private Boolean _isAccountIdModified;

            [NonSerialized()]
            private Boolean _isPostIdModified;

            [NonSerialized()]
            private Boolean _isTextModified;

            [NonSerialized()]
            private Boolean _isSourceModified;

            public Guid AccountId
            {
                get
                {
                    return this._accountId;
                }
                set
                {
                    this._accountId = value;
                    if (!this._isInitializing)
                    {
                        this.IsAccountIdModified = true;
                    }
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
                    if (!this._isInitializing)
                    {
                        this.IsPostIdModified = true;
                    }
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
                    if (!this._isInitializing)
                    {
                        this.IsTextModified = true;
                    }
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
                    if (!this._isInitializing)
                    {
                        this.IsSourceModified = true;
                    }
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

            public Boolean IsTextModified
            {
                get
                {
                    return this._isTextModified;
                }
                set
                {
                    this._isTextModified = value;
                }
            }

            public Boolean IsSourceModified
            {
                get
                {
                    return this._isSourceModified;
                }
                set
                {
                    this._isSourceModified = value;
                }
            }

            public void BeginInit()
            {
                this.IsAccountIdModified = false;
                this.IsPostIdModified = false;
                this.IsTextModified = false;
                this.IsSourceModified = false;
                this._isInitializing = true;
            }

            public void EndInit()
            {
                this._isInitializing = false;
            }
        }
    }
}