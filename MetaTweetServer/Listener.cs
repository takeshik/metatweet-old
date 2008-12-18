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
using System.Net;
using System.Runtime.Remoting.Messaging;
using XSpect.MetaTweet.Properties;

namespace XSpect.MetaTweet
{
    public abstract class Listener
        : IDisposable
    {
        private ServerCore _parent;

        private String _name;

        private readonly List<Action<Listener>> _beforeStartHooks = new List<Action<Listener>>();

        private readonly List<Action<Listener>> _afterStartHooks = new List<Action<Listener>>();
        
        private readonly List<Action<Listener>> _beforeStopHooks = new List<Action<Listener>>();

        private readonly List<Action<Listener>> _afterStopHooks = new List<Action<Listener>>();
        
        private readonly List<Action<Listener>> _beforeAbortHooks = new List<Action<Listener>>();

        private readonly List<Action<Listener>> _afterAbortHooks = new List<Action<Listener>>();

        private readonly List<Action<Listener>> _beforeWaitHooks = new List<Action<Listener>>();

        private readonly List<Action<Listener>> _afterWaitHooks = new List<Action<Listener>>();

        public IList<Action<Listener>> BeforeStartHooks
        {
            get
            {
                return this._beforeStartHooks;
            }
        }

        public IList<Action<Listener>> AfterStartHooks
        {
            get
            {
                return this._afterStartHooks;
            }
        }
        
        public IList<Action<Listener>> BeforeStopHooks
        {
            get
            {
                return this._beforeStopHooks;
            }
        }
        
        public IList<Action<Listener>> AfterStopHooks
        {
            get
            {
                return this._afterStopHooks;
            }
        }
        
        public IList<Action<Listener>> BeforeAbortHooks
        {
            get
            {
                return this._beforeAbortHooks;
            }
        }
        
        public IList<Action<Listener>> AfterAbortHooks
        {
            get
            {
                return this._afterAbortHooks;
            }
        }
        
        public IList<Action<Listener>> BeforeWaitHooks
        {
            get
            {
                return this._beforeWaitHooks;
            }
        }
        
        public IList<Action<Listener>> AfterWaitHooks
        {
            get
            {
                return this._afterWaitHooks;
            }
        }

        public ServerCore Parent
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

        public virtual void Dispose()
        {
            this.Abort();
        }

        public void Register(ServerCore parent, String name)
        {
            if (this._parent != null || this._name != null)
            {
                throw new InvalidOperationException();
            }
            this._parent = parent;
            this._name = name;
        }

        public void Start()
        {
            this._parent.Log.InfoFormat(Resources.ListenerStarting, this._name);
            foreach (Action<Listener> hook in this._beforeStartHooks)
            {
                hook(this);
            }
            this.StartImpl();
            foreach (Action<Listener> hook in this._afterStartHooks)
            {
                hook(this);
            }
            this._parent.Log.InfoFormat(Resources.ListenerStarted, this._name);
        }

        protected abstract void StartImpl();

        public void Stop()
        {
            this._parent.Log.InfoFormat(Resources.ListenerStopping, this._name);
            foreach (Action<Listener> hook in this._beforeStopHooks)
            {
                hook(this);
            }
            this.StopImpl();
            foreach (Action<Listener> hook in this._afterStopHooks)
            {
                hook(this);
            }
            this._parent.Log.InfoFormat(Resources.ListenerStopped, this._name);
        }

        protected abstract void StopImpl();

        public void Abort()
        {
            this._parent.Log.InfoFormat(Resources.ListenerAborting, this._name);
            foreach (Action<Listener> hook in this._beforeAbortHooks)
            {
                hook(this);
            }
            this.AbortImpl();
            foreach (Action<Listener> hook in this.AfterAbortHooks)
            {
                hook(this);
            }
            this._parent.Log.InfoFormat(Resources.ListenerAborted, this._name);
        }

        protected abstract void AbortImpl();

        public void Wait()
        {
            this._parent.Log.InfoFormat(Resources.ListenerWaiting, this._name);
            foreach (Action<Listener> hook in this._beforeWaitHooks)
            {
                hook(this);
            }
            this.WaitImpl();
            foreach (Action<Listener> hook in this._afterWaitHooks)
            {
                hook(this);
            }
            this._parent.Log.InfoFormat(Resources.ListenerWaited, this._name);
        }

        protected abstract void WaitImpl();

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
