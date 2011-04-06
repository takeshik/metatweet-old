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
using System.Runtime.Serialization;

namespace XSpect.MetaTweet.Objects
{
    [Serializable()]
    [DataContract()]
    public class Advertisement
        : StorageObject,
          IComparable<Advertisement>,
          IEquatable<Advertisement>
    {
        private Activity _activity;

        public override IStorageObjectId ObjectId
        {
            get
            {
                return this.Id;
            }
        }

        [DataMember(Order = 0)]
        public AdvertisementId Id
        {
            get;
            protected set;
        }

        [DataMember(Order = 1)]
        public ActivityId ActivityId
        {
            get;
            protected set;
        }

        [DataMember(Order = 2)]
        public DateTime Timestamp
        {
            get;
            protected set;
        }

        [DataMember(Order = 3)]
        public AdvertisementFlags Flags
        {
            get;
            protected set;
        }

        public String IdString
        {
            get
            {
                return this.Id.HexString;
            }
            protected set
            {
                this.Id = new AdvertisementId(value);
            }
        }

        public String ActivityIdString
        {
            get
            {
                return this.ActivityId.HexString;
            }
            protected set
            {
                this.ActivityId = new ActivityId(value);
            }
        }

        public Int32 FlagsValue
        {
            get
            {
                return (Int32) this.Flags;
            }
            protected set
            {
                this.Flags = (AdvertisementFlags) value;
            }
        }

        public Activity Activity
        {
            get
            {
                return this._activity ?? (this.Context != null
                    ? this._activity = this.Context.Load(this.ActivityId)
                    : null
                );
            }
            set
            {
                if (this.ActivityId != value.Id)
                {
                    throw new ArgumentException("value");
                }
                this._activity = value;
            }
        }

        public Advertisement()
        {
        }

        public static Boolean operator ==(Advertisement left, Advertisement right)
        {
            return Equals(left, right);
        }

        public static Boolean operator !=(Advertisement left, Advertisement right)
        {
            return !Equals(left, right);
        }

        public static Boolean operator <(Advertisement left, Advertisement right)
        {
            return Compare(left, right) < 0;
        }

        public static Boolean operator <=(Advertisement left, Advertisement right)
        {
            return Compare(left, right) <= 0;
        }

        public static Boolean operator >(Advertisement left, Advertisement right)
        {
            return Compare(left, right) > 0;
        }

        public static Boolean operator >=(Advertisement left, Advertisement right)
        {
            return Compare(left, right) >= 0;
        }

        public static Boolean Equals(Advertisement left, Advertisement right)
        {
            // Adviced: http://twitter.com/haxe/status/31482020349607936
            // Use operator== instead of ReferenceEquals.
            // See: http://twitter.com/haxe/status/31482557447016448
            return (Object) left == (Object) right
                || ((Object) left != null && (Object) right != null && left.Id == right.Id);
        }

        public static Int32 Compare(Advertisement left, Advertisement right)
        {
            Int32 result;
            return left == right // Equals(left, right)
                ? 0
                : (Object) left == null // reference equals
                      ? -1
                      : (Object) right == null // reference equals
                            ? 1
                            : (result = Activity.Compare(left.Activity, right.Activity)) != 0
                                  ? result
                                  : (result = left.Timestamp.CompareTo(right.Timestamp)) != 0
                                        ? result
                                        : left.Flags.CompareTo(right.Flags);
        }

        public static Int32 CompareById(Advertisement left, Advertisement right)
        {
            return left == right // Equals(left, right)
                ? 0
                : (Object) left == null // reference equals
                      ? -1
                      : (Object) right == null // reference equals
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

        public static Advertisement Create(AdvertisementCreationData data)
        {
            return Create(data.ActivityId, data.Timestamp, data.Flags);
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
            return String.Format(
                "Adv {0}: {1} @ {2} = {3}",
                this.Id.ToString(true),
                this.ActivityId.ToString(true),
                this.Timestamp.ToString("s"),
                this.Flags
            );
        }

        public Int32 CompareTo(Advertisement other)
        {
            return Compare(this, other);
        }

        public Boolean Equals(Advertisement other)
        {
            return Equals(this, other);
        }

        public Advertisement Clone()
        {
            return new Advertisement()
            {
                Id = this.Id,
                ActivityId = this.ActivityId,
                Timestamp = this.Timestamp,
                Flags = this.Flags,
            };
        }

        public AdvertisementTuple ToTuple()
        {
            return new AdvertisementTuple()
            {
                Id = this.Id,
                ActivityId = this.ActivityId,
                Timestamp = this.Timestamp,
                Flags = this.Flags,
            };
        }
    }
}