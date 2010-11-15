// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * SystemFlow
 *   MetaTweet Input/Output modules which provides generic system instructions
 *   Part of MetaTweet
 * Copyright © 2008-2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
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
using XSpect.Configuration;
using XSpect.Extension;
using XSpect.MetaTweet.Modules;
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
        public Object OutputNull(IEnumerable<StorageObject> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return null;
        }

        [FlowInterface("/.null")]
        public String OutputNullString(IEnumerable<StorageObject> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return String.Empty;
        }

        [FlowInterface("/.id")]
        public Object OutputAsIs(Object input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return input;
        }

        #endregion

        #region StorageObject

        [FlowInterface("/.obj")]
        public IEnumerable<StorageObject> OutputStorageObjects(IEnumerable<StorageObject> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return input.OrderByDescending(o => o).AsTransparent();
        }

        [FlowInterface("/.bin")]
        public Byte[] OutputBinaryData(IEnumerable<StorageObject> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return input.OfType<Activity>().SingleOrDefault(a => a.Data != null && a.Data.Length > 0)
                .TryGetData();
        }

        [FlowInterface("/.xml")]
        public String OutputStorageObjectsAsXml(IEnumerable<StorageObject> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return input.OrderByDescending(o => o)
                .ToArray()
                .XmlObjectSerializeToString<IEnumerable<StorageObject>, DataContractSerializer>();
        }
        
        [FlowInterface("/.json")]
        public String OutputStorageObjectsAsJson(IEnumerable<StorageObject> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return input.OrderByDescending(o => o)
                .ToArray()
                .XmlObjectSerializeToString<IEnumerable<StorageObject>, DataContractJsonSerializer>();
        }

        [FlowInterface("/.table")]
        public IList<IList<String>> OutputStorageObjectsAsTable(IEnumerable<StorageObject> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            switch (input.First().ObjectType)
            {
                case StorageObjectTypes.Account:
                    return Make.Sequence(Make.Array("AccountId", "Realm", "SeedString"))
                        .Concat(input.OfType<Account>().Select(a => Make.Array(
                            a.AccountId,
                            a.Realm,
                            a.SeedString
                        )))
                        .ToArray();
                case StorageObjectTypes.Activity:
                    return Make.Sequence(Make.Array("Account", "Timestamp", "Category", "SubId", "Value", "Data"))
                        .Concat(input.OfType<Activity>().Select(a => Make.Array(
                            a.Account.ToString(),
                            a.Timestamp.ToString("s"),
                            a.Category,
                            a.SubId,
                            a.Value,
                            a.Data == null
                                ? String.Empty
                                : String.Format(
                                      "<a href='/!/obj/activities?query=accountId:{0} timestamp:{1} category:{2} subId:{3}/!/.bin'>Size: {4}</a>",
                                      a.AccountId,
                                      a.Timestamp.ToString("o"),
                                      a.Category,
                                      a.SubId,
                                      a.Data.Length.ToString()
                                  )
                        )))
                        .ToArray();
                case StorageObjectTypes.Annotation:
                    return Make.Sequence(Make.Array("Account", "Name", "Value"))
                        .Concat(input.OfType<Annotation>().Select(a => Make.Array(
                            a.Account.ToString(),
                            a.Name,
                            a.Value
                        )))
                        .ToArray();
                case StorageObjectTypes.Relation:
                    return Make.Sequence(Make.Array("Account", "Name", "RelatingAccount"))
                        .Concat(input.OfType<Relation>().Select(r => Make.Array(
                            r.Account.ToString(),
                            r.Name,
                            r.RelatingAccount.ToString()
                        )))
                        .ToArray();
                case StorageObjectTypes.Mark:
                    return Make.Sequence(Make.Array("Account", "Name", "MarkingActivity"))
                        .Concat(input.OfType<Mark>().Select(m => Make.Array(
                            m.Account.ToString(),
                            m.Name,
                            m.MarkingActivity.ToString()
                        )))
                        .ToArray();
                case StorageObjectTypes.Reference:
                    return Make.Sequence(Make.Array("Activity", "Name", "ReferringActivity"))
                        .Concat(input.OfType<Reference>().Select(r => Make.Array(
                            r.Activity.ToString(),
                            r.Name,
                            r.ReferringActivity.ToString()
                        )))
                        .ToArray();
                default: // case StorageObjectTypes.Tag:
                    return Make.Sequence(Make.Array("Activity", "Name", "Value"))
                        .Concat(input.OfType<Tag>().Select(t => Make.Array(
                            t.Activity.ToString(),
                            t.Name,
                            t.Value.ToString()
                        )))
                        .ToArray();
            }
        }

        [FlowInterface("/.table.xml")]
        public String OutputStorageObjectsAsTableXml(IEnumerable<StorageObject> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return this.OutputStorageObjectsAsTable(input, storage, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractSerializer>();
        }

        [FlowInterface("/.table.json")]
        public String OutputStorageObjectsAsTableJson(IEnumerable<StorageObject> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return this.OutputStorageObjectsAsTable(input, storage, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractJsonSerializer>();
        }

        #endregion

        #region RequestTask

        [FlowInterface("/.table")]
        public IList<IList<String>> OutputRequestTasksAsTable(IEnumerable<RequestTask> input, StorageModule storage, String param, IDictionary<String, String> args)
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
        public String OutputRequestTasksAsTableXml(IEnumerable<RequestTask> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return this.OutputRequestTasksAsTable(input, storage, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractSerializer>();
        }

        [FlowInterface("/.table.json")]
        public String OutputRequestTasksAsTableJson(IEnumerable<RequestTask> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return this.OutputRequestTasksAsTable(input, storage, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractJsonSerializer>();
        }

        #endregion

        #region IModule

        [FlowInterface("/.table")]
        public IList<IList<String>> OutputModuleObjectsAsTable(IEnumerable<IModule> input, StorageModule storage, String param, IDictionary<String, String> args)
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
        public String OutputModuleObjectsAsTableXml(IEnumerable<IModule> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return this.OutputModuleObjectsAsTable(input, storage, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractSerializer>();
        }

        [FlowInterface("/.table.json")]
        public String OutputModuleObjectsAsTableJson(IEnumerable<IModule> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return this.OutputModuleObjectsAsTable(input, storage, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractJsonSerializer>();
        }

        [FlowInterface("/.table")]
        public IList<IList<String>> OutputModuleDomainsAsTable(IEnumerable<ModuleDomain> input, StorageModule storage, String param, IDictionary<String, String> args)
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
        public String OutputModuleDomainsAsTableXml(IEnumerable<ModuleDomain> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return this.OutputModuleDomainsAsTable(input, storage, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractSerializer>();
        }

        [FlowInterface("/.table.json")]
        public String OutputModuleDomainsAsTableJson(IEnumerable<ModuleDomain> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return this.OutputModuleDomainsAsTable(input, storage, param, args)
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
        public IList<IList<String>> OutputFlowInterfacesAsTable(IEnumerable<FlowInterfaceInfo> input, StorageModule storage, String param, IDictionary<String, String> args)
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
        public String OutputFlowInterfacesAsTableXml(IEnumerable<FlowInterfaceInfo> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return this.OutputFlowInterfacesAsTable(input, storage, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractSerializer>();
        }

        [FlowInterface("/.table.json")]
        public String OutputFlowInterfacesAsTableJson(IEnumerable<FlowInterfaceInfo> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return this.OutputFlowInterfacesAsTable(input, storage, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractJsonSerializer>();
        }

        #endregion

        #region StoredRequest

        [FlowInterface("/.table")]
        public IList<IList<String>> OutputStoredRequestAsTable(IEnumerable<StoredRequest> input, StorageModule storage, String param, IDictionary<String, String> args)
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
        public String OutputStoredRequestAsTableXml(IEnumerable<StoredRequest> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return this.OutputStoredRequestAsTable(input, storage, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractSerializer>();
        }

        [FlowInterface("/.table.json")]
        public String OutputStoredRequestAsTableJson(IEnumerable<StoredRequest> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return this.OutputStoredRequestAsTable(input, storage, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractJsonSerializer>();
        }


        #endregion
    }
}