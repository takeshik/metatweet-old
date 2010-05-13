// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Achiral.Extension;
using log4net;
using XSpect.Collections;
using XSpect.Extension;
using XSpect.Hooking;

namespace XSpect.MetaTweet
{
    public class RequestManager
        : MarshalByRefObject,
          IList<RequestTask>,
          IDisposable,
          ILoggable
    {
        private readonly HybridDictionary<Int32, RequestTask> _dictionary;

        public ServerCore Parent
        {
            get;
            private set;
        }

        public Int32 MaxRequestId
        {
            get;
            private set;
        }

        public FuncHook<RequestManager, Request, RequestTask> RegisterHook
        {
            get;
            private set;
        }

        public RequestManager(ServerCore parent)
        {
            this._dictionary = new HybridDictionary<int, RequestTask>((i, e) => e.Id);
            this.Parent = parent;
            this.MaxRequestId = 65536;
            this.RegisterHook = new FuncHook<RequestManager, Request, RequestTask>(this._Register);
        }

        #region Implementation of IEnumerable

        public IEnumerator<RequestTask> GetEnumerator()
        {
            return this._dictionary.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<RequestTask>

        void ICollection<RequestTask>.Add(RequestTask item)
        {
            throw new InvalidOperationException();
        }

        void ICollection<RequestTask>.Clear()
        {
            throw new InvalidOperationException();
        }

        public Boolean Contains(RequestTask item)
        {
            return this._dictionary.ContainsValue(item);
        }

        public void CopyTo(RequestTask[] array, Int32 arrayIndex)
        {
            this._dictionary.CopyToValues(array, arrayIndex);
        }

        Boolean ICollection<RequestTask>.Remove(RequestTask item)
        {
            throw new InvalidOperationException();
        }

        public Int32 Count
        {
            get
            {
                return this._dictionary.Count;
            }
        }

        Boolean ICollection<RequestTask>.IsReadOnly
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region Implementation of IList<RequestTask>

        public Int32 IndexOf(RequestTask item)
        {
            return this._dictionary.IndexOfValue(item);
        }

        void IList<RequestTask>.Insert(Int32 index, RequestTask item)
        {
            throw new InvalidOperationException();
        }

        void IList<RequestTask>.RemoveAt(Int32 index)
        {
            throw new InvalidOperationException();
        }

        RequestTask IList<RequestTask>.this[Int32 index]
        {
            get
            {
                return this._dictionary[index].Value;
            }
            set
            {
                throw new InvalidOperationException();
            }
        }

        #endregion

        public void Dispose()
        {
            this.ForEach(t => t.Cancel());
        }

        public Log Log
        {
            get
            {
                return this.Parent.Log;
            }
        }

        public RequestTask Register(Request request)
        {
            return this.RegisterHook.Execute(request);
        }

        private RequestTask _Register(Request request)
        {
            return new RequestTask(this, request).Let(this._dictionary.Add);
        }

        public RequestTask Start<TOutput>(Request request)
        {
            return this.Register(request).Let(t => t.Start<TOutput>());
        }

        public RequestTask Start(Request request, Type outputType)
        {
            return this.Register(request).Let(t => t.Start(outputType));
        }

        public TOutput Execute<TOutput>(Request request)
        {
            return this.Start<TOutput>(request).Execute<TOutput>();
        }

        public Object Execute(Request request, Type outputType)
        {
            return this.Start(request, outputType).Execute(outputType);
        }

        public void Clean(RequestTask task)
        {
            this._dictionary.RemoveValue(task);
        }

        public void Clean(Boolean cleanAll)
        {
            this._dictionary.RemoveRange(this._dictionary.Tuples
                .Where(t => t.Value.HasExited)
                .Select(t => t.Index)
                .If(l => !cleanAll, l => l.First().ToEnumerable())
            );
        }

        public void Clean()
        {
            this.Clean(false);
        }

        internal Int32 GetNewId()
        {
            if (this.Count == this.MaxRequestId)
            {
                this.Clean(false);
            }
            return 1.UpTo(this.MaxRequestId).Except(this._dictionary.Keys).First();
        }
    }
}