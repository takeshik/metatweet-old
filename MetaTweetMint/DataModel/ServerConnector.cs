// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetMint
 *   Extensible GUI client for MetaTweet
 *   Part of MetaTweet
 * Copyright © 2009-2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
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
using XSpect.Configuration;

namespace XSpect.MetaTweet.Clients.Mint.DataModel
{
    public class ServerConnector
        : Object
    {
        private IChannel _channel;

        public XmlConfiguration.Entry<ServerConnectorConfiguration> Configuration
        {
            get;
            private set;
        }

        public String Name
        {
            get
            {
                return this.Configuration.Value.Name;
            }
            private set
            {
                this.Configuration.Value.Name = value;
            }
        }

        public Type ChannelType
        {
            get
            {
                return Type.GetType(this.Configuration.Value.ChannelType);
            }
            set
            {
                this.Configuration.Value.ChannelType = value.AssemblyQualifiedName;
            }
        }

        public String Address
        {
            get
            {
                return this.Configuration.Value.Address;
            }
            set
            {
                this.Configuration.Value.Address = value;
            }
        }

        public String EndpointName
        {
            get
            {
                return this.Configuration.Value.EndpointName;
            }
            set
            {
                this.Configuration.Value.EndpointName = value;
            }
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

        private ServerConnector()
        {
            this.Views = new HybridDictionary<String, ObjectView>((i, v) => v.Name);
        }

        public ServerConnector(XmlConfiguration parent, String name)
            : this()
        {
            this.Configuration = new XmlConfiguration.Entry<ServerConnectorConfiguration>(parent);
            this.Name = name;
        }

        public ServerConnector(XmlConfiguration.Entry<ServerConnectorConfiguration> configuration)
            : this()
        {
            this.Configuration = configuration;
            // TODO: Load configurations for Views
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