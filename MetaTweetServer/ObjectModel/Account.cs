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
    public class Account
        : StorageObject<StorageDataSet.AccountsDataTable, StorageDataSet.AccountsRow>,
          IComparable<Account>
    {
        private Nullable<Guid> _accountId;

        private String _realm;

        private ICollection<FollowElement> _followMap;

        private ICollection<Activity> _activities = new List<Activity>();

        public Guid AccountId
        {
            get
            {
                if (!this._accountId.HasValue)
                {
                    this._accountId = this.UnderlyingDataRow.AccountId;
                }
                return this._accountId.Value;
            }
            set
            {
                this.UnderlyingDataRow.AccountId = value;
                this._accountId = value;
            }
        }

        public String Realm
        {
            get
            {
                return this._realm ?? (this._realm = this.UnderlyingDataRow.Realm);
            }
            set
            {
                this.UnderlyingDataRow.Realm = value;
                this._realm = value;
            }
        }

        public ICollection<FollowElement> FollowMap
        {
            get
            {
                return this._followMap ?? (this._followMap = this.Storage.GetFollowElements(this));
            }
        }

        public IEnumerable<Account> Following
        {
            get
            {
                return this.FollowMap.Where(e => e.Account == this).Select(e => e.FollowingAccount);
            }
        }

        public IEnumerable<Account> Followers
        {
            get
            {
                return this.FollowMap.Where(e => e.FollowingAccount == this).Select(e => e.Account);
            }
        }

        public ICollection<Activity> Activities
        {
            get
            {
                return this._activities ?? (this._activities = this.Storage.GetActivities(this, null, null).ToArray());
            }
        }

        public override Boolean Equals(Object obj)
        {
            return this._accountId == (obj as Account)._accountId;
        }

        public override Int32 GetHashCode()
        {
            return this._accountId.GetHashCode();
        }

        public Int32 CompareTo(Account other)
        {
            return this.AccountId.CompareTo(other.AccountId);
        }

        protected override void UpdateImpl()
        {
            if (this.IsModified)
            {
                this.Storage.Update(this.UnderlyingDataRow);
            }
        }
    }
}