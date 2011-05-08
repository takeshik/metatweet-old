// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * TwilogFlow
 *   Flow module to access data in Twilog (Twitter Status Archiving Service)
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of TwilogFlow.
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
using System.Xml;
using System.Xml.Linq;
using Achiral.Extension;
using XSpect.Extension;
using XSpect.MetaTweet.Objects;

namespace XSpect.MetaTweet.Modules
{
    public class TwilogFlow
        : FlowModule
    {
        [FlowInterface("/archive")]
        public IEnumerable<Activity> FetchArchive(StorageSession session, String param, IDictionary<String, String> args)
        {
            Account account = session.Query(StorageObjectExpressionQuery.Activity(
                new ActivityTuple
                {
                    Name = "ScreenName",
                    Value = args["target"],
                }
            ))
                .OrderByDescending(a => a)
                .First()
                .Account;
            DateTime date = args.ContainsKey("date") ? DateTime.ParseExact(args["date"], "yyMMdd", null) : DateTime.Now;
            return new StringReader("<_>" +
                new StringReader("<html>" + new WebClient()
                    .Dispose(c => Encoding.UTF8.GetString(c.DownloadData(
                        String.Format(
                            "http://twilog.org/source.cgi?id={0}&date={1}&order=asc&word={2}&cate=&filter=&type=div",
                            args["target"],
                            args["date"],
                            args.ContainsKey("word") ? args["word"] : ""
                        )
                    ))) + "</html>"
                )
                    .Dispose(sr => XmlReader.Create(sr)
                        .Dispose(xr => xr
                            .Apply(_ => _.ReadToFollowing("textarea"))
                            .ReadElementContentAsString()
                        )
                    ) + "</_>"
            )
                .Dispose(sr => XmlReader.Create(sr)
                    .Dispose(xr => XDocument.Load(xr))
                )
                .Descendants("div")
                .Select(xe => account.Act(
                    "Status",
                    Int64.Parse(xe.XPathEvaluate<String>("substring-after(p[@class='tl-posted']/a/@href,'status/')")),
                    date + TimeSpan.ParseExact(xe.XPathEvaluate<String>("string(p[@class='tl-posted']/a)"), @"hh\:mm\:ss", null),
                    a => a.Act("Body", xe.XPathEvaluate<String>("string(p[@class='tl-text'])"))
                ))
                .ToArray();
        }
    }
}