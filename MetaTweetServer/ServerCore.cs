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
        
        private readonly Hook<ServerCore, String> _executeCodeHook = new Hook<ServerCore, String>();

        private readonly ILog _log = LogManager.GetLogger(typeof(ServerCore));

        private readonly AssemblyManager _assemblyManager = new AssemblyManager();

        private readonly Dictionary<String, Module> _modules = new Dictionary<String, Module>();

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

        public IEnumerable<Module> Modules
        {
            get
            {
                return this._modules.Values;
            }
        }

        public IEnumerable<FlowModule> Flows
        {
            get
            {
                return this.Modules.OfType<FlowModule>();
            }
        }

        public IEnumerable<InputFlowModule> Inputs
        {
            get
            {
                return this.Modules.OfType<InputFlowModule>();
            }
        }

        public IEnumerable<FilterFlowModule> Filters
        {
            get
            {
                return this.Modules.OfType<FilterFlowModule>();
            }
        }

        public IEnumerable<OutputFlowModule> Outputs
        {
            get
            {
                return this.Modules.OfType<OutputFlowModule>();
            }
        }

        public IEnumerable<ServantModule> Servants
        {
            get
            {
                return this.Modules.OfType<ServantModule>();
            }
        }

        public IEnumerable<StorageModule> Storages
        {
            get
            {
                return this.Modules.OfType<StorageModule>();
            }
        }

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

        public Hook<ServerCore, String> ExecuteCodeHook
        {
            get
            {
                return this._executeCodeHook;
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
                typeof(System.Object).Assembly.Location,          // mscorlib
                typeof(System.Uri).Assembly.Location,             // System
                typeof(System.Linq.Enumerable).Assembly.Location, // System.Core
                typeof(System.Xml.XmlDocument).Assembly.Location, // System.Xml
                typeof(XSpect.Random).Assembly.Location,          // XSpectCommonFramework
                Assembly.GetExecutingAssembly().Location,         // MetaTweetServer
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
        }

        public void Start(IDictionary<String, String> arguments)
        {
            this._startHook.Execute(self =>
            {
                IEnumerable<IAsyncResult> asyncResults = self.Servants.Select(l => l.BeginStart(
                    r => (r.AsyncState as ServantModule).EndStart(r), l
                ));
                WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
            }, this);
        }

        public void Stop()
        {
            this._stopHook.Execute(self =>
            {
                IEnumerable<IAsyncResult> asyncResults = self.Servants.Select(l => l.BeginAbort(
                    r =>
                    {
                        (r.AsyncState as ServantModule).EndAbort(r);
                        (r.AsyncState as ServantModule).Stop();
                    }, l
                ));
                WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
            }, this);
        }

        public void StopGracefully()
        {
            this._stopHook.Execute(self =>
            {
                IEnumerable<IAsyncResult> asyncResults = self.Servants.Select(l => l.BeginWait(
                    r =>
                    {
                        (r.AsyncState as ServantModule).EndWait(r);
                        (r.AsyncState as ServantModule).Stop();
                    }, l
                ));
                WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
            }, this);
        }

        public void Pause()
        {
            this._pauseHook.Execute(self =>
            {
                IEnumerable<IAsyncResult> asyncResults = self.Servants.Select(l => l.BeginStop(
                    r => (r.AsyncState as ServantModule).EndStop(r), l
                ));
                WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
            }, this);
        }

        public void Resume()
        {
            this._resumeHook.Execute(self =>
            {
                IEnumerable<IAsyncResult> asyncResults = this.Servants.Select(l => l.BeginStart(
                    r => (r.AsyncState as ServantModule).EndStart(r), l
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
                IEnumerable<IAsyncResult> asyncResults = self.Servants.Select(l => l.BeginWait(
                    r => (r.AsyncState as ServantModule).EndStart(r), l
                ));
                WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
            }, this);
        }

        public void LoadAssembly(String key, AssemblyName assemblyRef)
        {
            this._assemblyManager.CreateDomain(key);
            this._assemblyManager.LoadAssembly(key, assemblyRef);
        }

        public void UnloadAssembly(String key)
        {
            foreach (String k in this._modules
                .Where(p => p.Value.GetType().Assembly == this._assemblyManager[key].GetAssemblies().Single())
                .Select(p => p.Key)
            )
            {
                this.Unload(k);
            }
            this._assemblyManager.UnloadDomain(key);
        }

        public void Load(String key, Type type)
        {
            if (!type.IsSubclassOf(typeof(Module)))
            {
                throw new ArgumentException("type");
            }
            this._modules.Add(key, Activator.CreateInstance(type) as Module);
        }

        public void Unload(String key)
        {
            this._modules.Remove(key);
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