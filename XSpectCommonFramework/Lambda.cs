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
        public static Action New(Action action)
        {
            return action;
        }

        public static Action<T> New<T>(Action<T> action)
        {
            return action;
        }

        public static Action<T1, T2> New<T1, T2>(Action<T1, T2> action)
        {
            return action;
        }

        public static Action<T1, T2, T3> New<T1, T2, T3>(Action<T1, T2, T3> action)
        {
            return action;
        }

        public static Action<T1, T2, T3, T4> New<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action)
        {
            return action;
        }

        public static Action<T1, T2, T3, T4, T5> New<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action)
        {
            return action;
        }

        public static Action<T1, T2, T3, T4, T5, T6> New<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action)
        {
            return action;
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7> New<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            return action;
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8> New<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            return action;
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> New<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
        {
            return action;
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
        {
            return action;
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
        {
            return action;
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
        {
            return action;
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
        {
            return action;
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
        {
            return action;
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
        {
            return action;
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
        {
            return action;
        }

        public static Func<TResult> New<TResult>(Func<TResult> func)
        {
            return func;
        }

        public static Func<T, TResult> New<T, TResult>(Func<T, TResult> func)
        {
            return func;
        }

        public static Func<T1, T2, TResult> New<T1, T2, TResult>(Func<T1, T2, TResult> func)
        {
            return func;
        }

        public static Func<T1, T2, T3, TResult> New<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func)
        {
            return func;
        }

        public static Func<T1, T2, T3, T4, TResult> New<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func)
        {
            return func;
        }

        public static Func<T1, T2, T3, T4, T5, TResult> New<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> func)
        {
            return func;
        }

        public static Func<T1, T2, T3, T4, T5, T6, TResult> New<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> func)
        {
            return func;
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, TResult> New<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> func)
        {
            return func;
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> New<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func)
        {
            return func;
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> New<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func)
        {
            return func;
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func)
        {
            return func;
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func)
        {
            return func;
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func)
        {
            return func;
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func)
        {
            return func;
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func)
        {
            return func;
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func)
        {
            return func;
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func)
        {
            return func;
        }

        public static Action Bind<T>(this Action<T> action, T value)
        {
            return new Action(() => action(value));
        }

        public static Action<T2> Bind1st<T1, T2>(this Action<T1, T2> action, T1 value1)
        {
            return new Action<T2>(value2 => action(value1, value2));
        }

        public static Action<T1> Bind2nd<T1, T2>(this Action<T1, T2> action, T2 value2)
        {
            return new Action<T1>(value1 => action(value1, value2));
        }

        public static Action<T2, T3> Bind1st<T1, T2, T3>(this Action<T1, T2, T3> action, T1 value1)
        {
            return new Action<T2, T3>((value2, value3) => action(value1, value2, value3));
        }

        public static Action<T1, T3> Bind2nd<T1, T2, T3>(this Action<T1, T2, T3> action, T2 value2)
        {
            return new Action<T1, T3>((value1, value3) => action(value1, value2, value3));
        }

        public static Action<T1, T2> Bind3rd<T1, T2, T3>(this Action<T1, T2, T3> action, T3 value3)
        {
            return new Action<T1, T2>((value1, value2) => action(value1, value2, value3));
        }

        public static Action<T2, T3, T4> Bind1st<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T1 value1)
        {
            return new Action<T2, T3, T4>((value2, value3, value4) => action(value1, value2, value3, value4));
        }

        public static Action<T1, T3, T4> Bind2nd<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T2 value2)
        {
            return new Action<T1, T3, T4>((value1, value3, value4) => action(value1, value2, value3, value4));
        }

        public static Action<T1, T2, T4> Bind3rd<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T3 value3)
        {
            return new Action<T1, T2, T4>((value1, value2, value4) => action(value1, value2, value3, value4));
        }

        public static Action<T1, T2, T3> Bind4th<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T4 value4)
        {
            return new Action<T1, T2, T3>((value1, value2, value3) => action(value1, value2, value3, value4));
        }

        public static Action<T2, T3, T4, T5> Bind1st<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action, T1 value1)
        {
            return new Action<T2, T3, T4, T5>((value2, value3, value4, value5) => action(value1, value2, value3, value4, value5));
        }

        public static Action<T1, T3, T4, T5> Bind2nd<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action, T2 value2)
        {
            return new Action<T1, T3, T4, T5>((value1, value3, value4, value5) => action(value1, value2, value3, value4, value5));
        }

        public static Action<T1, T2, T4, T5> Bind3rd<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action, T3 value3)
        {
            return new Action<T1, T2, T4, T5>((value1, value2, value4, value5) => action(value1, value2, value3, value4, value5));
        }

        public static Action<T1, T2, T3, T5> Bind4th<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action, T4 value4)
        {
            return new Action<T1, T2, T3, T5>((value1, value2, value3, value5) => action(value1, value2, value3, value4, value5));
        }

        public static Action<T1, T2, T3, T4> Bind5th<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action, T5 value5)
        {
            return new Action<T1, T2, T3, T4>((value1, value2, value3, value4) => action(value1, value2, value3, value4, value5));
        }

        public static Action<T2, T3, T4, T5, T6> Bind1st<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> action, T1 value1)
        {
            return new Action<T2, T3, T4, T5, T6>((value2, value3, value4, value5, value6) => action(value1, value2, value3, value4, value5, value6));
        }

        public static Action<T1, T3, T4, T5, T6> Bind2nd<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> action, T2 value2)
        {
            return new Action<T1, T3, T4, T5, T6>((value1, value3, value4, value5, value6) => action(value1, value2, value3, value4, value5, value6));
        }

        public static Action<T1, T2, T4, T5, T6> Bind3rd<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> action, T3 value3)
        {
            return new Action<T1, T2, T4, T5, T6>((value1, value2, value4, value5, value6) => action(value1, value2, value3, value4, value5, value6));
        }

        public static Action<T1, T2, T3, T5, T6> Bind4th<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> action, T4 value4)
        {
            return new Action<T1, T2, T3, T5, T6>((value1, value2, value3, value5, value6) => action(value1, value2, value3, value4, value5, value6));
        }

        public static Action<T1, T2, T3, T4, T6> Bind5th<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> action, T5 value5)
        {
            return new Action<T1, T2, T3, T4, T6>((value1, value2, value3, value4, value6) => action(value1, value2, value3, value4, value5, value6));
        }

        public static Action<T1, T2, T3, T4, T5> Bind6th<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> action, T6 value6)
        {
            return new Action<T1, T2, T3, T4, T5>((value1, value2, value3, value4, value5) => action(value1, value2, value3, value4, value5, value6));
        }

        public static Action<T2, T3, T4, T5, T6, T7> Bind1st<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action, T1 value1)
        {
            return new Action<T2, T3, T4, T5, T6, T7>((value2, value3, value4, value5, value6, value7) => action(value1, value2, value3, value4, value5, value6, value7));
        }

        public static Action<T1, T3, T4, T5, T6, T7> Bind2nd<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action, T2 value2)
        {
            return new Action<T1, T3, T4, T5, T6, T7>((value1, value3, value4, value5, value6, value7) => action(value1, value2, value3, value4, value5, value6, value7));
        }

        public static Action<T1, T2, T4, T5, T6, T7> Bind3rd<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action, T3 value3)
        {
            return new Action<T1, T2, T4, T5, T6, T7>((value1, value2, value4, value5, value6, value7) => action(value1, value2, value3, value4, value5, value6, value7));
        }

        public static Action<T1, T2, T3, T5, T6, T7> Bind4th<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action, T4 value4)
        {
            return new Action<T1, T2, T3, T5, T6, T7>((value1, value2, value3, value5, value6, value7) => action(value1, value2, value3, value4, value5, value6, value7));
        }

        public static Action<T1, T2, T3, T4, T6, T7> Bind5th<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action, T5 value5)
        {
            return new Action<T1, T2, T3, T4, T6, T7>((value1, value2, value3, value4, value6, value7) => action(value1, value2, value3, value4, value5, value6, value7));
        }

        public static Action<T1, T2, T3, T4, T5, T7> Bind6th<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action, T6 value6)
        {
            return new Action<T1, T2, T3, T4, T5, T7>((value1, value2, value3, value4, value5, value7) => action(value1, value2, value3, value4, value5, value6, value7));
        }

        public static Action<T1, T2, T3, T4, T5, T6> Bind7th<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action, T7 value7)
        {
            return new Action<T1, T2, T3, T4, T5, T6>((value1, value2, value3, value4, value5, value6) => action(value1, value2, value3, value4, value5, value6, value7));
        }

        public static Action<T2, T3, T4, T5, T6, T7, T8> Bind1st<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action, T1 value1)
        {
            return new Action<T2, T3, T4, T5, T6, T7, T8>((value2, value3, value4, value5, value6, value7, value8) => action(value1, value2, value3, value4, value5, value6, value7, value8));
        }

        public static Action<T1, T3, T4, T5, T6, T7, T8> Bind2nd<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action, T2 value2)
        {
            return new Action<T1, T3, T4, T5, T6, T7, T8>((value1, value3, value4, value5, value6, value7, value8) => action(value1, value2, value3, value4, value5, value6, value7, value8));
        }

        public static Action<T1, T2, T4, T5, T6, T7, T8> Bind3rd<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action, T3 value3)
        {
            return new Action<T1, T2, T4, T5, T6, T7, T8>((value1, value2, value4, value5, value6, value7, value8) => action(value1, value2, value3, value4, value5, value6, value7, value8));
        }

        public static Action<T1, T2, T3, T5, T6, T7, T8> Bind4th<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action, T4 value4)
        {
            return new Action<T1, T2, T3, T5, T6, T7, T8>((value1, value2, value3, value5, value6, value7, value8) => action(value1, value2, value3, value4, value5, value6, value7, value8));
        }

        public static Action<T1, T2, T3, T4, T6, T7, T8> Bind5th<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action, T5 value5)
        {
            return new Action<T1, T2, T3, T4, T6, T7, T8>((value1, value2, value3, value4, value6, value7, value8) => action(value1, value2, value3, value4, value5, value6, value7, value8));
        }

        public static Action<T1, T2, T3, T4, T5, T7, T8> Bind6th<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action, T6 value6)
        {
            return new Action<T1, T2, T3, T4, T5, T7, T8>((value1, value2, value3, value4, value5, value7, value8) => action(value1, value2, value3, value4, value5, value6, value7, value8));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T8> Bind7th<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action, T7 value7)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T8>((value1, value2, value3, value4, value5, value6, value8) => action(value1, value2, value3, value4, value5, value6, value7, value8));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7> Bind8th<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action, T8 value8)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7>((value1, value2, value3, value4, value5, value6, value7) => action(value1, value2, value3, value4, value5, value6, value7, value8));
        }

        public static Action<T2, T3, T4, T5, T6, T7, T8, T9> Bind1st<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, T1 value1)
        {
            return new Action<T2, T3, T4, T5, T6, T7, T8, T9>((value2, value3, value4, value5, value6, value7, value8, value9) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9));
        }

        public static Action<T1, T3, T4, T5, T6, T7, T8, T9> Bind2nd<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, T2 value2)
        {
            return new Action<T1, T3, T4, T5, T6, T7, T8, T9>((value1, value3, value4, value5, value6, value7, value8, value9) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9));
        }

        public static Action<T1, T2, T4, T5, T6, T7, T8, T9> Bind3rd<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, T3 value3)
        {
            return new Action<T1, T2, T4, T5, T6, T7, T8, T9>((value1, value2, value4, value5, value6, value7, value8, value9) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9));
        }

        public static Action<T1, T2, T3, T5, T6, T7, T8, T9> Bind4th<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, T4 value4)
        {
            return new Action<T1, T2, T3, T5, T6, T7, T8, T9>((value1, value2, value3, value5, value6, value7, value8, value9) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9));
        }

        public static Action<T1, T2, T3, T4, T6, T7, T8, T9> Bind5th<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, T5 value5)
        {
            return new Action<T1, T2, T3, T4, T6, T7, T8, T9>((value1, value2, value3, value4, value6, value7, value8, value9) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9));
        }

        public static Action<T1, T2, T3, T4, T5, T7, T8, T9> Bind6th<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, T6 value6)
        {
            return new Action<T1, T2, T3, T4, T5, T7, T8, T9>((value1, value2, value3, value4, value5, value7, value8, value9) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T8, T9> Bind7th<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, T7 value7)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T8, T9>((value1, value2, value3, value4, value5, value6, value8, value9) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T9> Bind8th<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, T8 value8)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T9>((value1, value2, value3, value4, value5, value6, value7, value9) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8> Bind9th<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, T9 value9)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8>((value1, value2, value3, value4, value5, value6, value7, value8) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9));
        }

        public static Action<T2, T3, T4, T5, T6, T7, T8, T9, T10> Bind1st<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, T1 value1)
        {
            return new Action<T2, T3, T4, T5, T6, T7, T8, T9, T10>((value2, value3, value4, value5, value6, value7, value8, value9, value10) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10));
        }

        public static Action<T1, T3, T4, T5, T6, T7, T8, T9, T10> Bind2nd<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, T2 value2)
        {
            return new Action<T1, T3, T4, T5, T6, T7, T8, T9, T10>((value1, value3, value4, value5, value6, value7, value8, value9, value10) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10));
        }

        public static Action<T1, T2, T4, T5, T6, T7, T8, T9, T10> Bind3rd<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, T3 value3)
        {
            return new Action<T1, T2, T4, T5, T6, T7, T8, T9, T10>((value1, value2, value4, value5, value6, value7, value8, value9, value10) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10));
        }

        public static Action<T1, T2, T3, T5, T6, T7, T8, T9, T10> Bind4th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, T4 value4)
        {
            return new Action<T1, T2, T3, T5, T6, T7, T8, T9, T10>((value1, value2, value3, value5, value6, value7, value8, value9, value10) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10));
        }

        public static Action<T1, T2, T3, T4, T6, T7, T8, T9, T10> Bind5th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, T5 value5)
        {
            return new Action<T1, T2, T3, T4, T6, T7, T8, T9, T10>((value1, value2, value3, value4, value6, value7, value8, value9, value10) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10));
        }

        public static Action<T1, T2, T3, T4, T5, T7, T8, T9, T10> Bind6th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, T6 value6)
        {
            return new Action<T1, T2, T3, T4, T5, T7, T8, T9, T10>((value1, value2, value3, value4, value5, value7, value8, value9, value10) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T8, T9, T10> Bind7th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, T7 value7)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T8, T9, T10>((value1, value2, value3, value4, value5, value6, value8, value9, value10) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T9, T10> Bind8th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, T8 value8)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T9, T10>((value1, value2, value3, value4, value5, value6, value7, value9, value10) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T10> Bind9th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, T9 value9)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T10>((value1, value2, value3, value4, value5, value6, value7, value8, value10) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> Bind10th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, T10 value10)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>((value1, value2, value3, value4, value5, value6, value7, value8, value9) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10));
        }

        public static Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Bind1st<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, T1 value1)
        {
            return new Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>((value2, value3, value4, value5, value6, value7, value8, value9, value10, value11) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11));
        }

        public static Action<T1, T3, T4, T5, T6, T7, T8, T9, T10, T11> Bind2nd<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, T2 value2)
        {
            return new Action<T1, T3, T4, T5, T6, T7, T8, T9, T10, T11>((value1, value3, value4, value5, value6, value7, value8, value9, value10, value11) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11));
        }

        public static Action<T1, T2, T4, T5, T6, T7, T8, T9, T10, T11> Bind3rd<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, T3 value3)
        {
            return new Action<T1, T2, T4, T5, T6, T7, T8, T9, T10, T11>((value1, value2, value4, value5, value6, value7, value8, value9, value10, value11) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11));
        }

        public static Action<T1, T2, T3, T5, T6, T7, T8, T9, T10, T11> Bind4th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, T4 value4)
        {
            return new Action<T1, T2, T3, T5, T6, T7, T8, T9, T10, T11>((value1, value2, value3, value5, value6, value7, value8, value9, value10, value11) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11));
        }

        public static Action<T1, T2, T3, T4, T6, T7, T8, T9, T10, T11> Bind5th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, T5 value5)
        {
            return new Action<T1, T2, T3, T4, T6, T7, T8, T9, T10, T11>((value1, value2, value3, value4, value6, value7, value8, value9, value10, value11) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11));
        }

        public static Action<T1, T2, T3, T4, T5, T7, T8, T9, T10, T11> Bind6th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, T6 value6)
        {
            return new Action<T1, T2, T3, T4, T5, T7, T8, T9, T10, T11>((value1, value2, value3, value4, value5, value7, value8, value9, value10, value11) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T8, T9, T10, T11> Bind7th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, T7 value7)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T8, T9, T10, T11>((value1, value2, value3, value4, value5, value6, value8, value9, value10, value11) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T9, T10, T11> Bind8th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, T8 value8)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T9, T10, T11>((value1, value2, value3, value4, value5, value6, value7, value9, value10, value11) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T10, T11> Bind9th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, T9 value9)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T10, T11>((value1, value2, value3, value4, value5, value6, value7, value8, value10, value11) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T11> Bind10th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, T10 value10)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T11>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value11) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Bind11th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, T11 value11)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11));
        }

        public static Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Bind1st<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, T1 value1)
        {
            return new Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>((value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12));
        }

        public static Action<T1, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Bind2nd<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, T2 value2)
        {
            return new Action<T1, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>((value1, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12));
        }

        public static Action<T1, T2, T4, T5, T6, T7, T8, T9, T10, T11, T12> Bind3rd<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, T3 value3)
        {
            return new Action<T1, T2, T4, T5, T6, T7, T8, T9, T10, T11, T12>((value1, value2, value4, value5, value6, value7, value8, value9, value10, value11, value12) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12));
        }

        public static Action<T1, T2, T3, T5, T6, T7, T8, T9, T10, T11, T12> Bind4th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, T4 value4)
        {
            return new Action<T1, T2, T3, T5, T6, T7, T8, T9, T10, T11, T12>((value1, value2, value3, value5, value6, value7, value8, value9, value10, value11, value12) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12));
        }

        public static Action<T1, T2, T3, T4, T6, T7, T8, T9, T10, T11, T12> Bind5th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, T5 value5)
        {
            return new Action<T1, T2, T3, T4, T6, T7, T8, T9, T10, T11, T12>((value1, value2, value3, value4, value6, value7, value8, value9, value10, value11, value12) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12));
        }

        public static Action<T1, T2, T3, T4, T5, T7, T8, T9, T10, T11, T12> Bind6th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, T6 value6)
        {
            return new Action<T1, T2, T3, T4, T5, T7, T8, T9, T10, T11, T12>((value1, value2, value3, value4, value5, value7, value8, value9, value10, value11, value12) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T8, T9, T10, T11, T12> Bind7th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, T7 value7)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T8, T9, T10, T11, T12>((value1, value2, value3, value4, value5, value6, value8, value9, value10, value11, value12) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T9, T10, T11, T12> Bind8th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, T8 value8)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T9, T10, T11, T12>((value1, value2, value3, value4, value5, value6, value7, value9, value10, value11, value12) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T10, T11, T12> Bind9th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, T9 value9)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T10, T11, T12>((value1, value2, value3, value4, value5, value6, value7, value8, value10, value11, value12) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T11, T12> Bind10th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, T10 value10)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T11, T12>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value11, value12) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T12> Bind11th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, T11 value11)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T12>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value12) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Bind12th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, T12 value12)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12));
        }

        public static Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Bind1st<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, T1 value1)
        {
            return new Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>((value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13));
        }

        public static Action<T1, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Bind2nd<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, T2 value2)
        {
            return new Action<T1, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>((value1, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13));
        }

        public static Action<T1, T2, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Bind3rd<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, T3 value3)
        {
            return new Action<T1, T2, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>((value1, value2, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13));
        }

        public static Action<T1, T2, T3, T5, T6, T7, T8, T9, T10, T11, T12, T13> Bind4th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, T4 value4)
        {
            return new Action<T1, T2, T3, T5, T6, T7, T8, T9, T10, T11, T12, T13>((value1, value2, value3, value5, value6, value7, value8, value9, value10, value11, value12, value13) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13));
        }

        public static Action<T1, T2, T3, T4, T6, T7, T8, T9, T10, T11, T12, T13> Bind5th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, T5 value5)
        {
            return new Action<T1, T2, T3, T4, T6, T7, T8, T9, T10, T11, T12, T13>((value1, value2, value3, value4, value6, value7, value8, value9, value10, value11, value12, value13) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13));
        }

        public static Action<T1, T2, T3, T4, T5, T7, T8, T9, T10, T11, T12, T13> Bind6th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, T6 value6)
        {
            return new Action<T1, T2, T3, T4, T5, T7, T8, T9, T10, T11, T12, T13>((value1, value2, value3, value4, value5, value7, value8, value9, value10, value11, value12, value13) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T8, T9, T10, T11, T12, T13> Bind7th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, T7 value7)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T8, T9, T10, T11, T12, T13>((value1, value2, value3, value4, value5, value6, value8, value9, value10, value11, value12, value13) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T9, T10, T11, T12, T13> Bind8th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, T8 value8)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T9, T10, T11, T12, T13>((value1, value2, value3, value4, value5, value6, value7, value9, value10, value11, value12, value13) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T10, T11, T12, T13> Bind9th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, T9 value9)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T10, T11, T12, T13>((value1, value2, value3, value4, value5, value6, value7, value8, value10, value11, value12, value13) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T11, T12, T13> Bind10th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, T10 value10)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T11, T12, T13>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value11, value12, value13) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T12, T13> Bind11th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, T11 value11)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T12, T13>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value12, value13) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T13> Bind12th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, T12 value12)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T13>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value13) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Bind13th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, T13 value13)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13));
        }

        public static Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Bind1st<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, T1 value1)
        {
            return new Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>((value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14));
        }

        public static Action<T1, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Bind2nd<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, T2 value2)
        {
            return new Action<T1, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>((value1, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14));
        }

        public static Action<T1, T2, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Bind3rd<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, T3 value3)
        {
            return new Action<T1, T2, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>((value1, value2, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14));
        }

        public static Action<T1, T2, T3, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Bind4th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, T4 value4)
        {
            return new Action<T1, T2, T3, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>((value1, value2, value3, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14));
        }

        public static Action<T1, T2, T3, T4, T6, T7, T8, T9, T10, T11, T12, T13, T14> Bind5th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, T5 value5)
        {
            return new Action<T1, T2, T3, T4, T6, T7, T8, T9, T10, T11, T12, T13, T14>((value1, value2, value3, value4, value6, value7, value8, value9, value10, value11, value12, value13, value14) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14));
        }

        public static Action<T1, T2, T3, T4, T5, T7, T8, T9, T10, T11, T12, T13, T14> Bind6th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, T6 value6)
        {
            return new Action<T1, T2, T3, T4, T5, T7, T8, T9, T10, T11, T12, T13, T14>((value1, value2, value3, value4, value5, value7, value8, value9, value10, value11, value12, value13, value14) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T8, T9, T10, T11, T12, T13, T14> Bind7th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, T7 value7)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T8, T9, T10, T11, T12, T13, T14>((value1, value2, value3, value4, value5, value6, value8, value9, value10, value11, value12, value13, value14) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T9, T10, T11, T12, T13, T14> Bind8th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, T8 value8)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T9, T10, T11, T12, T13, T14>((value1, value2, value3, value4, value5, value6, value7, value9, value10, value11, value12, value13, value14) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T10, T11, T12, T13, T14> Bind9th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, T9 value9)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T10, T11, T12, T13, T14>((value1, value2, value3, value4, value5, value6, value7, value8, value10, value11, value12, value13, value14) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T11, T12, T13, T14> Bind10th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, T10 value10)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T11, T12, T13, T14>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value11, value12, value13, value14) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T12, T13, T14> Bind11th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, T11 value11)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T12, T13, T14>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value12, value13, value14) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T13, T14> Bind12th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, T12 value12)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T13, T14>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value13, value14) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T14> Bind13th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, T13 value13)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T14>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value14) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Bind14th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, T14 value14)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14));
        }

        public static Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Bind1st<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, T1 value1)
        {
            return new Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>((value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Action<T1, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Bind2nd<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, T2 value2)
        {
            return new Action<T1, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>((value1, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Action<T1, T2, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Bind3rd<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, T3 value3)
        {
            return new Action<T1, T2, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>((value1, value2, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Action<T1, T2, T3, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Bind4th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, T4 value4)
        {
            return new Action<T1, T2, T3, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>((value1, value2, value3, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Action<T1, T2, T3, T4, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Bind5th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, T5 value5)
        {
            return new Action<T1, T2, T3, T4, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>((value1, value2, value3, value4, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Action<T1, T2, T3, T4, T5, T7, T8, T9, T10, T11, T12, T13, T14, T15> Bind6th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, T6 value6)
        {
            return new Action<T1, T2, T3, T4, T5, T7, T8, T9, T10, T11, T12, T13, T14, T15>((value1, value2, value3, value4, value5, value7, value8, value9, value10, value11, value12, value13, value14, value15) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T8, T9, T10, T11, T12, T13, T14, T15> Bind7th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, T7 value7)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T8, T9, T10, T11, T12, T13, T14, T15>((value1, value2, value3, value4, value5, value6, value8, value9, value10, value11, value12, value13, value14, value15) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T9, T10, T11, T12, T13, T14, T15> Bind8th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, T8 value8)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T9, T10, T11, T12, T13, T14, T15>((value1, value2, value3, value4, value5, value6, value7, value9, value10, value11, value12, value13, value14, value15) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T10, T11, T12, T13, T14, T15> Bind9th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, T9 value9)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T10, T11, T12, T13, T14, T15>((value1, value2, value3, value4, value5, value6, value7, value8, value10, value11, value12, value13, value14, value15) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T11, T12, T13, T14, T15> Bind10th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, T10 value10)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T11, T12, T13, T14, T15>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value11, value12, value13, value14, value15) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T12, T13, T14, T15> Bind11th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, T11 value11)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T12, T13, T14, T15>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value12, value13, value14, value15) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T13, T14, T15> Bind12th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, T12 value12)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T13, T14, T15>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value13, value14, value15) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T14, T15> Bind13th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, T13 value13)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T14, T15>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value14, value15) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T15> Bind14th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, T14 value14)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T15>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value15) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Bind15th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, T15 value15)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> Bind1st<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, T1 value1)
        {
            return new Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>((value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Action<T1, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> Bind2nd<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, T2 value2)
        {
            return new Action<T1, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>((value1, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Action<T1, T2, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> Bind3rd<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, T3 value3)
        {
            return new Action<T1, T2, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>((value1, value2, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Action<T1, T2, T3, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> Bind4th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, T4 value4)
        {
            return new Action<T1, T2, T3, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>((value1, value2, value3, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Action<T1, T2, T3, T4, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> Bind5th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, T5 value5)
        {
            return new Action<T1, T2, T3, T4, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>((value1, value2, value3, value4, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Action<T1, T2, T3, T4, T5, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> Bind6th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, T6 value6)
        {
            return new Action<T1, T2, T3, T4, T5, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>((value1, value2, value3, value4, value5, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T8, T9, T10, T11, T12, T13, T14, T15, T16> Bind7th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, T7 value7)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T8, T9, T10, T11, T12, T13, T14, T15, T16>((value1, value2, value3, value4, value5, value6, value8, value9, value10, value11, value12, value13, value14, value15, value16) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T9, T10, T11, T12, T13, T14, T15, T16> Bind8th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, T8 value8)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T9, T10, T11, T12, T13, T14, T15, T16>((value1, value2, value3, value4, value5, value6, value7, value9, value10, value11, value12, value13, value14, value15, value16) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T10, T11, T12, T13, T14, T15, T16> Bind9th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, T9 value9)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T10, T11, T12, T13, T14, T15, T16>((value1, value2, value3, value4, value5, value6, value7, value8, value10, value11, value12, value13, value14, value15, value16) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T11, T12, T13, T14, T15, T16> Bind10th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, T10 value10)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T11, T12, T13, T14, T15, T16>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value11, value12, value13, value14, value15, value16) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T12, T13, T14, T15, T16> Bind11th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, T11 value11)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T12, T13, T14, T15, T16>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value12, value13, value14, value15, value16) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T13, T14, T15, T16> Bind12th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, T12 value12)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T13, T14, T15, T16>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value13, value14, value15, value16) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T14, T15, T16> Bind13th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, T13 value13)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T14, T15, T16>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value14, value15, value16) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T15, T16> Bind14th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, T14 value14)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T15, T16>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value15, value16) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T16> Bind15th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, T15 value15)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T16>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value16) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Bind16th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, T16 value16)
        {
            return new Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15) => action(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Func<TResult> Bind<T, TResult>(this Func<T, TResult> func, T value)
        {
            return new Func<TResult>(() => func(value));
        }

        public static Func<T2, TResult> Bind1st<T1, T2, TResult>(this Func<T1, T2, TResult> func, T1 value1)
        {
            return new Func<T2, TResult>(value2 => func(value1, value2));
        }

        public static Func<T1, TResult> Bind2nd<T1, T2, TResult>(this Func<T1, T2, TResult> func, T2 value2)
        {
            return new Func<T1, TResult>(value1 => func(value1, value2));
        }

        public static Func<T2, T3, TResult> Bind1st<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func, T1 value1)
        {
            return new Func<T2, T3, TResult>((value2, value3) => func(value1, value2, value3));
        }

        public static Func<T1, T3, TResult> Bind2nd<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func, T2 value2)
        {
            return new Func<T1, T3, TResult>((value1, value3) => func(value1, value2, value3));
        }

        public static Func<T1, T2, TResult> Bind3rd<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func, T3 value3)
        {
            return new Func<T1, T2, TResult>((value1, value2) => func(value1, value2, value3));
        }

        public static Func<T2, T3, T4, TResult> Bind1st<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func, T1 value1)
        {
            return new Func<T2, T3, T4, TResult>((value2, value3, value4) => func(value1, value2, value3, value4));
        }

        public static Func<T1, T3, T4, TResult> Bind2nd<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func, T2 value2)
        {
            return new Func<T1, T3, T4, TResult>((value1, value3, value4) => func(value1, value2, value3, value4));
        }

        public static Func<T1, T2, T4, TResult> Bind3rd<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func, T3 value3)
        {
            return new Func<T1, T2, T4, TResult>((value1, value2, value4) => func(value1, value2, value3, value4));
        }

        public static Func<T1, T2, T3, TResult> Bind4th<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func, T4 value4)
        {
            return new Func<T1, T2, T3, TResult>((value1, value2, value3) => func(value1, value2, value3, value4));
        }

        public static Func<T2, T3, T4, T5, TResult> Bind1st<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> func, T1 value1)
        {
            return new Func<T2, T3, T4, T5, TResult>((value2, value3, value4, value5) => func(value1, value2, value3, value4, value5));
        }

        public static Func<T1, T3, T4, T5, TResult> Bind2nd<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> func, T2 value2)
        {
            return new Func<T1, T3, T4, T5, TResult>((value1, value3, value4, value5) => func(value1, value2, value3, value4, value5));
        }

        public static Func<T1, T2, T4, T5, TResult> Bind3rd<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> func, T3 value3)
        {
            return new Func<T1, T2, T4, T5, TResult>((value1, value2, value4, value5) => func(value1, value2, value3, value4, value5));
        }

        public static Func<T1, T2, T3, T5, TResult> Bind4th<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> func, T4 value4)
        {
            return new Func<T1, T2, T3, T5, TResult>((value1, value2, value3, value5) => func(value1, value2, value3, value4, value5));
        }

        public static Func<T1, T2, T3, T4, TResult> Bind5th<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> func, T5 value5)
        {
            return new Func<T1, T2, T3, T4, TResult>((value1, value2, value3, value4) => func(value1, value2, value3, value4, value5));
        }

        public static Func<T2, T3, T4, T5, T6, TResult> Bind1st<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> func, T1 value1)
        {
            return new Func<T2, T3, T4, T5, T6, TResult>((value2, value3, value4, value5, value6) => func(value1, value2, value3, value4, value5, value6));
        }

        public static Func<T1, T3, T4, T5, T6, TResult> Bind2nd<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> func, T2 value2)
        {
            return new Func<T1, T3, T4, T5, T6, TResult>((value1, value3, value4, value5, value6) => func(value1, value2, value3, value4, value5, value6));
        }

        public static Func<T1, T2, T4, T5, T6, TResult> Bind3rd<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> func, T3 value3)
        {
            return new Func<T1, T2, T4, T5, T6, TResult>((value1, value2, value4, value5, value6) => func(value1, value2, value3, value4, value5, value6));
        }

        public static Func<T1, T2, T3, T5, T6, TResult> Bind4th<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> func, T4 value4)
        {
            return new Func<T1, T2, T3, T5, T6, TResult>((value1, value2, value3, value5, value6) => func(value1, value2, value3, value4, value5, value6));
        }

        public static Func<T1, T2, T3, T4, T6, TResult> Bind5th<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> func, T5 value5)
        {
            return new Func<T1, T2, T3, T4, T6, TResult>((value1, value2, value3, value4, value6) => func(value1, value2, value3, value4, value5, value6));
        }

        public static Func<T1, T2, T3, T4, T5, TResult> Bind6th<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> func, T6 value6)
        {
            return new Func<T1, T2, T3, T4, T5, TResult>((value1, value2, value3, value4, value5) => func(value1, value2, value3, value4, value5, value6));
        }

        public static Func<T2, T3, T4, T5, T6, T7, TResult> Bind1st<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, T1 value1)
        {
            return new Func<T2, T3, T4, T5, T6, T7, TResult>((value2, value3, value4, value5, value6, value7) => func(value1, value2, value3, value4, value5, value6, value7));
        }

        public static Func<T1, T3, T4, T5, T6, T7, TResult> Bind2nd<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, T2 value2)
        {
            return new Func<T1, T3, T4, T5, T6, T7, TResult>((value1, value3, value4, value5, value6, value7) => func(value1, value2, value3, value4, value5, value6, value7));
        }

        public static Func<T1, T2, T4, T5, T6, T7, TResult> Bind3rd<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, T3 value3)
        {
            return new Func<T1, T2, T4, T5, T6, T7, TResult>((value1, value2, value4, value5, value6, value7) => func(value1, value2, value3, value4, value5, value6, value7));
        }

        public static Func<T1, T2, T3, T5, T6, T7, TResult> Bind4th<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, T4 value4)
        {
            return new Func<T1, T2, T3, T5, T6, T7, TResult>((value1, value2, value3, value5, value6, value7) => func(value1, value2, value3, value4, value5, value6, value7));
        }

        public static Func<T1, T2, T3, T4, T6, T7, TResult> Bind5th<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, T5 value5)
        {
            return new Func<T1, T2, T3, T4, T6, T7, TResult>((value1, value2, value3, value4, value6, value7) => func(value1, value2, value3, value4, value5, value6, value7));
        }

        public static Func<T1, T2, T3, T4, T5, T7, TResult> Bind6th<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, T6 value6)
        {
            return new Func<T1, T2, T3, T4, T5, T7, TResult>((value1, value2, value3, value4, value5, value7) => func(value1, value2, value3, value4, value5, value6, value7));
        }

        public static Func<T1, T2, T3, T4, T5, T6, TResult> Bind7th<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, T7 value7)
        {
            return new Func<T1, T2, T3, T4, T5, T6, TResult>((value1, value2, value3, value4, value5, value6) => func(value1, value2, value3, value4, value5, value6, value7));
        }

        public static Func<T2, T3, T4, T5, T6, T7, T8, TResult> Bind1st<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, T1 value1)
        {
            return new Func<T2, T3, T4, T5, T6, T7, T8, TResult>((value2, value3, value4, value5, value6, value7, value8) => func(value1, value2, value3, value4, value5, value6, value7, value8));
        }

        public static Func<T1, T3, T4, T5, T6, T7, T8, TResult> Bind2nd<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, T2 value2)
        {
            return new Func<T1, T3, T4, T5, T6, T7, T8, TResult>((value1, value3, value4, value5, value6, value7, value8) => func(value1, value2, value3, value4, value5, value6, value7, value8));
        }

        public static Func<T1, T2, T4, T5, T6, T7, T8, TResult> Bind3rd<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, T3 value3)
        {
            return new Func<T1, T2, T4, T5, T6, T7, T8, TResult>((value1, value2, value4, value5, value6, value7, value8) => func(value1, value2, value3, value4, value5, value6, value7, value8));
        }

        public static Func<T1, T2, T3, T5, T6, T7, T8, TResult> Bind4th<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, T4 value4)
        {
            return new Func<T1, T2, T3, T5, T6, T7, T8, TResult>((value1, value2, value3, value5, value6, value7, value8) => func(value1, value2, value3, value4, value5, value6, value7, value8));
        }

        public static Func<T1, T2, T3, T4, T6, T7, T8, TResult> Bind5th<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, T5 value5)
        {
            return new Func<T1, T2, T3, T4, T6, T7, T8, TResult>((value1, value2, value3, value4, value6, value7, value8) => func(value1, value2, value3, value4, value5, value6, value7, value8));
        }

        public static Func<T1, T2, T3, T4, T5, T7, T8, TResult> Bind6th<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, T6 value6)
        {
            return new Func<T1, T2, T3, T4, T5, T7, T8, TResult>((value1, value2, value3, value4, value5, value7, value8) => func(value1, value2, value3, value4, value5, value6, value7, value8));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T8, TResult> Bind7th<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, T7 value7)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T8, TResult>((value1, value2, value3, value4, value5, value6, value8) => func(value1, value2, value3, value4, value5, value6, value7, value8));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, TResult> Bind8th<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, T8 value8)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, TResult>((value1, value2, value3, value4, value5, value6, value7) => func(value1, value2, value3, value4, value5, value6, value7, value8));
        }

        public static Func<T2, T3, T4, T5, T6, T7, T8, T9, TResult> Bind1st<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func, T1 value1)
        {
            return new Func<T2, T3, T4, T5, T6, T7, T8, T9, TResult>((value2, value3, value4, value5, value6, value7, value8, value9) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9));
        }

        public static Func<T1, T3, T4, T5, T6, T7, T8, T9, TResult> Bind2nd<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func, T2 value2)
        {
            return new Func<T1, T3, T4, T5, T6, T7, T8, T9, TResult>((value1, value3, value4, value5, value6, value7, value8, value9) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9));
        }

        public static Func<T1, T2, T4, T5, T6, T7, T8, T9, TResult> Bind3rd<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func, T3 value3)
        {
            return new Func<T1, T2, T4, T5, T6, T7, T8, T9, TResult>((value1, value2, value4, value5, value6, value7, value8, value9) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9));
        }

        public static Func<T1, T2, T3, T5, T6, T7, T8, T9, TResult> Bind4th<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func, T4 value4)
        {
            return new Func<T1, T2, T3, T5, T6, T7, T8, T9, TResult>((value1, value2, value3, value5, value6, value7, value8, value9) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9));
        }

        public static Func<T1, T2, T3, T4, T6, T7, T8, T9, TResult> Bind5th<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func, T5 value5)
        {
            return new Func<T1, T2, T3, T4, T6, T7, T8, T9, TResult>((value1, value2, value3, value4, value6, value7, value8, value9) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9));
        }

        public static Func<T1, T2, T3, T4, T5, T7, T8, T9, TResult> Bind6th<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func, T6 value6)
        {
            return new Func<T1, T2, T3, T4, T5, T7, T8, T9, TResult>((value1, value2, value3, value4, value5, value7, value8, value9) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T8, T9, TResult> Bind7th<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func, T7 value7)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T8, T9, TResult>((value1, value2, value3, value4, value5, value6, value8, value9) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T9, TResult> Bind8th<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func, T8 value8)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T9, TResult>((value1, value2, value3, value4, value5, value6, value7, value9) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> Bind9th<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func, T9 value9)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>((value1, value2, value3, value4, value5, value6, value7, value8) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9));
        }

        public static Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> Bind1st<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func, T1 value1)
        {
            return new Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>((value2, value3, value4, value5, value6, value7, value8, value9, value10) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10));
        }

        public static Func<T1, T3, T4, T5, T6, T7, T8, T9, T10, TResult> Bind2nd<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func, T2 value2)
        {
            return new Func<T1, T3, T4, T5, T6, T7, T8, T9, T10, TResult>((value1, value3, value4, value5, value6, value7, value8, value9, value10) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10));
        }

        public static Func<T1, T2, T4, T5, T6, T7, T8, T9, T10, TResult> Bind3rd<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func, T3 value3)
        {
            return new Func<T1, T2, T4, T5, T6, T7, T8, T9, T10, TResult>((value1, value2, value4, value5, value6, value7, value8, value9, value10) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10));
        }

        public static Func<T1, T2, T3, T5, T6, T7, T8, T9, T10, TResult> Bind4th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func, T4 value4)
        {
            return new Func<T1, T2, T3, T5, T6, T7, T8, T9, T10, TResult>((value1, value2, value3, value5, value6, value7, value8, value9, value10) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10));
        }

        public static Func<T1, T2, T3, T4, T6, T7, T8, T9, T10, TResult> Bind5th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func, T5 value5)
        {
            return new Func<T1, T2, T3, T4, T6, T7, T8, T9, T10, TResult>((value1, value2, value3, value4, value6, value7, value8, value9, value10) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10));
        }

        public static Func<T1, T2, T3, T4, T5, T7, T8, T9, T10, TResult> Bind6th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func, T6 value6)
        {
            return new Func<T1, T2, T3, T4, T5, T7, T8, T9, T10, TResult>((value1, value2, value3, value4, value5, value7, value8, value9, value10) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T8, T9, T10, TResult> Bind7th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func, T7 value7)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T8, T9, T10, TResult>((value1, value2, value3, value4, value5, value6, value8, value9, value10) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T9, T10, TResult> Bind8th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func, T8 value8)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T9, T10, TResult>((value1, value2, value3, value4, value5, value6, value7, value9, value10) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T10, TResult> Bind9th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func, T9 value9)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T10, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value10) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> Bind10th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func, T10 value10)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value9) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10));
        }

        public static Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> Bind1st<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func, T1 value1)
        {
            return new Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>((value2, value3, value4, value5, value6, value7, value8, value9, value10, value11) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11));
        }

        public static Func<T1, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> Bind2nd<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func, T2 value2)
        {
            return new Func<T1, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>((value1, value3, value4, value5, value6, value7, value8, value9, value10, value11) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11));
        }

        public static Func<T1, T2, T4, T5, T6, T7, T8, T9, T10, T11, TResult> Bind3rd<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func, T3 value3)
        {
            return new Func<T1, T2, T4, T5, T6, T7, T8, T9, T10, T11, TResult>((value1, value2, value4, value5, value6, value7, value8, value9, value10, value11) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11));
        }

        public static Func<T1, T2, T3, T5, T6, T7, T8, T9, T10, T11, TResult> Bind4th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func, T4 value4)
        {
            return new Func<T1, T2, T3, T5, T6, T7, T8, T9, T10, T11, TResult>((value1, value2, value3, value5, value6, value7, value8, value9, value10, value11) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11));
        }

        public static Func<T1, T2, T3, T4, T6, T7, T8, T9, T10, T11, TResult> Bind5th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func, T5 value5)
        {
            return new Func<T1, T2, T3, T4, T6, T7, T8, T9, T10, T11, TResult>((value1, value2, value3, value4, value6, value7, value8, value9, value10, value11) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11));
        }

        public static Func<T1, T2, T3, T4, T5, T7, T8, T9, T10, T11, TResult> Bind6th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func, T6 value6)
        {
            return new Func<T1, T2, T3, T4, T5, T7, T8, T9, T10, T11, TResult>((value1, value2, value3, value4, value5, value7, value8, value9, value10, value11) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T8, T9, T10, T11, TResult> Bind7th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func, T7 value7)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T8, T9, T10, T11, TResult>((value1, value2, value3, value4, value5, value6, value8, value9, value10, value11) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T9, T10, T11, TResult> Bind8th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func, T8 value8)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T9, T10, T11, TResult>((value1, value2, value3, value4, value5, value6, value7, value9, value10, value11) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T10, T11, TResult> Bind9th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func, T9 value9)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T10, T11, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value10, value11) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T11, TResult> Bind10th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func, T10 value10)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T11, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value11) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> Bind11th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func, T11 value11)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11));
        }

        public static Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> Bind1st<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func, T1 value1)
        {
            return new Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>((value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12));
        }

        public static Func<T1, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> Bind2nd<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func, T2 value2)
        {
            return new Func<T1, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>((value1, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12));
        }

        public static Func<T1, T2, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> Bind3rd<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func, T3 value3)
        {
            return new Func<T1, T2, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>((value1, value2, value4, value5, value6, value7, value8, value9, value10, value11, value12) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12));
        }

        public static Func<T1, T2, T3, T5, T6, T7, T8, T9, T10, T11, T12, TResult> Bind4th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func, T4 value4)
        {
            return new Func<T1, T2, T3, T5, T6, T7, T8, T9, T10, T11, T12, TResult>((value1, value2, value3, value5, value6, value7, value8, value9, value10, value11, value12) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12));
        }

        public static Func<T1, T2, T3, T4, T6, T7, T8, T9, T10, T11, T12, TResult> Bind5th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func, T5 value5)
        {
            return new Func<T1, T2, T3, T4, T6, T7, T8, T9, T10, T11, T12, TResult>((value1, value2, value3, value4, value6, value7, value8, value9, value10, value11, value12) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12));
        }

        public static Func<T1, T2, T3, T4, T5, T7, T8, T9, T10, T11, T12, TResult> Bind6th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func, T6 value6)
        {
            return new Func<T1, T2, T3, T4, T5, T7, T8, T9, T10, T11, T12, TResult>((value1, value2, value3, value4, value5, value7, value8, value9, value10, value11, value12) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T8, T9, T10, T11, T12, TResult> Bind7th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func, T7 value7)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T8, T9, T10, T11, T12, TResult>((value1, value2, value3, value4, value5, value6, value8, value9, value10, value11, value12) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T9, T10, T11, T12, TResult> Bind8th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func, T8 value8)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T9, T10, T11, T12, TResult>((value1, value2, value3, value4, value5, value6, value7, value9, value10, value11, value12) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T10, T11, T12, TResult> Bind9th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func, T9 value9)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T10, T11, T12, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value10, value11, value12) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T11, T12, TResult> Bind10th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func, T10 value10)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T11, T12, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value11, value12) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T12, TResult> Bind11th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func, T11 value11)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T12, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value12) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> Bind12th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func, T12 value12)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12));
        }

        public static Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> Bind1st<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func, T1 value1)
        {
            return new Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>((value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13));
        }

        public static Func<T1, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> Bind2nd<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func, T2 value2)
        {
            return new Func<T1, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>((value1, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13));
        }

        public static Func<T1, T2, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> Bind3rd<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func, T3 value3)
        {
            return new Func<T1, T2, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>((value1, value2, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13));
        }

        public static Func<T1, T2, T3, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> Bind4th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func, T4 value4)
        {
            return new Func<T1, T2, T3, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>((value1, value2, value3, value5, value6, value7, value8, value9, value10, value11, value12, value13) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13));
        }

        public static Func<T1, T2, T3, T4, T6, T7, T8, T9, T10, T11, T12, T13, TResult> Bind5th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func, T5 value5)
        {
            return new Func<T1, T2, T3, T4, T6, T7, T8, T9, T10, T11, T12, T13, TResult>((value1, value2, value3, value4, value6, value7, value8, value9, value10, value11, value12, value13) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13));
        }

        public static Func<T1, T2, T3, T4, T5, T7, T8, T9, T10, T11, T12, T13, TResult> Bind6th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func, T6 value6)
        {
            return new Func<T1, T2, T3, T4, T5, T7, T8, T9, T10, T11, T12, T13, TResult>((value1, value2, value3, value4, value5, value7, value8, value9, value10, value11, value12, value13) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T8, T9, T10, T11, T12, T13, TResult> Bind7th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func, T7 value7)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T8, T9, T10, T11, T12, T13, TResult>((value1, value2, value3, value4, value5, value6, value8, value9, value10, value11, value12, value13) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T9, T10, T11, T12, T13, TResult> Bind8th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func, T8 value8)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T9, T10, T11, T12, T13, TResult>((value1, value2, value3, value4, value5, value6, value7, value9, value10, value11, value12, value13) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T10, T11, T12, T13, TResult> Bind9th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func, T9 value9)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T10, T11, T12, T13, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value10, value11, value12, value13) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T11, T12, T13, TResult> Bind10th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func, T10 value10)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T11, T12, T13, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value11, value12, value13) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T12, T13, TResult> Bind11th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func, T11 value11)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T12, T13, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value12, value13) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T13, TResult> Bind12th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func, T12 value12)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T13, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value13) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> Bind13th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func, T13 value13)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13));
        }

        public static Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> Bind1st<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func, T1 value1)
        {
            return new Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>((value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14));
        }

        public static Func<T1, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> Bind2nd<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func, T2 value2)
        {
            return new Func<T1, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>((value1, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14));
        }

        public static Func<T1, T2, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> Bind3rd<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func, T3 value3)
        {
            return new Func<T1, T2, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>((value1, value2, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14));
        }

        public static Func<T1, T2, T3, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> Bind4th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func, T4 value4)
        {
            return new Func<T1, T2, T3, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>((value1, value2, value3, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14));
        }

        public static Func<T1, T2, T3, T4, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> Bind5th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func, T5 value5)
        {
            return new Func<T1, T2, T3, T4, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>((value1, value2, value3, value4, value6, value7, value8, value9, value10, value11, value12, value13, value14) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14));
        }

        public static Func<T1, T2, T3, T4, T5, T7, T8, T9, T10, T11, T12, T13, T14, TResult> Bind6th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func, T6 value6)
        {
            return new Func<T1, T2, T3, T4, T5, T7, T8, T9, T10, T11, T12, T13, T14, TResult>((value1, value2, value3, value4, value5, value7, value8, value9, value10, value11, value12, value13, value14) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T8, T9, T10, T11, T12, T13, T14, TResult> Bind7th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func, T7 value7)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T8, T9, T10, T11, T12, T13, T14, TResult>((value1, value2, value3, value4, value5, value6, value8, value9, value10, value11, value12, value13, value14) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T9, T10, T11, T12, T13, T14, TResult> Bind8th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func, T8 value8)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T9, T10, T11, T12, T13, T14, TResult>((value1, value2, value3, value4, value5, value6, value7, value9, value10, value11, value12, value13, value14) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T10, T11, T12, T13, T14, TResult> Bind9th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func, T9 value9)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T10, T11, T12, T13, T14, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value10, value11, value12, value13, value14) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T11, T12, T13, T14, TResult> Bind10th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func, T10 value10)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T11, T12, T13, T14, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value11, value12, value13, value14) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T12, T13, T14, TResult> Bind11th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func, T11 value11)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T12, T13, T14, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value12, value13, value14) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T13, T14, TResult> Bind12th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func, T12 value12)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T13, T14, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value13, value14) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T14, TResult> Bind13th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func, T13 value13)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T14, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value14) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> Bind14th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func, T14 value14)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14));
        }

        public static Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> Bind1st<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func, T1 value1)
        {
            return new Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>((value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Func<T1, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> Bind2nd<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func, T2 value2)
        {
            return new Func<T1, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>((value1, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Func<T1, T2, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> Bind3rd<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func, T3 value3)
        {
            return new Func<T1, T2, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>((value1, value2, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Func<T1, T2, T3, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> Bind4th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func, T4 value4)
        {
            return new Func<T1, T2, T3, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>((value1, value2, value3, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Func<T1, T2, T3, T4, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> Bind5th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func, T5 value5)
        {
            return new Func<T1, T2, T3, T4, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>((value1, value2, value3, value4, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Func<T1, T2, T3, T4, T5, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> Bind6th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func, T6 value6)
        {
            return new Func<T1, T2, T3, T4, T5, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>((value1, value2, value3, value4, value5, value7, value8, value9, value10, value11, value12, value13, value14, value15) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T8, T9, T10, T11, T12, T13, T14, T15, TResult> Bind7th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func, T7 value7)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T8, T9, T10, T11, T12, T13, T14, T15, TResult>((value1, value2, value3, value4, value5, value6, value8, value9, value10, value11, value12, value13, value14, value15) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T9, T10, T11, T12, T13, T14, T15, TResult> Bind8th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func, T8 value8)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T9, T10, T11, T12, T13, T14, T15, TResult>((value1, value2, value3, value4, value5, value6, value7, value9, value10, value11, value12, value13, value14, value15) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T10, T11, T12, T13, T14, T15, TResult> Bind9th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func, T9 value9)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T10, T11, T12, T13, T14, T15, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value10, value11, value12, value13, value14, value15) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T11, T12, T13, T14, T15, TResult> Bind10th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func, T10 value10)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T11, T12, T13, T14, T15, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value11, value12, value13, value14, value15) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T12, T13, T14, T15, TResult> Bind11th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func, T11 value11)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T12, T13, T14, T15, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value12, value13, value14, value15) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T13, T14, T15, TResult> Bind12th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func, T12 value12)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T13, T14, T15, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value13, value14, value15) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T14, T15, TResult> Bind13th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func, T13 value13)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T14, T15, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value14, value15) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T15, TResult> Bind14th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func, T14 value14)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T15, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value15) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> Bind15th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func, T15 value15)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15));
        }

        public static Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> Bind1st<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func, T1 value1)
        {
            return new Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>((value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Func<T1, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> Bind2nd<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func, T2 value2)
        {
            return new Func<T1, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>((value1, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Func<T1, T2, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> Bind3rd<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func, T3 value3)
        {
            return new Func<T1, T2, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>((value1, value2, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Func<T1, T2, T3, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> Bind4th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func, T4 value4)
        {
            return new Func<T1, T2, T3, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>((value1, value2, value3, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Func<T1, T2, T3, T4, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> Bind5th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func, T5 value5)
        {
            return new Func<T1, T2, T3, T4, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>((value1, value2, value3, value4, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Func<T1, T2, T3, T4, T5, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> Bind6th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func, T6 value6)
        {
            return new Func<T1, T2, T3, T4, T5, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>((value1, value2, value3, value4, value5, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> Bind7th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func, T7 value7)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>((value1, value2, value3, value4, value5, value6, value8, value9, value10, value11, value12, value13, value14, value15, value16) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T9, T10, T11, T12, T13, T14, T15, T16, TResult> Bind8th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func, T8 value8)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T9, T10, T11, T12, T13, T14, T15, T16, TResult>((value1, value2, value3, value4, value5, value6, value7, value9, value10, value11, value12, value13, value14, value15, value16) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T10, T11, T12, T13, T14, T15, T16, TResult> Bind9th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func, T9 value9)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T10, T11, T12, T13, T14, T15, T16, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value10, value11, value12, value13, value14, value15, value16) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T11, T12, T13, T14, T15, T16, TResult> Bind10th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func, T10 value10)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T11, T12, T13, T14, T15, T16, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value11, value12, value13, value14, value15, value16) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T12, T13, T14, T15, T16, TResult> Bind11th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func, T11 value11)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T12, T13, T14, T15, T16, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value12, value13, value14, value15, value16) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T13, T14, T15, T16, TResult> Bind12th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func, T12 value12)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T13, T14, T15, T16, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value13, value14, value15, value16) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T14, T15, T16, TResult> Bind13th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func, T13 value13)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T14, T15, T16, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value14, value15, value16) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T15, T16, TResult> Bind14th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func, T14 value14)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T15, T16, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value15, value16) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T16, TResult> Bind15th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func, T15 value15)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T16, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value16) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> Bind16th<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func, T16 value16)
        {
            return new Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>((value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15) => func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16));
        }
    }
}