// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetObjectModel
 *   Object model and Storage interface for MetaTweet and other systems
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@xspect.org>
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
using System.Data;
using System.Linq;

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

        public override Boolean Equals(Object obj)
        {
            return this.UnderlyingUntypedDataRow == (obj as StorageObject).UnderlyingUntypedDataRow;
        }

        public override Int32 GetHashCode()
        {
            return this.UnderlyingUntypedDataRow.GetHashCode();
        }

        public void Delete()
        {
            this.UnderlyingUntypedDataRow.Delete();
        }

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

        public TRow UnderlyingDataRow
        {
            get
            {
                if (this._underlyingDataRow == null)
                {
                    this._underlyingDataRow = (TRow) this.Storage.UnderlyingDataSet.Tables
                        .OfType<TTable>()
                        .Single()
                        .NewRow();
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

        public override Boolean Equals(Object obj)
        {
            return this.UnderlyingDataRow == (obj as StorageObject<TTable, TRow>).UnderlyingDataRow;
        }

        public override Int32 GetHashCode()
        {
            return this.UnderlyingDataRow.GetHashCode();
        }

        public override void Update()
        {
            if (this.UnderlyingDataRow.RowState == DataRowState.Detached)
            {
                this.UnderlyingDataRow.Table.Rows.Add(this.UnderlyingDataRow);
            }
            this.Storage.Update();
        }
    }
}