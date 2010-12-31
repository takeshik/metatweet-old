﻿// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * TwitterUserStreamsServant
 *   MetaTweet Servant to fetch data by Twitter User Streams.
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of TwitterUserStreamsServant.
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
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Achiral;
using Achiral.Extension;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;
using Newtonsoft.Json.Linq;
using XSpect.Extension;
using XSpect.MetaTweet;
using XSpect.MetaTweet.Objects;

namespace XSpect.MetaTweet.Modules
{
    public class TwitterUserStreamsServant
        : ServantModule
    {
        public const String Realm = "com.twitter";

        private DesktopConsumer _consumer;

        private StreamReader _reader;

        private StorageModule _storage;

        private Account _self;

        private readonly Thread _thread;

        public String StorageName
        {
            get;
            set;
        }

        public Boolean FetchAllReplies
        {
            get;
            set;
        }

        public TwitterUserStreamsServant()
        {
            this._thread = new Thread(Read);
        }

        protected override void InitializeImpl()
        {
            this._consumer = new DesktopConsumer(new ServiceProviderDescription
            {
                AccessTokenEndpoint = new MessageReceivingEndpoint(
                    "https://twitter.com/oauth/access_token",
                    HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest
                ),
                ProtocolVersion = ProtocolVersion.V10a,
                RequestTokenEndpoint = new MessageReceivingEndpoint(
                    "https://twitter.com/oauth/request_token",
                    HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest
                ),
                UserAuthorizationEndpoint = new MessageReceivingEndpoint(
                    "https://twitter.com/oauth/authorize",
                    HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest
                ),
                TamperProtectionElements = new ITamperProtectionChannelBindingElement[]
                {
                    new HmacSha1SigningBindingElement()
                },
            }, new TokenManager(this.Host.Directories.RuntimeDirectory.File(this + "_token.dat")));
        }

        protected override void ConfigureImpl(FileInfo configFile)
        {
            base.ConfigureImpl(configFile);
            this.StorageName = this.Configuration.StorageName;
            this.FetchAllReplies = this.Configuration.FetchAllReplies;
        }

        protected override void StartImpl()
        {
            this._storage = this.Host.ModuleManager.GetModule<StorageModule>(this.StorageName);
            this._reader = this.Open(
                new MessageReceivingEndpoint("https://userstream.twitter.com/2/user.json"
                    + (this.FetchAllReplies ? "?replies=all" : ""),
                HttpDeliveryMethods.GetRequest)
            ).GetResponseReader();
            this._thread.Start();
        }

        protected override void StopImpl()
        {
            this._storage.EndWorkerScope();
            this._storage.Dispose();
            this._thread.Abort();
            this._reader.Dispose();
        }

        private IncomingWebResponse Open(MessageReceivingEndpoint endpoint)
        {
            if (((TokenManager) this._consumer.TokenManager).AccessToken == null)
            {
                String requestToken;
                String pin;
                Uri uri = this._consumer.RequestUserAuthorization(null, null, out requestToken);

                FileInfo uriFile = this.Host.Directories.RuntimeDirectory.File(this + "_auth.uri")
                    .Apply(f => f.WriteAllText(uri.AbsoluteUri));
                if (Environment.UserInteractive)
                {
                    Process.Start(uri.AbsoluteUri);
                    Console.Write(
@"{0}: Input OAuth authorization PIN, provided by Twitter, after
the service granted access to this module:
PIN> "
                    , this);
                    pin = Console.ReadLine();
                }
                else
                {
                    FileInfo inputFile = this.Host.Directories.RuntimeDirectory.File(this + "_pin.txt")
                        .Apply(f => f.Delete());
                    this.Log.Warn(
@"{0} is now being blocked to complete OAuth authorization. Open the directory:
    {1}
and then access the authorize page by your hand, the URI wrote in {2}
and create an new text file, named ""{3}"",
which only contains OAuth authorization PIN digits, provided by Twitter.",
                        this,
                        this.Host.Directories.RuntimeDirectory.FullName,
                        uriFile.Name,
                        inputFile.Name
                    );
                    pin = Observable.FromEvent<FileSystemEventArgs>(this.Host.Directories.RuntimeDirectoryWatcher, "Created")
                        .Select(e => e.EventArgs.Name)
                        .Where(n => n == inputFile.Name)
                        .First()
                        .Let(_ => inputFile.ReadAllLines().First().Trim());
                    inputFile.Delete();
                }
                uriFile.Delete();
                this._consumer.ProcessUserAuthorization(requestToken, pin);
            }

            return this._consumer.Channel.WebRequestHandler.GetResponse(
                this._consumer.PrepareAuthorizedRequest(endpoint, ((TokenManager) this._consumer.TokenManager).AccessToken)
            );
        }

        private void Read()
        {
            this._storage.Execute(_ =>
            {
                this._self = this._storage.GetActivities(StorageObjectQuery.Activity(new ActivityTuple()
                {
                    Category = "ScreenName",
                    Value = JObject.Parse(
                        this.Open(
                            new MessageReceivingEndpoint("https://api.twitter.com/1/account/verify_credentials.json", HttpDeliveryMethods.GetRequest)
                        ).GetResponseReader().Dispose(sr => sr.ReadToEnd())
                    ).Value<String>("screen_name"),
                }))
                    .Single()
                    .Account;
                Make.Repeat(this._reader)
                    .Select(r => r.ReadLine())
                    .Where(s => !String.IsNullOrWhiteSpace(s))
                    .Select(JObject.Parse)
                    .ForEach(j =>
                    {
                        if (j["in_reply_to_user_id"] != null)
                        {
                            AnalyzeStatus(j, this._storage);
                        }
                        else if (j["event"] != null)
                        {
                            switch (j["event"].Value<String>())
                            {
                                case "favorite":
                                    AnalyzeFavorite(j, this._storage);
                                    break;
                                case "retweet":
                                    AnalyzeRetweet(j, this._storage);
                                    break;
                                case "follow":
                                    AnalyzeFollow(j, this._storage);
                                    break;
                            }
                            this._storage.TryUpdate();
                        }
                        else if (j["friends"] != null)
                        {
                            AsyncAnalyzeFollowing(j, this._storage);
                        }
                    });
                Thread.Sleep(Timeout.Infinite);
            });
        }

        private Activity AnalyzeStatus(JObject jobj, StorageModule storage)
        {
            DateTime timestamp = ParseTimestamp(jobj.Value<String>("created_at"));
            Account account = this.AnalyzeUser(jobj.Value<JObject>("user"), storage, timestamp);
            Activity post = account.Act(
                timestamp,
                "Post",
                jobj.Value<String>("id"),
                jobj.Value<String>("source").If(s => s.Contains("</a>"), s =>
                    s.Remove(s.Length - 4 /* "</a>" */).Substring(s.IndexOf('>') + 1)
                ),
                jobj.Value<String>("text"),
                null
            );
            if (jobj.Value<Boolean>("favorited"))
            {
                this._self.Mark("Favorite", post);
            }
            // TODO: reply
            return post;
        }

        private Account AnalyzeUser(JObject jobj, StorageModule storage, DateTime timestamp)
        {
            Account account = storage.NewAccount(Realm, Create.Table("Id", jobj.Value<String>("id")));
            if (!account.Activities.Any(a => a.Category == "Id"))
            {
                account.Act(timestamp, "Id", jobj.Value<String>("id"));
            }
            UpdateActivity(account, timestamp, "CreatedAt", ParseTimestamp(jobj.Value<String>("created_at")).ToString("o"));
            UpdateActivity(account, timestamp, "Description", jobj.Value<String>("description"));
            UpdateActivity(account, timestamp, "FollowersCount", jobj.Value<String>("followers_count"));
            UpdateActivity(account, timestamp, "FollowingCount", jobj.Value<String>("friends_count"));
            UpdateActivity(account, timestamp, "Location", jobj.Value<String>("location"));
            UpdateActivity(account, timestamp, "Name", jobj.Value<String>("name"));
            UpdateActivity(account, timestamp, "ProfileBackgroundColor", jobj.Value<String>("profile_background_color"));
            UpdateActivity(account, timestamp, "ProfileBackgroundImage", jobj.Value<String>("profile_background_image_url"));
            UpdateActivity(account, timestamp, "ProfileBackgroundTile", jobj.Value<Boolean>("profile_background_tile").ToString());
            UpdateActivity(account, timestamp, "ProfileImage", Regex.Replace(jobj.Value<String>("profile_image_url"), @"_normal(\.\w+)$", "$1"));
            UpdateActivity(account, timestamp, "ProfileLinkColor", jobj.Value<String>("profile_link_color"));
            UpdateActivity(account, timestamp, "ProfileSidebarBorderColor", jobj.Value<String>("profile_sidebar_border_color"));
            UpdateActivity(account, timestamp, "ProfileSidebarFillColor", jobj.Value<String>("profile_sidebar_fill_color"));
            UpdateActivity(account, timestamp, "ProfileTextColor", jobj.Value<String>("profile_text_color"));
            UpdateActivity(account, timestamp, "Restricted", jobj.Value<Boolean>("protected").ToString());
            UpdateActivity(account, timestamp, "ScreenName", jobj.Value<String>("screen_name"));
            UpdateActivity(account, timestamp, "StatusesCount", jobj.Value<String>("statuses_count"));
            UpdateActivity(account, timestamp, "TimeZone", jobj.Value<String>("time_zone"));
            UpdateActivity(account, timestamp, "Uri", jobj.Value<String>("url"));

            return account;
        }
         
        private Mark AnalyzeFavorite(JObject jobj, StorageModule storage)
        {
            DateTime timestamp = ParseTimestamp(jobj.Value<String>("created_at"));
            return AnalyzeUser(jobj.Value<JObject>("source"), storage, timestamp)
                .Mark("Favorite", AnalyzeStatus(jobj.Value<JObject>("target_object"), storage));
        }

        private Mark AnalyzeRetweet(JObject jobj, StorageModule storage)
        {
            DateTime timestamp = ParseTimestamp(jobj.Value<String>("created_at"));
            return AnalyzeUser(jobj.Value<JObject>("source"), storage, timestamp)
                .Mark("Retweet", AnalyzeStatus(jobj.Value<JObject>("target_object"), storage));
        }

        private Relation AnalyzeFollow(JObject jobj, StorageModule storage)
        {
            DateTime timestamp = ParseTimestamp(jobj.Value<String>("created_at"));
            return AnalyzeUser(jobj.Value<JObject>("source"), storage, timestamp)
                .Relate("Follow", AnalyzeUser(jobj.Value<JObject>("target"), storage, timestamp));
        }

        private void AsyncAnalyzeFollowing(JObject jobj, StorageModule storage)
        {
            Lambda.New(() => storage.Execute(s =>
            {
                jobj.Value<JArray>("friends")
                    .Values<String>()
                    .Select(i => this._storage.NewAccount(Realm, Create.Table("Id", i)))
                    .ForEach(a => this._self.Relate("Follow", a));
                s.TryUpdate();
                this.Log.Info("Following data was updated with User Streams.");
            })).BeginInvoke(ar => ar.GetAsyncDelegate<Action>().EndInvoke(ar), null);
        }

        private static Activity UpdateActivity(
            Account account,
            DateTime timestamp,
            String category,
            String subId,
            String userAgent,
            String value,
            Byte[] data
        )
        {
            Activity activity = account[category, timestamp];
            if (activity == null)
            {
                return account.Act(timestamp, category, subId, userAgent, value, data);
            }
            if (activity.UserAgent != userAgent)
            {
                activity.UserAgent = userAgent;
            }
            if (activity.Value != value)
            {
                activity.Value = value;
            }
            if (activity.Data != data)
            {
                activity.Data = data;
            }
            return activity;
        }

        private static Activity UpdateActivity(
            Account account,
            DateTime timestamp,
            String category,
            String value
        )
        {
            return UpdateActivity(account, timestamp, category, null, null, value, null);
        }

        private static DateTime ParseTimestamp(String str)
        {
            return DateTime.ParseExact(
                str,
                "ddd MMM dd HH:mm:ss +0000 yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal
            ).ToUniversalTime();
        }
    }
}