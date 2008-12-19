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
using System.Threading;
using System.Runtime.Remoting.Messaging;
using System.Reflection;

namespace XSpect.MetaTweet
{
    public abstract class Proxy
        : Object
    {
        private Realm _parent;

        private String _name;

        private readonly List<Action<Proxy, StorageDataSetUnit, String[], IDictionary<String, String>>> _beforeFillHooks = new List<Action<Proxy, StorageDataSetUnit, String[], IDictionary<String, String>>>();

        private readonly List<Action<Proxy, StorageDataSetUnit, String[], IDictionary<String, String>>> _afterFillHooks = new List<Action<Proxy, StorageDataSetUnit, String[], IDictionary<String, String>>>();

        private List<IAsyncResult> _asyncResults = new List<IAsyncResult>();

        public Realm Parent
        {
            get
            {
                return this._parent;
            }
        }

        public String Name
        {
            get
            {
                return this._name;
            }
        }

        public IList<Action<Proxy, StorageDataSetUnit, String[], IDictionary<String, String>>> BeforeFillHooks
        {
            get
            {
                return this._beforeFillHooks;
            }
        }

        public IList<Action<Proxy, StorageDataSetUnit, String[], IDictionary<String, String>>> AfterFillHooks
        {
            get
            {
                return this._afterFillHooks;
            }
        }

        public void Register(Realm parent, String name)
        {
            if (this._parent != null || this._name != null)
            {
                throw new InvalidOperationException();
            }
            this._parent = parent;
            this._name = name;
        }

        public void Fill(StorageDataSetUnit datasets, String[] selector, IDictionary<String, String> arguments)
        {
            foreach (Action<Proxy, StorageDataSetUnit, String[], IDictionary<String, String>> hook in this._beforeFillHooks)
            {
                hook(this, datasets, selector, arguments);
            }

            this.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Single(m =>
                    m.GetCustomAttributes(typeof(ProxyInterfaceAttribute), true)
                        .Any(a => (a as ProxyInterfaceAttribute).Selector == selector)
                    && m.GetParameters().Select(p => p.ParameterType) == new Type[]
                    {
                        typeof(StorageDataSetUnit),
                        typeof(IDictionary<String, String>),
                    }
                ).Invoke(
                    this,
                    new Object[]
                    {
                        datasets,
                        arguments,
                    }
                );
            foreach (Action<Proxy, StorageDataSetUnit, String[], IDictionary<String, String>> hook in this._afterFillHooks)
            {
                hook(this, datasets, selector, arguments);
            }
        }

        public IAsyncResult BeginFill(
            StorageDataSetUnit datasets,
            String[] selector,
            IDictionary<String, String> arguments,
            AsyncCallback callback,
            Object state
        )
        {
            IAsyncResult asyncResult = new Action<StorageDataSetUnit, String[], IDictionary<String, String>>(this.Fill)
                .BeginInvoke(datasets, selector, arguments, callback, state);
            this._asyncResults.Add(asyncResult);
            return asyncResult;
        }

        public void EndFill(IAsyncResult asyncResult)
        {
            this._asyncResults.Remove(asyncResult);
            ((asyncResult as AsyncResult).AsyncDelegate as Action<StorageDataSetUnit, String[], IDictionary<String, String>>)
                .EndInvoke(asyncResult);
        }

        public StorageDataSetUnit GetData(String[] selector, IDictionary<String, String> arguments)
        {
            StorageDataSetUnit datasets = new StorageDataSetUnit();
            this.Fill(datasets, selector, arguments);
            return datasets;
        }

        public IAsyncResult BeginGetData(
            String[] selector,
            IDictionary<String, String> arguments,
            AsyncCallback callback,
            Object state
        )
        {
            IAsyncResult asyncResult = new Func<String[], IDictionary<String, String>, StorageDataSetUnit>(this.GetData).BeginInvoke(selector, arguments, callback, state);
            this._asyncResults.Add(asyncResult);
            return asyncResult;
        }

        public StorageDataSetUnit EndGetData(IAsyncResult asyncResult)
        {
            this._asyncResults.Remove(asyncResult);
            return ((asyncResult as AsyncResult).AsyncDelegate as Func<String[], IDictionary<String, String>, StorageDataSetUnit>).EndInvoke(asyncResult);
        }
    }
}
