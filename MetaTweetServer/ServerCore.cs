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
using System.Text.RegularExpressions;
using XSpect.MetaTweet.ObjectModel;

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
        
        private readonly Hook<ServerCore> _continueHook = new Hook<ServerCore>();
        
        private readonly Hook<ServerCore> _waitToEndHook = new Hook<ServerCore>();
        
        private readonly Hook<ServerCore> _terminateHook = new Hook<ServerCore>();

        private readonly Hook<ServerCore, String, AssemblyName> _loadAssemblyHook = new Hook<ServerCore, String, AssemblyName>();

        private readonly Hook<ServerCore, String> _unloadAssemblyHook = new Hook<ServerCore, String>();

        private readonly Hook<ServerCore, String, Type> _loadModuleHook = new Hook<ServerCore, String, Type>();

        private readonly Hook<ServerCore, String> _unloadModuleHook = new Hook<ServerCore, String>();

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

        public Hook<ServerCore> ContinueHook
        {
            get
            {
                return this._continueHook;
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

        public Hook<ServerCore, String, AssemblyName> LoadAssemblyHook
        {
            get
            {
                return this._loadAssemblyHook;
            }
        }

        public Hook<ServerCore, String> UnloadAssemblyHook
        {
            get
            {
                return this._unloadAssemblyHook;
            }
        }

        public Hook<ServerCore, String, Type> LoadModuleHook
        {
            get
            {
                return this._loadModuleHook;
            }
        }
        
        public Hook<ServerCore, String> UnloadModuleHook
        {
            get
            {
                return this._unloadModuleHook;
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
            //this.ExecuteCode(RootDirectory.GetFiles("init.*").Single().FullName);
            //this.ExecuteCode(RootDirectory.GetFiles("rc.*").Single().FullName);
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
            this.ContinueHook.Before.Add(self => self.Log.Info(Resources.ServerResuming));
            this.ContinueHook.After.Add(self => self.Log.Info(Resources.ServerResumed));
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
                IEnumerable<IAsyncResult> asyncResults = self.Servants.Select(l => l.BeginPause(
                    r => (r.AsyncState as ServantModule).EndPause(r), l
                ));
                WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
            }, this);
        }

        public void Continue()
        {
            this._continueHook.Execute(self =>
            {
                IEnumerable<IAsyncResult> asyncResults = this.Servants.Select(l => l.BeginContinue(
                    r => (r.AsyncState as ServantModule).EndContinue(r), l
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

        public void LoadAssembly(String name, AssemblyName assemblyRef)
        {
            this.LoadAssemblyHook.Execute((self, n, r) =>
            {
                self._assemblyManager.CreateDomain(n);
                self._assemblyManager.LoadAssembly(n, r);
            }, this, name, assemblyRef);
        }

        public void LoadAssembly(String name, String assemblyFile)
        {
            this.LoadAssembly(name, AssemblyName.GetAssemblyName(assemblyFile));
        }

        public void UnloadAssembly(String name)
        {
            this.UnloadAssemblyHook.Execute((self, n) =>
            {
                String[] keys = self._modules
                    .Where(p => p.Value.GetType().Assembly == this._assemblyManager[n].GetAssemblies().Last())
                    .Select(p => p.Key).ToArray();

                // TODO: Write more smartly.
                for (Int32 idx = 0; idx < keys.Length; ++idx)
                {
                    self.UnloadModule(keys[idx]);
                }
                self._assemblyManager.UnloadDomain(n);
            }, this, name);
        }

        public void LoadModule(String key, Type type)
        {
            this.LoadModuleHook.Execute((self, k, t) =>
            {
                if (!t.IsSubclassOf(typeof(Module)))
                {
                    throw new ArgumentException("type");
                }
                Module module = Activator.CreateInstance(t) as Module;
                self._modules.Add((!k.Contains(":") ? module.ModuleType + ":" : String.Empty) + k, module);
                module.Register(self, k);
            }, this, key, type);
        }

        public void LoadModule(String key, String type)
        {
            this.LoadModule(key, this._assemblyManager[key].GetAssemblies().Last().GetType(type, true));
        }

        public void UnloadModule(String key)
        {
            this.UnloadModuleHook.Execute((self, k) =>
            {
                self._modules[k].Unregister();
                self._modules.Remove(k);
            }, this, key);
        }

        public void UnloadModule(Module module)
        {
            this.UnloadModule(module.ModuleType + ":" + module.Name);
        }

        public Module GetModule(String key)
        {
            return this._modules[key];
        }

        public InputFlowModule GetInput(String name)
        {
            return this.GetModule(InputFlowModule.ModuleTypeString + ":" + name) as InputFlowModule;
        }

        public FilterFlowModule GetFilter(String name)
        {
            return this.GetModule(FilterFlowModule.ModuleTypeString + ":" + name) as FilterFlowModule;
        }

        public OutputFlowModule GetOutput(String name)
        {
            return this.GetModule(OutputFlowModule.ModuleTypeString + ":" + name) as OutputFlowModule;
        }

        public ServantModule GetServant(String name)
        {
            return this.GetModule(ServantModule.ModuleTypeString + ":" + name) as ServantModule;
        }

        public StorageModule GetStorage(String name)
        {
            return this.GetModule(StorageModule.ModuleTypeString + ":" + name) as StorageModule;
        }

        public T Request<T>(Uri uri)
        {
            String src = uri.AbsolutePath;
            if (src[src.LastIndexOf('/') + 1] != '.')
            {
                // example.ext?foo=bar -> example?foo=bar/!/.ext
                src = Regex.Replace(src, @"(\.[^?]*)(\?.*)?$", @"$2/!/$1");
            }

            String[] units = Regex.Split(src, "/[!$]");
            Int32 index = 0;
            IEnumerable<StorageObject> results = null;
            String storage = "main"; // Default Storage
            String module = "sys";   // Default Module

            // a) .../$storage!module/... -> storage!module/...
            // b) .../$storage!/...       -> storage!/...
            // c) .../!module/...         -> module/...
            // d) .../!/...               -> /...
            foreach (String elem in units)
            {
                ++index;

                String prefixes = elem.Substring(0, elem.IndexOf('/') - 1);
                if (prefixes.Contains('!'))
                {
                    if (!prefixes.EndsWith("!")) // a) Specified Storage and Module
                    {
                        String[] prefixArray = prefixes.Split('!');
                        storage = prefixArray[0];
                        module = prefixArray[1];
                    }
                    else // b) Specified Storage
                    {
                        storage = prefixes.TrimEnd('!');
                        // Module is taken over.
                    }
                }
                else
                {
                    if (prefixes != String.Empty) // c) Specified Module
                    {
                        // Storage is taken over.
                        module = prefixes;
                    }
                    else // d) Specified nothing
                    {
                        // Do nothing; Storage and Module are taken over.
                    }
                }

                String selector = elem.Substring(prefixes.Length, elem.IndexOf('?') - prefixes.Length);
                String arguments = elem.Substring(prefixes.Length + selector.Length);
                Dictionary<String, String> argumentDictionary = new Dictionary<String, String>();
                foreach (String[] pair in arguments
                    .TrimStart('?')
                    .Split('&')
                    .Select(s => s.Split('='))
                )
                {
                    argumentDictionary.Add(pair[0], pair[1]);
                }


                if (index == 0) // Invoking InputFlowModule
                {
                    results = this.GetInput(module).Input(selector, this.GetStorage(storage), argumentDictionary);
                }
                else if (index != units.Length - 1) // Invoking FilterFlowModule
                {
                    this.GetFilter(module).Filter(selector, results, this.GetStorage(storage), argumentDictionary);
                }
                else // Invoking OutputFlowModule
                {
                    return this.GetOutput(module).Output<T>(selector, results, this.GetStorage(storage), argumentDictionary);
                }
            }

            // Throws when not returned yet (it means Output module is not invoked.)
            throw new ArgumentException("uri");
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