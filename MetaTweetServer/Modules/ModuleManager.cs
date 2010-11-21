// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
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
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Scripting.Hosting;
using Achiral;
using Achiral.Extension;
using XSpect.Collections;
using XSpect.Extension;

namespace XSpect.MetaTweet.Modules
{
    /// <summary>
    /// モジュールの管理を行う機能を提供します。
    /// </summary>
    /// <remarks>
    /// <para>モジュールは、MetaTweet に機能を実装する、自由に着脱可能な機構です。モジュールは、
    /// <list type="bullet">
    /// <item><description><see cref="IModule"/> を実装する型 (モジュール型)</description></item>
    /// <item><description>モジュール型を 1 以上含むアセンブリ (モジュール アセンブリ)</description></item>
    /// <item><description>モジュール型のインスタンス (モジュール オブジェクト)</description></item>
    /// </list>
    /// の総称です。</para>
    /// <para><see cref="Load"/> メソッドを使用して、モジュール アセンブリをロードし、<see cref="Unload"/> メソッドを使用してアンロードすることができます。ロードとはモジュールのための独立した環境 (モジュール ドメイン: <see cref="ModuleDomain"/> オブジェクト) を構築し、その中でモジュールを読み込む作業です。アンロードとはロードしたモジュール アセンブリを解放し、ドメインを破棄する作業です。</para>
    /// <para>モジュール ドメインの基底構造はディレクトリです。モジュール ドメインが作成されると、ドメインの名前と同じディレクトリ内のアセンブリをロードします。</para>
    /// <para>モジュール オブジェクトの管理は、モジュール アセンブリが読み込まれた環境である <see cref="ModuleDomain"/> オブジェクトで行います。</para>
    /// </remarks>
    /// <seealso cref="ModuleDomain"/>
    public class ModuleManager
        : IDisposable,
          ILoggable
    {
        private static readonly AssemblyName[] _defaultAssemblies = Make.Array(
            typeof(System.Object),
            typeof(System.Uri),
            typeof(System.Linq.Enumerable),
            typeof(System.Data.DataSet),
            typeof(System.Xml.XmlDocument),
            typeof(System.Xml.Linq.XDocument),
            typeof(Achiral.Make),
            typeof(XSpect.Create),
            typeof(XSpect.MetaTweet.ServerCore),
            typeof(XSpect.MetaTweet.Objects.Storage)
        ).Select(t => t.Assembly.GetName()).ToArray();

        private Boolean _disposed;

        /// <summary>
        /// イベントを記録するログ ライタを取得します。
        /// </summary>
        /// <value>
        /// イベントを記録するログ ライタ。
        /// </value>
        public Log Log
        {
            get
            {
                return this.Parent.Let(
                    s => s.LogManager[s.Configuration.Loggers.ModuleManager]
                );
            }
        }

        /// <summary>
        /// このオブジェクトを保持する <see cref="ServerCore"/> オブジェクトを取得します。
        /// </summary>
        /// <value>
        /// このオブジェクトを保持する <see cref="ServerCore"/> オブジェクト。
        /// </value>
        public ServerCore Parent
        {
            get;
            private set;
        }

        /// <summary>
        /// このオブジェクトの設定を保持するオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// このオブジェクトの設定を保持するオブジェクト。
        /// </value>
        public dynamic Configuration
        {
            get;
            private set;
        }

        /// <summary>
        /// 現在管理されているモジュール ドメインのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// 現在管理されているモジュール ドメインのシーケンス。
        /// </value>
        public HybridDictionary<String, ModuleDomain> Domains
        {
            get;
            private set;
        }

        /// <summary>
        /// スクリプト コードを実行するためのオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// スクリプト コードを実行するためのオブジェクト。
        /// </value>
        public ScriptRuntime ScriptRuntime
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="ModuleManager"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="parent">このオブジェクトを生成する、親となるオブジェクト。</param>
        /// <param name="configFile">モジュールの初期化設定を行うための設定を行うスクリプト ファイル。</param>
        /// <param name="scriptingConfigFile"><see cref="ScriptRuntime"/> を初期化するための設定ファイル。</param>
        public ModuleManager(ServerCore parent, FileInfo configFile, FileInfo scriptingConfigFile)
        {
            this.Parent = parent;
            this.Domains = new HybridDictionary<String, ModuleDomain>((i, v) => v.Key);
            this.ScriptRuntime = new ScriptRuntime(ScriptRuntimeSetup.ReadConfiguration(scriptingConfigFile.FullName));
            this.Configuration = this.Execute(configFile, self => this, host => this.Parent);
        }

        /// <summary>
        /// <see cref="ModuleManager"/> がガベージ コレクションによってクリアされる前に、アンマネージ リソースを解放し、その他のクリーンアップ操作を実行します。
        /// </summary>
        ~ModuleManager()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// <see cref="ModuleManager"/> によって使用されているすべてのリソースを解放します。
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// <see cref="ModuleManager"/> によって使用されているアンマネージ リソースを解放し、オプションでマネージ リソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 <c>true</c>、破棄されない場合は <c>false</c>。</param>
        protected virtual void Dispose(Boolean disposing)
        {
            this._disposed = true;
            // Clear -> ClearItems == Dispose.
            this.Domains.Clear();
        }

        /// <summary>
        /// オブジェクトが破棄されているかどうかを確認し、破棄されている場合は例外を送出します。
        /// </summary>
        protected void CheckIfDisposed()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException("this");
            }
        }

        /// <summary>
        /// モジュール ドメインを作成し、モジュール アセンブリをロードします。
        /// </summary>
        /// <param name="domainName">ドメインの名前、即ち、ロードするモジュール アセンブリを含んだディレクトリの名前。</param>
        /// <returns>モジュール アセンブリがロードされたモジュール ドメイン。</returns>
        public ModuleDomain Load(String domainName)
        {
            this.Domains.Add(new ModuleDomain(this, domainName)
                .Apply(d => _defaultAssemblies.ForEach(ar => d.Load(ar))));
            this.Domains[domainName].Load();
            return this.Domains[domainName];
        }

        /// <summary>
        /// モジュール ドメインをアンロードします。
        /// </summary>
        /// <param name="domainName">アンロードするモジュール ドメインの名前。</param>
        public void Unload(String domainName)
        {
            this.Domains.Remove(domainName);
        }

        /// <summary>
        /// モジュール ドメインをリロードします。
        /// </summary>
        /// <param name="domainName">リロードするモジュール ドメインの名前。</param>
        public void Reload(String domainName)
        {
            IList<ModuleObjectSetup> snapshot = this.Domains[domainName].Snapshot;
            this.Unload(domainName);
            this.Load(domainName);
            snapshot.ForEach(s => this.Domains[domainName].Add(s));
        }

        #region GetModules / GetModule

        /// <summary>
        /// モジュール オブジェクトを検索します。
        /// </summary>
        /// <param name="domain">モジュール ドメインの名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="type">モジュール オブジェクトの型を表すオブジェクト。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュールのシーケンス。</returns>
        public IEnumerable<IModule> GetModules(String domain, String key, Type type)
        {
            return domain != null
                ? this.Domains[domain].GetModules(key, type)
                : this.GetModules(key, type);
        }

        /// <summary>
        /// モジュール オブジェクトを検索します。
        /// </summary>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="type">モジュール オブジェクトの型を表すオブジェクト。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュールのシーケンス。</returns>
        public IEnumerable<IModule> GetModules(String key, Type type)
        {
            return this.Domains.Values.SelectMany(d => d.GetModules(key, type));
        }

        /// <summary>
        /// モジュール オブジェクトを検索します。
        /// </summary>
        /// <typeparam name="TModule">モジュール オブジェクトの型。</typeparam>
        /// <param name="domain">モジュール ドメインの名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュールのシーケンス。</returns>
        public IEnumerable<TModule> GetModules<TModule>(String domain, String key)
            where TModule : IModule
        {
            return domain != null
                ? this.Domains[domain].GetModules<TModule>(key)
                : this.GetModules<TModule>(key);
        }

        /// <summary>
        /// モジュール オブジェクトを検索します。
        /// </summary>
        /// <typeparam name="TModule">モジュール オブジェクトの型。</typeparam>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュールのシーケンス。</returns>
        public IEnumerable<TModule> GetModules<TModule>(String key)
            where TModule : IModule
        {
            return key != null
                ? this.Domains.Values.SelectMany(d => d.GetModules<TModule>(key))
                : this.GetModules<TModule>();
        }

        /// <summary>
        /// モジュール オブジェクトを検索します。
        /// </summary>
        /// <typeparam name="TModule">モジュール オブジェクトの型。</typeparam>
        /// <returns>条件に合致するモジュールのシーケンス。</returns>
        public IEnumerable<TModule> GetModules<TModule>()
            where TModule : IModule
        {
            return this.Domains.Values.SelectMany(d => d.GetModules<TModule>());
        }

        /// <summary>
        /// モジュール オブジェクトを検索します。
        /// </summary>
        /// <param name="domain">モジュール ドメインの名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュールのシーケンス。</returns>
        public IEnumerable<IModule> GetModules(String domain, String key)
        {
            return domain != null
                ? this.Domains[domain].GetModules(key)
                : this.GetModules(key);
        }

        /// <summary>
        /// モジュール オブジェクトを検索します。
        /// </summary>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュールのシーケンス。</returns>
        public IEnumerable<IModule> GetModules(String key)
        {
            return this.Domains.Values.SelectMany(d => d.GetModules(key));
        }

        /// <summary>
        /// 全てのモジュール オブジェクトを取得します。
        /// </summary>
        /// <returns>全てのモジュールのシーケンス。</returns>
        public IEnumerable<IModule> GetModules()
        {
            return this.Domains.Values.SelectMany(d => d.GetModules());
        }

        /// <summary>
        /// モジュール オブジェクトを取得します。
        /// </summary>
        /// <param name="domain">モジュール ドメインの名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="type">モジュール オブジェクトの型を表すオブジェクト。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>一意に特定されたモジュール。</returns>
        public IModule GetModule(String domain, String key, Type type)
        {
            return domain != null
                ? this.Domains[domain].GetModule(key, type)
                : this.GetModule(key, type);
        }

        /// <summary>
        /// モジュール オブジェクトを取得します。
        /// </summary>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="type">モジュール オブジェクトの型を表すオブジェクト。条件を指定しない場合は <c>null</c>。</param>
        /// <returns></returns>
        public IModule GetModule(String key, Type type)
        {
            return this.GetModules(key, type).SingleOrDefault();
        }

        /// <summary>
        /// モジュール オブジェクトを取得します。
        /// </summary>
        /// <typeparam name="TModule">モジュール オブジェクトの型。</typeparam>
        /// <param name="domain">モジュール ドメインの名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>一意に特定されたモジュール。</returns>
        public TModule GetModule<TModule>(String domain, String key)
            where TModule : IModule
        {
            return domain != null
                ? this.Domains[domain].GetModule<TModule>(key)
                : this.GetModule<TModule>(key);
        }

        /// <summary>
        /// モジュール オブジェクトを取得します。
        /// </summary>
        /// <typeparam name="TModule">モジュール オブジェクトの型。</typeparam>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>一意に特定されたモジュール。</returns>
        public TModule GetModule<TModule>(String key)
            where TModule : IModule
        {
            return this.Domains.Values.SelectMany(d => d.GetModules<TModule>(key)).SingleOrDefault();
        }

        /// <summary>
        /// モジュール オブジェクトを取得します。
        /// </summary>
        /// <param name="domain">モジュール ドメインの名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>一意に特定されたモジュール。</returns>
        public IModule GetModule(String domain, String key)
        {
            return domain != null
                ? this.Domains[domain].GetModule(key)
                : this.GetModule(key);
        }

        /// <summary>
        /// モジュール オブジェクトを取得します。
        /// </summary>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>一意に特定されたモジュール。</returns>
        public IModule GetModule(String key)
        {
            return this.Domains.Values.SelectMany(d => d.GetModules(key)).SingleOrDefault();
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
            return engine.Execute<T>(source, engine.CreateScope(Make.Dictionary(arguments)));
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
    }
}
