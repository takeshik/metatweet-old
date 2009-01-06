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

        public DataRow UnderlyingUntypedDataRow
        {
            get
            {
                return this.UnderlyingUntypedDataRows.First();
            }
        }

        public abstract IEnumerable<DataRow> UnderlyingUntypedDataRows
        {
            get;
        }

        public virtual Boolean IsModified
        {
            get
            {
                return this.UnderlyingUntypedDataRows.All(r => r.RowState != DataRowState.Unchanged);
            }
        }

        public virtual Boolean IsStored
        {
            get
            {
                return this.UnderlyingUntypedDataRows != null;
            }
        }

        public abstract void Delete();

        public abstract void Update();
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
        private IEnumerable<TRow> _underlyingDataRows;

        public override IEnumerable<DataRow> UnderlyingUntypedDataRows
        {
            get
            {
                return this.UnderlyingDataRows.Cast<DataRow>();
            }
        }

        public override bool IsModified
        {
            get
            {
                return this._underlyingDataRows.All(r => r.RowState != DataRowState.Unchanged);
            }
        }

        public override bool IsStored
        {
            get
            {
                return this._underlyingDataRows != null;
            }
        }

        public TRow UnderlyingDataRow
        {
            get
            {
                return this._underlyingDataRows.First();
            }
            set
            {
                this._underlyingDataRows = Make.Array(value);
            }
        }

        public IEnumerable<TRow> UnderlyingDataRows
        {
            get
            {
                if (this._underlyingDataRows == null)
                {
                    TTable table = new TTable();
                    TRow row = (TRow) table.NewRow();
                    table.Rows.Add(row);
                    this._underlyingDataRows = Make.Array(row);
                }
                return this._underlyingDataRows;
            }
            set
            {
                this._underlyingDataRows = value;
            }
        }
    }
}