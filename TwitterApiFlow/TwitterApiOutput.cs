// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * TwitterApiFlow
 *   MetaTweet Input/Output modules which provides Twitter access with API
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
        [FlowInterface("/.xml")]
        public String OutputTwitterXmlFormat(IEnumerable<StorageObject> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            Account subject = this.GetAccount(session, args.GetValueOrDefault(
                "subject",
                String.IsNullOrWhiteSpace(this.Configuration.DefaultSubject)
                    ? (String) this.Configuration.DefaultSubject
                    : this.Host.ModuleManager.GetModule<TwitterApiInput>(this.Name).Authorization.ScreenName
            ));
            String type = input.All(o => o is Activity && ((Activity) o).Name == "Status")
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
                            : o is Activity && ((Activity) o).Name == "Status"
                                  ? this.OutputStatus((Activity) o, subject, true)
                                  : new XElement("not-supported-object")
                        )
                    )
                ).Save).ToString();
        }

        [FlowInterface("/.hr.table")]
        public IList<IList<String>> OutputHumanReadableTable(IEnumerable<StorageObject> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            Account subject = this.GetAccount(session, args.GetValueOrDefault(
                "subject",
                String.IsNullOrWhiteSpace(this.Configuration.DefaultSubject)
                    ? (String) this.Configuration.DefaultSubject
                    : this.Host.ModuleManager.GetModule<TwitterApiInput>(this.Name).Authorization.ScreenName
            ));
            switch (input.First().ObjectType)
            {
                case StorageObjectTypes.Account:
                    return Make.Sequence(Make.Array("ID", "ScreenName", "Name", "Location", "Bio", "Web", "F/F", "Flags"))
                        .Concat(input.OfType<Account>().Select(acc => Make.Array(
                            String.Format(
                                "<span title='{1}'>{0}</span>",
                                acc.Lookup("Id").TryGetValue<Int64>(),
                                acc.Id.ToString(true)
                            ),
                            acc.Lookup("ScreenName").TryGetValue<String>(),
                            acc.Lookup("Name").TryGetValue<String>(),
                            acc.Lookup("Location").TryGetValue<String>(),
                            acc.Lookup("Description").TryGetValue<String>(),
                            acc.Lookup("Uri").TryGetValue<String>(),
                            acc.Lookup("FollowingCount").TryGetValue<Int32>() + " / " + acc.Lookup("FollowersCount").TryGetValue<Int32>(),
                            String.Concat(
                                acc.Lookup("Restricted").TryGetValue<Boolean>() ? "<tt title='Protected'>P</tt>" : "<tt title='Not protected'>-</tt>",
                                acc["Follow", subject.Id] != null ? "<tt title='Following'>F</tt>" : "<tt title='Not following'>-</tt>",
                                subject["Follow", acc.Id] != null ? "<tt title='Follower'>f</tt>" : "<tt title='Not follower'>-</tt>"
                            )
                        )))
                        .ToArray();
                case StorageObjectTypes.Activity:
                    return Make.Sequence(Make.Array("User", "Timestamp", "Category", "Text", "Flags", "Source"))
                        .Concat(input.OfType<Activity>().Select(act => act.Account.Let(acc => Make.Array(
                            String.Format(
                                "<span title='{1} ({2})'>{0}</span>",
                                acc.Lookup("ScreenName").TryGetValue<String>(),
                                acc.Lookup("Name").TryGetValue<String>(),
                                acc.Lookup("Id").TryGetValue<Int64>()
                            ),
                            act.LastTimestamp.If(t => t.HasValue, t => t.Value.ToLocalTime().ToString("yy/MM/dd HH:mm:ss"), t => ""),
                            act.Name,
                            act.GetValue<String>(),
                            String.Concat(
                                acc.Lookup("Restricted").TryGetValue<Boolean>() ? "<tt title='Protected'>P</tt>" : "<tt title='Not protected'>-</tt>",
                                acc["Follow", subject.Id] != null ? "<tt title='Following'>F</tt>" : "<tt title='Not following'>-</tt>",
                                subject["Follow", acc.Id] != null ? "<tt title='Follower'>f</tt>" : "<tt title='Not follower'>-</tt>",
                                subject["Favorite", act.Id] != null ? "<tt title='Favorited'>S</tt>" : "<tt title='Not favorited'>-</tt>"
                            ),
                            act["Source"].SingleOrDefault().TryGetValue<String>()
                        ))))
                        .ToArray();
                default: // case StorageObjectTypes.Advertisement:
                    return Make.Sequence(Make.Array("User", "Timestamp", "Category", "Text", "Flags", "Source"))
                        .Concat(input.OfType<Advertisement>().Select(adv => adv.Activity.Let(act => act.Account.Let(acc => Make.Array(
                            String.Format(
                                "<span title='{1} ({2})'>{0}</span>",
                                acc.Lookup("ScreenName").TryGetValue<String>(),
                                acc.Lookup("Name").TryGetValue<String>(),
                                acc.Lookup("Id").TryGetValue<Int64>()
                            ),
                            act.LastTimestamp.If(t => t.HasValue, t => t.Value.ToLocalTime().ToString("yy/MM/dd HH:mm:ss"), t => ""),
                            act.Name,
                            act.GetValue<String>(),
                            String.Concat(
                                acc.Lookup("Restricted").TryGetValue<Boolean>() ? "<tt title='Protected'>P</tt>" : "<tt title='Not protected'>-</tt>",
                                acc["Follow", subject.Id] != null ? "<tt title='Following'>F</tt>" : "<tt title='Not following'>-</tt>",
                                subject["Follow", acc.Id] != null ? "<tt title='Follower'>f</tt>" : "<tt title='Not follower'>-</tt>",
                                subject["Favorite", act.Id] != null ? "<tt title='Favorited'>S</tt>" : "<tt title='Not favorited'>-</tt>"
                            ),
                            act["Source"].SingleOrDefault().TryGetValue<String>()
                        )))))
                        .ToArray();
            }
        }

        [FlowInterface("/.hr.table.xml")]
        public String OutputHumanReadableTableXml(IEnumerable<StorageObject> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return this.OutputHumanReadableTable(input, session, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractSerializer>();
        }

        [FlowInterface("/.hr.table.json")]
        public String OutputHumanReadableTableJson(IEnumerable<StorageObject> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return this.OutputHumanReadableTable(input, session, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractJsonSerializer>();
        }

        [FlowInterface("/.icons.table")]
        public IList<IList<String>> OutputIconListTable(IEnumerable<StorageObject> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return Make.Sequence(Make.Array("ID", "ScreenName", "Name", "Timestamp", "Image"))
                .Concat(input.OfType<Activity>()
                    .Where(act => act.Name =="ProfileImage" && act["Image"].Any())
                    .Select(act => act.Account.Let(acc => Make.Array(
                        String.Format(
                            "<span title='{1}'>{0}</span>",
                            acc.Lookup("Id").TryGetValue<Int64>(),
                            acc.Id
                        ),
                        acc.Lookup("ScreenName").TryGetValue<String>(),
                        acc.Lookup("Name").TryGetValue<String>(),
                        act.LastTimestamp.If(t => t.HasValue, t => t.Value.ToLocalTime().ToString("yy/MM/dd HH:mm:ss"), t => ""),
                        String.Format(
                            "<img src='/!/obj/activities?id={0}/!/.bin' title='{4}' />",
                            act["Image"].First().Id
                        )
                    )))
                )
                .ToArray();
        }

        [FlowInterface("/.icons.table.xml")]
        public String OutputIconListTableXml(IEnumerable<StorageObject> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return this.OutputIconListTable(input, session, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractSerializer>();
        }

        [FlowInterface("/.icons.table.json")]
        public String OutputIconListTableJson(IEnumerable<StorageObject> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return this.OutputIconListTable(input, session, param, args)
                .XmlObjectSerializeToString<IList<IList<String>>, DataContractJsonSerializer>();
        }

        private XElement OutputStatus(Activity activity, Account subject, Boolean includesUser)
        {
            return new XElement("status",
                new XAttribute("metatweet-activity-id", activity.Id),
                new XElement("created_at", activity.LastTimestamp.HasValue
                    ? activity.LastTimestamp.Value.ToString("ddd MMM dd HH:mm:ss +0000 yyyy", CultureInfo.InvariantCulture)
                    : ""
                ),
                new XElement("id", activity.GetValue<Int64>()),
                new XElement("text", activity["Body"].SingleOrDefault().TryGetValue<String>()),
                new XElement("source", "<a href=\"zapped\" rel=\"nofollow\">" + activity["Source"].SingleOrDefault().TryGetValue<String>() + "</a>"),
                new XElement("truncated", "false"),
                activity["Reply"]
                    .FirstOrDefault()
                    .TryGetValue<Activity>()
                    .Let(r => Make.Array(
                        new XElement("in_reply_to_status_id", r.TryGetValue<Int64>()),
                        new XElement("in_reply_to_user_id",  r.Null(_ => _.Account["Id"].SingleOrDefault().GetValue<Int64>())),
                        new XElement("in_reply_to_screen_name", r.Null(_ => _.Account.Lookup("ScreenName").TryGetValue<String>())
                    ))),
                new XElement("favorited", (subject["Favorite", activity.Id] != null).ToString().ToLower()),
                includesUser ? Make.Array(this.OutputUser(activity.Account, subject, false)) : null
            );
        }

        private XElement OutputUser(Account account, Account subject, Boolean includesStatus)
        {
            return new XElement("user",
                new XAttribute("metatweet-account-id", account.Id),
                new XElement("id", account.Lookup("Id").TryGetValue<Int64>()),
                new XElement("name", account.Lookup("Name").TryGetValue<String>()),
                new XElement("screen_name", account.Lookup("ScreenName").TryGetValue<String>()),
                new XElement("description", account.Lookup("Description").TryGetValue<String>()),
                new XElement("location", account.Lookup("Location").TryGetValue<String>()),
                new XElement("profile_image_url", account.Lookup("ProfileImage").TryGetValue<String>()),
                new XElement("url", account.Lookup("Uri").TryGetValue<String>()),
                new XElement("followers_count", account.Lookup("FollowersCount").TryGetValue<Int32>()),
                new XElement("friends_count", account.Lookup("FollowingCount").TryGetValue<Int32>()),
                new XElement("created_at", account.Lookup("CreatedAt").TryGetValue<DateTime>().If(a => a != default(DateTime),
                    a => a.ToString("ddd MMM dd HH:mm:ss +0000 yyyy", CultureInfo.InvariantCulture),
                    a => ""
                )),
                new XElement("favourites_count", account.Lookup("FavoritesCount").TryGetValue<Int32>()),
                new XElement("statuses_count", account.Lookup("StatusesCount").TryGetValue<Int32>()),
                new XElement("following", (subject["Follow", account.Id] != null).ToString().ToLower()),
                includesStatus && account["Status"] != null ? Make.Array(this.OutputStatus(account.Lookup("Status"), subject, false)) : null
            );
        }

        private Account GetAccount(StorageSession session, String screenName)
        {
            return session.Query(StorageObjectDynamicQuery.Activity(
                new ActivityTuple()
                {
                    Name = "ScreenName",
                    Value = screenName,
                }
            ))
                .OrderByDescending(a => a)
                .FirstOrDefault()
                .Account;
        }
    }
}
