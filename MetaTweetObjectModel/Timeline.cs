// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetObjectModel
 *   Object model and Storage Int32erface for MetaTweet and other systems
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace XSpect.MetaTweet.Objects
{
    using Value = Tuple<Activity, Boolean>;

    public class Timeline
        : MarshalByRefObject,
          ICollection<TimelineEntry>
    {
        private readonly Object _lockObject;

        private readonly SortedSet<TimelineEntry> _entries;

        public Int32 Count
        {
            get
            {
                return this._entries.Count;
            }
        }

        Boolean ICollection<TimelineEntry>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public Timeline()
        {
            this._lockObject = new Object();
            this._entries = new SortedSet<TimelineEntry>();
        }

        public override Object InitializeLifetimeService()
        {
            return null;
        }

        public IEnumerator<TimelineEntry> GetEnumerator()
        {
            return this._entries
                .AsTransparent()
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        void ICollection<TimelineEntry>.Add(TimelineEntry item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            lock (this._lockObject)
            {
                this._entries.Clear();
            }
        }

        public Boolean Contains(TimelineEntry item)
        {
            return this._entries.Contains(item);
        }

        public void CopyTo(TimelineEntry[] array, Int32 arrayIndex)
        {
            ((ICollection<TimelineEntry>) this._entries).CopyTo(array, arrayIndex);
        }

        Boolean ICollection<TimelineEntry>.Remove(TimelineEntry item)
        {
            throw new NotSupportedException();
        }

        public IEnumerable<KeyValuePair<DateTime, Activity>> Lookup(
            Nullable<DateTime> timestamp = null,
            Nullable<AccountId> accountId = null,
            String name = null
        )
        {
            return this._entries
                .Where(e => e.CompareToTimestamp(timestamp, accountId, name) <= 0 && e.Created)
                .Select(e => new KeyValuePair<DateTime, Activity>(e.Timestamp, e.Activity))
                .AsTransparent();
        }

        public Activity Get(
            Nullable<DateTime> timestamp = null,
            Nullable<AccountId> accountId = null,
            String name = null
        )
        {
            lock (this._lockObject)
            {
                return this._entries
                    .FirstOrDefault(e => e.CompareToTimestamp(timestamp, accountId, name) <= 0 && e.Created)
                    .Activity;
            }
        }

        public void Update(Advertisement advertisement)
        {
            Activity activity = advertisement.Activity;
            lock (this._lockObject)
            {
                TimelineEntry entry = this._entries
                    .FirstOrDefault(e => e.Activity == activity && e.Timestamp > advertisement.Timestamp);
                switch (advertisement.Flags)
                {
                    case AdvertisementFlags.Created:
                        if (entry.Activity != null && entry.Activity == activity && entry.Created)
                        {
                            this._entries.Remove(entry);
                        }
                        this._entries.Add(new TimelineEntry(advertisement.Timestamp, activity, true));
                        break;
                    case AdvertisementFlags.Deleted:
                        foreach (TimelineEntry e in this._entries
                            .Where(_ => _.CompareToTimestamp(advertisement.Timestamp, activity.AccountId, activity.Name) <= 0 && _.Activity == activity)
                            .ToArray()
                        )
                        {
                            this._entries.Remove(e);
                        }
                        this._entries.Add(new TimelineEntry(advertisement.Timestamp, activity, false));
                        break;
                }
            }
        }
    }
}