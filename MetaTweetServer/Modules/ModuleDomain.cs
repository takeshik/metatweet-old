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
using System.Collections;
using System.Collections.ObjectModel;
using XSpect.Collections;
using XSpect.Hooking;
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
using log4net;
using System.Text.RegularExpressions;

namespace XSpect.MetaTweet.Modules
{
    /// <summary>
    /// モジュール アセンブリを読み込み、モジュール オブジェクトを管理するための、独立した環境を提供します。
    /// </summary>
    /// <remarks>
    /// <para>モジュール ドメインは、<see cref="ModuleManager"/> によって作成される、モジュール アセンブリのための独立した環境です。<see cref="Add{TModule}(String, String)"/> メソッドを使用して新しいモジュール オブジェクトを生成し、<see cref="Remove{TModule}"/> メソッドを使用してそれを破棄することができます。</para>
    /// <para>モジュール オブジェクトは名前 (キー) と型によって一意に識別されます。型が異なる限りにおいて、同一の名前を使用できます。</para>
    /// <para>モジュール ドメインはドメインの名前と同一のディレクトリに対応します。対応先のディレクトリは <see cref="Directory"/> プロパティで参照できます。</para>
    /// </remarks>
    /// <seealso cref="ModuleManager"/>
    public class ModuleDomain
        : CodeDomain,
          ILoggable
    {
        /// <summary>
        /// アプリケーション ドメインおよび <see cref="CodeDomain"/> において、モジュール ドメインを示す接頭文字列を取得します。
        /// </summary>
        public const String Prefix = "Modules.";

        /// <summary>
        /// このモジュール ドメインの親である <see cref="ModuleManager"/> を取得します。
        /// </summary>
        /// <value>
        /// このモジュール ドメインの親である <see cref="ModuleManager"/>。
        /// </value>
        public new ModuleManager Parent
        {
            get
            {
                return base.Parent as ModuleManager;
            }
        }

        /// <summary>
        /// イベントを記録するログ ライタを取得します。
        /// </summary>
        /// <value>
        /// イベントを記録するログ ライタ。
        /// </value>
        public ILog Log
        {
            get
            {
                return this.Parent.Log;
            }
        }

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

        /// <summary>
        /// <see cref="Add(String, String, FileInfo)"/> のフックを取得します。
        /// </summary>
        /// <value>
        /// <see cref="Add(String, String, FileInfo)"/> のフック。
        /// </value>
        public FuncHook<ModuleDomain, String, String, FileInfo, IModule> AddHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="Remove{TModule}"/> のフックを取得します。
        /// </summary>
        /// <value>
        /// <see cref="Remove{TModule}"/> のフック。
        /// </value>
        public ActionHook<ModuleDomain, String, Type> RemoveHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="ModuleDomain"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="parent">モジュール ドメインを生成する、親となる <see cref="ModuleManager"/>。</param>
        /// <param name="domainName">モジュール ドメインの名前、すなわち、参照するディレクトリの名前。</param>
        public ModuleDomain(ModuleManager parent, String domainName)
            : base(
                  parent,
                  Prefix + domainName,
                  parent.Parent.Directories.BaseDirectory.FullName,
                  Make.Array(
                      new Uri(parent.Parent.Directories.BaseDirectory.FullName + "/")
                          .MakeRelativeUri(new Uri(parent.Parent.Directories.LibraryDirectory.FullName)),
                      new Uri(parent.Parent.Directories.BaseDirectory.FullName + "/")
                          .MakeRelativeUri(new Uri(parent.Parent.Directories.ModuleDirectory.Directory(domainName).FullName))
                  ).Select(u => u.ToString()),
                  info => info.ConfigurationFile = parent.Parent.Directories.ConfigDirectory
                      .Directory("modules.d")
                      .File(domainName + ".config")
                      .If(f => f.Exists, f => f.FullName, f => null)
              )
        {
            this.Directory = this.Parent.Parent.Directories.ModuleDirectory.Directory(domainName);
            this.Modules = new HybridDictionary<Tuple<String, String>, IModule>(
                (i, m) => Make.Tuple(m.Name, m.CreateObjRef(typeof(IModule)).TypeInfo.TypeName)
            );
            this.Modules.ItemsRemoved += (sender, e) => e.OldElements.ForEach(_ => _.Value.Dispose());
            this.Modules.ItemsReset += (sender, e) => e.OldElements.ForEach(_ => _.Value.Dispose());
            this.AddHook = new FuncHook<ModuleDomain, String, String, FileInfo, IModule>(this._Add);
            this.RemoveHook = new ActionHook<ModuleDomain, String, Type>(this._Remove);
        }

        /// <summary>
        /// <see cref="ModuleDomain"/> によって使用されているアンマネージ リソースを解放し、オプションでマネージ リソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 <c>true</c>、破棄されない場合は <c>false</c>。</param>
        protected override void Dispose(Boolean disposing)
        {
            this.Modules.Clear();
            base.Dispose(disposing);
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
        /// <returns>生成された型厳密なモジュール オブジェクト。</returns>
        public TModule Add<TModule>(String key, String typeName)
            where TModule : IModule
        {
            return (TModule) this.Add(key, typeName);
        }

        /// <summary>
        /// モジュール オブジェクトを生成します。
        /// </summary>
        /// <param name="key">モジュール オブジェクトに付ける名前。</param>
        /// <param name="typeName">生成するモジュール オブジェクトの完全な型名。</param>
        /// <returns>生成されたモジュール オブジェクト。</returns>
        public IModule Add(String key, String typeName)
        {
            return this.Add(
                key,
                typeName,
                this.Parent.Parent.Directories.ConfigDirectory.File(String.Format(
                    "modules.d/{0}-{1}.conf.xml",
                    typeName.Substring(typeName.LastIndexOf('.') + 1),
                    key
                ))
            );
        }

        /// <summary>
        /// モジュール オブジェクトを生成します。
        /// </summary>
        /// <param name="key">モジュール オブジェクトの名前。</param>
        /// <param name="typeName">生成するモジュール オブジェクトの完全な型名。</param>
        /// <param name="configFile">モジュール オブジェクトの初期化時に与える設定ファイル。</param>
        /// <returns>生成されたモジュール オブジェクト。</returns>
        public IModule Add(String key, String typeName, FileInfo configFile)
        {
            this.CheckIfDisposed();
            return this.AddHook.Execute(key, typeName, configFile);
        }

        private IModule _Add(String key, String typeName, FileInfo configFile)
        {
            Tuple<String, String> id = Make.Tuple(key, typeName);
            return this.Modules.ContainsKey(id)
                ? this.Modules[id]
                : (Activator.CreateInstance(
                      this.ApplicationDomain,
                      this.GetAssemblyByName(typeName).FullName,
                      typeName
                  ).Unwrap() as IModule)
                      .Let(
                          m => m.Register(
                              this,
                              key,
                              configFile.Null(f => XmlConfiguration.Load(f.FullName))
                          ),
                          this.Modules.Add
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
            this.RemoveHook.Execute(key, type);
        }

        private void _Remove(String key, Type type)
        {
            this.Modules.RemoveValue(this.GetModule(key, type).Let(m => m.Dispose()));
        }

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
            return this.GetModules(key, typeof(TModule)).OfType<TModule>();
        }

        /// <summary>
        /// 全てのモジュール オブジェクトを取得します。
        /// </summary>
        /// <returns>全てのモジュール オブジェクトのシーケンス。</returns>
        public IEnumerable<IModule> GetModules()
        {
            return this.Modules.Values;
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
                (type == null || m.CreateObjRef(typeof(IModule)).TypeInfo.CanCastTo(type, m))
            );
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
    }
}
