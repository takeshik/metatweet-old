// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@xspect.org>
 * All rights reserved.
 * 
 * This file is part of MetaTweetServer.
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
using Achiral;

namespace XSpect.MetaTweet.ObjectModel
{
    [Serializable()]
    public abstract class StorageObject
        : Object
    {
        private Storage _storage;

        public Storage Storage
        {
            get
            {
                return this._storage;
            }
            set
            {
                this._storage = value;
            }
        }

        public abstract DataRow UnderlyingUntypedDataRow
        {
            get;
            set;
        }

        public virtual Boolean IsModified
        {
            get
            {
                return this.UnderlyingUntypedDataRow.RowState != DataRowState.Unchanged;
            }
        }

        public virtual Boolean IsStored
        {
            get
            {
                return this.UnderlyingUntypedDataRow != null;
            }
        }

        public void Delete()
        {
            this.OnDeleting();
            this.DeleteImpl();
            this.OnDeleted();
        }

        protected virtual void OnDeleting()
        {
        }

        protected abstract void DeleteImpl();

        protected virtual void OnDeleted()
        {
        }

        public void Update()
        {
            this.OnUpdating();
            this.UpdateImpl();
            this.OnUpdated();
        }

        protected virtual void OnUpdating()
        {
        }

        protected abstract void UpdateImpl();

        protected virtual void OnUpdated()
        {
        }
    }

    [Serializable()]
    public abstract class StorageObject<TTable, TRow>
        : StorageObject
        where TTable
            : TypedTableBase<TRow>,
              new()
        where TRow
            : DataRow
    {
        private TRow _underlyingDataRow;

        public override DataRow UnderlyingUntypedDataRow
        {
            get
            {
                return this._underlyingDataRow;
            }
            set
            {
                this._underlyingDataRow = (TRow) value;
            }
        }

        public override bool IsModified
        {
            get
            {
                return this._underlyingDataRow.RowState != DataRowState.Unchanged;
            }
        }

        public override bool IsStored
        {
            get
            {
                return this._underlyingDataRow != null;
            }
        }

        public TRow UnderlyingDataRow
        {
            get
            {
                if (this._underlyingDataRow == null)
                {
                    this._underlyingDataRow = (TRow) new TTable().NewRow();
                }
                return this._underlyingDataRow;
            }
            set
            {
                // Suppress re-setting.
                if (this._underlyingDataRow != null)
                {
                    // TODO: Exception string resource
                    throw new InvalidOperationException();
                }
                this._underlyingDataRow = value;
            }
        }

        protected override void DeleteImpl()
        {
            this.UnderlyingDataRow.Delete();
        }

        protected override void OnUpdating()
        {
            if (this.UnderlyingDataRow.RowState == DataRowState.Detached)
            {
                this.UnderlyingDataRow.Table.Rows.Add(this.UnderlyingDataRow);
            }
        }
    }
}