// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * HttpServant
 *   MetaTweet Servant which provides HTTP service
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
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

using System.IO;
using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using HttpServer;
using System;
using HttpServer.Mvc;
using HttpServer.ViewEngine.Spark;
using XSpect.Extension;
using HttpListener = HttpServer.HttpListener;
using HttpServer.Routing;

namespace XSpect.MetaTweet.Modules
{
    public class HttpServant
        : ServantModule
    {
        private readonly MvcServer _server;

        public HttpServant()
        {
            this._server = new MvcServer()
            {
                ServerName = String.Format(
                    "MetaTweet/{0} ({1}) HttpServant/{2}",
                    ThisAssembly.EntireVersion,
                    Environment.OSVersion.Platform,
                    ThisAssembly.CombinedShortVersionInfo
                ),
            };
        }

        protected override void ConfigureImpl(FileInfo configFile)
        {
            base.ConfigureImpl(configFile);

            this._server.Add(String.IsNullOrEmpty(this.Configuration.CertificationFile)
                ? HttpListener.Create(
                      IPAddress.Parse(this.Configuration.ListenAddress),
                      this.Configuration.ListenPort
                  )
                : HttpListener.Create(
                      IPAddress.Parse(this.Configuration.ListenAddress),
                      this.Configuration.ListenPort,
                      X509Certificate.CreateFromCertFile(this.Configuration.CertificationFile)
                  )
            );
            this._server.ViewEngines.Add(new SparkEngine());
            this._server.Add(new SimpleRouter("/", "/view/"));
            this._server.Add(new RequestHandler(this));
            BootStrapper bootStrapper = new BootStrapper(this._server);
            bootStrapper.LoadEmbeddedViews(typeof(HttpServant).Assembly);
            bootStrapper.LoadControllers(typeof(HttpServant).Assembly);
        }

        protected override void StartImpl()
        {
            this._server.Start(8);
        }

        protected override void StopImpl()
        {
        }
    }
}
