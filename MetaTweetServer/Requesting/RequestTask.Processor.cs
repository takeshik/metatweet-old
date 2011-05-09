// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Achiral.Extension;
using XSpect.Extension;
using XSpect.MetaTweet.Modules;
using XSpect.MetaTweet.Objects;
using XSpect.MetaTweet.Properties;
using Achiral;
using System.Reflection;

namespace XSpect.MetaTweet.Requesting
{
    partial class RequestTask
    {
        internal class Processor
            : Object
        {
            private readonly RequestTask _task;

            private LinkedListNode<Fragment> _current;

            private Object _result;

            private StorageModule _storageModule;

            private StorageSession _session;

            public Processor(RequestTask task)
            {
                this._task = task;
                this._result = null;
            }

            public Object Process(IEnumerable<Fragment> target)
            {
                try
                {
                    this._current = new LinkedList<Fragment>(target).First;
                    while (this._current != null)
                    {
                        try
                        {
                            this.Prologue();
                            switch (this._task.CurrentFragment.Type)
                            {
                                case FragmentType.Flow:
                                    this.Process((FlowFragment) this._task.CurrentFragment);
                                    break;
                                case FragmentType.Code:
                                    this.Process((CodeFragment) this._task.CurrentFragment);
                                    break;
                                case FragmentType.Scope:
                                    this.Process((ScopeFragment) this._task.CurrentFragment);
                                    break;
                                default: // case FragmentType.Operator:
                                    this.Process((OperatorFragment) this._task.CurrentFragment);
                                    break;
                            }
                        }
                        catch (ThreadAbortException)
                        {
                            throw;
                        }
                        catch (Exception ex)
                        {
                            this._result = ex;
                            do
                            {
                                this._current = this._current.Next;
                            } while (!(this._current == null || this._current.Value is OperatorFragment));
                        }
                        this.Epilogue();
                    }
                    return this._result;
                }
                finally
                {
                    if (this._session != null)
                    {
                        this._session.Dispose();
                    }
                }
            }

            private void Prologue()
            {
                this._task.CurrentFragment = this._current.Value;
                this._task.CurrentFragment.Variables.Do(p => this._task.Variables[p.Key] = p.Value);
                if (this._storageModule == null || this._storageModule.Name != this.Variable<String>("storage"))
                {
                    if (this._session != null)
                    {
                        this._session.Dispose();
                        this._session = null;
                    }
                    this._storageModule = this._task.Parent.Parent.ModuleManager
                        .GetModule<StorageModule>(this.Variable<String>("storage"));
                }
                if (this._session == null)
                {
                    this._session = this._storageModule.OpenSession();
                }
            }

            private void Epilogue()
            {
                ++this._task.StepCount;
                if (this._task.State == RequestTaskState.WaitForPause)
                {
                    this._task.State = RequestTaskState.Paused;
                    this._task._signal.WaitOne();
                    this._task.State = RequestTaskState.Running;
                }
            }

            private void Process(FlowFragment fragment)
            {
                this._result = this._task.Parent.Parent.ModuleManager
                    .GetModule<FlowModule>(fragment.FlowName ?? Variable<String>("flow"))
                    .Perform(fragment.Selector, this._result, this._session, fragment.Arguments, this._task.Variables);
                if (fragment.FlowName != null)
                {
                    this._task.Variables["flow"] = fragment.FlowName;
                }
                this._current = this._current.Next;
            }

            private void Process(CodeFragment fragment)
            {
                this._result = TriDQL.ParseLambda(this._result.GetType(), null, fragment.Code, this._task.Variables)
                    .Compile()
                    .DynamicInvoke(this._result);
                this._current = this._current.Next;
            }

            private void Process(ScopeFragment fragment)
            {
                this._result = this.Process(fragment.Fragments);
                this._current = this._current.Next;
            }

            private void Process(OperatorFragment fragment)
            {
                switch (fragment.Name)
                {
                    case "back":
                        this._current = this._current.Previous;
                        break;
                    case "redo":
                        this._current = this._current.List.First;
                        break;
                    case "break":
                        this._current = this._current.List.Last;
                        break;
                    default:
                        this._current = this._current.Next;
                        break;
                }
            }

            private T Variable<T>(String key)
            {
                return (T) this._task.Variables[key];
            }

            private Nullable<Boolean> GetResultType()
            {
                return this._result is Exception
                    ? default(Nullable<Boolean>)
                    : this._result == null
                          || (this._result is String && String.IsNullOrEmpty((String) this._result))
                          || (this._result is IEnumerable && ((IEnumerable) this._result).Cast<Object>().Any())
                          || (this._result.GetType().GetInterface("System.IObservable`1") != null &&
                                 ((IObservable<Object>) typeof(Processor)
                                     .GetMethod("AsObject", BindingFlags.NonPublic | BindingFlags.Static)
                                     .MakeGenericMethod(this._result.GetType().GetInterface("System.IObservable`1").GetGenericArguments())
                                     .Invoke(null, Make.Array(this._result))
                                 ).Any().First()
                             );
            }

            private static IObservable<Object> AsObject<TSource>(IObservable<TSource> source)
            {
                return source.Select(_ => (Object) _);
            }
        }
    }
}