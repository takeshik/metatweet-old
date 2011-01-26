﻿// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
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
using System.Runtime.Serialization;

namespace XSpect.MetaTweet.Objects
{
    [DataContract()]
    public class Advertisement
        : StorageObject,
          IComparable<Advertisement>,
          IEquatable<Advertisement>
    {
        public override IStorageObjectId ObjectId
        {
            get
            {
                return this.Id;
            }
        }

        [DataMember()]
        public AdvertisementId Id
        {
            get;
            private set;
        }

        [DataMember()]
        public ActivityId ActivityId
        {
            get;
            private set;
        }

        [DataMember()]
        public DateTime Timestamp
        {
            get;
            private set;
        }

        [DataMember()]
        public AdvertisementFlags Flags
        {
            get;
            private set;
        }

        public Activity Activity
        {
            get
            {
                return this.Context.Load(this.ActivityId);
            }
        }

        public static Boolean operator ==(Advertisement left, Advertisement right)
        {
            return left.Equals(right);
        }

        public static Boolean operator !=(Advertisement left, Advertisement right)
        {
            return !left.Equals(right);
        }

        public static Boolean operator <(Advertisement left, Advertisement right)
        {
            return left.CompareTo(right) < 0;
        }

        public static Boolean operator <=(Advertisement left, Advertisement right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static Boolean operator >(Advertisement left, Advertisement right)
        {
            return left.CompareTo(right) > 0;
        }

        public static Boolean operator >=(Advertisement left, Advertisement right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static Boolean Equals(Advertisement left, Advertisement right)
        {
            return ReferenceEquals(left, right)
                || (ReferenceEquals(left, null) && ReferenceEquals(right, null) && left.Id.Equals(right.Id));
        }

        public static Int32 Compare(Advertisement left, Advertisement right)
        {
            Int32 result;
            return left == right
                ? 0
                : left == null
                      ? -1
                      : right == null
                            ? 1
                            : (result = left.Activity.CompareTo(right.Activity)) != 0
                                  ? result
                                  : (result = left.Timestamp.CompareTo(right.Timestamp)) != 0
                                        ? result
                                        : left.Flags.CompareTo(right.Flags);
        }

        public static Int32 CompareById(Advertisement left, Advertisement right)
        {
            return left == right
                ? 0
                : left == null
                      ? -1
                      : right == null
                            ? 1
                            : left.Id.CompareTo(right.Id);
        }

        public static Advertisement Create(ActivityId activityId, DateTime timestamp, AdvertisementFlags flags)
        {
            if (activityId == default(ActivityId))
            {
                throw new ArgumentException("activityId");
            }
            if (timestamp == default(DateTime))
            {
                throw new ArgumentException("timestamp");
            }
            if (flags == default(AdvertisementFlags))
            {
                throw new ArgumentException("flags");
            }
            switch (timestamp.Kind)
            {
                case DateTimeKind.Unspecified:
                    timestamp = DateTime.SpecifyKind(timestamp, DateTimeKind.Utc);
                    break;
                case DateTimeKind.Local:
                    timestamp = timestamp.ToUniversalTime();
                    break;
            }
            return new Advertisement()
            {
                Id = AdvertisementId.Create(activityId, timestamp, flags),
                ActivityId = activityId,
                Timestamp = timestamp,
                Flags = flags,
            };
        }

        public override Boolean Equals(Object obj)
        {
            return obj is Advertisement && this.Equals((Advertisement) obj);
        }

        public override Int32 GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override String ToString()
        {
            return String.Format("Adv {0}: {1} @ {2} = {3}", this.Id, this.ActivityId, this.Timestamp.ToString("s"), this.Flags);
        }

        public Int32 CompareTo(Advertisement other)
        {
            return Compare(this, other);
        }

        public Boolean Equals(Advertisement other)
        {
            return Equals(this, other);
        }
    }
}