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
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XSpect.Extension;
using XSpect.MetaTweet.Objects;
using XSpect.Codecs;
using Achiral;
using Achiral.Extension;
using XSpect.MetaTweet.Requesting;

namespace XSpect.MetaTweet.Modules
{
    public class SystemOutput
        : OutputFlowModule
    {
        #region Common

        [FlowInterface("/.null")]
        public Object OutputNull(IEnumerable<StorageObject> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return null;
        }

        [FlowInterface("/.null")]
        public String OutputNullString(IEnumerable<StorageObject> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return String.Empty;
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
            return input.OrderByDescending(o => o).AsTransparent();
        }

        [FlowInterface("/.bin")]
        public Byte[] OutputBinaryData(IEnumerable<StorageObject> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return input.OfType<Activity>().SingleOrDefault(a => a.GetValue().Type == JTokenType.Bytes)
                .GetValue<Byte[]>();
        }

        [FlowInterface("/.xml")]
        public String OutputStorageObjectsAsXml(IEnumerable<StorageObject> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return input.OrderByDescending(o => o)
                .ToArray()
                .XmlObjectSerializeToString<IEnumerable<StorageObject>, DataContractSerializer>();
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
                            a.AncestorIds.Count > 0 ? a.Ancestors.First().ToString() : "",
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
            return input.Select(o => o.XmlObjectSerializeToString<StorageObject, DataContractSerializer>());
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

        #endregion

        #region RequestTask

        [FlowInterface("/.table")]
        public IList<IList<String>> OutputRequestTasksAsTable(IEnumerable<RequestTask> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return Make.Sequence(Make.Array("Id", "State", "StartedAt", "ExitedAt", "Elapsed", "Position", "Request", "OutputType"))
                .Concat(input.OfType<RequestTask>().Select(t => Make.Array(
                    t.Id.ToString(),
                    t.State.ToString(),
                    t.StartTime != null ? t.StartTime.Value.ToString("r") : "-",
                    t.ExitTime != null ? t.ExitTime.Value.ToString("r") : "-",
                    t.ElapsedTime.ToString(),
                    t.CurrentPosition + " / " + t.RequestFragmentCount,
                    t.Request.ToString(),
                    t.OutputType != null ? t.OutputType.Name : "-"
                )))
                .ToArray();
        }

        [FlowInterface("/.table.xml")]
        public String OutputRequestTasksAsTableXml(IEnumerable<RequestTask> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return this.OutputRequestTasksAsTable(input, session, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractSerializer>();
        }

        [FlowInterface("/.table.json")]
        public String OutputRequestTasksAsTableJson(IEnumerable<RequestTask> input, StorageSession session, String param, IDictionary<String, String> args)
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
        public IList<IList<String>> OutputModuleDomainsAsTable(IEnumerable<ModuleDomain> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return Make.Sequence(Make.Array("Name", "DomainID", "DomainName", "ModuleCount"))
                .Concat(input.OfType<ModuleDomain>().Select(d => Make.Array(
                    d.Key,
                    d.AppDomain.Id.ToString(),
                    d.AppDomain.FriendlyName,
                    d.Modules.Count.ToString()
                )))
                .ToArray();
        }

        [FlowInterface("/.table.xml")]
        public String OutputModuleDomainsAsTableXml(IEnumerable<ModuleDomain> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return this.OutputModuleDomainsAsTable(input, session, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractSerializer>();
        }

        [FlowInterface("/.table.json")]
        public String OutputModuleDomainsAsTableJson(IEnumerable<ModuleDomain> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return this.OutputModuleDomainsAsTable(input, session, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractJsonSerializer>();
        }

        private static String GetKind(IModule module)
        {
            if (module is FlowModule)
            {
                if (module is InputFlowModule)
                {
                    return "InputFlow";
                }
                else if (module is FilterFlowModule)
                {
                    return "FilterFlow";
                }
                else if (module is OutputFlowModule)
                {
                    return "OutputFlow";
                }
                else
                {
                    return "?Flow";
                }
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
                        i.ReturnsAdditionalData ? "<tt title='Returns additional data'>A</tt>" : "<tt title='Not returns additional data'>-</tt>"
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
    }
}