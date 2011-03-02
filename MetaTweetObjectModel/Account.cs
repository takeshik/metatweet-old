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

        public override IStorageObjectId ObjectId
        {
            get
            {
                return this.Id;
            }
        }

        [DataMember()]
        public AccountId Id
        {
            get;
            private set;
        }

        [DataMember()]
        public String Realm
        {
            get;
            private set;
        }

        [DataMember()]
        public String Seed
        {
            get;
            private set;
        }

        public IEnumerable<Activity> this[String name]
        {
            get
            {
                return this.Context.GetActivities(
                    this.Id,
                    name,
                    maxDepth: 0
                );
            }
        }

        public Activity this[String name, Object value]
        {
            get
            {
                return this.Context.GetActivities(
                    this.Id,
                    name,
                    value,
                    null,
                    0
                ).SingleOrDefault();
            }
        }

        public IDictionary<String, String> Seeds
        {
            get
            {
                return this._seeds.Value;
            }
        }

        public IEnumerable<Activity> Activities
        {
            get
            {
                return this.Context.GetActivities(
                    this.Id
                );
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

        public IEnumerable<Activity> GetActivities(Int32 maxDepth)
        {
            return this.Context.GetActivities(
                this.Id,
                maxDepth: maxDepth
            );
        }

        public Activity LookupActivity(String name, DateTime maxTimestamp)
        {
            Activity result = this.Context.Parent.Timeline.Get(maxTimestamp, this.Id, name);
            if (result != null)
            {
                return result;
            }
            else
            {
                foreach (Activity a in this[name])
                {
                    a.GetAdvertisements(maxTimestamp);
                }
                return this.Context.Parent.Timeline.Get(maxTimestamp, this.Id, name);
            }
        }

        public Activity LookupActivity(String name)
        {
            return this.LookupActivity(name, DateTime.MaxValue);
        }

        public Activity Act(String name, Object value, params Action<Activity>[] actions)
        {
            Activity activity = this.Context.Create(this.Id, null, name, value);
            foreach (Action<Activity> action in actions)
            {
                action(activity);
            }
            return activity;
        }

        public Activity Act(String name, Object value, DateTime timestamp, params Action<Activity>[] actions)
        {
            return this.Act(name, value, new Action<Activity>[]
            {
                a => a.Advertise(timestamp, AdvertisementFlags.Created)
            }.Concat(actions).ToArray());
        }
    }
}