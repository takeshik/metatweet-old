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
using System.Reflection;
using System.Threading;
using log4net;
using XSpect.MetaTweet.Properties;
using XSpect.Reflection;
using System.IO;

namespace XSpect.MetaTweet
{
    public sealed class ServerCore
        : MarshalByRefObject,
          IDisposable
    {
        private readonly List<Action<ServerCore>> _beforeInitializeHooks = new List<Action<ServerCore>>();

        private readonly List<Action<ServerCore>> _afterInitializeHooks = new List<Action<ServerCore>>();

        private readonly List<Action<ServerCore>> _beforeStartHooks = new List<Action<ServerCore>>();
        
        private readonly List<Action<ServerCore>> _afterStartHooks = new List<Action<ServerCore>>();
        
        private readonly List<Action<ServerCore>> _beforeStopHooks = new List<Action<ServerCore>>();

        private readonly List<Action<ServerCore>> _afterStopHooks = new List<Action<ServerCore>>();

        private readonly List<Action<ServerCore>> _beforePauseHooks = new List<Action<ServerCore>>();

        private readonly List<Action<ServerCore>> _afterPauseHooks = new List<Action<ServerCore>>();

        private readonly List<Action<ServerCore>> _beforeResumeHooks = new List<Action<ServerCore>>();

        private readonly List<Action<ServerCore>> _afterResumeHooks = new List<Action<ServerCore>>();

        private readonly List<Action<ServerCore>> _beforeWaitToEndHooks = new List<Action<ServerCore>>();

        private readonly List<Action<ServerCore>> _afterWaitToEndHooks = new List<Action<ServerCore>>();

        private readonly List<Action<ServerCore>> _beforeTerminateHooks = new List<Action<ServerCore>>();

        private readonly List<Action<ServerCore>> _afterTerminateHooks = new List<Action<ServerCore>>();

        private readonly List<Action<ServerCore, String, Listener>> _beforeAddListenerHooks = new List<Action<ServerCore, String, Listener>>();

        private readonly List<Action<ServerCore, String, Listener>> _afterAddListenerHooks = new List<Action<ServerCore, String, Listener>>();
        
        private readonly List<Action<ServerCore, String>> _beforeAddRealmHooks = new List<Action<ServerCore, String>>();
        
        private readonly List<Action<ServerCore, String>> _afterAddRealmHooks = new List<Action<ServerCore, String>>();
        
        private readonly List<Action<ServerCore, String>> _beforeRemoveRealmHooks = new List<Action<ServerCore, String>>();
        
        private readonly List<Action<ServerCore, String>> _afterRemoveRealmHooks = new List<Action<ServerCore, String>>();

        private readonly List<Action<ServerCore, String>> _beforeExecuteCodeHooks = new List<Action<ServerCore, String>>();

        private readonly List<Action<ServerCore, String>> _afterExecuteCodeHooks = new List<Action<ServerCore, String>>();

        public List<Action<ServerCore>> BeforeInitializeHooks
        {
            get
            {
                return this._beforeInitializeHooks;
            }
        }

        public List<Action<ServerCore>> AfterInitializeHooks
        {
            get
            {
                return this._afterInitializeHooks;
            }
        }

        public List<Action<ServerCore>> BeforeStartHooks
        {
            get
            {
                return this._beforeStartHooks;
            }
        }

        public List<Action<ServerCore>> AfterStartHooks
        {
            get
            {
                return this._afterStartHooks;
            }
        }

        public List<Action<ServerCore>> BeforeStopHooks
        {
            get
            {
                return this._beforeStopHooks;
            }
        }

        public List<Action<ServerCore>> AfterStopHooks
        {
            get
            {
                return this._afterStopHooks;
            }
        }

        public List<Action<ServerCore>> BeforePauseHooks
        {
            get
            {
                return this._beforePauseHooks;
            }
        }

        public List<Action<ServerCore>> AfterPauseHooks
        {
            get
            {
                return this._afterPauseHooks;
            }
        }

        public List<Action<ServerCore>> BeforeResumeHooks
        {
            get
            {
                return this._beforeResumeHooks;
            }
        }

        public List<Action<ServerCore>> AfterResumeHooks
        {
            get
            {
                return this._afterResumeHooks;
            }
        }

        public List<Action<ServerCore>> BeforeWaitToEndHooks
        {
            get
            {
                return this._beforeWaitToEndHooks;
            }
        }

        public List<Action<ServerCore>> AfterWaitToEndHooks
        {
            get
            {
                return this._afterWaitToEndHooks;
            }
        }

        public List<Action<ServerCore>> BeforeTerminateHooks
        {
            get
            {
                return this._beforeTerminateHooks;
            }
        }

        public List<Action<ServerCore>> AfterTerminateHooks
        {
            get
            {
                return this._afterTerminateHooks;
            }
        }

        public List<Action<ServerCore, String, Listener>> BeforeAddListenerHooks
        {
            get
            {
                return this._beforeAddListenerHooks;
            }
        }

        public List<Action<ServerCore, String, Listener>> AfterAddListenerHooks
        {
            get
            {
                return this._afterAddListenerHooks;
            }
        }

        public List<Action<ServerCore, String>> BeforeAddRealmHooks
        {
            get
            {
                return this._beforeAddRealmHooks;
            }
        }

        public List<Action<ServerCore, String>> AfterAddRealmHooks
        {
            get
            {
                return this._afterAddRealmHooks;
            }
        }

        public List<Action<ServerCore, String>> BeforeRemoveRealmHooks
        {
            get
            {
                return this._beforeRemoveRealmHooks;
            }
        }

        public List<Action<ServerCore, String>> AfterRemoveRealmHooks
        {
            get
            {
                return this._afterRemoveRealmHooks;
            }
        }

        public List<Action<ServerCore, String>> BeforeExecuteCodeHooks
        {
            get
            {
                return this._beforeExecuteCodeHooks;
            }
        }

        public List<Action<ServerCore, String>> AfterExecuteCodeHooks
        {
            get
            {
                return this._afterExecuteCodeHooks;
            }
        } 

        private readonly ILog _log = LogManager.GetLogger(typeof(ServerCore));

        private readonly AssemblyManager _assemblyManager = new AssemblyManager();

        private readonly Dictionary<String, Listener> _listeners = new Dictionary<String, Listener>();

        private readonly Dictionary<String, Realm> _realms = new Dictionary<String, Realm>();

        public AssemblyManager AssemblyManager
        {
            get
            {
                return this._assemblyManager;
            }
        }

        public ILog Log
        {
            get
            {
                return this._log;
            }
        }

        public ServerCore()
        {
            this._log.InfoFormat(
                Resources.ServerInitializing,
                Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                Environment.OSVersion.ToString(),
                Thread.CurrentThread.CurrentUICulture.ToString()
            );
            foreach (Action<ServerCore> hook in this._beforeInitializeHooks)
            {
                hook(this);
            }
            foreach (Action<ServerCore> hook in this._afterInitializeHooks)
            {
                hook(this);
            }
        }

        public override Object InitializeLifetimeService()
        {
            return null;
        }

        public void Start(IDictionary<String, String> arguments)
        {
            this._log.Info(Resources.ServerStarting);
            foreach (Action<ServerCore> hook in this._beforeStartHooks)
            {
                hook(this);
            }
            IEnumerable<IAsyncResult> asyncResults = this._listeners.Values.Select(l => l.BeginStart(
                r => (r.AsyncState as Listener).EndStart(r), l
            ));
            WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
            foreach (Action<ServerCore> hook in this._afterStartHooks)
            {
                hook(this);
            }
            this._log.Info(Resources.ServerStarted);
        }

        public void Stop()
        {
            this._log.Info(Resources.ServerStopping);
            foreach (Action<ServerCore> hook in this._beforeStopHooks)
            {
                hook(this);
            }
            IEnumerable<IAsyncResult> asyncResults = this._listeners.Values.Select(l => l.BeginAbort(
                r =>
                {
                    (r.AsyncState as Listener).EndAbort(r);
                    (r.AsyncState as Listener).Stop();
                }, l
            ));
            WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
            foreach (Action<ServerCore> hook in this._afterStopHooks)
            {
                hook(this);
            }
            this._log.Info(Resources.ServerStopped);
        }

        public void StopGracefully()
        {
            this._log.Info(Resources.ServerStopping);
            IEnumerable<IAsyncResult> asyncResults = this._listeners.Values.Select(l => l.BeginWait(
                r =>
                {
                    (r.AsyncState as Listener).EndWait(r);
                    (r.AsyncState as Listener).Stop();
                }, l
            ));
            WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
            this._log.Info(Resources.ServerStopped);
        }

        public void Pause()
        {
            this._log.Info(Resources.ServerPausing);
            foreach (Action<ServerCore> hook in this._beforePauseHooks)
            {
                hook(this);
            }
            IEnumerable<IAsyncResult> asyncResults = this._listeners.Values.Select(l => l.BeginStop(
                r => (r.AsyncState as Listener).EndStop(r), l
            ));
            WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
            foreach (Action<ServerCore> hook in this._afterPauseHooks)
            {
                hook(this);
            }
            this._log.Info(Resources.ServerPaused);
        }

        public void Resume()
        {
            this._log.Info(Resources.ServerResuming);
            foreach (Action<ServerCore> hook in this._beforeResumeHooks)
            {
                hook(this);
            }
            IEnumerable<IAsyncResult> asyncResults = this._listeners.Values.Select(l => l.BeginStart(
                r => (r.AsyncState as Listener).EndStart(r), l
            ));
            WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
            foreach (Action<ServerCore> hook in this._afterResumeHooks)
            {
                hook(this);
            }
            this._log.Info(Resources.ServerResumed);
        }

        public void Dispose()
        {
            this._log.Info(Resources.ServerTerminating);
            foreach (Action<ServerCore> hook in this._beforeTerminateHooks)
            {
                hook(this);
            }
            foreach (Action<ServerCore> hook in this._afterTerminateHooks)
            {
                hook(this);
            }
            this._log.Info(Resources.ServerTerminated);
        }

        public void WaitToEnd()
        {
            this._log.Info(Resources.ServerWaitingToEnd);
            foreach (Action<ServerCore> hook in this._beforeWaitToEndHooks)
            {
                hook(this);
            }
            IEnumerable<IAsyncResult> asyncResults = this._listeners.Values.Select(l => l.BeginWait(
                r => (r.AsyncState as Listener).EndStart(r), l
            ));
            WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
            foreach (Action<ServerCore> hook in this._afterWaitToEndHooks)
            {
                hook(this);
            }
            this._log.Info(Resources.ServerWaitedToEnd);
        }

        public Listener GetListener(String id)
        {
            return this._listeners[id];
        }

        public void AddListener(String id, Listener listener)
        {
            listener.Register(this, id);
            this._listeners.Add(id, listener);
            this._log.InfoFormat(
                Resources.ListenerAdded,
                id,
                listener.GetType().AssemblyQualifiedName,
                listener.GetType().Assembly.CodeBase
            );
        }

        public void RemoveListener(String id)
        {
            this._realms.Remove(id);
            this._log.InfoFormat(Resources.ListenerRemoved, id);
        }

        public Realm GetRealm(String id)
        {
            return this._realms[id];
        }

        public void AddRealm(String id)
        {
            this._realms.Add(id, new Realm(this, id));
            this._log.InfoFormat(Resources.RealmAdded, id);
        }

        public void RemoveRealm(String id)
        {
            this._realms.Remove(id);
            this._log.InfoFormat(Resources.RealmAdded, id);
        }

        public void ExecuteCode(String path)
        {
            this._assemblyManager.CreateDomain("__tempScript");
            using (StreamReader reader = new StreamReader(path))
            {
                this._assemblyManager.Compile("__tempScript", Path.GetExtension(path), reader.ReadToEnd()).EntryPoint.Invoke(null, null);
            }
            this._assemblyManager.UnloadDomain("__tempScript");
        }
    }
}