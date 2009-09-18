// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * TwitterWebFlow
 *   MetaTweet Input/Output modules which provides Twitter access with web scraping
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of TwitterWebFlow.
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
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;
using TidyNet;
using XSpect.Extension;
using XSpect.MetaTweet.Modules;
using XSpect.MetaTweet.ObjectModel;
using XSpect.Net;
using Achiral.Extension;

namespace XSpect.MetaTweet.Modules
{
    public class TwitterWebInput
        : InputFlowModule
    {
        private readonly HttpClient _client;

        private readonly Tidy _tidy;

        private readonly Func<HttpWebResponse, XDocument> _processor;

        private String _authenticityToken;

        public TwitterWebInput()
        {
            // HACK: For Twitter server (see: http://muumoo.jp/news/2009/01/11/0expectationfailed.html)
            // CONSIDER: Is this change permanent?
            ServicePointManager.Expect100Continue = false;

            this._client = new HttpClient("MetaTweet TwitterWebInput/1.0");

            // HACK: Suppress to receive non-(X)HTML response (Twitter now uses XmlHttpRequest & JSON in the web)
            this._client = new HttpClient("MetaTweet TwitterWebInput/1.0", request =>
                request.Accept = "text/plain,text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"
            );

            this._tidy = new Tidy();
            this._tidy.Options.CharEncoding = CharEncoding.UTF8;
            this._tidy.Options.DocType = DocType.Strict;
            this._tidy.Options.DropEmptyParas = true;
            this._tidy.Options.DropFontTags = true;
            this._tidy.Options.EncloseBlockText = true;
            this._tidy.Options.EncloseText = true;
            this._tidy.Options.FixBackslash = true;
            this._tidy.Options.FixComments = true;
            this._tidy.Options.LiteralAttribs = true;
            this._tidy.Options.LogicalEmphasis = true;
            this._tidy.Options.MakeClean = true;
            this._tidy.Options.NumEntities = true;
            this._tidy.Options.QuoteAmpersand = true;
            this._tidy.Options.WrapAsp = true;
            this._tidy.Options.WrapAttVals = true;
            this._tidy.Options.WrapJste = true;
            this._tidy.Options.WrapPhp = true;
            this._tidy.Options.WrapScriptlets = true;
            this._tidy.Options.WrapSection = true;
            this._tidy.Options.XmlOut = true;

            this._processor = response => response.GetResponseStream().Dispose(stream =>
                XDocument.Parse(Regex.Replace(
                    new MemoryStream().Dispose(s =>
                    {
                        this._tidy.Parse(stream, s, new TidyMessageCollection());
                        s.Seek(0, SeekOrigin.Begin);
                        return Encoding.UTF8.GetString(s.GetBuffer());
                    }),
                    " xmlns=\".+?\"|<script.*?</script>",
                    String.Empty,
                    RegexOptions.Singleline
                ).TrimEnd('\0').Replace(ReplaceTables.XhtmlEntities))
            );
        }

        public override void Initialize()
        {
            this.Realm = this.Configuration.GetValue<String>("realm");
            this.Login();
            base.Initialize();
        }

        private void Login()
        {
            NetworkCredential credential = this.Configuration.GetValue<NetworkCredential>("credential");
            this._authenticityToken = this._client.Get(new Uri("https://twitter.com/"), this._processor)
                .XPathEvaluate<String>(this.Configuration.GetValue<String>(
                    "scrapingKeys", "xpath-s:login.authenticityToken"
                ));
            this._client.Post(new Uri("https://twitter.com/sessions/"), Encoding.UTF8.GetBytes(String.Format(
                this.Configuration.GetValue<String>(
                    "scrapingKeys", "format:login.sessionPost"
                ),
                this._authenticityToken,
                credential.UserName,
                credential.Password
            )), this._processor);
        }

        [FlowInterface("/")]
        public IEnumerable<StorageObject> FetchFriendsTimeline(StorageModule storage, String param, IDictionary<String, String> args)
        {
            if (args.Contains("crawl", "true"))
            {
                return this.Crawl(this.FetchFriendsTimeline, storage, param, args);
            }
            DateTime now = DateTime.UtcNow;
            return this.AnalyzeHome(
                this._client.Get(new Uri("https://twitter.com/" + args.ToUriQuery()), this._processor),
                now,
                storage
            ).Select(xe => this.AnalyzeStatus(xe, now, storage)).Cast<StorageObject>().ToList();
        }

        private IEnumerable<XElement> AnalyzeHome(XDocument xpage, DateTime timestamp, StorageModule storage)
        {
            String id = xpage.XPathEvaluate<Double>(this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-n:home.id"
                )).ToString();
            String name = xpage.XPathEvaluate<String>(this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-s:home.name"
            ));
            String screenName = xpage.XPathEvaluate<String>(this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-s:home.screenName"
            ));
            Uri profileImage = new Uri(xpage.XPathEvaluate<String>(this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-s:home.imageUri"
            )));
            UInt32 followingCount = (UInt32) xpage.XPathEvaluate<Double>(this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-n:home.followingCount"
            ));
            UInt32 followerCount = (UInt32) xpage.XPathEvaluate<Double>(this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-n:home.followerCount"
            ));
            UInt32 updateCount = (UInt32) xpage.XPathEvaluate<Double>(this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-n:home.updateCount"
            ));

            Activity userIdActivity = storage
                .GetActivities(null, null, "Id", null, id, null)
                .SingleOrDefault();

            Account account = userIdActivity == null
                ? storage.NewAccount(Guid.NewGuid(), this.Realm)
                : userIdActivity.GetAccount();

            Activity activity;

            if ((activity = account.GetActivityOf("Name")) == null
                ? (activity = account.NewActivity(timestamp, "Name")) != null /* true */
                : activity.Value != name
            )
            {
                activity.Value = name;
            }
            if ((activity = account.GetActivityOf("ScreenName")) == null
               ? (activity = account.NewActivity(timestamp, "ScreenName")) != null /* true */
               : activity.Value != screenName
                )
            {
                activity.Value = screenName;
            }
            if ((activity = account.GetActivityOf("ProfileImage")) == null
              ? (activity = account.NewActivity(timestamp, "ProfileImage")) != null /* true */
              : activity.Value != profileImage.ToString()
               )
            {
                activity.Value = profileImage.ToString();
            }
            if ((activity = account.GetActivityOf("FollowingCount")) == null
              ? (activity = account.NewActivity(timestamp, "FollowingCount")) != null /* true */
              : activity.Value != followingCount.ToString()
               )
            {
                activity.Value = followingCount.ToString();
            }
            if ((activity = account.GetActivityOf("FollowerCount")) == null
              ? (activity = account.NewActivity(timestamp, "FollowerCount")) != null /* true */
              : activity.Value != followerCount.ToString()
               )
            {
                activity.Value = followerCount.ToString();
            }
            if ((activity = account.GetActivityOf("UpdateCount")) == null
              ? (activity = account.NewActivity(timestamp, "UpdateCount")) != null /* true */
              : activity.Value != updateCount.ToString()
               )
            {
                activity.Value = updateCount.ToString();
            }

            return xpage.XPathSelectElements(
                this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-e:statuses.status"
            ));
        }

        private IEnumerable<XElement> AnalyzeTimeline(XDocument xpage, DateTime timestamp, StorageModule storage)
        {
            String id = xpage.XPathEvaluate<Double>(this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-n:timeline.id"
            )).ToString();
            String name = xpage.XPathEvaluate<String>(this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-s:timeline.name"
            ));
            String screenName = xpage.XPathEvaluate<String>(this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-s:timeline.screenName"
            ));
            String location = xpage.XPathEvaluate<String>(this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-n:timeline.location"
            ));
            String description = xpage.XPathEvaluate<String>(this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-n:timeline.description"
            ));
            String uri = xpage.XPathEvaluate<String>(this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-n:timeline.uri"
            ));
            Uri profileImage = new Uri(screenName != this._client.Credential.UserName
                ? xpage.XPathEvaluate<String>(this.Configuration.GetValue<String>(
                      "scrapingKeys", "xpath-n:timeline.imageUri"
                  ))
                : xpage.XPathEvaluate<String>(this.Configuration.GetValue<String>(
                      "scrapingKeys", "xpath-n:timeline.imageUri_home"
                  ))
            );
            UInt32 followingCount = (UInt32) xpage.XPathEvaluate<Double>(this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-n:timeline.followingCount"
            ));
            UInt32 followerCount = (UInt32) xpage.XPathEvaluate<Double>(this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-n:timeline.followerCount"
            ));
            UInt32 updateCount = (UInt32) xpage.XPathEvaluate<Double>(this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-n:timeline.updateCount"
            ));

            Activity userIdActivity = storage
                .GetActivities(null, null, "Id", null, id, null)
                .SingleOrDefault();

            Account account = userIdActivity == null
                ? storage.NewAccount(Guid.NewGuid(), this.Realm)
                : userIdActivity.GetAccount();

            Activity activity;

            if ((activity = account.GetActivityOf("Name")) == null
                ? (activity = account.NewActivity(timestamp, "Name")) != null /* true */
                : activity.Value != name
            )
            {
                activity.Value = name;
            }
            if ((activity = account.GetActivityOf("ScreenName")) == null
               ? (activity = account.NewActivity(timestamp, "ScreenName")) != null /* true */
               : activity.Value != screenName
                )
            {
                activity.Value = screenName;
            }
            if ((activity = account.GetActivityOf("Location")) == null
               ? (activity = account.NewActivity(timestamp, "Location")) != null /* true */
               : activity.Value != location
                )
            {
                activity.Value = location;
            }
            if ((activity = account.GetActivityOf("Description")) == null
               ? (activity = account.NewActivity(timestamp, "Description")) != null /* true */
               : activity.Value != description
                )
            {
                activity.Value = description;
            }
            if ((activity = account.GetActivityOf("Uri")) == null
               ? (activity = account.NewActivity(timestamp, "Uri")) != null /* true */
               : activity.Value != uri
                )
            {
                activity.Value = uri;
            }
            if ((activity = account.GetActivityOf("ProfileImage")) == null
              ? (activity = account.NewActivity(timestamp, "ProfileImage")) != null /* true */
              : activity.Value != profileImage.ToString()
               )
            {
                activity.Value = profileImage.ToString();
            }
            if ((activity = account.GetActivityOf("FollowingCount")) == null
              ? (activity = account.NewActivity(timestamp, "FollowingCount")) != null /* true */
              : activity.Value != followingCount.ToString()
               )
            {
                activity.Value = followingCount.ToString();
            }
            if ((activity = account.GetActivityOf("FollowerCount")) == null
              ? (activity = account.NewActivity(timestamp, "FollowerCount")) != null /* true */
              : activity.Value != followerCount.ToString()
               )
            {
                activity.Value = followerCount.ToString();
            }
            if ((activity = account.GetActivityOf("UpdateCount")) == null
              ? (activity = account.NewActivity(timestamp, "UpdateCount")) != null /* true */
              : activity.Value != updateCount.ToString()
               )
            {
                activity.Value = updateCount.ToString();
            }
            if ((activity = account.GetActivityOf("Uri")) == null
              ? (activity = account.NewActivity(timestamp, "Uri")) != null /* true */
              : activity.Value != uri
               )
            {
                activity.Value = uri;
            }

            return xpage.XPathSelectElements(
                this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-e:statuses.status"
            ));
        }

        private IEnumerable<XElement> AnalyzeUserList(XDocument xpage, DateTime timestamp, StorageModule storage)
        {
            return xpage.XPathSelectElements(
                this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-e:users.user"
            ));
        }

        private Post AnalyzeStatus(XElement xstatus, DateTime timestamp, StorageModule storage)
        {
            // Int32 id = ?
            String name = xstatus.XPathEvaluate<String>(this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-s:statuses.status.name"
            ));
            String screenName = xstatus.XPathEvaluate<String>(this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-s:statuses.status.screenName"
            ));
            // String location = ?
            // String description = ?
            Uri profileImageUri = new Uri(xstatus.XPathEvaluate<String>(this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-s:statuses.status.imageUri"
            )));
            // String uri = ?
            Boolean isProtected = xstatus.XPathEvaluate<Boolean>(this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-b:statuses.status.protected"
            ));
            // Int32 followersCount = ?

            UInt64 statusId = (UInt64) xstatus.XPathEvaluate<Double>(this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-n:statuses.status.id"
            ));
            String text = xstatus.XPathEvaluate<String>(this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-s:statuses.status.body"
            ));
            String source = xstatus.XPathEvaluate<String>(this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-s:statuses.status.source"
            ));
            // Boolean isTruncated = ?
            String inReplyTo = xstatus.XPathEvaluate<String>(this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-s:statuses.status.inReplyTo"
            ));
            Nullable<UInt64> inReplyToStatusId = inReplyTo.IsNullOrEmpty()
                ? default(Nullable<UInt64>)
                : UInt64.Parse(this.Configuration.GetValue<String>(
                    "scrapingKeys", "regexp:statuses.status.inReplyTo.statusId"
                  ).RegexMatch(inReplyTo).Groups[1].Value);
            String inReplyToScreenName = inReplyTo.IsNullOrEmpty()
                ? null
                : this.Configuration.GetValue<String>(
                    "scrapingKeys", "regexp:statuses.status.inReplyTo.screenName"
                  ).RegexMatch(inReplyTo).Groups[1].Value;
            Boolean isFavorited = xstatus.XPathEvaluate<Boolean>(this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-b:statuses.status.favorited"
            ));

            Activity userActivity = storage
                .GetActivities(null, null, "ScreenName", null, screenName, null)
                .OrderByDescending(a => a.Timestamp)
                .ThenBy(a => a.Subindex)
                // TODO: Why not SingleOrDefault?
                .FirstOrDefault();

            Account account = userActivity == null
                ? storage.NewAccount(Guid.NewGuid(), this.Realm)
                : userActivity.GetAccount();

            Activity activity;

            // Id?

            if ((activity = account.GetActivityOf("Name")) == null
                ? (activity = account.NewActivity(timestamp, "Name")) != null /* true */
                : activity.Value != name
            )
            {
                activity.Value = name;
            }

            if ((activity = account.GetActivityOf("ScreenName")) == null
                ? (activity = account.NewActivity(timestamp, "ScreenName")) != null /* true */
                : activity.Value != screenName
            )
            {
                activity.Value = screenName;
            }

            // Location?

            // Description?

            if ((activity = account.GetActivityOf("ProfileImage")) == null
                ? (activity = account.NewActivity(timestamp, "ProfileImage")) != null /* true */
                : activity.Value != profileImageUri.ToString()
            )
            {
                activity.Value = profileImageUri.ToString();
                // TODO: Fetching ImageUri / Thumbnail (or original) activity?
            }

            // Uri?

            if ((activity = account.GetActivityOf("IsRestricted")) == null
                ? (activity = account.NewActivity(timestamp, "IsRestricted")) != null /* true */
                : activity.Value != isProtected.ToString()
            )
            {
                activity.Value = isProtected.ToString();
            }

            // FollowersCount?

            // Status analyzing:

            if ((activity = storage.GetActivities(
                r => r.AccountId == account.AccountId
                  && r.Category == "Post"
                  && r.Value == statusId.ToString()
                ).SingleOrDefault()) == null
            )
            {
                activity = account.NewActivity(timestamp, "Post");
                activity.Value = statusId.ToString();
            }

            Post post = activity.GetPost();
            post.Text = text;
            post.Source = source;
            if (inReplyToStatusId.HasValue)
            {/*
                // Load in-reply-to from the backend
                storage.LoadPostsDataTable(null, inReplyToStatusId.Value.ToString());
                
                // debug.
                var x = storage.GetPosts(r => r.PostId == inReplyToStatusId.Value.ToString());
                if (x.Count > 1)
                {
                    System.Diagnostics.Debugger.Break();
                }
                Post inReplyToPost = x.SingleOrDefault();
                if (inReplyToPost != null)
                {
                    post.AddReplying(inReplyToPost);
                }*/
            }
            return post;
        }

        private Account AnalyzeUser(XElement xstatus, DateTime timestamp, StorageModule storage)
        {
            String id = xstatus.XPathEvaluate<Double>(this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-n:users.user.id"
            )).ToString();
            String name = xstatus.XPathEvaluate<String>(this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-s:users.user.name"
            ));
            String screenName = xstatus.XPathEvaluate<String>(this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-s:users.user.screenName"
            ));
            Uri imageUri = new Uri(xstatus.XPathEvaluate<String>(this.Configuration.GetValue<String>(
                "scrapingKeys", "xpath-s:users.user.imageUri"
            )));

            Activity userActivity = storage
                .GetActivities(null, null, "ScreenName", null, screenName, null)
                .OrderByDescending(a => a.Timestamp)
                .ThenBy(a => a.Subindex)
                .SingleOrDefault();

            Account account = userActivity == null
                ? storage.NewAccount(Guid.NewGuid(), this.Realm)
                : userActivity.GetAccount();

            Activity activity;

            if ((activity = account.GetActivityOf("Id")) == null
                ? (activity = account.NewActivity(timestamp, "Id")) != null /* true */
                : activity.Value != id
            )
            {
                activity.Value = id;
            }
            if ((activity = account.GetActivityOf("Name")) == null
                ? (activity = account.NewActivity(timestamp, "Name")) != null /* true */
                : activity.Value != name
            )
            {
                activity.Value = name;
            }
            if ((activity = account.GetActivityOf("ScreenName")) == null
                ? (activity = account.NewActivity(timestamp, "ScreenName")) != null /* true */
                : activity.Value != screenName
            )
            {
                activity.Value = screenName;
            }
            if ((activity = account.GetActivityOf("ProfileImage")) == null
                ? (activity = account.NewActivity(timestamp, "ProfileImage")) != null /* true */
                : activity.Value != imageUri.ToString()
            )
            {
                activity.Value = imageUri.ToString();
            }
            return account;
        }

        // parameter distanceBase:
        //   The count of elements of the reminder of the page and the next page.
        //   ex) friends_timeline, the reminder of page=1 and page=2 is 20 (= distanceBase).
        public IEnumerable<StorageObject> Crawl(
            Func<StorageModule, String, IDictionary<String, String>, IEnumerable<StorageObject>> method,
            StorageModule storage,
            String param,
            IDictionary<String, String> args
        )
        {
            IEnumerable<StorageObject> result = new List<StorageObject>();
            IEnumerable<StorageObject> ret;
            if (args.ContainsKey("page"))
            {
                args.Add("page", "1");
            }
            do
            {
                args["page"] = (Int32.Parse(args["page"]) + 1).ToString();
                ret = method(storage, param, args);
                result = result.Concat(ret);
            } while (ret.Count() == 20);
            return result;
        }
    }
}