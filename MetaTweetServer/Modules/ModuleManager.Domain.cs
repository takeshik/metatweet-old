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
using System.Collections;
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

#if FUTURE
namespace XSpect.MetaTweet.Modules
{
    partial class ModuleManager
    {
        public new sealed class Domain
            : CodeManager.Domain
        {
            public new ModuleManager Parent
            {
                get
                {
                    return base.Parent as ModuleManager;
                }
            }

            public ModuleCollection Modules
            {
                get;
                private set;
            }

            public DirectoryInfo Directory
            {
                get;
                private set;
            }

            public Hook<Domain, String, String, FileInfo> AddHook
            {
                get;
                private set;
            }

            public Domain(ModuleManager parent, String domainName)
                : base(
                      parent,
                      ModulePrefix + domainName,
                      parent.Parent.Directories.BaseDirectory.FullName,
                      Make.Array(
                          new Uri(parent.Parent.Directories.BaseDirectory.FullName + "/")
                              .MakeRelativeUri(new Uri(parent.Parent.Directories.LibraryDirectory.FullName)),
                          new Uri(parent.Parent.Directories.BaseDirectory.FullName + "/")
                              .MakeRelativeUri(new Uri(parent.Parent.Directories.ModuleDirectory.Directory(domainName).FullName))
                      ).Select(u => u.ToString())
                  )
            {
                this.Directory = this.Parent.Parent.Directories.ModuleDirectory.Directory(domainName);
            }

            public override void Dispose(Boolean disposing)
            {
                this.Modules.Dispose();
                base.Dispose(disposing);
            }

            public void Load()
            {
                this.CheckIfDisposed();
                this.Directory.GetFiles("*.dll")
                    .Concat(this.Directory.GetFiles(".exe"))
                    .ForEach(f => this.LoadFrom(f.FullName));
            }

            public void Unload()
            {
                this.Dispose();
            }

            public TModule Add<TModule>(String key, String typeName)
                where TModule : IModule
            {
                return (TModule) this.Add(
                    key,
                    typeName,
                    this.Parent.Parent.Directories.ConfigDirectory.File(String.Format(
                        "modules.d/{0}-{1}.conf.xml",
                        typeName.Substring(typeName.LastIndexOf('.') + 1),
                        key
                    ))
                );
            }

            public IModule Add(String key, String typeName, FileInfo configFile)
            {
                this.CheckIfDisposed();
                return this.AddHook.Execute((self, key_, typeName_, configFile_) =>
                {
                    Tuple<String, Type> id = Make.Tuple(key, Type.GetType(typeName));

                    return this.Modules.Contains(id)
                               ? this.Modules[id]
                               : (Activator.CreateInstance(this.AppDomain, null, typeName).Unwrap() as IModule)
                                     .Do(m => m.Register(this.Parent.Parent, key))
                                     .Do(this.Modules.Add);
                }, this, key, typeName, configFile);
            }

            public void Remove<TModule>(String key)
                where TModule : IModule
            {
                this.Modules.Remove(this.GetModule<TModule>(key).Do(m => m.Dispose()));
            }

            public TModule GetModule<TModule>(String key)
                where TModule : IModule
            {
                return this.GetModules<TModule>(key).Single();
            }

            public IModule GetModule(String key, Type type)
            {
                return this.GetModules(key, type).Single();
            }

            public IEnumerable<TModule> GetModules<TModule>()
                where TModule : IModule
            {
                return this.GetModules<TModule>(null);
            }

            public IEnumerable<TModule> GetModules<TModule>(String key)
                where TModule : IModule
            {
                return this.GetModules(key, typeof(TModule)).OfType<TModule>();
            }

            public IEnumerable<IModule> GetModules()
            {
                return this.Modules;
            }

            public IEnumerable<IModule> GetModules(String key)
            {
                return this.GetModules(key, null);
            }

            public IEnumerable<IModule> GetModules(Type type)
            {
                return this.GetModules(null, type);
            }

            public IEnumerable<IModule> GetModules(String key, Type type)
            {
                return this.Modules.Where(m =>
                    (key != null || m.Name == key) &&
                    (type != null || m.GetType().IsSubclassOf(type))
                );
            }
        }
    }
}
#endif