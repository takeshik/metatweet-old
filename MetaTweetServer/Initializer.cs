// -*- mode: csharp; encoding: utf-8; -*-
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
using System.Text;
using XSpect.MetaTweet.Modules;
using XSpect.Configuration;
using XSpect.MetaTweet.Properties;
using System.Reflection;
using XSpect.Extension;

namespace XSpect.MetaTweet
{
    public class Initializer
        : Object
    {
        private static ServerCore _host;

        public static void Initialize(ServerCore host, IDictionary<String, String> args)
        {
            _host = host;
            RegisterHooks();

            host.ModuleManager.Load("SQLiteStorage");
            host.ModuleManager.Add("SQLiteStorage", "main", "XSpect.MetaTweet.SQLiteStorage");

            host.ModuleManager.Load("TwitterApiFlow");
            host.ModuleManager.Add("TwitterApiFlow", "twitter", "XSpect.MetaTweet.TwitterApiInput");
            host.ModuleManager.Add("TwitterApiFlow", "twitter", "XSpect.MetaTweet.TwitterApiOutput");

            host.ModuleManager.Load("RemotingServant");
            host.ModuleManager.Add("RemotingServant", "remoting", "XSpect.MetaTweet.RemotingTcpServant");

            host.ModuleManager.Load("LocalServant");
            host.ModuleManager.Add("LocalServant", "local", "XSpect.MetaTweet.LocalServant");
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
                    self.Log.InfoFormat(Resources.ModuleAdded, domain, key, typeName, configFile.Name),
                (self, domain, key, typeName, configFile) =>
                    RegisterModuleHook(self.GetModules(domain, key).Single(m => m.GetType().FullName == typeName))
            );
            _host.ModuleManager.RemoveHook.After.Add((self, domain, type, key) =>
                self.Log.InfoFormat(Resources.ModuleRemoved, domain, type.FullName, key)
            );
        }

        private static void RegisterModuleHook(IModule module)
        {
            if (module is InputFlowModule)
            {
                InputFlowModule input = module as InputFlowModule;
                input.InputHook.Before.Add((self, selector, storage, args) =>
                    self.Log.InfoFormat(
                        Resources.InputFlowInputStarted,
                        self.Name,
                        selector,
                        storage is StorageModule
                            ? (storage as StorageModule).Name
                            : String.Format("({0})", storage.GetType().FullName),
                        args.Inspect().ForEachLine(l => new String(' ', 4) + l)
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
                        storage is StorageModule
                            ? (storage as StorageModule).Name
                            : String.Format("({0})", storage.GetType().FullName),
                        args.Inspect().ForEachLine(l => new String(' ', 4) + l)
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
                        storage is StorageModule
                            ? (storage as StorageModule).Name
                            : String.Format("({0})", storage.GetType().FullName),
                        args.Inspect().ForEachLine(l => new String(' ', 4) + l),
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
                storage.LoadAccountsDataTableHook.Before.Add((self, accountId, realm) =>
                    self.Log.DebugFormat(Resources.AccountsLoading, self.Name, accountId, realm)
                );
                storage.LoadAccountsDataTableHook.After.Add((self, accountId, realm) =>
                    self.Log.DebugFormat(Resources.AccountsLoaded, self.Name, accountId, realm)
                );
                storage.LoadActivitiesDataTableHook.Before.Add((self, accountId, timestamp, category, subindex, value, data) =>
                    self.Log.DebugFormat(Resources.ActivitiesLoading, self.Name, accountId, timestamp, category, subindex, value, data)
                );
                storage.LoadActivitiesDataTableHook.After.Add((self, accountId, timestamp, category, subindex, value, data) =>
                    self.Log.DebugFormat(Resources.ActivitiesLoaded, self.Name, accountId, timestamp, category, subindex, value, data)
                );
                storage.LoadFavorMapDataTableHook.Before.Add((self, accountId, favoringAccountId, favoringTimestamp, favoringCategory, favoringSubindex) =>
                    self.Log.DebugFormat(Resources.FavorMapLoading, self.Name, accountId, favoringAccountId, favoringTimestamp, favoringCategory, favoringSubindex)
                );
                storage.LoadFavorMapDataTableHook.After.Add((self, accountId, favoringAccountId, favoringTimestamp, favoringCategory, favoringSubindex) =>
                    self.Log.DebugFormat(Resources.FavorMapLoaded, self.Name, accountId, favoringAccountId, favoringTimestamp, favoringCategory, favoringSubindex)
                );
                storage.LoadFollowMapDataTableHook.Before.Add((self, accountId, followingAccountId) =>
                    self.Log.DebugFormat(Resources.FollowMapLoading, self.Name, accountId, followingAccountId)
                );
                storage.LoadFollowMapDataTableHook.After.Add((self, accountId, followingAccountId) =>
                    self.Log.DebugFormat(Resources.FollowMapLoaded, self.Name, accountId, followingAccountId)
                );
                storage.LoadPostsDataTableHook.Before.Add((self, accountId, postId, text, source) =>
                    self.Log.DebugFormat(Resources.PostsLoading, self.Name, accountId, postId, text, source)
                );
                storage.LoadPostsDataTableHook.After.Add((self, accountId, postId, text, source) =>
                    self.Log.DebugFormat(Resources.PostsLoaded, self.Name, accountId, postId, text, source)
                );
                storage.LoadReplyMapDataTableHook.Before.Add((self, accountId, postId, inReplyToAccountId, inReplyToPostId) =>
                    self.Log.DebugFormat(Resources.ReplyMapLoading, self.Name, accountId, postId, inReplyToAccountId, inReplyToPostId)
                );
                storage.LoadReplyMapDataTableHook.After.Add((self, accountId, postId, inReplyToAccountId, inReplyToPostId) =>
                    self.Log.DebugFormat(Resources.ReplyMapLoaded, self.Name, accountId, postId, inReplyToAccountId, inReplyToPostId)
                );
                storage.LoadTagMapDataTableHook.Before.Add((self, accountId, timestamp, category, subindex, tag) =>
                    self.Log.DebugFormat(Resources.TagMapLoading, self.Name, accountId, timestamp, category, subindex, tag)
                );
                storage.LoadTagMapDataTableHook.After.Add((self, accountId, timestamp, category, subindex, tag) =>
                    self.Log.DebugFormat(Resources.TagMapLoaded, self.Name, accountId, timestamp, category, subindex, tag)
                );

                storage.GetAccountHook.Before.Add((self, row) =>
                    self.Log.DebugFormat(Resources.AccountGetting, self.Name, row)
                );
                storage.GetAccountHook.After.Add((self, row) =>
                    self.Log.DebugFormat(Resources.AccountGot, self.Name, row)
                );
                storage.GetActivityHook.Before.Add((self, row) =>
                    self.Log.DebugFormat(Resources.ActivityGetting, self.Name, row)
                );
                storage.GetActivityHook.After.Add((self, row) =>
                    self.Log.DebugFormat(Resources.ActivityGot, self.Name, row)
                );
                storage.GetFavorElementHook.Before.Add((self, row) =>
                    self.Log.DebugFormat(Resources.FavorElementGetting, self.Name, row)
                );
                storage.GetFavorElementHook.After.Add((self, row) =>
                    self.Log.DebugFormat(Resources.FavorElementGot, self.Name, row)
                );
                storage.GetFollowElementHook.Before.Add((self, row) =>
                    self.Log.DebugFormat(Resources.FollowElementGetting, self.Name, row)
                );
                storage.GetFollowElementHook.After.Add((self, row) =>
                    self.Log.DebugFormat(Resources.FollowElementGot, self.Name, row)
                );
                storage.GetPostHook.Before.Add((self, row) =>
                    self.Log.DebugFormat(Resources.PostGetting, self.Name, row)
                );
                storage.GetPostHook.After.Add((self, row) =>
                    self.Log.DebugFormat(Resources.PostGot, self.Name, row)
                );
                storage.GetReplyElementHook.Before.Add((self, row) =>
                    self.Log.DebugFormat(Resources.ReplyElementGetting, self.Name, row)
                );
                storage.GetReplyElementHook.After.Add((self, row) =>
                    self.Log.DebugFormat(Resources.ReplyElementGot, self.Name, row)
                );
                storage.GetTagElementHook.Before.Add((self, row) =>
                    self.Log.DebugFormat(Resources.TagElementGetting, self.Name, row)
                );
                storage.GetTagElementHook.After.Add((self, row) =>
                    self.Log.DebugFormat(Resources.TagElementGot, self.Name, row)
                );

                storage.NewAccountHook.Before.Add((self, accountId, realm) =>
                    self.Log.DebugFormat(Resources.AccountCreating, self.Name, accountId, realm)
                );
                storage.NewAccountHook.After.Add((self, accountId, realm) =>
                    self.Log.DebugFormat(Resources.AccountCreated, self.Name, accountId, realm)
                );
                storage.NewActivityHook.Before.Add((self, accountId, timestamp, category, subindex) =>
                    self.Log.DebugFormat(Resources.ActivityCreating, self.Name, accountId, timestamp, category, subindex)
                );
                storage.NewActivityHook.After.Add((self, accountId, timestamp, category, subindex) =>
                    self.Log.DebugFormat(Resources.ActivityCreated, self.Name, accountId, timestamp, category, subindex)
                );
                storage.NewFavorElementHook.Before.Add((self, account, activity) =>
                    self.Log.DebugFormat(Resources.FavorElementCreating, self.Name, account, activity)
                );
                storage.NewFavorElementHook.After.Add((self, account, activity) =>
                    self.Log.DebugFormat(Resources.FavorElementCreated, self.Name, account, activity)
                );
                storage.NewFollowElementHook.Before.Add((self, account, followingAccount) =>
                    self.Log.DebugFormat(Resources.FollowElementCreating, self.Name, account, followingAccount)
                );
                storage.NewFollowElementHook.After.Add((self, account, followingAccount) =>
                    self.Log.DebugFormat(Resources.FollowElementCreated, self.Name, account, followingAccount)
                );
                storage.NewPostHook.Before.Add((self, activity) =>
                    self.Log.DebugFormat(Resources.PostCreating, self.Name, activity)
                );
                storage.NewPostHook.After.Add((self, activity) =>
                    self.Log.DebugFormat(Resources.PostCreated, self.Name, activity)
                );
                storage.NewReplyElementHook.Before.Add((self, post, inReplyToPost) =>
                    self.Log.DebugFormat(Resources.ReplyElementCreating, post, self.Name, inReplyToPost)
                );
                storage.NewReplyElementHook.After.Add((self, post, inReplyToPost) =>
                    self.Log.DebugFormat(Resources.ReplyElementCreated, self.Name, post, inReplyToPost)
                );
                storage.NewTagElementHook.Before.Add((self, activity, tag) =>
                    self.Log.DebugFormat(Resources.TagElementCreating, self.Name, activity, tag)
                );
                storage.NewTagElementHook.After.Add((self, activity, tag) =>
                    self.Log.DebugFormat(Resources.TagElementCreated, self.Name, activity, tag)
                );
            }
        }
    }
}
