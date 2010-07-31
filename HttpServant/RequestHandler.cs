// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * HttpServant
 *   MetaTweet Servant which provides HTTP service
 *   Part of MetaTweet
 * Copyright © 2008-2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of HttpServant.
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
using System.Net;
using System.Linq;
using System.Text;
using Achiral;
using Achiral.Extension;
using HttpServer;
using HttpServer.Sessions;
using HttpServer.HttpModules;
using XSpect.Codecs;
using XSpect.Extension;
using ServerResources = XSpect.MetaTweet.Properties.Resources;

namespace XSpect.MetaTweet.Modules
{
    [CLSCompliant(false)]
    public class RequestHandler
        : HttpModule
    {
        public HttpServant Servant
        {
            get;
            private set;
        }

        public RequestHandler(HttpServant parent)
        {
            this.Servant = parent;
        }

        public override Boolean Process(IHttpRequest request, IHttpResponse response, IHttpSession session)
        {
            if (request.UriPath.StartsWith("/!") || request.UriPath.StartsWith("/$"))
            {
                StreamWriter writer = new StreamWriter(response.Body, Encoding.UTF8);
                String ret;
                try
                {
                    ret = this.Servant.Host.RequestManager.Execute<String>(Request.Parse(request.UriPath.UriDecode()));
                    if (ret == null)
                    {
                        ret = "(No return data)";
                    }
                    response.ContentType = ret.StartsWith("<?")
                        ? "application/xml"
                        : ret.StartsWith("{")
                              ? "application/json"
                              : "text/plain";
                    writer.Write(ret);
                }
                catch (Exception ex)
                {
                    response.Status = HttpStatusCode.InternalServerError;
                    response.ContentType = "text/plain";
                    writer.WriteLine("ERROR: " + ex);
                }
                finally
                {
                    writer.Flush();
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}