// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetObjectModel
 *   Object model and Storage interface for MetaTweet and other systems
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
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
using System.Linq;

namespace XSpect.MetaTweet.Objects
{
    [Serializable()]
    public class ActivityCreationData
        : StorageObjectCreationData
    {
        private readonly Lazy<ActivityId> _id;

        private Account _account;

        private AccountId _accountId;

        public ActivityId Id
        {
            get
            {
                if (this.AccountId == default(AccountId)
                    || this.Name == null
                    || this.Value == null
                )
                {
                    throw new InvalidOperationException();
                }
                return this._id.Value;
            }
        }

        public Account Account
        {
            get
            {
                return this._account;
            }
            set
            {
                this._account = value;
                this._accountId = value.Id;
            }
        }

        public AccountId AccountId
        {
            get
            {
                return this._accountId;
            }
            set
            {
                this._account = null;
                this._accountId = value;
            }
        }

        public IEnumerable<ActivityId> AncestorIds
        {
            get;
            set;
        }

        public String Name
        {
            get;
            set;
        }

        public Object Value
        {
            get;
            set;
        }

        public ICollection<ActivityId> SelfAndAncestorIds
        {
            get
            {
                return this.AncestorIds.StartWith(this.Id).ToArray();
            }
        }

        public ActivityCreationData()
        {
            this._id = new Lazy<ActivityId>(() => ActivityId.Create(this.AccountId, this.AncestorIds, this.Name, this.Value));
        }

        public Account GetAccount(StorageSession context)
        {
            return this._account != null && this._account.Context == context
                ? this._account
                : this._account = context.Load(this._accountId);
        }
    }
}