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
    partial class TagElement
    {
        private sealed class InternalRow
            : ITagMapRow
        {
            private Guid _accountId;

            private DateTime _timestamp;

            private String _category;

            private Int32 _subindex;

            private String _tag;

            [NonSerialized()]
            private Boolean _isAccountIdModified;

            [NonSerialized()]
            private Boolean _isTimestampModified;

            [NonSerialized()]
            private Boolean _isCategoryModified;

            [NonSerialized()]
            private Boolean _isSubindexModified;

            [NonSerialized()]
            private Boolean _isTagModified;

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

            public DateTime Timestamp
            {
                get
                {
                    return this._timestamp;
                }
                set
                {
                    this._timestamp = value;
                    this.IsTimestampModified = true;
                }
            }

            public String Category
            {
                get
                {
                    return this._category;
                }
                set
                {
                    this._category = value;
                    this.IsCategoryModified = true;
                }
            }

            public Int32 Subindex
            {
                get
                {
                    return this._subindex;
                }
                set
                {
                    this._subindex = value;
                    this.IsSubindexModified = true;
                }
            }

            public String Tag
            {
                get
                {
                    return this._tag;
                }
                set
                {
                    this._tag = value;
                    this.IsTagModified = true;
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

            public Boolean IsTimestampModified
            {
                get
                {
                    return this._isTimestampModified;
                }
                set
                {
                    this._isTimestampModified = value;
                }
            }

            public Boolean IsCategoryModified
            {
                get
                {
                    return this._isCategoryModified;
                }
                set
                {
                    this._isCategoryModified = value;
                }
            }

            public Boolean IsSubindexModified
            {
                get
                {
                    return this._isSubindexModified;
                }
                set
                {
                    this._isSubindexModified = value;
                }
            }

            public Boolean IsTagModified
            {
                get
                {
                    return this._isTagModified;
                }
                set
                {
                    this._isTagModified = value;
                }
            }
        }
    }
}