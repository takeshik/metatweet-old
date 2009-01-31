﻿// -*- mode: csharp; encoding: utf-8; -*-
/* XSpect Common Framework - Generic Utility Class Library
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@xspect.org>
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
using System.Linq;
using System.Reflection;
using System.Threading;

namespace XSpect.Reflection
{
    public class AssemblyManager
        : MarshalByRefObject,
          IDisposable
    {
        private readonly Dictionary<String, AppDomain> _domains;

        private Dictionary<String, String> _defaultOptions;

        private CompilerParameters _defaultParameters;

        private Mutex _compileMutex = new Mutex();

        private CodeDomProvider _provider;

        private CompilerParameters _parameters;

        private String[] _sources;

        private CompilerResults _results;

        public AppDomain this[String key]
        {
            get
            {
                return key != null ? this._domains[key] : AppDomain.CurrentDomain;
            }
        }

        public Dictionary<String, String> DefaultOptions
        {
            get
            {
                return this._defaultOptions;
            }
        }

        public CompilerParameters DefaultParameters
        {
            get
            {
                return this._defaultParameters;
            }
        }

        public AssemblyManager()
        {
            this._domains = new Dictionary<String, AppDomain>();
            this._defaultOptions = new Dictionary<String, String>();
            this._defaultParameters = new CompilerParameters();
        }

        public AppDomain CreateDomain(String key)
        {
            AppDomain domain = AppDomain.CreateDomain(key);
            this._domains.Add(key, domain);
            return domain;
        }

        public void UnloadDomain(String key)
        {
            // FIXME: Sometimes this code locks running.
            AppDomain.Unload(this._domains[key]);
            this._domains.Remove(key);
        }

        public Assembly LoadAssembly(String key, AssemblyName assemblyRef)
        {
            return this[key].Load(assemblyRef);
        }

        public Assembly Compile(
            String key,
            String language,
            params String[] sources
        )
        {
            return this.Compile(key, language, this._defaultOptions, this._defaultParameters, sources);
        }

        public Assembly Compile(
            String key,
            String language,
            IDictionary<String, String> options,
            CompilerParameters parameters,
            params String[] sources
        )
        {
            parameters.OutputAssembly = key + ".dll";
            this._compileMutex.WaitOne();
            this._provider = this.GetCodeDomProvider(language, options);
            this._parameters = parameters;
            this._sources = sources;
            this._domains[key].DoCallBack(() =>
            {
                this._results = this._provider.CompileAssemblyFromSource(
                    this._parameters,
                    this._sources
                );
            });
            this._provider = null;
            this._parameters = null;
            this._sources = null;
            Assembly assembly = this.CheckResults(this._results);
            this._results = null;
            this._compileMutex.ReleaseMutex();
            return assembly;
        }

        public IEnumerable<Type> GetTypes(String key)
        {
            return this[key].GetAssemblies().SelectMany(a => a.GetTypes());
        }

        private Assembly CheckResults(
            CompilerResults results
        )
        {
            if (results.Errors.HasErrors)
            {
                String message = String.Empty;
                foreach (CompilerError error in results.Errors)
                {
                    message += String.Format(
                        "{0} ({1}, {2}) {3}: {4}{5}",
                        error.FileName,
                        error.Line,
                        error.Column,
                        error.ErrorNumber,
                        error.ErrorText,
                        Environment.NewLine
                    );
                }
                throw new InvalidOperationException(message);
            }
            return results.CompiledAssembly;
        }

        private CodeDomProvider GetCodeDomProvider(String language, IDictionary<String, String> options)
        {
            Type providerType = CodeDomProvider.GetCompilerInfo(
                language.StartsWith(".")
                    ? CodeDomProvider.GetLanguageFromExtension(language)
                    : language
            ).CodeDomProviderType;
            ConstructorInfo constructor;

            if ((constructor = providerType.GetConstructors().SingleOrDefault(c =>
                c.GetParameters().Any(p => p.ParameterType == typeof(IDictionary<String, String>))
            )) != null)
            {
                return constructor.Invoke(new Object[]
                {
                    options,
                }) as CodeDomProvider;
            }
            else
            {
                return providerType.GetConstructor(Type.EmptyTypes).Invoke(new Object[]
                {
                }) as CodeDomProvider;
            }
        }

        public void Dispose()
        {
            foreach (AppDomain domain in this._domains.Values)
            {
                AppDomain.Unload(domain);
            }
            this._domains.Clear();
            this._compileMutex.Close();
        }
    }
}