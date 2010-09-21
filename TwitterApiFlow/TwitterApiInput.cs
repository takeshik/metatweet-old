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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Achiral.Extension;
using LinqToTwitter;
using XSpect.MetaTweet.Objects;
using Twitter = LinqToTwitter;
using XSpect.Extension;
using Achiral;

namespace XSpect.MetaTweet.Modules
{
    public class TwitterApiInput
        : InputFlowModule
    {
        protected override String DefaultRealm
        {
            get
            {
                return "com.twitter";
            }
        }

        [CLSCompliant(false)]
        public TwitterContext Context
        {
            get;
            private set;
        }

        [CLSCompliant(false)]
        public DesktopOAuthAuthorization Authorization
        {
            get;
            private set;
        }

        public TwitterApiInput()
        {
        }

        protected override void InitializeImpl()
        {
            this.Authorization = new DesktopOAuthAuthorization(new MetaTweetTokenManager(this.Host.Directories.RuntimeDirectory.File(this + "_token.dat")));
            this.Context = new TwitterContext(this.Authorization, "https://api.twitter.com/1/", "http://search.twitter.com/");
            this.Authorization.GetVerifier = uri =>
            {
                FileInfo uriFile = this.Host.Directories.RuntimeDirectory.File(this + "_auth.uri")
                    .Let(f => f.WriteAllText(uri.AbsoluteUri));
                if (Environment.UserInteractive)
                {
                    Process.Start(uri.AbsoluteUri);
                    Console.Write(
@"{0}: Input OAuth authorization PIN, provided by Twitter, after
the service granted access to this module:
PIN> "
                    , this);
                    return Console.ReadLine().Let(_ => uriFile.Delete());
                }
                else
                {
                    FileInfo inputFile = this.Host.Directories.RuntimeDirectory.File(this + "_pin.txt")
                        .Let(f => f.Delete());
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
                        .Do(_ => inputFile.ReadAllLines().First().Trim())
                        .Let(_ => inputFile.Delete(), _ => uriFile.Delete());
                }
            };
            this.Authorization.SignOn();
            base.InitializeImpl();
        }

        protected override void Dispose(Boolean disposing)
        {
            this.Authorization.SignOff();
            base.Dispose(disposing);
        }

        [FlowInterface("/statuses/public_timeline")]
        public IEnumerable<Activity> FetchPublicTimeline(StorageModule storage, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(storage);
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
                .Select(s => this.AnalyzeStatus(storage, s, self, null))
                .ToArray();
        }

        [FlowInterface("/statuses/home_timeline")]
        [FlowInterface("/statuses/friends_timeline")]
        public IEnumerable<Activity> FetchHomeTimeline(StorageModule storage, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(storage);
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
                .Select(s => this.AnalyzeStatus(storage, s, self, null))
                .ToArray();
        }

        [FlowInterface("/statuses/user_timeline")]
        public IEnumerable<Activity> FetchUserTimeline(StorageModule storage, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(storage);
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
                .Select(s => this.AnalyzeStatus(storage, s, self, null))
                .ToArray();
        }
        
        [FlowInterface("/statuses/replies")]
        [FlowInterface("/statuses/mentions")]
        public IEnumerable<Activity> FetchMentions(StorageModule storage, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(storage);
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
                .Select(s => this.AnalyzeStatus(storage, s, self, null))
                .ToArray();
        }

        [FlowInterface("/statuses/retweeted_by_me")]
        public IEnumerable<Activity> FetchRetweetedByMe(StorageModule storage, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(storage);
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
                .Select(s => this.AnalyzeStatus(storage, s, self, null))
                .ToArray();
        }

        [FlowInterface("/statuses/retweeted_to_me")]
        public IEnumerable<Activity> FetchRetweetedToMe(StorageModule storage, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(storage);
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
                .Select(s => this.AnalyzeStatus(storage, s, self, null))
                .ToArray();
        }

        [FlowInterface("/statuses/retweets_of_me")]
        public IEnumerable<Activity> FetchRetweetsOfMe(StorageModule storage, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(storage);
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
                .Select(s => this.AnalyzeStatus(storage, s, self, null))
                .ToArray();
        }

        [FlowInterface("/statuses/show")]
        public IEnumerable<Activity> GetStatus(StorageModule storage, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(storage);
            Expression<Func<Status, Boolean>> query = s => s.Type == StatusType.Show;
            if (args.ContainsKey("id"))
            {
                query = ConcatQuery(query, u => u.StatusID == args["id"]);
            }
            return this.Context.Status
                .Where(query)
                .AsEnumerable()
                .Select(s => this.AnalyzeStatus(storage, s, self, null))
                .ToArray();
        }

        [FlowInterface("/statuses/update")]
        public IEnumerable<Activity> UpdateStatus(StorageModule storage, String param, IDictionary<String, String> args)
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
        public IEnumerable<Activity> DestroyStatus(StorageModule storage, String param, IDictionary<String, String> args)
        {
            this.Context.DestroyStatus(param.IsNullOrEmpty() ? args["id"] : param);
            return Enumerable.Empty<Activity>();
        }

        [FlowInterface("/statuses/retweet")]
        public IEnumerable<Mark> Retweet(StorageModule storage, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(storage);
            this.Context.Retweet(args["id"]);
            // TODO: Create Strage Object? (or get from continuing input?)
            return Enumerable.Empty<Mark>();
        }

        [FlowInterface("/statuses/retweets")]
        public IEnumerable<Activity> FetchRetweets(StorageModule storage, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(storage);
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
                .Select(s => this.AnalyzeStatus(storage, s, self, null))
                .ToArray();
        }

        [FlowInterface("/statuses/friends")]
        [FlowInterface("/users/following")]
        public IEnumerable<Objects.Account> GetFollowingUsers(StorageModule storage, String param, IDictionary<String, String> args)
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
                    storage,
                    u,
                    DateTime.UtcNow,
                    u.ScreenName != this.Context.UserName
                        ? this.GetSelfAccount(storage)
                        : null,
                    true
                ))
                .ToArray();
        }

        [FlowInterface("/statuses/followers")]
        [FlowInterface("/users/followers")]
        public IEnumerable<Objects.Account> GetFollowerUsers(StorageModule storage, String param, IDictionary<String, String> args)
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
                    storage,
                    u,
                    DateTime.UtcNow,
                    u.ScreenName != this.Context.UserName
                        ? this.GetSelfAccount(storage)
                        : null,
                    true
                ))
                .ToArray();
        }
        
        [FlowInterface("/users/show")]
        public IEnumerable<Objects.Account> GetUser(StorageModule storage, String param, IDictionary<String, String> args)
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
                    storage,
                    u,
                    DateTime.UtcNow,
                    u.ScreenName != this.Context.UserName
                        ? this.GetSelfAccount(storage)
                        : null,
                    true
                ))
                .ToArray();
        }

        [FlowInterface("/friendships/create")]
        public IEnumerable<Relation> Follow(StorageModule storage, String param, IDictionary<String, String> args)
        {
            this.Context.CreateFriendship(
                args.GetValueOrDefault("id"),
                args.GetValueOrDefault("user_id"),
                args.GetValueOrDefault("screen_name"),
                false
            );
            // TODO: Create Strage Object? (or get from continuing input?)
            return Enumerable.Empty<Relation>();
        }

        [FlowInterface("/friendships/destroy")]
        public IEnumerable<Relation> Unfollow(StorageModule storage, String param, IDictionary<String, String> args)
        {
            this.Context.DestroyFriendship(
                args.GetValueOrDefault("id"),
                args.GetValueOrDefault("user_id"),
                args.GetValueOrDefault("screen_name")
            );
            // TODO: Remove Strage Object? (or get from continuing input?)
            return Enumerable.Empty<Relation>();
        }

        [FlowInterface("/friends/ids")]
        [FlowInterface("/users/following_ids")]
        public IEnumerable<Objects.Account> GetFollowingIds(StorageModule storage, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(storage);
            return this.Context.SocialGraph.Where(g => g.Type == SocialGraphType.Friends)
                .Select(g => self.Relate("Follow", this.TryGetAccount(storage, g.ID, DateTime.UtcNow)).RelatingAccount)
                .ToArray();
        }

        [FlowInterface("/followers/ids")]
        [FlowInterface("/users/follower_ids")]
        public IEnumerable<Objects.Account> GetFollowerIds(StorageModule storage, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(storage);
            return this.Context.SocialGraph.Where(g => g.Type == SocialGraphType.Followers)
                .Select(g => self.Related("Follow", this.TryGetAccount(storage, g.ID, DateTime.UtcNow)).Account)
                .ToArray();
        }

        [FlowInterface("/favorites")]
        [FlowInterface("/statuses/favorites")]
        public IEnumerable<Activity> FetchFavorites(StorageModule storage, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(storage);
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
                .Select(f => this.AnalyzeStatus(storage, f, self, null))
                .ToArray();
        }

        [FlowInterface("/favorites/create")]
        public IEnumerable<Mark> CreateFavorite(StorageModule storage, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(storage);
            this.Context.CreateFavorite(args["id"]);
            // TODO: Create Strage Object? (or get from continuing input?)
            return Enumerable.Empty<Mark>();
        }

        [FlowInterface("/favorites/destroy")]
        public IEnumerable<Mark> DestroyFavorite(StorageModule storage, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(storage);
            this.Context.DestroyFavorite(args["id"]);
            // TODO: Remove Strage Object? (or get from continuing input?)
            return Enumerable.Empty<Mark>();
        }

        [FlowInterface("/lists/users")]
        public IEnumerable<Annotation> GetList(StorageModule storage, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(storage);
            List list = this.Context.List.Where(l => l.Type == ListType.Members && l.ScreenName == args["screen_name"] && l.ListID == args["id"]).Single();
            return list.Users.Select(u => this.AnalyzeUser(storage, u, DateTime.UtcNow, self, false)
                .Annotate("List", list.ScreenName + "/" + list.ID)
            );
        }

        private Objects.Account GetSelfAccount(StorageModule storage)
        {
            Activity selfInfo = storage.GetActivities(
                default(String),
                null,
                "ScreenName",
                null,
                null,
                this.Context.UserName,
                null
            )
                .AsEnumerable()
                .OrderByDescending(a => a)
                .FirstOrDefault();

            return selfInfo != null
                ? selfInfo.Account
                : (Objects.Account) this.GetUser(storage, null, Create.Table("screen_name", this.Context.UserName))
                      .Single();
        }

        private Activity AnalyzeStatus(StorageModule storage, Status status, Objects.Account self, Objects.Account account)
        {
            if (self == null)
            {
                self = this.GetSelfAccount(storage);
            }

            if (account == null)
            {
                account = this.AnalyzeUser(storage, status.User, status.CreatedAt, self, false);
            }
            Activity post = account.Act(
                status.CreatedAt,
                "Post",
                status.StatusID,
                status.Source.If(s => s.Contains("</a>"), s =>
                    s.Remove(s.Length - 4 /* "</a>" */).Substring(s.IndexOf('>') + 1)
                ),
                status.Text,
                null
            );
            if (status.Favorited)
            {
                self.Mark("Favorite", post);
            }
            if (!status.InReplyToUserID.IsNullOrEmpty())
            {
                Objects.Account inReplyToAccount = this.TryGetAccount(storage, status.InReplyToUserID, status.CreatedAt);
                Activity inReplyToPost = inReplyToAccount
                    .ActivitiesOf("Post", status.InReplyToStatusID)
                    .FirstOrDefault();
                if (inReplyToPost != null)
                {
                    post.Refer("Mention", inReplyToPost);
                }
            }
            return post;
        }

        private Objects.Account AnalyzeUser(StorageModule storage, User user, DateTime timestamp, Objects.Account self, Boolean analyzeStatus)
        {
            // Escape to fill self informations when this is called by GetSelfAccount method.
            if (self == null && user.ScreenName != this.Context.UserName)
            {
                self = this.GetSelfAccount(storage);
            }

            Objects.Account account = this.TryGetAccount(storage, user.Identifier.ID, timestamp);

            UpdateActivity(account, timestamp, "CreatedAt", user.CreatedAt.ToString("s"));
            UpdateActivity(account, timestamp, "Description", user.Description);
            UpdateActivity(account, timestamp, "FavoritesCount", user.FavoritesCount.ToString());
            UpdateActivity(account, timestamp, "FollowersCount", user.FollowersCount.ToString());
            UpdateActivity(account, timestamp, "FollowingCount", user.FriendsCount.ToString());
            UpdateActivity(account, timestamp, "Location", user.Location);
            UpdateActivity(account, timestamp, "Name", user.Name);
            UpdateActivity(account, timestamp, "ProfileBackgroundColor", user.ProfileBackgroundColor);
            UpdateActivity(account, timestamp, "ProfileBackgroundImage", user.ProfileBackgroundImageUrl);
            UpdateActivity(account, timestamp, "ProfileBackgroundTile", user.ProfileBackgroundTile);
            UpdateActivity(account, timestamp, "ProfileImage", user.ProfileImageUrl);
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
                Relation relation = storage.GetRelations(self, "Follow", account).FirstOrDefault();
                if (relation == null && user.ScreenName != this.Context.UserName)
                {
                    self.Relate("Follow", account);
                }
            }
            if (analyzeStatus && user.Status != null)
            {
                this.AnalyzeStatus(storage, user.Status, self, account);
            }

            return account;
        }

        private static Objects.Activity UpdateActivity(
            Objects.Account account,
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

        private static Objects.Activity UpdateActivity(
            Objects.Account account,
            DateTime timestamp,
            String category,
            String value
        )
        {
            return UpdateActivity(account, timestamp, category, null, null, value, null);
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

        private Objects.Account TryGetAccount(StorageModule storage, String userId, DateTime timestamp)
        {
            Objects.Account account = Create.Table("Id", userId).Do(seeds =>
                Objects.Account.GetAccountId(this.Realm, seeds).Do(id =>
                    storage.GetAccounts(id).SingleOrDefault()
                        ?? storage.NewAccount(id, this.Realm, seeds)
                )
            );

            if (!account.Activities.Any(a => a.Category == "Id"))
            {
                account.Act(timestamp, "Id", null, null, userId, null);
            }

            return account;
        }
    }
}