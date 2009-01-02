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
using System.Linq;
using System.Collections;
using System.Data;
using Achiral;

namespace XSpect.MetaTweet.ObjectModel
{
    [Serializable()]
    public abstract class StorageMap<TRow, TKey, TValue>
        : StorageObject<TRow>,
          IList<KeyValuePair<TKey, TValue>>
        where TRow : DataRow
    {
        private IList<KeyValuePair<TKey, TValue>> _list = new List<KeyValuePair<TKey, TValue>>();

        #region IEnumerable members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey, TValue>> members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this._list.GetEnumerator();
        }

        #endregion

        #region ICollection<KeyValuePair<TKey, TValue>> members

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.UnderlyingDataRow.Table.Rows.Add(item.Key, item.Value);
            this._list.Add(item);
        }

        public void Clear()
        {
            this.UnderlyingDataRow.Table.Rows.Clear();
            this._list.Clear();
        }

        public Boolean Contains(KeyValuePair<TKey, TValue> item)
        {
            return this._list.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, Int32 arrayIndex)
        {
            // TODO: It may be incorrect.
            this.UnderlyingDataRow.Table.Rows.CopyTo(array, arrayIndex);
            this._list.CopyTo(array, arrayIndex);
        }

        public Int32 Count
        {
            get
            {
                return this._list.Count;
            }
        }

        public Boolean IsReadOnly
        {
            get
            {
                return this._list.IsReadOnly;
            }
        }

        public Boolean Remove(KeyValuePair<TKey, TValue> item)
        {
            DataTable table = this.UnderlyingDataRow.Table;
            table.Rows.Remove(table.Rows.Cast<DataRow>().Single(r => r.ItemArray == Make.Array<Object>(item.Key, item.Value)));
            return this._list.Remove(item);
        }

        #endregion

        #region IList<KeyValuePair<TKey, TValue>> members

        public Int32 IndexOf(KeyValuePair<TKey, TValue> item)
        {
            return this._list.IndexOf(item);
        }

        public void Insert(Int32 index, KeyValuePair<TKey, TValue> item)
        {
            DataRow row = this.UnderlyingDataRow.Table.NewRow();
            row.ItemArray = Make.Array<Object>(item.Key, item.Value);
            this.UnderlyingDataRow.Table.Rows.InsertAt(row, index);
            this._list.Insert(index, item);
        }

        public void RemoveAt(Int32 index)
        {
            this.UnderlyingDataRow.Table.Rows.RemoveAt(index);
            this._list.RemoveAt(index);
        }

        public KeyValuePair<TKey, TValue> this[Int32 index]
        {
            get
            {
                return this._list[index];
            }
            set
            {
                this.UnderlyingDataRow.Table.Rows[index].ItemArray = Make.Array<Object>(value.Key, value.Value);
                this._list[index] = value;
            }
        }

        #endregion

        public void Add(TKey key, TValue value)
        {
            this.Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> pairs)
        {
            foreach (KeyValuePair<TKey, TValue> pair in pairs)
            {
                this.Add(pair);
            }
        }
    }
}