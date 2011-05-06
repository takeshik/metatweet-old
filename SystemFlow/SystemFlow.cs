// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Xml.Linq;
using Achiral;
using Achiral.Extension;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XSpect.Codecs;
using XSpect.Extension;
using XSpect.MetaTweet.Objects;
using XSpect.MetaTweet.Requesting;

namespace XSpect.MetaTweet.Modules
{
    public class SystemFlow
        : FlowModule
    {
        #region Input

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
        public IObservable<StorageObject> SubscribeObjects(StorageSession session, String param, IDictionary<String, String> args)
        {
            return Observable.FromEvent<StorageObjectEventArgs>(session.Parent, "Created")
                .SelectMany(e => e.EventArgs.Objects)
                .AsQbservable()
                .If(_ => args.ContainsKey("filter"), _ => _.Where(TriDQL.ParseLambda<StorageObject, Boolean>(args["filter"]).Compile()));
        }

        [FlowInterface("/obj/accounts-created")]
        public IObservable<Account> SubscribeAccounts(StorageSession session, String param, IDictionary<String, String> args)
        {
            return Observable.FromEvent<StorageObjectEventArgs>(session.Parent, "Created")
                .SelectMany(e => e.EventArgs.Objects)
                .OfType<Account>()
                .AsQbservable()
                .If(_ => args.ContainsKey("filter"), _ => _.Where(TriDQL.ParseLambda<Account, Boolean>(args["filter"]).Compile()));
        }

        [FlowInterface("/obj/activities-created")]
        public IObservable<Activity> SubscribeActivity(StorageSession session, String param, IDictionary<String, String> args)
        {
            return Observable.FromEvent<StorageObjectEventArgs>(session.Parent, "Created")
                .SelectMany(e => e.EventArgs.Objects)
                .OfType<Activity>()
                .If(_ => args.ContainsKey("filter"), _ => _.Where(TriDQL.ParseLambda<Activity, Boolean>(args["filter"]).Compile()));
        }

        [FlowInterface("/obj/advertisements-created")]
        public IObservable<Advertisement> SubscribeAdvertisements(StorageSession session, String param, IDictionary<String, String> args)
        {
            return Observable.FromEvent<StorageObjectEventArgs>(session.Parent, "Created")
                .SelectMany(e => e.EventArgs.Objects)
                .OfType<Advertisement>()
                .AsQbservable()
                .If(_ => args.ContainsKey("filter"), _ => _.Where(TriDQL.ParseLambda<Advertisement, Boolean>(args["filter"]).Compile()));
        }

        #endregion

        #region RequestTask

        [FlowInterface("/reqmgr/tasks")]
        public IEnumerable<IRequestTask> GetRequestTasks(StorageSession session, String param, IDictionary<String, String> args)
        {
            IQueryable tasks = this.Host.RequestManager
                .OrderByDescending(t => t.Id)
                .AsQueryable();
            if (args.ContainsKey("query"))
            {
                tasks = tasks.Query(args["query"]);
            }
            return tasks.Cast<IRequestTask>();
        }

        [FlowInterface("/reqmgr/cancel")]
        public IEnumerable<IRequestTask> CancelRequestTask(StorageSession session, String param, IDictionary<String, String> args)
        {
            this.Host.RequestManager[Int32.Parse(args["id"])].Cancel();
            return null;
        }

        [FlowInterface("/reqmgr/kill")]
        public IEnumerable<IRequestTask> KillRequestTask(StorageSession session, String param, IDictionary<String, String> args)
        {
            this.Host.RequestManager[Int32.Parse(args["id"])].Cancel();
            return null;
        }

        #endregion

        #region IModule

        [FlowInterface("/modmgr/domains")]
        public IEnumerable<IModuleDomain> GetModuleDomains(StorageSession session, String param, IDictionary<String, String> args)
        {
            IQueryable domains = this.Host.ModuleManager.Domains.AsQueryable();
            if (args.ContainsKey("query"))
            {
                domains = domains.Query(args["query"]);
            }
            return domains.Cast<IModuleDomain>();
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
                    ? Type.GetType(args["type"])
                    : typeof(FlowModule)
            )
                .OfType<FlowModule>()
                .Single()
                .GetFlowInterfaces()
                .Select(p => p.Key)
                .OrderBy(i => i.Id)
                .ThenBy(i => i.InputType != null ? i.InputType.FullName : "")
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

        #endregion

        #region Filter

        [FlowInterface("/resolve")]
        public IEnumerable<StorageObject> ResolveReference(IEnumerable<Activity> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return input.Select(a =>
            {
                switch (a.GetValue<String>().Length)
                {
                    case AccountId.HexStringLength:
                        return (StorageObject) a.GetValue<Account>();
                    case ActivityId.HexStringLength:
                        return a.GetValue<Activity>();
                    default: // AdvertisementId.HexStringLength:
                        return a.GetValue<Advertisement>();
                }
            }).ToArray();
        }

        [FlowInterface("/download")]
        public IEnumerable<StorageObject> Download(IEnumerable<Activity> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            IDisposable _ = session.SuppressDispose();
            return input
                .Do(a => a.Act("Body", ((HttpWebResponse) WebRequest.Create(a.GetValue<String>()).GetResponse()).If(
                    r => (Int32) r.StatusCode < 300,
                    r => new Byte[r.ContentLength].Apply(b => r.GetResponseStream().Dispose(s => s.Read(b, 0, b.Length))),
                    r => new Byte[0]
                )))
                .Finally(_.Dispose);
        }

        [FlowInterface("/download")]
        public IObservable<StorageObject> Download(IObservable<Activity> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            IDisposable _ = session.SuppressDispose();
            return input
                .Do(a => a.Act("Body", ((HttpWebResponse) WebRequest.Create(a.GetValue<String>()).GetResponse()).If(
                    r => (Int32) r.StatusCode < 300,
                    r => new Byte[r.ContentLength].Apply(b => r.GetResponseStream().Dispose(s => s.Read(b, 0, b.Length))),
                    r => new Byte[0]
                )))
                .Finally(_.Dispose);
        }

        #endregion

        #region Output

        #region Common

        [FlowInterface("/.null")]
        public Object OutputNull(IEnumerable<StorageObject> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return null;
        }

        [FlowInterface("/.null")]
        public String OutputNullString(IEnumerable<StorageObject> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return "";
        }

        [FlowInterface("/.id")]
        public Object OutputAsIs(Object input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return args.ContainsKey("code")
                ? TriDQL.ParseLambda(input.GetType(), typeof(Object), args["code"])
                      .Compile()
                      .DynamicInvoke(input)
                : input;
        }

        #endregion

        #region StorageObject

        [FlowInterface("/.obj")]
        public IEnumerable<StorageObject> OutputStorageObjects(IEnumerable<StorageObject> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return input.OrderByDescending(o => o).Remotable();
        }

        [FlowInterface("/.bin")]
        public Byte[] OutputBinaryData(IEnumerable<StorageObject> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return input.OfType<Activity>().SingleOrDefault(a => a.GetValue() is IEnumerable<Byte>)
                .GetValue<Byte[]>();
        }

        [FlowInterface("/.custom-text")]
        public String OutputStorageObjectsAsCustomText(IEnumerable<StorageObject> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return String.Concat(input
                .OrderByDescending(o => o)
                .ToArray()
                .Select(TriDQL.ParseLambda<StorageObject, String>(args["code"]).Compile())
            );
        }

        [FlowInterface("/.xml")]
        public String OutputStorageObjectsAsXml(IEnumerable<StorageObject> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            DataContractSerializer serializer = new DataContractSerializer(
                typeof(IEnumerable<StorageObject>),
                null,
                Int32.MaxValue,
                true,
                false,
                new StorageObjectIdConverter()
            );
            return input
                .OrderByDescending(o => o)
                .ToArray()
                .XmlObjectSerializeToString(serializer);
        }

        [FlowInterface("/.json")]
        public String OutputStorageObjectsAsJson(IEnumerable<StorageObject> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return new StringWriter().Apply(_ => _.Dispose(w => JsonSerializer.Create(new JsonSerializerSettings()
            {
                Converters = new JsonConverter[] { new StorageObjectIdConverter(), },
                DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                TypeNameHandling = TypeNameHandling.None,
            }).Serialize(w, input.OrderByDescending(o => o).ToArray()))).ToString();

        }

        [FlowInterface("/.table")]
        public IList<IList<String>> OutputStorageObjectsAsTable(IEnumerable<StorageObject> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            switch (input.First().ObjectType)
            {
                case StorageObjectTypes.Account:
                    return Make.Sequence(Make.Array("Id", "Realm", "Seed"))
                        .Concat(input.OfType<Account>().Select(a => Make.Array(
                            a.Id.ToString(),
                            a.Realm,
                            a.Seed
                        )))
                        .ToArray();
                case StorageObjectTypes.Activity:
                    return Make.Sequence(Make.Array("Id", "Account", "Parent", "Name", "Value"))
                        .Concat(input.OfType<Activity>().Select(a => Make.Array(
                            a.Id.ToString(),
                            a.Account.ToString(),
                            a.Depth > 0 ? a.Ancestors.First().ToString() : "",
                            a.Name,
                            a.GetValue<String>()
                        )))
                        .ToArray();
                default: // case StorageObjectTypes.Advertisement:
                    return Make.Sequence(Make.Array("Id", "Activity", "Timestamp", "Flags"))
                        .Concat(input.OfType<Advertisement>().Select(a => Make.Array(
                            a.Id.ToString(),
                            a.Activity.ToString(),
                            a.Timestamp.ToString("s"),
                            a.Flags.ToString()
                        )))
                        .ToArray();
            }
        }

        [FlowInterface("/.table.xml")]
        public String OutputStorageObjectsAsTableXml(IEnumerable<StorageObject> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return this.OutputStorageObjectsAsTable(input, session, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractSerializer>();
        }

        [FlowInterface("/.table.json")]
        public String OutputStorageObjectsAsTableJson(IEnumerable<StorageObject> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return this.OutputStorageObjectsAsTable(input, session, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractJsonSerializer>();
        }

        #endregion

        #region StorageObject (Streaming)

        [FlowInterface("/.xml")]
        public IObservable<String> OutputStorageObjectAsXmlStream(IObservable<StorageObject> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            DataContractSerializer serializer = new DataContractSerializer(
                typeof(StorageObject),
                null,
                Int32.MaxValue,
                true,
                false,
                new StorageObjectIdConverter()
            );
            return input.Select(o => o.XmlObjectSerializeToString(serializer));
        }

        [FlowInterface("/.json")]
        public IObservable<String> OutputStorageObjectAsJsonStream(IObservable<StorageObject> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings()
            {
                Converters = new JsonConverter[] { new StorageObjectIdConverter(), },
                DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                TypeNameHandling = TypeNameHandling.None,
            });
            return input.Select(o => new StringWriter().Apply(_ => _.Dispose(w => serializer.Serialize(w, o))).ToString());
        }

        [FlowInterface("/.custom.json")]
        public IObservable<String> OutputStorageObjectsAsCustomJsonStream(IObservable<StorageObject> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            IDisposable _ = session.SuppressDispose();
            return input
                .Select(o => JObject.FromObject(TriDQL.ParseLambda<StorageObject, Object>(args["code"]).Compile()(o))
                    .ToString(args.Contains("oneline", "true") ? Formatting.None : Formatting.Indented) + "\r\n"
                )
                .Catch((Exception ex) => Observable.Return(ex.ToString()))
                .Finally(_.Dispose);
        }

        #endregion

        #region RequestTask

        [FlowInterface("/.table")]
        public IList<IList<String>> OutputRequestTasksAsTable(IEnumerable<IRequestTask> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return Make.Sequence(Make.Array("Id", "State", "StartedAt", "ExitedAt", "Elapsed", "Step", "Request", "OutputType"))
                .Concat(input.OfType<IRequestTask>().Select(t => Make.Array(
                    t.Id.ToString(),
                    t.State.ToString(),
                    t.StartTime != null ? t.StartTime.Value.ToString("r") : "-",
                    t.ExitTime != null ? t.ExitTime.Value.ToString("r") : "-",
                    t.ElapsedTime.ToString(),
                    t.StepCount.ToString(),
                    t.Request.ToString(),
                    t.OutputType != null ? t.OutputType.Name : "-"
                )))
                .ToArray();
        }

        [FlowInterface("/.table.xml")]
        public String OutputRequestTasksAsTableXml(IEnumerable<IRequestTask> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return this.OutputRequestTasksAsTable(input, session, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractSerializer>();
        }

        [FlowInterface("/.table.json")]
        public String OutputRequestTasksAsTableJson(IEnumerable<IRequestTask> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return this.OutputRequestTasksAsTable(input, session, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractJsonSerializer>();
        }

        #endregion

        #region IModule

        [FlowInterface("/.table")]
        public IList<IList<String>> OutputModuleObjectsAsTable(IEnumerable<IModule> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return Make.Sequence(Make.Array("Name", "Kind", "Type", "DomainID", "State"))
                .Concat(input.OfType<IModule>().Select(m => Make.Array(
                    m.Name,
                    GetKind(m),
                    m.GetType().Name,
                    m.Domain.AppDomain.Id.ToString(),
                    m is ServantModule
                        ? ((ServantModule) m).IsStarted
                              ? "Start"
                              : "Stop"
                        : "-"
                )))
                .ToArray();
        }

        [FlowInterface("/.table.xml")]
        public String OutputModuleObjectsAsTableXml(IEnumerable<IModule> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return this.OutputModuleObjectsAsTable(input, session, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractSerializer>();
        }

        [FlowInterface("/.table.json")]
        public String OutputModuleObjectsAsTableJson(IEnumerable<IModule> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return this.OutputModuleObjectsAsTable(input, session, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractJsonSerializer>();
        }

        [FlowInterface("/.table")]
        public IList<IList<String>> OutputModuleDomainsAsTable(IEnumerable<IModuleDomain> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return Make.Sequence(Make.Array("Name", "DomainID", "DomainName", "ModuleCount"))
                .Concat(input.OfType<IModuleDomain>().Select(d => Make.Array(
                    d.Key,
                    d.AppDomain.Id.ToString(),
                    d.AppDomain.FriendlyName,
                    d.Modules.Count.ToString()
                )))
                .ToArray();
        }

        [FlowInterface("/.table.xml")]
        public String OutputModuleDomainsAsTableXml(IEnumerable<IModuleDomain> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return this.OutputModuleDomainsAsTable(input, session, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractSerializer>();
        }

        [FlowInterface("/.table.json")]
        public String OutputModuleDomainsAsTableJson(IEnumerable<IModuleDomain> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return this.OutputModuleDomainsAsTable(input, session, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractJsonSerializer>();
        }

        private static String GetKind(IModule module)
        {
            if (module is FlowModule)
            {
                return "Flow";
            }
            else if (module is ServantModule)
            {
                return "Servant";
            }
            else if (module is StorageModule)
            {
                return "Storage";
            }
            else
            {
                return "?Module";
            }
        }

        #endregion

        #region FlowInterfaceInfo

        [FlowInterface("/.table")]
        public IList<IList<String>> OutputFlowInterfacesAsTable(IEnumerable<FlowInterfaceInfo> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return Make.Sequence(Make.Array("ID", "Summary", "Remarks", "Input", "Output", "Flags"))
                .Concat(input.OfType<FlowInterfaceInfo>().Select(i => Make.Array(
                    i.Id,
                    i.Summary ?? "(null)",
                    i.Remarks ?? "(null)",
                    i.InputType != null
                        ? i.InputType.ToString().Substring(i.InputType.Namespace.Length + 1)
                        : "-",
                    i.OutputType.ToString().Substring(i.OutputType.Namespace.Length + 1),
                    String.Concat(
                        i.RequiresInput ? "<tt title='Requires input'>I</tt>" : "<tt title='Not requires input'>-</tt>",
                        i.HandlesVariables ? "<tt title='Handles variables'>V</tt>" : "<tt title='Not handles variables'>-</tt>"
                    )
                )))
                .ToArray();
        }

        [FlowInterface("/.table.xml")]
        public String OutputFlowInterfacesAsTableXml(IEnumerable<FlowInterfaceInfo> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return this.OutputFlowInterfacesAsTable(input, session, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractSerializer>();
        }

        [FlowInterface("/.table.json")]
        public String OutputFlowInterfacesAsTableJson(IEnumerable<FlowInterfaceInfo> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return this.OutputFlowInterfacesAsTable(input, session, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractJsonSerializer>();
        }

        #endregion

        #region StoredRequest

        [FlowInterface("/.table")]
        public IList<IList<String>> OutputStoredRequestAsTable(IEnumerable<StoredRequest> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return (args.Contains("sign", "true")
                ? Make.Sequence(Make.Array("Name", "Description", "Signature"))
                      .Concat(input.OfType<StoredRequest>().Select(s => Make.Array(
                          s.Name,
                          s.Description,
                          "[" +
                              s.Parameters.Values
                                  .Select(d => "{" + d.Select(p => String.Format("\"{0}\":\"{1}\"", p.Key, p.Value)).Join(",") + "}")
                                  .Join(",") +
                          "]"
                      )))
                : Make.Sequence(Make.Array("Name", "Description", "Parameters"))
                      .Concat(input.OfType<StoredRequest>().Select(s => Make.Array(
                          s.Name,
                          s.Description,
                          s.Parameters.Values.Select(e => e["name"]).Join(", ")
                      )))
            ).ToArray();
        }

        [FlowInterface("/.table.xml")]
        public String OutputStoredRequestAsTableXml(IEnumerable<StoredRequest> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return this.OutputStoredRequestAsTable(input, session, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractSerializer>();
        }

        [FlowInterface("/.table.json")]
        public String OutputStoredRequestAsTableJson(IEnumerable<StoredRequest> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return this.OutputStoredRequestAsTable(input, session, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractJsonSerializer>();
        }


        #endregion

        #endregion
    }
}