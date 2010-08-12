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
using System.Linq;
using System.Linq.Expressions;
using Achiral.Extension;
using LinqToTwitter;
using XSpect.MetaTweet.Objects;
using Twitter = LinqToTwitter;
using XSpect.Net;
using System.Xml;
using System.Globalization;
using XSpect.MetaTweet.Modules;
using XSpect.Extension;
using System.Net;
using System.Xml.Linq;
using Achiral;

namespace XSpect.MetaTweet.Modules
{
    public class TwitterApiInput
        : InputFlowModule
    {
        private const String ConsumerKey = "yR1QZk9UQSxuMEpaYLclNw";

        private const String ConsumerSecret = "tcg66ewkX96Kk9m6MQam2GWhXBqpY74UJpqIfqqCA";

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
            this.Authorization = new DesktopOAuthAuthorization(ConsumerKey, ConsumerSecret);
            this.Context = new TwitterContext(this.Authorization, "https://api.twitter.com/1/", "http://search.twitter.com/");
            this.Authorization.Proxy = this.Proxy;
            this.Authorization.GetVerifier = () =>
            {
                Console.Write(
@"TwitterApiInput: Input OAuth authorization PIN, provided by Twitter, after
the service granted access to this module:
PIN> "
                );
                return Console.ReadLine();
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
        public IEnumerable<StorageObject> FetchPublicTimeline(StorageModule storage, String param, IDictionary<String, String> args)
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
                .Cast<StorageObject>()
                .ToArray();
        }

        [FlowInterface("/statuses/home_timeline")]
        [FlowInterface("/statuses/friends_timeline")]
        public IEnumerable<StorageObject> FetchHomeTimeline(StorageModule storage, String param, IDictionary<String, String> args)
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
                .Cast<StorageObject>()
                .ToArray();
        }

        [FlowInterface("/statuses/mentions")]
        public IEnumerable<StorageObject> FetchMentions(StorageModule storage, String param, IDictionary<String, String> args)
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
                .Cast<StorageObject>()
                .ToArray();
        }

        [FlowInterface("/statuses/user_timeline")]
        public IEnumerable<StorageObject> FetchUserTimeline(StorageModule storage, String param, IDictionary<String, String> args)
        {
            Objects.Account self = this.GetSelfAccount(storage);
            Expression<Func<Status, Boolean>> query = s => s.Type == StatusType.User;
            if (args.ContainsKey("id"))
            {
                query = ConcatQuery(query, s => s.ID == args["id"]);
            }
            if (args.ContainsKey("screen_name"))
            {
                query = ConcatQuery(query, s => s.ScreenName == args["since_id"]);
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
                .Cast<StorageObject>()
                .ToArray();
        }

        [FlowInterface("/statuses/show")]
        public IEnumerable<StorageObject> GetStatus(StorageModule storage, String param, IDictionary<String, String> args)
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
                .Cast<StorageObject>()
                .ToArray();
        }

        [FlowInterface("/statuses/update")]
        public IEnumerable<StorageObject> UpdateStatus(StorageModule storage, String param, IDictionary<String, String> args)
        {
            try
            {
                this.Context.UpdateStatus(args["status"]);
            }
            // HACK: Ignore since even so updated
            catch
            {
            }
            return Enumerable.Empty<StorageObject>();
        }

        [FlowInterface("/users/show")]
        public IEnumerable<StorageObject> GetUser(StorageModule storage, String param, IDictionary<String, String> args)
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
                .Cast<StorageObject>()
                .ToArray();
        }

        [FlowInterface("/statuses/friends")]
        [FlowInterface("/users/following")]
        public IEnumerable<StorageObject> GetFollowingUsers(StorageModule storage, String param, IDictionary<String, String> args)
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
                .Cast<StorageObject>()
                .ToArray();
        }

        [FlowInterface("/statuses/followers")]
        [FlowInterface("/users/followers")]
        public IEnumerable<StorageObject> GetFollowerUsers(StorageModule storage, String param, IDictionary<String, String> args)
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
                .Cast<StorageObject>()
                .ToArray();
        }

        [FlowInterface("/friends/ids")]
        [FlowInterface("/users/following_ids")]
        public IEnumerable<StorageObject> GetFollowingIds(StorageModule storage, String param, IDictionary<String, String> args)
        {
            throw new NotImplementedException();
        }

        [FlowInterface("/followers/ids")]
        [FlowInterface("/users/follower_ids")]
        public IEnumerable<StorageObject> GetFollowerIds(StorageModule storage, String param, IDictionary<String, String> args)
        {
            throw new NotImplementedException();
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

        private static Objects.Activity UpdateActivity(Objects.Account account, DateTime timestamp, String category, String value)
        {
            Activity activity = account[category, timestamp];
            if (activity == null
                ? (activity = account.Act(timestamp, category, null)) is Activity /* true */
                : activity.Value != value
            )
            {
                activity.Value = value;
            }
            return activity;
        }

        private static Expression<Func<T, Boolean>> ConcatQuery<T>(Expression<Func<T, Boolean>> left, Expression<Func<T, Boolean>> right)
        {
            return Expression.Lambda<Func<T, Boolean>>(
                Expression.And(
                    left.Body,
                    Expression.Invoke(
                        right,
                        left.Parameters.Cast<Expression>()
                    )
                ),
                left.Parameters
            );
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