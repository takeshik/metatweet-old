// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetObjectModel
 *   Object model and Storage interface for MetaTweet and other systems
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetObjectModel.
 * 
 * This library is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation; either version 3 of the License, or (at your
 * option) any later version.
 * 
 * This library is distributed in the hope that it will be useful, but
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

namespace XSpect.MetaTweet.Objects
{
    public abstract class Storage
        : MarshalByRefObject,
          IDisposable
    {
        private readonly Dictionary<Guid, StorageSession> _sessions;

        public Timeline Timeline
        {
            get;
            private set;
        }

        public event EventHandler<StorageObjectSequenceEventArgs> Queried;

        public event EventHandler<StorageObjectSequenceEventArgs> Loaded;

        public event EventHandler<StorageObjectEventArgs> Created;

        public event EventHandler<StorageObjectEventArgs> Deleted;

        public event EventHandler<EventArgs> Updated;

        protected Storage()
        {
            this._sessions = new Dictionary<Guid, StorageSession>();
            this.Timeline = new Timeline();
        }

        public override Object InitializeLifetimeService()
        {
            return null;
        }

        public void Dispose()
        {
            foreach (StorageSession session in this._sessions.Values)
            {
                session.Dispose();
            }
            this._sessions.Clear();
        }

        public abstract void Initialize(IDictionary<String, Object> connectionSettings);

        protected abstract StorageSession InitializeSession();

        public virtual StorageSession OpenSession()
        {
            StorageSession session = this.InitializeSession();
            this._sessions.Add(session.Id, session);
            session.Queried += this.Queried;
            session.Loaded += this.Loaded;
            session.Created += this.Created;
            session.Deleted += this.Deleted;
            session.Updated += this.Updated;
            return session;
        }

        public virtual void CloseSession(Guid id)
        {
            this._sessions.Remove(id);
        }
    }
}