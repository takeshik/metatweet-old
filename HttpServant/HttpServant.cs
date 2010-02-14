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
using Achiral;
using Achiral.Extension;
using XSpect.Extension;
using XSpect.MetaTweet.Modules;
using System.Timers;

namespace XSpect.MetaTweet.Modules
{
    public class HttpServant
        : ServantModule
    {
        private readonly HttpListener _listener;

        public HttpServant()
        {
            this._listener = new HttpListener();
        }

        protected override void InitializeImpl()
        {
            base.InitializeImpl();
        }

        protected override void StartImpl()
        {
            this._listener.Start();
        }

        protected override void StopImpl()
        {
            _listener.Stop();
        }
    }
}
