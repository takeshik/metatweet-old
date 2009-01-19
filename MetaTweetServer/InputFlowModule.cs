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
    public abstract class InputFlowModule
        : FlowModule
    {
        private readonly Hook<InputFlowModule, String, IDictionary<String, String>> _inputHook
            = new Hook<InputFlowModule, String, IDictionary<String, String>>();

        public override String ModuleType
        {
            get
            {
                return "input";
            }
        }

        public Hook<InputFlowModule, String, IDictionary<String, String>> InputHook
        {
            get
            {
                return this._inputHook;
            }
        }

        public IEnumerable<StorageObject> Input(String selector, IDictionary<String, String> arguments)
        {
            return this.InputHook.Execute<IEnumerable<StorageObject>>((self, sel, args) =>
            {
                return this.GetMethod(selector).Invoke(this, new Object[]
                {
                    arguments,
                }) as IEnumerable<StorageObject>;
            }, this, selector, arguments);
        }

        public IAsyncResult BeginOutput(
            String selector,
            IDictionary<String, String> arguments,
            AsyncCallback callback,
            Object state
        )
        {
            return new Func<String, IDictionary<String, String>, IEnumerable<StorageObject>>(this.Input).BeginInvoke(
                selector,
                arguments,
                callback,
                state
            );
        }

        public IEnumerable<StorageObject> EndOutput(IAsyncResult result)
        {
            return ((result as AsyncResult).AsyncDelegate as Func<String, IDictionary<String, String>, IEnumerable<StorageObject>>)
                .EndInvoke(result);
        }
    }
}