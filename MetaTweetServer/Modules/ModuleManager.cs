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
using XSpect.Reflection;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using XSpect.Extension;
using System.IO;
using Achiral.Extension;
using Achiral;

namespace XSpect.MetaTweet.Modules
{
    public class ModuleManager
        : MarshalByRefObject,
          IDisposable
    {
        private static readonly DirectoryInfo _moduleDirectory = ServerCore.RootDirectory.SubDirectoryOf("module");

        private static readonly DirectoryInfo _cacheDirectory = ServerCore.RootDirectory.SubDirectoryOf("cache");

        private readonly Dictionary<String, ICollection<KeyValuePair<String, IModule>>> _modules;

        private readonly AssemblyManager _assemblyManager;

        public static DirectoryInfo ModuleDirectory
        {
            get
            {
                return _moduleDirectory;
            }
        }

        public static DirectoryInfo CacheDirectory
        {
            get
            {
                return _cacheDirectory;
            }
        }

        public ServerCore Parent
        {
            get;
            private set;
        }

        public Hook<ModuleManager, String> LoadHook
        {
            get;
            private set;
        }

        public Hook<ModuleManager, String, FileInfo> ExecuteHook
        {
            get;
            private set;
        }

        public Hook<ModuleManager, String> UnloadHook
        {
            get;
            private set;
        }

        public Hook<ModuleManager, String, String, String> AddHook
        {
            get;
            private set;
        }

        public Hook<ModuleManager, String, String> RemoveHook
        {
            get;
            private set;
        }

        public ModuleManager(ServerCore parent)
        {
            this.Parent = parent;
            this._assemblyManager = new AssemblyManager();
            this._modules = new Dictionary<String, ICollection<KeyValuePair<String, IModule>>>();
            this.LoadHook = new Hook<ModuleManager, String>();
            this.ExecuteHook = new Hook<ModuleManager, String, FileInfo>();
            this.UnloadHook = new Hook<ModuleManager, String>();
            this.AddHook = new Hook<ModuleManager, String, String, String>();
            this.RemoveHook = new Hook<ModuleManager, String, String>();
        }

        public virtual void Dispose()
        {
            foreach (String moduleName in this._modules.Keys.ToArray())
            {
                this.Unload(moduleName);
            }
        }

        protected virtual void Initialize()
        {
            this._assemblyManager.DefaultAppDomainSetup.ApplicationBase
                = ModuleDirectory.FullName;
            this._assemblyManager.DefaultAppDomainSetup.ApplicationName = "MetaTweet Server, Module Manager";
            this._assemblyManager.DefaultAppDomainSetup.CachePath
                = CacheDirectory.FullName;
            this._assemblyManager.DefaultAppDomainSetup.DynamicBase
                = this._assemblyManager.DefaultAppDomainSetup.CachePath;
            this._assemblyManager.DefaultAppDomainSetup.LoaderOptimization = LoaderOptimization.MultiDomainHost;
            this._assemblyManager.DefaultAppDomainSetup.ShadowCopyFiles = "true";
            this._assemblyManager.DefaultOptions.Add("CompilerVersion", "v3.5");
            this._assemblyManager.DefaultParameters.GenerateExecutable = false;
            this._assemblyManager.DefaultParameters.IncludeDebugInformation = true;
            this._assemblyManager.DefaultParameters.ReferencedAssemblies.AddRange(new String[]
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

        public virtual void Load(String moduleName)
        {
            this.LoadHook.Execute((self, moduleName_) =>
            {
                FileInfo moduleFile = ModuleDirectory.GetFiles(moduleName_).Single();
                FileInfo moduleSymbolStoreFile = new FileInfo(Path.ChangeExtension(moduleFile.FullName, ".pdb"));
                self._assemblyManager.Load(
                    moduleName_,
                    moduleFile,
                    moduleSymbolStoreFile.Exists ? moduleSymbolStoreFile : null
                );
                this._modules.Add(moduleName_, new Dictionary<String, IModule>());
            }, this, moduleName);
        }

        public virtual void Execute(String moduleName, FileInfo file)
        {
            this.ExecuteHook.Execute((self, moduleName_, file_) =>
            {
                Assembly assembly = self._assemblyManager.Compile(moduleName_, file_);
                if (assembly.GetTypes().Any(t => t.IsAssignableFrom(typeof(IModule))))
                {
                    self._modules.Add(moduleName_, new Dictionary<String, IModule>());
                }
                else
                {
                    assembly.GetTypes()
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
                        self.Parent.Parameters,
                    });
                }
            }, this, moduleName, file);
        }

        public virtual void Execute(FileInfo file)
        {
            this.Execute(null, file);
        }

        public virtual void Execute(String moduleName, String path)
        {
            this.Execute(moduleName, new FileInfo(path));
        }

        public virtual void Execute(String path)
        {
            this.Execute(null, new FileInfo(path));
        }
        
        public virtual void Unload(String moduleName)
        {
            this.UnloadHook.Execute((self, moduleName_) =>
            {
                self._modules[moduleName].ForEach(p => p.Value.Dispose());
                self._modules.Remove(moduleName_);
                self._assemblyManager.Unload(moduleName_);
            }, this, moduleName);
        }

        public virtual IModule Add(String moduleName, String key, String typeName)
        {
            return this.AddHook.Execute((self, moduleName_, key_, typeName_) =>
            {
                IModule module = self._assemblyManager[moduleName_].CreateInstance(typeName_) as IModule;
                self._modules[moduleName_].Add(key_, module);
                module.Register(self.Parent, key_);
                return module;
            }, this, moduleName, key, typeName);
        }

        public virtual void Remove(String moduleName, String key)
        {
            this.RemoveHook.Execute((self, moduleName_, key_) =>
            {
                IModule module = self.GetModule(moduleName_, key_);
                self._modules[moduleName_].Remove(new KeyValuePair<String, IModule>(key_, module));
                module.Dispose();
            }, this, moduleName, key);
        }

        public virtual IEnumerable<TModule> GetModules<TModule>(String moduleName, String key)
            where TModule : IModule
        {
            return (moduleName != null
                ? this._modules[moduleName]
                : this._modules.SelectMany(p => p.Value)
            )
                .Where(p => p.Key == (key ?? p.Key))
                .OfType<TModule>();
        }

        public IEnumerable<TModule> GetModules<TModule>(String key)
            where TModule : IModule
        {
            return this.GetModules<TModule>(null, key);
        }

        public IEnumerable<TModule> GetModules<TModule>()
            where TModule : IModule
        {
            return this.GetModules<TModule>(null, null);
        }

        public IEnumerable<IModule> GetModules(String moduleName, String key)
        {
            return this.GetModules<IModule>(moduleName, key);
        }

        public IEnumerable<IModule> GetModules(String key)
        {
            return this.GetModules<IModule>(key);
        }

        public IEnumerable<IModule> GetModules()
        {
            return this.GetModules<IModule>();
        }

        public TModule GetModule<TModule>(String moduleName, String key)
            where TModule : IModule
        {
            return this.GetModules<TModule>(moduleName, key).Single();
        }

        public TModule GetModule<TModule>(String key)
            where TModule : IModule
        {
            return this.GetModules<TModule>(key).Single();
        }

        public IModule GetModule(String moduleName, String key)
        {
            return this.GetModules(moduleName, key).Single();
        }

        public IModule GetModule(String key)
        {
            return this.GetModules(key).Single();
        }
    }
}