// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * RemotingServant
 *   MetaTweet Servant module which provides .NET Remoting server
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
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

namespace XSpect.MetaTweet
{
    public class RemotingServant
        : ServantModule
    {
        public const Int32 DefaultPortNumber = 7784;

        private Int32 _portNumber;

        private TcpServerChannel _channel;

        public override void Initialize(IDictionary<String, String> args)
        {
            this._portNumber = args.ContainsKey("port")
                ? Int32.Parse(args["port"])
                : DefaultPortNumber;
        }

        protected override void StartImpl()
        {

            this._channel = new TcpServerChannel(new Dictionary<Object, Object>()
            {
                {"port", this._portNumber},
                {"useIpAddress", false},
            }, new BinaryServerFormatterSinkProvider()
            {
                TypeFilterLevel = TypeFilterLevel.Full,
            });
            ChannelServices.RegisterChannel(this._channel, false);
            RemotingServices.Marshal(this.Host, "MetaTweet" , typeof(ServerCore));
        }

        protected override void StopImpl()
        {
            ChannelServices.UnregisterChannel(this._channel);
            this._channel = null;
        }

        protected override void PauseImpl()
        {
            this._channel.StopListening(null);
        }

        protected override void ContinueImpl()
        {
            this._channel.StartListening(null);
        }
    }
}