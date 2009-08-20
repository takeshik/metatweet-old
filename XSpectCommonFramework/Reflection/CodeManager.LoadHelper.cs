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
using System.Reflection;

namespace XSpect.Reflection
{
    partial class CodeManager
    {
        [Serializable()]
        protected sealed class LoadHelper
            : MarshalByRefObject
        {
            private enum ArgumentType
            {
                Unknown = 0,
                AssemblyName = 1,
                String = 2,
                ByteArray = 3,
                ByteArrayByteArray = 4,
            }

            private readonly AppDomain _domain;

            private readonly ArgumentType _argumentType;

            private readonly AssemblyName _assemblyRef;

            private readonly String _assemblyStringOrFile;

            private readonly Byte[] _rawAssembly;

            private readonly Byte[] _rawSymbolStore;

            private Assembly _assembly;

            private LoadHelper(AppDomain domain)
            {
                this._domain = domain;
            }

            public LoadHelper(AppDomain domain, AssemblyName assemblyRef)
                : this(domain)
            {
                this._argumentType = ArgumentType.AssemblyName;
                this._assemblyRef = assemblyRef;
            }

            public LoadHelper(AppDomain domain, String assemblyStringOrFile)
                : this(domain)
            {
                this._argumentType = ArgumentType.String;
                this._assemblyStringOrFile = assemblyStringOrFile;
            }

            public LoadHelper(AppDomain domain, Byte[] rawAssembly)
                : this(domain)
            {
                this._argumentType = ArgumentType.ByteArray;
                this._rawAssembly = rawAssembly;
            }

            public LoadHelper(AppDomain domain, Byte[] rawAssembly, Byte[] rawSymbolStore)
                : this(domain)
            {
                this._argumentType = ArgumentType.ByteArrayByteArray;
                this._rawAssembly = rawAssembly;
                this._rawSymbolStore = rawSymbolStore;
            }

            public Assembly Load()
            {
                switch (this._argumentType)
                {
                    case ArgumentType.AssemblyName:
                        this._domain.DoCallBack(() =>
                            this._assembly = Assembly.Load(this._assemblyRef));
                        break;
                    case ArgumentType.String:
                        this._domain.DoCallBack(() =>
                            this._assembly = Assembly.Load(this._assemblyStringOrFile));
                        break;
                    case ArgumentType.ByteArray:
                        this._domain.DoCallBack(() =>
                            this._assembly = Assembly.Load(this._rawAssembly));
                        break;
                    case ArgumentType.ByteArrayByteArray:
                        this._domain.DoCallBack(() =>
                            this._assembly = Assembly.Load(this._rawAssembly, this._rawSymbolStore));
                        break;
                }
                return this._assembly;
            }

            public Assembly LoadFile()
            {
                this._domain.DoCallBack(() =>
                    this._assembly = Assembly.LoadFile(this._assemblyStringOrFile));
                return this._assembly;
            }

            public Assembly LoadFrom()
            {
                this._domain.DoCallBack(() =>
                    this._assembly = Assembly.LoadFrom(this._assemblyStringOrFile));
                return this._assembly;
            }
        }
    }
}