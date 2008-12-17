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

namespace XSpect.MetaTweet
{
    public abstract class Proxy
        : Object
    {
        private Realm _parent;

        private String _name;

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

        public void Register(Realm parent, String name)
        {
            if (this._parent != null || this._name != null)
            {
                throw new InvalidOperationException();
            }
            this._parent = parent;
            this._name = name;
        }

        public abstract Int32 Fill(StorageDataSetUnit datasets, String[] selector, IDictionary<String, String> arguments);

        public IAsyncResult BeginFill(
            StorageDataSetUnit datasets,
            String[] selector,
            IDictionary<String, String> arguments,
            AsyncCallback callback,
            Object state
        )
        {
            IAsyncResult asyncResult = new Func<StorageDataSetUnit, String[], IDictionary<String, String>, Int32>(this.Fill).BeginInvoke(datasets, selector, arguments, callback, state);
            this._asyncResults.Add(asyncResult);
            return asyncResult;
        }

        public Int32 EndFill(IAsyncResult asyncResult)
        {
            this._asyncResults.Remove(asyncResult);
            return ((asyncResult as AsyncResult).AsyncDelegate as Func<StorageDataSetUnit, String[], IDictionary<String, String>, Int32>).EndInvoke(asyncResult);
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
