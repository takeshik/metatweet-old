// -*- mode: csharp; encoding: utf-8; -*-
/* XSpect Common Framework - Generic Utility Class Library
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
using XSpect.Extension;
using Achiral;
using Achiral.Extension;

namespace XSpect
{
    public static class Lambda
        : Object
    {
        public static void Bind<T>(Action<T> action, T value)
        {
            action(value);
        }

        public static void Bind<T1, T2>(Action<T1, T2> action, T1 value1, T2 value2)
        {
            action(value1, value2);
        }

        public static void Bind<T1, T2, T3>(Action<T1, T2, T3> action, T1 value1, T2 value2, T3 value3)
        {
            action(value1, value2, value3);
        }

        public static void Bind<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, T1 value1, T2 value2, T3 value3, T4 value4)
        {
            action(value1, value2, value3, value4);
        }

        public static void Bind<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action, T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
        {
            action(value1, value2, value3, value4, value5);
        }

        public static void Bind<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action, T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6)
        {
            action(value1, value2, value3, value4, value5, value6);
        }

        public static void Bind<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action, T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7)
        {
            action(value1, value2, value3, value4, value5, value6, value7);
        }

        public static void Bind<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action, T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8)
        {
            action(value1, value2, value3, value4, value5, value6, value7, value8);
        }

        public static TResult Bind<T, TResult>(Func<T, TResult> func, T value)
        {
            return func(value);
        }

        public static TResult Bind<T1, T2, TResult>(Func<T1, T2, TResult> func, T1 value1, T2 value2)
        {
            return func(value1, value2);
        }

        public static TResult Bind<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, T1 value1, T2 value2, T3 value3)
        {
            return func(value1, value2, value3);
        }

        public static TResult Bind<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func, T1 value1, T2 value2, T3 value3, T4 value4)
        {
            return func(value1, value2, value3, value4);
        }

        public static TResult Bind<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> func, T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
        {
            return func(value1, value2, value3, value4, value5);
        }

        public static TResult Bind<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> func, T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6)
        {
            return func(value1, value2, value3, value4, value5, value6);
        }

        public static TResult Bind<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7)
        {
            return func(value1, value2, value3, value4, value5, value6, value7);
        }

        public static TResult Bind<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8)
        {
            return func(value1, value2, value3, value4, value5, value6, value7, value8);
        }
    }
}