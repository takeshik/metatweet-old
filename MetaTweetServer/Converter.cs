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

namespace XSpect.MetaTweet
{
    public abstract class Converter
        : Object
    {
        private Realm _parent;

        private String _name;

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

        public void Register(Realm parent, String name)
        {
            if (this._parent != null || this._name != null)
            {
                throw new InvalidOperationException();
            }
            this._parent = parent;
            this._name = name;
        }

        public abstract T Convert<T>(
            StorageDataSetUnit unit
        );

        public IAsyncResult BeginConvert<T>(
            StorageDataSetUnit unit,
            AsyncCallback callback,
            Object state
        )
        {
            return new Func<StorageDataSetUnit, T>(this.Convert<T>).BeginInvoke(unit, callback, state);
        }

        public T EndConvert<T>(IAsyncResult asyncResult)
        {
            return ((asyncResult as AsyncResult).AsyncDelegate as Func<StorageDataSetUnit, T>).EndInvoke(asyncResult);
        }

        public String Convert(
            StorageDataSetUnit unit
        )
        {
            return this.Convert<String>(unit);
        }

        public IAsyncResult BeginConvert(
            StorageDataSetUnit unit,
            AsyncCallback callback,
            Object state
        )
        {
            return this.BeginConvert<String>(unit, callback, state);
        }

        public String EndConvert(IAsyncResult asyncResult)
        {
            return this.EndConvert<String>(asyncResult);
        }

        public abstract StorageDataSetUnit Deconvert<T>(T obj);

        public IAsyncResult BeginDeconvert<T>(
            T obj,
            AsyncCallback callback,
            Object state
        )
        {
            return new Func<T, StorageDataSetUnit>(this.Deconvert<T>).BeginInvoke(obj, callback, state);
        }

        public StorageDataSetUnit EndDeconvert<T>(IAsyncResult asyncResult)
        {
            return ((asyncResult as AsyncResult).AsyncDelegate as Func<T, StorageDataSetUnit>).EndInvoke(asyncResult);
        }

        public StorageDataSetUnit Deconvert(
            String str
        )
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

        public StorageDataSetUnit EndDeconvert(IAsyncResult asyncResult)
        {
            return this.EndDeconvert<String>(asyncResult);
        }
    }
}
