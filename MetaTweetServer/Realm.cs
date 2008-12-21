// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008 Takeshi KIRIYA, XSpect Project <takeshik@xspect.org>
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
using System.Linq;
using System.Text;
using log4net;
using System.Reflection;
using XSpect.Reflection;
using System.Threading;
using XSpect.MetaTweet.Properties;

namespace XSpect.MetaTweet
{
    public class Realm
        : Object
    {
        private readonly ServerCore _parent;

        private readonly String _name;

        private readonly Hook<Realm, String, Proxy> _addProxyHook = new Hook<Realm, String, Proxy>();

        private readonly Hook<Realm, String> _removeProxyHook = new Hook<Realm, String>();

        private readonly Hook<Realm, String, Converter> _addConverterHook = new Hook<Realm, String, Converter>();
        
        private readonly Hook<Realm, String> _removeConverterHook = new Hook<Realm, String>();
        
        private readonly Dictionary<String, Proxy> _proxies = new Dictionary<String, Proxy>();

        private readonly Dictionary<String, Converter> _converters = new Dictionary<String, Converter>();

        public String Name
        {
            get
            {
                return this._name;
            }
        }

        public ServerCore Parent
        {
            get
            {
                return this._parent;
            }
        }

        public Hook<Realm, String, Proxy> AddProxyHook
        {
            get
            {
                return _addProxyHook;
            }
        }

        public Hook<Realm, String> RemoveProxyHook
        {
            get
            {
                return _removeProxyHook;
            }
        }

        public Hook<Realm, String, Converter> AddConverterHook
        {
            get
            {
                return _addConverterHook;
            }
        }

        public Hook<Realm, String> RemoveConverterHook
        {
            get
            {
                return _removeConverterHook;
            }
        } 

        public IEnumerable<KeyValuePair<String, Proxy>> Proxies
        {
            get
            {
                return this._proxies;
            }
        }

        public IEnumerable<KeyValuePair<String, Converter>> Converters
        {
            get
            {
                return this._converters;
            }
        }

        public Realm(ServerCore parent, String name)
        {
            this._parent = parent;
            this._name = name;
        }

        public void AddProxy(String key, Proxy proxy)
        {
            this._addProxyHook.Execute((self, k, p) =>
            {
                proxy.Register(self, k);
                self._proxies.Add(k, p);
            }, this, key, proxy);
        }

        public void RemoveProxy(String key)
        {
            this._removeProxyHook.Execute((self, k) =>
            {
                self._proxies.Remove(k);
            }, this, key);
        }

        public void AddConverter(String extension, Converter converter)
        {
            this._addConverterHook.Execute((self, e, c) =>
            {
                converter.Register(self, e);
                self._converters.Add(e, c);
            }, this, extension, converter);
        }

        public void RemoveConverter(String extension)
        {
            this._removeConverterHook.Execute((self, e) =>
            {
                self._converters.Remove(e);
            }, this, extension);
        }
    }
}