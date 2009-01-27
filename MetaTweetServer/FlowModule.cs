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
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace XSpect.MetaTweet
{
    public abstract class FlowModule
        : IModule
    {
        public const String ModuleTypeString = "flow";

        private ServerCore _host;

        private String _name;

        public ServerCore Host
        {
            get
            {
                return this._host;
            }
            set
            {
                if (this._host != null)
                {
                    throw new InvalidOperationException();
                }
                this._host = value;
            }
        }

        public String Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (this._name != null)
                {
                    throw new InvalidOperationException();
                }
                this._name = value;
            }
        }

        public virtual String ModuleType
        {
            get
            {
                return ModuleTypeString;
            }
        }

        public String Realm
        {
            get;
            set;
        }

        public virtual void Dispose()
        {
        }

        public virtual void Initialize(IDictionary<String, String> args)
        {
        }

        public MethodInfo GetMethod(String selector, out String parameter)
        {
            var target = this.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .SelectMany(
                    m => m.GetCustomAttributes(typeof(FlowInterfaceAttribute), true)
                        .Cast<FlowInterfaceAttribute>()
                        .Select(a => a.Selector),
                    (m, s) => new
                    {
                        Method = m,
                        Selector = s,
                    }
                )
                .Where(o => selector.StartsWith(o.Selector))
                .OrderByDescending(o => o.Selector.Length)
                .First();
            parameter = selector.Substring(target.Selector.Length);
            return target.Method;
        }
    }
}