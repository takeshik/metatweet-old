// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetObjectModel
 *   Object model and Storage interface for MetaTweet and other systems
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
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
using System.ComponentModel;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;

namespace XSpect.MetaTweet.Objects
{
    [Serializable()]
    public abstract class StorageObject
        : EntityObject,
          IComparable<StorageObject>,
          ISupportInitialize
    {
        [NonSerialized()]
        private Storage _storage;

        [NonSerialized()]
        private Boolean _isInitializing;

        public Storage Storage
        {
            get
            {
                return this._storage;
            }
            set
            {
                if (this._storage != null)
                {
                    throw new InvalidOperationException("Storage is already set; this property is allowed to set only once.");
                }
                this._storage = value;
            }
        }

        protected Boolean IsInitializing
        {
            get
            {
                return this._isInitializing;
            }
        }

        protected StorageObject(Storage storage)
        {
            this.BeginInit();
            this.Storage = storage;
        }

        public abstract Int32 CompareTo(StorageObject other);

        public virtual void BeginInit()
        {
            this._isInitializing = true;
        }

        public virtual void EndInit()
        {
            this._isInitializing = false;
        }

        public void Delete()
        {
            this.Storage.Entities.DeleteObject(this);
        }

        public void Refresh(RefreshMode refreshMode)
        {
            this.Storage.Entities.Refresh(refreshMode, this);
        }
    }
}