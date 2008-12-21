// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
/* TwitterApiProxy
 *   Proxy for Twitter Timeline with RESTish API 
 *   Part of MetaTweet
 * Copyright © 2008 Takeshi KIRIYA, XSpect Project <takeshik@xspect.org>
 * All rights reserved.
 * 
 * This file is part of TwitterApiProxy.
 * 
 * This library is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation; either version 3 of the License, or (at your
 * option) any later version.
 * 
 * This library is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
 * or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public
 * License for more details. 
 * 
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>,
 * or write to the Free Software Foundation, Inc., 51 Franklin Street,
 * Fifth Floor, Boston, MA 02110-1301, USA.
 */

using XSpect.MetaTweet;
using XSpect.Net;
using System.Collections.Generic;
using System;
using System.Net;
using System.Xml;

namespace com.twitter
{
    public class TwitterApiProxy
        : Proxy
    {
        private readonly HttpClient _client = new HttpClient("MetaTweet-TwitterApiProxy");

        public TwitterApiProxy(String username, String password)
        {
            this._client.Credential = new NetworkCredential(username,password);
        }

        [ProxyInterface("statuses/public_timeline")]
        public void GetPublicTimeline(StorageDataSetUnit datasets, IDictionary<String, String> arguments)
        {
            // TODO: Implement
            throw new NotImplementedException();
        }
    }
}