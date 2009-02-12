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

        private readonly Dictionary<String, Dictionary<Tuple<Type, String>, IModule>> _modules;

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

        public Hook<ModuleManager, String, Type, String> RemoveHook
        {
            get;
            private set;
        }

        public ModuleManager(ServerCore parent)
        {
            this.Parent = parent;
            this._assemblyManager = new AssemblyManager();
            this._modules = new Dictionary<String, Dictionary<Tuple<Type, String>, IModule>>();
            this.LoadHook = new Hook<ModuleManager, String>();
            this.ExecuteHook = new Hook<ModuleManager, String, FileInfo>();
            this.UnloadHook = new Hook<ModuleManager, String>();
            this.AddHook = new Hook<ModuleManager, String, String, String>();
            this.RemoveHook = new Hook<ModuleManager, String, Type, String>();
            this.Initialize();
        }

        public virtual void Dispose()
        {
            foreach (String domain in this._modules.Keys.ToArray())
            {
                this.Unload(domain);
            }
        }

        protected virtual void Initialize()
        {
            this._assemblyManager.DefaultAppDomainSetup.ApplicationBase
                = ServerCore.RootDirectory.FullName;
            this._assemblyManager.DefaultAppDomainSetup.ApplicationName = "ModuleManager";
            this._assemblyManager.DefaultAppDomainSetup.CachePath
                = CacheDirectory.FullName;
            this._assemblyManager.DefaultAppDomainSetup.DynamicBase
                = this._assemblyManager.DefaultAppDomainSetup.CachePath;
            this._assemblyManager.DefaultAppDomainSetup.LoaderOptimization = LoaderOptimization.MultiDomainHost;
            this._assemblyManager.DefaultAppDomainSetup.PrivateBinPath = String.Join(";", new String[]
            {
                ServerCore.RootDirectory.FullName,
                ModuleDirectory.FullName,
            });
            this._assemblyManager.DefaultAppDomainSetup.PrivateBinPathProbe = "true";
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

        public virtual void Load(String domain)
        {
            this.LoadHook.Execute((self, domain_) =>
            {
                FileInfo moduleFile = ModuleDirectory.GetFiles(Path.ChangeExtension(domain_, ".dll")).Single();
                FileInfo moduleSymbolStoreFile = new FileInfo(Path.ChangeExtension(moduleFile.FullName, ".pdb"));
                self._assemblyManager.Load(
                    domain_,
                    moduleFile,
                    moduleSymbolStoreFile.Exists ? moduleSymbolStoreFile : null
                );
                this._modules.Add(domain_, new Dictionary<Tuple<Type, String>, IModule>());
            }, this, domain);
        }

        public virtual void Execute(String domain, FileInfo file)
        {
            this.ExecuteHook.Execute((self, domain_, file_) =>
            {
                Assembly assembly = self._assemblyManager.Compile(domain_, file_);
                if (assembly.GetTypes().Any(t => t.IsAssignableFrom(typeof(IModule))))
                {
                    self._modules.Add(domain_, new Dictionary<Tuple<Type, String>, IModule>());
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
                        self.Parent,
                        self.Parent.Parameters,
                    });
                    self._assemblyManager.Unload(domain_);
                }
            }, this, domain, file);
        }

        public virtual void Execute(FileInfo file)
        {
            this.Execute(Guid.NewGuid().ToString("n"), file);
        }

        public virtual void Execute(String domain, String path)
        {
            this.Execute(domain, new FileInfo(path));
        }

        public virtual void Execute(String path)
        {
            this.Execute(null, new FileInfo(path));
        }
        
        public virtual void Unload(String domain)
        {
            this.UnloadHook.Execute((self, domain_) =>
            {
                self._modules[domain].ForEach(p => p.Value.Dispose());
                self._modules.Remove(domain_);
                self._assemblyManager.Unload(domain_);
            }, this, domain);
        }

        public virtual TModule Add<TModule>(String domain, String key, String typeName)
            where TModule : IModule
        {
            return (TModule) this.Add(domain, key, typeName);
        }

        public virtual IModule Add(String domain, String key, String typeName)
        {
            return this.AddHook.Execute((self, domain_, key_, typeName_) =>
            {
                IModule module = self._assemblyManager[domain_].CreateInstance(typeName_) as IModule;
                self._modules[domain_].Add(Make.Tuple(module.GetType(), key_), module);
                module.Register(self.Parent, key_);
                return module;
            }, this, domain, key, typeName);
        }

        protected virtual void Remove(String domain, Type type, String key)
        {
            this.RemoveHook.Execute((self, domain_, type_, key_) =>
            {
                IModule module = self.GetModule(domain_, type_, key_);
                self._modules[domain_].Remove(Make.Tuple(type_, key_));
                module.Dispose();
            }, this, domain, type, key);
        }

        public virtual void Remove<TModule>(String domain, String key)
            where TModule : IModule
        {
            this.Remove(domain, typeof(TModule), key);
        }

        public virtual void Remove(String domain, String key)
        {
            Type type = this.GetModule(domain, key).GetType();
            this.Remove(domain, type, key);
        }

        protected virtual IEnumerable<IModule> GetModules(String domain, Type type, String key)
        {
            var x = this._modules.SelectMany(p => p.Value);
            return (domain != null
                ? this._modules[domain]
                : this._modules.SelectMany(p => p.Value)
            )
                .Where(
                    p => p.Key.Item1.IsSubclassOf(type != null ? type : p.Key.Item1) &&
                         p.Key.Item2 == (key != null ? key : p.Key.Item2)
                 )
                .Select(p => p.Value);
        }

        public IEnumerable<TModule> GetModules<TModule>(String domain, String key)
        {
            return this.GetModules(domain, typeof(TModule), key)
                .OfType<TModule>();
        }

        public IEnumerable<TModule> GetModules<TModule>(String key)
        {
            return this.GetModules(null, typeof(TModule), key)
                .OfType<TModule>();
        }

        public IEnumerable<TModule> GetModules<TModule>()
        {
            return this.GetModules(null, typeof(TModule), null)
                .OfType<TModule>();
        }

        public IEnumerable<IModule> GetModules(String domain, String key)
        {
            return this.GetModules(domain, null, key);
        }

        public IEnumerable<IModule> GetModules(String key)
        {
            return this.GetModules(null, null, key);
        }

        public IEnumerable<IModule> GetModules()
        {
            return this.GetModules(null, null, null);
        }

        protected IModule GetModule(String domain, Type type, String key)
        {
            return this.GetModules(domain, type, key).Single();
        }

        public TModule GetModule<TModule>(String domain, String key)
        {
            return this.GetModules<TModule>(domain, key).Single();
        }

        public TModule GetModule<TModule>(String key)
        {
            return this.GetModules<TModule>(key).Single();
        }

        public IModule GetModule(String domain, String key)
        {
            return this.GetModules(domain, key).Single();
        }

        public IModule GetModule(String key)
        {
            return this.GetModules(key).Single();
        }
    }
}