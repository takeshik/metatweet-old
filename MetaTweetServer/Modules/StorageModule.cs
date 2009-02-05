﻿// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
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
using XSpect.MetaTweet.ObjectModel;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace XSpect.MetaTweet.Modules
{
    public abstract class StorageModule
        : Storage,
          IModule
    {
        public ServerCore Host
        {
            get;
            private set;
        }

        public String Name
        {
            get;
            private set;
        }

        public IDictionary<String, String> Arguments
        {
            get;
            private set;
        }

        public virtual void Register(ServerCore host, String name)
        {
            this.Host = host;
            this.Name = name;
        }

        public virtual void Initialize(IDictionary<String, String> args)
        {
            this.Arguments = args;

            if (args.ContainsKey("connections"))
            {
                this.Initialize(args["connection"]);
            }
        }
    }
}