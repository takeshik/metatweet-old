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
using Achiral;
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
        public String OutputTwitterXmlFormat(IEnumerable<StorageObject> source, StorageModule storage, String param, IDictionary<String, String> args)
        {
            this.CheckMyself(storage);
            // TODO: Support <users> output: check source elements' type?
            return new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("statuses",
                    new XAttribute("type", "array"),
                    new XAttribute("metatweet-version", ThisAssembly.EntireCommitId),
                    source
                        .OfType<Activity>()
                        .Where(a => a.Category == "Post")
                        .Select(a =>
                            new XElement("status",
                                new XElement("created_at", a.Timestamp
                                    .ToString("ddd MMM dd HH:mm:ss +0000 yyyy", CultureInfo.InvariantCulture)
                                ),
                                new XElement("id", a.SubId),
                                new XElement("text", a.Value),
                                new XElement("source", "<a href=\"zapped\"> rel=\"nofollow\">" + a.UserAgent + "</a>"),
                                new XElement("truncated", "false"),
                                a.ReferrersOf("Mention")
                                    // TODO: First?
                                    .FirstOrDefault()
                                    .Do(m => Make.Array(
                                        new XElement("in_reply_to_status_id", m.Null(_ => _.SubId)),
                                        new XElement("in_reply_to_user_id", m.Null(_ => _.Account["Id"].Value)),
                                        new XElement("in_reply_to_screen_name", m.Null(_ => _.Account["ScreenName"].Value))
                                    )),
                                new XElement("favorited", a.IsMarked("Favorite", this.Myself).ToString().ToLower()),
                                new XElement("user",
                                    new XElement("id", a.Account["Id"].Value),
                                    new XElement("name", a.Account["Name"].Value),
                                    new XElement("screen_name", a.Account["ScreenName"].Value),
                                    new XElement("location", a.Account["Location"].Value),
                                    new XElement("profile_image_url", a.Account["ProfileImage"].Value),
                                    new XElement("url", a.Account["Uri"].Value),
                                    new XElement("followers_count", a.Account["FollowersCount"].Value),
                                    new XElement("friends_count", a.Account["FollowingCount"].Value),
                                    new XElement("created_at", DateTime.Parse(a.Account["CreatedAt"].Value)
                                        .ToString("ddd MMM dd HH:mm:ss +0000 yyyy", CultureInfo.InvariantCulture)
                                    ),
                                    new XElement("favourites_count", a.Account["FavoritesCount"].Value),
                                    new XElement("statuses_count", a.Account["StatusesCount"].Value),
                                    new XElement("following", a.Account.IsRelated("Follow", this.Myself))
                                )
                            )
                        )
                )
            ).ToString();
        }

        private void CheckMyself(StorageModule storage)
        {
            if (this.Myself == null)
            {
                this.Myself = storage.GetActivities(
                    null,
                    null,
                    "ScreenName",
                    null,
                    null,
                    this.Configuration.ResolveValue<String>("username"),
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