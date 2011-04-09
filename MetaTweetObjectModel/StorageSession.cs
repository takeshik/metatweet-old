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
using System.Disposables;
using System.Linq;
using System.Transactions;

namespace XSpect.MetaTweet.Objects
{
    public abstract class StorageSession
        : MarshalByRefObject,
          IDisposable
    {
        private readonly Object _updateLock;

        private readonly RefCountDisposable _disposer;

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

        public event EventHandler<StorageObjectEventArgs> Queried;

        public event EventHandler<StorageObjectEventArgs> Loaded;

        public event EventHandler<StorageObjectEventArgs> Created;

        public event EventHandler<StorageObjectEventArgs> Deleted;

        public event EventHandler<StorageObjectEventArgs> Updated;

        protected StorageSession(Storage parent)
        {
            this.Parent = parent;
            this._disposer = new RefCountDisposable(Disposable.Create(this._Dispose));
            this._updateLock = new Object();
            this.Id = this.Parent.GenerateId();
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

        protected abstract ICollection<TObject> LoadObjects<TObject>(IEnumerable<IStorageObjectId<TObject>> ids)
            where TObject : StorageObject;

        protected abstract void LoadObjects(Account account);

        protected abstract void LoadObjects(Activity activity);

        protected abstract void LoadObjects(Advertisement advertisement);

        protected abstract void StoreObject<TObject>(TObject obj)
            where TObject : StorageObject;

        protected abstract void DeleteObject<TObject>(TObject obj)
            where TObject : StorageObject;

        protected abstract void SaveChanges();

        #endregion

        protected virtual void Dispose(Boolean disposing)
        {
            this._disposer.Dispose();
        }

        private void _Dispose()
        {
            this.AddingObjects.Clear();
            this.Parent.CloseSession(this.Id);
            this.IsDisposed = true;
        }

        protected virtual void OnQueried(IStorageObjectQuery query, ICollection<StorageObject> result)
        {
            if (this.Queried != null)
            {
                this.Queried(this, new StorageObjectEventArgs(this.Id, query.ToString(), result));
            }
        }

        protected virtual void OnLoaded(IEnumerable<IStorageObjectId> ids, ICollection<StorageObject> result)
        {
            if(this.Loaded != null)
            {
                this.Loaded(this, new StorageObjectEventArgs(
                    this.Id,
                    String.Join(Environment.NewLine, ids.Select(i => i.HexString)),
                    result
                ));
            }
        }

        protected virtual void OnCreated(ICollection<StorageObject> result)
        {
            foreach (Advertisement advertisement in result.OfType<Advertisement>())
            {
                advertisement.Activity.UpdateLastAdvertisement(advertisement);
            }
            if(this.Created != null)
            {
                this.Created(this, new StorageObjectEventArgs(
                    this.Id,
                    String.Join(Environment.NewLine, result.Select(o => o.ToString())),
                    result
                ));
            }
        }

        protected virtual void OnDeleted(ICollection<StorageObject> objects)
        {
            if (this.Deleted != null)
            {
                this.Deleted(this, new StorageObjectEventArgs(
                    this.Id,
                    String.Join(Environment.NewLine, objects.Select(o => o.ToString())),
                    objects
                ));
            }
        }

        protected virtual void OnUpdated()
        {
            if(this.Updated != null)
            {
                this.Updated(this, new StorageObjectEventArgs(this.Id, null, null));
            }
        }

        public virtual ICollection<TObject> Query<TObject>(IStorageObjectQuery<TObject> query)
            where TObject : StorageObject
        {
            ICollection<TObject> result = query.Evaluate(this.QueryObjects<TObject>());
            foreach (TObject obj in result)
            {
                obj.Context = this;
            }
            result = result
                .Concat(query.Evaluate(this.AddingObjects.OfType<TObject>().AsQueryable()))
                .ToArray();
            this.OnQueried(query, (ICollection<StorageObject>) result);
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
            this.OnLoaded(new IStorageObjectId<TObject>[] { id, }, result != null
                ? new StorageObject[] { result, }
                : new StorageObject[0]
            );
            return (TObject) result;
        }

        protected virtual ICollection<TObject> Load<TObject>(IEnumerable<IStorageObjectId<TObject>> ids)
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
            TObject[] resultArray = result.ToArray();
            this.OnLoaded(ids, resultArray);
            return resultArray;
        }

        public virtual void Load(Account account)
        {
            if (!account.IsTemporary)
            {
                this.LoadObjects(account);
            }
            foreach (Activity activity in account.Activities.Where(a => a.Context != this))
            {
                activity.Context = this;
            }
            foreach (Activity activity in this.AddingObjects
                .OfType<Activity>()
                .Where(a => a.AccountId == account.Id && !account.Activities.Contains(a))
            )
            {
                account.Activities.Add(activity);
            }
        }

        public virtual void Load(Activity activity)
        {
            activity.Account.Context = this;
            if (!activity.IsTemporary)
            {
                this.LoadObjects(activity);
            }
            foreach (Advertisement advertisement in activity.Advertisements.Where(a => a.Context != this))
            {
                advertisement.Context = this;
            }
            foreach (Advertisement advertisement in this.AddingObjects
                .OfType<Advertisement>()
                .Where(a => a.ActivityId == activity.Id && !activity.Advertisements.Contains(a))
            )
            {
                activity.Advertisements.Add(advertisement);
            }
        }

        public virtual void Load(Advertisement advertisement)
        {
            this.LoadObjects(advertisement);
            advertisement.Activity.Context = this;
        }

        public ICollection<Account> Load(IEnumerable<AccountId> ids)
        {
            return this.Load(ids.Cast<IStorageObjectId<Account>>());
        }

        public ICollection<Activity> Load(IEnumerable<ActivityId> ids)
        {
            return this.Load(ids.Cast<IStorageObjectId<Activity>>());
        }

        public ICollection<Advertisement> Load(IEnumerable<AdvertisementId> ids)
        {
            return this.Load(ids.Cast<IStorageObjectId<Advertisement>>());
        }

        protected virtual Tuple<Account, Boolean> CreateObject(AccountCreationData data)
        {
            StorageObject obj;
            if ((obj = this.Load(data.Id)) == null && !this.AddingObjects.TryGetValue(data.Id, out obj))
            {
                obj = Account.Create(data);
                this.AddingObjects.Add(obj.ObjectId, obj);
                obj.Context = this;
                return Tuple.Create((Account) obj, true);
            }
            if (obj.Context == null || obj.Context.IsDisposed)
            {
                obj.Context = this;
            }
            return Tuple.Create((Account) obj, false);
        }

        protected virtual Tuple<Activity, Boolean> CreateObject(ActivityCreationData data)
        {
            StorageObject obj;
            Tuple<Activity, Boolean> ret;
            if ((obj = this.Load(data.Id)) == null && !this.AddingObjects.TryGetValue(data.Id, out obj))
            {
                obj = Activity.Create(data);
                this.AddingObjects.Add(obj.ObjectId, obj);
                obj.Context = this;
                ret = Tuple.Create((Activity) obj, true);
            }
            else
            {
                if (obj.Context == null)
                {
                    obj.Context = this;
                }
                ret = Tuple.Create((Activity) obj, false);
            }
            if (data.GetAccount(this).Activities.Contains(ret.Item1))
            {
                data.Account.Activities.Add(ret.Item1);
            }
            return ret;
        }

        protected virtual Tuple<Advertisement, Boolean> CreateObject(AdvertisementCreationData data)
        {
            StorageObject obj;
            Tuple<Advertisement, Boolean> ret;
            if ((obj = this.Load(data.Id)) == null && !this.AddingObjects.TryGetValue(data.Id, out obj))
            {
                obj = Advertisement.Create(data);
                this.AddingObjects.Add(obj.ObjectId, obj);
                obj.Context = this;
                ret = Tuple.Create((Advertisement) obj, true);
            }
            else
            {
                if (obj.Context == null || obj.Context.IsDisposed)
                {
                    obj.Context = this;
                }
                ret = Tuple.Create((Advertisement) obj, false);
            }
            if (data.GetActivity(this).Advertisements.Contains(ret.Item1))
            {
                data.Activity.Advertisements.Add(ret.Item1);
            }
            return ret;
        }

        public ICollection<Account> Create(IEnumerable<AccountCreationData> data)
        {
            IEnumerable<Tuple<Account, Boolean>> results = data.Select(this.CreateObject);
            this.OnCreated(results.Where(t => t.Item2).Select(t => t.Item1).ToArray());
            return results.Select(t => t.Item1).ToArray();
        }

        public ICollection<Account> Create(params AccountCreationData[] data)
        {
            return this.Create((IEnumerable<AccountCreationData>) data);
        }

        public Account Create(String realm, String seed)
        {
            Tuple<Account, Boolean> result = this.CreateObject(StorageObjectCreationData.Create(realm, seed));
            if (result.Item2)
            {
                this.OnCreated(new Account[] { result.Item1, });
            }
            return result.Item1;
        }

        public ICollection<Activity> Create(IEnumerable<ActivityCreationData> data)
        {
            IEnumerable<Tuple<Activity, Boolean>> results = data.Select(this.CreateObject);
            this.OnCreated(results.Where(t => t.Item2).Select(t => t.Item1).ToArray());
            return results.Select(t => t.Item1).ToArray();
        }

        public ICollection<Activity> Create(params ActivityCreationData[] data)
        {
            return this.Create((IEnumerable<ActivityCreationData>) data);
        }

        public Activity Create(Account account, IEnumerable<ActivityId> ancestorIds, String name, Object value)
        {
            Tuple<Activity, Boolean> result = this.CreateObject(StorageObjectCreationData.Create(account, ancestorIds, name, value));
            if (result.Item2)
            {
                this.OnCreated(new Activity[] { result.Item1, });
            }
            return result.Item1;
        }

        public ICollection<Advertisement> Create(IEnumerable<AdvertisementCreationData> data)
        {
            IEnumerable<Tuple<Advertisement, Boolean>> results = data.Select(this.CreateObject);
            this.OnCreated(results.Where(t => t.Item2).Select(t => t.Item1).ToArray());
            return results.Select(t => t.Item1).ToArray();
        }

        public ICollection<Advertisement> Create(params AdvertisementCreationData[] data)
        {
            return this.Create((IEnumerable<AdvertisementCreationData>) data);
        }

        public Advertisement Create(Activity activity, DateTime timestamp, AdvertisementFlags flags)
        {
            Tuple<Advertisement, Boolean> result = this.CreateObject(StorageObjectCreationData.Create(activity, timestamp, flags));
            if (result.Item2)
            {
                this.OnCreated(new Advertisement[] { result.Item1, });
            }
            return result.Item1;
        }

        public ICollection<StorageObject> Create(IEnumerable<StorageObjectCreationData> data)
        {
            IEnumerable<Tuple<StorageObject, Boolean>> results = data.Select(d =>
            {
                if (d is AccountCreationData)
                {
                    Tuple<Account, Boolean> ret = this.CreateObject((AccountCreationData) d);
                    return Tuple.Create((StorageObject) ret.Item1, ret.Item2);
                }
                if (d is ActivityCreationData)
                {
                    Tuple<Activity, Boolean> ret = this.CreateObject((ActivityCreationData) d);
                    return Tuple.Create((StorageObject) ret.Item1, ret.Item2);
                }
                else
                {
                    Tuple<Advertisement, Boolean> ret = this.CreateObject((AdvertisementCreationData) d);
                    return Tuple.Create((StorageObject) ret.Item1, ret.Item2);
                }
            });
            this.OnCreated(results.Where(t => t.Item2).Select(t => t.Item1).ToArray());
            return results.Select(t => t.Item1).ToArray();
        }

        public ICollection<StorageObject> Create(params StorageObjectCreationData[] data)
        {
            return this.Create((IEnumerable<StorageObjectCreationData>) data);
        }

        public virtual void Store<TObject>(TObject obj)
            where TObject : StorageObject
        {
            if (!this.AddingObjects.ContainsKey(obj.ObjectId))
            {
                this.StoreObject(obj);
            }
        }

        public virtual void Delete<TObject>(IEnumerable<TObject> objects)
            where TObject : StorageObject
        {
            TObject[] targets = objects.Where(o => !this.AddingObjects.Remove(o.ObjectId)).ToArray();
            foreach (TObject obj in targets)
            {
                this.DeleteObject(obj);
            }
            this.OnDeleted(targets);
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

        public IDisposable SuppressDispose()
        {
            return this._disposer.GetDisposable();
        }
    }
}