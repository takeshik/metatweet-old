// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
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

        public override void Initialize()
        {
            this.Authorization = new DesktopOAuthAuthorization(ConsumerKey, ConsumerSecret);
            this.Context = new TwitterContext(this.Authorization, "https://twitter.com/", "http://search.twitter.com/");
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
        }

        protected override void Dispose(Boolean disposing)
        {
            this.Authorization.SignOff();
            base.Dispose(disposing);
        }

        [FlowInterface("/statuses/public_timeline")]
        public IEnumerable<StorageObject> FetchPublicTimeline(StorageModule storage, String param, IDictionary<String, String> args)
        {
            IQueryable<Status> statuses = this.Context.Status
                .Where(s => s.Type == StatusType.Public);
            if (args.ContainsKey("count"))
            {
                statuses = statuses.Where(s => s.Count == Int32.Parse(args["count"]));
            }
            if (args.ContainsKey("page"))
            {
                statuses = statuses.Where(s => s.Page == Int32.Parse(args["page"]));
            }
            return statuses.Select(s => this.AnalyzeStatus(storage, s)).OfType<StorageObject>();
        }

        [FlowInterface("/statuses/home_timeline")]
        [FlowInterface("/statuses/friends_timeline")]
        public IEnumerable<StorageObject> FetchHomeTimeline(StorageModule storage, String param, IDictionary<String, String> args)
        {
            IQueryable<Status> statuses = this.Context.Status
                .Where(s => s.Type == StatusType.Friends);
            if (args.ContainsKey("count"))
            {
                statuses = statuses.Where(s => s.Count == Int32.Parse(args["count"]));
            }
            if (args.ContainsKey("page"))
            {
                statuses = statuses.Where(s => s.Page == Int32.Parse(args["page"]));
            }
            return statuses.Select(s => this.AnalyzeStatus(storage, s)).OfType<StorageObject>();
        }

        [FlowInterface("/statuses/user_timeline")]
        public IEnumerable<StorageObject> FetchUserTimeline(StorageModule storage, String param, IDictionary<String, String> args)
        {
            IQueryable<Status> statuses = this.Context.Status
                .Where(s => s.Type == StatusType.User)
                .Where(s => s.ScreenName == param);
            if (args.ContainsKey("count"))
            {
                statuses = statuses.Where(s => s.Count == Int32.Parse(args["count"]));
            }
            if (args.ContainsKey("page"))
            {
                statuses = statuses.Where(s => s.Page == Int32.Parse(args["page"]));
            }
            return statuses.Select(s => this.AnalyzeStatus(storage, s)).OfType<StorageObject>();
        }

        private Activity AnalyzeStatus(Storage storage, Status status)
        {
            throw new NotImplementedException();
        }

        private Objects.Account AnalyzeUser(Storage storage, User user)
        {
            throw new NotImplementedException();
        }
    }
}