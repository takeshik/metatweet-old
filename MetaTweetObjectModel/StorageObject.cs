﻿// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
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
using System.Runtime.Serialization;

namespace XSpect.MetaTweet.Objects
{
    [Serializable()]
    [DataContract()]
    [KnownType(typeof(Account))]
    [KnownType(typeof(Activity))]
    [KnownType(typeof(Advertisement))]
    public abstract class StorageObject
        : IComparable<StorageObject>,
          IEquatable<StorageObject>
    {
        private StorageSession _context;

        public abstract IStorageObjectId ObjectId
        {
            get;
        }

        public StorageObjectTypes ObjectType
        {
            get
            {
                return this.ObjectId.ObjectType;
            }
        }

        public Boolean IsTemporary
        {
            get
            {
                return this.Context == null || this.Context.AddingObjects.ContainsKey(this.ObjectId);
            }
        }

        public Boolean IsLoaded
        {
            get;
            protected set;
        }

        public virtual StorageSession Context
        {
            get
            {
                return this._context == null || this._context.IsDisposed
                    ? null
                    : this._context;
            }
            set
            {
                this._context = value;
            }
        }

        public Int32 CompareTo(StorageObject other)
        {
            switch (this.ObjectType)
            {
                case StorageObjectTypes.Account:
                    return Account.Compare((Account) this, other as Account);
                case StorageObjectTypes.Activity:
                    return Activity.Compare((Activity) this, other as Activity);
                default: // case StorageObjectTypes.Advertisement:
                    return Advertisement.Compare((Advertisement) this, other as Advertisement);
            }
        }

        public Boolean Equals(StorageObject other)
        {
            return this.ObjectId.Equals(other.ObjectId);
        }

        public void Load()
        {
            if (!this.IsLoaded)
            {
                switch (this.ObjectType)
                {
                    case StorageObjectTypes.Account:
                        this.Context.Load((Account) this);
                        break;
                    case StorageObjectTypes.Activity:
                        this.Context.Load((Activity) this);
                        break;
                    default: // case StorageObjectTypes.Advertisement:
                        this.Context.Load((Advertisement) this);
                        break;
                }
                this.IsLoaded = true;
            }
        }
    }
}