// -*- mode: csharp; encoding: utf-8; -*-
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * TwitterApiFlow
 *   MetaTweet Input/Output modules which provides Twitter access with API
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
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
using XSpect.MetaTweet.ObjectModel;
using XSpect.MetaTweet.Modules;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Net;

namespace XSpect.MetaTweet
{
    public class TwitterApiOutput
        : OutputFlowModule
    {
        private NetworkCredential _credential;

        public override void Initialize()
        {
            this.Realm = this.Configuration.GetValueOrDefault("realm", "com.twitter");
            this._credential
                = this.Configuration.GetValueOrDefault<NetworkCredential>("credential");
            base.Initialize();
        }

        [FlowInterface("/.xml")]
        public XDocument OutputXml(IEnumerable<StorageObject> source, StorageModule storage, String param, IDictionary<String, String> args)
        {
            if (source.All(o => o is Account))
            {
                return new XDocument(
                    new XDeclaration("1.0", "utf-8", "no"),
                    new XElement("users",
                        new XAttribute("type", "array"),
                        source.OfType<Account>().Select(a => this.OutputAccount(a, true, DateTime.Now))
                    )
                );
            }
            else if (source.All(o => o is Post))
            {
                return new XDocument(
                    new XDeclaration("1.0", "utf-8", "no"),
                    new XElement("statuses",
                        new XAttribute("type", "array"),
                        source.OfType<Post>().Select(p => this.OutputPost(p, true))
                    )
                );
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        [FlowInterface("/.xml")]
        public String OutputXmlString(IEnumerable<StorageObject> source, StorageModule storage, String param, IDictionary<String, String> args)
        {
            // HACK: Add XML declaration
            return "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"no\"?>\n"
                + this.OutputXml(source, storage, param, args).ToString();
        }

        protected virtual XElement OutputAccount(Account account, Boolean includesPost, DateTime baseline)
        {
            XElement xuser = new XElement("user",
                new XElement("id", account["Id", baseline]),
                new XElement("name", account["Name", baseline]),
                new XElement("screen_name", account["ScreenName", baseline]),
                new XElement("location", account["Location", baseline]),
                new XElement("description", account["Description", baseline]),
                new XElement("profile_image_url", account["ProfileImage", baseline]),
                new XElement("url", account["Uri", baseline]),
                new XElement("protected", account["IsRestricted", baseline]),
                new XElement("followers_count", account["FollowersCount", baseline])
            );
            if (includesPost)
            {
                xuser.Add(new XElement("status", this.OutputPost(account.GetActivityOf("Post", baseline).GetPost(), false)));
            }
            return xuser;
        }

        protected virtual XElement OutputPost(Post post, Boolean includesAccount)
        {
            XElement xstatus = new XElement("status",
                new XElement("created_at", post.Activity.Timestamp.ToString("r")),
                new XElement("id", post.PostId),
                new XElement("text", post.Text),
                new XElement("source", post.Source),
                new XElement("truncated", post.Text.Length > 140),
                new XElement("in_reply_to_status_id", post.GetReplying() != null
                    ? null
                    : post.Replying.First().PostId
                ),
                new XElement("in_reply_to_user_id", post.Replying != null
                    ? null
                    : post.Replying.First().Activity.GetAccount()["Id"]
                ),
                new XElement("in_reply_to_screen_name", post.Replying != null
                    ? null
                    : post.Replying.First().Activity.GetAccount()["Id"]
                ),
                new XElement("favorited", post.Activity.GetFavorers().Any(a => a["Id"] == this._credential.UserName))
            );
            if (includesAccount)
            {
                xstatus.Add(new XElement("status", this.OutputAccount(post.Activity.GetAccount(), false, post.Activity.Timestamp)));
            }
            return xstatus;
        }

        [FlowInterface("/.rss")]
        public XmlDocument OutputRss(IEnumerable<StorageObject> source, StorageModule storage, String param, IDictionary<String, String> args)
        {
            throw new NotImplementedException();
        }

        [FlowInterface("/.atom")]
        public XmlDocument OutputAtom(IEnumerable<StorageObject> source, StorageModule storage, String param, IDictionary<String, String> args)
        {
            throw new NotImplementedException();
        }

        [FlowInterface("/.json")]
        public String OutputJson(IEnumerable<StorageObject> source, StorageModule storage, String param, IDictionary<String, String> args)
        {
            throw new NotImplementedException();
        }
    }
}