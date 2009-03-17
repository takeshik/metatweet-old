// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetServer.
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

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using XSpect.MetaTweet.ObjectModel;
using XSpect.MetaTweet.Modules;
using XSpect.MetaTweet;
using System.Collections;
using Achiral;
using Achiral.Extension;
using XSpect.Extension;

namespace XSpect
{
    [Serializable()]
    public class Request
        : Object,
          IEnumerable<Request>
    {
        private readonly Request _nextRequest;

        public String TargetStorageName
        {
            get;
            private set;
        }

        public String TargetFlowName
        {
            get;
            private set;
        }

        public String Selector
        {
            get;
            private set;
        }

        public IDictionary<String, String> Arguments
        {
            get;
            private set;
        }

        public String OriginalRequest
        {
            get;
            private set;
        }

        public Request(
            String targetStorageName,
            String targetFlowName,
            String selector,
            IDictionary<String, String> arguments,
            String originalRequest,
            Request nextRequest
        )
        {
            this.TargetStorageName = targetStorageName;
            this.TargetFlowName = targetFlowName;
            this.Selector = selector;
            this.Arguments = arguments;
            this.OriginalRequest = originalRequest;
            this._nextRequest = nextRequest;
        }

        public static Request Parse(String str)
        {
            if (str[str.LastIndexOf('/') + 1] != '.')
            {
                // example.ext?foo=bar -> example?foo=bar/!/.ext
                str = Regex.Replace(str, @"(\.[^?]*)(\?.*)?$", @"$2/!/$1");
            }

            return Make.Array(default(String))
                // Skip first empty string
                .Concat(Regex.Split(str, "/[!$]").SkipWhile(String.IsNullOrEmpty))
                .Pairwise((prev, one) => Make.Tuple(prev, one))
                .Do(tuples => Parse(tuples.First().Item2, null, null, tuples.Skip(1)));
        }

        protected static Request Parse(
            String str,
            String previousStorage,
            String previousFlow,
            IEnumerable<Tuple<String, String>> rest
        )
        {
            String prefixes = str.Substring(0, str.IndexOf('/'));
            String storage = previousStorage ?? "main";
            String flow = previousFlow ?? "sys";

            // a) .../$storage!module/... -> storage!module/...
            // b) .../$storage!/...       -> storage!/...
            // c) .../!module/...         -> module/...
            // d) .../!/...               -> /...
            if (prefixes.Contains("!"))
            {
                if (!prefixes.EndsWith("!")) // a) Specified Storage and Module
                {
                    String[] prefixArray = prefixes.Split('!');
                    storage = prefixArray[0];
                    flow = prefixArray[1];
                }
                else // b) Specified Storage
                {
                    storage = prefixes.TrimEnd('!');
                    // Module is taken over.
                }
            }
            else
            {
                if (prefixes != String.Empty) // c) Specified Module
                {
                    // Storage is taken over.
                    flow = prefixes;
                }
                else // d) Specified nothing
                {
                    // Do nothing; Storage and Module are taken over.
                }
            }

            String selector;
            Dictionary<String, String> argumentDictionary = new Dictionary<String, String>();

            if (str.Contains("?"))
            {
                selector = str.Substring(prefixes.Length, str.IndexOf('?') - prefixes.Length);
                String arguments = str.Substring(prefixes.Length + selector.Length);
                argumentDictionary.AddRange(arguments
                    .TrimStart('?')
                    .Split('&')
                    .Select(s => s.Split('='))
                    .Select(s => Create.KeyValuePair(s[0], s[1])
                ));
            }
            else
            {
                selector = str.Substring(prefixes.Length);
            }

            return new Request(
                storage,
                flow,
                selector,
                argumentDictionary,
                str,
                // rest: tuple (prev, one)
                rest.Any() ? Parse(rest.First().Item2, storage, flow, rest.Skip(1)) : null
            );
        }

        public IEnumerator<Request> GetEnumerator()
        {
            yield return this;
            if (this._nextRequest == null)
            {
                yield break;
            }
            yield return this._nextRequest;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public override String ToString()
        {
            return this
                .Select(req => String.Format(
                    "/${0}!{1}{2}{3}",
                    req.TargetStorageName,
                    req.TargetFlowName,
                    req.Selector,
                    req.Arguments.ToUriQuery()
                ))
                .Join("");
        }
    }
}