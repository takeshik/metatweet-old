// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
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
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Microsoft.Scripting.Hosting;
using XSpect.Collections;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using XSpect.Extension;
using System.IO;
using Achiral;
using Achiral.Extension;
using XSpect.MetaTweet.Objects;
using XSpect.MetaTweet.Properties;

namespace XSpect.MetaTweet.Modules
{
    /// <summary>
    /// モジュール アセンブリを読み込み、モジュール オブジェクトを管理するための、独立した環境を提供します。
    /// </summary>
    /// <remarks>
    /// <para>モジュール ドメインは、<see cref="ModuleManager"/> によって作成される、モジュール アセンブリのための独立した環境です。<see cref="Add(string,string,System.Collections.Generic.IList{string})"/> メソッドを使用して新しいモジュール オブジェクトを生成し、<see cref="Remove{TModule}"/> メソッドを使用してそれを破棄することができます。</para>
    /// <para>モジュール オブジェクトは名前 (キー) と型によって一意に識別されます。型が異なる限りにおいて、同一の名前を使用できます。</para>
    /// <para>モジュール ドメインはドメインの名前と同一のディレクトリに対応します。対応先のディレクトリは <see cref="Directory"/> プロパティで参照できます。</para>
    /// </remarks>
    /// <seealso cref="ModuleManager"/>
    public class ModuleDomain
        : MarshalByRefObject,
          IDisposable,
          ILoggable
    {
        /// <summary>
        /// アプリケーション ドメインおよび <see cref="ModuleDomain"/> において、モジュール ドメインを示す接頭文字列を取得します。
        /// </summary>
        public const String Prefix = "Modules.";

        private Boolean _tainted;

        private Boolean _disposed;

        public Log Log
        {
            get
            {
                return this.Parent.Parent.Let(s => s.LogManager[
                    s.MainAppDomain.Invoke(d => (String) d.Get<ServerCore>("_").Configuration.Loggers.ModuleDomain, _ => s)
                ]);
            }
        }

        /// <summary>
        /// このモジュール ドメインの親である <see cref="ModuleManager"/> を取得します。
        /// </summary>
        /// <value>
        /// このモジュール ドメインの親である <see cref="ModuleManager"/>。
        /// </value>
        public ModuleManager Parent
        {
            get;
            private set;
        }

        public String Key
        {
            get;
            private set;
        }

        public AppDomain AppDomain
        {
            get;
            private set;
        }

        public ScriptRuntime ScriptRuntime
        {
            get;
            private set;
        }

        public IEnumerable<AssemblyName> Assemblies
        {
            get
            {
                return this.AppDomain.Invoke(() => AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetName()));
            }
        }

        /// <summary>
        /// イベントを記録するログ ライタを取得します。
        /// </summary>
        /// <value>
        /// イベントを記録するログ ライタ。
        /// </value>

        /// <summary>
        /// 生成されたモジュール オブジェクトのコレクションを取得します。
        /// </summary>
        /// <value>
        /// 生成されたモジュール オブジェクトのコレクション。
        /// </value>
        public HybridDictionary<Tuple<String, String>, IModule> Modules
        {
            get;
            private set;
        }

        /// <summary>
        /// このモジュール ドメイン上に現時点で存在するモジュール オブジェクトの初期か情報のリストを取得します。
        /// </summary>
        /// <value>
        /// このモジュール ドメイン上に現時点で存在するモジュール オブジェクトの初期か情報のリスト。
        /// </value>
        public IList<ModuleObjectSetup> Snapshot
        {
            get
            {
                return this.Modules
                    .Select(t => new ModuleObjectSetup()
                    {
                        Key = t.Key.Item1,
                        TypeName = t.Key.Item2.Remove(t.Key.Item2.IndexOf(',')),
                        Options = new Collection<String>(t.Value.Options),
                    })
                    .OrderBy(s => s.GetOrder())
                    .ToList();
            }
        }

        /// <summary>
        /// このモジュール ドメインに対応するディレクトリを取得します。
        /// </summary>
        /// <value>
        /// このモジュール ドメインに対応するディレクトリ。
        /// </value>
        public DirectoryInfo Directory
        {
            get;
            private set;
        }

        public ModuleDomain(ModuleManager parent, String domainName, ScriptRuntimeSetup scriptingSetup)
        {
            // HACK: Prevent from finalizing; this code should be replaced in the future
            GC.SuppressFinalize(this);
            this.Parent = parent;
            this.Key = domainName;
            this.AppDomain = AppDomain.CurrentDomain;
            this.ScriptRuntime = new ScriptRuntime(scriptingSetup);
            this.Directory = this.Parent.Parent.Directories.ModuleDirectory.Directory(domainName);
            this.Modules = new HybridDictionary<Tuple<String, String>, IModule>(
                (i, m) => Tuple.Create(m.Name, m.GetType().AssemblyQualifiedName)
            );
            this.Modules.ItemsRemoved += (sender, e) => e.OldElements.ForEach(_ => _.Value.Dispose());
            this.Modules.ItemsReset += (sender, e) => e.OldElements.ForEach(_ => _.Value.Dispose());
        }

        ~ModuleDomain()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(Boolean disposing)
        {
            if (this._disposed)
            {
                this.Modules.Clear();
            }
            AppDomain.Unload(this.AppDomain);
            this._disposed = true;
        }

        protected void CheckIfDisposed()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }

        /// <summary>
        /// 既定の動作に従い、モジュール ドメインにアセンブリをロードします。
        /// </summary>
        public void Load()
        {
            this.CheckIfDisposed();
            this.Directory.GetFiles("*.dll")
                .Concat(this.Directory.GetFiles(".exe"))
                .ForEach(f => this.LoadFrom(f.FullName));
        }

        /// <summary>
        /// モジュール ドメインをアンロードします。
        /// </summary>
        public void Unload()
        {
            this.Dispose();
        }

        /// <summary>
        /// モジュール オブジェクトを生成します。
        /// </summary>
        /// <typeparam name="TModule">返り値となるモジュール オブジェクトの型。</typeparam>
        /// <param name="key">モジュール オブジェクトに付ける名前。</param>
        /// <param name="typeName">生成するモジュール オブジェクトの完全な型名。</param>
        /// <param name="options">モジュール オブジェクトに渡されるオプションのリスト。</param>
        /// <returns>生成された型厳密なモジュール オブジェクト。</returns>
        public TModule Add<TModule>(String key, String typeName, IList<String> options)
            where TModule : IModule
        {
            return (TModule) this.Add(key, typeName, options);
        }

        /// <summary>
        /// モジュール オブジェクトを生成します。
        /// </summary>
        /// <param name="key">モジュール オブジェクトに付ける名前。</param>
        /// <param name="typeName">生成するモジュール オブジェクトの完全な型名。</param>
        /// <param name="options">モジュール オブジェクトに渡されるオプションのリスト。</param>
        /// <returns>生成されたモジュール オブジェクト。</returns>
        public IModule Add(String key, String typeName, IList<String> options)
        {
            return this.Add(
                key,
                typeName,
                options,
                this.Parent.Parent.Directories.ConfigDirectory.File(String.Format(
                    "modules.d/{0}-{1}.conf.*",
                    typeName.Substring(typeName.LastIndexOf('.') + 1),
                    key
                ))
            );
        }

        /// <summary>
        /// モジュール オブジェクトを生成します。
        /// </summary>
        /// <param name="setup">モジュール オブジェクトを生成するための情報。</param>
        /// <returns>生成されたモジュール オブジェクト。</returns>
        public IModule Add(ModuleObjectSetup setup)
        {
            return this.Add(setup.Key, setup.TypeName, setup.Options);
        }

        /// <summary>
        /// モジュール オブジェクトを生成します。
        /// </summary>
        /// <param name="key">モジュール オブジェクトの名前。</param>
        /// <param name="typeName">生成するモジュール オブジェクトの完全な型名。</param>
        /// <param name="options">モジュール オブジェクトに渡されるオプションのリスト。</param>
        /// <param name="configFile">モジュール オブジェクトの初期化時に与える設定ファイル。</param>
        /// <returns>生成されたモジュール オブジェクト。</returns>
        public IModule Add(String key, String typeName, IList<String> options, FileInfo configFile)
        {
            this.CheckIfDisposed();
            if (!(options.Contains("separate") || this._tainted))
            {
                new AppDomainInvoker(this.Parent.Parent.MainAppDomain, d =>
                    d.Get<String[]>("f").ForEach(f => Assembly.LoadFrom(f)),
                    Make.Dictionary<Object>(f => this.Directory.GetFiles("*.dll")
                        .Concat(this.Directory.GetFiles(".exe"))
                        .Select(_ => _.FullName)
                        .ToArray()
                    )
                ).Invoke();
                this._tainted = true;
            }
            return Tuple.Create(key, typeName).Let(id =>
                this.Modules.ContainsKey(id)
                    ? this.Modules[id]
                    : (options.Contains("separate")
                          ? this.AppDomain
                          : this.Parent.Parent.MainAppDomain
                      ).Let(d =>
                          new AppDomainInvoker<Object>(
                              d,
                              _ => Activator.CreateInstance(AppDomain.CurrentDomain.GetAssemblies()
                                  .Select(a => a.GetType(_.Get<String>("t")))
                                  .Single(t => t != null)
                              ),
                              Make.Dictionary<Object>(t => typeName)
                          ).Invoke()
                              .If(
                                  o => o is Storage,
                                  s => ((StorageModule) Activator.CreateInstance(
                                      d,
                                      typeof(StorageModule).Assembly.FullName,
                                      typeof(StorageModule).FullName,
                                      false,
                                      BindingFlags.Default,
                                      null,
                                      Make.Array(s),
                                      null,
                                      null
                                  ).Unwrap()),
                                  o => (IModule) o
                              )
                      ).Apply(
                          m => m.Register(this, key, options),
                          this.Modules.Add,
                          m => this.Log.Info(Resources.ModuleAssemblyLoaded, this.Key, key, typeName),
                          m => m.Configure(configFile),
                          m => m.Initialize()
                      )
            );
        }

        /// <summary>
        /// モジュール オブジェクトを破棄します。
        /// </summary>
        /// <typeparam name="TModule">破棄するモジュール オブジェクトの型。</typeparam>
        /// <param name="key">破棄するモジュール オブジェクトの名前。</param>
        public void Remove<TModule>(String key)
            where TModule : IModule
        {
            this.Remove(key, typeof(TModule));
        }

        /// <summary>
        /// モジュール オブジェクトを破棄します。
        /// </summary>
        /// <param name="key">破棄するモジュール オブジェクトの名前。</param>
        /// <param name="type">破棄するモジュール オブジェクトの型を表すオブジェクト。</param>
        public void Remove(String key, Type type)
        {
            this.Modules.RemoveValue(this.GetModule(key, type).Apply(m => m.Dispose()));
            this.Log.Info(Resources.ModuleObjectRemoved, this.Key, type.FullName, key);
        }

        #region Load / LoadFile / LoadFrom

        public AssemblyName Load(AssemblyName assemblyRef)
        {
            this.CheckIfDisposed();
            return this.AppDomain.Invoke(
                d => Assembly.Load(d.Get<AssemblyName>("r")).GetName(),
                r => assemblyRef
            );
        }

        public AssemblyName Load(String assemblyString)
        {
            this.CheckIfDisposed();
            return this.AppDomain.Invoke(
                d => Assembly.Load(d.Get<String>("s")).GetName(),
                s => assemblyString
            );
        }

        public AssemblyName Load(Byte[] rawAssembly)
        {
            this.CheckIfDisposed();
            return this.AppDomain.Invoke(
                d => Assembly.Load(d.Get<Byte[]>("a")).GetName(),
                a => rawAssembly
            );
        }

        public AssemblyName Load(Byte[] rawAssembly, Byte[] rawSymbolStore)
        {
            this.CheckIfDisposed();
            return this.AppDomain.Invoke(
                d => Assembly.Load(d.Get<Byte[]>("a"), d.Get<Byte[]>("s")).GetName(),
                a => rawAssembly, s => rawSymbolStore
            );
        }

        public AssemblyName LoadFile(String path)
        {
            this.CheckIfDisposed();
            return this.AppDomain.Invoke(
                d => Assembly.LoadFile(d.Get<String>("p")).GetName(),
                Makep => path
            );
        }

        public AssemblyName LoadFrom(String assemblyFile)
        {
            this.CheckIfDisposed();
            return this.AppDomain.Invoke(
                d => Assembly.LoadFrom(d.Get<String>("f")).GetName(),
                f => assemblyFile
            );
        }

        #endregion

        #region GetModules / GetModule

        /// <summary>
        /// モジュール オブジェクトを検索します。
        /// </summary>
        /// <typeparam name="TModule">モジュール オブジェクトの型。</typeparam>
        /// <returns>条件に合致するモジュール オブジェクトのシーケンス。</returns>
        public IEnumerable<TModule> GetModules<TModule>()
            where TModule : IModule
        {
            return this.GetModules<TModule>(null);
        }

        /// <summary>
        /// モジュール オブジェクトを検索します。
        /// </summary>
        /// <typeparam name="TModule">モジュール オブジェクトの型。</typeparam>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュール オブジェクトのシーケンス。</returns>
        public IEnumerable<TModule> GetModules<TModule>(String key)
            where TModule : IModule
        {
            return this.GetModules(key, typeof(TModule)).OfType<TModule>().AsTransparent();
        }

        /// <summary>
        /// 全てのモジュール オブジェクトを取得します。
        /// </summary>
        /// <returns>全てのモジュール オブジェクトのシーケンス。</returns>
        public IEnumerable<IModule> GetModules()
        {
            return this.Modules.Values.AsTransparent();
        }

        /// <summary>
        /// モジュール オブジェクトを検索します。
        /// </summary>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュール オブジェクトのシーケンス。</returns>
        public IEnumerable<IModule> GetModules(String key)
        {
            return this.GetModules(key, null);
        }

        /// <summary>
        /// モジュール オブジェクトを検索します。
        /// </summary>
        /// <param name="type">モジュール オブジェクトの型を表すオブジェクト。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュール オブジェクトのシーケンス。</returns>
        public IEnumerable<IModule> GetModules(Type type)
        {
            return this.GetModules(null, type);
        }

        /// <summary>
        /// モジュール オブジェクトを検索します。
        /// </summary>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="type">モジュール オブジェクトの型を表すオブジェクト。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュール オブジェクトのシーケンス。</returns>
        public IEnumerable<IModule> GetModules(String key, Type type)
        {
            return this.Modules.Values.Where(m =>
                (key == null || m.Name == key) &&
                (type == null || m.CreateObjRef().TypeInfo
                    .Let(ti => ti.TypeName == type.AssemblyQualifiedName || ti.CanCastTo(type, m))
                )
            ).AsTransparent();
        }

        /// <summary>
        /// モジュール オブジェクトを取得します。
        /// </summary>
        /// <typeparam name="TModule">モジュール オブジェクトの型。</typeparam>
        /// <returns>一意に特定されたモジュール オブジェクト。</returns>
        public TModule GetModule<TModule>()
            where TModule : IModule
        {
            return this.GetModules<TModule>().Single();
        }

        /// <summary>
        /// モジュール オブジェクトを取得します。
        /// </summary>
        /// <typeparam name="TModule">モジュール オブジェクトの型。</typeparam>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>一意に特定されたモジュール オブジェクト。</returns>
        public TModule GetModule<TModule>(String key)
            where TModule : IModule
        {
            return this.GetModules<TModule>(key).Single();
        }

        /// <summary>
        /// モジュール オブジェクトを取得します。
        /// </summary>
        /// <returns>唯一のモジュール オブジェクト。</returns>
        public IModule GetModule()
        {
            return this.GetModules().Single();
        }

        /// <summary>
        /// モジュール オブジェクトを取得します。
        /// </summary>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>一意に特定されたモジュール オブジェクト。</returns>
        public IModule GetModule(String key)
        {
            return this.GetModules(key).Single();
        }
        
        /// <summary>
        /// モジュール オブジェクトを取得します。
        /// </summary>
        /// <param name="type">モジュール オブジェクトの型を表すオブジェクト。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>一意に特定されたモジュール オブジェクト。</returns>
        public IModule GetModule(Type type)
        {
            return this.GetModules(type).Single();
        }
        
        /// <summary>
        /// モジュール オブジェクトを取得します。
        /// </summary>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="type">モジュール オブジェクトの型を表すオブジェクト。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>一意に特定されたモジュール オブジェクト。</returns>
        public IModule GetModule(String key, Type type)
        {
            return this.GetModules(key, type).Single();
        }

        #endregion

        #region Execute

        /// <summary>
        /// スクリプト コードを実行します。
        /// </summary>
        /// <typeparam name="T">コードの返り値の型。</typeparam>
        /// <value>コードを実行する <see cref="ScriptEngine"/>。</value>
        /// <param name="source">実行するコード。</param>
        /// <param name="arguments">コードに与える引数とその値のリスト。</param>
        /// <returns>コードの評価の結果となる返り値。</returns>
        protected T Execute<T>(ScriptEngine engine, String source, params Expression<Func<Object, dynamic>>[] arguments)
        {
            return engine.Execute<T>(
                source,
                this.Parent.Parent.Directories.ConfigDirectory.GetFiles("global.*")
                    .SingleOrDefault(f => engine.Setup.FileExtensions.Contains(f.Extension))
                    .If(
                        f => f != null,
                        f => engine.ExecuteFile(f.FullName, engine.CreateScope(Make.Dictionary(arguments))),
                        _ => engine.CreateScope(Make.Dictionary(arguments))
                    )
            );
        }

        /// <summary>
        /// スクリプト コードを実行します。
        /// </summary>
        /// <typeparam name="T">コードの返り値の型。</typeparam>
        /// <param name="language">コードの言語を表す文字列。</param>
        /// <param name="source">実行するコード。</param>
        /// <param name="arguments">コードに与える引数とその値のリスト。</param>
        /// <returns>コードの評価の結果となる返り値。</returns>
        public T Execute<T>(String language, String source, params Expression<Func<Object, dynamic>>[] arguments)
        {
            return this.Execute<T>(this.ScriptRuntime.GetEngine(language), source, arguments);
        }

        /// <summary>
        /// スクリプト コードを実行します。
        /// </summary>
        /// <param name="language">コードの言語を表す文字列。</param>
        /// <param name="source">実行するコード。</param>
        /// <param name="arguments">コードに与える引数とその値のリスト。</param>
        /// <returns>コードの評価の結果となる返り値。</returns>
        public dynamic Execute(String language, String source, params Expression<Func<Object, dynamic>>[] arguments)
        {
            return this.Execute<dynamic>(language, source, arguments);
        }

        /// <summary>
        /// スクリプト コードを実行します。
        /// </summary>
        /// <typeparam name="T">コードの返り値の型。</typeparam>
        /// <param name="file">実行するコード ファイル。</param>
        /// <param name="arguments">コードに与える引数とその値のリスト。</param>
        /// <returns>コードの評価の結果となる返り値。</returns>
        public T Execute<T>(FileInfo file, params Expression<Func<Object, dynamic>>[] arguments)
        {
            return this.Execute<T>(this.ScriptRuntime.GetEngineByFileExtension(file.Extension), file.ReadAllText(), arguments);
        }

        /// <summary>
        /// スクリプト コードを実行します。
        /// </summary>
        /// <param name="file">実行するコード ファイル。</param>
        /// <param name="arguments">コードに与える引数とその値のリスト。</param>
        /// <returns>コードの評価の結果となる返り値。</returns>
        public dynamic Execute(FileInfo file, params Expression<Func<Object, dynamic>>[] arguments)
        {
            return this.Execute<dynamic>(file, arguments);
        }

        #endregion

        /// <summary>
        /// 型名を表す文字列から、その型が含まれるアセンブリの名前を返します。
        /// </summary>
        /// <param name="typeName">検索する型を表す文字列。</param>
        /// <returns>指定した型が含まれるアセンブリの名前を表すオブジェクト。</returns>
        public AssemblyName GetAssemblyByName(String typeName)
        {
            return this.AppDomain.Invoke(d =>
                AppDomain.CurrentDomain.GetAssemblies()
                    .First(a => a.GetType(d.Get<String>("t")) != null)
                    .GetName(),
                t => typeName
            );
        }
    }
}
