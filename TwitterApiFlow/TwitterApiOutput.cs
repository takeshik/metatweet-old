// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * TwitterApiFlow
 *   MetaTweet Input/Output modules which provides Twitter access with API
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
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using Achiral;
using Achiral.Extension;
using XSpect.Codecs;
using XSpect.Extension;
using XSpect.MetaTweet.Modules;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Net;
using XSpect.MetaTweet.Objects;

namespace XSpect.MetaTweet.Modules
{
    public class TwitterApiOutput
        : OutputFlowModule
    {
        protected override String DefaultRealm
        {
            get
            {
                return "com.twitter";
            }
        }

        public Account Myself
        {
            get;
            private set;
        }

        [FlowInterface("/.xml", WriteTo = StorageObjectTypes.None)]
        public String OutputTwitterXmlFormat(IEnumerable<StorageObject> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            String type = input.All(o => o is Activity && (o as Activity).Category == "Post")
                ? "statuses"
                : input.All(o => o is Account)
                      ? "users"
                      : "objects";
            this.CheckMyself(storage);
            return new StringWriter().Let(
                // TODO: Support <users> output: check input elements' type?
                new XDocument(
                    new XDeclaration("1.0", "utf-16", "yes"),
                    new XElement(type,
                        new XAttribute("type", "array"),
                        new XAttribute("metatweet-version", ThisAssembly.EntireCommitId),
                        input.OrderByDescending(o => o).Select(o => o is Account
                            ? this.OutputUser(o as Account, true)
                            : o is Activity && (o as Activity).Category == "Post"
                                  ? this.OutputStatus(o as Activity, true)
                                  : new XElement("not-supported-object")
                        )
                    )
                ).Save).ToString();
        }

        [FlowInterface("/.hr.table", WriteTo = StorageObjectTypes.None)]
        public IList<IList<String>> OutputHumanReadableTable(IEnumerable<StorageObject> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            switch (input.First().ObjectType)
            {
                case StorageObjectTypes.Account:
                    return Make.Sequence(Make.Array("ID", "ScreenName", "Name", "Location", "Bio", "Web", "F/F", "Flags"))
                        .Concat(input.OfType<Account>().Select(a => Make.Array(
                            String.Format(
                                "<span title='{1}'>{0}</span>",
                                a["Id"].Value,
                                a.AccountId
                            ),
                            a["ScreenName"].Value,
                            a["Name"].Value,
                            a["Location"].Value,
                            a["Description"].Value,
                            a["Uri"].Value,
                            a["FollowingCount"].Value + " / " + a["FollowersCount"].Value,
                            String.Concat(
                                a["Restricted"].Value == "True" ? "<span title='Protected'>P</span>" : "<span title='Not protected'>-</span>",
                                a.IsRelated("Follow", this.Myself) ? "<span title='Following'>F</span>" : "<span title='Not following'>-</span>",
                                a.IsRelating("Follow", this.Myself) ? "<span title='Follower'>f</span>" : "<span title='Not follower'>-</span>"
                            )
                        )))
                        .ToArray();
                case StorageObjectTypes.Activity:
                    return Make.Sequence(Make.Array("User", "Timestamp", "Category", "Text", "Flags", "Source"))
                        .Concat(input.OfType<Activity>().Select(a => Make.Array(
                            String.Format(
                                "<span title='{1} ({2})'>{0}</span>",
                                a.Account["ScreenName"].Value,
                                a.Account["Name"].Value,
                                a.Account["Id"].Value
                            ),
                            a.Timestamp.ToLocalTime().ToString("yy/MM/dd HH:mm:ss"),
                            a.Category,
                            a.Value,
                            String.Concat(
                                a.Account["Restricted"].Value == "True" ? "<span title='Protected'>P</span>" : "<span title='Not protected'>-</span>",
                                a.Account.IsRelated("Follow", this.Myself) ? "<span title='Following'>F</span>" : "<span title='Not following'>-</span>",
                                a.Account.IsRelating("Follow", this.Myself) ? "<span title='Follower'>f</span>" : "<span title='Not follower'>-</span>",
                                a.IsMarked("Favorite", this.Myself) ? "<span title='Favorited'>S</span>" : "<span title='Not favorited'>-</span>"
                            ),
                            a.UserAgent
                        )))
                        .ToArray();
                case StorageObjectTypes.Annotation:
                    return Make.Sequence(Make.Array("ID", "ScreenName", "UserName", "Name", "Value"))
                        .Concat(input.OfType<Annotation>().Select(a => Make.Array(
                            String.Format(
                                "<span title='{1}'>{0}</span>",
                                a.Account["Id"].Value,
                                a.AccountId
                            ),
                            a.Account["ScreenName"].Value,
                            a.Account["Name"].Value,
                            a.Name,
                            a.Value
                        )))
                        .ToArray();
                case StorageObjectTypes.Relation:
                    return Make.Sequence(Make.Array("ID", "ScreenName", "UserName", "Name", "RelID", "RelScreenName", "RelName"))
                        .Concat(input.OfType<Relation>().Select(r => Make.Array(
                            String.Format(
                                "<span title='{1}'>{0}</span>",
                                r.Account["Id"].Value,
                                r.AccountId
                            ),
                            r.Account["ScreenName"].Value,
                            r.Account["Name"].Value,
                            r.Name,
                            String.Format(
                                "<span title='{1}'>{0}</span>",
                                r.RelatingAccount["Id"].Value,
                                r.RelatingAccountId
                            ),
                            r.RelatingAccount["ScreenName"].Value,
                            r.RelatingAccount["Name"].Value
                        )))
                        .ToArray();
                case StorageObjectTypes.Mark:
                    return Make.Sequence(Make.Array("ID", "ScreenName", "UserName", "Name", "MarkUser", "MarkTimestamp", "MarkCategory", "MarkText"))
                        .Concat(input.OfType<Mark>().Select(m => Make.Array(
                            String.Format(
                                "<span title='{1}'>{0}</span>",
                                m.Account["Id"].Value,
                                m.AccountId
                            ),
                            m.Account["ScreenName"].Value,
                            m.Account["Name"].Value,
                            m.Name,
                            String.Format(
                                "<span title='{1} ({2})'>{0}</span>",
                                m.MarkingActivity.Account["ScreenName"].Value,
                                m.MarkingActivity.Account["Name"].Value,
                                m.MarkingActivity.Account["Id"].Value
                            ),
                            m.MarkingActivity.Timestamp.ToLocalTime().ToString("yy/MM/dd HH:mm:ss"),
                            m.MarkingActivity.Category,
                            m.MarkingActivity.Value
                        )))
                        .ToArray();
                case StorageObjectTypes.Reference:
                    return Make.Sequence(Make.Array("User", "Timestamp", "Category", "Name", "Text", "RefUser", "RefTimestamp", "RefCategory", "RefText"))
                        .Concat(input.OfType<Reference>().Select(r => Make.Array(
                            String.Format(
                                "<span title='{1} ({2})'>{0}</span>",
                                r.Activity.Account["ScreenName"].Value,
                                r.Activity.Account["Name"].Value,
                                r.Activity.Account["Id"].Value
                            ),
                            r.Activity.Timestamp.ToLocalTime().ToString("yy/MM/dd HH:mm:ss"),
                            r.Activity.Category,
                            r.Activity.Value,
                            r.Name,
                            String.Format(
                                "<span title='{1} ({2})'>{0}</span>",
                                r.ReferringActivity.Account["ScreenName"].Value,
                                r.ReferringActivity.Account["Name"].Value,
                                r.ReferringActivity.Account["Id"].Value
                            ),
                            r.ReferringActivity.Timestamp.ToLocalTime().ToString("yy/MM/dd HH:mm:ss"),
                            r.ReferringActivity.Category,
                            r.ReferringActivity.Value
                        )))
                        .ToArray();
                default: // case StorageObjectTypes.Tag:
                    return Make.Sequence(Make.Array("User", "Timestamp", "Category", "Text", "Name", "Value"))
                        .Concat(input.OfType<Tag>().Select(t => Make.Array(
                            String.Format(
                                "<span title='{1} ({2})'>{0}</span>",
                                t.Activity.Account["ScreenName"].Value,
                                t.Activity.Account["Name"].Value,
                                t.Activity.Account["Id"].Value
                            ),
                            t.Activity.Timestamp.ToLocalTime().ToString("yy/MM/dd HH:mm:ss"),
                            t.Activity.Category,
                            t.Activity.Value,
                            t.Name,
                            t.Value
                        )))
                        .ToArray();
            }
        }

        [FlowInterface("/.hr.table.xml", WriteTo = StorageObjectTypes.None)]
        public String OutputHumanReadableTableXml(IEnumerable<StorageObject> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return this.OutputHumanReadableTable(input, storage, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractSerializer>();
        }

        [FlowInterface("/.hr.table.json", WriteTo = StorageObjectTypes.None)]
        public String OutputHumanReadableTableJson(IEnumerable<StorageObject> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return this.OutputHumanReadableTable(input, storage, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractJsonSerializer>();
        }

        private XElement OutputStatus(Activity activity, Boolean includesUser)
        {
            return new XElement("status",
                new XElement("created_at", activity.Timestamp
                    .ToString("ddd MMM dd HH:mm:ss +0000 yyyy", CultureInfo.InvariantCulture)
                ),
                new XElement("id", activity.SubId),
                new XElement("text", activity.Value),
                new XElement("source", "<a href=\"zapped\"> rel=\"nofollow\">" + activity.UserAgent + "</a>"),
                new XElement("truncated", "false"),
                activity.ReferrersOf("Mention")
                // TODO: First?
                    .FirstOrDefault()
                    .Do(m => Make.Array(
                        new XElement("in_reply_to_status_id", m.Null(_ => _.SubId)),
                        new XElement("in_reply_to_user_id", m.Null(_ => _.Account["Id"].Value)),
                        new XElement("in_reply_to_screen_name", m.Null(_ => _.Account["ScreenName"].Value))
                    )),
                new XElement("favorited", activity.IsMarked("Favorite", this.Myself).ToString().ToLower()),
                includesUser ? Make.Array(this.OutputUser(activity.Account, false)) : null
            );
        }

        private XElement OutputUser(Account account, Boolean includesStatus)
        {
            return new XElement("user",
                new XAttribute("metatweet-account-id", account.AccountId),
                new XElement("id", account["Id"].Value),
                new XElement("name", account["Name"].Value),
                new XElement("screen_name", account["ScreenName"].Value),
                new XElement("location", account["Location"].Value),
                new XElement("profile_image_url", account["ProfileImage"].Value),
                new XElement("url", account["Uri"].Value),
                new XElement("followers_count", account["FollowersCount"].Value),
                new XElement("friends_count", account["FollowingCount"].Value),
                new XElement("created_at", DateTime.Parse(account["CreatedAt"].Value)
                    .ToString("ddd MMM dd HH:mm:ss +0000 yyyy", CultureInfo.InvariantCulture)
                ),
                new XElement("favourites_count", account["FavoritesCount"].Value),
                new XElement("statuses_count", account["StatusesCount"].Value),
                new XElement("following", account.IsRelated("Follow", this.Myself)),
                includesStatus && account["Post"] != null ? Make.Array(this.OutputStatus(account["Post"], false)) : null
            );
        }

        private void CheckMyself(StorageModule storage)
        {
            if (!this.Configuration.Exists("userName") || this.Configuration.ResolveValue<String>("userName").IsNullOrEmpty())
            {
                throw new InvalidOperationException("Please specify username in configuration file for this module.");
            }
            if (this.Myself == null)
            {
                this.Myself = storage.GetActivities(
                    default(String),
                    null,
                    "ScreenName",
                    null,
                    null,
                    this.Configuration.ResolveValue<String>("userName"),
                    null
                )
                    .AsEnumerable()
                    .OrderByDescending(a => a)
                    .FirstOrDefault()
                    .Account;
            }
        }
    }
}