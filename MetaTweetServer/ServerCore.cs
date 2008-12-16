// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system of Twitter-like communication service
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008 Takeshi KIRIYA, XSpect Project <takeshik@xspect.org>
 * All rights reserved.
 * 
 * This file is part of MetaTweetServer.
 * 
 * This program is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License as published by the Free
 * Software Foundation; either version 3 of the License, or (at your option)
 * any later version.
 * 
 * This program is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
 * or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License
 * for more details. 
 * 
 * You should have received a copy of the GNU General Public License along
 * with this program. If not, see <http://www.gnu.org/licenses/>, or write to
 * the Free Software Foundation, Inc., 51 Franklin Street - Fifth Floor,
 * Boston, MA 02110-1301, USA.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Reflection;
using XSpect.Reflection;
using System.Threading;

namespace XSpect.MetaTweet
{
    public sealed class ServerCore
        : Object,
          IDisposable
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(ServerCore));

        private readonly AssemblyManager _assemblyManager = new AssemblyManager();

        private readonly Dictionary<String, Listener> _listeners = new Dictionary<String, Listener>();

		private readonly Dictionary<String, Realm> _realms = new Dictionary<String, Realm>();

        public void Start(IDictionary<String, String> arguments)
        {
        }

        public void Stop()
        {
        }

        public void StopGracefully()
        {
        }

        public void Pause()
        {
        }

        public void PauseGracefully()
        {
        }

        public void Resume()
        {
        }

        public void Dispose()
        {
        }

		public void AddListener(String id, Listener listener)
		{
			listener.Register(this, id);
			this._listeners.Add(id, listener);
		}

		public void RemoveListener(String id)
		{
			this._realms.Remove(id);
		}

		public void AddRealm(String id)
		{
			this._realms.Add(id, new Realm(this, id));
		}

		public void RemoveRealm(String id)
		{
			this._realms.Remove(id);
		}
    }
}