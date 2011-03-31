﻿// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * SystemFlow
 *   MetaTweet Input/Output modules which provides generic system instructions
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of TwitterApiFlow.
 * 
 * This program is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation; either version 3 of the License, or (at your
 * option) any later version.
 * 
 * This program is distributed in the hope that it will be useful, but
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
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using XSpect.Codecs;
using XSpect.Extension;
using XSpect.MetaTweet.Objects;
using XSpect.MetaTweet.Requesting;
using Achiral;
using Achiral.Extension;

namespace XSpect.MetaTweet.Modules
{
    public class SystemInput
        : InputFlowModule
    {
        #region Common

        [FlowInterface("/null")]
        public IEnumerable<StorageObject> NullInput(StorageSession session, String param, IDictionary<String, String> args)
        {
            return Enumerable.Empty<StorageObject>();
        }

        #endregion

        #region StorageObject

        [FlowInterface("/obj/accounts")]
        public IEnumerable<Account> GetAccounts(StorageSession session, String param, IDictionary<String, String> args)
        {
            return session.Query(StorageObjectDynamicQuery.Account(args.GetValueOrDefault("query")));
        }

        [FlowInterface("/obj/activities")]
        public IEnumerable<Activity> GetActivities(StorageSession session, String param, IDictionary<String, String> args)
        {
            return session.Query(StorageObjectDynamicQuery.Activity(args.GetValueOrDefault("query")));
        }

        [FlowInterface("/obj/advertisements")]
        public IEnumerable<Advertisement> GetAdvertisements(StorageSession session, String param, IDictionary<String, String> args)
        {
            return session.Query(StorageObjectDynamicQuery.Advertisement(args.GetValueOrDefault("query")));
        }

        [FlowInterface("/obj/created")]
        public IObservable<StorageObject> SubscribeActivities(StorageSession session, String param, IDictionary<String, String> args)
        {
            return Observable.FromEvent<StorageObjectEventArgs>(session.Parent, "Created")
                .SelectMany(e => e.EventArgs.Objects);
        }

        #endregion

        #region RequestTask

        [FlowInterface("/reqmgr/tasks")]
        public IEnumerable<RequestTask> GetRequestTasks(StorageSession session, String param, IDictionary<String, String> args)
        {
            IQueryable tasks = this.Host.RequestManager
                .OrderByDescending(t => t.Id)
                .AsQueryable();
            if (args.ContainsKey("query"))
            {
                tasks = tasks.Query(args["query"]);
            }
            return tasks.Cast<RequestTask>();
        }

        #endregion

        #region IModule

        [FlowInterface("/modmgr/domains")]
        public IEnumerable<ModuleDomain> GetModuleDomains(StorageSession session, String param, IDictionary<String, String> args)
        {
            IQueryable domains = this.Host.ModuleManager.Domains.AsQueryable();
            if (args.ContainsKey("query"))
            {
                domains = domains.Query(args["query"]);
            }
            return domains.Cast<ModuleDomain>();
        }

        [FlowInterface("/modmgr/objects")]
        [FlowInterface("/modmgr/modules")]
        public IEnumerable<IModule> GetModuleObjects(StorageSession session, String param, IDictionary<String, String> args)
        {
            IQueryable modules = this.Host.ModuleManager.GetModules(
                args.ContainsKey("domain") ? args["domain"] : null,
                args.ContainsKey("key") ? args["key"] : null,
                args.ContainsKey("type")
                    ? args["type"].Let(k =>
                      {
                          switch (k)
                          {
                              case "flow":
                                  return typeof(FlowModule);
                              case "input":
                                  return typeof(InputFlowModule);
                              case "filter":
                                  return typeof(FilterFlowModule);
                              case "output":
                                  return typeof(OutputFlowModule);
                              case "servant":
                                  return typeof(ServantModule);
                              case "storage":
                                  return typeof(StorageModule);
                              default:
                                  return Type.GetType(k);
                          }
                      })
                    : typeof(IModule)
            )
                .OrderBy(m => m.Name)
                .ThenBy(m => m.GetType().FullName)
                .AsQueryable();
            if (args.ContainsKey("query"))
            {
                modules = modules.Query(args["query"]);
            }
            return modules.Cast<IModule>();
        }

        [FlowInterface("/modmgr/load")]
        public Object LoadModuleAssembly(StorageSession session, String param, IDictionary<String, String> args)
        {
            this.Host.ModuleManager.Load(args["domain"]);
            return null;
        }

        [FlowInterface("/modmgr/unload")]
        public Object UnloadModuleAssembly(StorageSession session, String param, IDictionary<String, String> args)
        {
            this.Host.ModuleManager.Unload(args["domain"]);
            return null;
        }

        [FlowInterface("/modmgr/reload")]
        public Object ReloadModuleAssembly(StorageSession session, String param, IDictionary<String, String> args)
        {
            this.Host.ModuleManager.Reload(args["domain"]);
            return null;
        }

        [FlowInterface("/modmgr/add")]
        public Object AddModuleObject(StorageSession session, String param, IDictionary<String, String> args)
        {
            this.Host.ModuleManager.Domains[args["domain"]].Add(args["key"], args["type"], new List<String>());
            return null;
        }

        [FlowInterface("/modmgr/remove")]
        public Object RemoveModuleObject(StorageSession session, String param, IDictionary<String, String> args)
        {
            this.Host.ModuleManager.Domains[args["domain"]].Remove(args["key"], Type.GetType(args["type"]));
            return null;
        }

        [FlowInterface("/modmgr/start-servant")]
        public Object StartServant(StorageSession session, String param, IDictionary<String, String> args)
        {
            this.Host.ModuleManager.GetModule<ServantModule>(args["key"]).Start();
            return null;
        }

        [FlowInterface("/modmgr/stop-servant")]
        public Object StopServant(StorageSession session, String param, IDictionary<String, String> args)
        {
            this.Host.ModuleManager.GetModule<ServantModule>(args["key"]).Stop();
            return null;
        }

        [FlowInterface("/modmgr/abort-servant")]
        public Object AbortServant(StorageSession session, String param, IDictionary<String, String> args)
        {
            this.Host.ModuleManager.GetModule<ServantModule>(args["key"]).Abort();
            return null;
        }

        [FlowInterface("/modmgr/restart-servant")]
        public Object RestartServant(StorageSession session, String param, IDictionary<String, String> args)
        {
            this.Host.ModuleManager.GetModule<ServantModule>(args["key"]).Stop();
            this.Host.ModuleManager.GetModule<ServantModule>(args["key"]).Start();
            return null;
        }

        #endregion

        #region FlowInterfaceInfo

        [FlowInterface("/modmgr/flow-interfaces")]
        public IEnumerable<FlowInterfaceInfo> GetSelfFlowInterfaces(StorageSession session, String param, IDictionary<String, String> args)
        {
            IQueryable interfaces = this.Host.ModuleManager.GetModules(
                args.ContainsKey("domain") ? args["domain"] : null,
                args.ContainsKey("key") ? args["key"] : null,
                args.ContainsKey("type")
                    ? args["type"].Let(k =>
                      {
                          switch (k)
                          {
                              case "input":
                                  return typeof(InputFlowModule);
                              case "filter":
                                  return typeof(FilterFlowModule);
                              case "output":
                                  return typeof(OutputFlowModule);
                              default:
                                  return Type.GetType(k);
                          }
                      })
                    : typeof(FlowModule)
            )
                .OfType<FlowModule>()
                .Single()
                .GetFlowInterfaces()
                .Select(p => p.Key)
                .OrderBy(i => i.Id)
                .ThenBy(i => i.InputType != null ? i.InputType.FullName : String.Empty)
                .ThenBy(i => i.OutputType.FullName)
                .AsQueryable();
            if (args.ContainsKey("query"))
            {
                interfaces = interfaces.Query(args["query"]);
            }
            return interfaces.Cast<FlowInterfaceInfo>();
        }

        #endregion

        #region StoredRequest

        [FlowInterface("/storedmgr/stored-requests")]
        public IEnumerable<StoredRequest> GetStoredRequests(StorageSession session, String param, IDictionary<String, String> args)
        {
            IQueryable storedRequests = this.Host.StoredRequestManager.StoredRequests.Values
                .OrderBy(s => s.Name)
                .AsQueryable();
            if (args.ContainsKey("query"))
            {
                storedRequests = storedRequests.Query(args["query"]);
            }
            return storedRequests.Cast<StoredRequest>();
        }

        [FlowInterface("/storedmgr/apply/")]
        public Object ApplyStoredRequest(StorageSession session, String param, IDictionary<String, String> args)
        {
            return this.Host.StoredRequestManager.Execute(param, args);
        }

        #endregion
    }
}