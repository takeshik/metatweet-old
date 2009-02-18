// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetClient
 *   Bandled GUI client for MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetClient.
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
using System.Threading;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
using System.Collections.Generic;
using XSpect.MetaTweet.ObjectModel;
using System.Linq;
using XSpect.MetaTweet.Modules;

namespace XSpect.MetaTweet.Clients
{
    public class MetaTweetClient
        : Object
    {
        private TcpClientChannel _channel = new TcpClientChannel();
        ServerCore _host;

        public ServerCore Host
        {
            get
            {
                return this._host;
            }
        }

        public void Connect()
        {
            ChannelServices.RegisterChannel(this._channel, false);
            RemotingConfiguration.RegisterWellKnownClientType(typeof(ServerCore), "tcp://localhost:7784/MetaTweet");
            this._host = new ServerCore();
        }

        public void Disconnect()
        {
            this._host = null;
            ChannelServices.UnregisterChannel(this._channel);
        }

        public List<Post> GetFriendsTimeLine(DateTime since)
        {
            var s = this.Host.ModuleManager.GetModule<StorageModule>("main");
            var t = this.Host.ModuleManager.GetModule<InputFlowModule>("twitter");
            try
            {
                return t.Input("/statuses/friends_timeline", s, new Dictionary<String, String>()
                {
                    {"count", "100"},
                    {"since", since.ToString("R")},
                }).Cast<Post>().ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType().FullName);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine();
                if (e.InnerException != null)
                {
                    Console.WriteLine(e.InnerException.GetType().FullName);
                    Console.WriteLine(e.InnerException.Message);
                    Console.WriteLine(e.InnerException.StackTrace);
                }
                return new List<Post>();
            }
        }

        public List<Post> Update(String text)
        {
            try
            {
                var s = this.Host.ModuleManager.GetModule<StorageModule>("main");
                var t = this.Host.ModuleManager.GetModule<InputFlowModule>("twitter");
                return t.Input("/statuses/update", s, new Dictionary<String, String>()
                {
                    {"status", text},
                    {"source", "metatweet"},
                }).Cast<Post>().ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType().FullName);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine();
                if (e.InnerException != null)
                {
                    Console.WriteLine(e.InnerException.GetType().FullName);
                    Console.WriteLine(e.InnerException.Message);
                    Console.WriteLine(e.InnerException.StackTrace);
                }
                return new List<Post>();
            }
        }
    }
}