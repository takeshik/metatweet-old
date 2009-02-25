// -*- mode: csharp; encoding: utf-8; -*-
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
using System.Linq;
using XSpect.MetaTweet.ObjectModel;
using XSpect.Net;
using System.Xml;
using System.IO;
using System.Globalization;
using XSpect.MetaTweet.Modules;
using XSpect.Extension;
using XSpect.Configuration;
using System.Net;
using System.Xml.Linq;
using Achiral;

namespace XSpect.MetaTweet
{
    public class TwitterApiInput
        : InputFlowModule
    {
        public const String TwitterHost = "https://twitter.com";

        private HttpClient _client;

        private Func<Stream, XmlDocument> _generateXml;

        private DateTime _friendsTimelineSince = DateTime.MinValue;

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

        public override void Initialize()
        {
            this.Realm = this.Configuration.GetValueOrDefault("realm", "com.twitter");
            this._client.Credential
                = this.Configuration.GetValueOrDefault<NetworkCredential>("credential");
        }

        [FlowInterface("/statuses/public_timeline")]
        public IEnumerable<StorageObject> FetchPublicTimeline(Storage storage, String param, IDictionary<String, String> args)
        {
            if (args.Contains("crawl", "true"))
            {
                return this.Crawl(this.FetchPublicTimeline, storage, param, args, 20);
            }
            return this.PostRest(new Uri(TwitterHost + "/statuses/public_timeline.xml" + args.ToUriQuery()))
                .Descendants("status").Reverse().Select(xe => this.AnalyzeStatus(xe, storage)).Cast<StorageObject>().ToList();
        }

        [FlowInterface("/statuses/friends_timeline")]
        public IEnumerable<StorageObject> FetchFriendsTimeline(Storage storage, String param, IDictionary<String, String> args)
        {
            if (args.Contains("crawl", "true"))
            {
                return this.Crawl(this.FetchFriendsTimeline, storage, param, args, 20);
            }
            return this.PostRest(new Uri(TwitterHost + "/statuses/friends_timeline.xml" + args.ToUriQuery()))
                .Descendants("status").Reverse().Select(xe => this.AnalyzeStatus(xe, storage)).Cast<StorageObject>().ToList();
        }

        [FlowInterface("/statuses/user_timeline")]
        public IEnumerable<StorageObject> FetchUserTimeline(Storage storage, String param, IDictionary<String, String> args)
        {
            if (args.Contains("crawl", "true"))
            {
                return this.Crawl(this.FetchUserTimeline, storage, param, args, 20);
            }
            return this.PostRest(new Uri(TwitterHost + "/statuses/user_timeline.xml" + args.ToUriQuery()))
                .Descendants("status").Reverse().Select(xe => this.AnalyzeStatus(xe, storage)).Cast<StorageObject>().ToList();
        }

        [FlowInterface("/statuses/show/")]
        public IEnumerable<StorageObject> FetchStatus(Storage storage, String param, IDictionary<String, String> args)
        {
            return this.PostRest(new Uri(TwitterHost + "/statuses/show/" + param + ".xml" + args.ToUriQuery()))
                .Descendants("status").Reverse().Select(xe => this.AnalyzeStatus(xe, storage)).Cast<StorageObject>().ToList();
        }

        [FlowInterface("/statuses/update")]
        public IEnumerable<StorageObject> UpdateStatus(Storage storage, String param, IDictionary<String, String> args)
        {
            return this.PostRest(new Uri(TwitterHost + "/statuses/update.xml" + args.ToUriQuery()))
                .Descendants("status").Reverse().Select(xe => this.AnalyzeStatus(xe, storage)).Cast<StorageObject>().ToList();
        }

        [FlowInterface("/statuses/replies")]
        public IEnumerable<StorageObject> FetchReplies(Storage storage, String param, IDictionary<String, String> args)
        {
            if (args.Contains("crawl", "true"))
            {
                return this.Crawl(this.FetchReplies, storage, param, args, 20);
            }
            return this.PostRest(new Uri(TwitterHost + "/statuses/replies.xml" + param + ".xml" + args.ToUriQuery()))
                .Descendants("status").Reverse().Select(xe => this.AnalyzeStatus(xe, storage)).Cast<StorageObject>().ToList();
        }

        [FlowInterface("/statuses/destroy/")]
        public IEnumerable<StorageObject> DestroyStatus(Storage storage, String param, IDictionary<String, String> args)
        {
            // TODO: research the response
            // TODO: Consider not to analyze status
            return this.PostRest(new Uri(TwitterHost + "/statuses/destroy/" + param + ".xml" + args.ToUriQuery()))
                .Descendants("status").Reverse().Select(xe => this.AnalyzeStatus(xe, storage))
                .Select(p =>
                {
                    p.Delete();
                    storage.Update();
                    return p;
                }).Cast<StorageObject>().ToList();
        }

        [FlowInterface("/statuses/friends")]
        public IEnumerable<StorageObject> GetFollowing(Storage storage, String param, IDictionary<String, String> args)
        {
            if (args.Contains("crawl", "true"))
            {
                return this.Crawl(this.GetFollowing, storage, param, args, 100);
            }
            return this.PostRest(new Uri(TwitterHost + "/statuses/friends.xml" + param + ".xml" + args.ToUriQuery()))
                .Descendants("user")
                .Reverse()
                .Select(xe => this.AnalyzeUser(xe, DateTime.Now, storage))
                .Select(acc =>
                {
                    storage.GetAccounts()
                        .Single(a => a["ScreenName"] == this._client.Credential.UserName)
                        .AddFollowing(acc);
                    return acc;
                })
                .Cast<StorageObject>()
                .ToList();
        }

        [FlowInterface("/statuses/followers")]
        public IEnumerable<StorageObject> GetFollowers(Storage storage, String param, IDictionary<String, String> args)
        {
            if (args.Contains("crawl", "true"))
            {
                return this.Crawl(this.GetFollowers, storage, param, args, 100);
            }
            return this.PostRest(new Uri(TwitterHost + "/statuses/followers.xml" + param + ".xml" + args.ToUriQuery()))
                .Descendants("user")
                .Reverse()
                .Select(xe => this.AnalyzeUser(xe, DateTime.Now, storage))
                .Select(acc =>
                {
                    storage.GetAccounts()
                        .Single(a => a["ScreenName"] == this._client.Credential.UserName)
                        .AddFollower(acc);
                    return acc;
                })
                .Cast<StorageObject>()
                .ToList();
        }

        [FlowInterface("/users/show")]
        public IEnumerable<StorageObject> GetUser(Storage storage, String param, IDictionary<String, String> args)
        {
            return this.PostRest(new Uri(TwitterHost + "/users/show/" + param + ".xml" + args.ToUriQuery()))
                .Descendants("user").Reverse().Select(xe => this.AnalyzeUser(xe, DateTime.Now, storage)).Cast<StorageObject>().ToList();
        }

        // parameter distanceBase:
        //   The count of elements of the reminder of the page and the next page.
        //   ex) friends_timeline, the reminder of page=1 and page=2 is 20 (= distanceBase).
        public IEnumerable<StorageObject> Crawl(
            Func<Storage, String, IDictionary<String, String>, IEnumerable<StorageObject>> method,
            Storage storage,
            String param,
            IDictionary<String, String> args,
            Int32 distanceBase
        )
        {
            IEnumerable<StorageObject> result = new List<StorageObject>();
            IEnumerable<StorageObject> ret;
            Int32 distance = Int32.Parse(args.GetValueOrDefault("count", "20")) / distanceBase;
            if (args.ContainsKey("page"))
            {
                args.Add("page", "1");
            }
            do
            {
                args["page"] = (Int32.Parse(args["page"]) + distance).ToString();
                ret = method(storage, param, args);
                result = result.Concat(ret);
            } while (ret.Count() == 100);
            return result;
        }

        public XDocument PostRest(Uri uri)
        {
            return this._client.Post(uri, new Byte[0], s =>XDocument.Load(XmlReader.Create(s)));
        }

        public Account AnalyzeUser(XElement xuser, DateTime timestamp, Storage storage)
        {
            String idString = xuser.Element("id").Value;
            Int32 id = Int32.Parse(idString);
            String name = xuser.Element("name").Value;
            String screenName = xuser.Element("screen_name").Value;
            String location = xuser.Element("location").Value;
            String description = xuser.Element("description").Value;
            Uri profileImageUri = new Uri(xuser.Element("profile_image_url").Value);
            String uri = xuser.Element("url").Value;
            Boolean isProtected = Boolean.Parse(xuser.Element("protected").Value);
            Int32 followersCount = Int32.Parse(xuser.Element("followers_count").Value);

            Activity userIdActivity = storage
                .GetActivities()
                .SingleOrDefault(a => a.Category == "Id" && a.Value == idString);
            
            Account account;
            if (userIdActivity == null)
            {
                account = storage.NewAccount(Guid.NewGuid());
                account.Realm = this.Realm;
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
                    : activity.Value != uri
                )
                {
                    activity.Value = uri;
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
            storage.Update();
            return account;
        }

        public Post AnalyzeStatus(XElement xstatus, Storage storage)
        {
            DateTime createdAt = DateTime.ParseExact(
                xstatus.Element("created_at").Value,
                "ddd MMM dd HH:mm:ss zzzz yyyy",
                CultureInfo.GetCultureInfo("en-US").DateTimeFormat,
                DateTimeStyles.AssumeUniversal
            );
            Int32 id = Int32.Parse(xstatus.Element("id").Value);
            String text = xstatus.Element("text").Value;
            Int32 tempIndex;
            String sourceHtml = xstatus.Element("source").Value;
            String source = sourceHtml.Contains("href")
                ? sourceHtml.Substring((tempIndex = sourceHtml.LastIndexOf('>', sourceHtml.Length - 2) + 1), sourceHtml.LastIndexOf('<') - tempIndex)
                : sourceHtml;
            Boolean isTruncated = Boolean.Parse(xstatus.Element("truncated").Value);
            Nullable<Int32> inReplyToStatusId = xstatus.Element("in_reply_to_status_id").Value != String.Empty
                ? Int32.Parse(xstatus.Element("in_reply_to_status_id").Value)
                : default(Nullable<Int32>);
            Nullable<Int32> inReplyToUserId = xstatus.Element("in_reply_to_user_id").Value != String.Empty
                ? Int32.Parse(xstatus.Element("in_reply_to_user_id").Value)
                : default(Nullable<Int32>);
            Boolean isFavorited = Boolean.Parse(xstatus.Element("favorited").Value);

            Account account = this.AnalyzeUser(xstatus.Element("user"), createdAt, storage);
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
                // Load in-reply-to from the backend
                storage.LoadPostsDataTableBy(null, inReplyToStatusId.Value.ToString());
                Post inReplyToPost = storage.GetPosts(r => r.PostId == inReplyToStatusId.Value.ToString()).SingleOrDefault();
                if (inReplyToPost != null)
                {
                    post.AddReplying(inReplyToPost);
                }
            }
            storage.Update();
            return post;
        }
    }
}