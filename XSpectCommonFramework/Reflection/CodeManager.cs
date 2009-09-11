// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* XSpect Common Framework - Generic utility class library
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of XSpect Common Framework.
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Achiral.Extension;
using Microsoft.Scripting.Hosting;
using XSpect.Collections;
using XSpect.Configuration;
using XSpect.Extension;
using System.Reflection;

namespace XSpect.Reflection
{
    public class CodeManager
        : MarshalByRefObject,
          ICollection<CodeDomain>,
          IDisposable
    {
        private Boolean _disposed;

        public CodeDomain this[String key]
        {
            get
            {
                return this.CodeDomains[key];
            }
        }

        public KeyedCollection<String, CodeDomain> CodeDomains
        {
            get;
            private set;
        }

        public ScriptRuntime ScriptRuntime
        {
            get;
            private set;
        }

        public XmlConfiguration Configuration
        {
            get;
            set;
        }

        public ICollection<LanguageSetting> Languages
        {
            get;
            set;
        }

        public String DefaultApplicationBase
        {
            get;
            set;
        }

        public IEnumerable<String> DefaultPrivateBinPaths
        {
            get;
            set;
        }

        public IEnumerable<AssemblyName> DefaultAssemblies
        {
            get;
            set;
        }

        public IEnumerable<AssemblyName> AdditionalAssembliesForTemporary
        {
            get;
            set;
        }

        public CodeManager(FileInfo configFile)
        {
            this.Configuration = XmlConfiguration.Load(configFile);
            this.Languages = new List<LanguageSetting>();
            this.CodeDomains = new DisposableKeyedCollection<String, CodeDomain>(d => d.Key);
            this.Setup();
        }

        ~CodeManager()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(Boolean disposing)
        {
            this._disposed = true;
            // Clear -> ClearItems == Dispose.
            this.CodeDomains.Clear();
        }

        protected void CheckIfDisposed()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException("this");
            }
        }

        public void Add(CodeDomain item)
        {
            this.DefaultAssemblies.ForEach(ar => item.Load(ar));
            this.CodeDomains.Add(item);
        }

        public CodeDomain Add(String key)
        {
            return this.Add(key, this.DefaultPrivateBinPaths);
        }

        public CodeDomain Add(String key, IEnumerable<String> privateBinPaths)
        {
            return this.Add(key, this.DefaultApplicationBase, privateBinPaths);
        }

        public CodeDomain Add(String key, String applicationBase, IEnumerable<String> privateBinPaths)
        {
            return new CodeDomain(this, key, applicationBase, privateBinPaths)
                .Do(d => this.DefaultAssemblies.ForEach(ar => d.Load(ar)))
                .Do(this.Add);
        }

        public void Clear()
        {
            this.CodeDomains.Clear();
        }

        public Boolean Contains(CodeDomain item)
        {
            return this.CodeDomains.Contains(item);
        }

        public void CopyTo(CodeDomain[] array, Int32 arrayIndex)
        {
            this.CodeDomains.CopyTo(array, arrayIndex);
        }

        public Boolean Remove(CodeDomain item)
        {
            return this.CodeDomains.Remove(item);
        }

        public Int32 Count
        {
            get
            {
                return this.CodeDomains.Count;
            }
        }

        public Boolean IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public IEnumerator<CodeDomain> GetEnumerator()
        {
            return this.CodeDomains.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        protected virtual void Setup()
        {
            this.Languages.AddRange(this.Configuration.ResolveChild("languages")
                .OfValueType<LanguageSetting>()
            );
            this.ScriptRuntime = new ScriptRuntime(new ScriptRuntimeSetup().Do(setup =>
            {
                setup.LanguageSetups.AddRange(this.Languages
                    .Where(l => l.IsDynamicLanguage)
                    .Select(l => new LanguageSetup(
                        l.Type.AssemblyQualifiedName,
                        l.Name,
                        l.Identifiers,
                        l.Extensions
                    ))
                );
                setup.DebugMode = this.Configuration.ResolveValue<Boolean>("dlr", "debug");
                setup.HostArguments = this.Configuration.ResolveValue<List<Object>>("dlr", "arguments");
                setup.Options.AddRange(
                    this.Configuration.ResolveValue<List<Struct<String, Object>>>("dlr", "options")
                        .Select(s => Create.KeyValuePair(s.Item1, s.Item2))
                );
            }));
            this.DefaultApplicationBase = this.Configuration.ResolveValue<String>("default", "appbase")
                .If(String.IsNullOrEmpty, AppDomain.CurrentDomain.BaseDirectory);
            this.DefaultAssemblies = this.Configuration.ResolveValue<List<String>>("default", "assemblies")
                .Select(s => new AssemblyName(s));
            this.AdditionalAssembliesForTemporary = this.Configuration.ResolveValue<List<String>>("default", "assembliesForTemp")
                .Select(s => new AssemblyName(s));
        }

        public CodeDomain Add()
        {
            return this.Add("temp_" + Guid.NewGuid().ToString("d"))
                .Do(d => this.AdditionalAssembliesForTemporary.ForEach(ar => d.Load(ar)));
        }

        public LanguageSetting GetLanguage(String language)
        {
            return this.Languages.Single(l =>
                l.Name == language ||
                l.Identifiers.Contains(language) ||
                l.Extensions.Contains(language)
            );
        }

        public CodeDomain Clone(String key, String keyCloning)
        {
            AppDomain domain = this.CodeDomains[keyCloning].ApplicationDomain;
            return this.Add(key, domain.BaseDirectory, domain.SetupInformation.PrivateBinPath.Split(';'))
                .Do(d => domain.GetAssemblies().ForEach(a => d.Load(a.GetName())));
        }

        #region Execute

        public T Execute<T>(
            LanguageSetting language,
            String source,
            IDictionary<String, Object> arguments
        )
        {
            return this.Add().Dispose(d => d.Execute<T>(language, source, arguments));
        }

        public Object Execute(
            LanguageSetting language,
            String source,
            IDictionary<String, Object> arguments
        )
        {
            return this.Execute<Object>(language, source, arguments);
        }

        public T Execute<T>(
            String language,
            String source,
            IDictionary<String, Object> arguments
        )
        {
            return this.Execute<T>(this.GetLanguage(language), source, arguments);
        }

        public Object Execute(
            String language,
            String source,
            IDictionary<String, Object> arguments
        )
        {
            return this.Execute<Object>(language, source, arguments);
        }

        public T Execute<T>(
            FileInfo file,
            IDictionary<String, Object> arguments
        )
        {
            return this.Execute<T>(file.Extension, file.ReadAllText(), arguments);
        }

        public Object Execute(
            FileInfo file,
            IDictionary<String, Object> arguments
        )
        {
            return this.Execute<Object>(file.Extension, file.ReadAllText(), arguments);
        }

        public T Execute<T>(
            String path,
            IDictionary<String, Object> arguments
        )
        {
            return this.Execute<T>(new FileInfo(path), arguments);
        }

        public Object Execute(
            String path,
            IDictionary<String, Object> arguments
        )
        {
            return this.Execute<Object>(new FileInfo(path), arguments);
        }

        #endregion
    }
}