// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@xspect.org>
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
using System.Collections.Generic;

namespace XSpect.MetaTweet
{
    public abstract class Module
        : Object,
          IDisposable
    {
        private ServerCore _host;

        private String _name;

        public ServerCore Host
        {
            get
            {
                return this._host;
            }
        }

        public String Name
        {
            get
            {
                return this._name;
            }
        }

        public virtual String ModuleType
        {
            get
            {
                return "module";
            }
        }

        public void Register(ServerCore host, String name)
        {
            if (host == null || name == null)
            {
                // TODO: resource
                throw new InvalidOperationException();
            }
            this._host = host;
            this._name = name;
        }

        public void Unregister()
        {
            this._host = null;
            this._name = null;
        }

        public virtual void Initialize(IDictionary<String, String> args)
        {
        }

        public virtual void Dispose()
        {
        }
    }
}