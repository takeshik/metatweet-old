// -*- mode: csharp; encoding: utf-8; -*-
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

namespace XSpect.MetaTweet
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
            this.Realm = this.Configuration.GetValueOrDefault("realm", "com.twitter");
            this.Login();
        }

        private void Login()
        {
            NetworkCredential credential = this.Configuration.GetValueOrDefault<NetworkCredential>("credential");
            this._authenticityToken = this._client.Get(new Uri("https://twitter.com/"), this._processor)
                .XPathEvaluate<String>(this.Configuration.GetChild("scrapingKeys").GetValueOrDefault(
                    "xpath:login.authenticityToken",
                    "string(//input[@id='authenticity_token']/@value)"
                ));
            this._client.Post(new Uri("https://twitter.com/sessions/"), Encoding.UTF8.GetBytes(String.Format(
                this.Configuration.GetChild("scrapingKeys").GetValueOrDefault(
                    "format:login.sessionPost",
                    "authenticity_token={0}&session[username_or_email]={1}&session[password]={2}&remember_me=1"
                ),
                this._authenticityToken,
                credential.UserName,
                credential.Password
            )), this._processor);
        }

        [FlowInterface("/home")]
        public IEnumerable<StorageObject> FetchPublicTimeline(StorageModule storage, String param, IDictionary<String, String> args)
        {
            DateTime now = DateTime.Now;
            return this._client.Get(new Uri("https://twitter.com/home" + args.ToUriQuery()), this._processor).XPathSelectElements(
                this.Configuration.GetChild("scrapingKeys").GetValueOrDefault(
                "xpath-e:statuses.status",
                "//ol[@id='timeline']/li"
            )).Select(xelement => this.AnalyzeStatus(xelement, now, storage)).Cast<StorageObject>();
        }

        private Post AnalyzeStatus(XElement xstatus, DateTime timestamp, StorageModule storage)
        {
            // Int32 id = ?
            String name = xstatus.XPathEvaluate<String>(this.Configuration.GetChild("scrapingKeys").GetValueOrDefault(
                "xpath-s:statuses.status.name",
                "string(.//a[@class='screen-name']/@title)"
            ));
            String screenName = xstatus.XPathEvaluate<String>(this.Configuration.GetChild("scrapingKeys").GetValueOrDefault(
                "xpath-s:statuses.status.screenName",
                "string(.//a[@class='screen-name'])"
            ));
            // String location = ?
            // String description = ?
            Uri profileImageUri = new Uri(xstatus.XPathEvaluate<String>(this.Configuration.GetChild("scrapingKeys").GetValueOrDefault(
                "xpath-s:statuses.status.imageUri",
                "string(.//img[contains(@class,'photo')]/@src)"
            )));
            // String uri = ?
            Boolean isProtected = xstatus.XPathEvaluate<Boolean>(this.Configuration.GetChild("scrapingKeys").GetValueOrDefault(
                "xpath-b:statuses.status.protected",
                "boolean(//img[@class='lock'])"
            ));
            // Int32 followersCount = ?

            Int32 statusId = Int32.Parse(xstatus.XPathEvaluate<String>(this.Configuration.GetChild("scrapingKeys").GetValueOrDefault(
                "xpath-s:statuses.status.id",
                "substring-after(string(@id),'status_')"
            )));
            String text = xstatus.XPathEvaluate<String>(this.Configuration.GetChild("scrapingKeys").GetValueOrDefault(
                "xpath-s:statuses.status.body",
                "string(.//span[@class='entry-content'])"
            ));
            String source = xstatus.XPathEvaluate<String>(this.Configuration.GetChild("scrapingKeys").GetValueOrDefault(
                "xpath-s:statuses.status.source",
                "string(.//span[count(./@*)=0]/a)"
            ));
            // Boolean isTruncated = ?
            String inReplyTo = xstatus.XPathEvaluate<String>(this.Configuration.GetChild("scrapingKeys").GetValueOrDefault(
                "xpath-s:statuses.status.inReplyTo",
                "string(.//a[starts-with(string(.), 'in')][contains(string(.), 'reply')]/@href)"
            ));
            Nullable<Int32> inReplyToStatusId = inReplyTo.IsNullOrEmpty()
                ? default(Nullable<Int32>)
                : Int32.Parse(this.Configuration.GetChild("scrapingKeys").GetValueOrDefault(
                    "regexp:statuses.status.inReplyTo.statusId",
                    "(\\d+$)"
                  ).RegexMatch(inReplyTo).Groups[1].Value);
            String inReplyToScreenName = inReplyTo.IsNullOrEmpty()
                ? null
                : this.Configuration.GetChild("scrapingKeys").GetValueOrDefault(
                    "regexp:statuses.status.inReplyTo.screenName",
                    "twitter.com/(.+)/status"
                  ).RegexMatch(inReplyTo).Groups[1].Value;
            Boolean isFavorited = xstatus.XPathEvaluate<Boolean>(this.Configuration.GetChild("scrapingKeys").GetValueOrDefault(
                "xpath-b:statuses.status.favorited",
                "not(boolean(//a[contains(@class,'non-fav')]))"
            ));

            Activity userActivity = storage
                .GetActivities()
                .Where(a => a.Category == "ScreenName" && a.Value == screenName)
                .OrderByDescending(a => a.Timestamp)
                .ThenBy(a => a.Subindex)
                .SingleOrDefault();

            Account account;
            if (userActivity == null)
            {
                account = storage.NewAccount(Guid.NewGuid(), this.Realm);
            }
            else
            {
                account = userActivity.GetAccount();
            }

            Activity activity;

            // TODO: test whether timestamp is status/created_at or DateTime.Now (responsed at).

            // User analyzing:

            // Id?

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

            // Location?

            // Description?

            if ((activity = account.GetActivityOf("ProfileImage", timestamp)) == null
                ? (activity = account.NewActivity(timestamp, "ProfileImage")) != null /* true */
                : activity.Value != profileImageUri.ToString()
            )
            {
                activity.Value = profileImageUri.ToString();
                // TODO: Fetching ImageUri / Thumbnail (or original) activity?
            }

            // Uri?

            if ((activity = account.GetActivityOf("IsRestricted", timestamp)) == null
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

            Post post = activity.ToPost();
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
    }
}