// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
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
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using XSpect.MetaTweet.Modules;

namespace XSpect.MetaTweet.Modules
{
    public class RemotingIpcServant
        : ServantModule
    {
        private String _portName;

        private IpcServerChannel _channel;

        public override void Initialize()
        {
            this._portName = this.Configuration.ResolveValue<String>("portName");
            base.Initialize();
        }

        protected override void StartImpl()
        {

            this._channel = new IpcServerChannel(new Dictionary<Object, Object>()
            {
                {"name", String.Empty},
                {"secure", true},
                {"portName", this._portName},
            }, new BinaryServerFormatterSinkProvider()
            {
                TypeFilterLevel = TypeFilterLevel.Full,
            });
            ChannelServices.RegisterChannel(this._channel, false);
            RemotingServices.Marshal(this.Host, String.Empty, typeof(ServerCore));
        }

        protected override void StopImpl()
        {
            ChannelServices.UnregisterChannel(this._channel);
            this._channel = null;
        }
    }
}