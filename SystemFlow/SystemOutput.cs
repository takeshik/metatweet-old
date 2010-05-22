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

namespace XSpect.MetaTweet.Modules
{
    public class SystemOutput
        : OutputFlowModule
    {
        protected override String DefaultRealm
        {
            get
            {
                return String.Empty;
            }
        }

        #region Common

        [FlowInterface("/.null", WriteTo = StorageObjectTypes.None)]
        public Object OutputNull(IEnumerable<StorageObject> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return null;
        }

        [FlowInterface("/.null", WriteTo = StorageObjectTypes.None)]
        public String OutputNullString(IEnumerable<StorageObject> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return String.Empty;
        }

        [FlowInterface("/.id", WriteTo = StorageObjectTypes.None)]
        public Object OutputAsIs(Object input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return input;
        }

        #endregion

        #region StorageObject

        [FlowInterface("/.obj", WriteTo = StorageObjectTypes.None)]
        public IEnumerable<StorageObject> OutputStorageObjects(IEnumerable<StorageObject> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return input.OrderByDescending(o => o).AsTransparent();
        }

        [FlowInterface("/.xml", WriteTo = StorageObjectTypes.None)]
        public String OutputStorageObjectsAsXml(IEnumerable<StorageObject> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return input.OrderByDescending(o => o)
                .ToArray()
                .XmlObjectSerializeToString<IEnumerable<StorageObject>, DataContractSerializer>();
        }
        
        [FlowInterface("/.json", WriteTo = StorageObjectTypes.None)]
        public String OutputStorageObjectsAsJson(IEnumerable<StorageObject> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return input.OrderByDescending(o => o)
                .ToArray()
                .XmlObjectSerializeToString<IEnumerable<StorageObject>, DataContractJsonSerializer>();
        }

        [FlowInterface("/.table", WriteTo = StorageObjectTypes.None)]
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
                    return Make.Sequence(Make.Array("Account", "Timestamp", "Category", "SubId", "Value", "sizeof(Data)"))
                        .Concat(input.OfType<Activity>().Select(a => Make.Array(
                            a.Account.ToString(),
                            a.Timestamp.ToString("s"),
                            a.Category,
                            a.SubId,
                            a.Value,
                            a.Data == null ? String.Empty : a.Data.Length.ToString()
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

        [FlowInterface("/.table.xml", WriteTo = StorageObjectTypes.None)]
        public String OutputStorageObjectsAsTableXml(IEnumerable<StorageObject> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return this.OutputStorageObjectsAsTable(input, storage, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractSerializer>();
        }

        [FlowInterface("/.table.json", WriteTo = StorageObjectTypes.None)]
        public String OutputStorageObjectsAsTableJson(IEnumerable<StorageObject> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return this.OutputStorageObjectsAsTable(input, storage, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractJsonSerializer>();
        }

        #endregion
    }
}