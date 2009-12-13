// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetMint
 *   Extensible GUI client for MetaTweet
 *   Part of MetaTweet
 * Copyright c 2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetMint.
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
using XSpect.Collections;

namespace XSpect.MetaTweet.Clients.Mint
{
    public class ServerConnector
        : Object
    {
        private IChannel _channel;

        public String Name
        {
            get;
            private set;
        }

        public Boolean IsConnected
        {
            get
            {
                return this.Host != null;
            }
        }

        public IChannel Channel
        {
            get
            {
                return this._channel;
            }
            set
            {
                if (this.IsConnected)
                {
                    throw new InvalidOperationException("Channel is now used.");
                }
                this._channel = value;
            }
        }

        public ServerCore Host
        {
            get;
            private set;
        }

        public HybridDictionary<String, ObjectView> Views
        {
            get;
            private set;
        }

        public ServerConnector(String name)
        {
            this.Name = name;
            this.Views = new HybridDictionary<String, ObjectView>((i, v) => v.Name);
        }

        public void Connect(Uri uri)
        {
            if (!this.IsConnected)
            {
                ChannelServices.RegisterChannel(this._channel, true);
                RemotingConfiguration.RegisterWellKnownClientType(typeof(ServerCore), uri.ToString());
                this.Host = Activator.CreateInstance<ServerCore>();
            }
        }

        public void Disconnect()
        {
            if (this.IsConnected)
            {
                this.Host = null;
                ChannelServices.UnregisterChannel(this._channel);
            }
        }
    }
}