// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
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
using System.Diagnostics;
using XSpect.MetaTweet.Modules;

namespace XSpect.MetaTweet
{
    public sealed class ServerCore
        : MarshalByRefObject,
          IDisposable
    {
        private static readonly DirectoryInfo _rootDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;

        public static DirectoryInfo RootDirectory
        {
            get
            {
                return _rootDirectory;
            }
        }

        public IDictionary<String, String> Parameters
        {
            get;
            private set;
        }

        public AssemblyManager AssemblyManager
        {
            get;
            private set;
        }

        public ILog Log
        {
            get;
            private set;
        }

        public IList<IModule> Modules
        {
            get;
            private set;
        }

        public IEnumerable<FlowModule> Flows
        {
            get;
            private set;
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
            get;
            private set;
        }

        public Hook<ServerCore> StartHook
        {
            get;
            private set;
        }

        public Hook<ServerCore> StopHook
        {
            get;
            private set;
        }

        public Hook<ServerCore> PauseHook
        {
            get;
            private set;
        }

        public Hook<ServerCore> ContinueHook
        {
            get;
            private set;
        }

        public Hook<ServerCore> WaitToEndHook
        {
            get;
            private set;
        }

        public Hook<ServerCore> TerminateHook
        {
            get;
            private set;
        }

        public Hook<ServerCore, String, AssemblyName> LoadAssemblyHook
        {
            get;
            private set;
        }

        public Hook<ServerCore, String> UnloadAssemblyHook
        {
            get;
            private set;
        }

        public Hook<ServerCore, String, Type> LoadModuleHook
        {
            get;
            private set;
        }

        public Hook<ServerCore, String> UnloadModuleHook
        {
            get;
            private set;
        }

        public Hook<ServerCore, String> ExecuteCodeHook
        {
            get;
            private set;
        }

        public ServerCore()
        {
            this.Parameters = new Dictionary<String, String>();
            this.AssemblyManager = new AssemblyManager();
            this.Log = LogManager.GetLogger(typeof(ServerCore));
            this.Modules = new List<IModule>();
            this.InitializeHook = new Hook<ServerCore>();
            this.StartHook = new Hook<ServerCore>();
            this.StopHook = new Hook<ServerCore>();
            this.PauseHook = new Hook<ServerCore>();
            this.ContinueHook = new Hook<ServerCore>();
            this.WaitToEndHook = new Hook<ServerCore>();
            this.TerminateHook = new Hook<ServerCore>();
            this.LoadAssemblyHook = new Hook<ServerCore, String, AssemblyName>();
            this.UnloadAssemblyHook = new Hook<ServerCore, String>();
            this.LoadModuleHook = new Hook<ServerCore, String, Type>();
            this.UnloadModuleHook = new Hook<ServerCore, String>();
            this.ExecuteCodeHook = new Hook<ServerCore, String>();
        }

        /// <summary>
        /// 対象のインスタンスの有効期間ポリシーを制御する、有効期間サービス オブジェクトを取得します。
        /// </summary>
        /// <returns>
        /// 対象のインスタンスの有効期間ポリシーを制御するときに使用する、<see cref="T:System.Runtime.Remoting.Lifetime.ILease"/> 型のオブジェクト。存在する場合は、このインスタンスの現在の有効期間サービス オブジェクトです。それ以外の場合は、<see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime"/> プロパティの値に初期化された新しい有効期間サービス オブジェクトです。
        /// </returns>
        /// <exception cref="T:System.Security.SecurityException">直前の呼び出し元に、インフラストラクチャ アクセス許可がありません。</exception>
        /// <PermissionSet>
        /// <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure"/>
        /// </PermissionSet>
        public override Object InitializeLifetimeService()
        {
            return null;
        }

        public void Initialize(IDictionary<String, String> arguments)
        {
            this.Parameters = arguments;

            if (this.Parameters.ContainsKey("debug") && this.Parameters["debug"] == "true")
            {
                Debugger.Launch();
                Debugger.Break();
            }

            this.InitializeDefaultCompilerSettings();
            this.InitializeDefaultLogHooks();
            this.ExecuteCode(RootDirectory.GetFiles("init.*").Single().FullName);
            this.ExecuteCode(RootDirectory.GetFiles("rc.*").Single().FullName);
        }

        private void InitializeDefaultCompilerSettings()
        {
            this.AssemblyManager.DefaultOptions.Add("CompilerVersion", "v3.5");
            this.AssemblyManager.DefaultParameters.GenerateExecutable = false;
            this.AssemblyManager.DefaultParameters.IncludeDebugInformation = true;
            this.AssemblyManager.DefaultParameters.ReferencedAssemblies.AddRange(new String[]
            {
                typeof(System.Object).Assembly.Location,            // mscorlib
                typeof(System.Uri).Assembly.Location,               // System
                typeof(System.Linq.Enumerable).Assembly.Location,   // System.Core
                typeof(System.Data.DataSet).Assembly.Location,      // System.Data
                typeof(System.Xml.XmlDocument).Assembly.Location,   // System.Xml
                typeof(XSpect.Random).Assembly.Location,            // XSpectCommonFramework
                Assembly.GetExecutingAssembly().Location,           // MetaTweetServer
                typeof(XSpect.MetaTweet.Storage).Assembly.Location, // MetaTweetObjectModel
                typeof(log4net.ILog).Assembly.Location,             // log4net
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

        public void Start()
        {
            this.StartHook.Execute(self =>
            {
                if (self.Servants.Any())
                {
                    IEnumerable<IAsyncResult> asyncResults = self.Servants.Select(l => l.BeginStart(
                        r => (r.AsyncState as ServantModule).EndStart(r), l
                    ));
                    WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
                }
            }, this);
        }

        public void Stop()
        {
            this.StopHook.Execute(self =>
            {
                if (self.Servants.Any())
                {
                    IEnumerable<IAsyncResult> asyncResults = self.Servants.Select(l => l.BeginAbort(
                        r =>
                        {
                            (r.AsyncState as ServantModule).EndAbort(r);
                            (r.AsyncState as ServantModule).Stop();
                        }, l
                    ));
                    WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
                }
            }, this);
        }

        public void StopGracefully()
        {
            this.StopHook.Execute(self =>
            {
                if (self.Servants.Any())
                {
                    IEnumerable<IAsyncResult> asyncResults = self.Servants.Select(l => l.BeginWait(
                        r =>
                        {
                            (r.AsyncState as ServantModule).EndWait(r);
                            (r.AsyncState as ServantModule).Stop();
                        }, l
                    ));
                    WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
                }
            }, this);
        }

        public void Pause()
        {
            this.PauseHook.Execute(self =>
            {
                if (self.Servants.Any())
                {
                    IEnumerable<IAsyncResult> asyncResults = self.Servants.Select(l => l.BeginPause(
                        r => (r.AsyncState as ServantModule).EndPause(r), l
                    ));
                    WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
                }
            }, this);
        }

        public void Continue()
        {
            this.ContinueHook.Execute(self =>
            {
                if (self.Servants.Any())
                {
                    IEnumerable<IAsyncResult> asyncResults = this.Servants.Select(l => l.BeginContinue(
                        r => (r.AsyncState as ServantModule).EndContinue(r), l
                    ));
                    WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
                }
            }, this);
        }

        public void Dispose()
        {
            this.TerminateHook.Execute(self =>
            {
            }, this);
        }

        public void WaitToEnd()
        {
            this.WaitToEndHook.Execute(self =>
            {
                if (self.Servants.Any())
                {
                    IEnumerable<IAsyncResult> asyncResults = self.Servants.Select(l => l.BeginWait(
                        r => (r.AsyncState as ServantModule).EndStart(r), l
                    ));
                    WaitHandle.WaitAll(asyncResults.Select(r => r.AsyncWaitHandle).ToArray());
                }
            }, this);
        }

        public void LoadAssembly(String name, AssemblyName assemblyRef)
        {
            this.LoadAssemblyHook.Execute((self, n, r) =>
            {
                self.AssemblyManager.CreateDomain(n);
                self.AssemblyManager.LoadAssembly(n, r);
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
                IModule[] modules = self.Modules
                    .Where(e => e.GetType().Assembly == this.AssemblyManager[n].GetAssemblies().Last())
                    .ToArray();

                // TODO: Write more smartly.
                for (Int32 idx = 0; idx < modules.Length; ++idx)
                {
                    self.UnloadModule(modules[idx]);
                }
                self.AssemblyManager.UnloadDomain(n);
            }, this, name);
        }

        public void LoadModule(String key, Type type)
        {
            this.LoadModuleHook.Execute((self, k, t) =>
            {
                if ((typeof(IModule).IsSubclassOf(t)))
                {
                    throw new ArgumentException("type");
                }
                IModule module = Activator.CreateInstance(t) as IModule;
                module.Host = self;
                module.Name =  module.ModuleType + ":" + k;
                self.Modules.Add(module);
            }, this, key, type);
        }

        public void LoadModule(String assemblyKey, String key, String type)
        {
            this.LoadModule(key, this.AssemblyManager[assemblyKey].GetAssemblies().Last().GetType(type, true));
        }

        public void UnloadModule(String key)
        {
            this.UnloadModuleHook.Execute((self, k) =>
            {
                IModule module = self.Modules.Single(m => m.Name == k);
                module.Host = null;
                module.Name = null;
                self.Modules.Remove(module);
            }, this, key);
        }

        public void UnloadModule(IModule module)
        {
            this.UnloadModule(module.ModuleType + ":" + module.Name);
        }

        public IModule GetModule(String key)
        {
            return this.Modules.Single(m => m.Name == key);
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
                if (prefixes.Contains("!"))
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
            this.ExecuteCodeHook.Execute((self, p) =>
            {
                String name = "__temp_" + Guid.NewGuid().ToString();
                self.AssemblyManager.CreateDomain(name);
                using (StreamReader reader = new StreamReader(p))
                {
                    this.AssemblyManager.Compile(name, Path.GetExtension(p), reader.ReadToEnd())
                        .GetTypes()
                        .SelectMany(t => t.GetMethods(
                            BindingFlags.NonPublic |
                            BindingFlags.Public |
                            BindingFlags.Static
                        ))
                        .Single(m => m.GetParameters()
                            .Select(a => a.ParameterType)
                            .SequenceEqual(new Type[]
                            {
                                typeof(ServerCore),
                                typeof(IDictionary<String, String>),
                            })
                        )
                        .Invoke(null, new Object[]
                        {
                            self,
                            self.Parameters,
                        });
                }
                self.AssemblyManager.UnloadDomain(name);
            }, this, path);
        }
    }
}