// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * RavenLightweightStorage
 *   MetaTweet storage which is provided by Raven Document Database (lightweight client)
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
using System.Linq;
using System.Reflection;
using Raven.Client;
using Raven.Client.Document;

namespace XSpect.MetaTweet.Objects
{
    public class RavenLightweightStorageSession
        : StorageSession
    {
        private readonly IDocumentSession _session;

        public RavenLightweightStorageSession(RavenLightweightStorage parent, IDocumentSession session)
            : base(parent)
        {
            this._session = session;
            this._session.Advanced.MaxNumberOfRequestsPerSession = Int32.MaxValue;
        }

        protected override IQueryable<TObject> QueryObjects<TObject>()
        {
            return this._session.Query<TObject>();
        }

        protected override TObject LoadObject<TObject>(IStorageObjectId<TObject> id)
        {
            return this._session.Load<TObject>(id.ToString());
        }

        protected override ICollection<TObject> LoadObjects<TObject>(IEnumerable<IStorageObjectId<TObject>> ids)
        {
            return this._session.Load<TObject>(ids.Select(i => i.ToString()));
        }

        protected override void LoadObjects(Account account)
        {
            foreach(Activity activity in this.QueryObjects<Activity>().Where(a => a.AccountId == account.Id))
            {
                account.Activities.Add(activity);
            }
        }

        protected override void LoadObjects(Activity activity)
        {
            activity.Account = this.LoadObject(activity.AccountId);
            foreach (Advertisement advertisement in this.QueryObjects<Advertisement>().Where(a => a.ActivityId == activity.Id))
            {
                activity.Advertisements.Add(advertisement);
            }
        }

        protected override void LoadObjects(Advertisement advertisement)
        {
            advertisement.Activity = this.LoadObject(advertisement.ActivityId);
        }

        protected override void StoreObject<TObject>(TObject obj)
        {
            this._session.Store(obj);
        }

        protected override void DeleteObject<TObject>(TObject obj)
        {
            this._session.Delete(obj);
        }

        public override void Clean()
        {
            this._session.Advanced.Clear();
            base.Clean();
        }

        protected override void SaveChanges()
        {
            foreach (StorageObject obj in this.AddingObjects.Values)
            {
                this._session.Store(obj);
            }
            this._session.SaveChanges();
            // HACK: Reset NumberOfRequest counter
            typeof(InMemoryDocumentSessionOperations)
                .GetField("<NumberOfRequests>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(this._session, 0);
        }
    }
}