// -*- mode: csharp; encoding: utf-8; -*-
/* XSpect Common Framework - Generic Utility Class Library
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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace XSpect
{
    public class ExceptionHandler
        : Object
    {
        private readonly Exception _exception;

        private String _indent;

        public Exception Exception
        {
            get
            {
                return this._exception;
            }
        }


        public ExceptionHandler(Exception ex)
        {
            this._indent = String.Empty;
            this._exception = ex;
        }

        public virtual String GetDiagnosticMessage()
        {
            return String.Format(
                #region Here Document
@"XSpect Global Exception Handler Information

{0}
{1}
{2}
",
                #endregion
                this.GetSystemInformation(),
                this.GetExceptionInformation(this._exception),
                this.GetDefaultAppDomainInformation()
            );
        }

        protected void Indent(Int32 n)
        {
            this._indent += new String(' ', 4 * n);
        }

        protected void Unindent(Int32 n)
        {
            this._indent = this._indent.Remove(0, 4 * n);
        }

        protected virtual String GetSystemInformation()
        {
            this.Indent(1);
            String systemInfo = String.Format(
                #region Here Document
@"SystemInformation:
{0}OperatingSystem = {1}
{0}RuntimeVersion = {2}
{0}Uptime = {3}
",
            #endregion
                this._indent,
                Environment.OSVersion.VersionString,
                Environment.Version.ToString(),
                new TimeSpan((long) Environment.TickCount * 10000).ToString()
            );
            this.Unindent(1);
            return systemInfo;
        }
        
        protected virtual String GetExceptionInformation(Exception exception)
        {
            String exceptionInfo = "ExceptionStack:\r\n";

            IEnumerable<Exception> exceptions = new Exception[] { exception, };
            while (exceptions.Last().InnerException != null)
            {
                exceptions = exceptions.Concat(new Exception[] { exception.InnerException, });
            }
            exceptions = exceptions.Reverse();

            this.Indent(1);
            foreach (Exception ex in exceptions)
            {
                exceptionInfo += String.Format(
                    "{0}{1}{2}:\r\n",
                    this._indent,
                    !ex.GetType().IsGenericType
                        ? String.Format("[{0}]", ex.GetType().Assembly.GetName().Name)
                        : String.Empty
                    ,
                    ex.GetType().FullName
                );
                this.Indent(1);
                if (!String.IsNullOrEmpty(ex.Message))
                {
                    exceptionInfo += String.Format(
                        "{0}Message = {1}\r\n",
                        this._indent,
                        ex.Message
                    );
                }
                if (ex.Data != null && ex.Data.Count > 0)
                {
                    exceptionInfo += String.Format(
                        "{0}Data:\r\n",
                        this._indent
                    );
                    foreach (String key in ex.Data.Keys)
                    {
                        foreach (String value in ex.Data.Values)
                        {
                            exceptionInfo += String.Format(
                                "{0}{1} = {2}\r\n",
                                key,
                                value
                            );
                        }
                    }
                }
                if (!String.IsNullOrEmpty(ex.HelpLink))
                {
                    exceptionInfo += String.Format(
                        "{0}HelpLink = {1}\r\n",
                        this._indent,
                        ex.HelpLink
                    );
                }
                if (!String.IsNullOrEmpty(ex.Source))
                {
                    exceptionInfo += String.Format(
                        "{0}Source = {1}\r\n",
                        this._indent,
                        ex.Source
                    );
                }
                if (ex.TargetSite != null)
                {
                    exceptionInfo += String.Format(
                        "{0}TargetSite = {1}\r\n",
                        this._indent,
                        this.GetMethodSignature(ex.TargetSite)
                    );
                }

                #region StackTrace
                String stacktraceInformation = "";
                StackTrace stackTrace = new StackTrace(ex, true);
                if (stackTrace.FrameCount > 0)
                {
                    exceptionInfo += String.Format(
                        "{0}StackTrace:\r\n",
                        this._indent
                    );
                    this.Indent(1);

                    foreach (StackFrame frame in stackTrace.GetFrames())
                    {
                        String offsets = "";
                        if (frame.GetILOffset() != StackFrame.OFFSET_UNKNOWN || frame.GetNativeOffset() != StackFrame.OFFSET_UNKNOWN)
                        {
                            this.Indent(1);
                            offsets = String.Format(
                                "{0}at ",
                                this._indent
                            );
                            Int32 offset;
                            if ((offset = frame.GetILOffset()) != StackFrame.OFFSET_UNKNOWN)
                            {
                                offsets += String.Format("IL + 0x{0:x}", offset);
                            }
                            if (offsets.Contains("IL"))
                            {
                                offsets += ", ";
                            }

                            if ((offset = frame.GetNativeOffset()) != StackFrame.OFFSET_UNKNOWN)
                            {
                                offsets += String.Format("Native + 0x{0:x}", offset);
                            }
                            this.Unindent(1);
                        }

                        stacktraceInformation += String.Format(
                            "{0}{1}\r\n{2}\r\n",
                            this._indent,
                            this.GetMethodSignature(frame.GetMethod()),
                            offsets
                        );

                        if (frame.GetFileLineNumber() > 0)
                        {
                            this.Indent(1);
                            stacktraceInformation += String.Format(
                                "{0}in {1}({2}, {3})\r\n",
                                this._indent,
                                frame.GetFileName(),
                                frame.GetFileLineNumber(),
                                frame.GetFileColumnNumber()
                            );
                            this.Unindent(1);
                        }
                    }
                    exceptionInfo += stacktraceInformation;
                    this.Unindent(1);
                }
                #endregion
                this.Unindent(1);
            }
            this.Unindent(1);
            return exceptionInfo;
        }

        protected virtual String GetDefaultAppDomainInformation()
        {
            String appDomainInfo = "CurrentAppDomain:\r\n";

            this.Indent(1);
            appDomainInfo += String.Format(
                "{0}LoadedAssemblies:\r\n",
                this._indent
            );
            this.Indent(1);
            AppDomain domain = AppDomain.CurrentDomain;
            foreach (Assembly assembly in domain.GetAssemblies())
            {
                appDomainInfo += String.Format(
                    "{0}{1}, ProcessorArchitecture={2}\r\n",
                    this._indent,
                    assembly.GetName().ToString(),
                    Enum.GetName(typeof(ProcessorArchitecture), assembly.GetName().ProcessorArchitecture).ToLower()
                );
                this.Indent(1);

                if (!String.IsNullOrEmpty(assembly.CodeBase))
                {
                    appDomainInfo += String.Format(
                        "{0}CodeBase = {1}\r\n",
                        this._indent,
                        assembly.CodeBase
                    );
                }
                appDomainInfo += String.Format(
                    "{0}GlobalAssemblyCache = {1}\r\n",
                    this._indent,
                    assembly.GlobalAssemblyCache.ToString().ToLower()
                );
                if (File.Exists(assembly.Location))
                {
                    FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

                    if (!String.IsNullOrEmpty(versionInfo.FileVersion))
                    {
                        appDomainInfo += String.Format(
                            "{0}FileVersion = {1}\r\n",
                            this._indent,
                            versionInfo.FileVersion
                        );
                    }
                    if (!String.IsNullOrEmpty(versionInfo.Comments))
                    {
                        appDomainInfo += String.Format(
                            "{0}Comments = {1}\r\n",
                            this._indent,
                            versionInfo.Comments
                        );
                    }
                    if (!String.IsNullOrEmpty(versionInfo.CompanyName))
                    {
                        appDomainInfo += String.Format(
                            "{0}CompanyName = {1}\r\n",
                            this._indent,
                            versionInfo.CompanyName
                        );
                    }
                    if (!String.IsNullOrEmpty(versionInfo.FileDescription))
                    {
                        appDomainInfo += String.Format(
                            "{0}FileDescription = {1}\r\n",
                            this._indent,
                            versionInfo.FileDescription
                        );
                    }
                    if (!String.IsNullOrEmpty(versionInfo.LegalCopyright))
                    {
                        appDomainInfo += String.Format(
                            "{0}LegalCopyright = {1}\r\n",
                            this._indent,
                            versionInfo.LegalCopyright
                        );
                    }
                    if (!String.IsNullOrEmpty(versionInfo.LegalTrademarks))
                    {
                        appDomainInfo += String.Format(
                            "{0}LegalTrademarks = {1}\r\n",
                            this._indent,
                            versionInfo.LegalTrademarks
                        );
                    }
                    if (!String.IsNullOrEmpty(versionInfo.Language))
                    {
                        appDomainInfo += String.Format(
                            "{0}Language = {1}\r\n",
                            this._indent,
                            versionInfo.Language
                        );
                    }
                    if (!String.IsNullOrEmpty(versionInfo.InternalName))
                    {
                        appDomainInfo += String.Format(
                            "{0}InternalName = {1}\r\n",
                            this._indent,
                            versionInfo.InternalName
                        );
                    }
                    if (!String.IsNullOrEmpty(versionInfo.OriginalFilename))
                    {
                        appDomainInfo += String.Format(
                            "{0}OriginalFileName = {1}\r\n",
                            this._indent,
                            versionInfo.OriginalFilename
                        );
                    }
                }
                this.Unindent(1);
            }
            this.Unindent(2);
            return appDomainInfo;
        }

        protected virtual String GetMethodSignature(MethodBase method)
        {
            StringBuilder genericParam = new StringBuilder();
            if (method.IsGenericMethod)
            {
                genericParam.Append("<");
                foreach (Type type in method.GetGenericArguments())
                {
                    genericParam.AppendFormat("{0}, ", type.Name);
                }
                genericParam.Remove(genericParam.Length - 2, 2).Append(">");
            }

            StringBuilder parameters = new StringBuilder();
            foreach (ParameterInfo parameter in method.GetParameters())
            {
                parameters.AppendFormat(
                    "{0} : {1}, ",
                    parameter.Name,
                    !parameter.ParameterType.IsGenericParameter
                        ? String.Format(
                              "[{0}]{1}",
                              parameter.ParameterType.Assembly.GetName().Name,
                              parameter.ParameterType.FullName
                          )
                        : String.Format("{0}", parameter.ParameterType.Name)
                );
            }

            if (parameters.Length > 0)
            {
                parameters.Remove(parameters.Length - 2, 2);
            }

            return String.Format(
                "{0}{1}{2}{3}{4}({5}){6}",
                !method.ReflectedType.IsGenericType
                    ? String.Format("[{0}]", method.ReflectedType.Assembly.GetName().Name)
                    : String.Empty
                ,
                method.ReflectedType.FullName,
                method.IsStatic ? "::" : "#",
                method.Name,
                genericParam,
                parameters,
                method.IsConstructor ? "" : String.Format(
                    " : {0}{1}",
                    !((MethodInfo) method).ReturnType.IsGenericType
                        ? String.Format("[{0}]", ((MethodInfo) method).ReturnType.Assembly.GetName().Name)
                        : String.Empty
                    ,
                    ((MethodInfo) method).ReturnType
                )
            );
        }
    }
}
