// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * SqlServerStorage
 *   MetaTweet Storage module which is provided by Microsoft SQL Server RDBMS.
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of SqlServerStorage.
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
using System.Data;
using System.Linq;

namespace XSpect.MetaTweet.Objects
{
    public class SqlServerStorageSession
        : StorageSession
    {
        private readonly StorageObjectContext _context;

        public SqlServerStorageSession(Storage parent, StorageObjectContext context)
            : base(parent)
        {
            this._context = context;
            this._context.MetadataWorkspace.LoadFromAssembly(typeof(Account).Assembly);
            this._context.Connection.Open();
        }

        protected override void Dispose(Boolean disposing)
        {
            this._context.Connection.Dispose();
            this._context.Dispose();
            base.Dispose(disposing);
        }

        protected override IQueryable<TObject> QueryObjects<TObject>()
        {
            return this._context.GetObjectSet<TObject>();
        }

        protected override TObject LoadObject<TObject>(IStorageObjectId<TObject> id)
        {
            Object value;
            return this._context.TryGetObjectByKey(
                new EntityKey(this._context.GetEntitySetName<TObject>(), "IdString", id.HexString),
                out value
            )
                ? (TObject) value
                : null;
        }

        protected override IEnumerable<TObject> LoadObjects<TObject>(IEnumerable<IStorageObjectId<TObject>> ids)
        {
            return ids.Select(this.LoadObject);
        }

        protected override void StoreObject<TObject>(TObject obj)
        {
            this._context.ApplyCurrentValues(this._context.GetEntitySetName<TObject>(), obj);
        }

        protected override void DeleteObject<TObject>(TObject obj)
        {
            this._context.DeleteObject(obj);
        }

        protected override void SaveChanges()
        {
            this._context.SaveChanges();
        }

        protected override void OnCreated(StorageObject obj)
        {
            /*
            Account account;
            Activity activity;
            Advertisement advertisement;
            if ((account = obj as Account) != null)
            {
                this._context.Accounts.AddObject(account);
            }
            else if ((activity = obj as Activity) != null)
            {
                this._context.Activities.AddObject(activity);
            }
            else if ((advertisement = obj as Advertisement) != null)
            {
                this._context.Advertisements.AddObject(advertisement);
            }
            */
            this._context.AddObject(this._context.GetEntitySetName(obj), obj);
            base.OnCreated(obj);
        }
    }
}