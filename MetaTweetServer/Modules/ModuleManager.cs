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
using XSpect.Configuration;
using System.CodeDom.Compiler;

namespace XSpect.MetaTweet.Modules
{
    /// <summary>
    /// モジュールの追加と削除および管理を行う機能を提供します。
    /// </summary>
    /// <remarks>
    /// <para>モジュール (モジュール オブジェクト) とは <see cref="IModule"/> を実装する型、またはそれを 1 以上持つアセンブリ (モジュール アセンブリ) を指します。このクラスは、モジュールの動的なロード、アンロード、および検索の機能を提供します。</para>
    /// <para>モジュールはモジュール アセンブリを識別する名前 (ドメイン)、モジュール オブジェクトの型、およびモジュール オブジェクトを識別する名前 (キー) の 3 つによって一意に識別されます。即ち、同一のドメインにおいて、モジュールオブジェクトの型が異なれば、同じキーを使用することが可能です。</para>
    /// </remarks>
    public class ModuleManager
        : MarshalByRefObject,
          IDisposable
    {
        private readonly Dictionary<String, Dictionary<Tuple<Type, String>, IModule>> _modules;

        private readonly AssemblyManager _assemblyManager;

        /// <summary>
        /// モジュールを検索する起点となるディレクトリを取得します。
        /// </summary>
        /// <value>
        /// モジュールが配置されている、検索の起点となるディレクトリ。
        /// </value>
        /// <remarks>
        /// 指定されているディレクトリが存在しない場合、新規に作成されます。
        /// </remarks>
        public DirectoryInfo ModuleDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// モジュールのシャドウコピーを保持するディレクトリを取得します。
        /// </summary>
        /// <value>
        /// モジュールのシャドウコピーを保持するディレクトリ。
        /// </value>
        /// <remarks>
        /// <para>モジュールは読み出される際にこのプロパティで示されるディレクトリにキャッシュされます。</para>
        /// <para>指定されているディレクトリが存在しない場合、新規に作成されます。</para>
        /// </remarks>
        public DirectoryInfo CacheDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// モジュールの設定ファイルが配置されているディレクトリを取得します。
        /// </summary>
        /// <value>
        /// モジュールの設定ファイルが配置されているディレクトリ。
        /// </value>
        /// <remarks>
        /// 指定されているディレクトリが存在しない場合、新規に作成されます。
        /// </remarks>
        public DirectoryInfo ConfigDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// 一時ファイルが生成されるディレクトリを取得します。
        /// </summary>
        /// <value>
        /// 一時ファイルが生成されるディレクトリ。
        /// </value>
        /// <remarks>
        /// 指定されているディレクトリが存在しない場合、新規に作成されます。
        /// </remarks>
        public DirectoryInfo TempDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// このオブジェクトがホストされているサーバ オブジェクトを取得します。
        /// </summary>
        /// <value>
        /// このオブジェクトがホストされているサーバ オブジェクト。
        /// </value>
        /// <remarks>
        /// 指定されているディレクトリが存在しない場合、新規に作成されます。
        /// </remarks>
        public ServerCore Parent
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="Load"/> のフック リストを取得します。
        /// </summary>
        /// <value><see cref="Load"/> のフック リスト。</value>
        public Hook<ModuleManager, String> LoadHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="Execute(String, FileInfo)"/> のフック リストを取得します。
        /// </summary>
        /// <value><see cref="Execute(String, FileInfo)"/> のフック リスト。</value>
        public Hook<ModuleManager, String, FileInfo> ExecuteHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="Unload"/> のフック リストを取得します。
        /// </summary>
        /// <value><see cref="Unload"/> のフック リスト。</value>
        public Hook<ModuleManager, String> UnloadHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="Add(String, String, String, FileInfo)"/> のフック リストを取得します。
        /// </summary>
        /// <value><see cref="Add(String, String, String, FileInfo)"/> のフック リスト。</value>
        public Hook<ModuleManager, String, String, String, FileInfo> AddHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="Remove(String, Type, String)"/> のフック リストを取得します。
        /// </summary>
        /// <value><see cref="Remove(String, Type, String)"/> のフック リスト。</value>
        public Hook<ModuleManager, String, Type, String> RemoveHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="ModuleManager"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="parent">ホストされるサーバ オブジェクト。</param>
        /// <param name="moduleDirectory">モジュールを配置するディレクトリ。</param>
        /// <param name="cacheDirectory"><paramref name="moduleDirectory"/> のシャドウ コピーを配置するディレクトリ。</param>
        /// <param name="configDirectory">モジュールの設定ファイルを配置するディレクトリ。</param>
        /// <param name="tempDirectory">一時ファイルを生成するディレクトリ。</param>
        public ModuleManager(
            ServerCore parent,
            DirectoryInfo moduleDirectory,
            DirectoryInfo cacheDirectory,
            DirectoryInfo configDirectory,
            DirectoryInfo tempDirectory
        )
        {
            this.Parent = parent;
            this._assemblyManager = new AssemblyManager();
            this._modules = new Dictionary<String, Dictionary<Tuple<Type, String>, IModule>>();
            this.LoadHook = new Hook<ModuleManager, String>();
            this.ExecuteHook = new Hook<ModuleManager, String, FileInfo>();
            this.UnloadHook = new Hook<ModuleManager, String>();
            this.AddHook = new Hook<ModuleManager, String, String, String, FileInfo>();
            this.RemoveHook = new Hook<ModuleManager, String, Type, String>();

            this.ModuleDirectory = moduleDirectory;
            this.CacheDirectory = cacheDirectory;
            this.ConfigDirectory = configDirectory;
            this.TempDirectory = tempDirectory;

            this.Initialize();
        }

        /// <summary>
        /// <see cref="ModuleManager"/> によって使用されているすべてのリソースを解放します。
        /// </summary>
        public virtual void Dispose()
        {
            foreach (String domain in this._modules.Keys.ToArray())
            {
                this.Unload(domain);
            }
        }

        /// <summary>
        /// このオブジェクトに標準の初期設定を適用します。
        /// </summary>
        protected virtual void Initialize()
        {
            this._assemblyManager.DefaultAppDomainSetup.ApplicationBase
                = this.Parent.RootDirectory.FullName;
            this._assemblyManager.DefaultAppDomainSetup.ApplicationName = "ModuleManager";
            this._assemblyManager.DefaultAppDomainSetup.CachePath
                = this.CacheDirectory.FullName;
            this._assemblyManager.DefaultAppDomainSetup.DynamicBase
                = this._assemblyManager.DefaultAppDomainSetup.CachePath;
            this._assemblyManager.DefaultAppDomainSetup.LoaderOptimization = LoaderOptimization.MultiDomainHost;
            this._assemblyManager.DefaultAppDomainSetup.PrivateBinPath = String.Join(";", new String[]
            {
                this.Parent.RootDirectory.FullName,
                this.ModuleDirectory.FullName,
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

        /// <summary>
        /// モジュール アセンブリをロードします。
        /// </summary>
        /// <param name="domain">モジュール アセンブリを識別する名前。</param>
        public virtual void Load(String domain)
        {
            this.LoadHook.Execute((self, domain_) =>
            {
                FileInfo moduleFile = this.ModuleDirectory.GetFiles(Path.ChangeExtension(domain_, ".dll")).Single();
                FileInfo moduleSymbolStoreFile = new FileInfo(Path.ChangeExtension(moduleFile.FullName, ".pdb"));
                self._assemblyManager.Load(
                    domain_,
                    moduleFile,
                    moduleSymbolStoreFile.Exists ? moduleSymbolStoreFile : null
                );
                this._modules.Add(domain_, new Dictionary<Tuple<Type, String>, IModule>());
            }, this, domain);
        }

        /// <summary>
        /// 指定されたファイルをコンパイルし、実行するか、またはモジュール アセンブリとしてロードします。
        /// </summary>
        /// <param name="domain">モジュール アセンブリを識別する名前。モジュールアセンブリでない場合は <c>null</c>。</param>
        /// <param name="file">コンパイルするファイル。</param>
        /// <remarks>
        /// <para>コンパイルした結果得られたアセンブリにモジュール (<see cref="IModule"/> を実装する型) が含まれていた場合、モジュール アセンブリとしてロードされます。</para>
        /// <para>それ以外の場合、<see cref="ServerCore"/>、<see cref="XSpect.Configuration.XmlConfiguration"/> の二つを順序通りに引数とするメソッドが呼び出されます。この場合、<paramref name="domain"/> の値は無視されます。</para>
        /// </remarks>
        protected virtual void Execute(String domain, FileInfo file)
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

        /// <summary>
        /// 指定されたファイルをコンパイルし、実行します。
        /// </summary>
        /// <param name="file">コンパイルするファイル。</param>
        public virtual void Execute(FileInfo file)
        {
            TempFileCollection tempFiles = (this._assemblyManager.DefaultParameters.TempFiles
                = new TempFileCollection(this.TempDirectory.FullName, false));
            this.Execute("<temp>" + tempFiles.BasePath.Substring(tempFiles.TempDir.Length + 1), file);
        }

        /// <summary>
        /// 指定されたファイルをコンパイルし、実行します。
        /// </summary>
        /// <param name="path">コンパイルするファイル。</param>
        /// <remarks>
        /// 詳細は他のオーバーロードを参照してください。
        /// </remarks>
        public virtual void Execute(String path)
        {
            this.Execute(null, new FileInfo(path));
        }
        
        /// <summary>
        /// モジュール アセンブリをアンロードします。
        /// </summary>
        /// <param name="domain">モジュール アセンブリを識別する名前。</param>
        public virtual void Unload(String domain)
        {
            this.UnloadHook.Execute((self, domain_) =>
            {
                self._modules[domain].ForEach(p => p.Value.Dispose());
                self._modules.Remove(domain_);
                self._assemblyManager.Unload(domain_);
            }, this, domain);
        }

        /// <summary>
        /// モジュール アセンブリからモジュール オブジェクトを生成します。
        /// </summary>
        /// <typeparam name="TModule">モジュールを一意に選択するための型。</typeparam>
        /// <param name="domain">モジュール アセンブリを識別する名前。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。</param>
        /// <param name="typeName">生成するモジュール オブジェクトの完全な型名。</param>
        /// <returns>生成された型厳密なモジュール オブジェクト。</returns>
        public virtual TModule Add<TModule>(String domain, String key, String typeName)
            where TModule
                : IModule
        {
            return (TModule) this.Add(domain, key, typeName);
        }

        /// <summary>
        /// モジュール アセンブリからモジュール オブジェクトを生成します。
        /// </summary>
        /// <param name="domain">モジュール アセンブリを識別する名前。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。</param>
        /// <param name="typeName">生成するモジュール オブジェクトの完全な型名。</param>
        /// <param name="configFile">生成するモジュール オブジェクトに適用する設定ファイル。</param>
        /// <returns>生成された型厳密でないモジュール オブジェクト。</returns>
        public virtual IModule Add(String domain, String key, String typeName, FileInfo configFile)
        {
            return this.AddHook.Execute((self, domain_, key_, typeName_, configFile_) =>
            {
                IModule module = self._assemblyManager[domain_].CreateInstance(typeName_) as IModule;
                self._modules[domain_].Add(Make.Tuple(module.GetType(), key_), module);
                module.Register(self.Parent, key_);
                module.Initialize(XmlConfiguration.Load(configFile_.FullName));
                return module;
            }, this, domain, key, typeName, configFile);
        }

        /// <summary>
        /// モジュール アセンブリからモジュール オブジェクトを生成します。
        /// </summary>
        /// <param name="domain">モジュール アセンブリを識別する名前。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。</param>
        /// <param name="typeName">生成するモジュール オブジェクトの完全な型名。</param>
        /// <returns>生成された型厳密でないモジュール オブジェクト。</returns>
        public IModule Add(String domain, String key, String typeName)
        {
            return this.Add(domain, key, typeName, this.ConfigDirectory.GetFiles(String.Format(
                "{0}-{1}.conf.xml",
                typeName.Substring(typeName.LastIndexOf('.') + 1),
                key
            )).Single());
        }

        /// <summary>
        /// モジュール オブジェクトを破棄し、登録を解除します。
        /// </summary>
        /// <param name="domain">モジュール アセンブリを識別する名前。</param>
        /// <param name="type">破棄するモジュール オブジェクトの型を表すオブジェクト。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。</param>
        protected virtual void Remove(String domain, Type type, String key)
        {
            this.RemoveHook.Execute((self, domain_, type_, key_) =>
            {
                IModule module = self.GetModule(domain_, type_, key_);
                self._modules[domain_].Remove(Make.Tuple(type_, key_));
                module.Dispose();
            }, this, domain, type, key);
        }

        /// <summary>
        /// モジュール オブジェクトを破棄し、登録を解除します。
        /// </summary>
        /// <typeparam name="TModule">破棄するモジュール オブジェクトの型。</typeparam>
        /// <param name="domain">モジュール アセンブリを識別する名前。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。</param>
        public virtual void Remove<TModule>(String domain, String key)
            where TModule
                : IModule
        {
            this.Remove(domain, typeof(TModule), key);
        }

        /// <summary>
        /// モジュール オブジェクトを破棄し、登録を解除します。
        /// </summary>
        /// <param name="domain">モジュール アセンブリを識別する名前。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。</param>
        /// <remarks>
        /// <paramref name="domain"/> および <paramref name="key"/> のみによって一意に特定できる
        /// モジュール オブジェクトを破棄するためのメソッドです。一意に特定できない場合は例外が
        /// 発生します。
        /// </remarks>
        public virtual void Remove(String domain, String key)
        {
            Type type = this.GetModule(domain, key).GetType();
            this.Remove(domain, type, key);
        }

        // TODO: RemoveAll: GetModules[<T>](domain, key[, type]).ForEach(Remove)

        /// <summary>
        /// モジュールを検索します。
        /// </summary>
        /// <param name="domain">モジュール アセンブリを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="type">モジュール オブジェクトの型を表すオブジェクト。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュールの集合。</returns>
        protected virtual IEnumerable<IModule> GetModules(String domain, Type type, String key)
        {
            return (domain != null
                ? this._modules[domain]
                : this._modules.SelectMany(p => p.Value)
            )
                .Where(
                    p => p.Key.Item1.IsSubclassOf(type != null ? type : typeof(Object)) &&
                         p.Key.Item2 == (key != null ? key : p.Key.Item2)
                 )
                .Select(p => p.Value);
        }

        /// <summary>
        /// モジュールを検索します。
        /// </summary>
        /// <typeparam name="TModule">モジュール オブジェクトの型。</typeparam>
        /// <param name="domain">モジュール アセンブリを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュールの集合。</returns>
        public IEnumerable<TModule> GetModules<TModule>(String domain, String key)
            where TModule
                : IModule
        {
            return this.GetModules(domain, typeof(TModule), key)
                .OfType<TModule>();
        }

        /// <summary>
        /// モジュールを検索します。
        /// </summary>
        /// <typeparam name="TModule">モジュール オブジェクトの型。</typeparam>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュールの集合。</returns>
        public IEnumerable<TModule> GetModules<TModule>(String key)
            where TModule
                : IModule
        {
            return this.GetModules(null, typeof(TModule), key)
                .OfType<TModule>();
        }

        /// <summary>
        /// モジュールを検索します。
        /// </summary>
        /// <typeparam name="TModule">モジュール オブジェクトの型。</typeparam>
        /// <returns>条件に合致するモジュールの集合。</returns>
        public IEnumerable<TModule> GetModules<TModule>()
            where TModule
                : IModule
        {
            return this.GetModules(null, typeof(TModule), null)
                .OfType<TModule>();
        }

        /// <summary>
        /// モジュールを検索します。
        /// </summary>
        /// <param name="domain">モジュール アセンブリを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュールの集合。</returns>
        public IEnumerable<IModule> GetModules(String domain, String key)
        {
            return this.GetModules(domain, null, key);
        }

        /// <summary>
        /// モジュールを検索します。
        /// </summary>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュールの集合。</returns>
        public IEnumerable<IModule> GetModules(String key)
        {
            return this.GetModules(null, null, key);
        }

        /// <summary>
        /// 全てのモジュールの集合を取得します。
        /// </summary>
        /// <returns>全てのモジュールの集合。</returns>
        public IEnumerable<IModule> GetModules()
        {
            return this.GetModules(null, null, null);
        }

        /// <summary>
        /// モジュールを取得します。
        /// </summary>
        /// <param name="domain">モジュール アセンブリを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="type">モジュール オブジェクトの型を表すオブジェクト。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>一意に特定されたモジュール。</returns>
        protected IModule GetModule(String domain, Type type, String key)
        {
            return this.GetModules(domain, type, key).Single();
        }

        /// <summary>
        /// モジュールを取得します。
        /// </summary>
        /// <typeparam name="TModule">モジュール オブジェクトの型。</typeparam>
        /// <param name="domain">モジュール アセンブリを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>一意に特定されたモジュール。</returns>
        public TModule GetModule<TModule>(String domain, String key)
            where TModule
                : IModule
        {
            return this.GetModules<TModule>(domain, key).Single();
        }

        /// <summary>
        /// モジュールを取得します。
        /// </summary>
        /// <typeparam name="TModule">モジュール オブジェクトの型。</typeparam>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>一意に特定されたモジュール。</returns>
        public TModule GetModule<TModule>(String key)
            where TModule
                : IModule
        {
            return this.GetModules<TModule>(key).Single();
        }

        /// <summary>
        /// モジュールを取得します。
        /// </summary>
        /// <param name="domain">モジュール アセンブリを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>一意に特定されたモジュール。</returns>
        public IModule GetModule(String domain, String key)
        {
            return this.GetModules(domain, key).Single();
        }

        /// <summary>
        /// モジュールを取得します。
        /// </summary>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>一意に特定されたモジュール。</returns>
        public IModule GetModule(String key)
        {
            return this.GetModules(key).Single();
        }
    }
}