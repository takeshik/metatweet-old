// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008 Takeshi KIRIYA, XSpect Project <takeshik@xspect.org>
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

namespace XSpect.MetaTweet.ObjectModel
{
    [Serializable()]
    public abstract class StorageMap<TTable, TKey, TValue>
        : StorageObject<TTable>,
          IList<KeyValuePair<TKey, TValue>>
        where TTable : DataTable
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
            this._list.Add(item);
        }

        public void Clear()
        {
            this._list.Clear();
        }

        public Boolean Contains(KeyValuePair<TKey, TValue> item)
        {
            return this._list.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, Int32 arrayIndex)
        {
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
            this._list.Insert(index, item);
        }

        public void RemoveAt(Int32 index)
        {
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
                this._list[index] = value;
            }
        }

        #endregion
    }
}