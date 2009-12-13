// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* XSpect Common Framework - Generic utility class library
 * Copyright c 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
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
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using Achiral;
using Achiral.Extension;
using XSpect;
using XSpect.Collections;
using XSpect.Extension;

namespace XSpect.Reflection
{
    [Serializable()]
    public partial class CodeDomain
        : MarshalByRefObject,
          IDisposable
    {
        private Boolean _disposed;

        private readonly GeneralKeyedCollection<AssemblyName, Assembly> _assemblies;

        public CodeManager Parent
        {
            get;
            private set;
        }

        public String Key
        {
            get;
            private set;
        }

        public AppDomain ApplicationDomain
        {
            get;
            private set;
        }

        public IEnumerable<Assembly> Assemblies
        {
            get
            {
                return this._assemblies;
            }
        }

        public CodeDomain(CodeManager parent, String key, AppDomainSetup info)
        {
            this._assemblies = new GeneralKeyedCollection<AssemblyName, Assembly>(a => a.GetName());
            this.Parent = parent;
            this.Key = key;
            this.ApplicationDomain = AppDomain.CreateDomain("CodeMgr." + key, null, info);
        }

        public CodeDomain(CodeManager parent, String key, String applicationBase, IEnumerable<String> privateBinPaths)
            : this(parent, key, new AppDomainSetup()
              {
                  ApplicationBase = applicationBase,
                  ApplicationName = "CodeManager." + key,
                  LoaderOptimization = LoaderOptimization.MultiDomainHost,
                  PrivateBinPath = privateBinPaths != null ? privateBinPaths.Join(";") : null,
                  PrivateBinPathProbe = "true",
              })
        {
        }

        ~CodeDomain()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(Boolean disposing)
        {
            if (!this._disposed)
            {
                AppDomain.Unload(this.ApplicationDomain);
            }
            this._disposed = true;
        }

        protected void CheckIfDisposed()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }

        private Assembly RegisterAssembly(Assembly assembly)
        {
            this._assemblies.Add(assembly);
            return assembly;
        }

        #region Load / LoadFile / LoadFrom

        public Assembly Load(AssemblyName assemblyRef)
        {
            this.CheckIfDisposed();
            return RegisterAssembly(
                new LoadHelper(this.ApplicationDomain, assemblyRef).Load()
            );
        }

        public Assembly Load(String assemblyString)
        {
            this.CheckIfDisposed();
            return RegisterAssembly(
                new LoadHelper(this.ApplicationDomain, assemblyString).Load()
            );
        }

        public Assembly Load(Byte[] rawAssembly)
        {
            this.CheckIfDisposed();
            return RegisterAssembly(
                new LoadHelper(this.ApplicationDomain, rawAssembly).Load()
            );
        }

        public Assembly Load(Byte[] rawAssembly, Byte[] rawSymbolStore)
        {
            this.CheckIfDisposed();
            return RegisterAssembly(
                new LoadHelper(this.ApplicationDomain, rawAssembly, rawSymbolStore).Load()
            );
        }

        public Assembly LoadFile(String path)
        {
            this.CheckIfDisposed();
            return RegisterAssembly(
                new LoadHelper(this.ApplicationDomain, path).LoadFile()
            );
        }

        public Assembly LoadFrom(String assemblyFile)
        {
            this.CheckIfDisposed();
            return RegisterAssembly(
                new LoadHelper(this.ApplicationDomain, assemblyFile).LoadFrom()
            );
        }

        #endregion

        #region Compile

        private Assembly Compile(LanguageSetting language, String source, Boolean generateInMemory)
        {
            this.CheckIfDisposed();
            return RegisterAssembly(new CompileHelper(
                this.ApplicationDomain,
                language.Type
                    .GetConstructor(Create.TypeArray<IDictionary<String, String>>())
                    .Invoke(Make.Array(language.Options)) as CodeDomProvider,
                new CompilerParameters(
                    this.ApplicationDomain.GetAssemblies()
                    .Select(a => a.GetName().FullName)
                    .ToArray()
                )
                {
                    GenerateInMemory = generateInMemory,
                },
                source
            ).Compile());
        }

        public Assembly Compile(LanguageSetting language, String source)
        {
            return this.Compile(language, source, false);
        }

        public Assembly Compile(String language, String source)
        {
            return this.Compile(this.Parent.GetLanguage(language), source);
        }

        public Assembly Compile(FileInfo file)
        {
            return this.Compile(file.Extension, file.ReadAllText());
        }

        public Assembly Compile(String file)
        {
            return this.Compile(new FileInfo(file));
        }

        #endregion

        #region Execute

        public T Execute<T>(
            LanguageSetting language,
            String source,
            IDictionary<String, Object> arguments
        )
        {
            this.CheckIfDisposed();
            return language.IsDynamicLanguage
                ? this.Parent.ScriptRuntime
                      .GetEngineByTypeName(language.Type.AssemblyQualifiedName)
                      .Do(e => e.CreateScriptSourceFromString(source, SourceCodeKind.File)
                          .Execute<T>(e.CreateScope()
                              .Do(s => arguments.ForEach(p => s.SetVariable(p.Key, p.Value)))
                          )
                      )
                : (T) this.Compile(language, source, true).GetTypes()
                      .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static))
                      .Where(m => m.GetParameters()
                          .Select(p => p.ParameterType)
                          .SequenceEqual(Make.Sequence(typeof(IDictionary<String, Object>)))
                      )
                      .SingleOrPredicatedSingle(m => m.Name == "Begin")
                      .Invoke(null, Make.Array(arguments));
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
            return this.Execute<T>(this.Parent.GetLanguage(language), source, arguments);
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
            return this.Execute<T>(this.Parent.GetLanguage(file.Extension), file.ReadAllText(), arguments);
        }

        public Object Execute(
            FileInfo file,
            IDictionary<String, Object> arguments
        )
        {
            return this.Execute<Object>(this.Parent.GetLanguage(file.Extension), file.ReadAllText(), arguments);
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