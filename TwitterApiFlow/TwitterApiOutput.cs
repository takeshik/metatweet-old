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

        [FlowInterface("/.xml")]
        public String OutputTwitterXmlFormat(IEnumerable<StorageObject> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            String s;
            Account subject = this.GetAccount(storage, args.GetValueOrDefault(
                "subject",
                this.Configuration.TryResolveValue("defaultSubject", out s) && !String.IsNullOrWhiteSpace(s)
                    ? s
                    : this.Host.ModuleManager.GetModule<TwitterApiInput>(this.Name).Context.UserName
            ));
            String type = input.All(o => o is Activity && ((Activity) o).Category == "Post")
                ? "statuses"
                : input.All(o => o is Account)
                      ? "users"
                      : "objects";
            return new StringWriter().Apply(
                // TODO: Support <users> output: check input elements' type?
                new XDocument(
                    new XDeclaration("1.0", "utf-16", "yes"),
                    new XElement(type,
                        new XAttribute("type", "array"),
                        new XAttribute("metatweet-version", ThisAssembly.EntireCommitId),
                        input.OrderByDescending(o => o).Select(o => o is Account
                            ? this.OutputUser((Account) o, subject, true)
                            : o is Activity && ((Activity) o).Category == "Post"
                                  ? this.OutputStatus((Activity) o, subject, true)
                                  : new XElement("not-supported-object")
                        )
                    )
                ).Save).ToString();
        }

        [FlowInterface("/.hr.table")]
        public IList<IList<String>> OutputHumanReadableTable(IEnumerable<StorageObject> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            String s;
            Account subject = this.GetAccount(storage, args.GetValueOrDefault(
                "subject",
                this.Configuration.TryResolveValue("defaultSubject", out s) && !String.IsNullOrWhiteSpace(s)
                    ? s
                    : this.Host.ModuleManager.GetModule<TwitterApiInput>(this.Name).Context.UserName
            ));
            switch (input.First().ObjectType)
            {
                case StorageObjectTypes.Account:
                    return Make.Sequence(Make.Array("ID", "ScreenName", "Name", "Location", "Bio", "Web", "F/F", "Flags"))
                        .Concat(input.OfType<Account>().Select(a => Make.Array(
                            String.Format(
                                "<span title='{1}'>{0}</span>",
                                a["Id"].TryGetValue(),
                                a.AccountId
                            ),
                            a["ScreenName"].TryGetValue(),
                            a["Name"].TryGetValue(),
                            a["Location"].TryGetValue(),
                            a["Description"].TryGetValue(),
                            a["Uri"].TryGetValue(),
                            a["FollowingCount"].TryGetValue() + " / " + a["FollowersCount"].TryGetValue(),
                            String.Concat(
                                a["Restricted"].TryGetValue() == "True" ? "<tt title='Protected'>P</tt>" : "<tt title='Not protected'>-</tt>",
                                a.IsRelated("Follow", subject) ? "<tt title='Following'>F</tt>" : "<tt title='Not following'>-</tt>",
                                a.IsRelating("Follow", subject) ? "<tt title='Follower'>f</tt>" : "<tt title='Not follower'>-</tt>"
                            )
                        )))
                        .ToArray();
                case StorageObjectTypes.Activity:
                    return Make.Sequence(Make.Array("User", "Timestamp", "Category", "Text", "Flags", "Source"))
                        .Concat(input.OfType<Activity>().Select(a => Make.Array(
                            String.Format(
                                "<span title='{1} ({2})'>{0}</span>",
                                a.Account["ScreenName"].TryGetValue(),
                                a.Account["Name"].TryGetValue(),
                                a.Account["Id"].TryGetValue()
                            ),
                            a.Timestamp.ToLocalTime().ToString("yy/MM/dd HH:mm:ss"),
                            a.Category,
                            a.Value,
                            String.Concat(
                                a.Account["Restricted"].TryGetValue() == "True" ? "<tt title='Protected'>P</tt>" : "<tt title='Not protected'>-</tt>",
                                a.Account.IsRelated("Follow", subject) ? "<tt title='Following'>F</tt>" : "<tt title='Not following'>-</tt>",
                                a.Account.IsRelating("Follow", subject) ? "<tt title='Follower'>f</tt>" : "<tt title='Not follower'>-</tt>",
                                a.IsMarked("Favorite", subject) ? "<tt title='Favorited'>S</tt>" : "<tt title='Not favorited'>-</tt>"
                            ),
                            a.UserAgent
                        )))
                        .ToArray();
                case StorageObjectTypes.Annotation:
                    return Make.Sequence(Make.Array("ID", "ScreenName", "UserName", "Name", "Value"))
                        .Concat(input.OfType<Annotation>().Select(a => Make.Array(
                            String.Format(
                                "<span title='{1}'>{0}</span>",
                                a.Account["Id"].TryGetValue(),
                                a.AccountId
                            ),
                            a.Account["ScreenName"].TryGetValue(),
                            a.Account["Name"].TryGetValue(),
                            a.Name,
                            a.Value
                        )))
                        .ToArray();
                case StorageObjectTypes.Relation:
                    return Make.Sequence(Make.Array("ID", "ScreenName", "UserName", "Name", "RelID", "RelScreenName", "RelName"))
                        .Concat(input.OfType<Relation>().Select(r => Make.Array(
                            String.Format(
                                "<span title='{1}'>{0}</span>",
                                r.Account["Id"].TryGetValue(),
                                r.AccountId
                            ),
                            r.Account["ScreenName"].TryGetValue(),
                            r.Account["Name"].TryGetValue(),
                            r.Name,
                            String.Format(
                                "<span title='{1}'>{0}</span>",
                                r.RelatingAccount["Id"].TryGetValue(),
                                r.RelatingAccountId
                            ),
                            r.RelatingAccount["ScreenName"].TryGetValue(),
                            r.RelatingAccount["Name"].TryGetValue()
                        )))
                        .ToArray();
                case StorageObjectTypes.Mark:
                    return Make.Sequence(Make.Array("ID", "ScreenName", "UserName", "Name", "MarkUser", "MarkTimestamp", "MarkCategory", "MarkText"))
                        .Concat(input.OfType<Mark>().Select(m => Make.Array(
                            String.Format(
                                "<span title='{1}'>{0}</span>",
                                m.Account["Id"].TryGetValue(),
                                m.AccountId
                            ),
                            m.Account["ScreenName"].TryGetValue(),
                            m.Account["Name"].TryGetValue(),
                            m.Name,
                            String.Format(
                                "<span title='{1} ({2})'>{0}</span>",
                                m.MarkingActivity.Account["ScreenName"].TryGetValue(),
                                m.MarkingActivity.Account["Name"].TryGetValue(),
                                m.MarkingActivity.Account["Id"].TryGetValue()
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
                                r.Activity.Account["ScreenName"].TryGetValue(),
                                r.Activity.Account["Name"].TryGetValue(),
                                r.Activity.Account["Id"].TryGetValue()
                            ),
                            r.Activity.Timestamp.ToLocalTime().ToString("yy/MM/dd HH:mm:ss"),
                            r.Activity.Category,
                            r.Activity.Value,
                            r.Name,
                            String.Format(
                                "<span title='{1} ({2})'>{0}</span>",
                                r.ReferringActivity.Account["ScreenName"].TryGetValue(),
                                r.ReferringActivity.Account["Name"].TryGetValue(),
                                r.ReferringActivity.Account["Id"].TryGetValue()
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
                                t.Activity.Account["ScreenName"].TryGetValue(),
                                t.Activity.Account["Name"].TryGetValue(),
                                t.Activity.Account["Id"].TryGetValue()
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

        [FlowInterface("/.hr.table.xml")]
        public String OutputHumanReadableTableXml(IEnumerable<StorageObject> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return this.OutputHumanReadableTable(input, storage, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractSerializer>();
        }

        [FlowInterface("/.hr.table.json")]
        public String OutputHumanReadableTableJson(IEnumerable<StorageObject> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return this.OutputHumanReadableTable(input, storage, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractJsonSerializer>();
        }

        [FlowInterface("/.icons.table")]
        public IList<IList<String>> OutputIconListTable(IEnumerable<StorageObject> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return Make.Sequence(Make.Array("ID", "ScreenName", "Name", "Timestamp", "Image"))
                .Concat(input.OfType<Activity>()
                    .Where(a => a.Category =="ProfileImage" && a.Data != null && a.Data.Length > 0)
                    .Select(a => Make.Array(
                        String.Format(
                            "<span title='{1}'>{0}</span>",
                            a.Account["Id"].TryGetValue(),
                            a.AccountId
                        ),
                        a.Account["ScreenName"].TryGetValue(),
                        a.Account["Name"].TryGetValue(),
                        a.Timestamp.ToLocalTime().ToString("yy/MM/dd HH:mm:ss"),
                        String.Format(
                            "<img src='/!/obj/activities?query=accountId:{0} timestamp:{1} category:{2} subId:{3}/!/.bin' title='{4}' />",
                            a.AccountId,
                            a.Timestamp.ToString("o"),
                            a.Category,
                            a.SubId,
                            a.Value.Substring(a.Value.LastIndexOf('/') + 1)
                        )
                    ))
                )
                .ToArray();
        }

        [FlowInterface("/.icons.table.xml")]
        public String OutputIconListTableXml(IEnumerable<StorageObject> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return this.OutputIconListTable(input, storage, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractSerializer>();
        }

        [FlowInterface("/.icons.table.json")]
        public String OutputIconListTableJson(IEnumerable<StorageObject> input, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return this.OutputIconListTable(input, storage, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractJsonSerializer>();
        }

        private XElement OutputStatus(Activity activity, Account subject, Boolean includesUser)
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
                    .Let(m => Make.Array(
                        new XElement("in_reply_to_status_id", m.Null(_ => _.SubId)),
                        new XElement("in_reply_to_user_id", m.Null(_ => _.Account["Id"].TryGetValue())),
                        new XElement("in_reply_to_screen_name", m.Null(_ => _.Account["ScreenName"].TryGetValue()))
                    )),
                new XElement("favorited", activity.IsMarked("Favorite", subject).ToString().ToLower()),
                includesUser ? Make.Array(this.OutputUser(activity.Account, subject, false)) : null
            );
        }

        private XElement OutputUser(Account account, Account subject, Boolean includesStatus)
        {
            return new XElement("user",
                new XAttribute("metatweet-account-id", account.AccountId),
                new XElement("id", account["Id"].TryGetValue()),
                new XElement("name", account["Name"].TryGetValue()),
                new XElement("screen_name", account["ScreenName"].TryGetValue()),
                new XElement("location", account["Location"].TryGetValue()),
                new XElement("profile_image_url", account["ProfileImage"].TryGetValue()),
                new XElement("url", account["Uri"].TryGetValue()),
                new XElement("followers_count", account["FollowersCount"].TryGetValue()),
                new XElement("friends_count", account["FollowingCount"].TryGetValue()),
                new XElement("created_at", account["CreatedAt"].Null(a =>
                    DateTime.Parse(
                        a.TryGetValue(),
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.RoundtripKind
                    )
                        .ToString("ddd MMM dd HH:mm:ss +0000 yyyy", CultureInfo.InvariantCulture)
                )),
                new XElement("favourites_count", account["FavoritesCount"].TryGetValue()),
                new XElement("statuses_count", account["StatusesCount"].TryGetValue()),
                new XElement("following", account.IsRelated("Follow", subject)),
                includesStatus && account["Post"] != null ? Make.Array(this.OutputStatus(account["Post"], subject, false)) : null
            );
        }

        private Account GetAccount(StorageModule storage, String screenName)
        {
            return storage.GetActivities(StorageObjectEntityQuery.Activity(
                scalarMatch: new ActivityTuple()
                {
                    Category = "ScreenName",
                    Value = screenName,
                },
                postExpression: _ => _.OrderByDescending(a => a)
            ))
                .FirstOrDefault()
                .Account;
        }
    }
}