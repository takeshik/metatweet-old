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
    public sealed class StorageObject
        : Object
    {
        private Storage _storage;

        private IEnumerable<DataRow> _underlyingDataRows;

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

        public DataRow UnderlyingDataRow
        {
            get
            {
                return this._underlyingDataRows.Single();
            }
            set
            {
                this._underlyingDataRows = Make.Array(value);
            }
        }

        public IEnumerable<DataRow> UnderlyingDataRows
        {
            get
            {
                return this._underlyingDataRows;
            }
            set
            {
                this._underlyingDataRows = value;
            }
        }
    }

    [Serializable()]
    public abstract class StorageObject<T>
        : Object
        where T : DataRow
    {
        private Storage _storage;

        private IEnumerable<T> _underlyingDataRows;

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

        public T UnderlyingDataRow
        {
            protected get
            {
                return this._underlyingDataRows.First();
            }
            set
            {
                this._underlyingDataRows = Make.Array(value);
            }
        }

        public IEnumerable<T> UnderlyingDataRows
        {
            protected get
            {
                return this._underlyingDataRows;
            }
            set
            {
                // Restrict to set the value only one time.
                if (this._underlyingDataRows != null)
                {
                    // TODO: exception string resource
                    throw new InvalidOperationException();
                }
                this._underlyingDataRows = value;
            }
        }

        public static implicit operator StorageObject(StorageObject<T> self)
        {
            return new StorageObject()
            {
                Storage = self._storage,
                UnderlyingDataRows = self._underlyingDataRows.Cast<DataRow>().ToArray(),
            };
        }
    }
}