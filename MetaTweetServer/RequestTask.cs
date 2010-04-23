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
        : MarshalByRefObject,
          IDisposable
    {
        private Thread _thread;

        private readonly AutoResetEvent _signal;

        private Object _outputValue;

        private readonly Object _lockObject;

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

        public Request CurrentRequestFragment
        {
            get
            {
                return this.Request.ElementAt(this.CurrentPosition);
            }
        }

        public Int32 RequestFragmentCount
        {
            get
            {
                return this.Request.Count();
            }
        }

        public Type OutputType
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
            this._signal = new AutoResetEvent(true);
            this._lockObject = new Object();
            this.Parent = parent;
            this.Id = this.Parent.GetNewId();
            this.Request = request;
            this.State = RequestTaskState.Initialized;
        }

        public void Dispose()
        {
            this.Parent.Clean(this);
            this._signal.Close();
        }

        public void Start(Type outputType)
        {
            lock (this._lockObject)
            {
                if (this.State == RequestTaskState.Initialized)
                {
                    this.OutputType = outputType;
                    this.State = RequestTaskState.WaitForStart;
                    // Stub for blocking situations at start
                    this._thread = new Thread(() =>
                    {
                        try
                        {
                            this.Execute();
                            this.State = RequestTaskState.Succeeded;
                        }
                        catch (ThreadAbortException ex)
                        {
                            this.State = RequestTaskState.Canceled;
                        }
                        catch (Exception ex)
                        {
                            this.State = RequestTaskState.Failed;
                        }
                        finally
                        {
                            this.ExitTime = DateTime.UtcNow;
                        }
                    })
                    {
                        Name = "RequestTask#" + this.Id,
                        IsBackground = true,
                    };
                    this.StartTime = DateTime.UtcNow;
                    this._thread.Start();
                }
            }
        }

        public void Pause()
        {
            lock (this._lockObject)
            {
                if (this.State == RequestTaskState.Running)
                {
                    this.State = RequestTaskState.WaitForPause;
                    this._signal.Reset();
                }
            }
        }

        public void Continue()
        {
            lock (this._lockObject)
            {
                if (this.State == RequestTaskState.Paused)
                {
                    this.State = RequestTaskState.WaitForContinue;
                    this._signal.Set();
                }
            }
        }

        public void Wait()
        {
            lock (this._lockObject)
            {
                this._thread.Join();
            }
        }

        public void Wait(Int32 millisecondsTimeout)
        {
            lock (this._lockObject)
            {
                this._thread.Join(millisecondsTimeout);
            }
        }

        public void Wait(TimeSpan timeout)
        {
            lock (this._lockObject)
            {
                this._thread.Join(timeout);
            }
        }

        public void Cancel()
        {
            lock (this._lockObject)
            {
                this._thread.Abort();
            }
        }

        public void Kill()
        {
            lock (this._lockObject)
            {
                this.Cancel();
                this.Dispose();
            }
        }

        public TOutput GetOutput<TOutput>()
        {
            lock (this._lockObject)
            {
                if (this.State != RequestTaskState.Succeeded)
                {
                    throw new InvalidOperationException();
                }
                return (TOutput) this._outputValue;
            }
        }

        public TOutput End<TOutput>()
        {
            lock (this._lockObject)
            {
                this.Wait();
                TOutput output = this.GetOutput<TOutput>();
                this.Dispose();
                return output;
            }
        }

        private void Execute()
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

                    this._outputValue = flowModule.Output(
                        req.Selector,
                        results,
                        storageModule,
                        req.Arguments,
                        this.OutputType
                    );
                }

                if (this.State == RequestTaskState.WaitForPause)
                {
                    this.State = RequestTaskState.Paused;
                    this._signal.WaitOne();
                    this.State = RequestTaskState.Running;
                }
                ++this.CurrentPosition;
            }
            // Whether the process is not finished:
            this._outputValue = typeof(IEnumerable<StorageObject>).IsAssignableFrom(this.OutputType)
                ? results
                : null;
        }
    }
}