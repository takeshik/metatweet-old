// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * SystemFlow
 *   MetaTweet Input/Output modules which provides generic system instructions
 *   Part of MetaTweet
 * Copyright © 2008-2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of TwitterApiFlow.
 * 
 * This program is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation; either version 3 of the License, or (at your
 * option) any later version.
 * 
 * This program is distributed in the hope that it will be useful, but
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
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using XSpect.Codecs;
using XSpect.Extension;
using Achiral;
using Achiral.Extension;

namespace XSpect.MetaTweet.Objects
{
    public static class StorageObjectQueryParser
    {
        public static StorageObjectEntityQuery<Account, AccountTuple> Account(String query)
        {
            if (String.IsNullOrWhiteSpace(query))
            {
                return null;
            }
            IDictionary<String, String> tokens = Tokenize(
                query,
                "accountId",
                "realm",
                "seedString"
            );
            return StorageObjectEntityQuery.Account(
                tokens.GetValueOrDefault("sql"),
                new AccountTuple()
                {
                    AccountId = tokens.GetValueOrDefault("accountId"),
                    Realm = tokens.GetValueOrDefault("realm"),
                    SeedString = tokens.GetValueOrDefault("seedString"),
                },
                ExpressionGenerator.Execute<Account>(tokens.GetValueOrDefault("expr")),
                ExpressionGenerator.Execute<Account>(tokens.GetValueOrDefault("post"))
            );
        }

        public static StorageObjectEntityQuery<Activity, ActivityTuple> Activity(String query)
        {
            if (String.IsNullOrWhiteSpace(query))
            {
                return null;
            }
            IDictionary<String, String> tokens = Tokenize(
                query,
                "accountId",
                "timestamp",
                "category",
                "subId",
                "userAgent",
                "value",
                "data"
            );
            return StorageObjectEntityQuery.Activity(
                tokens.GetValueOrDefault("sql"),
                new ActivityTuple()
                {
                    AccountId = tokens.GetValueOrDefault("accountId"),
                    Timestamp = tokens.ContainsKey("timestamp")
                        ? DateTime.Parse(
                              tokens["timestamp"],
                              CultureInfo.InvariantCulture,
                              DateTimeStyles.RoundtripKind
                          )
                        : default(Nullable<DateTime>),
                    Category = tokens.GetValueOrDefault("category"),
                    SubId = tokens.GetValueOrDefault("subId"),
                    UserAgent = tokens.GetValueOrDefault("userAgent"),
                    Value = tokens.GetValueOrDefault("value"),
                    Data = tokens.ContainsKey("data")
                        ? tokens["data"] == "empty"
                              ? (Object) DBNull.Value
                              : Base64Codec.Decode(tokens["data"])
                        : null,
                },
                ExpressionGenerator.Execute<Activity>(tokens.GetValueOrDefault("expr")),
                ExpressionGenerator.Execute<Activity>(tokens.GetValueOrDefault("post"))
            );
        }

        public static StorageObjectEntityQuery<Annotation, AnnotationTuple> Annotation(String query)
        {
            if (String.IsNullOrWhiteSpace(query))
            {
                return null;
            }
            IDictionary<String, String> tokens = Tokenize(
                query,
                "accountId",
                "name",
                "value"
            );
            return StorageObjectEntityQuery.Annotation(
                tokens.GetValueOrDefault("sql"),
                new AnnotationTuple()
                {
                    AccountId = tokens.GetValueOrDefault("accountId"),
                    Name = tokens.GetValueOrDefault("name"),
                    Value = tokens.GetValueOrDefault("value"),
                },
                ExpressionGenerator.Execute<Annotation>(tokens.GetValueOrDefault("expr")),
                ExpressionGenerator.Execute<Annotation>(tokens.GetValueOrDefault("post"))
            );
        }

        public static StorageObjectEntityQuery<Relation, RelationTuple> Relation(String query)
        {
            if (String.IsNullOrWhiteSpace(query))
            {
                return null;
            }
            IDictionary<String, String> tokens = Tokenize(
                query,
                "accountId",
                "name",
                "relatingAccountId"
            );
            return StorageObjectEntityQuery.Relation(
                tokens.GetValueOrDefault("sql"),
                new RelationTuple()
                {
                    AccountId = tokens.GetValueOrDefault("accountId"),
                    Name = tokens.GetValueOrDefault("name"),
                    RelatingAccountId = tokens.GetValueOrDefault("relatingAccountId"),
                },
                ExpressionGenerator.Execute<Relation>(tokens.GetValueOrDefault("expr")),
                ExpressionGenerator.Execute<Relation>(tokens.GetValueOrDefault("post"))
            );
        }

        public static StorageObjectEntityQuery<Mark, MarkTuple> Mark(String query)
        {
            if (String.IsNullOrWhiteSpace(query))
            {
                return null;
            }
            IDictionary<String, String> tokens = Tokenize(
                query,
                "accountId",
                "name",
                "markingAccountId",
                "markingTimestamp",
                "markingCategory",
                "markingSubId"
            );
            return StorageObjectEntityQuery.Mark(
                tokens.GetValueOrDefault("sql"),
                new MarkTuple()
                {
                    AccountId = tokens.GetValueOrDefault("accountId"),
                    Name = tokens.GetValueOrDefault("name"),
                    MarkingAccountId = tokens.GetValueOrDefault("markingAccountId"),
                    MarkingTimestamp = tokens.ContainsKey("markingTimestamp")
                        ? DateTime.Parse(
                              tokens["markingTimestamp"],
                              CultureInfo.InvariantCulture,
                              DateTimeStyles.RoundtripKind
                          )
                        : default(Nullable<DateTime>),
                    MarkingCategory = tokens.GetValueOrDefault("markingCategory"),
                    MarkingSubId = tokens.GetValueOrDefault("markingSubId"),
                },
                ExpressionGenerator.Execute<Mark>(tokens.GetValueOrDefault("expr")),
                ExpressionGenerator.Execute<Mark>(tokens.GetValueOrDefault("post"))
            );
        }

        public static StorageObjectEntityQuery<Reference, ReferenceTuple> Reference(String query)
        {
            if (String.IsNullOrWhiteSpace(query))
            {
                return null;
            }
            IDictionary<String, String> tokens = Tokenize(
                query,
                "accountId",
                "timestamp",
                "category",
                "subId",
                "name",
                "referringAccountId",
                "referringTimestamp",
                "referringCategory",
                "referringSubId"
            );
            return StorageObjectEntityQuery.Reference(
                tokens.GetValueOrDefault("sql"),
                new ReferenceTuple()
                {
                    AccountId = tokens.GetValueOrDefault("accountId"),
                    Timestamp = tokens.ContainsKey("timestamp")
                        ? DateTime.Parse(
                              tokens["timestamp"],
                              CultureInfo.InvariantCulture,
                              DateTimeStyles.RoundtripKind
                          )
                        : default(Nullable<DateTime>),
                    Category = tokens.GetValueOrDefault("category"),
                    SubId = tokens.GetValueOrDefault("subId"),
                    Name = tokens.GetValueOrDefault("name"),
                    ReferringAccountId = tokens.GetValueOrDefault("referringAccountId"),
                    ReferringTimestamp = tokens.ContainsKey("referringTimestamp")
                        ? DateTime.Parse(
                              tokens["referringTimestamp"],
                              CultureInfo.InvariantCulture,
                              DateTimeStyles.RoundtripKind
                          )
                        : default(Nullable<DateTime>),
                    ReferringCategory = tokens.GetValueOrDefault("referringCategory"),
                    ReferringSubId = tokens.GetValueOrDefault("referringSubId"),
                },
                ExpressionGenerator.Execute<Reference>(tokens.GetValueOrDefault("expr")),
                ExpressionGenerator.Execute<Reference>(tokens.GetValueOrDefault("post"))
            );
        }

        public static StorageObjectEntityQuery<Tag, TagTuple> Tag(String query)
        {
            if (String.IsNullOrWhiteSpace(query))
            {
                return null;
            }
            IDictionary<String, String> tokens = Tokenize(
                query,
                "accountId",
                "timestamp",
                "category",
                "subId",
                "name",
                "value"
            );
            return StorageObjectEntityQuery.Tag(
                tokens.GetValueOrDefault("sql"),
                new TagTuple()
                {
                    AccountId = tokens.GetValueOrDefault("accountId"),
                    Timestamp = tokens.ContainsKey("timestamp")
                        ? DateTime.Parse(
                              tokens["timestamp"],
                              CultureInfo.InvariantCulture,
                              DateTimeStyles.RoundtripKind
                          )
                        : default(Nullable<DateTime>),
                    Category = tokens.GetValueOrDefault("category"),
                    SubId = tokens.GetValueOrDefault("subId"),
                    Name = tokens.GetValueOrDefault("name"),
                    Value = tokens.GetValueOrDefault("value"),
                },
                ExpressionGenerator.Execute<Tag>(tokens.GetValueOrDefault("expr")),
                ExpressionGenerator.Execute<Tag>(tokens.GetValueOrDefault("post"))
            );
        }

        private static IDictionary<String, String> Tokenize(String query, params String[] additionalKeywords)
        {
            return Regex.Split(
                query.Replace(Environment.NewLine, " "),
                " (?=sql|expr|post|" + additionalKeywords.Join("|") + ")"
            )
                .Select(s => s.Split(new[] { ":", }, 2, StringSplitOptions.None))
                .Select(a => new KeyValuePair<String, String>(a[0], a[1].Trim()))
                .ToDictionary();
        }
    }
}
