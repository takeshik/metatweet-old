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
using System.Linq.Expressions;
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

        public Boolean IsDisposed
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

        public event EventHandler<StorageObjectSequenceEventArgs> Queried;

        public event EventHandler<StorageObjectSequenceEventArgs> Loaded;

        public event EventHandler<StorageObjectEventArgs> Created;

        public event EventHandler<StorageObjectEventArgs> Deleted;

        public event EventHandler<EventArgs> Updated;

        protected StorageSession(Storage parent)
        {
            this.Parent = parent;
            this._updateLock = new Object();
            this.Id = Guid.NewGuid();
            this.AddingObjects = new Dictionary<IStorageObjectId, StorageObject>();
        }

        ~StorageSession()
        {
            this.Dispose(false);
        }

        public override String ToString()
        {
            return this.Id.ToString("d");
        }

        public override Object InitializeLifetimeService()
        {
            return null;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #region Abstract Methods

        protected abstract IQueryable<TObject> QueryObjects<TObject>()
            where TObject : StorageObject;

        protected abstract TObject LoadObject<TObject>(IStorageObjectId<TObject> id)
            where TObject : StorageObject;

        protected abstract IEnumerable<TObject> LoadObjects<TObject>(IEnumerable<IStorageObjectId<TObject>> ids)
            where TObject : StorageObject;

        protected abstract void StoreObject<TObject>(TObject obj)
            where TObject : StorageObject;

        protected abstract void DeleteObject<TObject>(TObject obj)
            where TObject : StorageObject;

        protected abstract void SaveChanges();

        #endregion

        #region Internal Queryings

        protected internal virtual IEnumerable<Activity> GetActivities(
            AccountId accountId = default(AccountId),
            String name = null,
            Object value = null,
            Nullable<ActivityId> parentId = null,
            Nullable<Int32> maxDepth = null
        )
        {
            return this.Query(StorageObjectExpressionQuery.Activity(
                new ActivityTuple()
                {
                    AccountId = accountId,
                    Name = name,
                    Value = value,
                },
                maxDepth != null
                    ? _ => _.Where(a => a.AncestorIds.Count >= maxDepth.Value)
                    : default(Expression<Func<IQueryable<Activity>, IQueryable<Activity>>>),
                parentId != null
                    ? ((Expression<Func<IQueryable<Activity>, IQueryable<Activity>>>) (_ =>
                          _.Where(a => a.AncestorIds[0] == parentId.Value)
                      ))
                    : null
            ));
        }

        protected internal virtual IEnumerable<Advertisement> GetAdvertisements(
            ActivityId activityId = default(ActivityId),
            Nullable<DateTime> maxTimestamp = null
        )
        {
            return this.Query(StorageObjectExpressionQuery.Advertisement(
                new AdvertisementTuple()
                {
                    ActivityId = activityId,
                },
                maxTimestamp != null
                    ? _ => _.Where(a => a.Timestamp <= maxTimestamp)
                    : default(Expression<Func<IQueryable<Advertisement>, IQueryable<Advertisement>>>)
            ));
        }

        #endregion

        protected virtual void Dispose(Boolean disposing)
        {
            this.AddingObjects.Clear();
            this.Parent.CloseSession(this.Id);
            this.IsDisposed = true;
        }

        protected virtual void OnQueried(IEnumerable<StorageObject> result)
        {
            foreach (Advertisement advertisement in result.OfType<Advertisement>())
            {
                this.Parent.Timeline.Update(advertisement);
            }
            if (this.Queried != null)
            {
                this.Queried(this, new StorageObjectSequenceEventArgs(result));
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
                this.Loaded(this, new StorageObjectSequenceEventArgs(result));
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
                this.Created(this, new StorageObjectEventArgs(obj));
            }
        }

        protected virtual void OnDeleted(StorageObject obj)
        {
            if (obj is Activity)
            {
                this.Parent.Timeline.Remove((Activity) obj);
            }
            else if (obj is Advertisement)
            {
                this.Parent.Timeline.Remove((Advertisement) obj);
            }
            if (this.Deleted != null)
            {
                this.Deleted(this, new StorageObjectEventArgs(obj));
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

        public IQueryable<TObject> Query<TObject>()
            where TObject : StorageObject
        {
            return this.QueryObjects<TObject>();
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
            AccountId id = AccountId.Create(realm, seed);
            StorageObject obj;
            if ((obj = this.Load(id)) == null && !this.AddingObjects.TryGetValue(id, out obj))
            {
                obj = Account.Create(realm, seed);
                this.AddingObjects.Add(obj.ObjectId, obj);
                obj.Context = this;
                this.OnCreated(obj);
            }
            else if (obj.Context == null || obj.Context.IsDisposed)
            {
                obj.Context = this;
            }
            return (Account) obj;
        }

        public virtual Activity Create(AccountId accountId, IEnumerable<ActivityId> ancestorIds, String name, Object value)
        {
            ActivityId id = ActivityId.Create(accountId, ancestorIds, name, value);
            StorageObject obj;
            if ((obj = this.Load(id)) == null && !this.AddingObjects.TryGetValue(id, out obj))
            {
                obj = Activity.Create(accountId, ancestorIds, name, value);
                this.AddingObjects.Add(obj.ObjectId, obj);
                obj.Context = this;
                this.OnCreated(obj);
            }
            else if (obj.Context == null || obj.Context.IsDisposed)
            {
                obj.Context = this;
            }
            return (Activity) obj;
        }

        public virtual Advertisement Create(ActivityId activityId, DateTime timestamp, AdvertisementFlags flags)
        {
            AdvertisementId id = AdvertisementId.Create(activityId, timestamp, flags);
            StorageObject obj;
            if ((obj = this.Load(id)) == null && !this.AddingObjects.TryGetValue(id, out obj))
            {
                obj = Advertisement.Create(activityId, timestamp, flags);
                this.AddingObjects.Add(obj.ObjectId, obj);
                obj.Context = this;
                this.OnCreated(obj);
            }
            else if (obj.Context == null || obj.Context.IsDisposed)
            {
                obj.Context = this;
            }
            return (Advertisement) obj;
        }

        public virtual void Store<TObject>(TObject obj)
            where TObject : StorageObject
        {
            if (!this.AddingObjects.ContainsKey(obj.ObjectId))
            {
                this.StoreObject(obj);
            }
        }

        public virtual void Delete<TObject>(TObject obj)
            where TObject : StorageObject
        {
            if(!this.AddingObjects.Remove(obj.ObjectId))
            {
                this.DeleteObject(obj);
            }
        }

        public virtual void Clean()
        {
            this.AddingObjects.Clear();
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

    public sealed class StorageObjectSequenceEventArgs
        : EventArgs
    {
        public IEnumerable<StorageObject> Objects
        {
            get;
            private set;
        }

        public StorageObjectSequenceEventArgs(IEnumerable<StorageObject> objects)
        {
            this.Objects = objects;
        }
    }

    public sealed class StorageObjectEventArgs
        : EventArgs
    {
        public StorageObject Object
        {
            get;
            private set;
        }

        public StorageObjectEventArgs(StorageObject obj)
        {
            this.Object = obj;
        }
    }

    #endregion
}