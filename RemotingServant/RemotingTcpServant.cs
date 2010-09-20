// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * RemotingServant
 *   MetaTweet Servant module which provides .NET Remoting server
 *   Part of MetaTweet
 * Copyright © 2008-2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of RemotingServant.
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
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using XSpect.Extension;

namespace XSpect.MetaTweet.Modules
{
    public class RemotingTcpServant
        : ServantModule
    {
        private String _bindAddress;

        private Int32 _portNumber;

        private TcpServerChannel _channel;

        protected override void ConfigureImpl()
        {
            this._bindAddress = this.Configuration.ResolveValue<String>("bindAddress");
            this._portNumber = this.Configuration.ResolveValue<Int32>("portNumber");
            base.ConfigureImpl();
        }

        protected override void StartImpl()
        {
            this._channel = new TcpServerChannel(new Dictionary<Object, Object>()
            {
                {"name", "tcp server " + this},
                {"bindTo", this._bindAddress},
                {"port", this._portNumber},
                {"useIpAddress", false},
            }, new BinaryServerFormatterSinkProvider()
            {
                TypeFilterLevel = TypeFilterLevel.Full,
            });
            ChannelServices.RegisterChannel(this._channel, false);
            String uri = "tcp://localhost:" + this._portNumber + RemotingServices.Marshal(this.Host, "core", typeof(ServerCore)).URI;
            this.Log.Info("TCP Remoting URI is: {0}", uri);
            this.Host.Directories.RuntimeDirectory
                .File(this + ".uri")
                .WriteAllText(uri);
        }

        protected override void StopImpl()
        {
            ChannelServices.UnregisterChannel(this._channel);
            this._channel = null;
        }
    }
}