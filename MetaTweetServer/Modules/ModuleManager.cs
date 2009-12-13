// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright c 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
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
using XSpect.Extension;
using XSpect.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Achiral;
using Achiral.Extension;
using log4net;

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
        : CodeManager,
          ILoggable
    {
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
        /// 現在管理されているモジュール ドメインのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// 現在管理されているモジュール ドメインのシーケンス。
        /// </value>
        public IEnumerable<ModuleDomain> ModuleDomains
        {
            get
            {
                return this.CodeDomains.OfType<ModuleDomain>();
            }
        }

        /// <summary>
        /// <see cref="Load"/> のフックを取得します。
        /// </summary>
        /// <value>
        /// <see cref="Load"/> のフック。
        /// </value>
        public Hook<ModuleManager, String> LoadHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="Unload"/> のフックを取得します。
        /// </summary>
        /// <value>
        /// <see cref="Unload"/> のフックを取得します。
        /// </value>
        public Hook<ModuleManager, String> UnloadHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="ModuleManager"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="parent">このオブジェクトを生成する、親となるオブジェクト。</param>
        /// <param name="configFile">設定ファイル。</param>
        public ModuleManager(ServerCore parent, FileInfo configFile)
            : base(configFile)
        {
            this.Parent = parent;
            this.LoadHook = new Hook<ModuleManager, String>();
            this.UnloadHook = new Hook<ModuleManager, String>();
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
        /// モジュール ドメインを取得します。
        /// </summary>
        /// <param domainName="key">モジュール ドメインの名前。</param>
        /// <returns>指定した名前を持つモジュール ドメイン。</returns>
        public new ModuleDomain this[String domainName]
        {
            get
            {
                return (ModuleDomain) this.CodeDomains[ModuleDomain.Prefix + domainName];
            }
        }

        /// <summary>
        /// モジュール ドメインを作成し、モジュール アセンブリをロードします。
        /// </summary>
        /// <param name="domainName">ドメインの名前、即ち、ロードするモジュール アセンブリを含んだディレクトリの名前。</param>
        /// <returns>モジュール アセンブリがロードされたモジュール ドメイン。</returns>
        public CodeDomain Load(String domainName)
        {
            return this.LoadHook.Execute((self, domainName_) =>
            {
                this.Add(new ModuleDomain(this, domainName));
                this[domainName_].Load();
                return this[domainName_];
            }, this, domainName);
        }

        /// <summary>
        /// モジュール ドメインをアンロードします。
        /// </summary>
        /// <param name="domainName">アンロードするモジュール ドメインの名前。</param>
        public void Unload(String domainName)
        {
            this.UnloadHook.Execute((self, domainName_) =>
                this.Remove(this.CodeDomains[ModuleDomain.Prefix + domainName]),
                this, domainName
            );
        }

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
                ? this[domain].GetModules(key, type)
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
            return this.ModuleDomains.SelectMany(d => d.GetModules(key, type));
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
                ? this[domain].GetModules<TModule>(key)
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
                ? this.ModuleDomains.SelectMany(d => d.GetModules<TModule>(key))
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
            return this.ModuleDomains.SelectMany(d => d.GetModules<TModule>());
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
                ? this[domain].GetModules(key)
                : this.GetModules(key);
        }

        /// <summary>
        /// モジュール オブジェクトを検索します。
        /// </summary>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュールのシーケンス。</returns>
        public IEnumerable<IModule> GetModules(String key)
        {
            return this.ModuleDomains.SelectMany(d => d.GetModules(key));
        }

        /// <summary>
        /// 全てのモジュール オブジェクトを取得します。
        /// </summary>
        /// <returns>全てのモジュールのシーケンス。</returns>
        public IEnumerable<IModule> GetModules()
        {
            return this.ModuleDomains.SelectMany(d => d.GetModules());
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
                ? this[domain].GetModule(key, type)
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
            return this.GetModules(key, type).Single();
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
                ? this[domain].GetModule<TModule>(key)
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
            return this.ModuleDomains.SelectMany(d => d.GetModules<TModule>(key)).Single();
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
                ? this[domain].GetModule(key)
                : this.GetModule(key);
        }

        /// <summary>
        /// モジュール オブジェクトを取得します。
        /// </summary>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>一意に特定されたモジュール。</returns>
        public IModule GetModule(String key)
        {
            return this.ModuleDomains.SelectMany(d => d.GetModules(key)).Single();
        }
    }
}
