// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * SQLiteStorage
 *   MetaTweet Storage module which is provided by SQLite3 RDBMS.
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of SQLiteStorage.
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
using System.Data.EntityClient;
using System.Data.Objects;
using System.Reflection;

namespace XSpect.MetaTweet.Objects
{
    public class StorageObjectContext
        : ObjectContext
    {
        public const String ContainerName = "StorageObjectContext";

        private ObjectSet<Account> _accounts;

        private ObjectSet<Activity> _activities;

        private ObjectSet<Advertisement> _advertisements;

        public StorageObjectContext(String connectionString)
            : base(connectionString, ContainerName)
        {
            this.ContextOptions.LazyLoadingEnabled = false;
            this.ContextOptions.ProxyCreationEnabled = false;
        }

        public StorageObjectContext(EntityConnection connection)
            : base(connection, ContainerName)
        {
            this.ContextOptions.LazyLoadingEnabled = false;
            this.ContextOptions.ProxyCreationEnabled = false;
        }

        public ObjectSet<Account> Accounts
        {
            get
            {
                return this._accounts ?? (this._accounts = this.CreateObjectSet<Account>("Accounts"));
            }
        }

        public ObjectSet<Activity> Activities
        {
            get
            {
                return this._activities ?? (this._activities = this.CreateObjectSet<Activity>("Activities"));
            }
        }

        public ObjectSet<Advertisement> Advertisements
        {
            get
            {
                return this._advertisements ?? (this._advertisements = this.CreateObjectSet<Advertisement>("Advertisements"));
            }
        }

        public Boolean IsDisposed
        {
            get
            {
                // HACK: Depends on internal structure, accessing non-public field
                return typeof(ObjectContext)
                    .GetField("_connection", BindingFlags.Instance | BindingFlags.NonPublic)
                    .GetValue(this) == null;
            }
        }

        public ObjectSet<TObject> GetObjectSet<TObject>()
            where TObject : StorageObject
        {
            return (ObjectSet<TObject>) (typeof(TObject) == typeof(Account)
                ? (Object) this.Accounts
                : typeof(TObject) == typeof(Activity)
                      ? (Object) this.Activities
                      : this.Advertisements
            );
        }

        public String GetEntitySetName<TObject>()
            where TObject : StorageObject
        {
            return typeof(TObject) == typeof(Account)
                ? ContainerName + ".Accounts"
                : typeof(TObject) == typeof(Activity)
                      ? ContainerName + ".Activities"
                      : ContainerName + ".Advertisements";
        }

        public String GetEntitySetName(StorageObject obj)
        {
            return obj is Account
                ? ContainerName + ".Accounts"
                : obj is Activity
                      ? ContainerName + ".Activities"
                      : ContainerName + ".Advertisements";
        }

    }
}
