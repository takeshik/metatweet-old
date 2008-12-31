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
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using System.Reflection;
using XSpect.MetaTweet.ObjectModel;

namespace XSpect.MetaTweet
{
    public abstract class Proxy
        : Object
    {
        private Realm _parent;

        private String _name;

        private readonly Hook<Proxy, IEnumerable<StorageObject>, String, IDictionary<String, String>> _fillHook = new Hook<Proxy, IEnumerable<StorageObject>, string, IDictionary<String, String>>();

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

        public Hook<Proxy, IEnumerable<StorageObject>, String, IDictionary<String, String>> FillHook
        {
            get
            {
                return this._fillHook;
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

        public void Fill(IEnumerable<StorageObject> objects, String selector, IDictionary<String, String> arguments)
        {
            this._fillHook.Execute((self, d, s, args) =>
            {
                self.GetType()
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .Single(m =>
                        m.GetCustomAttributes(typeof(ProxyInterfaceAttribute), true)
                            .Any(a => (a as ProxyInterfaceAttribute).Selector == s)
                        && m.GetParameters().Select(p => p.ParameterType) == new Type[]
                    {
                        typeof(IEnumerable<StorageObject>),
                        typeof(IDictionary<String, String>),
                    }
                    ).Invoke(
                        self,
                        new Object[]
                    {
                        d,
                        args,
                    }
                    );
            }, this, objects, selector, arguments);
        }

        public IAsyncResult BeginFill(
            IEnumerable<StorageObject> objects,
            String selector,
            IDictionary<String, String> arguments,
            AsyncCallback callback,
            Object state
        )
        {
            IAsyncResult asyncResult = new Action<IEnumerable<StorageObject>, String, IDictionary<String, String>>(this.Fill)
                .BeginInvoke(objects, selector, arguments, callback, state);
            this._asyncResults.Add(asyncResult);
            return asyncResult;
        }

        public void EndFill(IAsyncResult asyncResult)
        {
            this._asyncResults.Remove(asyncResult);
            ((asyncResult as AsyncResult).AsyncDelegate as Action<IEnumerable<StorageObject>, String[], IDictionary<String, String>>)
                .EndInvoke(asyncResult);
        }

        public IEnumerable<StorageObject> GetData(String selector, IDictionary<String, String> arguments)
        {
            IEnumerable<StorageObject> objects = new List<StorageObject>();
            this.Fill(objects, selector, arguments);
            return objects;
        }

        public IAsyncResult BeginGetData(
            String selector,
            IDictionary<String, String> arguments,
            AsyncCallback callback,
            Object state
        )
        {
            IAsyncResult asyncResult = new Func<String, IDictionary<String, String>, IEnumerable<StorageObject>>(this.GetData).BeginInvoke(selector, arguments, callback, state);
            this._asyncResults.Add(asyncResult);
            return asyncResult;
        }

        public IEnumerable<StorageObject> EndGetData(IAsyncResult asyncResult)
        {
            this._asyncResults.Remove(asyncResult);
            return ((asyncResult as AsyncResult).AsyncDelegate as Func<String[], IDictionary<String, String>, IEnumerable<StorageObject>>).EndInvoke(asyncResult);
        }
    }
}
