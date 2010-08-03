// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
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
using System.Linq;
using XSpect.Extension;
using System.Text.RegularExpressions;

namespace XSpect.MetaTweet
{
    public class RequestTemplate
        : StoredRequest
    {
        public String Template
        {
            get;
            set;
        }

        public override Request Apply(IDictionary<String, String> arguments)
        {
            return this.Replace(Request.Parse(this.Template), arguments);
        }

        private Request Replace(Request request, IDictionary<String, String> arguments)
        {
            return request != null
                ? new Request(
                      this.Replace(request.StorageName, arguments),
                      this.Replace(request.FlowName, arguments),
                      this.Replace(request.Selector, arguments),
                      request.Arguments
                          .Select(p => Create.KeyValuePair(
                              this.Replace(p.Key, arguments),
                              this.Replace(p.Value, arguments))
                          ).ToDictionary(),
                      this.Replace(request.ElementAtOrDefault(1), arguments)
                  )
                : null;
        }

        private String Replace(String str, IDictionary<String, String> arguments)
        {
            return Regex.Replace(str, @"\$\((\w+)\)", m => arguments[m.Groups[1].Value]);
        }
    }
}
