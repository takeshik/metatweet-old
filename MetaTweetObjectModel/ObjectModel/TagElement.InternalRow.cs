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
using System.Collections.Generic;
using System.ComponentModel;

namespace XSpect.MetaTweet.ObjectModel
{
    partial class TagElement
    {
        [Serializable()]
        private sealed class InternalRow
            : ITagMapRow,
              ISupportInitialize
        {
            [NonSerialized()]
            private Boolean _isInitializing;

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

            public IList<Object> Items
            {
                get
                {
                    return new Object[]
                    {
                        this.AccountId,
                        this.Timestamp,
                        this.Category,
                        this.Subindex,
                        this.Tag,
                    };
                }
            }

            public IList<Object> PrimaryKeys
            {
                get
                {
                    return this.Items;
                }
            }

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

            public DateTime Timestamp
            {
                get
                {
                    return this._timestamp;
                }
                set
                {
                    this._timestamp = value;
                    if (!this._isInitializing)
                    {
                        this.IsTimestampModified = true;
                    }
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
                    if (!this._isInitializing)
                    {
                        this.IsCategoryModified = true;
                    }
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
                    if (!this._isInitializing)
                    {
                        this.IsSubindexModified = true;
                    }
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
                    if (!this._isInitializing)
                    {
                        this.IsTagModified = true;
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

            public override String ToString()
            {
                return String.Format(
                    "{{Tag {0}, {1}, {2}, {3}, {4}}}",
                    this.AccountId.ToString("d"),
                    this.Timestamp.ToString("s"),
                    this.Category,
                    this.Subindex,
                    this.Tag
                );
            }

            public void BeginInit()
            {
                this.IsAccountIdModified = false;
                this.IsTimestampModified = false;
                this.IsCategoryModified = false;
                this.IsSubindexModified = false;
                this.IsTagModified = false;
                this._isInitializing = true;
            }

            public void EndInit()
            {
                this._isInitializing = false;
            }
        }
    }
}