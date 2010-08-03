// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetServer.
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Achiral.Extension;
using log4net;
using XSpect.Collections;
using XSpect.Configuration;
using XSpect.Extension;
using XSpect.Hooking;

namespace XSpect.MetaTweet
{
    public class StoredRequestManager
        : MarshalByRefObject
    {
        public ServerCore Parent
        {
            get;
            private set;
        }

        public XmlConfiguration Configuration
        {
            get;
            private set;
        }

        public HybridDictionary<String, StoredRequest> StoredRequests
        {
            get;
            private set;
        }

        public StoredRequestManager(ServerCore parent, FileInfo configFile)
        {
            this.Parent = parent;
            this.Configuration = XmlConfiguration.Load(configFile);
            this.StoredRequests = new HybridDictionary<string, StoredRequest>((i, e) => e.Name);
            this.StoredRequests.AddRange(this.Configuration.ResolveValue<List<StoredRequest>>("storedRequests"));
        }

        public TOutput Execute<TOutput>(String name, IDictionary<String, String> args)
        {
            return this.Parent.RequestManager.Execute<TOutput>(this.StoredRequests[name].Apply(args));
        }

        public Object Execute(String name, IDictionary<String, String> args, Type outputType)
        {
            return this.Parent.RequestManager.Execute(this.StoredRequests[name].Apply(args), outputType);
        }

        public Object Execute(String name, IDictionary<String, String> args)
        {
            return this.Execute(name, args, null);
        }
    }
}