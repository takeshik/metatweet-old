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
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace XSpect.MetaTweet.Objects
{
    [Serializable()]
    [DataContract()]
    public class Account
        : StorageObject,
          IComparable<Account>,
          IEquatable<Account>
    {
        private readonly Lazy<IDictionary<String, String>> _seeds;

        private ICollection<Activity> _activities;

        public override IStorageObjectId ObjectId
        {
            get
            {
                return this.Id;
            }
        }

        public override StorageSession Context
        {
            get
            {
                return base.Context;
            }
            set
            {
                if (this.Context != value)
                {
                    base.Context = value;
                    this.Activities = null;
                }
            }
        }

        [DataMember(Order = 0)]
        public virtual AccountId Id
        {
            get;
            protected set;
        }

        [DataMember(Order = 1)]
        public virtual String Realm
        {
            get;
            protected set;
        }

        [DataMember(Order = 2)]
        public virtual String Seed
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
                this.Id = new AccountId(value);
            }
        }

        public virtual ICollection<Activity> Activities
        {
            get
            {
                return this._activities ?? (this._activities = new HashSet<Activity>());
            }
            set
            {
                this._activities = value;
            }
        }

        public IEnumerable<Activity> this[String name]
        {
            get
            {
                return this.GetActivities(name, maxDepth: 0);
            }
        }

        public Activity this[String name, Object value]
        {
            get
            {
                return this.GetActivities(name, value, maxDepth: 0).SingleOrDefault();
            }
        }

        public IDictionary<String, String> Seeds
        {
            get
            {
                return this._seeds.Value;
            }
        }

        public static Boolean operator ==(Account left, Account right)
        {
            return Equals(left, right);
        }

        public static Boolean operator !=(Account left, Account right)
        {
            return !Equals(left, right);
        }

        public static Boolean operator <(Account left, Account right)
        {
            return Compare(left, right) < 0;
        }

        public static Boolean operator <=(Account left, Account right)
        {
            return Compare(left, right) <= 0;
        }

        public static Boolean operator >(Account left, Account right)
        {
            return Compare(left, right) > 0;
        }

        public static Boolean operator >=(Account left, Account right)
        {
            return Compare(left, right) >= 0;
        }

        public Account()
        {
            this._seeds = new Lazy<IDictionary<String, String>>(() => GetSeeds(this.Seed));
        }

        public static Boolean Equals(Account left, Account right)
        {
            // Adviced: http://twitter.com/haxe/status/31482020349607936
            // Use operator== instead of ReferenceEquals.
            // See: http://twitter.com/haxe/status/31482557447016448
            return (Object) left == (Object) right
                || ((Object) left != null && (Object) right != null && left.Id == right.Id);
        }

        public static Int32 Compare(Account left, Account right)
        {
            Int32 result;
            return left == right // Equals(left, right)
                ? 0
                : (Object) left == null // reference equals
                      ? -1
                      : (Object) right == null // reference equals
                            ? 1
                            : (result = left.Realm.CompareTo(right.Realm)) != 0
                                  ? result
                                  : left.Seed.CompareTo(right.Seed);
        }

        public static Int32 CompareById(Account left, Account right)
        {
            return left == right // Equals(left, right)
                ? 0
                : (Object) left == null // reference equals
                      ? -1
                      : (Object) right == null // reference equals
                            ? 1
                            : left.Id.CompareTo(right.Id);
        }

        public static IDictionary<String, String> GetSeeds(String seed)
        {
            return seed
                .Split(new Char[] { '!', }, StringSplitOptions.RemoveEmptyEntries)
                .OrderBy(s => s)
                .Select(s => s.Split('='))
                .ToDictionary(a => a[0], a => a[1]);
        }

        public static String GetSeed(IDictionary<String, String> seeds)
        {
            return String.Join(String.Empty, seeds.Select(p => "!" + p.Key + "=" + p.Value).OrderBy(s => s));
        }

        public static Account Create(String realm, String seed)
        {
            if (realm == null)
            {
                throw new ArgumentNullException("realm");
            }
            if (seed == null)
            {
                throw new ArgumentNullException("seed");
            }
            return new Account()
            {
                Id = AccountId.Create(realm, seed),
                Realm = realm,
                Seed = seed,
            };
        }

        public static Account Create(String realm, IDictionary<String, String> seeds)
        {
            return Create(realm, GetSeed(seeds));
        }

        public static Account Create(AccountCreationData data)
        {
            return Create(data.Realm, data.Seed);
        }

        public override Boolean Equals(Object obj)
        {
            return obj is Account && this.Equals((Account) obj);
        }

        public override Int32 GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override String ToString()
        {
            return String.Format(
                "Acc {0}: {1}@{2}",
                this.Id.ToString(true),
                this.Seed,
                this.Realm
            );
        }

        public Int32 CompareTo(Account other)
        {
            return Compare(this, other);
        }

        public Boolean Equals(Account other)
        {
            return Equals(this, other);
        }

        public Account Clone()
        {
            return new Account()
            {
                Id = this.Id,
                Realm = this.Realm,
                Seed = this.Seed,
            };
        }

        public AccountTuple ToTuple()
        {
            return new AccountTuple()
            {
                Id = this.Id,
                Realm = this.Realm,
                Seed = this.Seed,
            };
        }

        public ICollection<Activity> GetActivities(
            String name = null,
            Object value = null,
            Nullable<ActivityId> parentId = null,
            Nullable<Int32> maxDepth = null
        )
        {
            this.Load();
            IEnumerable<Activity> result = this.Activities;
            if (name != null)
            {
                result = result.Where(a => a.Name == name);
            }
            if (value != null)
            {
                result = result.Where(a => a.GetValue<Object>() == value);
            }
            if (parentId != null)
            {
                result = result.Where(a => a.AncestorIds.FirstOrDefault() == parentId);
            }
            if (maxDepth != null)
            {
                result = result.Where(a => a.Depth <= maxDepth);
            }
            return result.ToArray();
        }

        public Activity Lookup(String name, Nullable<DateTime> maxTimestamp = null)
        {
            return this.LookupInternal(name, maxTimestamp);
        }

        private Activity LookupInternal(String name, Nullable<DateTime> maxTimestamp = null)
        {
            this.Load();
            IEnumerable<Activity> result = this.Activities
                .Where(a => a.Name == name && a.LastFlags == AdvertisementFlags.Created);
            if (maxTimestamp != null)
            {
                result = result.Where(a => a.LastTimestamp <= maxTimestamp);
            }
            return result.OrderByDescending(a => a.LastTimestamp).FirstOrDefault();
        }

        public Activity Act(String name, Object value, params Expression<Action<Activity>>[] actions)
        {
            return (Activity) this.Context.Create(
                Activity.CreateCreationData(this.Id, null, name, value, actions)
            ).First();
        }

        public Activity Act(String name, Object value, DateTime timestamp, params Expression<Action<Activity>>[] actions)
        {
            return (Activity) this.Context.Create(
                Activity.CreateCreationData(this.Id, null, name, value, actions
                    .StartWith(a => a.Advertise(timestamp, AdvertisementFlags.Created))
                )
            ).First();
        }
    }
}