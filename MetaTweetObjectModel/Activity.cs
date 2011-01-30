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
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace XSpect.MetaTweet.Objects
{
    [DataContract()]
    public class Activity
        : StorageObject,
          IComparable<Activity>,
          IEquatable<Activity>
    {
        public override IStorageObjectId ObjectId
        {
            get
            {
                return this.Id;
            }
        }

        [DataMember()]
        public ActivityId Id
        {
            get;
            private set;
        }

        [DataMember()]
        public AccountId AccountId
        {
            get;
            private set;
        }

        [DataMember()]
        public ReadOnlyCollection<ActivityId> AncestorIds
        {
            get;
            private set;
        }

        [DataMember()]
        public String Name
        {
            get;
            private set;
        }

        [DataMember()]
        public JObject Value
        {
            get;
            private set;
        }

        public IEnumerable<Activity> this[String name]
        {
            get
            {
                return this.Context.Query(StorageObjectDynamicQuery.Activity(
                    new ActivityTuple()
                    {
                        AccountId = this.AccountId,
                        AncestorIds = new ActivityId[]
                        {
                            this.Id,
                        }.Concat(this.AncestorIds).ToArray(),
                        Name = name,
                    }
                ));
            }
        }

        public Activity this[String name, Object value]
        {
            get
            {
                return this.Context.Query(StorageObjectDynamicQuery.Activity(
                    new ActivityTuple()
                    {
                        AccountId = this.AccountId,
                        AncestorIds = new ActivityId[]
                        {
                            this.Id,
                        }.Concat(this.AncestorIds).ToArray(),
                        Name = name,
                        Value = value,
                    }
                )).SingleOrDefault();
            }
        }

        public Account Account
        {
            get
            {
                return this.Context.Load(this.AccountId);
            }
        }

        public IEnumerable<Activity> Ancestors
        {
            get
            {
                return this.Context.Load(this.AncestorIds.Cast<IStorageObjectId<Activity>>());
            }
        }

        public IEnumerable<Activity> Children
        {
            get
            {
                return this.Context.Query(StorageObjectQuery.Activity(
                    new ActivityTuple()
                    {
                        AccountId = this.AccountId,
                    },
                    _ => _.Where(a => a.AncestorIds[0] == this.Id)
                ));
            }
        }

        public IEnumerable<Advertisement> Advertisements
        {
            get
            {
                return this.Context.Query(StorageObjectDynamicQuery.Advertisement(
                    new AdvertisementTuple()
                    {
                        ActivityId = this.Id,
                    }
                )).OrderByDescending(a => a);
            }
        }

        public Nullable<DateTime> EstimatedTimestamp
        {
            get
            {
                Advertisement advertisement = this.Advertisements.OrderByDescending(a => a)
                    .FirstOrDefault();
                return advertisement != null && advertisement.Flags == AdvertisementFlags.Created
                    ? advertisement.Timestamp
                    : default(Nullable<DateTime>);
            }
        }

        public static Boolean operator ==(Activity left, Activity right)
        {
            return Equals(left, right);
        }

        public static Boolean operator !=(Activity left, Activity right)
        {
            return !Equals(left, right);
        }

        public static Boolean operator <(Activity left, Activity right)
        {
            return Compare(left, right) < 0;
        }

        public static Boolean operator <=(Activity left, Activity right)
        {
            return Compare(left, right) <= 0;
        }

        public static Boolean operator >(Activity left, Activity right)
        {
            return Compare(left, right) > 0;
        }

        public static Boolean operator >=(Activity left, Activity right)
        {
            return Compare(left, right) >= 0;
        }

        public static Boolean Equals(Activity left, Activity right)
        {
            // Adviced: http://twitter.com/haxe/status/31482020349607936
            // Use operator== instead of ReferenceEquals.
            // See: http://twitter.com/haxe/status/31482557447016448
            return (Object) left == (Object) right
                || ((Object) left != null && (Object) right != null && left.Id == right.Id);
        }

        public static Int32 Compare(Activity left, Activity right)
        {
            Int32 result;
            return left == right // Equals(left, right)
                ? 0
                : (Object) left == null // reference equals
                      ? -1
                      : (Object) right == null // reference equals
                            ? 1
                            : (result = left.Account.CompareTo(right.Account)) != 0
                                  ? result
                                  : (result = left.AncestorIds.Count.CompareTo(right.AncestorIds.Count)) != 0
                                        ? result
                                        : (result = left.Name.CompareTo(right.Name)) != 0
                                              ? result
                                              : left.Value.ToString(Formatting.None).CompareTo(right.Value.ToString(Formatting.None));
        }

        public static Int32 CompareById(Activity left, Activity right)
        {
            return left == right // Equals(left, right)
                ? 0
                : (Object) left == null // reference equals
                      ? -1
                      : (Object) right == null // reference equals
                            ? 1
                            : left.Id.CompareTo(right.Id);
        }

        public static Activity Create(AccountId accountId, IEnumerable<ActivityId> ancestorIds, String name, Object value)
        {
            if (accountId == default(AccountId))
            {
                throw new ArgumentException("accountId");
            }
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            JObject jvalue = value is JObject
                    ? (JObject) value
                    : new JObject(new JProperty("_", value));
            return new Activity()
            {
                Id = ActivityId.Create(accountId, ancestorIds, name, jvalue),
                AccountId = accountId,
                AncestorIds = new ReadOnlyCollection<ActivityId>((ancestorIds ?? Enumerable.Empty<ActivityId>()).ToArray()),
                Name = name,
                Value = jvalue,
            };
        }

        public override Boolean Equals(Object obj)
        {
            return obj is Activity && this.Equals((Activity) obj);
        }

        public override Int32 GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override String ToString()
        {
            return String.Format(
                "Act {0}: {1} ({2}) {3} = {4}",
                this.Id,
                this.AccountId,
                this.AncestorIds.Count,
                this.Name,
                this.Value.ToString(Formatting.None)
            );
        }

        public Int32 CompareTo(Activity other)
        {
            return Compare(this, other);
        }

        public Boolean Equals(Activity other)
        {
            return Equals(this, other);
        }

        public JToken GetValue()
        {
            return this.Value["_"];
        }

        public T GetValue<T>()
        {
            if (typeof(T) == typeof(Account))
            {
                return (T) ((Object) this.Context.Load(this.GetValue<AccountId>()));
            }
            else if (typeof(T) == typeof(AccountId))
            {
                return (T) ((Object) new AccountId(this.GetValue<String>()));
            }
            else if (typeof(T) == typeof(Activity))
            {
                return (T) ((Object) this.Context.Load(this.GetValue<ActivityId>()));
            }
            else if (typeof(T) == typeof(ActivityId))
            {
                return (T) ((Object) new ActivityId(this.GetValue<String>()));
            }
            else if (typeof(T) == typeof(Advertisement))
            {
                return (T) ((Object) this.Context.Load(this.GetValue<AdvertisementId>()));
            }
            else if (typeof(T) == typeof(AdvertisementId))
            {
                return (T) ((Object) new AdvertisementId(this.GetValue<String>()));
            }
            else
            {
                return this.GetValue().Value<T>();
            }
        }

        public IEnumerable<Advertisement> GetAdvertisements(DateTime maxTimestamp)
        {
            return this.Advertisements
                .Where(a => a.Timestamp <= maxTimestamp);
        }

        public Activity Act(String name, Object value)
        {
            return this.Context.Create(
                this.AccountId,
                this.AncestorIds.Concat(new ActivityId[] { this.Id, }),
                name,
                value
            );
        }

        public Advertisement Advertise(DateTime timestamp, AdvertisementFlags flags)
        {
            return this.Context.Create(this.Id, timestamp, flags);
        }
    }
}