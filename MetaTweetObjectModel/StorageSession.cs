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
using System.Linq;
using System.Transactions;

namespace XSpect.MetaTweet.Objects
{
    public abstract class StorageSession
        : MarshalByRefObject,
          IDisposable
    {
        private readonly Object _updateLock;

        public Guid Id
        {
            get;
            private set;
        }

        public Storage Parent
        {
            get;
            private set;
        }

        public Dictionary<IStorageObjectId, StorageObject> AddingObjects
        {
            get;
            private set;
        }

        public event EventHandler<ObjectGotEventArgs> Queried;

        public event EventHandler<ObjectGotEventArgs> Loaded;

        public event EventHandler<ObjectCreatedEventArgs> Created;

        public event EventHandler<EventArgs> Updated;

        protected StorageSession(Storage parent)
        {
            this.Parent = parent;
            this._updateLock = new Object();
            this.Id = Guid.NewGuid();
            this.AddingObjects = new Dictionary<IStorageObjectId, StorageObject>();
        }

        public override String ToString()
        {
            return this.Id.ToString("d");
        }

        public virtual void Dispose()
        {
            this.AddingObjects.Clear();
            this.Parent.CloseSession(this.Id);
        }

        #region Abstract Methods

        protected abstract IQueryable<TObject> QueryObjects<TObject>()
            where TObject : StorageObject;

        protected abstract TObject LoadObject<TObject>(IStorageObjectId<TObject> id)
            where TObject : StorageObject;

        protected abstract IEnumerable<TObject> LoadObjects<TObject>(IEnumerable<IStorageObjectId<TObject>> ids)
            where TObject : StorageObject;

        protected abstract void SaveChanges();

        #endregion

        protected virtual void OnQueried(IEnumerable<StorageObject> result)
        {
            foreach (Advertisement advertisement in result.OfType<Advertisement>())
            {
                this.Parent.Timeline.Update(advertisement);
            }
            if (this.Queried != null)
            {
                this.Queried(this, new ObjectGotEventArgs(result));
            }
        }

        protected virtual void OnLoaded(IEnumerable<StorageObject> result)
        {
            foreach (Advertisement advertisement in result.OfType<Advertisement>())
            {
                this.Parent.Timeline.Update(advertisement);
            }
            if(this.Loaded != null)
            {
                this.Loaded(this, new ObjectGotEventArgs(result));
            }
        }

        protected virtual void OnCreated(StorageObject obj)
        {
            if (obj is Advertisement)
            {
                this.Parent.Timeline.Update((Advertisement) obj);
            }
            if(this.Created != null)
            {
                this.Created(this, new ObjectCreatedEventArgs(obj));
            }
        }

        protected virtual void OnUpdated()
        {
            if(this.Updated != null)
            {
                this.Updated(this, new EventArgs());
            }
        }

        public virtual IEnumerable<TObject> Query<TObject>(IStorageObjectQuery<TObject> query)
            where TObject : StorageObject
        {
            IEnumerable<TObject> result = query.Evaluate(this.QueryObjects<TObject>())
                .AsEnumerable()
                .AsTransparent();
            foreach (TObject obj in result)
            {
                obj.Context = this;
            }
            result = result
                .Concat(query.Evaluate(this.AddingObjects.OfType<TObject>().AsQueryable()));
            this.OnQueried(result);
            return result;
        }

        public virtual IEnumerable<TObject> Query<TObject>()
            where TObject : StorageObject
        {
            return this.Query(new StorageObjectNullQuery<TObject>());
        }

        public virtual TObject Load<TObject>(IStorageObjectId<TObject> id)
            where TObject : StorageObject
        {
            StorageObject result;
            if (!this.AddingObjects.TryGetValue(id, out result))
            {
                result = this.LoadObject(id);
                if (result != null)
                {
                    result.Context = this;
                }
            }
            this.OnLoaded(result != null
                ? new StorageObject[] { result, }
                : Enumerable.Empty<StorageObject>()
            );
            return (TObject) result;
        }

        protected virtual IEnumerable<TObject> Load<TObject>(IEnumerable<IStorageObjectId<TObject>> ids)
            where TObject : StorageObject
        {
            IEnumerable<TObject> addings = ids
                .Where(this.AddingObjects.ContainsKey)
                .Select(i => this.AddingObjects[i])
                .Cast<TObject>();
            IEnumerable<TObject> result = this.LoadObjects(ids
                .Except(addings.Select(o => o.ObjectId))
                .Cast<IStorageObjectId<TObject>>()
            );
            foreach (TObject obj in result)
            {
                obj.Context = this;
            }
            this.OnLoaded(result.Concat(addings));
            return result;
        }

        public IEnumerable<Account> Load(IEnumerable<AccountId> ids)
        {
            return this.Load(ids.Cast<IStorageObjectId<Account>>());
        }

        public IEnumerable<Activity> Load(IEnumerable<ActivityId> ids)
        {
            return this.Load(ids.Cast<IStorageObjectId<Activity>>());
        }

        public IEnumerable<Advertisement> Load(IEnumerable<AdvertisementId> ids)
        {
            return this.Load(ids.Cast<IStorageObjectId<Advertisement>>());
        }

        public virtual Account Create(String realm, String seed)
        {
            Account account = Account.Create(realm, seed);
            account.Context = this;
            this.AddingObjects.Add(account.Id, account);
            this.OnCreated(account);
            return account;
        }

        public virtual Activity Create(AccountId accountId, IEnumerable<ActivityId> ancestorIds, String name, Object value)
        {
            Activity activity = Activity.Create(accountId, ancestorIds, name, value);
            activity.Context = this;
            this.AddingObjects.Add(activity.Id, activity);
            this.OnCreated(activity);
            return activity;
        }

        public virtual Advertisement Create(ActivityId activityId, DateTime timestamp, AdvertisementFlags flags)
        {
            Advertisement advertisement = Advertisement.Create(activityId, timestamp, flags);
            advertisement.Context = this;
            this.AddingObjects.Add(advertisement.Id, advertisement);
            this.OnCreated(advertisement);
            return advertisement;
        }

        public virtual void Update()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                lock (this._updateLock)
                {
                    this.SaveChanges();
                    this.AddingObjects.Clear();
                    scope.Complete();
                }
            }
            this.OnUpdated();
        }
    }

    #region EventArgs classes

    public sealed class ObjectGotEventArgs
        : EventArgs
    {
        public IEnumerable<StorageObject> Result
        {
            get;
            private set;
        }

        public ObjectGotEventArgs(IEnumerable<StorageObject> result)
        {
            this.Result = result;
        }
    }

    public sealed class ObjectCreatedEventArgs
        : EventArgs
    {
        public StorageObject Object
        {
            get;
            private set;
        }

        public ObjectCreatedEventArgs(StorageObject obj)
        {
            this.Object = obj;
        }
    }

    #endregion
}