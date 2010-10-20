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
using System.Net;
using System.Linq;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Achiral;
using Achiral.Extension;
using HttpServer;
using HttpServer.Helpers;
using HttpServer.MVC;
using HttpServer.MVC.Rendering;
using HttpServer.MVC.Rendering.Haml;
using H = HttpServer;
using HttpServer.HttpModules;
using XSpect.Extension;
using ServerResources = XSpect.MetaTweet.Properties.Resources;

namespace XSpect.MetaTweet.Modules
{
    public class HttpServant
        : ServantModule
    {
        private H.HttpServer _server;

        public HttpServant()
        {
        }

        protected override void InitializeImpl()
        {
            this._server = new H.HttpServer()
            {
                ServerName = String.Format(
                    "MetaTweet/{0} ({1}) HttpServant/{2}",
                    ThisAssembly.EntireVersion,
                    Environment.OSVersion.Platform,
                    ThisAssembly.FileVersion
                ),
            }.Apply(
                s => s.ExceptionThrown += (sender, e) => this.Log.Fatal("Unhandled exception occured.", e),
                s => s.Add(new ControllerModule().Apply(
                    c => c.Add(new DefaultController(
                        new TemplateManager(
                            new ResourceTemplateLoader().Apply(
                                l => l.LoadTemplates("/", Assembly.GetExecutingAssembly(), "XSpect.MetaTweet.Modules.Resources.Templates")
                            )
                        ).Apply(
                            m => m.AddType(typeof(WebHelper)),
                            m => m.AddType(typeof(Helper)),
                            m => m.Add("haml", new HamlGenerator())
                        ),
                        this
                    ))
                )),
                s => s.Add(new ResourceFileModule().Apply(
                    m => m.AddResources("/", Assembly.GetExecutingAssembly(), "XSpect.MetaTweet.Modules.Resources.Documents")
                )),
                s => s.Add(new RequestHandler(this))
            );
            base.InitializeImpl();
        }

        protected override void StartImpl()
        {
            this._server.Start(
                IPAddress.Parse(this.Configuration.ResolveValue<String>("listenAddress")),
                this.Configuration.ResolveValue<Int32>("listenPort"),
                "certificationFile".Let(
                    _ => this.Configuration.Exists(_)
                        ? this.Configuration.ResolveValue<String>(_).If(
                              String.IsNullOrEmpty, s => null, X509Certificate.CreateFromCertFile
                          )
                        : null
                ),
                SslProtocols.Tls,
                null,
                false
            );
        }

        protected override void StopImpl()
        {
            this._server.Stop();
        }
    }
}
