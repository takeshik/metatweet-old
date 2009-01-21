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

namespace XSpect.MetaTweet
{
    public class TwitterApiInput
        : InputFlowModule
    {
        public const String TwitterHost = "https://twitter.com";

        private HttpClient _client = new HttpClient("MetaTweet TwitterApiClient/1.0");

        private Func<Stream, XmlDocument> _generateXml = s =>
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(s);
            return xdoc;
        };

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
            foreach (XmlElement xstatus in xresponse.SelectNodes("//status"))
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
            throw new NotImplementedException();
        }

        // id : int | string
        // count : int
        // since : datetime
        // since_id : int
        // page : int
        [FlowInterface("/statuses/user_timeline")]
        public IEnumerable<StorageObject> FetchUserTimeline(String param, StorageModule storage, IDictionary<String, String> args)
        {
            throw new NotImplementedException();
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
            String idString = xuser.SelectSingleNode("id/text()").Value;
            Int32 id = Int32.Parse(idString);
            String name = xuser.SelectSingleNode("name/nam()").Value;
            String screenName = xuser.SelectSingleNode("screen_name/text()").Value;
            String location = xuser.SelectSingleNode("location/text()").Value;
            String description = xuser.SelectSingleNode("description/text()").Value;
            Uri profileImageUri = new Uri(xuser.SelectSingleNode("profile_image_url/text()").Value);
            Uri uri = new Uri(xuser.SelectSingleNode("url/text()").Value);
            Boolean isProtected = Boolean.Parse(xuser.SelectSingleNode("protected/text()").Value);
            Int32 followersCount = Int32.Parse(xuser.SelectSingleNode("followers_count/text()").Value);

            Activity userIdActivity = storage
                .GetActivities()
                .SingleOrDefault(a => a.Category == "Id" && a.Value == idString);
            
            Account account;
            if (userIdActivity == null)
            {
                account = storage.NewAccount();
                account.AccountId = Guid.NewGuid();
                account.Realm = this.Realm;
                account.Update();
            }
            else
            {
                account = userIdActivity.Account;
            }
            
            Activity activity;

            if ((activity = account.GetActivityOf("name")) == null
                ? (activity = account.NewActivity()) == null /* false */
                : activity.Value != name
            )
            {
                activity = account.NewActivity();
                // TODO: test whether timestamp is status/created_at or DateTime.Now (responsed at).
                activity.Timestamp = timestamp;
                activity.Category = "name";
                activity.Value = name;
                activity.Update();
            }

            if ((activity = account.GetActivityOf("ScreenName")) == null
                ? (activity = account.NewActivity()) == null /* false */
                : activity.Value != name
            )
            {
                activity = account.NewActivity();
                // TODO: test whether timestamp is status/created_at or DateTime.Now (responsed at).
                activity.Timestamp = timestamp;
                activity.Category = "ScreenName";
                activity.Value = screenName;
                activity.Update();
            }

            if ((activity = account.GetActivityOf("Location")) == null
                ? (activity = account.NewActivity()) == null /* false */
                : activity.Value != name
            )
            {
                activity = account.NewActivity();
                // TODO: test whether timestamp is status/created_at or DateTime.Now (responsed at).
                activity.Timestamp = timestamp;
                activity.Category = "Location";
                activity.Value = location;
                activity.Update();
            }

            if ((activity = account.GetActivityOf("Description")) == null
                ? (activity = account.NewActivity()) == null /* false */
                : activity.Value != name
            )
            {
                activity = account.NewActivity();
                // TODO: test whether timestamp is status/created_at or DateTime.Now (responsed at).
                activity.Timestamp = timestamp;
                activity.Category = "Description";
                activity.Value = description;
                activity.Update();
            }

            if ((activity = account.GetActivityOf("ProfileImage")) == null
                ? (activity = account.NewActivity()) == null /* false */
                : activity.Value != name
            )
            {
                activity = account.NewActivity();
                // TODO: test whether timestamp is status/created_at or DateTime.Now (responsed at).
                activity.Timestamp = timestamp;
                activity.Category = "ProfileImage";
                activity.Value = profileImageUri.ToString();
                // TODO: Fetching ImageUri / Thumbnail (or original) activity?
                activity.Update();
            }

            if ((activity = account.GetActivityOf("Uri")) == null
                ? (activity = account.NewActivity()) == null /* false */
                : activity.Value != name
            )
            {
                activity = account.NewActivity();
                // TODO: test whether timestamp is status/created_at or DateTime.Now (responsed at).
                activity.Timestamp = timestamp;
                activity.Category = "Uri";
                activity.Value = uri.ToString();
                activity.Update();
            }

            if ((activity = account.GetActivityOf("IsResticted")) == null
                ? (activity = account.NewActivity()) == null /* false */
                : activity.Value != name
            )
            {
                activity = account.NewActivity();
                // TODO: test whether timestamp is status/created_at or DateTime.Now (responsed at).
                activity.Timestamp = timestamp;
                activity.Category = "IsResticted";
                activity.Value = isProtected.ToString();
                activity.Update();
            }

            if ((activity = account.GetActivityOf("FollowersCount")) == null
                ? (activity = account.NewActivity()) == null /* false */
                : activity.Value != name
            )
            {
                activity = account.NewActivity();
                // TODO: test whether timestamp is status/created_at or DateTime.Now (responsed at).
                activity.Timestamp = timestamp;
                activity.Category = "FollowersCount";
                activity.Value = followersCount.ToString();
                activity.Update();
            }
            return account;
        }

        public Post AnalyzeStatus(XmlElement xstatus, StorageModule storage)
        {
            DateTime createdAt = DateTime.Parse(xstatus.SelectSingleNode("created_at/text()").Value);
            Int32 id = Int32.Parse(xstatus.SelectSingleNode("id/text()").Value);
            String text = xstatus.SelectSingleNode("text/text()").Value;
            String sourceHtml = xstatus.SelectSingleNode("source/text()").Value;
            String source;
            Int32 tempIndex;
            if (sourceHtml.Contains("href"))
            {
                source = sourceHtml.Substring((tempIndex = sourceHtml.LastIndexOf('>')) + 1, sourceHtml.LastIndexOf('<') - tempIndex);
            }
            else
            {
                source = sourceHtml;
            }
            Boolean isTruncated = Boolean.Parse(xstatus.SelectSingleNode("truncated/text()").Value);
            Nullable<Int32> inReplyToStatusId = xstatus.SelectSingleNode("in_reply_to_status_id/text()").Value == String.Empty
                ? Int32.Parse(xstatus.SelectSingleNode("in_reply_to_status_id/text()").Value)
                : default(Nullable<Int32>);
            Nullable<Int32> inReplyToUserId = xstatus.SelectSingleNode("in_reply_to_user_id/text()").Value == String.Empty
                ? Int32.Parse(xstatus.SelectSingleNode("in_reply_to_user_id/text()").Value)
                : default(Nullable<Int32>);
            String inReplyToUserScreenName = xstatus.SelectSingleNode("in_reply_to_screen_name/text()").Value;
            Boolean isFavorited = Boolean.Parse(xstatus.SelectSingleNode("favorited/text()").Value);

            Account account = this.AnalyzeUser(xstatus.SelectSingleNode("user") as XmlElement, createdAt, storage);
            Post post = null;
            if ((post = storage.GetPosts(r => r.PostId == id.ToString()).SingleOrDefault()) != null)
            {
                Activity activity = account.NewActivity();
                activity.Timestamp = createdAt;
                activity.Category = "Post";
                activity.Value = id.ToString();
                activity.Update();
                post = activity.NewPost();
            }
            post.Text = text;
            post.Source = source;
            post.IsFavorited = isFavorited;
            post.IsRestricted = Boolean.Parse(account["IsResticted"]);
            if (inReplyToStatusId.HasValue)
            {
                post.AddReply(storage.GetPosts(r => r.PostId == inReplyToStatusId.Value.ToString()).Single());
            }
            return post;
        }
    }
}