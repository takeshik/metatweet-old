// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
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
using System.Linq.Expressions;
using log4net;
using XSpect.Hooking;
using XSpect.MetaTweet.Modules;
using XSpect.Configuration;
using XSpect.MetaTweet.Properties;
using XSpect.Extension;
using Achiral;
using Achiral.Extension;
using System.Reflection;

namespace XSpect.MetaTweet
{
    /// <summary>
    /// MetaTweet サーバの既定の初期化動作を提供します。
    /// </summary>
    /// <remarks>
    /// MetaTweet サーバは、自身と同じディレクトリの <c>init.*</c> コード ファイルを検索します。ファイルが存在しない場合、<see cref="Initialize"/> メソッドが呼び出されます。存在する場合、コードはコンパイルされ、このクラスと置き換わる形で実行されます。
    /// </remarks>
    public class Initializer
        : Object
    {
        private static ServerCore _host;

        /// <summary>
        /// MetaTweet サーバを初期化します。
        /// </summary>
        /// <param name="host">初期化されるサーバ オブジェクト。</param>
        /// <param name="args">サーバ オブジェクトに渡された引数。</param>
        public static void Initialize(ServerCore host, IDictionary<String, String> args)
        {
            _host = host;
            _host.ModuleManager.LoadHook.After.Add((manager, domainKey) =>
            {
                manager.Log.InfoFormat(Resources.ModuleAssemblyLoaded, domainKey);
                manager[domainKey].AddHook.After.AddRange(
                    (domain, moduleKey, typeName, configFile) =>
                        manager.Log.InfoFormat(
                            Resources.ModuleObjectAdded, domainKey, moduleKey, typeName, configFile.Null(f => f.Name)
                        ),
                    (domain, moduleKey, typeName, configFile) =>
                        RegisterModuleHook(
                            manager.GetModules(domainKey, moduleKey).Single(m => m.GetType().FullName == typeName)
                        ),
                    (domain, moduleKey, typeName, configFile) =>
                        manager.GetModules(domainKey, moduleKey).Single(m => m.GetType().FullName == typeName)
                            .Initialize()
                );
                manager[domainKey].RemoveHook.After.Add((domain, moduleKey, type) =>
                    manager.Log.InfoFormat(Resources.ModuleObjectRemoved, domainKey, type.FullName, moduleKey)
                );
            });
            _host.ModuleManager.UnloadHook.After.Add((self, domain) =>
                self.Log.InfoFormat(Resources.ModuleAssemblyUnloaded, domain)
            );
            host.ModuleManager.Configuration.ResolveChild("init").ForEach(entry =>
            {
                host.ModuleManager.Load(entry.Key);
                entry.Get<IList<Struct<String, String>>>()
                    .ForEach(e => host.ModuleManager[entry.Key].Add(e.Item1, e.Item2));
            });
        }

        private static void RegisterModuleHook(IModule module)
        {
            module.InitializeHook.Before.Add((self) =>
                self.Log.InfoFormat(
                    Resources.ModuleObjectInitializing,
                    self.Name
                )
            );
            module.InitializeHook.After.Add((self) =>
                self.Log.InfoFormat(
                    Resources.ModuleObjectInitialized,
                    self.Name,
                    self.Configuration.ConfigurationFile.Name
                )
            );

            if (module is InputFlowModule)
            {
                var input = module as InputFlowModule;
                input.InputHook.Before.Add((self, selector, storage, args) =>
                    self.Log.InfoFormat(
                        Resources.InputFlowPerforming,
                        self.Name,
                        selector,
                        storage.Name,
                        args.Inspect().Indent(4)
                    )
                );
                input.InputHook.After.Add((self, selector, storage, args) =>
                    self.Log.InfoFormat(Resources.InputFlowPerformed, self.Name)
                );
            }
            else if (module is FilterFlowModule)
            {
                var filter = module as FilterFlowModule;
                filter.FilterHook.Before.Add((self, selector, source, storage, args) =>
                    self.Log.InfoFormat(
                        Resources.FilterFlowPerforming,
                        self.Name,
                        selector,
                        source.Count(),
                        storage.Name,
                        args.Inspect().Indent(4)
                    )
                );
                filter.FilterHook.After.Add((self, selector, source, storage, args) =>
                    self.Log.InfoFormat(Resources.FilterFlowPerformed, self.Name)
                );
            }
            else if (module is OutputFlowModule)
            {
                var output = module as OutputFlowModule;
                output.OutputHook.Before.Add((self, selector, source, storage, args, type) =>
                    self.Log.InfoFormat(
                        Resources.OutputFlowPerforming,
                        self.Name,
                        selector,
                        source.Count(),
                        storage.Name,
                        args.Inspect().Indent(4),
                        type.FullName
                    )
                );
                output.OutputHook.After.Add((self, selector, source, storage, args, type) =>
                    self.Log.InfoFormat(Resources.OutputFlowPerformed, self.Name)
                );
            }
            else if (module is ServantModule)
            {
                var servant = module as ServantModule;
                servant.StartHook.Before.Add(self => self.Log.InfoFormat(Resources.ServantStarting, self.Name));
                servant.StartHook.After.Add(self => self.Log.InfoFormat(Resources.ServantStarted, self.Name));
                servant.StopHook.Before.Add(self => self.Log.InfoFormat(Resources.ServantStopping, self.Name));
                servant.StopHook.After.Add(self => self.Log.InfoFormat(Resources.ServantStopped, self.Name));
                servant.AbortHook.Before.Add(self => self.Log.InfoFormat(Resources.ServantAborting, self.Name));
                servant.AbortHook.After.Add(self => self.Log.InfoFormat(Resources.ServantAborted, self.Name));
            }
            else if (module is StorageModule)
            {
                var storage = module as StorageModule;
            }
        }

        private static void InitializeHooksInObject(Object obj)
        {
            obj.GetType().GetProperties()
                .Where(p => p.PropertyType.BaseType.Do(
                    t => t != null
                        && t.IsGenericType
                        && t.GetGenericTypeDefinition() == typeof(Hook<,,,,>)
                ))
                .Select(p => p.GetValue(obj, null))
                .Select(o => (IList) o.GetType().GetProperty("Failed").GetValue(o, null))
                .ForEach(l => l.Add(l.GetType()
                    .GetGenericArguments()
                    .Single()
                    .Do(d => d.GetGenericArguments()
                        .Select((t, i) => Expression.Parameter(t, "_" + i))
                        .ToArray()
                        .Do(p =>
                            // (self, ..., ex) => ((Object) self is ILoggable
                            //     ? (self as ILoggable).Log.Fatal("Unhandled exception occured.", ex) is Object : false
                            // ).Void();
                            Expression.Lambda(
                                d,
                                Expression.Call(
                                    null,
                                    typeof(ObjectUtil).GetMethod("Void"),
                                    Expression.Convert(
                                        Expression.Condition(
                                            Expression.TypeIs(p.First(), typeof(ILoggable)),
                                            // HACK: "returns_void() is object" hack (See http://d.hatena.ne.jp/NyaRuRu/20080207/p1)
                                            // NOTE: It raises a warning CS0184 in normal C# code
                                            Expression.TypeIs(
                                                Expression.Call(
                                                    Expression.Property(
                                                        Expression.TypeAs(p.First(), typeof(ILoggable)),
                                                        typeof(ILoggable).GetProperty("Log")
                                                    ),
                                                    typeof(ILog).GetMethod("Fatal", Create.TypeArray<Object, Exception>()),
                                                    Expression.Constant("Unhandled exception occured."),
                                                    p.Last()
                                                ),
                                                typeof(Object)
                                            ),
                                            Expression.Constant(false)
                                        ),
                                        typeof(Object)
                                    )
                                ),
                                p
                            )
                            .Compile()
                        )
                    )
                ));
        }
    }
}
