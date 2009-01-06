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
    public class Activity
        : StorageObject<StorageDataSet.ActivitiesDataTable, StorageDataSet.ActivitiesRow>,
          IComparable<Activity>
    {
        private Account _account;

        private Nullable<DateTime> _timestamp;

        private String _category;

        private String _value;

        private Byte[] _data;

        private TagMap _tagMap;

        public Account Account
        {
            get
            {
                // Account must be set in constructing.
                return this._account;
            }
            set
            {
                this.UnderlyingDataRow.AccountId = value.AccountId;
                this._account = value;
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

        public String Category
        {
            get
            {
                return this._category ?? (this._category = this.UnderlyingDataRow.Category);
            }
            set
            {
                this.UnderlyingDataRow.Category = value;
                this._category = value;
            }
        }

        public String Value
        {
            get
            {
                return this._value ?? (this._value = this.UnderlyingDataRow.Value);
            }
            set
            {
                this.UnderlyingDataRow.Value = value;
                this._value = value;
            }
        }

        public Byte[] Data
        {
            get
            {
                return this._data ?? (this._data = this.UnderlyingDataRow.Data);
            }
            set
            {
                this.UnderlyingDataRow.Data = value;
                this._data = value;
            }
        }

        public TagMap TagMap
        {
            get
            {
                return this._tagMap ?? (this._tagMap = this.Storage.GetTagMap(this, null));
            }
        }

        public IEnumerable<String> Tags
        {
            get
            {
                return this.TagMap.GetTags(this);
            }
        }

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

        public override Boolean Equals(Object obj)
        {
            Activity other = obj as Activity;
            return this.Account == other.Account
                && this.Category == other.Category
                && this.Timestamp == other.Timestamp;
        }

        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        protected override void UpdateImpl()
        {
            this.Storage.Update(this.UnderlyingDataRows);
        }
    }
}