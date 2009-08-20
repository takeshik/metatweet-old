// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
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
    partial class FavorElement
    {
        [Serializable()]
        private sealed class InternalRow
            : IFavorMapRow,
              ISupportInitialize
        {
            [NonSerialized()]
            private Boolean _isInitializing;

            private Guid _accountId;

            private Guid _favoringAccountId;

            private DateTime _favoringTimestamp;

            private String _favoringCategory;

            private Int32 _favoringSubindex;

            [NonSerialized()]
            private Boolean _isAccountIdModified;

            [NonSerialized()]
            private Boolean _isFavoringAccountIdModified;

            [NonSerialized()]
            private Boolean _isFavoringTimestampModified;

            [NonSerialized()]
            private Boolean _isFavoringCategoryModified;

            [NonSerialized()]
            private Boolean _isFavoringSubindexModified;

            public IList<Object> Items
            {
                get
                {
                    return new Object[]
                    {
                        this.AccountId,
                        this.FavoringAccountId,
                        this.FavoringTimestamp,
                        this.FavoringCategory,
                        this.FavoringSubindex,
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

            public Guid FavoringAccountId
            {
                get
                {
                    return this._favoringAccountId;
                }
                set
                {
                    this._favoringAccountId = value;
                    if (!this._isInitializing)
                    {
                        this.IsFavoringAccountIdModified = true;
                    }
                }
            }

            public DateTime FavoringTimestamp
            {
                get
                {
                    return this._favoringTimestamp;
                }
                set
                {
                    this._favoringTimestamp = value;
                    if (!this._isInitializing)
                    {
                        this.IsFavoringTimestampModified = true;
                    }
                }
            }

            public String FavoringCategory
            {
                get
                {
                    return this._favoringCategory;
                }
                set
                {
                    this._favoringCategory = value;
                    if (!this._isInitializing)
                    {
                        this.IsFavoringCategoryModified = true;
                    }
                }
            }

            public Int32 FavoringSubindex
            {
                get
                {
                    return this._favoringSubindex;
                }
                set
                {
                    this._favoringSubindex = value;
                    if (!this._isInitializing)
                    {
                        this.IsFavoringSubindexModified = true;
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

            public Boolean IsFavoringAccountIdModified
            {
                get
                {
                    return this._isFavoringAccountIdModified;
                }
                set
                {
                    this._isFavoringAccountIdModified = value;
                }
            }
            
            public Boolean IsFavoringTimestampModified
            {
                get
                {
                    return this._isFavoringTimestampModified;
                }
                set
                {
                    this._isFavoringTimestampModified = value;
                }
            }
            
            public Boolean IsFavoringCategoryModified
            {
                get
                {
                    return this._isFavoringCategoryModified;
                }
                set
                {
                    this._isFavoringCategoryModified = value;
                }
            }
            
            public Boolean IsFavoringSubindexModified
            {
                get
                {
                    return this._isFavoringSubindexModified;
                }
                set
                {
                    this._isFavoringSubindexModified = value;
                }
            }

            public override String ToString()
            {
                return String.Format(
                    "{{Fav {0}, {1}, {2}, {3}, {4}}}",
                    this.AccountId.ToString("d"),
                    this.FavoringAccountId.ToString("d"),
                    this.FavoringTimestamp.ToString("s"),
                    this.FavoringCategory,
                    this.FavoringSubindex
                );
            }

            public void BeginInit()
            {
                this.IsAccountIdModified = false;
                this.IsFavoringAccountIdModified = false;
                this.IsFavoringTimestampModified = false;
                this.IsFavoringCategoryModified = false;
                this.IsFavoringSubindexModified = false;
                this._isInitializing = true;
            }

            public void EndInit()
            {
                this._isInitializing = false;
            }
        }
    }
}