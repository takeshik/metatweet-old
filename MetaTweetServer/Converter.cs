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
using System.Runtime.Remoting.Messaging;
using XSpect.MetaTweet.ObjectModel;

namespace XSpect.MetaTweet
{
    public abstract class Converter
        : Object
    {
        private Realm _parent;

        private String _name;

        private readonly Hook<Converter, Type, IEnumerable<StorageObject>> _convertHook = new Hook<Converter, Type, IEnumerable<StorageObject>>();

        private readonly Hook<Converter, Type, Object> _deconvertHook = new Hook<Converter, Type, Object>();
        
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

        public Hook<Converter, Type, IEnumerable<StorageObject>> ConvertHook
        {
            get
            {
                return this._convertHook;
            }
        }

        public Hook<Converter, Type, Object> DeconvertHook
        {
            get
            {
                return this._deconvertHook;
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

        public T Convert<T>(IEnumerable<StorageObject> objects)
        {
            return this._convertHook.Execute<T>((self, t, u) =>
            {
                return self.ConvertImpl<T>(u);
            }, this, typeof(T), objects);
        }

        protected abstract T ConvertImpl<T>(IEnumerable<StorageObject> objects);

        public IAsyncResult BeginConvert<T>(
            IEnumerable<StorageObject> objects,
            AsyncCallback callback,
            Object state
        )
        {
            return new Func<IEnumerable<StorageObject>, T>(this.Convert<T>).BeginInvoke(objects, callback, state);
        }

        public T EndConvert<T>(IAsyncResult asyncResult)
        {
            return ((asyncResult as AsyncResult).AsyncDelegate as Func<IEnumerable<StorageObject>, T>).EndInvoke(asyncResult);
        }

        public String Convert(IEnumerable<StorageObject> objects)
        {
            return this.Convert<String>(objects);
        }

        public IAsyncResult BeginConvert(
            IEnumerable<StorageObject> objects,
            AsyncCallback callback,
            Object state
        )
        {
            return this.BeginConvert<String>(objects, callback, state);
        }

        public String EndConvert(IAsyncResult asyncResult)
        {
            return this.EndConvert<String>(asyncResult);
        }

        public IEnumerable<StorageObject> Deconvert<T>(T obj)
        {
            return this._deconvertHook.Execute<IEnumerable<StorageObject>>((self, t, o) =>
            {
                return self.DeconvertImpl<T>((T) o);
            }, this, typeof(T), obj);

        }

        protected abstract IEnumerable<StorageObject> DeconvertImpl<T>(T obj);

        public IAsyncResult BeginDeconvert<T>(
            T obj,
            AsyncCallback callback,
            Object state
        )
        {
            return new Func<T, IEnumerable<StorageObject>>(this.Deconvert<T>).BeginInvoke(obj, callback, state);
        }

        public IEnumerable<StorageObject> EndDeconvert<T>(IAsyncResult asyncResult)
        {
            return ((asyncResult as AsyncResult).AsyncDelegate as Func<T, IEnumerable<StorageObject>>).EndInvoke(asyncResult);
        }

        public IEnumerable<StorageObject> Deconvert(String str)
        {
            return this.Deconvert<String>(str);
        }

        public IAsyncResult BeginDeconvert(
            String str,
            AsyncCallback callback,
            Object state
        )
        {
            return this.BeginDeconvert<String>(str, callback, state);
        }

        public IEnumerable<StorageObject> EndDeconvert(IAsyncResult asyncResult)
        {
            return this.EndDeconvert<String>(asyncResult);
        }
    }
}
