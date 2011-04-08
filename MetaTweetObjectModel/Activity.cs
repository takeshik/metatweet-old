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
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace XSpect.MetaTweet.Objects
{
    [Serializable()]
    [DataContract()]
    public class Activity
        : StorageObject,
          IComparable<Activity>,
          IEquatable<Activity>
    {
        private Account _account;

        private ICollection<Advertisement> _advertisements;

        public override IStorageObjectId ObjectId
        {
            get
            {
                return this.Id;
            }
        }

        [DataMember(Order = 0)]
        public virtual ActivityId Id
        {
            get;
            protected set;
        }

        [DataMember(Order = 1)]
        public virtual AccountId AccountId
        {
            get;
            protected set;
        }

        [DataMember(Order = 2)]
        public virtual ReadOnlyCollection<ActivityId> AncestorIds
        {
            get;
            protected set;
        }

        [DataMember(Order = 3)]
        public virtual String Name
        {
            get;
            protected set;
        }

        [JsonProperty()]
        public virtual JObject Value
        {
            get;
            protected set;
        }

        [DataMember(Order = 5, EmitDefaultValue = false)]
        public virtual Nullable<DateTime> LastTimestamp
        {
            get;
            protected set;
        }

        [DataMember(Order = 6,  EmitDefaultValue = false)]
        public virtual Nullable<AdvertisementFlags> LastFlags
        {
            get;
            protected set;
        }

        public virtual String IdString
        {
            get
            {
                return this.Id.HexString;
            }
            protected set
            {
                this.Id = new ActivityId(value);
            }
        }

        public virtual String AccountIdString
        {
            get
            {
                return this.AccountId.HexString;
            }
            protected set
            {
                this.AccountId = new AccountId(value);
            }
        }

        public virtual String AncestorIdsString
        {
            get
            {
                return String.Concat(this.AncestorIds.Select(i => i.HexString).ToArray());
            }
            protected set
            {
                this.AncestorIds = new ReadOnlyCollection<ActivityId>(
                    Enumerable.Range(0, value.Length / ActivityId.HexStringLength)
                        .Select(i => new ActivityId(value.Substring(i * ActivityId.HexStringLength, ActivityId.HexStringLength)))
                        .ToArray()
                );
            }
        }

        [DataMember(Order = 4)]
        [JsonIgnore()]
        public virtual String ValueString
        {
            get
            {
                return this.Value.ToString(Formatting.None);
            }
            protected set
            {
                this.Value = JObject.Parse(value);
            }
        }

        public virtual Nullable<Int32> LastFlagsValue
        {
            get
            {
                return (Nullable<Int32>) this.LastFlags;
            }
            protected set
            {
                this.LastFlags = (Nullable<AdvertisementFlags>) value;
            }
        }

        public virtual Account Account
        {
            get
            {
                return this._account ?? (this.Context != null
                    ? this._account = this.Context.Load(this.AccountId)
                    : null
                );
            }
            set
            {
                if (this.AccountId != value.Id)
                {
                    throw new ArgumentException("value");
                }
                this._account = value;
            }
        }

        public virtual ICollection<Advertisement> Advertisements
        {
            get
            {
                return this._advertisements ?? (this._advertisements = new HashSet<Advertisement>());
            }
            set
            {
                this._advertisements = value;
            }
        }

        public ICollection<Activity> this[String name]
        {
            get
            {
                return this.Account.GetActivities(name, parentId: this.Id);
            }
        }

        public Activity this[String name, Object value]
        {
            get
            {
                return this.Account.GetActivities(name, value, this.Id).SingleOrDefault();
            }
        }

        public ICollection<ActivityId> SelfAndAncestorIds
        {
            get
            {
                return this.AncestorIds.StartWith(this.Id).ToArray();
            }
        }

        public ICollection<Activity> Ancestors
        {
            get
            {
                return this.Context.Load(this.AncestorIds);
            }
        }

        public ICollection<Activity> Children
        {
            get
            {
                return this.Account.GetActivities(parentId: this.Id, maxDepth: this.AncestorIds.Count + 1);
            }
        }

        public Activity()
        {
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
                            : (result = Account.Compare(left.Account, right.Account)) != 0
                                  ? result
                                  : (result = left.AncestorIds.Count.CompareTo(right.AncestorIds.Count)) != 0
                                        ? result
                                        : (result = left.Name.CompareTo(right.Name)) != 0
                                              ? result
                                              : left.ValueString.CompareTo(right.ValueString);
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
            JObject jvalue = CreateValue(value);
            return new Activity()
            {
                Id = ActivityId.Create(accountId, ancestorIds, name, jvalue),
                AccountId = accountId,
                AncestorIds = new ReadOnlyCollection<ActivityId>((ancestorIds ?? Enumerable.Empty<ActivityId>()).ToArray()),
                Name = name,
                Value = jvalue,
            };
        }

        public static Activity Create(ActivityCreationData data)
        {
            return Create(data.AccountId, data.AncestorIds, data.Name, data.Value);
        }

        public static JObject CreateValue(Object obj)
        {
            return obj is JObject
                ? (JObject) obj
                : new JObject(new JProperty("_",
                      obj is IStorageObjectId
                          ? ((IStorageObjectId) obj).HexString
                          : obj
                  ));
        }

        internal static IEnumerable<StorageObjectCreationData> CreateCreationData(
            AccountId accountId,
            IEnumerable<ActivityId> ancestorIds,
            String name,
            Object value,
            IEnumerable<Expression<Action<Activity>>> actions
        )
        {
            ActivityCreationData data = StorageObjectCreationData.Create(accountId, ancestorIds, name, value);
            return actions
                .Select(e => (MethodCallExpression) e.Body)
                .SelectMany(e =>
                {
                    if (e.Method.Name == "Act" && e.Method.GetParameters().Length == 3)
                    {
                        return CreateCreationData(
                            accountId,
                            data.SelfAndAncestorIds,
                            Expression.Lambda<Func<String>>(e.Arguments[0]).Compile()(),
                            Expression.Lambda<Func<Object>>(e.Arguments[1]).Compile()(),
                            ((NewArrayExpression) e.Arguments[2]).Expressions
                                .Select(_ => (Expression<Action<Activity>>) ((UnaryExpression) _).Operand)
                        );
                    }
                    else if (e.Method.Name == "Act" && e.Method.GetParameters().Length == 4)
                    {
                        return CreateCreationData(
                            accountId,
                            data.SelfAndAncestorIds,
                            Expression.Lambda<Func<String>>(e.Arguments[0]).Compile()(),
                            Expression.Lambda<Func<Object>>(e.Arguments[1]).Compile()(),
                            ((NewArrayExpression) e.Arguments[3]).Expressions
                                .Select(_ => (Expression<Action<Activity>>) ((UnaryExpression) _).Operand)
                        ).StartWith(StorageObjectCreationData.Create(
                            data.Id,
                            Expression.Lambda<Func<DateTime>>(e.Arguments[2]).Compile()(),
                            AdvertisementFlags.Created
                        ));
                    }
                    else if (e.Method.Name == "Advertise")
                    {
                        return EnumerableEx.Return(StorageObjectCreationData.Create(
                            data.Id,
                            Expression.Lambda<Func<DateTime>>(e.Arguments[0]).Compile()(),
                            Expression.Lambda<Func<AdvertisementFlags>>(e.Arguments[1]).Compile()()
                        ));
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                })
                .StartWith(data);
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
                this.Id.ToString(true),
                this.AccountId.ToString(true),
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

        public Activity Clone()
        {
            return new Activity()
            {
                Id = this.Id,
                AccountId = this.AccountId,
                AncestorIds = this.AncestorIds,
                Name = this.Name,
                ValueString = this.ValueString,
            };
        }

        public ActivityTuple ToTuple()
        {
            return new ActivityTuple()
            {
                Id = this.Id,
                AccountId = this.AccountId,
                AncestorIds = this.AncestorIds.ToArray(),
                Name = this.Name,
                Value = this.Value,
            };
        }

        public Object GetValue()
        {
            return this.Value["_"].Value<Object>();
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
                return (T) this.GetValue();
            }
        }

        public IEnumerable<Advertisement> GetAdvertisements(DateTime maxTimestamp)
        {
            this.Load();
            return this.Advertisements.Where(a => a.Timestamp <= maxTimestamp);
        }

        public Activity Act(String name, Object value, params Expression<Action<Activity>>[] actions)
        {
            return (Activity) this.Context.Create(
                CreateCreationData(this.AccountId, this.SelfAndAncestorIds, name, value, actions)
            ).First();
        }

        public Activity Act(String name, Object value, DateTime timestamp, params Expression<Action<Activity>>[] actions)
        {
            return (Activity) this.Context.Create(
                CreateCreationData(this.AccountId, this.SelfAndAncestorIds, name, value, actions
                    .StartWith(a => a.Advertise(timestamp, AdvertisementFlags.Created))
                )
            ).First();
        }

        public Advertisement Advertise(DateTime timestamp, AdvertisementFlags flags)
        {
            Advertisement advertisement = this.Context.Create(this, timestamp, flags);
            return advertisement;
        }

        public Boolean UpdateLastAdvertisement(Advertisement advertisement)
        {
            if (this.LastFlagsValue == null || this.LastTimestamp <= advertisement.Timestamp)
            {
                this.LastTimestamp = advertisement.Timestamp;
                this.LastFlags = advertisement.Flags;
                this.Context.Store(this);
                // TODO: this.Context.Create(advertisement.ActivityId, advertisement.Timestamp, advertisement.Flags); ?
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}