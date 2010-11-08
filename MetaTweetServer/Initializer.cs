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
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using XSpect.Hooking;
using XSpect.MetaTweet.Modules;
using XSpect.Configuration;
using XSpect.MetaTweet.Objects;
using XSpect.MetaTweet.Properties;
using XSpect.Extension;
using Achiral;
using Achiral.Extension;
using XSpect.MetaTweet.Requesting;

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
            InitializeHooksInObject(_host);
            InitializeHooksInObject(_host.ModuleManager);
            _host.ModuleManager.LoadHook.Succeeded.Add((manager, domainKey, ret) =>
            {
                ModuleDomain dom = (ModuleDomain) ret;
                InitializeHooksInObject(dom);
                manager.Log.Info(Resources.ModuleAssemblyLoaded, domainKey);
                dom.AddHook.Succeeded.AddRange(
                    (domain, moduleKey, typeName, options, configFile, module) =>
                        manager.Log.Info(
                            Resources.ModuleObjectAdded, domainKey, moduleKey, typeName
                        ),
                    (domain, moduleKey, typeName, options, configFile, module) =>
                        RegisterModuleHook(module),
                    (domain, moduleKey, typeName, options, configFile, module) =>
                        module.Configure(XmlConfiguration.Load(configFile)),
                    (domain, moduleKey, typeName, options, configFile, module) =>
                        module.Initialize()
                );
                dom.RemoveHook.Succeeded.Add((domain, moduleKey, type) =>
                    manager.Log.Info(Resources.ModuleObjectRemoved, domainKey, type.FullName, moduleKey)
                );
            });
            _host.ModuleManager.UnloadHook.Succeeded.Add((self, domain) =>
                self.Log.Info(Resources.ModuleAssemblyUnloaded, domain)
            );
            _host.RequestManager.RegisterHook.Succeeded.AddRange(
                (manager, req, task) => task.ProcessHook.Before.Add((self, type) =>
                    self.Log.Info(String.Format(
                        Resources.ServerRequestExecuting,
                        req
                    ))
                ),
                (manager, req, task) => task.ProcessHook.Succeeded.AddRange(
                    (self, type, ret) =>
                        self.Log.Info(String.Format(
                            Resources.ServerRequestExecuted,
                            req,
                            self.ElapsedTime
                        )),
                    (self, type, ret) =>
                        (self.State == RequestTaskState.Succeeded
                            ? self.AccessLog.Info
                            : self.State == RequestTaskState.Failed
                                  ? self.AccessLog.Error
                                  : self.State == RequestTaskState.Canceled
                                        ? self.AccessLog.Warn
                                        : Lambda.Nop<String, Object[]>()
                        )(
                            "{0} +{1} #{2}:{3} {4} {5}",
                            Make.Array<Object>(
                                self.ExitTime.Value.ToString("yyyy/MM/dd hh:mm:ss.fff"),
                                self.ElapsedTime.ToString(@"hh\:mm\:ss\.fff"),
                                self.Id,
                                self.State.ToString().Substring(0, 1),
                                "-", // user@host (not supported)
                                self.Request
                            )
                        )
                ),
                (manager, req, task) => task.ProcessHook.Failed.Add((self, type, ex) =>
                    self.Log.Fatal(String.Format(
                        Resources.ServerRequestExecuted,
                        req,
                        self.ElapsedTime
                    ), ex)
                )
            );
            InitializeHooksInObject(_host.RequestManager);

            host.ModuleManager.Configuration.ResolveChild("init").Values.ForEach(entry =>
            {
                host.ModuleManager.Load(entry.Key);
                entry.Get<IList<ModuleObjectSetup>>()
                    .ForEach(e => host.ModuleManager[entry.Key].Add(e));
            });
        }

        private static void RegisterModuleHook(IModule module)
        {
            InitializeHooksInObject(module);
            module.ConfigureHook.After.Add((self, conf) =>
                self.Log.Info(
                    Resources.ModuleObjectConfigured,
                    self.Name,
                    conf.ConfigurationFile.Name
                )
            );
            module.InitializeHook.Before.Add(self =>
                self.Log.Info(
                    Resources.ModuleObjectInitializing,
                    self.Name
                )
            );
            module.InitializeHook.Succeeded.Add(self =>
                self.Log.Info(
                    Resources.ModuleObjectInitialized,
                    self.Name,
                    self.Configuration.ConfigurationFile.Name
                )
            );
            module.DisposeHook.Before.Add(self =>
                self.Log.Info(
                    Resources.ModuleObjectDisposing,
                    self.Name
                )
            );
            module.DisposeHook.Succeeded.Add(self =>
                self.Log.Info(
                    Resources.ModuleObjectDisposed,
                    self.Name
                )
            );

            if (module is InputFlowModule)
            {
                var input = (InputFlowModule) module;
                input.InputHook.Before.Add((self, selector, storage, args) =>
                    self.Log.Debug(
                        Resources.InputFlowPerforming,
                        self.Name,
                        selector,
                        storage.Name,
                        args.Inspect().Indent(4)
                    )
                );
                input.InputHook.Succeeded.AddRange((self, selector, storage, args, ret) =>
                    self.Log.Debug(Resources.InputFlowPerformed, self.Name, ret.Item1 is IEnumerable
                        ? ((IEnumerable) ret.Item1).Cast<Object>().Count()
                              .If(i => i > 1, i => i + " objects", i => i +  " object")
                        : ret.Item1
                    )
                );
            }
            else if (module is FilterFlowModule)
            {
                var filter = (FilterFlowModule) module;
                filter.FilterHook.Before.Add((self, selector, input, storage, args) =>
                    self.Log.Debug(
                        Resources.FilterFlowPerforming,
                        self.Name,
                        selector,
                        input is IEnumerable
                            ? ((IEnumerable) input).Cast<Object>().Count()
                                  .If(i => i > 1, i => i + " objects", i => i + " object")
                            : input,
                        storage.Name,
                        args.Inspect().Indent(4)
                    )
                );
                filter.FilterHook.Succeeded.Add((self, selector, input, storage, args, ret) =>
                    self.Log.Debug(Resources.FilterFlowPerformed, self.Name, ret.Item1 is IEnumerable
                        ? ((IEnumerable) ret.Item1).Cast<Object>().Count()
                              .If(i => i > 1, i => i + " objects", i => i + " object")
                        : ret.Item1
                    )
                );
            }
            else if (module is OutputFlowModule)
            {
                var output = (OutputFlowModule) module;
                output.OutputHook.Before.Add((self, selector, input, storage, args, type) =>
                    self.Log.Debug(
                        Resources.OutputFlowPerforming,
                        self.Name,
                        selector,
                        input is IEnumerable
                            ? ((IEnumerable) input).Cast<Object>().Count()
                                  .If(i => i > 1, i => i + " objects", i => i + " object")
                            : input,
                        storage.Name,
                        args.Inspect().Indent(4),
                        type != null ? type.FullName : "(any)"
                    )
                );
                output.OutputHook.Succeeded.Add((self, selector, input, storage, args, type, ret) =>
                    self.Log.Debug(Resources.OutputFlowPerformed, self.Name)
                );
            }
            else if (module is ServantModule)
            {
                var servant = (ServantModule) module;
                servant.StartHook.Before.Add(self => self.Log.Info(Resources.ServantStarting, self.Name));
                servant.StartHook.Succeeded.Add(self => self.Log.Info(Resources.ServantStarted, self.Name));
                servant.StopHook.Before.Add(self => self.Log.Info(Resources.ServantStopping, self.Name));
                servant.StopHook.Succeeded.Add(self => self.Log.Info(Resources.ServantStopped, self.Name));
                servant.AbortHook.Before.Add(self => self.Log.Info(Resources.ServantAborting, self.Name));
                servant.AbortHook.Succeeded.Add(self => self.Log.Info(Resources.ServantAborted, self.Name));
            }
            else if (module is StorageModule)
            {
                var storage = (StorageModule) module;
                storage.GetAccountsHook.Succeeded.Add((self, query, ret) =>
                    self.Log.Verbose(
                        Resources.StorageGotAccounts,
                        self.Name,
                        query.ToString().Indent(2),
                        ret.Count().If(i => i > 1, i => i + " objects", i => i + " object")
                    )
                );
                storage.GetActivitiesHook.Succeeded.Add((self, query, ret) =>
                    self.Log.Verbose(
                        Resources.StorageGotActivities,
                        self.Name,
                        query.ToString().Indent(2),
                        ret.Count().If(i => i > 1, i => i + " objects", i => i + " object")
                    )
                );
                storage.GetAnnotationsHook.Succeeded.Add((self, query, ret) =>
                    self.Log.Verbose(
                        Resources.StorageGotAnnotations,
                        self.Name,
                        query.ToString().Indent(2),
                        ret.Count().If(i => i > 1, i => i + " objects", i => i + " object")
                    )
                );
                storage.GetRelationsHook.Succeeded.Add((self, query, ret) =>
                    self.Log.Verbose(
                        Resources.StorageGotRelations,
                        self.Name,
                        query.ToString().Indent(2),
                        ret.Count().If(i => i > 1, i => i + " objects", i => i + " object")
                    )
                );
                storage.GetMarksHook.Succeeded.Add((self, query, ret) =>
                    self.Log.Verbose(
                        Resources.StorageGotMarks,
                        self.Name,
                        query.ToString().Indent(2),
                        ret.Count().If(i => i > 1, i => i + " objects", i => i + " object")
                    )
                );
                storage.GetReferencesHook.Succeeded.Add((self, query, ret) =>
                    self.Log.Verbose(
                        Resources.StorageGotReferences,
                        self.Name,
                        query.ToString().Indent(2),
                        ret.Count().If(i => i > 1, i => i + " objects", i => i + " object")
                    )
                );
                storage.GetTagsHook.Succeeded.Add((self, query, ret) =>
                    self.Log.Verbose(
                        Resources.StorageGotTags,
                        self.Name,
                        query.ToString().Indent(2),
                        ret.Count().If(i => i > 1, i => i + " objects", i => i + " object")
                    )
                );
                storage.NewAccountHook.Succeeded.Add((self, accountId, realm, seeds, ret) =>
                    self.Log.Verbose(
                        ret.Item2 ? Resources.StorageAddedAccount : Resources.StorageAddedExistingAccount,
                        self.Name,
                        ret.Item1.ToString()
                    )
                );
                storage.NewActivityHook.Succeeded.Add((self, account, timestamp, category, subid, userAgent, value, data, ret) =>
                    self.Log.Verbose(
                        ret.Item2 ? Resources.StorageAddedActivity : Resources.StorageAddedExistingActivity,
                        self.Name,
                        ret.Item1.ToString()
                    )
                );
                storage.NewAnnotationHook.Succeeded.Add((self, account, name, value, ret) =>
                    self.Log.Verbose(
                        ret.Item2 ? Resources.StorageAddedAnnotation : Resources.StorageAddedExistingAnnotation,
                        self.Name,
                        ret.Item1.ToString()
                    )
                );
                storage.NewRelationHook.Succeeded.Add((self, account, name, relatingAccount, ret) =>
                    self.Log.Verbose(
                        ret.Item2 ? Resources.StorageAddedRelation : Resources.StorageAddedExistingRelation,
                        self.Name,
                        ret.Item1.ToString()
                    )
                );
                storage.NewMarkHook.Succeeded.Add((self, account, name, markingActivity, ret) =>
                    self.Log.Verbose(
                        ret.Item2 ? Resources.StorageAddedMark : Resources.StorageAddedExistingMark,
                        self.Name,
                        ret.Item1.ToString()
                    )
                );
                storage.NewReferenceHook.Succeeded.Add((self, activity, name, referringActivity, ret) =>
                    self.Log.Verbose(
                        ret.Item2 ? Resources.StorageAddedReference : Resources.StorageAddedExistingReference,
                        self.Name,
                        ret.Item1.ToString()
                    )
                );
                storage.NewTagHook.Succeeded.Add((self, activity, name, value, ret) =>
                    self.Log.Verbose(
                        ret.Item2 ? Resources.StorageAddedTag : Resources.StorageAddedExistingTag,
                        self.Name,
                        ret.Item1.ToString()
                    )
                );
                storage.UpdateHook.Succeeded.Add((self, ret) =>
                    self.Log.Debug(
                        Resources.StorageUpdated,
                        self.Name,
                        ret.If(i => i > 1, i => i + " objects", i => i + " object")
                    )
                );
                storage.UpdateHook.Failed.Clear();
                storage.UpdateHook.Failed.Add((self, ex) =>
                {
                    if (!new StackTrace().GetFrames().Any(f => f.GetMethod() ==
                        typeof(ObjectContextStorage).GetMethod(
                            "TryUpdate",
                            Create.TypeArray<Nullable<Int32>, Nullable<TimeSpan>, Boolean>()
                        )
                    ))
                    {
                        self.Log.Fatal("Unhandled exception occured.", ex);
                    }
                });
            }
        }

        private static void InitializeHooksInObject(Object obj)
        {
            obj.GetType().GetProperties()
                .Where(p => p.PropertyType.BaseType.Let(
                    t => t != null
                        && t.IsGenericType
                        && t.GetGenericTypeDefinition() == typeof(Hook<,,,,>)
                ))
                .Select(p => p.GetValue(obj, null))
                .Select(o => (IList) o.GetType().GetProperty("Failed").GetValue(o, null))
                .ForEach(l => l.Add(l.GetType()
                    .GetGenericArguments()
                    .Single()
                    .Let(d => d.GetGenericArguments()
                        .Select((t, i) => Expression.Parameter(t, "_" + i))
                        .ToArray()
                        .Let(p =>
                            // (self, ..., ex) => ((ILoggable) self).Log.Fatal("Unhandled exception occured.", ex);
                            Expression.Lambda(
                                d,
                                Expression.Call(
                                    Expression.Property(
                                        Expression.Convert(p.First(), typeof(ILoggable)),
                                        typeof(ILoggable).GetProperty("Log")
                                    ),
                                    typeof(Log).GetMethod("Fatal", Create.TypeArray<String, Exception>()),
                                    Expression.Constant("Unhandled exception occured."),
                                    p.Last()
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
