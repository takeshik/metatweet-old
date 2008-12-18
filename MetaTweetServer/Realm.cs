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

        private readonly List<Action<Realm, String, Proxy>> _beforeAddProxyHooks = new List<Action<Realm, String, Proxy>>();

        private readonly List<Action<Realm, String, Proxy>> _afterAddProxyHooks = new List<Action<Realm, String, Proxy>>();

        private readonly List<Action<Realm, String>> _beforeRemoveProxyHooks = new List<Action<Realm, String>>();
        
        private readonly List<Action<Realm, String>> _afterRemoveProxyHooks = new List<Action<Realm, String>>();
        
        private readonly List<Action<Realm, String, Converter>> _beforeAddConverterHooks = new List<Action<Realm, String, Converter>>();
        
        private readonly List<Action<Realm, String, Converter>> _afterAddConverterHooks = new List<Action<Realm, String, Converter>>();
        
        private readonly List<Action<Realm, String>> _beforeRemoveConverterHooks = new List<Action<Realm, String>>();
        
        private readonly List<Action<Realm, String>> _afterRemoveConverterHooks = new List<Action<Realm, String>>();

        public IList<Action<Realm, String, Proxy>> BeforeAddProxyHooks
        {
            get
            {
                return this._beforeAddProxyHooks;
            }
        }

        public IList<Action<Realm, String, Proxy>> AfterAddProxyHooks
        {
            get
            {
                return this._afterAddProxyHooks;
            }
        }

        public IList<Action<Realm, String>> BeforeRemoveProxyHooks
        {
            get
            {
                return this._beforeRemoveProxyHooks;
            }
        }

        public IList<Action<Realm, String>> AfterRemoveProxyHooks
        {
            get
            {
                return this._afterRemoveProxyHooks;
            }
        }

        public IList<Action<Realm, String, Converter>> BeforeAddConverterHooks
        {
            get
            {
                return this._beforeAddConverterHooks;
            }
        }

        public IList<Action<Realm, String, Converter>> AfterAddConverterHooks
        {
            get
            {
                return this._afterAddConverterHooks;
            }
        }

        public IList<Action<Realm, String>> BeforeRemoveConverterHooks
        {
            get
            {
                return this._beforeRemoveConverterHooks;
            }
        }

        public IList<Action<Realm, String>> AfterRemoveConverterHooks
        {
            get
            {
                return this._afterRemoveConverterHooks;
            }
        } 

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

        public void AddProxy(String id, Proxy proxy)
        {
            foreach (Action<Realm, String, Proxy> hook in this._beforeAddProxyHooks)
            {
                hook(this, id, proxy);
            }
            proxy.Register(this, id);
            this._proxies.Add(id, proxy);
            this.Parent.Log.InfoFormat(
                Resources.RealmProxyAdded,
                this._name,
                id,
                proxy.GetType().AssemblyQualifiedName,
                proxy.GetType().Assembly.CodeBase
            );
            foreach (Action<Realm, String, Proxy> hook in this._afterAddProxyHooks)
            {
                hook(this, id, proxy);
            }
        }

        public void RemoveProxy(String id)
        {
            foreach (Action<Realm, String> hook in this._beforeRemoveProxyHooks)
            {
                hook(this, id);
            }
            this._proxies.Remove(id);
            this._parent.Log.InfoFormat(
                Resources.RealmProxyRemoved,
                this._name,
                id
            );
            foreach (Action<Realm, String> hook in this._afterRemoveProxyHooks)
            {
                hook(this, id);
            }
        }

        public void AddConverter(String extension, Converter converter)
        {
            foreach (Action<Realm, String, Converter> hook in this._beforeAddConverterHooks)
            {
                hook(this, extension, converter);
            }
            converter.Register(this, extension);
            this._converters.Add(extension, converter);
            this.Parent.Log.InfoFormat(
                Resources.RealmConverterAdded,
                this._name,
                extension,
                converter.GetType().AssemblyQualifiedName,
                converter.GetType().Assembly.CodeBase
            );
            foreach (Action<Realm, String, Converter> hook in this._afterAddConverterHooks)
            {
                hook(this, extension, converter);
            }
        }

        public void RemoveConverter(String extension)
        {
            foreach (Action<Realm, String> hook in this._beforeRemoveConverterHooks)
            {
                hook(this, extension);
            }
            this._converters.Remove(extension);
            this._parent.Log.InfoFormat(
                Resources.RealmConverterRemoved,
                this._name,
                extension
            );
            foreach (Action<Realm, String> hook in this._afterRemoveConverterHooks)
            {
                hook(this, extension);
            }
        }
    }
}