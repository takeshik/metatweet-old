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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Achiral.Extension;
using LinqToTwitter;
using Newtonsoft.Json.Linq;
using XSpect.MetaTweet.Objects;
using Twitter = LinqToTwitter;
using XSpect.Extension;
using Achiral;
using System.Text.RegularExpressions;
using Account = LinqToTwitter.Account;

namespace XSpect.MetaTweet.Modules
{
    public class TwitterApiInput
        : FlowModule
    {
        public const String Realm = "com.twitter";

        [CLSCompliant(false)]
        public TwitterContext Context
        {
            get;
            private set;
        }

        [CLSCompliant(false)]
        public MetaTweetAuthorizer Authorization
        {
            get;
            private set;
        }

        public TwitterApiInput()
        {
        }

        protected override void InitializeImpl()
        {
            this.Authorization = new MetaTweetAuthorizer(this.Host.Directories.RuntimeDirectory.File(this + "_token.dat"));
            this.Context = new TwitterContext(this.Authorization, "https://api.twitter.com/1/", "http://search.twitter.com/");
            this.Authorization.GetPin = uri =>
            {
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
                    return Console.ReadLine().Apply(_ => uriFile.Delete());
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
                    return Observable.FromEvent<FileSystemEventArgs>(this.Host.Directories.RuntimeDirectoryWatcher, "Created")
                        .Select(e => e.EventArgs.Name)
                        .Where(n => n == inputFile.Name)
                        .First()
                        .Let(_ => inputFile.ReadAllLines().First().Trim())
                        .Apply(_ => inputFile.Delete(), _ => uriFile.Delete());
                }
            };
            this.Authorization.Authorize();
            base.InitializeImpl();
        }

        protected override void Dispose(Boolean disposing)
        {
            this.Authorization.Save();
            this.Context.EndAccountSession();
            this.Context.Dispose();
            base.Dispose(disposing);
        }

        [FlowInterface("/statuses/public_timeline")]
        public IEnumerable<Activity> FetchPublicTimeline(StorageSession session, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(session);
            Expression<Func<Status, Boolean>> query = s => s.Type == StatusType.Public;
            if (args.ContainsKey("count"))
            {
                query = ConcatQuery(query, s => s.Count == Int32.Parse(args["count"]));
            }
            if (args.ContainsKey("page"))
            {
                query = ConcatQuery(query, s => s.Page == Int32.Parse(args["page"]));
            }
            return this.Context.Status
                .Where(query)
                .AsEnumerable()
                .Select(s => this.AnalyzeStatus(session, s, self, null))
                .ToArray();
        }

        [FlowInterface("/statuses/home_timeline")]
        [FlowInterface("/statuses/friends_timeline")]
        public IEnumerable<Activity> FetchHomeTimeline(StorageSession session, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(session);
            Expression<Func<Status, Boolean>> query = s => s.Type == StatusType.Home;
            if (args.ContainsKey("since_id"))
            {
                query = ConcatQuery(query, s => s.SinceID == UInt64.Parse(args["since_id"]));
            }
            if (args.ContainsKey("max_id"))
            {
                query = ConcatQuery(query, s => s.MaxID == UInt64.Parse(args["max_id"]));
            }
            if (args.ContainsKey("count"))
            {
                query = ConcatQuery(query, s => s.Count == Int32.Parse(args["count"]));
            }
            if (args.ContainsKey("page"))
            {
                query = ConcatQuery(query, s => s.Page == Int32.Parse(args["page"]));
            }
            return this.Context.Status
                .Where(query)
                .AsEnumerable()
                .Select(s => this.AnalyzeStatus(session, s, self, null))
                .ToArray();
        }

        [FlowInterface("/statuses/user_timeline")]
        public IEnumerable<Activity> FetchUserTimeline(StorageSession session, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(session);
            Expression<Func<Status, Boolean>> query = s => s.Type == StatusType.User;
            if (args.ContainsKey("id"))
            {
                query = ConcatQuery(query, s => s.ID == args["id"]);
            }
            if (args.ContainsKey("screen_name"))
            {
                query = ConcatQuery(query, s => s.ScreenName == args["screen_name"]);
            }
            if (args.ContainsKey("user_id"))
            {
                query = ConcatQuery(query, s => s.UserID == args["user_id"]);
            }
            if (args.ContainsKey("since_id"))
            {
                query = ConcatQuery(query, s => s.SinceID == UInt64.Parse(args["since_id"]));
            }
            if (args.ContainsKey("max_id"))
            {
                query = ConcatQuery(query, s => s.MaxID == UInt64.Parse(args["max_id"]));
            }
            if (args.ContainsKey("count"))
            {
                query = ConcatQuery(query, s => s.Count == Int32.Parse(args["count"]));
            }
            if (args.ContainsKey("page"))
            {
                query = ConcatQuery(query, s => s.Page == Int32.Parse(args["page"]));
            }
            return this.Context.Status
                .Where(query)
                .AsEnumerable()
                .Select(s => this.AnalyzeStatus(session, s, self, null))
                .ToArray();
        }
        
        [FlowInterface("/statuses/replies")]
        [FlowInterface("/statuses/mentions")]
        public IEnumerable<Activity> FetchMentions(StorageSession session, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(session);
            Expression<Func<Status, Boolean>> query = s => s.Type == StatusType.Mentions;
            if (args.ContainsKey("since_id"))
            {
                query = ConcatQuery(query, s => s.SinceID == UInt64.Parse(args["since_id"]));
            }
            if (args.ContainsKey("max_id"))
            {
                query = ConcatQuery(query, s => s.MaxID == UInt64.Parse(args["max_id"]));
            }
            if (args.ContainsKey("count"))
            {
                query = ConcatQuery(query, s => s.Count == Int32.Parse(args["count"]));
            }
            if (args.ContainsKey("page"))
            {
                query = ConcatQuery(query, s => s.Page == Int32.Parse(args["page"]));
            }
            return this.Context.Status
                .Where(query)
                .AsEnumerable()
                .Select(s => this.AnalyzeStatus(session, s, self, null))
                .ToArray();
        }

        [FlowInterface("/statuses/retweeted_by_me")]
        public IEnumerable<Activity> FetchRetweetedByMe(StorageSession session, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(session);
            Expression<Func<Status, Boolean>> query = s => s.Type == StatusType.RetweetedByMe;
            if (args.ContainsKey("since_id"))
            {
                query = ConcatQuery(query, s => s.SinceID == UInt64.Parse(args["since_id"]));
            }
            if (args.ContainsKey("max_id"))
            {
                query = ConcatQuery(query, s => s.MaxID == UInt64.Parse(args["max_id"]));
            }
            if (args.ContainsKey("count"))
            {
                query = ConcatQuery(query, s => s.Count == Int32.Parse(args["count"]));
            }
            if (args.ContainsKey("page"))
            {
                query = ConcatQuery(query, s => s.Page == Int32.Parse(args["page"]));
            }
            return this.Context.Status
                .Where(query)
                .AsEnumerable()
                .Select(s => this.AnalyzeStatus(session, s, self, null))
                .ToArray();
        }

        [FlowInterface("/statuses/retweeted_to_me")]
        public IEnumerable<Activity> FetchRetweetedToMe(StorageSession session, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(session);
            Expression<Func<Status, Boolean>> query = s => s.Type == StatusType.RetweetedToMe;
            if (args.ContainsKey("since_id"))
            {
                query = ConcatQuery(query, s => s.SinceID == UInt64.Parse(args["since_id"]));
            }
            if (args.ContainsKey("max_id"))
            {
                query = ConcatQuery(query, s => s.MaxID == UInt64.Parse(args["max_id"]));
            }
            if (args.ContainsKey("count"))
            {
                query = ConcatQuery(query, s => s.Count == Int32.Parse(args["count"]));
            }
            if (args.ContainsKey("page"))
            {
                query = ConcatQuery(query, s => s.Page == Int32.Parse(args["page"]));
            }
            return this.Context.Status
                .Where(query)
                .AsEnumerable()
                .Select(s => this.AnalyzeStatus(session, s, self, null))
                .ToArray();
        }

        [FlowInterface("/statuses/retweets_of_me")]
        public IEnumerable<Activity> FetchRetweetsOfMe(StorageSession session, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(session);
            Expression<Func<Status, Boolean>> query = s => s.Type == StatusType.RetweetsOfMe;
            if (args.ContainsKey("since_id"))
            {
                query = ConcatQuery(query, s => s.SinceID == UInt64.Parse(args["since_id"]));
            }
            if (args.ContainsKey("max_id"))
            {
                query = ConcatQuery(query, s => s.MaxID == UInt64.Parse(args["max_id"]));
            }
            if (args.ContainsKey("count"))
            {
                query = ConcatQuery(query, s => s.Count == Int32.Parse(args["count"]));
            }
            if (args.ContainsKey("page"))
            {
                query = ConcatQuery(query, s => s.Page == Int32.Parse(args["page"]));
            }
            return this.Context.Status
                .Where(query)
                .AsEnumerable()
                .Select(s => this.AnalyzeStatus(session, s, self, null))
                .ToArray();
        }

        [FlowInterface("/statuses/show")]
        public IEnumerable<Activity> GetStatus(StorageSession session, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(session);
            Expression<Func<Status, Boolean>> query = s => s.Type == StatusType.Show;
            if (args.ContainsKey("id"))
            {
                query = ConcatQuery(query, u => u.StatusID == args["id"]);
            }
            return this.Context.Status
                .Where(query)
                .AsEnumerable()
                .Select(s => this.AnalyzeStatus(session, s, self, null))
                .ToArray();
        }

        [FlowInterface("/statuses/update")]
        public IEnumerable<Activity> UpdateStatus(StorageSession session, String param, IDictionary<String, String> args)
        {
            try
            {
                this.Context.UpdateStatus(args["status"]);
            }
            // HACK: Ignore since even so updated
            catch
            {
            }
            return Enumerable.Empty<Activity>();
        }

        [FlowInterface("/statuses/destroy")]
        [FlowInterface("/statuses/destroy/")]
        public IEnumerable<Activity> DestroyStatus(StorageSession session, String param, IDictionary<String, String> args)
        {
            this.Context.DestroyStatus(param.IsNullOrEmpty() ? args["id"] : param);
            return Enumerable.Empty<Activity>();
        }

        [FlowInterface("/statuses/retweet")]
        public IEnumerable<Activity> Retweet(StorageSession session, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(session);
            this.Context.Retweet(args["id"]);
            // TODO: Create Storage Object? (or get from continuing input?)
            return Enumerable.Empty<Activity>();
        }

        [FlowInterface("/statuses/retweets")]
        public IEnumerable<Activity> FetchRetweets(StorageSession session, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(session);
            Expression<Func<Status, Boolean>> query = s => s.Type == StatusType.Retweets;
            if (args.ContainsKey("id"))
            {
                query = ConcatQuery(query, s => s.ID == args["id"]);
            }
            if (args.ContainsKey("screen_name"))
            {
                query = ConcatQuery(query, s => s.ScreenName == args["screen_name"]);
            }
            if (args.ContainsKey("user_id"))
            {
                query = ConcatQuery(query, s => s.UserID == args["user_id"]);
            }
            if (args.ContainsKey("since_id"))
            {
                query = ConcatQuery(query, s => s.SinceID == UInt64.Parse(args["since_id"]));
            }
            if (args.ContainsKey("max_id"))
            {
                query = ConcatQuery(query, s => s.MaxID == UInt64.Parse(args["max_id"]));
            }
            if (args.ContainsKey("count"))
            {
                query = ConcatQuery(query, s => s.Count == Int32.Parse(args["count"]));
            }
            if (args.ContainsKey("page"))
            {
                query = ConcatQuery(query, s => s.Page == Int32.Parse(args["page"]));
            }
            return this.Context.Status
                .Where(query)
                .AsEnumerable()
                .Select(s => this.AnalyzeStatus(session, s, self, null))
                .ToArray();
        }

        [FlowInterface("/statuses/friends")]
        [FlowInterface("/users/following")]
        public IEnumerable<Objects.Account> GetFollowingUsers(StorageSession session, String param, IDictionary<String, String> args)
        {
            Expression<Func<User, Boolean>> query = u => u.Type == UserType.Friends;
            if (args.ContainsKey("id"))
            {
                query = ConcatQuery(query, u => u.ID == args["id"]);
            }
            if (args.ContainsKey("user_id"))
            {
                query = ConcatQuery(query, u => u.UserID == args["user_id"]);
            }
            if (args.ContainsKey("screen_name"))
            {
                query = ConcatQuery(query, u => u.ScreenName == args["screen_name"]);
            }
            if (args.ContainsKey("cursor"))
            {
                query = ConcatQuery(query, u => u.Cursor == args["cursor"]);
            }
            // TODO: Count, Page (Modify LinqToTwitter?)
            return this.Context.User
                .Where(query)
                .AsEnumerable()
                .Select(u => this.AnalyzeUser(
                    session,
                    u,
                    DateTime.UtcNow,
                    u.ScreenName != this.Context.UserName
                        ? this.GetSelfAccount(session)
                        : null,
                    true
                ))
                .ToArray();
        }

        [FlowInterface("/statuses/followers")]
        [FlowInterface("/users/followers")]
        public IEnumerable<Objects.Account> GetFollowerUsers(StorageSession session, String param, IDictionary<String, String> args)
        {
            Expression<Func<User, Boolean>> query = u => u.Type == UserType.Followers;
            if (args.ContainsKey("id"))
            {
                query = ConcatQuery(query, u => u.ID == args["id"]);
            }
            if (args.ContainsKey("user_id"))
            {
                query = ConcatQuery(query, u => u.UserID == args["user_id"]);
            }
            if (args.ContainsKey("screen_name"))
            {
                query = ConcatQuery(query, u => u.ScreenName == args["screen_name"]);
            }
            if (args.ContainsKey("cursor"))
            {
                query = ConcatQuery(query, u => u.Cursor == args["cursor"]);
            }
            // TODO: Count, Page (Modify LinqToTwitter?)
            return this.Context.User
                .Where(query)
                .AsEnumerable()
                .Select(u => this.AnalyzeUser(
                    session,
                    u,
                    DateTime.UtcNow,
                    u.ScreenName != this.Context.UserName
                        ? this.GetSelfAccount(session)
                        : null,
                    true
                ))
                .ToArray();
        }
        
        [FlowInterface("/users/show")]
        public IEnumerable<Objects.Account> GetUser(StorageSession session, String param, IDictionary<String, String> args)
        {
            Expression<Func<User, Boolean>> query = u => u.Type == UserType.Show;
            if (args.ContainsKey("id"))
            {
                query = ConcatQuery(query, u => u.ID == args["id"]);
            }
            if (args.ContainsKey("user_id"))
            {
                query = ConcatQuery(query, u => u.UserID == args["user_id"]);
            }
            if (args.ContainsKey("screen_name"))
            {
                query = ConcatQuery(query, u => u.ScreenName == args["screen_name"]);
            }
            return this.Context.User
                .Where(query)
                .AsEnumerable()
                .Select(u => this.AnalyzeUser(
                    session,
                    u,
                    DateTime.UtcNow,
                    u.ScreenName != this.Context.UserName
                        ? this.GetSelfAccount(session)
                        : null,
                    true
                ))
                .ToArray();
        }

        [FlowInterface("/users/lookup")]
        public IEnumerable<Objects.Account> LookupUsers(StorageSession session, String param, IDictionary<String, String> args)
        {
            Expression<Func<User, Boolean>> query = u => u.Type == UserType.Lookup;
            if (args.ContainsKey("user_id"))
            {
                query = ConcatQuery(query, u => u.UserID == args["user_id"]);
            }
            if (args.ContainsKey("screen_name"))
            {
                query = ConcatQuery(query, u => u.ScreenName == args["screen_name"]);
            }
            return this.Context.User
                .Where(query)
                .AsEnumerable()
                .Select(u => this.AnalyzeUser(
                    session,
                    u,
                    DateTime.UtcNow,
                    u.ScreenName != this.Context.UserName
                        ? this.GetSelfAccount(session)
                        : null,
                    true
                ))
                .ToArray();
        }

        [FlowInterface("/friendships/create")]
        public IEnumerable<Activity> Follow(StorageSession session, String param, IDictionary<String, String> args)
        {
            this.Context.CreateFriendship(
                args.GetValueOrDefault("id"),
                args.GetValueOrDefault("user_id"),
                args.GetValueOrDefault("screen_name"),
                false
            );
            // TODO: Create Strage Object? (or get from continuing input?)
            return Enumerable.Empty<Activity>();
        }

        [FlowInterface("/friendships/destroy")]
        public IEnumerable<Activity> Unfollow(StorageSession session, String param, IDictionary<String, String> args)
        {
            this.Context.DestroyFriendship(
                args.GetValueOrDefault("id"),
                args.GetValueOrDefault("user_id"),
                args.GetValueOrDefault("screen_name")
            );
            // TODO: Remove Strage Object? (or get from continuing input?)
            return Enumerable.Empty<Activity>();
        }

        [FlowInterface("/friends/ids")]
        [FlowInterface("/users/following_ids")]
        public IEnumerable<Objects.Account> GetFollowingIds(StorageSession session, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(session);
            return this.Context.SocialGraph.Where(g => g.Type == SocialGraphType.Friends)
                .Select(g => self.Act("Follow", this.TryGetAccount(session, g.ID, DateTime.UtcNow).Id).GetValue<Objects.Account>())
                .ToArray();
        }

        [FlowInterface("/followers/ids")]
        [FlowInterface("/users/follower_ids")]
        public IEnumerable<Objects.Account> GetFollowerIds(StorageSession session, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(session);
            return this.Context.SocialGraph.Where(g => g.Type == SocialGraphType.Followers)
                .Select(g => this.TryGetAccount(session, g.ID, DateTime.UtcNow).Act("Follow", self.Id).GetValue<Objects.Account>())
                .ToArray();
        }

        [FlowInterface("/favorites")]
        [FlowInterface("/statuses/favorites")]
        public IEnumerable<Activity> FetchFavorites(StorageSession session, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(session);
            Expression<Func<Favorites, Boolean>> query = null;
            if (args.ContainsKey("since_id"))
            {
                query = ConcatQuery(query, f => f.SinceID == UInt64.Parse(args["since_id"]));
            }
            if (args.ContainsKey("max_id"))
            {
                query = ConcatQuery(query, f => f.MaxID == UInt64.Parse(args["max_id"]));
            }
            if (args.ContainsKey("count"))
            {
                query = ConcatQuery(query, f => f.Count == Int32.Parse(args["count"]));
            }
            if (args.ContainsKey("page"))
            {
                query = ConcatQuery(query, f => f.Page == Int32.Parse(args["page"]));
            }
            return this.Context.Favorites
                .Where(query)
                .AsEnumerable()
                .Select(f => this.AnalyzeStatus(session, f, self, null))
                .ToArray();
        }

        [FlowInterface("/favorites/create")]
        public IEnumerable<Activity> CreateFavorite(StorageSession session, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(session);
            this.Context.CreateFavorite(args["id"]);
            // TODO: Create Strage Object? (or get from continuing input?)
            return Enumerable.Empty<Activity>();
        }

        [FlowInterface("/favorites/destroy")]
        public IEnumerable<Activity> DestroyFavorite(StorageSession session, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(session);
            this.Context.DestroyFavorite(args["id"]);
            // TODO: Remove Strage Object? (or get from continuing input?)
            return Enumerable.Empty<Activity>();
        }

        [FlowInterface("/lists/users")]
        public IEnumerable<Objects.Activity> GetList(StorageSession session, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(session);
            List list = this.Context.List.Where(l => l.Type == ListType.Members && l.ScreenName == args["screen_name"] && l.ListID == args["id"]).Single();
            return list.Users.Select(u => this.AnalyzeUser(session, u, DateTime.UtcNow, self, false)
                .Act("ListMember", list.ScreenName + "/" + list.ID)
            );
        }

        [FlowInterface("/search")]
        public IEnumerable<Activity> Search(StorageSession session, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(session);
            Expression<Func<Search, Boolean>> query = s => s.Type == SearchType.Search;
            if (args.ContainsKey("query"))
            {
                query = ConcatQuery(query, s => s.Query == args["query"]);
            }
            if (args.ContainsKey("since_id"))
            {
                query = ConcatQuery(query, s => s.SinceID == UInt64.Parse(args["since_id"]));
            }
            if (args.ContainsKey("max_id"))
            {
                query = ConcatQuery(query, s => s.MaxID == UInt64.Parse(args["max_id"]));
            }
            if (args.ContainsKey("page_size"))
            {
                query = ConcatQuery(query, s => s.PageSize == Int32.Parse(args["page_size"]));
            }
            if (args.ContainsKey("page"))
            {
                query = ConcatQuery(query, s => s.Page == Int32.Parse(args["page"]));
            }
            return this.Context.Search.Where(query)
                .AsEnumerable()
                .SelectMany(s => this.AnalyzeSearchResult(session, s, self))
                .ToArray();
        }

        private Objects.Account GetSelfAccount(StorageSession session)
        {
            Activity selfInfo = session.Query(StorageObjectDynamicQuery.Activity(
                new ActivityTuple()
                {
                    Name = "ScreenName",
                    Value = this.Context.UserName,
                }
            ))
                .OrderByDescending(a => a)
                .FirstOrDefault();

            return selfInfo != null
                ? selfInfo.Account
                : this.GetUser(session, null, Create.Table("screen_name", this.Context.UserName))
                      .Single();
        }

        private Activity AnalyzeStatus(StorageSession session, Status status, Objects.Account self, Objects.Account account)
        {
            if (self == null && !(status.User == null || status.User.ScreenName == this.Context.UserName))
            {
                self = this.GetSelfAccount(session);
            }

            if (account == null)
            {
                account = this.AnalyzeUser(session, status.User, status.CreatedAt, self, false);
            }
            Activity activity = account.Act("Status", status.StatusID,
                a => a.Advertise(status.CreatedAt, AdvertisementFlags.Created),
                a => a.Act("Body", status.Text),
                a => a.Act("Source", status.Source.If(s => s.Contains("</a>"), s =>
                    s.Remove(s.Length - 4 /* "</a>" */).Substring(s.IndexOf('>') + 1)
                )
            ));
            if (status.Favorited)
            {
                self.Act("Favorite", activity.Id);
            }
            if (!status.InReplyToUserID.IsNullOrEmpty())
            {
                Objects.Account inReplyToAccount = this.TryGetAccount(session, status.InReplyToUserID, status.CreatedAt);
                Activity inReplyToActivity = inReplyToAccount["Post"]
                    .SingleOrDefault(a => a.GetValue<Int64>() == Int64.Parse(status.InReplyToStatusID));
                if (inReplyToActivity != null)
                {
                    activity.Act("Mention", inReplyToActivity.Id);
                }
            }
            return activity;
        }

        private Objects.Account AnalyzeUser(StorageSession session, User user, DateTime timestamp, Objects.Account self, Boolean analyzeStatus)
        {
            // Escape to fill self informations when this is called by GetSelfAccount method.
            if (self == null && user.ScreenName != this.Context.UserName)
            {
                self = this.GetSelfAccount(session);
            }

            Objects.Account account = this.TryGetAccount(session, user.Identifier.ID, timestamp);

            UpdateActivity(account, timestamp, "CreatedAt", user.CreatedAt.ToUniversalTime().ToString("o"));
            UpdateActivity(account, timestamp, "Description", user.Description);
            UpdateActivity(account, timestamp, "FavoritesCount", user.FavoritesCount.ToString());
            UpdateActivity(account, timestamp, "FollowersCount", user.FollowersCount.ToString());
            UpdateActivity(account, timestamp, "FollowingCount", user.FriendsCount.ToString());
            UpdateActivity(account, timestamp, "Location", user.Location);
            UpdateActivity(account, timestamp, "Name", user.Name);
            UpdateActivity(account, timestamp, "ProfileBackgroundColor", user.ProfileBackgroundColor);
            UpdateActivity(account, timestamp, "ProfileBackgroundImage", user.ProfileBackgroundImageUrl);
            UpdateActivity(account, timestamp, "ProfileBackgroundTile", user.ProfileBackgroundTile);
            UpdateActivity(account, timestamp, "ProfileImage", user.ProfileImageUrl.If(
                u => !Regex.IsMatch(u, @"/images/default_profile_\d+\.png"),
                u => Regex.Replace(u, @"_normal(\.\w+)$", "$1")
            ));
            UpdateActivity(account, timestamp, "ProfileLinkColor", user.ProfileLinkColor);
            UpdateActivity(account, timestamp, "ProfileSidebarBorderColor", user.ProfileSidebarBorderColor);
            UpdateActivity(account, timestamp, "ProfileSidebarFillColor", user.ProfileSidebarFillColor);
            UpdateActivity(account, timestamp, "ProfileTextColor", user.ProfileTextColor);
            UpdateActivity(account, timestamp, "Restricted", user.Protected.ToString());
            UpdateActivity(account, timestamp, "ScreenName", user.Identifier.ScreenName);
            UpdateActivity(account, timestamp, "StatusesCount", user.StatusesCount.ToString());
            UpdateActivity(account, timestamp, "TimeZone", user.TimeZone);
            UpdateActivity(account, timestamp, "Uri", user.URL);

            if (user.Following)
            {
                Activity follow = session.Query(StorageObjectExpressionQuery.Activity(
                    new ActivityTuple()
                    {
                        AccountId = self.Id,
                        Name = "Follow",
                    }
                )).FirstOrDefault(a => a.GetValue<AccountId>() == account.Id);
                if (follow == null && user.ScreenName != this.Context.UserName)
                {
                    self.Act("Follow", account.Id);
                }
            }
            if (analyzeStatus && user.Status != null)
            {
                this.AnalyzeStatus(session, user.Status, self, account);
            }

            return account;
        }

        private IEnumerable<Activity> AnalyzeSearchResult(StorageSession session, Search feed, Objects.Account self)
        {
            IDictionary<String, Objects.Account> accounts = feed.Entries
                .Select(e => e.Author.URI.Let(s => s.Substring(s.LastIndexOf('/') + 1)))
                .Distinct()
                .BufferWithCount(100)
                .SelectMany(p => this.LookupUsers(session, null, Create.Table("screen_name", p.Join(","))))
                .ToDictionary(a => a.Lookup("ScreenName").GetValue<String>().ToLower());
            return feed.Entries
                .Select(e => this.AnalyzeAtomEntry(session, e, self, accounts[e.Author.URI.Let(s => s.Substring(s.LastIndexOf('/') + 1)).ToLower()]))
                .ToArray();
        }

        private Activity AnalyzeAtomEntry(StorageSession session, AtomEntry entry, Objects.Account self, Objects.Account account)
        {
            if (self == null)
            {
                self = this.GetSelfAccount(session);
            }
            // TODO: Check entry.Image?
            return account.Act(
                "Status",
                entry.Alternate.Substring(entry.Alternate.LastIndexOf('/') + 1),
                a => a.Advertise(entry.Updated.ToUniversalTime(), AdvertisementFlags.Created),
                a => a.Act("Body", entry.Title),
                a => a.Act("Source", entry.Source.If(s => s.Contains("</a>"), s =>
                    s.Remove(s.Length - 4 /* "</a>" */).Substring(s.IndexOf('>') + 1)
                ))
            );
        }

        private Activity UpdateActivity(Objects.Account account, DateTime timestamp, String name, Object value)
        {
            return value != null
                ? account.Act(name, value, timestamp)
                : null;
        }

        private static Expression<Func<T, Boolean>> ConcatQuery<T>(Expression<Func<T, Boolean>> left, Expression<Func<T, Boolean>> right)
        {
            return left != null && right != null
                ? Expression.Lambda<Func<T, Boolean>>(
                      Expression.And(
                          left.Body,
                          Expression.Invoke(right, left.Parameters)
                      ),
                      left.Parameters
                  )
                : left ?? right;
        }

        private Objects.Account TryGetAccount(StorageSession session, String userId, DateTime timestamp)
        {
            Objects.Account account = Objects.Account.GetSeed(Create.Table("Id", userId)).Let(seed =>
                AccountId.Create(Realm, seed).Let(id =>
                    session.Load(id) ?? session.Create(Realm, seed)
                )
            );

            if (!account.Activities.Any(a => a.Name == "Id"))
            {
                account.Act("Id", Int64.Parse(userId), a => a.Advertise(DateTime.UtcNow, AdvertisementFlags.Created));
            }

            return account;
        }
    }
}