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
    /// <summary>
    /// The untyped base class of object model.
    /// </summary>
    [Serializable()]
    public abstract class StorageObject
        : Object
    {
        private Storage _storage;

        /// <summary>
        /// Gets the <see cref="Storage"/> which is used for resolving external data.
        /// </summary>
        public Storage Storage
        {
            get
            {
                return this._storage;
            }
            internal set
            {
                this._storage = value;
            }
        }

        /// <summary>
        /// Gets (or sets) the source of the <see cref="StorageObject"/> as not-strongly-typed <see cref="DataRow"/>.
        /// </summary>
        /// <remarks>
        /// This property can set the value only one time.
        /// </remarks>
        public abstract DataRow UnderlyingUntypedDataRow
        {
            get;
            set;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">the <see cref="Object"/>.</param>
        /// <returns>
        /// <c>true</c> if the underlying source is same as <paramref name="obj"/>; otherwise, <c>false</c>.
        /// </returns>
        public override Boolean Equals(Object obj)
        {
            return this.UnderlyingUntypedDataRow == (obj as StorageObject).UnderlyingUntypedDataRow;
        }

        /// <summary>
        /// Returns the hash code for <see cref="UnderlyingUntypedDataRow"/> of the <see cref="StorageObject"/>.
        /// </summary>
        /// <returns>
        /// The hash code for <see cref="UnderlyingUntypedDataRow"/>.
        /// </returns>
        public override Int32 GetHashCode()
        {
            return this.UnderlyingUntypedDataRow.GetHashCode();
        }

        /// <summary>
        /// Marks to delete the underlying source.
        /// </summary>
        public void Delete()
        {
            this.UnderlyingUntypedDataRow.Delete();
        }

        /// <summary>
        /// Commit the changes of the <see cref="UnderlyingUntypedDataRow"/> to the <see cref="Storage"/>.
        /// </summary>
        public virtual void Update()
        {
            if (this.UnderlyingUntypedDataRow.RowState == DataRowState.Detached)
            {
                this.UnderlyingUntypedDataRow.Table.Rows.Add(this.UnderlyingUntypedDataRow);
            }
            this.Storage.Update();
        }
    }

    /// <summary>
    /// The strongly typed base class of object model.
    /// </summary>
    /// <typeparam name="TTable">The type of underlying source's <see cref="DataTable"/>.</typeparam>
    /// <typeparam name="TRow">The type of underlying source's <see cref="DataRow"/>.</typeparam>
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

        /// <summary>
        /// Gets or sets the <see cref="UnderlyingDataRow"/> as <see cref="DataRow"/>.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the source of the <see cref="StorageObject"/> as strongly-typed <see cref="DataRow"/>.
        /// </summary>
        /// <remarks>
        /// This property can set the value only one time.
        /// </remarks>
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

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">the <see cref="Object"/>.</param>
        /// <returns>
        /// <c>true</c> if the underlying source is same as <paramref name="obj"/>; otherwise, <c>false</c>.
        /// </returns>
        public override Boolean Equals(Object obj)
        {
            return this.UnderlyingDataRow == (obj as StorageObject<TTable, TRow>).UnderlyingDataRow;
        }

        /// <summary>
        /// Returns the hash code for <see cref="UnderlyingDataRow"/> of the <see cref="StorageObject"/>.
        /// </summary>
        /// <returns>The hash code for <see cref="UnderlyingDataRow"/>.</returns>
        public override Int32 GetHashCode()
        {
            return this.UnderlyingDataRow.GetHashCode();
        }

        /// <summary>
        /// Commit the changes of the <see cref="UnderlyingDataRow"/> to the <see cref="Storage"/>.
        /// </summary>
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