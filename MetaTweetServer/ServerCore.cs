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

        private readonly Hook<ServerCore> _initializeHook = new Hook<ServerCore>();

        private readonly Hook<ServerCore> _startHook = new Hook<ServerCore>();
        
        private readonly Hook<ServerCore> _stopHook = new Hook<ServerCore>();
        
        private readonly Hook<ServerCore> _pauseHook = new Hook<ServerCore>();
        
        private readonly Hook<ServerCore> _resumeHook = new Hook<ServerCore>();
        
        private readonly Hook<ServerCore> _waitToEndHook = new Hook<ServerCore>();
        
        private readonly Hook<ServerCore> _terminateHook = new Hook<ServerCore>();
        
        private readonly Hook<ServerCore, String, Listener> _addListenerHook = new Hook<ServerCore, String, Listener>();
        
        private readonly Hook<ServerCore, String> _removeListenerHook = new Hook<ServerCore, String>();
        
        private readonly Hook<ServerCore, String> _addRealmHook = new Hook<ServerCore, String>();
        
        private readonly Hook<ServerCore, String> _removeRealmHook = new Hook<ServerCore, String>();

        private readonly Hook<ServerCore, Storage, String> _loadStorageHook = new Hook<ServerCore, Storage, String>();

        private readonly Hook<ServerCore> _unloadStorageHook = new Hook<ServerCore>();

        private readonly Hook<ServerCore, String> _executeCodeHook = new Hook<ServerCore, String>();
        
        public Hook<ServerCore> InitializeHook
        {
            get
            {
                return this._initializeHook;
            }
        }

        public Hook<ServerCore> StartHook
        {
            get
            {
                return this._startHook;
            }
        }

        public Hook<ServerCore> StopHook
        {
            get
            {
                return this._stopHook;
            }
        }

        public Hook<ServerCore> PauseHook
        {
            get
            {
                return this._pauseHook;
            }
        }

        public Hook<ServerCore> ResumeHook
        {
            get
            {
                return this._resumeHook;
            }
        }

        public Hook<ServerCore> WaitToEndHook
        {
            get
            {
                return this._waitToEndHook;
            }
        }

        public Hook<ServerCore> TerminateHook
        {
            get
            {
                return this._terminateHook;
            }
        }

        public Hook<ServerCore, String, Listener> AddListenerHook
        {
            get
            {
                return this._addListenerHook;
            }
        }

        public Hook<ServerCore, String> RemoveListenerHook
        {
            get
            {
                return this._removeListenerHook;
            }
        }

        public Hook<ServerCore, String> AddRealmHook
        {
            get
            {
                return this._addRealmHook;
            }
        }

        public Hook<ServerCore, String> RemoveRealmHook
        {
            get
            {
                return this._removeRealmHook;
            }
        }

        public Hook<ServerCore, Storage, String> LoadStorageHook
        {
            get
            {
                return this._loadStorageHook;
            }
        }

        public Hook<ServerCore> UnloadStorageHook
        {
            get
            {
                return this._unloadStorageHook;
            }
        } 

        public Hook<ServerCore, String> ExecuteCodeHook
        {
            get
            {
                return this._executeCodeHook;
            }
        } 

        private readonly ILog _log = LogManager.GetLogger(typeof(ServerCore));

        private readonly AssemblyManager _assemblyManager = new AssemblyManager();

        private readonly Dictionary<String, Listener> _listeners = new Dictionary<String, Listener>();

        private readonly Dictionary<String, Realm> _realms = new Dictionary<String, Realm>();

        private Storage _storage;

        public static DirectoryInfo RootDirectory
        {
            get
            {
                return _rootDirectory;
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
            this._initializeHook.Execute(self =>
            {
                self.Initialize();
            }, this);
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
            this.InitializeHook.Before.Add(self => self.Log.InfoFormat(
                Resources.ServerInitializing,
                Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                Environment.OSVersion.ToString(),
                Thread.CurrentThread.CurrentUICulture.ToString()
            ));
            this.InitializeHook.After.Add(self => self.Log.Info(Resources.ServerInitialized));
            this.StartHook.Before.Add(self => self.Log.Info(Resources.ServerStarting));
            this.StartHook.After.Add(self => self.Log.Info(Resources.ServerStarted));
            this.StopHook.Before.Add(self => self.Log.Info(Resources.ServerStopping));
            this.StopHook.After.Add(self => self.Log.Info(Resources.ServerStopped));
            this.PauseHook.Before.Add(self => self.Log.Info(Resources.ServerPausing));
            this.PauseHook.After.Add(self => self.Log.Info(Resources.ServerPaused));
            this.ResumeHook.Before.Add(self => self.Log.Info(Resources.ServerResuming));
            this.ResumeHook.After.Add(self => self.Log.Info(Resources.ServerResumed));
            this.TerminateHook.Before.Add(self => self.Log.Info(Resources.ServerTerminating));
            this.TerminateHook.After.Add(self => self.Log.Info(Resources.ServerTerminated));
            this.WaitToEndHook.Before.Add(self => self.Log.Info(Resources.ServerWaitingToEnd));
            this.WaitToEndHook.After.Add(self => self.Log.Info(Resources.ServerWaitedToEnd));
            this.AddListenerHook.After.Add((self, id, listener) => self.Log.InfoFormat(
                Resources.ListenerAdded,
                id,
                listener.GetType().AssemblyQualifiedName,
                listener.GetType().Assembly.CodeBase
            ));
            this.RemoveListenerHook.After.Add((self, id) => self.Log.InfoFormat(
                Resources.ListenerRemoved,
                id
            ));
            this.AddRealmHook.After.Add((self, id) => self.Log.InfoFormat(
                Resources.RealmAdded,
                id
            ));
            this.RemoveRealmHook.After.Add((self, id) => self.Log.InfoFormat(
                Resources.RealmRemoved,
                id
            ));
            this.ExecuteCodeHook.After.Add((self, path) => self.Log.InfoFormat(
                Resources.CodeExecuted,
                path
            ));
        }

        public void Start(IDictionary<String, String> arguments)
        {
            this._startHook.Execute(self =>
            {
                IEnumerable<IAsyncResult> asyncResults = self._listeners.Values.Select(l => l.BeginStart(
                    r => (r.AsyncState as Listener).EndStart(r), l
                ));
                WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
            }, this);
        }

        public void Stop()
        {
            this._stopHook.Execute(self =>
            {
                IEnumerable<IAsyncResult> asyncResults = self._listeners.Values.Select(l => l.BeginAbort(
                    r =>
                    {
                        (r.AsyncState as Listener).EndAbort(r);
                        (r.AsyncState as Listener).Stop();
                    }, l
                ));
                WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
            }, this);
        }

        public void StopGracefully()
        {
            this._stopHook.Execute(self =>
            {
                IEnumerable<IAsyncResult> asyncResults = self._listeners.Values.Select(l => l.BeginWait(
                    r =>
                    {
                        (r.AsyncState as Listener).EndWait(r);
                        (r.AsyncState as Listener).Stop();
                    }, l
                ));
                WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
            }, this);
        }

        public void Pause()
        {
            this._pauseHook.Execute(self =>
            {
                IEnumerable<IAsyncResult> asyncResults = self._listeners.Values.Select(l => l.BeginStop(
                    r => (r.AsyncState as Listener).EndStop(r), l
                ));
                WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
            }, this);
        }

        public void Resume()
        {
            this._resumeHook.Execute(self =>
            {
                IEnumerable<IAsyncResult> asyncResults = this._listeners.Values.Select(l => l.BeginStart(
                    r => (r.AsyncState as Listener).EndStart(r), l
                ));
                WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
            }, this);
        }

        public void Dispose()
        {
            this._terminateHook.Execute(self =>
            {
            }, this);
        }

        public void WaitToEnd()
        {
            this._waitToEndHook.Execute(self =>
            {
                IEnumerable<IAsyncResult> asyncResults = self._listeners.Values.Select(l => l.BeginWait(
                    r => (r.AsyncState as Listener).EndStart(r), l
                ));
                WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
            }, this);
        }

        public Listener GetListener(String key)
        {
            return this._listeners[key];
        }

        public void AddListener(String key, Listener listener)
        {
            this._addListenerHook.Execute((self, k, l) =>
            {
                listener.Register(self, k);
                self._listeners.Add(k, l);
            }, this, key, listener);
        }

        public void RemoveListener(String key)
        {
            this._removeListenerHook.Execute((self, k) =>
            {
                self._realms.Remove(k);
            }, this, key);
        }

        public Realm GetRealm(String key)
        {
            return this._realms[key];
        }

        public void AddRealm(String key)
        {
            this._addRealmHook.Execute((self, k) =>
            {
                self._realms.Add(k, new Realm(self, k));
            }, this, key);
        }

        public void RemoveRealm(String key)
        {
            this._removeRealmHook.Execute((self, k) =>
            {
                self._realms.Remove(k);
            }, this, key);
        }

        public void LoadStorage(Storage storage, String connectionString)
        {
            this._loadStorageHook.Execute((self, s, c) =>
            {
                self._storage = s;
                self._storage.Initialize(c);
            }, this, storage, connectionString);
        }

        public void UnloadStorage()
        {
            this._unloadStorageHook.Execute((self =>
            {
                this._storage.Dispose();
                this._storage = null;
            }), this);
        }

        public void ExecuteCode(String path)
        {
            this._executeCodeHook.Execute((self, p) =>
            {
                self._assemblyManager.CreateDomain("__tempScript");
                using (StreamReader reader = new StreamReader(p))
                {
                    this._assemblyManager.Compile("__tempScript", Path.GetExtension(p), reader.ReadToEnd()).EntryPoint.Invoke(null, null);
                }
                self._assemblyManager.UnloadDomain("__tempScript");
            }, this, path);
        }
    }
}