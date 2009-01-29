// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * TwitterApiFlow
 *   MetaTweet Input/Output modules which provides Twitter access with API
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@xspect.org>
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
using System.Linq;
using XSpect.MetaTweet.ObjectModel;
using XSpect.Net;
using System.Xml;
using System.IO;
using System.Globalization;

namespace XSpect.MetaTweet
{
    public class TwitterApiInput
        : InputFlowModule
    {
        public const String TwitterHost = "https://twitter.com";

        private HttpClient _client;

        private Func<Stream, XmlDocument> _generateXml;

        public TwitterApiInput()
        {
            this._client = new HttpClient("MetaTweet TwitterApiClient/1.0");
            this._generateXml = s =>
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(s);
                return xdoc;
            };
        }

        public override void Initialize(IDictionary<String, String> args)
        {
            this._client.Credential.UserName = args.ContainsKey("username") ? args["username"] : String.Empty;
            this._client.Credential.Password = args.ContainsKey("password") ? args["password"] : String.Empty;
        }

        // since_id : int
        [FlowInterface("/statuses/public_timeline")]
        public IEnumerable<StorageObject> FetchPublicTimeline(String param, StorageModule storage, IDictionary<String, String> args)
        {
            XmlDocument xresponse = this._client.Post(
                new Uri(TwitterHost + "/statuses/public_timeline.xml" + args.ToUriQuery()),
                new Byte[0],
                this._generateXml
            );
            foreach (XmlElement xstatus in xresponse.SelectNodes("//status").Cast<XmlElement>().Reverse())
            {
                yield return this.AnalyzeStatus(xstatus, storage);
            }
        }

        // id : int | string
        // since : datetime
        // count : int
        // page : int
        [FlowInterface("/statuses/friends_timeline")]
        public IEnumerable<StorageObject> FetchFriendsTimeline(String param, StorageModule storage, IDictionary<String, String> args)
        {
            XmlDocument xresponse = this._client.Post(
                new Uri(TwitterHost + "/statuses/friends_timeline.xml" + args.ToUriQuery()),
                new Byte[0],
                this._generateXml
            );
            foreach (XmlElement xstatus in xresponse.SelectNodes("//status").Cast<XmlElement>().Reverse())
            {
                yield return this.AnalyzeStatus(xstatus, storage);
            }
        }

        // id : int | string
        // count : int
        // since : datetime
        // since_id : int
        // page : int
        [FlowInterface("/statuses/user_timeline")]
        public IEnumerable<StorageObject> FetchUserTimeline(String param, StorageModule storage, IDictionary<String, String> args)
        {
            XmlDocument xresponse = this._client.Post(
                new Uri(TwitterHost + "/statuses/user_timeline.xml" + args.ToUriQuery()),
                new Byte[0],
                this._generateXml
            );
            foreach (XmlElement xstatus in xresponse.SelectNodes("//status").Cast<XmlElement>().Reverse())
            {
                yield return this.AnalyzeStatus(xstatus, storage);
            }
        }

        // id : int (mandatory)
        [FlowInterface("/statuses/show/")]
        public IEnumerable<StorageObject> FetchStatus(String param, StorageModule storage, IDictionary<String, String> args)
        {
            throw new NotImplementedException();
        }

        // status : string (mandatory)
        // in_reply_to_status_id : int
        // source : string
        [FlowInterface("/statuses/update")]
        public IEnumerable<StorageObject> UpdateStatus(String param, StorageModule storage, IDictionary<String, String> args)
        {
            throw new NotImplementedException();
        }

        // page : int
        // since : datetime
        // since_id : int
        [FlowInterface("/statuses/replies")]
        public IEnumerable<StorageObject> FetchReplies(String param, StorageModule storage, IDictionary<String, String> args)
        {
            throw new NotImplementedException();
        }

        // (last-segment) : int (mandatory)
        [FlowInterface("/statuses/destroy/")]
        public IEnumerable<StorageObject> DestroyStatus(String param, StorageModule storage, IDictionary<String, String> args)
        {
            throw new NotImplementedException();
        }

        public XmlDocument InvokeRest(Uri uri, String invokeMethod)
        {
            if (invokeMethod == "GET")
            {
                return this._client.Get(uri, s =>
                {
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.Load(s);
                    return xdoc;
                });
            }
            else if (invokeMethod == "POST")
            {
                return this._client.Post(uri, new Byte[0], s =>
                {
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.Load(s);
                    return xdoc;
                });
            }
            else
            {
                throw new ArgumentException("args");
            }
        }

        public Account AnalyzeUser(XmlElement xuser, DateTime timestamp, StorageModule storage)
        {
            String idString = xuser.SelectSingleNode("id").InnerText;
            Int32 id = Int32.Parse(idString);
            String name = xuser.SelectSingleNode("name").InnerText;
            String screenName = xuser.SelectSingleNode("screen_name").InnerText;
            String location = xuser.SelectSingleNode("location").InnerText;
            String description = xuser.SelectSingleNode("description").InnerText;
            Uri profileImageUri = new Uri(xuser.SelectSingleNode("profile_image_url").InnerText);
            Uri uri = xuser.SelectSingleNode("url").InnerText == String.Empty ? null : new Uri(xuser.SelectSingleNode("url").InnerText);
            Boolean isProtected = Boolean.Parse(xuser.SelectSingleNode("protected").InnerText);
            Int32 followersCount = Int32.Parse(xuser.SelectSingleNode("followers_count").InnerText);

            Activity userIdActivity = storage
                .GetActivities()
                .SingleOrDefault(a => a.Category == "Id" && a.Value == idString);
            
            Account account;
            if (userIdActivity == null)
            {
                account = storage.NewAccount(Guid.NewGuid(), this.Realm);
            }
            else
            {
                account = userIdActivity.Account;
            }
            
            Activity activity;

            // TODO: test whether timestamp is status/created_at or DateTime.Now (responsed at).

            if ((activity = account.GetActivityOf("Id", timestamp)) == null
                ? (activity = account.NewActivity(timestamp, "Id")) != null /* true */
                : activity.Value != id.ToString()
            )
            {
                activity.Value = id.ToString();
            }

            if ((activity = account.GetActivityOf("Name", timestamp)) == null
                ? (activity = account.NewActivity(timestamp, "Name")) != null /* true */
                : activity.Value != name
            )
            {
                activity.Value = name;
            }

            if ((activity = account.GetActivityOf("ScreenName", timestamp)) == null
                ? (activity = account.NewActivity(timestamp, "ScreenName")) != null /* true */
                : activity.Value != screenName
            )
            {
                activity.Value = screenName;
            }

            if ((activity = account.GetActivityOf("Location", timestamp)) == null
                ? (activity = account.NewActivity(timestamp, "Location")) != null /* true */
                : activity.Value != location
            )
            {
                activity.Value = location;
            }

            if ((activity = account.GetActivityOf("Description", timestamp)) == null
                ? (activity = account.NewActivity(timestamp, "Description")) != null /* true */
                : activity.Value != description
            )
            {
                activity.Value = description;
            }

            if ((activity = account.GetActivityOf("ProfileImage", timestamp)) == null
                ? (activity = account.NewActivity(timestamp, "ProfileImage")) != null /* true */
                : activity.Value != profileImageUri.ToString()
            )
            {
                activity.Value = profileImageUri.ToString();
                // TODO: Fetching ImageUri / Thumbnail (or original) activity?
            }

            if (uri != null)
            {
                if ((activity = account.GetActivityOf("Uri", timestamp)) == null
                    ? (activity = account.NewActivity(timestamp, "Uri")) != null /* true */
                    : activity.Value != uri.ToString()
                )
                {
                    activity.Value = uri.ToString();
                }
            }

            if ((activity = account.GetActivityOf("IsRestricted", timestamp)) == null
                ? (activity = account.NewActivity(timestamp, "IsRestricted")) != null /* true */
                : activity.Value != isProtected.ToString()
            )
            {
                activity.Value = isProtected.ToString();
            }

            if ((activity = account.GetActivityOf("FollowersCount", timestamp)) == null
                ? (activity = account.NewActivity(timestamp, "FollowersCount")) != null /* true */
                : activity.Value != followersCount.ToString()
            )
            {
                activity.Value = followersCount.ToString();
            }
            return account;
        }

        public Post AnalyzeStatus(XmlElement xstatus, StorageModule storage)
        {
            DateTime createdAt = DateTime.ParseExact(
                xstatus.SelectSingleNode("created_at").InnerText,
                "ddd MMM dd HH:mm:ss zzzz yyyy",
                CultureInfo.GetCultureInfo("en-US").DateTimeFormat,
                DateTimeStyles.AssumeUniversal
            );
            Int32 id = Int32.Parse(xstatus.SelectSingleNode("id").InnerText);
            String text = xstatus.SelectSingleNode("text").InnerText;
            String sourceHtml = xstatus.SelectSingleNode("source").InnerText;
            String source;
            Int32 tempIndex;
            if (sourceHtml.Contains("href"))
            {
                source = sourceHtml.Substring((tempIndex = sourceHtml.LastIndexOf('>', sourceHtml.Length - 2) + 1), sourceHtml.LastIndexOf('<') - tempIndex);
            }
            else
            {
                source = sourceHtml;
            }
            Boolean isTruncated = Boolean.Parse(xstatus.SelectSingleNode("truncated").InnerText);
            Nullable<Int32> inReplyToStatusId = xstatus.SelectSingleNode("in_reply_to_status_id").InnerText != String.Empty
                ? Int32.Parse(xstatus.SelectSingleNode("in_reply_to_status_id").InnerText)
                : default(Nullable<Int32>);
            Nullable<Int32> inReplyToUserId = xstatus.SelectSingleNode("in_reply_to_user_id").InnerText != String.Empty
                ? Int32.Parse(xstatus.SelectSingleNode("in_reply_to_user_id").InnerText)
                : default(Nullable<Int32>);
            Boolean isFavorited = Boolean.Parse(xstatus.SelectSingleNode("favorited").InnerText);

            Account account = this.AnalyzeUser(xstatus.SelectSingleNode("user") as XmlElement, createdAt, storage);
            Activity activity;
            if ((activity = storage.GetActivities(
                r => r.AccountId == account.AccountId
                  && r.Category == "Post"
                  && r.Value == id.ToString()
                ).SingleOrDefault()) == null
            )
            {
                activity = account.NewActivity(createdAt, "Post");
                activity.Value = id.ToString();
            }

            Post post = activity.ToPost();
            post.Text = text;
            post.Source = source;
            if (inReplyToStatusId.HasValue)
            {
                // TODO the reply is not exists in the DB
                Post inReplyToPost = storage.GetPosts(r => r.PostId == inReplyToStatusId.Value.ToString()).SingleOrDefault();
                if (inReplyToPost != null)
                {
                    //post.AddReplying(inReplyToPost);
                }
            }
            return post;
        }
    }
}