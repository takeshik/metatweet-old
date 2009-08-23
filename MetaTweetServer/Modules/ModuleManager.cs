// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
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
    public class ModuleManager
        : CodeManager,
          ILoggable
    {
        public ServerCore Parent
        {
            get;
            private set;
        }

        public IEnumerable<ModuleDomain> ModuleDomains
        {
            get
            {
                return this.CodeDomains.OfType<ModuleDomain>();
            }
        }

        public Hook<ModuleManager, String> LoadHook
        {
            get;
            private set;
        }

        public Hook<ModuleManager, String> UnloadHook
        {
            get;
            private set;
        }

        public ModuleManager(ServerCore parent, FileInfo configFile)
            : base(configFile)
        {
            this.Parent = parent;
            this.LoadHook = new Hook<ModuleManager, String>();
            this.UnloadHook = new Hook<ModuleManager, String>();
        }

        #region Implementation of ILoggable
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
        #endregion

        public new ModuleDomain this[String key]
        {
            get
            {
                return (ModuleDomain) this.CodeDomains[ModuleDomain.Prefix + key];
            }
        }

        public void Load(String domainName)
        {
            this.LoadHook.Execute((self, domainName_) => 
                this.Add(new ModuleDomain(this, domainName)),
                this, domainName
            );
        }

        public void Unload(String domainName)
        {
            this.UnloadHook.Execute((self, domainName_) =>
                this.Remove(this.CodeDomains[ModuleDomain.Prefix + domainName]),
                this, domainName
            );
        }

        /// <summary>
        /// モジュールを検索します。
        /// </summary>
        /// <param name="domain">モジュール アセンブリを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="type">モジュール オブジェクトの型を表すオブジェクト。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュールのシーケンス。</returns>
        protected virtual IEnumerable<IModule> GetModules(String domain, String key, Type type)
        {
            return this[domain].GetModules(key, type);
        }

        /// <summary>
        /// モジュールを検索します。
        /// </summary>
        /// <typeparam name="TModule">モジュール オブジェクトの型。</typeparam>
        /// <param name="domain">モジュール アセンブリを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュールのシーケンス。</returns>
        public IEnumerable<TModule> GetModules<TModule>(String domain, String key)
            where TModule : IModule
        {
            return this[domain].GetModules<TModule>(key);
        }

        /// <summary>
        /// モジュールを検索します。
        /// </summary>
        /// <typeparam name="TModule">モジュール オブジェクトの型。</typeparam>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュールのシーケンス。</returns>
        public IEnumerable<TModule> GetModules<TModule>(String key)
            where TModule : IModule
        {
            return this.ModuleDomains.SelectMany(d => d.GetModules<TModule>(key));
        }

        /// <summary>
        /// モジュールを検索します。
        /// </summary>
        /// <typeparam name="TModule">モジュール オブジェクトの型。</typeparam>
        /// <returns>条件に合致するモジュールのシーケンス。</returns>
        public IEnumerable<TModule> GetModules<TModule>()
            where TModule : IModule
        {
            return this.ModuleDomains.SelectMany(d => d.GetModules<TModule>());
        }

        /// <summary>
        /// モジュールを検索します。
        /// </summary>
        /// <param name="domain">モジュール アセンブリを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュールのシーケンス。</returns>
        public IEnumerable<IModule> GetModules(String domain, String key)
        {
            return this[domain].GetModules(key);
        }

        /// <summary>
        /// モジュールを検索します。
        /// </summary>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュールのシーケンス。</returns>
        public IEnumerable<IModule> GetModules(String key)
        {
            return this.ModuleDomains.SelectMany(d => d.GetModules(key));
        }

        /// <summary>
        /// 全てのモジュールの集合を取得します。
        /// </summary>
        /// <returns>全てのモジュールのシーケンス。</returns>
        public IEnumerable<IModule> GetModules()
        {
            return this.ModuleDomains.SelectMany(d => d.GetModules());
        }

        /// <summary>
        /// モジュールを取得します。
        /// </summary>
        /// <param name="domain">モジュール アセンブリを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="type">モジュール オブジェクトの型を表すオブジェクト。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>一意に特定されたモジュール。</returns>
        protected IModule GetModule(String domain, String key, Type type)
        {
            return this[domain].GetModule(key, type);
        }

        /// <summary>
        /// モジュールを取得します。
        /// </summary>
        /// <typeparam name="TModule">モジュール オブジェクトの型。</typeparam>
        /// <param name="domain">モジュール アセンブリを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>一意に特定されたモジュール。</returns>
        public TModule GetModule<TModule>(String domain, String key)
            where TModule : IModule
        {
            return this[domain].GetModule<TModule>(key);
        }

        /// <summary>
        /// モジュールを取得します。
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
        /// モジュールを取得します。
        /// </summary>
        /// <param name="domain">モジュール アセンブリを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>一意に特定されたモジュール。</returns>
        public IModule GetModule(String domain, String key)
        {
            return this[domain].GetModule(key);
        }

        /// <summary>
        /// モジュールを取得します。
        /// </summary>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>一意に特定されたモジュール。</returns>
        public IModule GetModule(String key)
        {
            return this.ModuleDomains.SelectMany(d => d.GetModules(key)).Single();
        }
    }
}
