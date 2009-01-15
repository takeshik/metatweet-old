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
using XSpect.MetaTweet.ObjectModel;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace XSpect.MetaTweet
{
    public abstract class OutputFlowModule
        : FlowModule
    {
        private readonly Hook<OutputFlowModule, String, IEnumerable<StorageObject>, IDictionary<String, String>> _outputHook
            = new Hook<OutputFlowModule, String, IEnumerable<StorageObject>, IDictionary<String, String>>();

        public Hook<OutputFlowModule, String, IEnumerable<StorageObject>, IDictionary<String, String>> OutputHook
        {
            get
            {
                return this._outputHook;
            }
        }

        public T Output<T>(String selector, IEnumerable<StorageObject> source, IDictionary<String, String> arguments)
        {
            return this.OutputHook.Execute<T>((self, sel, src, args) =>
            {
                return (T) this.GetMethod(selector).Invoke(this, new Object[]
                {
                    source,
                    arguments,
                });
            }, this, selector, source, arguments);
        }

        public IAsyncResult BeginOutput<T>(
            String selector,
            IEnumerable<StorageObject> source,
            IDictionary<String, String> arguments,
            AsyncCallback callback,
            Object state
        )
        {
            return new Func<String, IEnumerable<StorageObject>, IDictionary<String, String>, T>(this.Output<T>).BeginInvoke(
                selector,
                source,
                arguments,
                callback,
                state
            );
        }

        public T EndOutput<T>(IAsyncResult result)
        {
            return ((result as AsyncResult).AsyncDelegate as Func<String, IEnumerable<StorageObject>, IDictionary<String, String>, T>)
                .EndInvoke(result);
        }
    }
}