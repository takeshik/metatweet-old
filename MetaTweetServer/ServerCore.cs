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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;
using XSpect.MetaTweet.Properties;
using XSpect.Reflection;

namespace XSpect.MetaTweet
{
    public sealed class ServerCore
        : MarshalByRefObject,
          IDisposable
    {
        private static readonly DirectoryInfo _rootDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;

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

        private readonly List<Action<ServerCore, String>> _beforeRemoveListenerHooks = new List<Action<ServerCore, String>>();

        private readonly List<Action<ServerCore, String>> _afterRemoveListenerHooks = new List<Action<ServerCore, String>>();

        private readonly List<Action<ServerCore, String>> _beforeAddRealmHooks = new List<Action<ServerCore, String>>();
        
        private readonly List<Action<ServerCore, String>> _afterAddRealmHooks = new List<Action<ServerCore, String>>();
        
        private readonly List<Action<ServerCore, String>> _beforeRemoveRealmHooks = new List<Action<ServerCore, String>>();
        
        private readonly List<Action<ServerCore, String>> _afterRemoveRealmHooks = new List<Action<ServerCore, String>>();

        private readonly List<Action<ServerCore, String>> _beforeExecuteCodeHooks = new List<Action<ServerCore, String>>();

        private readonly List<Action<ServerCore, String>> _afterExecuteCodeHooks = new List<Action<ServerCore, String>>();

        private readonly ILog _log = LogManager.GetLogger(typeof(ServerCore));

        private readonly AssemblyManager _assemblyManager = new AssemblyManager();

        private readonly Dictionary<String, Listener> _listeners = new Dictionary<String, Listener>();

        private readonly Dictionary<String, Realm> _realms = new Dictionary<String, Realm>();

        private readonly Storage _storage;

        public static DirectoryInfo RootDirectory
        {
            get
            {
                return _rootDirectory;
            }
        }

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

        public List<Action<ServerCore, String>> BeforeRemoveListenerHooks
        {
            get
            {
                return _beforeRemoveListenerHooks;
            }
        }

        public List<Action<ServerCore, String>> AfterRemoveListenerHooks
        {
            get
            {
                return _afterRemoveListenerHooks;
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

        public Storage Storage
        {
            get
            {
                return this._storage;
            }
        }

        public ServerCore()
        {
            foreach (Action<ServerCore> hook in this._beforeInitializeHooks)
            {
                hook(this);
            }
            this._storage = new Storage(this, @"data source=""Tween.db""");
            this.Initialize();
            foreach (Action<ServerCore> hook in this._afterInitializeHooks)
            {
                hook(this);
            }
        }

        public override Object InitializeLifetimeService()
        {
            return null;
        }

        public void Initialize()
        {
            this.InitializeDefaultCompilerSettings();
            this.InitializeDefaultLogHooks();
            this.ExecuteCode(RootDirectory.GetFiles("init.*").Single().FullName);
            this.ExecuteCode(RootDirectory.GetFiles("rc.*").Single().FullName);
        }

        private void InitializeDefaultCompilerSettings()
        {
            this.AssemblyManager.DefaultOptions.Add("CompilerVersion", "v3.5");
            this.AssemblyManager.DefaultParameters.GenerateExecutable = true;
            this.AssemblyManager.DefaultParameters.IncludeDebugInformation = true;
            this.AssemblyManager.DefaultParameters.ReferencedAssemblies.AddRange(new String[]
            {
                typeof(System.Object).Assembly.Location,                // mscorlib
                typeof(System.Uri).Assembly.Location,                   // System
                typeof(System.Linq.Enumerable).Assembly.Location,       // System.Core
                typeof(System.Data.DataSet).Assembly.Location,          // System.Data
                typeof(System.Xml.XmlDocument).Assembly.Location,       // System.Xml
                typeof(System.Xml.Linq.XDocument).Assembly.Location,    // System.Xml.Linq
                typeof(XSpect.Random).Assembly.Location,                // XSpectCommonFramework
                Assembly.GetExecutingAssembly().Location,               // MetaTweetServer
            });
        }

        private void InitializeDefaultLogHooks()
        {
            this.BeforeInitializeHooks.Add(self => self.Log.InfoFormat(
                Resources.ServerInitializing,
                Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                Environment.OSVersion.ToString(),
                Thread.CurrentThread.CurrentUICulture.ToString()
            ));
            this.AfterInitializeHooks.Add(self => self.Log.Info(Resources.ServerInitialized));
            this.BeforeStartHooks.Add(self => self.Log.Info(Resources.ServerStarting));
            this.AfterStartHooks.Add(self => self.Log.Info(Resources.ServerStarted));
            this.BeforeStopHooks.Add(self => self.Log.Info(Resources.ServerStopping));
            this.AfterStopHooks.Add(self => self.Log.Info(Resources.ServerStopped));
            this.BeforePauseHooks.Add(self => self.Log.Info(Resources.ServerPausing));
            this.AfterPauseHooks.Add(self => self.Log.Info(Resources.ServerPaused));
            this.BeforeResumeHooks.Add(self => self.Log.Info(Resources.ServerResuming));
            this.AfterResumeHooks.Add(self => self.Log.Info(Resources.ServerResumed));
            this.BeforeTerminateHooks.Add(self => self.Log.Info(Resources.ServerTerminating));
            this.AfterTerminateHooks.Add(self => self.Log.Info(Resources.ServerTerminated));
            this.BeforeWaitToEndHooks.Add(self => self.Log.Info(Resources.ServerWaitingToEnd));
            this.AfterWaitToEndHooks.Add(self => self.Log.Info(Resources.ServerWaitedToEnd));
            this.AfterAddListenerHooks.Add((self, id, listener) => self.Log.InfoFormat(
                Resources.ListenerAdded,
                id,
                listener.GetType().AssemblyQualifiedName,
                listener.GetType().Assembly.CodeBase
            ));
            this.AfterRemoveListenerHooks.Add((self, id) => self.Log.InfoFormat(
                Resources.ListenerRemoved,
                id
            ));
            this.AfterAddRealmHooks.Add((self, id) => self.Log.InfoFormat(
                Resources.RealmAdded,
                id
            ));
            this.AfterRemoveRealmHooks.Add((self, id) => self.Log.InfoFormat(
                Resources.RealmRemoved,
                id
            ));
            this.AfterExecuteCodeHooks.Add((self, path) => self.Log.InfoFormat(
                Resources.CodeExecuted,
                path
            ));
            this.Storage.AfterActivateHooks.Add(self => self.Parent.Log.Info(Resources.StorageActivated));
            this.Storage.AfterInactivateHooks.Add(self => self.Parent.Log.Info(Resources.StorageInactivated));
        }

        public void Start(IDictionary<String, String> arguments)
        {
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
        }

        public void Stop()
        {
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
        }

        public void StopGracefully()
        {
            IEnumerable<IAsyncResult> asyncResults = this._listeners.Values.Select(l => l.BeginWait(
                r =>
                {
                    (r.AsyncState as Listener).EndWait(r);
                    (r.AsyncState as Listener).Stop();
                }, l
            ));
            WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
        }

        public void Pause()
        {
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
        }

        public void Resume()
        {
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
        }

        public void Dispose()
        {
            foreach (Action<ServerCore> hook in this._beforeTerminateHooks)
            {
                hook(this);
            }
            foreach (Action<ServerCore> hook in this._afterTerminateHooks)
            {
                hook(this);
            }
        }

        public void WaitToEnd()
        {
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
        }

        public Listener GetListener(String id)
        {
            return this._listeners[id];
        }

        public void AddListener(String id, Listener listener)
        {
            foreach (Action<ServerCore, String, Listener> hook in this._beforeAddListenerHooks)
            {
                hook(this, id, listener);
            }
            listener.Register(this, id);
            this._listeners.Add(id, listener);
            foreach (Action<ServerCore, String, Listener> hook in this._afterAddListenerHooks)
            {
                hook(this, id, listener);
            }
        }

        public void RemoveListener(String id)
        {
            foreach (Action<ServerCore, String> hook in this._beforeRemoveListenerHooks)
            {
                hook(this, id);
            }
            this._realms.Remove(id);
            foreach (Action<ServerCore, String> hook in this._afterRemoveListenerHooks)
            {
                hook(this, id);
            }
        }

        public Realm GetRealm(String id)
        {
            return this._realms[id];
        }

        public void AddRealm(String id)
        {
            foreach (Action<ServerCore, String> hook in this._beforeAddRealmHooks)
            {
                hook(this, id);
            }
            this._realms.Add(id, new Realm(this, id));
            foreach (Action<ServerCore, String> hook in this._afterAddRealmHooks)
            {
                hook(this, id);
            }
        }

        public void RemoveRealm(String id)
        {
            foreach (Action<ServerCore, String> hook in this._beforeRemoveRealmHooks)
            {
                hook(this, id);
            }
            this._realms.Remove(id);
            foreach (Action<ServerCore, String> hook in this._afterRemoveRealmHooks)
            {
                hook(this, id);
            }
        }

        public void ExecuteCode(String path)
        {
            foreach (Action<ServerCore, String> hook in this._beforeExecuteCodeHooks)
            {
                hook(this, path);
            }
            this._assemblyManager.CreateDomain("__tempScript");
            using (StreamReader reader = new StreamReader(path))
            {
                this._assemblyManager.Compile("__tempScript", Path.GetExtension(path), reader.ReadToEnd()).EntryPoint.Invoke(null, null);
            }
            this._assemblyManager.UnloadDomain("__tempScript");
            foreach (Action<ServerCore, String> hook in this._afterExecuteCodeHooks)
            {
                hook(this, path);
            }
        }
    }
}