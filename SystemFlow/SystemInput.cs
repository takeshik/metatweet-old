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
using XSpect.Extension;
using XSpect.MetaTweet.Modules;
using XSpect.MetaTweet.Objects;
using XSpect.Net;

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

        [FlowInterface("/null")]
        public IEnumerable<StorageObject> NullInput(StorageModule storage, String param, IDictionary<String, String> args)
        {
            return Enumerable.Empty<StorageObject>();
        }

        [FlowInterface("/activities")]
        public IEnumerable<StorageObject> GetActivities(StorageModule storage, String param, IDictionary<String, String> args)
        {
            IQueryable activities = storage.GetActivities().OrderByDescending(a => a).AsQueryable();
            if (args.ContainsKey("where"))
            {
                activities = activities.Where(args["where"]);
            }
            if (args.ContainsKey("skip"))
            {
                activities = activities.Skip(args["skip"]);
            }
            if (args.ContainsKey("take"))
            {
                activities = activities.Take(args["take"]);
            }
            return activities.Cast<StorageObject>();
        }

        [FlowInterface("/posts")]
        public IEnumerable<StorageObject> GetPosts(StorageModule storage, String param, IDictionary<String, String> args)
        {
            IQueryable activities = storage.GetActivities(null, null, "Post", null).OrderByDescending(a => a).AsQueryable();
            if (args.ContainsKey("where"))
            {
                activities = activities.Where(args["where"]);
            }
            if (args.ContainsKey("skip"))
            {
                activities = activities.Skip(args["skip"]);
            }
            if (args.ContainsKey("take"))
            {
                activities = activities.Take(args["take"]);
            }

            return activities.Cast<StorageObject>();
        }
    }
}