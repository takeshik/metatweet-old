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

using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Reflection;
using Achiral;
using Achiral.Extension;
using HttpServer;
using HttpServer.MVC;
using HttpServer.MVC.Controllers;
using HttpServer.MVC.Rendering;
using H = HttpServer;
using HttpServer.HttpModules;
using XSpect.Codecs;
using XSpect.Extension;
using ServerResources = XSpect.MetaTweet.Properties.Resources;

namespace XSpect.MetaTweet.Modules
{
    [CLSCompliant(false)]
    [ControllerName("view")]
    public class DefaultController
        : ViewController
    {
        public HttpServant Servant
        {
            get;
            private set;
        }

        public DefaultController(TemplateManager manager, HttpServant parent)
            : base(manager)
        {
            this.Servant = parent;
        }

        public DefaultController(DefaultController controller)
            : base(controller)
        {
            this.Servant = controller.Servant;
        }

        public override Object Clone()
        {
            return new DefaultController(this);
        }

        public String Index()
        {
            return this.Render();
        }

        public String Query()
        {
            return this.Render();
        }
    }
}