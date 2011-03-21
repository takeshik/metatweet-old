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
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace XSpect.MetaTweet.Objects
{
    [Serializable()]
    public class StorageObjectDynamicQuery<TObject, TTuple>
        : IStorageObjectQuery<TObject>
        where TObject : StorageObject
        where TTuple : StorageObjectTuple<TObject>
    {
        public TTuple ScalarMatch
        {
            get;
            set;
        }

        public String QueryExpression
        {
            get;
            set;
        }

        public String PostExpression
        {
            get;
            set;
        }

        public Object[] Values
        {
            get;
            set;
        }

        public override String ToString()
        {
            return String.Format(
                "Scalar: {1}{0}QueryExpression: {2}{0}PostExpression: {3}",
                Environment.NewLine,
                this.ScalarMatch,
                this.QueryExpression,
                this.PostExpression
            );
        }

        public virtual ICollection<TObject> Evaluate(IQueryable<TObject> source)
        {
            return this.CastResult(
                this.ExecutePostExpression(
                    this.ExecuteQueryExpression(
                        this.ExecuteScalarMatch(source)
                    )
                )
            );
        }

        protected IQueryable ExecuteScalarMatch(IQueryable<TObject> source)
        {
            return this.ScalarMatch != null
                ? source.Where(this.ScalarMatch.GetMatchExpression())
                : source;
        }

        protected IQueryable ExecuteQueryExpression(IQueryable source)
        {
            return !String.IsNullOrWhiteSpace(this.QueryExpression)
                ? TriDQL.ParseLambda<IQueryable, IQueryable>(this.QueryExpression, this.Values).Compile()(source)
                : source;
        }

        protected IQueryable ExecutePostExpression(IQueryable source)
        {
            return !String.IsNullOrWhiteSpace(this.PostExpression)
                ? TriDQL.ParseLambda<IQueryable, IQueryable>(this.PostExpression, this.Values).Compile()(
                      ((IEnumerable) source).Cast<TObject>().ToArray().AsQueryable()
                  )
                : source;
        }

        protected ICollection<TObject> CastResult(IQueryable source)
        {
            return ((IEnumerable<TObject>) source).ToArray();
        }
    }

    public static class StorageObjectDynamicQuery
    {
        public static StorageObjectDynamicQuery<Account, AccountTuple> Account(
            AccountTuple scalarMatch,
            String queryExpression = null,
            String postExpression = null,
            params Object[] values
        )
        {
            return new StorageObjectDynamicQuery<Account, AccountTuple>()
            {
                ScalarMatch = scalarMatch,
                QueryExpression = queryExpression,
                PostExpression = postExpression,
                Values = values,
            };
        }

        public static StorageObjectDynamicQuery<Account, AccountTuple> Account(String query)
        {
            if (String.IsNullOrWhiteSpace(query))
            {
                return null;
            }
            IDictionary<String, String> tokens = Tokenize(
                query,
                "id",
                "realm",
                "seed"
            );
            return Account(
                new AccountTuple()
                {
                    Id = GetValueOrDefault(tokens, "id"),
                    Realm = GetValueOrDefault(tokens, "realm"),
                    Seed = GetValueOrDefault(tokens, "seed"),
                },
                GetValueOrDefault(tokens, "expr"),
                GetValueOrDefault(tokens, "post")
            );
        }

        public static StorageObjectDynamicQuery<Activity, ActivityTuple> Activity(
            ActivityTuple scalarMatch,
            String queryExpression = null,
            String postExpression = null,
            params Object[] values
        )
        {
            return new StorageObjectDynamicQuery<Activity, ActivityTuple>()
            {
                ScalarMatch = scalarMatch,
                QueryExpression = queryExpression,
                PostExpression = postExpression,
                Values = values,
            };
        }

        public static StorageObjectDynamicQuery<Activity, ActivityTuple> Activity(String query)
        {
            if (String.IsNullOrWhiteSpace(query))
            {
                return null;
            }
            IDictionary<String, String> tokens = Tokenize(
                query,
                "id",
                "accountId",
                "ancestorIds",
                "name",
                "value"
            );
            return Activity(
                new ActivityTuple()
                {
                    Id = GetValueOrDefault(tokens, "id"),
                    AccountId = GetValueOrDefault(tokens, "accountId"),
                    AncestorIds = tokens.ContainsKey("ancestorIds")
                        ? tokens["ancestorIds"].Split(',')
                              .Select(s => (ActivityId) s)
                              .ToArray()
                        : null,
                    Name = GetValueOrDefault(tokens, "name"),
                    Value = tokens.ContainsKey("value")
                        ? JObject.FromObject(TriDQL.ParseLambda<Object>(tokens["value"]).Compile()())
                        : null,
                },
                GetValueOrDefault(tokens, "expr"),
                GetValueOrDefault(tokens, "post")
            );
        }

        public static StorageObjectDynamicQuery<Advertisement, AdvertisementTuple> Advertisement(
            AdvertisementTuple scalarMatch,
            String queryExpression = null,
            String postExpression = null,
            params Object[] values
        )
        {
            return new StorageObjectDynamicQuery<Advertisement, AdvertisementTuple>()
            {
                ScalarMatch = scalarMatch,
                QueryExpression = queryExpression,
                PostExpression = postExpression,
                Values = values,
            };
        }

        public static StorageObjectDynamicQuery<Advertisement, AdvertisementTuple> Advertisement(String query)
        {
            if (String.IsNullOrWhiteSpace(query))
            {
                return null;
            }
            IDictionary<String, String> tokens = Tokenize(
                query,
                "id",
                "activityId",
                "timestamp",
                "flags"
            );
            return Advertisement(
                new AdvertisementTuple()
                {
                    Id = GetValueOrDefault(tokens, "id"),
                    ActivityId = GetValueOrDefault(tokens, "activityId"),
                    Timestamp = tokens.ContainsKey("timestamp")
                        ? DateTime.Parse(
                              tokens["timestamp"],
                              CultureInfo.InvariantCulture,
                              DateTimeStyles.RoundtripKind
                          )
                        : default(DateTime),
                    Flags = tokens.ContainsKey("flags")
                        ? (AdvertisementFlags) Enum.Parse(typeof(AdvertisementFlags), tokens["flags"])
                        : default(AdvertisementFlags),
                },
                GetValueOrDefault(tokens, "expr"),
                GetValueOrDefault(tokens, "post")
            );
        }

        private static IDictionary<String, String> Tokenize(String query, params String[] additionalKeywords)
        {
            return Regex.Split(
                query.Replace(Environment.NewLine, " "),
                " (?=sql|expr|post|" + String.Join("|", additionalKeywords) + ")"
            )
                .Select(s => s.Split(new[] { ":", }, 2, StringSplitOptions.None))
                .Select(a => new KeyValuePair<String, String>(a[0], a[1].Trim()))
                .ToDictionary(p => p.Key, p => p.Value);
        }

        private static String GetValueOrDefault(IDictionary<String, String> dictionary, String key)
        {
            return dictionary.ContainsKey(key) ? dictionary[key] : null;
        }
    }
}