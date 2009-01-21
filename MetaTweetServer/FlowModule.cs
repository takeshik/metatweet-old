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
        : Module
    {
        public new const String ModuleTypeString = "flow";
        
        private String _realm;

        public override String ModuleType
        {
            get
            {
                return ModuleTypeString;
            }
        }

        public String Realm
        {
            get
            {
                if (_realm == null)
                {
                    this._realm = String.Empty;
                }
                return this._realm;
            }
            set
            {
                if (value == null)
                {
                    value = String.Empty;
                }
                this._realm = value;
            }
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