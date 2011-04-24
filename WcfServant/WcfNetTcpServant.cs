// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * WcfServant
 *   MetaTweet Servant module which provides access to server objects via WCF
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of WcfServant.
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
using System.ServiceModel;

namespace XSpect.MetaTweet.Modules
{
    public class WcfNetTcpServant
        : ServantModule
    {
        public ServiceHost ServiceHost
        {
            get;
            private set;
        }

        public Uri ServiceEndpoint
        {
            get;
            private set;
        }

        protected override void InitializeImpl()
        {
            this.ServiceHost = new ServiceHost(this.Host);
            this.ServiceHost.AddServiceEndpoint(typeof(ServerCore), new NetTcpBinding(SecurityMode.Transport, true), this.ServiceEndpoint);
            base.InitializeImpl();
        }

        protected override void ConfigureImpl(System.IO.FileInfo configFile)
        {
            base.ConfigureImpl(configFile);
            this.ServiceEndpoint = new Uri("net.tcp://" + this.Configuration.Endpoint);
        }

        protected override void StartImpl()
        {
            this.ServiceHost.Open();
        }

        protected override void StopImpl()
        {
            this.ServiceHost.Close();
        }
    }
}