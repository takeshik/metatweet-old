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
        public IEnumerable<StorageObject> FetchPublicTimeline(String param, IDictionary<String, String> args)
        {
            XmlDocument xresponse = this._client.Post(
                new Uri(TwitterHost + "/statuses/public_timeline.xml" + args.ToUriQuery()),
                new Byte[0],
                this._generateXml
            );
            throw new NotImplementedException();
        }

        // id : int | string
        // since : datetime
        // count : int
        // page : int
        [FlowInterface("/statuses/friends_timeline")]
        public IEnumerable<StorageObject> FetchFriendsTimeline(String param, IDictionary<String, String> args)
        {
            throw new NotImplementedException();
        }

        // id : int | string
        // count : int
        // since : datetime
        // since_id : int
        // page : int
        [FlowInterface("/statuses/user_timeline")]
        public IEnumerable<StorageObject> FetchUserTimeline(String param, IDictionary<String, String> args)
        {
            throw new NotImplementedException();
        }

        // id : int (mandatory)
        [FlowInterface("/statuses/show/")]
        public IEnumerable<StorageObject> FetchStatus(String param, IDictionary<String, String> args)
        {
            throw new NotImplementedException();
        }

        // status : string (mandatory)
        // in_reply_to_status_id : int
        // source : string
        [FlowInterface("/statuses/update")]
        public IEnumerable<StorageObject> UpdateStatus(String param, IDictionary<String, String> args)
        {
            throw new NotImplementedException();
        }

        // page : int
        // since : datetime
        // since_id : int
        [FlowInterface("/statuses/replies")]
        public IEnumerable<StorageObject> FetchReplies(String param, IDictionary<String, String> args)
        {
            throw new NotImplementedException();
        }

        // (last-segment) : int (mandatory)
        [FlowInterface("/statuses/destroy/")]
        public IEnumerable<StorageObject> DestroyStatus(String param, IDictionary<String, String> args)
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


    }
}