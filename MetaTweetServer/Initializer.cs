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
using System.Collections.Generic;
using System.Linq;
using XSpect.MetaTweet.Modules;
using XSpect.Configuration;
using XSpect.MetaTweet.Properties;
using XSpect.Extension;
using Achiral;
using Achiral.Extension;

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
            RegisterHooks();
            _host.Configuration.ResolveChild("modules").ForEach(entry =>
            {
                host.ModuleManager.Load(entry.Key);
                (entry.UntypedValue as IList<Struct<String, String>>)
                    .ForEach(module => host.ModuleManager.Add(entry.Key, module.Item1, module.Item2));
            });
        }

        private static void RegisterHooks()
        {
            _host.ModuleManager.LoadHook.After.Add((self, domain) =>
                self.Log.InfoFormat(Resources.ModuleLoaded, domain)
            );
            _host.ModuleManager.UnloadHook.After.Add((self, domain) =>
                self.Log.InfoFormat(Resources.ModuleUnloaded, domain)
            );
            _host.ModuleManager.ExecuteHook.Before.Add((self, domain, file) =>
                self.Log.InfoFormat(Resources.CodeExecuting, domain, file)
            );
            _host.ModuleManager.ExecuteHook.After.Add((self, domain, file) =>
                self.Log.InfoFormat(Resources.CodeExecuted, domain, file)
            );
            _host.ModuleManager.AddHook.After.AddRange(
                (self, domain, key, typeName, configFile) =>
                    self.Log.InfoFormat(Resources.ModuleAdded, domain, key, typeName, configFile.Null(f => f.Name)),
                (self, domain, key, typeName, configFile) =>
                    RegisterModuleHook(self.GetModules(domain, key).Single(m => m.GetType().FullName == typeName)),
                (self, domain, key, typeName, configFile) =>
                    self.GetModules(domain, key).Single(m => m.GetType().FullName == typeName).Initialize(
                        configFile.Null(f => XmlConfiguration.Load(f.FullName))
                    )
            );
            _host.ModuleManager.RemoveHook.After.Add((self, domain, type, key) =>
                self.Log.InfoFormat(Resources.ModuleRemoved, domain, type.FullName, key)
            );
        }

        private static void RegisterModuleHook(IModule module)
        {
            module.InitializeHook.Before.Add((self, configuration) =>
                self.Log.InfoFormat(
                    Resources.ModuleInitializing,
                    self.Name
                )
            );
            module.InitializeHook.After.Add((self, configuration) =>
                self.Log.InfoFormat(
                    Resources.ModuleInitialized,
                    self.Name,
                    configuration.ConfigurationFile.Name
                )
            );

            if (module is InputFlowModule)
            {
                InputFlowModule input = module as InputFlowModule;
                input.InputHook.Before.Add((self, selector, storage, args) =>
                    self.Log.InfoFormat(
                        Resources.InputFlowInputStarted,
                        self.Name,
                        selector,
                        storage.Name,
                        args.Inspect().Indent(4)
                    )
                );
                input.InputHook.After.Add((self, selector, storage, args) =>
                    self.Log.InfoFormat(Resources.InputFlowInputFinished, self.Name)
                );
            }
            else if (module is FilterFlowModule)
            {
                FilterFlowModule filter = module as FilterFlowModule;
                filter.FilterHook.Before.Add((self, selector, source, storage, args) =>
                    self.Log.InfoFormat(
                        Resources.FilterFlowFilterStarted,
                        self.Name,
                        selector,
                        source.Count(),
                        storage.Name,
                        args.Inspect().Indent(4)
                    )
                );
                filter.FilterHook.After.Add((self, selector, source, storage, args) =>
                    self.Log.InfoFormat(Resources.FilterFlowFilterStarted, self.Name)
                );
            }
            else if (module is OutputFlowModule)
            {
                OutputFlowModule output = module as OutputFlowModule;
                output.OutputHook.Before.Add((self, selector, source, storage, args, type) =>
                    self.Log.InfoFormat(
                        Resources.OutputFlowOutputStarted,
                        self.Name,
                        selector,
                        source.Count(),
                        storage.Name,
                        args.Inspect().Indent(4),
                        type.FullName
                    )
                );
                output.OutputHook.After.Add((self, selector, source, storage, args, type) =>
                    self.Log.InfoFormat(Resources.OutputFlowOutputFinished, self.Name)
                );
            }
            else if (module is ServantModule)
            {
                ServantModule servant = module as ServantModule;
                servant.StartHook.Before.Add(self => self.Log.InfoFormat(Resources.ServantStarting, self.Name));
                servant.StartHook.After.Add(self => self.Log.InfoFormat(Resources.ServantStarted, self.Name));
                servant.StopHook.Before.Add(self => self.Log.InfoFormat(Resources.ServantStopping, self.Name));
                servant.StopHook.After.Add(self => self.Log.InfoFormat(Resources.ServantStopped, self.Name));
                servant.AbortHook.Before.Add(self => self.Log.InfoFormat(Resources.ServantAborting, self.Name));
                servant.AbortHook.After.Add(self => self.Log.InfoFormat(Resources.ServantAborted, self.Name));
            }
            else if (module is StorageModule)
            {
                StorageModule storage = module as StorageModule;
                storage.LoadAccountsDataTableHook.Before.Add((self, clauses) =>
                    self.Log.DebugFormat(Resources.AccountsLoading, self.Name, clauses)
                );
                storage.LoadActivitiesDataTableHook.Before.Add((self, clauses) =>
                    self.Log.DebugFormat(Resources.ActivitiesLoading, self.Name, clauses)
                );
                storage.LoadFavorMapDataTableHook.Before.Add((self, clauses) =>
                    self.Log.DebugFormat(Resources.FavorMapLoading, self.Name, clauses)
                );
                storage.LoadFollowMapDataTableHook.Before.Add((self, clauses) =>
                    self.Log.DebugFormat(Resources.FollowMapLoading, self.Name, clauses)
                );
                storage.LoadPostsDataTableHook.Before.Add((self, clauses) =>
                    self.Log.DebugFormat(Resources.PostsLoading, self.Name, clauses)
                );
                storage.LoadReplyMapDataTableHook.Before.Add((self, clauses) =>
                    self.Log.DebugFormat(Resources.ReplyMapLoading, self.Name, clauses)
                );
                storage.LoadTagMapDataTableHook.Before.Add((self, clauses) =>
                    self.Log.DebugFormat(Resources.TagMapLoading, self.Name, clauses)
                );

                storage.GetAccountHook.Before.Add((self, row) =>
                    self.Log.DebugFormat(Resources.AccountGetting, self.Name, row)
                );
                storage.GetActivityHook.Before.Add((self, row) =>
                    self.Log.DebugFormat(Resources.ActivityGetting, self.Name, row)
                );
                storage.GetFavorElementHook.Before.Add((self, row) =>
                    self.Log.DebugFormat(Resources.FavorElementGetting, self.Name,row)
                );
                storage.GetFollowElementHook.Before.Add((self, row) =>
                    self.Log.DebugFormat(Resources.FollowElementGetting, self.Name, row)
                );
                storage.GetPostHook.Before.Add((self, row) =>
                    self.Log.DebugFormat(Resources.PostGetting, self.Name, row)
                );
                storage.GetReplyElementHook.Before.Add((self, row) =>
                    self.Log.DebugFormat(Resources.ReplyElementGetting, self.Name, row)
                );
                storage.GetTagElementHook.Before.Add((self, row) =>
                    self.Log.DebugFormat(Resources.TagElementGetting, self.Name, row)
                );

                storage.NewAccountHook.Before.Add((self, accountId, realm) =>
                    self.Log.DebugFormat(Resources.AccountCreating, self.Name, accountId.ToString("d"), realm)
                );
                storage.NewActivityHook.Before.Add((self, account, timestamp, category, subindex) =>
                    self.Log.DebugFormat(Resources.ActivityCreating, self.Name, account, timestamp.ToString("s"), category, subindex)
                );
                storage.NewFavorElementHook.Before.Add((self, account, activity) =>
                    self.Log.DebugFormat(Resources.FavorElementCreating, self.Name, account, activity)
                );
                storage.NewFollowElementHook.Before.Add((self, account, followingAccount) =>
                    self.Log.DebugFormat(Resources.FollowElementCreating, self.Name, account, followingAccount)
                );
                storage.NewPostHook.Before.Add((self, activity) =>
                    self.Log.DebugFormat(Resources.PostCreating, self.Name, activity)
                );
                storage.NewReplyElementHook.Before.Add((self, post, inReplyToPost) =>
                    self.Log.DebugFormat(Resources.ReplyElementCreating, self.Name, post, inReplyToPost)
                );
                storage.NewTagElementHook.Before.Add((self, activity, tag) =>
                    self.Log.DebugFormat(Resources.TagElementCreating, self.Name, activity, tag)
                );
            }
        }
    }
}
