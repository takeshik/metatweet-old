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

namespace XSpect.MetaTweet.Objects
{
    [Serializable()]
    public class TimelineEntry
        : IComparable<TimelineEntry>,
          IEquatable<TimelineEntry>
    {
        public DateTime Timestamp
        {
            get;
            private set;
        }

        public Activity Activity
        {
            get;
            private set;
        }

        public Boolean Created
        {
            get;
            private set;
        }

        public TimelineEntry(DateTime timestamp, Activity activity, Boolean created)
        {
            this.Timestamp = timestamp;
            this.Activity = activity;
            this.Created = created;
        }

        public override Boolean Equals(Object obj)
        {
            return obj is TimelineEntry && this.Equals((TimelineEntry) obj);
        }

        public override Int32 GetHashCode()
        {
            return unchecked(
                this.Timestamp.GetHashCode() * 397 ^
                (this.Activity != null ? this.Activity.GetHashCode() : 0)
            );
        }

        public override String ToString()
        {
            return String.Format(
                "{0}{1} {2}",
                this.Timestamp.ToString("s"),
                this.Created ? ":" : "*",
                this.Activity != null
                    ? this.Activity.ToString()
                    : "(null)"
            );
        }

        public Int32 CompareTo(TimelineEntry other)
        {
            Int32 result;
            return (result = this.Timestamp.CompareTo(other.Timestamp)) != 0
                ? -result
                : this.Activity.CompareTo(other.Activity);
        }

        public Boolean Equals(TimelineEntry other)
        {
            return this.Timestamp == other.Timestamp
                && this.Activity == other.Activity;
        }

        public Nullable<Int32> CompareToTimestamp(
            Nullable<DateTime> timestamp = null,
            Nullable<AccountId> accountId = null,
            String name = null
        )
        {
            return (accountId == null || this.Activity.AccountId == accountId) && (name == null || this.Activity.Name == name)
                ? timestamp == null
                      ? 0
                      : this.Timestamp.CompareTo(timestamp)
                : default(Nullable<Int32>);
        }
    }
}