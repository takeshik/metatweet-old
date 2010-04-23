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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using XSpect.MetaTweet.Modules;
using XSpect.MetaTweet.Objects;

namespace XSpect.MetaTweet
{
    public class RequestTask
        : MarshalByRefObject
    {
        public RequestManager Parent
        {
            get;
            private set;
        }

        public Int32 Id
        {
            get;
            private set;
        }

        public Request Request
        {
            get;
            private set;
        }

        public RequestTaskState State
        {
            get;
            private set;
        }

        public Int32 CurrentPosition
        {
            get;
            private set;
        }

        public Nullable<DateTime> StartTime
        {
            get;
            private set;
        }

        public Nullable<DateTime> ExitTime
        {
            get;
            private set;
        }

        public TimeSpan ElapsedTime
        {
            get
            {
                return this.State == RequestTaskState.Initialized
                    ? (this.HasExited
                          ? this.ExitTime.Value
                          : DateTime.UtcNow
                      ).Subtract(this.StartTime.Value)
                    : TimeSpan.Zero;
            }
        }

        public Boolean HasExited
        {
            get
            {
                return this.State == (
                    RequestTaskState.Succeeded |
                    RequestTaskState.Failed |
                    RequestTaskState.Canceled
                );
            }
        }

        public RequestTask(RequestManager parent, Request request)
        {
            this.Parent = parent;
            this.Id = this.Parent.GetNewId();
            this.Request = request;
            this.State = RequestTaskState.Initialized;
        }

        public void Start()
        {
            this.State = RequestTaskState.WaitForStart;
            // Stub for blocking situations at start
        }

        public Object Process(Type outputType)
        {
            this.CurrentPosition = 0;
            IEnumerable<StorageObject> results = null;

            foreach (Request req in this.Request)
            {
                StorageModule storageModule = this.Parent.Parent.ModuleManager.GetModule<StorageModule>(req.StorageName);

                if (this.CurrentPosition == 0) // Invoking InputFlowModule
                {
                    InputFlowModule flowModule = this.Parent.Parent.ModuleManager.GetModule<InputFlowModule>(req.FlowName);
                    results = flowModule.Input(
                        req.Selector,
                        storageModule,
                        req.Arguments
                    );
                }
                else if (this.CurrentPosition != this.Request.Count() - 1) // Invoking FilterFlowModule
                {
                    FilterFlowModule flowModule = this.Parent.Parent.ModuleManager.GetModule<FilterFlowModule>(req.FlowName);

                    flowModule.Filter(
                        req.Selector,
                        results,
                        storageModule,
                        req.Arguments
                    );
                }
                else // Invoking OutputFlowModule (End of flow)
                {
                    OutputFlowModule flowModule = this.Parent.Parent.ModuleManager.GetModule<OutputFlowModule>(req.FlowName);

                    return flowModule.Output(
                        req.Selector,
                        results,
                        storageModule,
                        req.Arguments,
                        outputType
                    );
                }

                ++this.CurrentPosition;
            }
            // Whether the process is not finished:
            return typeof(IEnumerable<StorageObject>).IsAssignableFrom(outputType)
                ? results
                : null;
        }
    }
}