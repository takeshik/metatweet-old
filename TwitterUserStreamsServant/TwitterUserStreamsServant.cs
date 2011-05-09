// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
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

        private Account _self;

        private readonly Thread _thread;

        public StorageModule Storage
        {
            get;
            private set;
        }

        public StorageSession Session
        {
            get;
            private set;
        }

        public Boolean FetchAllReplies
        {
            get;
            set;
        }

        public Int32 UpdateThreshold
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
            this.Storage = this.Host.ModuleManager.GetModule<StorageModule>(this.Configuration.StorageName);
            this.FetchAllReplies = this.Configuration.FetchAllReplies;
            this.UpdateThreshold = this.Configuration.UpdateThreshold;
        }

        protected override void StartImpl()
        {
            this._reader = this.Open(
                new MessageReceivingEndpoint("https://userstream.twitter.com/2/user.json"
                    + (this.FetchAllReplies ? "?replies=all" : ""),
                HttpDeliveryMethods.GetRequest)
            ).GetResponseReader();
            this.Session = this.Storage.OpenSession();
            this._thread.Start();
        }

        protected override void StopImpl()
        {
            this._thread.Abort();
            this.Session.Update();
            this.Session.Dispose();
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
            this._self = this.Session.Query(StorageObjectDynamicQuery.Activity(new ActivityTuple()
            {
                Name = "ScreenName",
                Value = JObject.Parse(
                    this.Open(
                        new MessageReceivingEndpoint("https://api.twitter.com/1/account/verify_credentials.json", HttpDeliveryMethods.GetRequest)
                    ).GetResponseReader().Dispose(sr => sr.ReadToEnd())
                ).Value<String>("screen_name"),
            }))
                .ToArray()
                .Single()
                .Account;
            Make.Repeat(this._reader)
                .Select(r => r.ReadLine())
                .Catch(Enumerable.Empty<String>())
                .Where(s => !String.IsNullOrWhiteSpace(s))
                .Select(JObject.Parse)
                .ForEach(j =>
                {
#if DEBUG
                    if (this.Options.Contains("debug-dump"))
                    {
                        this.Host.Directories.LogDirectory
                            .CreateSubdirectory(this + "_dump")
                            .File(DateTime.UtcNow.ToString("yyyyMMdd-HHmmssfff") + ".json")
                            .WriteAllText(j.ToString());
                    }
#endif
                    try
                    {
                        if (j["in_reply_to_user_id"] != null)
                        {
                            AnalyzeStatus(j);
                        }
                        else if (j["event"] != null)
                        {
                            switch (j["event"].Value<String>())
                            {
                                case "favorite":
                                    AnalyzeFavorite(j);
                                    break;
                                case "retweet":
                                    AnalyzeRetweet(j);
                                    break;
                                case "follow":
                                    AnalyzeFollow(j);
                                    break;
                            }
                        }
                        else if (j["friends"] != null)
                        {
                            AsyncAnalyzeFollowing(j);
                        }
                        if (this.Session.AddingObjects.Count >= this.UpdateThreshold)
                        {
                            this.Session.Update();
                            if (!this.Session.AddingObjects.Any())
                            {
                                this.Session.Dispose();
                                this.Session = this.Storage.OpenSession();
                                // Migrate the context of self account to new one
                                this._self = this.Session.Load(this._self.Id);
                            }
                        }
                    }
                    catch (ThreadAbortException)
                    {
                    }
                    catch (Exception ex)
                    {
                        this.Log.Error(
                            "Exception was thrown in process to read this JSON object (note this does not means following JSON is invalid data)." +
                            Environment.NewLine +
                            j.ToString(),
                            ex
                        );
                    }
                });
            Thread.Sleep(Timeout.Infinite);
        }

        private Activity AnalyzeStatus(JObject jobj)
        {
            DateTime timestamp = ParseTimestamp(jobj.Value<String>("created_at"));
            Account account = this.AnalyzeUser(jobj.Value<JObject>("user"), timestamp);
            Activity post = account.Act(
                "Status",
                jobj.Value<Int64>("id"),
                a => a.Advertise(timestamp, AdvertisementFlags.Created),
                a => a.Act("Body", jobj.Value<String>("text")),
                a => a.Act("Source", jobj.Value<String>("source").If(s => s.Contains("</a>"), s =>
                    s.Remove(s.Length - 4 /* "</a>" */).Substring(s.IndexOf('>') + 1)
                ))
            );
            if (jobj.Value<Boolean>("favorited"))
            {
                this._self.Act("Favorite", post.Id);
            }
            // TODO: reply
            return post;
        }

        private Account AnalyzeUser(JObject jobj, DateTime timestamp)
        {
            Account account = this.Session.Create(Realm, Account.GetSeed(Create.Table("Id", jobj.Value<String>("id"))));
            UpdateActivity(account, "Id", jobj.Value<Int64>("id"), timestamp);
            UpdateActivity(account, "CreatedAt", ParseTimestamp(jobj.Value<String>("created_at")), timestamp);
            UpdateActivity(account, "Description", jobj.Value<String>("description"), timestamp);
            UpdateActivity(account, "FollowersCount", jobj.Value<Int32>("followers_count"), timestamp);
            UpdateActivity(account, "FollowingCount", jobj.Value<Int32>("friends_count"), timestamp);
            UpdateActivity(account, "Location", jobj.Value<String>("location"), timestamp);
            UpdateActivity(account, "Name", jobj.Value<String>("name"), timestamp);
            UpdateActivity(account, "ProfileBackgroundColor", jobj.Value<String>("profile_background_color"), timestamp);
            UpdateActivity(account, "ProfileBackgroundImage", jobj.Value<String>("profile_background_image_url"), timestamp);
            UpdateActivity(account, "ProfileBackgroundTile", jobj.Value<Boolean>("profile_background_tile"), timestamp);
            UpdateActivity(account, "ProfileImage", Regex.Replace(jobj.Value<String>("profile_image_url"), @"_normal(\.\w+)$", "$1"), timestamp);
            UpdateActivity(account, "ProfileLinkColor", jobj.Value<String>("profile_link_color"), timestamp);
            UpdateActivity(account, "ProfileSidebarBorderColor", jobj.Value<String>("profile_sidebar_border_color"), timestamp);
            UpdateActivity(account, "ProfileSidebarFillColor", jobj.Value<String>("profile_sidebar_fill_color"), timestamp);
            UpdateActivity(account, "ProfileTextColor", jobj.Value<String>("profile_text_color"), timestamp);
            UpdateActivity(account, "Restricted", jobj.Value<Boolean>("protected"), timestamp);
            UpdateActivity(account, "ScreenName", jobj.Value<String>("screen_name"), timestamp);
            UpdateActivity(account, "StatusesCount", jobj.Value<Int32>("statuses_count"), timestamp);
            UpdateActivity(account, "TimeZone", jobj.Value<String>("time_zone"), timestamp);
            UpdateActivity(account, "Uri", jobj.Value<String>("url"), timestamp);
            return account;
        }
         
        private Activity AnalyzeFavorite(JObject jobj)
        {
            DateTime timestamp = ParseTimestamp(jobj.Value<String>("created_at"));
            return AnalyzeUser(jobj.Value<JObject>("source"), timestamp)
                .Act("Favorite", AnalyzeStatus(jobj.Value<JObject>("target_object")).Id, timestamp);
        }

        private Activity AnalyzeRetweet(JObject jobj)
        {
            DateTime timestamp = ParseTimestamp(jobj.Value<String>("created_at"));
            return AnalyzeUser(jobj.Value<JObject>("source"), timestamp)
                .Act("Retweet", AnalyzeStatus(jobj.Value<JObject>("target_object")).Id, timestamp);
        }

        private Activity AnalyzeFollow(JObject jobj)
        {
            DateTime timestamp = ParseTimestamp(jobj.Value<String>("created_at"));
            return AnalyzeUser(jobj.Value<JObject>("source"), timestamp)
                .Act("Follow", AnalyzeUser(jobj.Value<JObject>("target_object"), timestamp).Id, timestamp);
        }

        private void AsyncAnalyzeFollowing(JObject jobj)
        {
            jobj.Value<JArray>("friends")
                .Values<String>()
                .Select(i => this.Session.Create(Realm, Account.GetSeed(Create.Table("Id", i))))
                .ForEach(a => this._self.Act("Follow", a.Id));
            this.Log.Info("Following data was updated with User Streams.");
        }

        private static Activity UpdateActivity(Account account, String name, Object value, DateTime timestamp)
        {
            return value != null
                ? account.Act(name, value, timestamp)
                : null;
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
