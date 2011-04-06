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
    public abstract class StorageObjectCreationData
    {
        public static AccountCreationData Create(String realm, String seed)
        {
            return new AccountCreationData()
            {
                Realm = realm,
                Seed = seed,
            };
        }

        public static ActivityCreationData Create(Account account, IEnumerable<ActivityId> ancestorIds, String name, Object value)
        {
            return new ActivityCreationData()
            {
                Account = account,
                AncestorIds = ancestorIds ?? Enumerable.Empty<ActivityId>(),
                Name = name,
                Value = value,
            };
        }

        public static ActivityCreationData Create(AccountId accountId, IEnumerable<ActivityId> ancestorIds, String name, Object value)
        {
            return new ActivityCreationData()
            {
                AccountId = accountId,
                AncestorIds = ancestorIds ?? Enumerable.Empty<ActivityId>(),
                Name = name,
                Value = value,
            };
        }

        public static AdvertisementCreationData Create(Activity activity, DateTime timestamp, AdvertisementFlags flags)
        {
            return new AdvertisementCreationData()
            {
                Activity = activity,
                Timestamp = timestamp,
                Flags = flags,
            };
        }

        public static AdvertisementCreationData Create(ActivityId activityId, DateTime timestamp, AdvertisementFlags flags)
        {
            return new AdvertisementCreationData()
            {
                ActivityId = activityId,
                Timestamp = timestamp,
                Flags = flags,
            };
        }
    }
}