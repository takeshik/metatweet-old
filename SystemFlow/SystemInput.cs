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
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Xml.Linq;
using XSpect.Configuration;
using XSpect.Codecs;
using XSpect.Extension;
using XSpect.MetaTweet.Modules;
using XSpect.MetaTweet.Objects;
using XSpect.Net;
using Achiral;
using Achiral.Extension;

namespace XSpect.MetaTweet.Modules
{
    public class SystemInput
        : InputFlowModule
    {
        protected override String DefaultRealm
        {
            get
            {
                return String.Empty;
            }
        }

        #region Common

        [FlowInterface("/null", WriteTo = StorageObjectTypes.None)]
        public IEnumerable<StorageObject> NullInput(StorageModule storage, String param, IDictionary<String, String> args)
        {
            return Enumerable.Empty<StorageObject>();
        }

        #endregion

        #region StorageObject

        [FlowInterface("/accounts", WriteTo = StorageObjectTypes.None)]
        public IEnumerable<StorageObject> GetAccounts(StorageModule storage, String param, IDictionary<String, String> args)
        {
            IQueryable activities = storage.GetAccounts(
                args.GetValueOrDefault("accountId"),
                args.GetValueOrDefault("realm"),
                args.GetValueOrDefault("seedString")
            ).OrderByDescending(a => a).AsQueryable();
            if (args.ContainsKey("query"))
            {
                activities = activities.Execute(args["query"]);
            }
            return activities.Cast<StorageObject>();
        }

        [FlowInterface("/activities", WriteTo = StorageObjectTypes.None)]
        public IEnumerable<StorageObject> GetActivities(StorageModule storage, String param, IDictionary<String, String> args)
        {
            IQueryable activities = storage.GetActivities(
                args.GetValueOrDefault("accountId"),
                args.ContainsKey("timestamp") ? DateTime.Parse(args["timestamp"]) : default(Nullable<DateTime>),
                args.GetValueOrDefault("category"),
                args.GetValueOrDefault("subId"),
                args.GetValueOrDefault("userAgent"),
                args.ContainsKey("value")
                    ? args["value"].If(String.IsNullOrEmpty, s => DBNull.Value, s => (Object) s)
                    : null,
                args.ContainsKey("data")
                    ? args["data"].If(String.IsNullOrEmpty, s => DBNull.Value, s => (Object) s.Base64Decode().ToArray())
                    : null
            ).OrderByDescending(a => a).AsQueryable();
            if (args.ContainsKey("query"))
            {
                activities = activities.Execute(args["query"]);
            }
            return activities.Cast<StorageObject>();
        }

        [FlowInterface("/posts", WriteTo = StorageObjectTypes.None)]
        public IEnumerable<StorageObject> GetPosts(StorageModule storage, String param, IDictionary<String, String> args)
        {
            IQueryable posts = storage.GetActivities(
                args.GetValueOrDefault("accountId"),
                args.ContainsKey("timestamp") ? DateTime.Parse(args["timestamp"]) : default(Nullable<DateTime>),
                "Post",
                args.GetValueOrDefault("subId"),
                args.GetValueOrDefault("userAgent"),
                args.ContainsKey("value")
                    ? args["value"].If(String.IsNullOrEmpty, s => DBNull.Value, s => (Object) s)
                    : null,
                args.ContainsKey("data")
                    ? args["data"].If(String.IsNullOrEmpty, s => DBNull.Value, s => (Object) s.Base64Decode().ToArray())
                    : null
            ).OrderByDescending(p => p).AsQueryable();
            if (args.ContainsKey("query"))
            {
                posts = posts.Execute(args["query"]);
            }
            return posts.Cast<StorageObject>();
        }

        [FlowInterface("/annotations", WriteTo = StorageObjectTypes.None)]
        public IEnumerable<StorageObject> GetAnnotations(StorageModule storage, String param, IDictionary<String, String> args)
        {
            IQueryable annotations = storage.GetAnnotations(
                args.GetValueOrDefault("accountId"),
                args.GetValueOrDefault("name"),
                args.GetValueOrDefault("value")
            ).OrderByDescending(a => a).AsQueryable();
            if (args.ContainsKey("query"))
            {
                annotations = annotations.Execute(args["query"]);
            }
            return annotations.Cast<StorageObject>();
        }

        [FlowInterface("/relations", WriteTo = StorageObjectTypes.None)]
        public IEnumerable<StorageObject> GetRelations(StorageModule storage, String param, IDictionary<String, String> args)
        {
            IQueryable relations = storage.GetRelations(
                args.GetValueOrDefault("accountId"),
                args.GetValueOrDefault("name"),
                args.GetValueOrDefault("relatingAccountId")
            ).OrderByDescending(r => r).AsQueryable();
            if (args.ContainsKey("query"))
            {
                relations = relations.Execute(args["query"]);
            }
            return relations.Cast<StorageObject>();
        }

        [FlowInterface("/marks", WriteTo = StorageObjectTypes.None)]
        public IEnumerable<StorageObject> GetMarks(StorageModule storage, String param, IDictionary<String, String> args)
        {
            IQueryable marks = storage.GetMarks(
                args.GetValueOrDefault("accountId"),
                args.GetValueOrDefault("name"),
                args.GetValueOrDefault("markingAccountId"),
                args.ContainsKey("markingTimestamp") ? DateTime.Parse(args["markingTimestamp"]) : default(Nullable<DateTime>),
                args.GetValueOrDefault("markingCategory"),
                args.GetValueOrDefault("markingSubId")
            ).OrderByDescending(m => m).AsQueryable();
            if (args.ContainsKey("query"))
            {
                marks = marks.Execute(args["query"]);
            }
            return marks.Cast<StorageObject>();
        }

        [FlowInterface("/references", WriteTo = StorageObjectTypes.None)]
        public IEnumerable<StorageObject> GetReferences(StorageModule storage, String param, IDictionary<String, String> args)
        {
            IQueryable references = storage.GetReferences(
                args.GetValueOrDefault("accountId"),
                args.ContainsKey("timestamp") ? DateTime.Parse(args["timestamp"]) : default(Nullable<DateTime>),
                args.GetValueOrDefault("category"),
                args.GetValueOrDefault("subId"),
                args.GetValueOrDefault("name"),
                args.GetValueOrDefault("referringAccountId"),
                args.ContainsKey("referringTimestamp") ? DateTime.Parse(args["referringTimestamp"]) : default(Nullable<DateTime>),
                args.GetValueOrDefault("referringCategory"),
                args.GetValueOrDefault("referringSubId")
            ).OrderByDescending(r => r).AsQueryable();
            if (args.ContainsKey("query"))
            {
                references = references.Execute(args["query"]);
            }
            return references.Cast<StorageObject>();
        }

        [FlowInterface("/tags", WriteTo = StorageObjectTypes.None)]
        public IEnumerable<StorageObject> GetTags(StorageModule storage, String param, IDictionary<String, String> args)
        {
            IQueryable tags = storage.GetTags(
                args.GetValueOrDefault("accountId"),
                args.ContainsKey("timestamp") ? DateTime.Parse(args["timestamp"]) : default(Nullable<DateTime>),
                args.GetValueOrDefault("category"),
                args.GetValueOrDefault("subId"),
                args.GetValueOrDefault("name"),
                args.GetValueOrDefault("value")
            ).OrderByDescending(t => t).AsQueryable();
            if (args.ContainsKey("query"))
            {
                tags = tags.Execute(args["query"]);
            }
            return tags.Cast<StorageObject>();
        }

        #endregion

        #region RequestTask

        [FlowInterface("/reqmgr/tasks", WriteTo = StorageObjectTypes.None)]
        public IEnumerable<RequestTask> GetRequestTasks(StorageModule storage, String param, IDictionary<String, String> args)
        {
            IQueryable tasks = this.Host.RequestManager.AsQueryable();
            if (args.ContainsKey("query"))
            {
                tasks = tasks.Execute(args["query"]);
            }
            return tasks.Cast<RequestTask>().OrderByDescending(t => t.Id);
        }

        #endregion
    }
}