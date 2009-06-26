// -*- mode: csharp; encoding: utf-8; -*-
// $Id$
/* XSpect Common Framework - Generic utility class library
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of XSpect Common Framework.
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
using Achiral.Extension;

namespace XSpect
{
    public static class Create
    {
        public static KeyValuePair<TKey, TValue> KeyValuePair<TKey, TValue>(TKey key, TValue value)
        {
            return new KeyValuePair<TKey, TValue>(key, value);
        }

        public static Struct<T1, T2> Struct<T1, T2>(T1 item1, T2 item2)
        {
            return new Struct<T1, T2>(item1, item2);
        }

        public static Struct<T1, T2, T3> Struct<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
        {
            return new Struct<T1, T2, T3>(item1, item2, item3);
        }

        public static Struct<T1, T2, T3, T4> Struct<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            return new Struct<T1, T2, T3, T4>(item1, item2, item3, item4);
        }

        public static Struct<T1, T2, T3, T4, T5> Struct<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
        {
            return new Struct<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);
        }

        public static Struct<T1, T2, T3, T4, T5, T6> Struct<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
        {
            return new Struct<T1, T2, T3, T4, T5, T6>(item1, item2, item3, item4, item5, item6);
        }

        public static Struct<T1, T2, T3, T4, T5, T6, T7> Struct<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
        {
            return new Struct<T1, T2, T3, T4, T5, T6, T7>(item1, item2, item3, item4, item5, item6, item7);
        }

        public static Struct<T1, T2, T3, T4, T5, T6, T7, T8> Struct<T1, T2, T3, T4, T5, T6, T7, T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
        {
            return new Struct<T1, T2, T3, T4, T5, T6, T7, T8>(item1, item2, item3, item4, item5, item6, item7, item8);
        }

        public static IDictionary<T, T> Table<T>(params T[] elements)
        {
            if (elements.Length % 2 == 1)
            {
                throw new ArgumentException("elements");
            }
            return elements
                .Where((e, i) => i % 2 == 0)
                .ZipWith(
                    elements.Where((e, i) => i % 2 == 1),
                    (k, v) => KeyValuePair(k, v)
                )
                .ToDictionary(p => p.Key, p => p.Value);
        }

    }
}