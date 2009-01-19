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
using System.Runtime.Remoting.Messaging;
using System.Collections.Generic;

namespace XSpect.MetaTweet
{
    public abstract class ServantModule
        : Module
    {
        private readonly Hook<ServantModule> _startHook = new Hook<ServantModule>();

        private readonly Hook<ServantModule> _stopHook = new Hook<ServantModule>();

        private readonly Hook<ServantModule> _pauseHook = new Hook<ServantModule>();

        private readonly Hook<ServantModule> _continueHook = new Hook<ServantModule>();

        private readonly Hook<ServantModule> _abortHook = new Hook<ServantModule>();

        private readonly Hook<ServantModule> _waitHook = new Hook<ServantModule>();

        public Hook<ServantModule> StartHook
        {
            get
            {
                return this._startHook;
            }
        }

        public Hook<ServantModule> StopHook
        {
            get
            {
                return this._stopHook;
            }
        }

        public Hook<ServantModule> PauseHook
        {
            get
            {
                return this._pauseHook;
            }
        }

        public Hook<ServantModule> ContinueHook
        {
            get
            {
                return this._continueHook;
            }
        }

        public Hook<ServantModule> AbortHook
        {
            get
            {
                return this._abortHook;
            }
        }

        public Hook<ServantModule> WaitHook
        {
            get
            {
                return this._waitHook;
            }
        }

        public override void Dispose()
        {
            this.Abort();
        }

        public void Start()
        {
            this._startHook.Execute(self =>
            {
                self.StartImpl();
            }, this);
        }

        protected abstract void StartImpl();

        public void Stop()
        {
            this._stopHook.Execute(self =>
            {
                self.StopImpl();
            }, this);
        }

        protected abstract void StopImpl();

        public void Pause()
        {
            this._pauseHook.Execute(self =>
            {
                self.PauseImpl();
            }, this);
        }

        protected virtual void PauseImpl()
        {
            this.StopImpl();
        }

        public void Continue()
        {
            this._continueHook.Execute(self =>
            {
                self.ContinueImpl();
            }, this);
        }

        protected virtual void ContinueImpl()
        {
            this.StartImpl();
        }

        public void Abort()
        {
            this._abortHook.Execute(self =>
            {
                self.AbortImpl();
            }, this);
        }

        protected virtual void AbortImpl()
        {
        }

        public void Wait()
        {
            this._waitHook.Execute(self =>
            {
                self.WaitImpl();
            }, this);
        }

        protected virtual void WaitImpl()
        {
        }

        public IAsyncResult BeginStart(AsyncCallback callback, Object state)
        {
            return new Action(this.Start).BeginInvoke(callback, state);
        }

        public void EndStart(IAsyncResult asyncResult)
        {
            ((asyncResult as AsyncResult).AsyncDelegate as Action).EndInvoke(asyncResult);
        }

        public IAsyncResult BeginStop(AsyncCallback callback, Object state)
        {
            return new Action(this.Stop).BeginInvoke(callback, state);
        }

        public void EndStop(IAsyncResult asyncResult)
        {
            ((asyncResult as AsyncResult).AsyncDelegate as Action).EndInvoke(asyncResult);
        }

        public IAsyncResult BeginPause(AsyncCallback callback, Object state)
        {
            return new Action(this.Pause).BeginInvoke(callback, state);
        }

        public void EndPause(IAsyncResult asyncResult)
        {
            ((asyncResult as AsyncResult).AsyncDelegate as Action).EndInvoke(asyncResult);
        }

        public IAsyncResult BeginContinue(AsyncCallback callback, Object state)
        {
            return new Action(this.Continue).BeginInvoke(callback, state);
        }

        public void EndContinue(IAsyncResult asyncResult)
        {
            ((asyncResult as AsyncResult).AsyncDelegate as Action).EndInvoke(asyncResult);
        }

        public IAsyncResult BeginAbort(AsyncCallback callback, Object state)
        {
            return new Action(this.Abort).BeginInvoke(callback, state);
        }

        public void EndAbort(IAsyncResult asyncResult)
        {
            ((asyncResult as AsyncResult).AsyncDelegate as Action).EndInvoke(asyncResult);
        }

        public IAsyncResult BeginWait(AsyncCallback callback, Object state)
        {
            return new Action(this.Wait).BeginInvoke(callback, state);
        }

        public void EndWait(IAsyncResult asyncResult)
        {
            ((asyncResult as AsyncResult).AsyncDelegate as Action).EndInvoke(asyncResult);
        }
    }
}
