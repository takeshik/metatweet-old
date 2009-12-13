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
using System.Linq;
using System.Reflection;
using System.Threading;

namespace XSpect.Reflection
{
    partial class CodeDomain
    {
        [Serializable()]
        protected class CompileHelper
            : MarshalByRefObject
        {
            private readonly AppDomain _domain;

            private readonly CodeDomProvider _provider;

            private readonly CompilerParameters _parameters;

            private readonly String[] _sources;

            private CompilerResults _results;

            internal CompileHelper(
                AppDomain domain,
                CodeDomProvider provider,
                CompilerParameters parameters,
                params String[] sources
            )
            {
                this._domain = domain;
                this._provider = provider;
                this._parameters = parameters;
                this._sources = sources;
            }

            public Assembly Compile()
            {
                this._domain.DoCallBack(() =>
                {
                    this._results = this._provider.CompileAssemblyFromSource(this._parameters, this._sources);
                });
                if (this._results.Errors.HasErrors)
                {
                    String message = String.Empty;
                    foreach (CompilerError error in this._results.Errors)
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
                this._results.TempFiles.Delete();
                return this._results.CompiledAssembly;
            }
        }
    }
}