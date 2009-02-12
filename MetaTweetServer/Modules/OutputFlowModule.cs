// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
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
using System.Reflection;

namespace XSpect.MetaTweet.Modules
{
    public abstract class OutputFlowModule
        : FlowModule
    {
        public Hook<OutputFlowModule, String, IEnumerable<StorageObject>, Storage, IDictionary<String, String>, Type> OutputHook
        {
            get;
            private set;
        }

        public OutputFlowModule()
        {
            this.OutputHook = new Hook<OutputFlowModule, String, IEnumerable<StorageObject>, Storage, IDictionary<String, String>, Type>();
        }

        public TOutput Output<TOutput>(String selector, IEnumerable<StorageObject> source, Storage storage, IDictionary<String, String> arguments)
        {
            return this.OutputHook.Execute<TOutput>((self, selector_, source_, storage_, arguments_, type_) =>
            {
                String param;
                return this.GetFlowInterface<TOutput>(selector_, out param).Invoke<TOutput>(
                    self,
                    source_,
                    storage_,
                    param,
                    arguments_
                );
            }, this, selector, source, storage, arguments, typeof(TOutput));
        }

        public IAsyncResult BeginOutput<TOutput>(
            String selector,
            IEnumerable<StorageObject> source,
            Storage storage,
            IDictionary<String, String> arguments,
            AsyncCallback callback,
            Object state
        )
        {
            return new Func<String, IEnumerable<StorageObject>, Storage, IDictionary<String, String>, TOutput>(this.Output<TOutput>).BeginInvoke(
                selector,
                source,
                storage,
                arguments,
                callback,
                state
            );
        }

        public TOutput EndOutput<TOutput>(IAsyncResult result)
        {
            return ((result as AsyncResult).AsyncDelegate as Func<String, IEnumerable<StorageObject>, IDictionary<String, String>, TOutput>)
                .EndInvoke(result);
        }
    }
}