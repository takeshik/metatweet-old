// -*- mode: csharp; encoding: utf-8; -*-
/* XSpect Common Framework - Generic utility class library
 * Copyright c 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
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
    partial class Lambda
    {
        public static Func<T1, Action<T2>> Curry1st<T1, T2>(this Action<T1, T2> action)
        {
            return New((T1 value1) => New((T2 value2) => action(value1, value2)));
        }

        public static Func<T2, Action<T1>> Curry2nd<T1, T2>(this Action<T1, T2> action)
        {
            return New((T2 value2) => New((T1 value1) => action(value1, value2)));
        }

        public static Func<TC1, Func<T1, Action>> Curry<TC1, T1>(this Func<TC1, Action<T1>> action)
        {
            return New((TC1 valueC1) => New((T1 value1) => New(() => action(valueC1)(value1))));
        }

        public static Func<T1, Action<T2, T3>> Curry1st<T1, T2, T3>(this Action<T1, T2, T3> action)
        {
            return New((T1 value1) => New((T2 value2, T3 value3) => action(value1, value2, value3)));
        }

        public static Func<T2, Action<T1, T3>> Curry2nd<T1, T2, T3>(this Action<T1, T2, T3> action)
        {
            return New((T2 value2) => New((T1 value1, T3 value3) => action(value1, value2, value3)));
        }

        public static Func<T3, Action<T1, T2>> Curry3rd<T1, T2, T3>(this Action<T1, T2, T3> action)
        {
            return New((T3 value3) => New((T1 value1, T2 value2) => action(value1, value2, value3)));
        }

        public static Func<TC1, Func<T1, Action<T2>>> Curry1st<TC1, T1, T2>(this Func<TC1, Action<T1, T2>> action)
        {
            return New((TC1 valueC1) => New((T1 value1) => New((T2 value2) => action(valueC1)(value1, value2))));
        }

        public static Func<TC1, Func<T2, Action<T1>>> Curry2nd<TC1, T1, T2>(this Func<TC1, Action<T1, T2>> action)
        {
            return New((TC1 valueC1) => New((T2 value2) => New((T1 value1) => action(valueC1)(value1, value2))));
        }

        public static Func<TC1, Func<TC2, Func<T1, Action>>> Curry<TC1, TC2, T1>(this Func<TC1, Func<TC2, Action<T1>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T1 value1) => New(() => action(valueC1)(valueC2)(value1)))));
        }

        public static Func<T1, Action<T2, T3, T4>> Curry1st<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action)
        {
            return New((T1 value1) => New((T2 value2, T3 value3, T4 value4) => action(value1, value2, value3, value4)));
        }

        public static Func<T2, Action<T1, T3, T4>> Curry2nd<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action)
        {
            return New((T2 value2) => New((T1 value1, T3 value3, T4 value4) => action(value1, value2, value3, value4)));
        }

        public static Func<T3, Action<T1, T2, T4>> Curry3rd<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action)
        {
            return New((T3 value3) => New((T1 value1, T2 value2, T4 value4) => action(value1, value2, value3, value4)));
        }

        public static Func<T4, Action<T1, T2, T3>> Curry4th<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action)
        {
            return New((T4 value4) => New((T1 value1, T2 value2, T3 value3) => action(value1, value2, value3, value4)));
        }

        public static Func<TC1, Func<T1, Action<T2, T3>>> Curry1st<TC1, T1, T2, T3>(this Func<TC1, Action<T1, T2, T3>> action)
        {
            return New((TC1 valueC1) => New((T1 value1) => New((T2 value2, T3 value3) => action(valueC1)(value1, value2, value3))));
        }

        public static Func<TC1, Func<T2, Action<T1, T3>>> Curry2nd<TC1, T1, T2, T3>(this Func<TC1, Action<T1, T2, T3>> action)
        {
            return New((TC1 valueC1) => New((T2 value2) => New((T1 value1, T3 value3) => action(valueC1)(value1, value2, value3))));
        }

        public static Func<TC1, Func<T3, Action<T1, T2>>> Curry3rd<TC1, T1, T2, T3>(this Func<TC1, Action<T1, T2, T3>> action)
        {
            return New((TC1 valueC1) => New((T3 value3) => New((T1 value1, T2 value2) => action(valueC1)(value1, value2, value3))));
        }

        public static Func<TC1, Func<TC2, Func<T1, Action<T2>>>> Curry1st<TC1, TC2, T1, T2>(this Func<TC1, Func<TC2, Action<T1, T2>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T1 value1) => New((T2 value2) => action(valueC1)(valueC2)(value1, value2)))));
        }

        public static Func<TC1, Func<TC2, Func<T2, Action<T1>>>> Curry2nd<TC1, TC2, T1, T2>(this Func<TC1, Func<TC2, Action<T1, T2>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T2 value2) => New((T1 value1) => action(valueC1)(valueC2)(value1, value2)))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T1, Action>>>> Curry<TC1, TC2, TC3, T1>(this Func<TC1, Func<TC2, Func<TC3, Action<T1>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T1 value1) => New(() => action(valueC1)(valueC2)(valueC3)(value1))))));
        }

        public static Func<T1, Action<T2, T3, T4, T5>> Curry1st<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action)
        {
            return New((T1 value1) => New((T2 value2, T3 value3, T4 value4, T5 value5) => action(value1, value2, value3, value4, value5)));
        }

        public static Func<T2, Action<T1, T3, T4, T5>> Curry2nd<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action)
        {
            return New((T2 value2) => New((T1 value1, T3 value3, T4 value4, T5 value5) => action(value1, value2, value3, value4, value5)));
        }

        public static Func<T3, Action<T1, T2, T4, T5>> Curry3rd<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action)
        {
            return New((T3 value3) => New((T1 value1, T2 value2, T4 value4, T5 value5) => action(value1, value2, value3, value4, value5)));
        }

        public static Func<T4, Action<T1, T2, T3, T5>> Curry4th<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action)
        {
            return New((T4 value4) => New((T1 value1, T2 value2, T3 value3, T5 value5) => action(value1, value2, value3, value4, value5)));
        }

        public static Func<T5, Action<T1, T2, T3, T4>> Curry5th<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action)
        {
            return New((T5 value5) => New((T1 value1, T2 value2, T3 value3, T4 value4) => action(value1, value2, value3, value4, value5)));
        }

        public static Func<TC1, Func<T1, Action<T2, T3, T4>>> Curry1st<TC1, T1, T2, T3, T4>(this Func<TC1, Action<T1, T2, T3, T4>> action)
        {
            return New((TC1 valueC1) => New((T1 value1) => New((T2 value2, T3 value3, T4 value4) => action(valueC1)(value1, value2, value3, value4))));
        }

        public static Func<TC1, Func<T2, Action<T1, T3, T4>>> Curry2nd<TC1, T1, T2, T3, T4>(this Func<TC1, Action<T1, T2, T3, T4>> action)
        {
            return New((TC1 valueC1) => New((T2 value2) => New((T1 value1, T3 value3, T4 value4) => action(valueC1)(value1, value2, value3, value4))));
        }

        public static Func<TC1, Func<T3, Action<T1, T2, T4>>> Curry3rd<TC1, T1, T2, T3, T4>(this Func<TC1, Action<T1, T2, T3, T4>> action)
        {
            return New((TC1 valueC1) => New((T3 value3) => New((T1 value1, T2 value2, T4 value4) => action(valueC1)(value1, value2, value3, value4))));
        }

        public static Func<TC1, Func<T4, Action<T1, T2, T3>>> Curry4th<TC1, T1, T2, T3, T4>(this Func<TC1, Action<T1, T2, T3, T4>> action)
        {
            return New((TC1 valueC1) => New((T4 value4) => New((T1 value1, T2 value2, T3 value3) => action(valueC1)(value1, value2, value3, value4))));
        }

        public static Func<TC1, Func<TC2, Func<T1, Action<T2, T3>>>> Curry1st<TC1, TC2, T1, T2, T3>(this Func<TC1, Func<TC2, Action<T1, T2, T3>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T1 value1) => New((T2 value2, T3 value3) => action(valueC1)(valueC2)(value1, value2, value3)))));
        }

        public static Func<TC1, Func<TC2, Func<T2, Action<T1, T3>>>> Curry2nd<TC1, TC2, T1, T2, T3>(this Func<TC1, Func<TC2, Action<T1, T2, T3>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T2 value2) => New((T1 value1, T3 value3) => action(valueC1)(valueC2)(value1, value2, value3)))));
        }

        public static Func<TC1, Func<TC2, Func<T3, Action<T1, T2>>>> Curry3rd<TC1, TC2, T1, T2, T3>(this Func<TC1, Func<TC2, Action<T1, T2, T3>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T3 value3) => New((T1 value1, T2 value2) => action(valueC1)(valueC2)(value1, value2, value3)))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T1, Action<T2>>>>> Curry1st<TC1, TC2, TC3, T1, T2>(this Func<TC1, Func<TC2, Func<TC3, Action<T1, T2>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T1 value1) => New((T2 value2) => action(valueC1)(valueC2)(valueC3)(value1, value2))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T2, Action<T1>>>>> Curry2nd<TC1, TC2, TC3, T1, T2>(this Func<TC1, Func<TC2, Func<TC3, Action<T1, T2>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T2 value2) => New((T1 value1) => action(valueC1)(valueC2)(valueC3)(value1, value2))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T1, Action>>>>> Curry<TC1, TC2, TC3, TC4, T1>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Action<T1>>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((T1 value1) => New(() => action(valueC1)(valueC2)(valueC3)(valueC4)(value1)))))));
        }

        public static Func<T1, Action<T2, T3, T4, T5, T6>> Curry1st<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> action)
        {
            return New((T1 value1) => New((T2 value2, T3 value3, T4 value4, T5 value5, T6 value6) => action(value1, value2, value3, value4, value5, value6)));
        }

        public static Func<T2, Action<T1, T3, T4, T5, T6>> Curry2nd<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> action)
        {
            return New((T2 value2) => New((T1 value1, T3 value3, T4 value4, T5 value5, T6 value6) => action(value1, value2, value3, value4, value5, value6)));
        }

        public static Func<T3, Action<T1, T2, T4, T5, T6>> Curry3rd<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> action)
        {
            return New((T3 value3) => New((T1 value1, T2 value2, T4 value4, T5 value5, T6 value6) => action(value1, value2, value3, value4, value5, value6)));
        }

        public static Func<T4, Action<T1, T2, T3, T5, T6>> Curry4th<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> action)
        {
            return New((T4 value4) => New((T1 value1, T2 value2, T3 value3, T5 value5, T6 value6) => action(value1, value2, value3, value4, value5, value6)));
        }

        public static Func<T5, Action<T1, T2, T3, T4, T6>> Curry5th<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> action)
        {
            return New((T5 value5) => New((T1 value1, T2 value2, T3 value3, T4 value4, T6 value6) => action(value1, value2, value3, value4, value5, value6)));
        }

        public static Func<T6, Action<T1, T2, T3, T4, T5>> Curry6th<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> action)
        {
            return New((T6 value6) => New((T1 value1, T2 value2, T3 value3, T4 value4, T5 value5) => action(value1, value2, value3, value4, value5, value6)));
        }

        public static Func<TC1, Func<T1, Action<T2, T3, T4, T5>>> Curry1st<TC1, T1, T2, T3, T4, T5>(this Func<TC1, Action<T1, T2, T3, T4, T5>> action)
        {
            return New((TC1 valueC1) => New((T1 value1) => New((T2 value2, T3 value3, T4 value4, T5 value5) => action(valueC1)(value1, value2, value3, value4, value5))));
        }

        public static Func<TC1, Func<T2, Action<T1, T3, T4, T5>>> Curry2nd<TC1, T1, T2, T3, T4, T5>(this Func<TC1, Action<T1, T2, T3, T4, T5>> action)
        {
            return New((TC1 valueC1) => New((T2 value2) => New((T1 value1, T3 value3, T4 value4, T5 value5) => action(valueC1)(value1, value2, value3, value4, value5))));
        }

        public static Func<TC1, Func<T3, Action<T1, T2, T4, T5>>> Curry3rd<TC1, T1, T2, T3, T4, T5>(this Func<TC1, Action<T1, T2, T3, T4, T5>> action)
        {
            return New((TC1 valueC1) => New((T3 value3) => New((T1 value1, T2 value2, T4 value4, T5 value5) => action(valueC1)(value1, value2, value3, value4, value5))));
        }

        public static Func<TC1, Func<T4, Action<T1, T2, T3, T5>>> Curry4th<TC1, T1, T2, T3, T4, T5>(this Func<TC1, Action<T1, T2, T3, T4, T5>> action)
        {
            return New((TC1 valueC1) => New((T4 value4) => New((T1 value1, T2 value2, T3 value3, T5 value5) => action(valueC1)(value1, value2, value3, value4, value5))));
        }

        public static Func<TC1, Func<T5, Action<T1, T2, T3, T4>>> Curry5th<TC1, T1, T2, T3, T4, T5>(this Func<TC1, Action<T1, T2, T3, T4, T5>> action)
        {
            return New((TC1 valueC1) => New((T5 value5) => New((T1 value1, T2 value2, T3 value3, T4 value4) => action(valueC1)(value1, value2, value3, value4, value5))));
        }

        public static Func<TC1, Func<TC2, Func<T1, Action<T2, T3, T4>>>> Curry1st<TC1, TC2, T1, T2, T3, T4>(this Func<TC1, Func<TC2, Action<T1, T2, T3, T4>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T1 value1) => New((T2 value2, T3 value3, T4 value4) => action(valueC1)(valueC2)(value1, value2, value3, value4)))));
        }

        public static Func<TC1, Func<TC2, Func<T2, Action<T1, T3, T4>>>> Curry2nd<TC1, TC2, T1, T2, T3, T4>(this Func<TC1, Func<TC2, Action<T1, T2, T3, T4>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T2 value2) => New((T1 value1, T3 value3, T4 value4) => action(valueC1)(valueC2)(value1, value2, value3, value4)))));
        }

        public static Func<TC1, Func<TC2, Func<T3, Action<T1, T2, T4>>>> Curry3rd<TC1, TC2, T1, T2, T3, T4>(this Func<TC1, Func<TC2, Action<T1, T2, T3, T4>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T3 value3) => New((T1 value1, T2 value2, T4 value4) => action(valueC1)(valueC2)(value1, value2, value3, value4)))));
        }

        public static Func<TC1, Func<TC2, Func<T4, Action<T1, T2, T3>>>> Curry4th<TC1, TC2, T1, T2, T3, T4>(this Func<TC1, Func<TC2, Action<T1, T2, T3, T4>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T4 value4) => New((T1 value1, T2 value2, T3 value3) => action(valueC1)(valueC2)(value1, value2, value3, value4)))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T1, Action<T2, T3>>>>> Curry1st<TC1, TC2, TC3, T1, T2, T3>(this Func<TC1, Func<TC2, Func<TC3, Action<T1, T2, T3>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T1 value1) => New((T2 value2, T3 value3) => action(valueC1)(valueC2)(valueC3)(value1, value2, value3))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T2, Action<T1, T3>>>>> Curry2nd<TC1, TC2, TC3, T1, T2, T3>(this Func<TC1, Func<TC2, Func<TC3, Action<T1, T2, T3>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T2 value2) => New((T1 value1, T3 value3) => action(valueC1)(valueC2)(valueC3)(value1, value2, value3))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T3, Action<T1, T2>>>>> Curry3rd<TC1, TC2, TC3, T1, T2, T3>(this Func<TC1, Func<TC2, Func<TC3, Action<T1, T2, T3>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T3 value3) => New((T1 value1, T2 value2) => action(valueC1)(valueC2)(valueC3)(value1, value2, value3))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T1, Action<T2>>>>>> Curry1st<TC1, TC2, TC3, TC4, T1, T2>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Action<T1, T2>>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((T1 value1) => New((T2 value2) => action(valueC1)(valueC2)(valueC3)(valueC4)(value1, value2)))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T2, Action<T1>>>>>> Curry2nd<TC1, TC2, TC3, TC4, T1, T2>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Action<T1, T2>>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((T2 value2) => New((T1 value1) => action(valueC1)(valueC2)(valueC3)(valueC4)(value1, value2)))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<T1, Action>>>>>> Curry<TC1, TC2, TC3, TC4, TC5, T1>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Action<T1>>>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((TC5 valueC5) => New((T1 value1) => New(() => action(valueC1)(valueC2)(valueC3)(valueC4)(valueC5)(value1))))))));
        }

        public static Func<T1, Action<T2, T3, T4, T5, T6, T7>> Curry1st<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            return New((T1 value1) => New((T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7) => action(value1, value2, value3, value4, value5, value6, value7)));
        }

        public static Func<T2, Action<T1, T3, T4, T5, T6, T7>> Curry2nd<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            return New((T2 value2) => New((T1 value1, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7) => action(value1, value2, value3, value4, value5, value6, value7)));
        }

        public static Func<T3, Action<T1, T2, T4, T5, T6, T7>> Curry3rd<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            return New((T3 value3) => New((T1 value1, T2 value2, T4 value4, T5 value5, T6 value6, T7 value7) => action(value1, value2, value3, value4, value5, value6, value7)));
        }

        public static Func<T4, Action<T1, T2, T3, T5, T6, T7>> Curry4th<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            return New((T4 value4) => New((T1 value1, T2 value2, T3 value3, T5 value5, T6 value6, T7 value7) => action(value1, value2, value3, value4, value5, value6, value7)));
        }

        public static Func<T5, Action<T1, T2, T3, T4, T6, T7>> Curry5th<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            return New((T5 value5) => New((T1 value1, T2 value2, T3 value3, T4 value4, T6 value6, T7 value7) => action(value1, value2, value3, value4, value5, value6, value7)));
        }

        public static Func<T6, Action<T1, T2, T3, T4, T5, T7>> Curry6th<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            return New((T6 value6) => New((T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T7 value7) => action(value1, value2, value3, value4, value5, value6, value7)));
        }

        public static Func<T7, Action<T1, T2, T3, T4, T5, T6>> Curry7th<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            return New((T7 value7) => New((T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6) => action(value1, value2, value3, value4, value5, value6, value7)));
        }

        public static Func<TC1, Func<T1, Action<T2, T3, T4, T5, T6>>> Curry1st<TC1, T1, T2, T3, T4, T5, T6>(this Func<TC1, Action<T1, T2, T3, T4, T5, T6>> action)
        {
            return New((TC1 valueC1) => New((T1 value1) => New((T2 value2, T3 value3, T4 value4, T5 value5, T6 value6) => action(valueC1)(value1, value2, value3, value4, value5, value6))));
        }

        public static Func<TC1, Func<T2, Action<T1, T3, T4, T5, T6>>> Curry2nd<TC1, T1, T2, T3, T4, T5, T6>(this Func<TC1, Action<T1, T2, T3, T4, T5, T6>> action)
        {
            return New((TC1 valueC1) => New((T2 value2) => New((T1 value1, T3 value3, T4 value4, T5 value5, T6 value6) => action(valueC1)(value1, value2, value3, value4, value5, value6))));
        }

        public static Func<TC1, Func<T3, Action<T1, T2, T4, T5, T6>>> Curry3rd<TC1, T1, T2, T3, T4, T5, T6>(this Func<TC1, Action<T1, T2, T3, T4, T5, T6>> action)
        {
            return New((TC1 valueC1) => New((T3 value3) => New((T1 value1, T2 value2, T4 value4, T5 value5, T6 value6) => action(valueC1)(value1, value2, value3, value4, value5, value6))));
        }

        public static Func<TC1, Func<T4, Action<T1, T2, T3, T5, T6>>> Curry4th<TC1, T1, T2, T3, T4, T5, T6>(this Func<TC1, Action<T1, T2, T3, T4, T5, T6>> action)
        {
            return New((TC1 valueC1) => New((T4 value4) => New((T1 value1, T2 value2, T3 value3, T5 value5, T6 value6) => action(valueC1)(value1, value2, value3, value4, value5, value6))));
        }

        public static Func<TC1, Func<T5, Action<T1, T2, T3, T4, T6>>> Curry5th<TC1, T1, T2, T3, T4, T5, T6>(this Func<TC1, Action<T1, T2, T3, T4, T5, T6>> action)
        {
            return New((TC1 valueC1) => New((T5 value5) => New((T1 value1, T2 value2, T3 value3, T4 value4, T6 value6) => action(valueC1)(value1, value2, value3, value4, value5, value6))));
        }

        public static Func<TC1, Func<T6, Action<T1, T2, T3, T4, T5>>> Curry6th<TC1, T1, T2, T3, T4, T5, T6>(this Func<TC1, Action<T1, T2, T3, T4, T5, T6>> action)
        {
            return New((TC1 valueC1) => New((T6 value6) => New((T1 value1, T2 value2, T3 value3, T4 value4, T5 value5) => action(valueC1)(value1, value2, value3, value4, value5, value6))));
        }

        public static Func<TC1, Func<TC2, Func<T1, Action<T2, T3, T4, T5>>>> Curry1st<TC1, TC2, T1, T2, T3, T4, T5>(this Func<TC1, Func<TC2, Action<T1, T2, T3, T4, T5>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T1 value1) => New((T2 value2, T3 value3, T4 value4, T5 value5) => action(valueC1)(valueC2)(value1, value2, value3, value4, value5)))));
        }

        public static Func<TC1, Func<TC2, Func<T2, Action<T1, T3, T4, T5>>>> Curry2nd<TC1, TC2, T1, T2, T3, T4, T5>(this Func<TC1, Func<TC2, Action<T1, T2, T3, T4, T5>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T2 value2) => New((T1 value1, T3 value3, T4 value4, T5 value5) => action(valueC1)(valueC2)(value1, value2, value3, value4, value5)))));
        }

        public static Func<TC1, Func<TC2, Func<T3, Action<T1, T2, T4, T5>>>> Curry3rd<TC1, TC2, T1, T2, T3, T4, T5>(this Func<TC1, Func<TC2, Action<T1, T2, T3, T4, T5>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T3 value3) => New((T1 value1, T2 value2, T4 value4, T5 value5) => action(valueC1)(valueC2)(value1, value2, value3, value4, value5)))));
        }

        public static Func<TC1, Func<TC2, Func<T4, Action<T1, T2, T3, T5>>>> Curry4th<TC1, TC2, T1, T2, T3, T4, T5>(this Func<TC1, Func<TC2, Action<T1, T2, T3, T4, T5>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T4 value4) => New((T1 value1, T2 value2, T3 value3, T5 value5) => action(valueC1)(valueC2)(value1, value2, value3, value4, value5)))));
        }

        public static Func<TC1, Func<TC2, Func<T5, Action<T1, T2, T3, T4>>>> Curry5th<TC1, TC2, T1, T2, T3, T4, T5>(this Func<TC1, Func<TC2, Action<T1, T2, T3, T4, T5>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T5 value5) => New((T1 value1, T2 value2, T3 value3, T4 value4) => action(valueC1)(valueC2)(value1, value2, value3, value4, value5)))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T1, Action<T2, T3, T4>>>>> Curry1st<TC1, TC2, TC3, T1, T2, T3, T4>(this Func<TC1, Func<TC2, Func<TC3, Action<T1, T2, T3, T4>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T1 value1) => New((T2 value2, T3 value3, T4 value4) => action(valueC1)(valueC2)(valueC3)(value1, value2, value3, value4))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T2, Action<T1, T3, T4>>>>> Curry2nd<TC1, TC2, TC3, T1, T2, T3, T4>(this Func<TC1, Func<TC2, Func<TC3, Action<T1, T2, T3, T4>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T2 value2) => New((T1 value1, T3 value3, T4 value4) => action(valueC1)(valueC2)(valueC3)(value1, value2, value3, value4))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T3, Action<T1, T2, T4>>>>> Curry3rd<TC1, TC2, TC3, T1, T2, T3, T4>(this Func<TC1, Func<TC2, Func<TC3, Action<T1, T2, T3, T4>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T3 value3) => New((T1 value1, T2 value2, T4 value4) => action(valueC1)(valueC2)(valueC3)(value1, value2, value3, value4))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T4, Action<T1, T2, T3>>>>> Curry4th<TC1, TC2, TC3, T1, T2, T3, T4>(this Func<TC1, Func<TC2, Func<TC3, Action<T1, T2, T3, T4>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T4 value4) => New((T1 value1, T2 value2, T3 value3) => action(valueC1)(valueC2)(valueC3)(value1, value2, value3, value4))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T1, Action<T2, T3>>>>>> Curry1st<TC1, TC2, TC3, TC4, T1, T2, T3>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Action<T1, T2, T3>>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((T1 value1) => New((T2 value2, T3 value3) => action(valueC1)(valueC2)(valueC3)(valueC4)(value1, value2, value3)))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T2, Action<T1, T3>>>>>> Curry2nd<TC1, TC2, TC3, TC4, T1, T2, T3>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Action<T1, T2, T3>>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((T2 value2) => New((T1 value1, T3 value3) => action(valueC1)(valueC2)(valueC3)(valueC4)(value1, value2, value3)))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T3, Action<T1, T2>>>>>> Curry3rd<TC1, TC2, TC3, TC4, T1, T2, T3>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Action<T1, T2, T3>>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((T3 value3) => New((T1 value1, T2 value2) => action(valueC1)(valueC2)(valueC3)(valueC4)(value1, value2, value3)))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<T1, Action<T2>>>>>>> Curry1st<TC1, TC2, TC3, TC4, TC5, T1, T2>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Action<T1, T2>>>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((TC5 valueC5) => New((T1 value1) => New((T2 value2) => action(valueC1)(valueC2)(valueC3)(valueC4)(valueC5)(value1, value2))))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<T2, Action<T1>>>>>>> Curry2nd<TC1, TC2, TC3, TC4, TC5, T1, T2>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Action<T1, T2>>>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((TC5 valueC5) => New((T2 value2) => New((T1 value1) => action(valueC1)(valueC2)(valueC3)(valueC4)(valueC5)(value1, value2))))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<TC6, Func<T1, Action>>>>>>> Curry<TC1, TC2, TC3, TC4, TC5, TC6, T1>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<TC6, Action<T1>>>>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((TC5 valueC5) => New((TC6 valueC6) => New((T1 value1) => New(() => action(valueC1)(valueC2)(valueC3)(valueC4)(valueC5)(valueC6)(value1)))))))));
        }

        public static Func<T1, Action<T2, T3, T4, T5, T6, T7, T8>> Curry1st<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            return New((T1 value1) => New((T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8) => action(value1, value2, value3, value4, value5, value6, value7, value8)));
        }

        public static Func<T2, Action<T1, T3, T4, T5, T6, T7, T8>> Curry2nd<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            return New((T2 value2) => New((T1 value1, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8) => action(value1, value2, value3, value4, value5, value6, value7, value8)));
        }

        public static Func<T3, Action<T1, T2, T4, T5, T6, T7, T8>> Curry3rd<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            return New((T3 value3) => New((T1 value1, T2 value2, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8) => action(value1, value2, value3, value4, value5, value6, value7, value8)));
        }

        public static Func<T4, Action<T1, T2, T3, T5, T6, T7, T8>> Curry4th<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            return New((T4 value4) => New((T1 value1, T2 value2, T3 value3, T5 value5, T6 value6, T7 value7, T8 value8) => action(value1, value2, value3, value4, value5, value6, value7, value8)));
        }

        public static Func<T5, Action<T1, T2, T3, T4, T6, T7, T8>> Curry5th<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            return New((T5 value5) => New((T1 value1, T2 value2, T3 value3, T4 value4, T6 value6, T7 value7, T8 value8) => action(value1, value2, value3, value4, value5, value6, value7, value8)));
        }

        public static Func<T6, Action<T1, T2, T3, T4, T5, T7, T8>> Curry6th<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            return New((T6 value6) => New((T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T7 value7, T8 value8) => action(value1, value2, value3, value4, value5, value6, value7, value8)));
        }

        public static Func<T7, Action<T1, T2, T3, T4, T5, T6, T8>> Curry7th<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            return New((T7 value7) => New((T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T8 value8) => action(value1, value2, value3, value4, value5, value6, value7, value8)));
        }

        public static Func<T8, Action<T1, T2, T3, T4, T5, T6, T7>> Curry8th<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            return New((T8 value8) => New((T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7) => action(value1, value2, value3, value4, value5, value6, value7, value8)));
        }

        public static Func<TC1, Func<T1, Action<T2, T3, T4, T5, T6, T7>>> Curry1st<TC1, T1, T2, T3, T4, T5, T6, T7>(this Func<TC1, Action<T1, T2, T3, T4, T5, T6, T7>> action)
        {
            return New((TC1 valueC1) => New((T1 value1) => New((T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7) => action(valueC1)(value1, value2, value3, value4, value5, value6, value7))));
        }

        public static Func<TC1, Func<T2, Action<T1, T3, T4, T5, T6, T7>>> Curry2nd<TC1, T1, T2, T3, T4, T5, T6, T7>(this Func<TC1, Action<T1, T2, T3, T4, T5, T6, T7>> action)
        {
            return New((TC1 valueC1) => New((T2 value2) => New((T1 value1, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7) => action(valueC1)(value1, value2, value3, value4, value5, value6, value7))));
        }

        public static Func<TC1, Func<T3, Action<T1, T2, T4, T5, T6, T7>>> Curry3rd<TC1, T1, T2, T3, T4, T5, T6, T7>(this Func<TC1, Action<T1, T2, T3, T4, T5, T6, T7>> action)
        {
            return New((TC1 valueC1) => New((T3 value3) => New((T1 value1, T2 value2, T4 value4, T5 value5, T6 value6, T7 value7) => action(valueC1)(value1, value2, value3, value4, value5, value6, value7))));
        }

        public static Func<TC1, Func<T4, Action<T1, T2, T3, T5, T6, T7>>> Curry4th<TC1, T1, T2, T3, T4, T5, T6, T7>(this Func<TC1, Action<T1, T2, T3, T4, T5, T6, T7>> action)
        {
            return New((TC1 valueC1) => New((T4 value4) => New((T1 value1, T2 value2, T3 value3, T5 value5, T6 value6, T7 value7) => action(valueC1)(value1, value2, value3, value4, value5, value6, value7))));
        }

        public static Func<TC1, Func<T5, Action<T1, T2, T3, T4, T6, T7>>> Curry5th<TC1, T1, T2, T3, T4, T5, T6, T7>(this Func<TC1, Action<T1, T2, T3, T4, T5, T6, T7>> action)
        {
            return New((TC1 valueC1) => New((T5 value5) => New((T1 value1, T2 value2, T3 value3, T4 value4, T6 value6, T7 value7) => action(valueC1)(value1, value2, value3, value4, value5, value6, value7))));
        }

        public static Func<TC1, Func<T6, Action<T1, T2, T3, T4, T5, T7>>> Curry6th<TC1, T1, T2, T3, T4, T5, T6, T7>(this Func<TC1, Action<T1, T2, T3, T4, T5, T6, T7>> action)
        {
            return New((TC1 valueC1) => New((T6 value6) => New((T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T7 value7) => action(valueC1)(value1, value2, value3, value4, value5, value6, value7))));
        }

        public static Func<TC1, Func<T7, Action<T1, T2, T3, T4, T5, T6>>> Curry7th<TC1, T1, T2, T3, T4, T5, T6, T7>(this Func<TC1, Action<T1, T2, T3, T4, T5, T6, T7>> action)
        {
            return New((TC1 valueC1) => New((T7 value7) => New((T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6) => action(valueC1)(value1, value2, value3, value4, value5, value6, value7))));
        }

        public static Func<TC1, Func<TC2, Func<T1, Action<T2, T3, T4, T5, T6>>>> Curry1st<TC1, TC2, T1, T2, T3, T4, T5, T6>(this Func<TC1, Func<TC2, Action<T1, T2, T3, T4, T5, T6>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T1 value1) => New((T2 value2, T3 value3, T4 value4, T5 value5, T6 value6) => action(valueC1)(valueC2)(value1, value2, value3, value4, value5, value6)))));
        }

        public static Func<TC1, Func<TC2, Func<T2, Action<T1, T3, T4, T5, T6>>>> Curry2nd<TC1, TC2, T1, T2, T3, T4, T5, T6>(this Func<TC1, Func<TC2, Action<T1, T2, T3, T4, T5, T6>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T2 value2) => New((T1 value1, T3 value3, T4 value4, T5 value5, T6 value6) => action(valueC1)(valueC2)(value1, value2, value3, value4, value5, value6)))));
        }

        public static Func<TC1, Func<TC2, Func<T3, Action<T1, T2, T4, T5, T6>>>> Curry3rd<TC1, TC2, T1, T2, T3, T4, T5, T6>(this Func<TC1, Func<TC2, Action<T1, T2, T3, T4, T5, T6>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T3 value3) => New((T1 value1, T2 value2, T4 value4, T5 value5, T6 value6) => action(valueC1)(valueC2)(value1, value2, value3, value4, value5, value6)))));
        }

        public static Func<TC1, Func<TC2, Func<T4, Action<T1, T2, T3, T5, T6>>>> Curry4th<TC1, TC2, T1, T2, T3, T4, T5, T6>(this Func<TC1, Func<TC2, Action<T1, T2, T3, T4, T5, T6>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T4 value4) => New((T1 value1, T2 value2, T3 value3, T5 value5, T6 value6) => action(valueC1)(valueC2)(value1, value2, value3, value4, value5, value6)))));
        }

        public static Func<TC1, Func<TC2, Func<T5, Action<T1, T2, T3, T4, T6>>>> Curry5th<TC1, TC2, T1, T2, T3, T4, T5, T6>(this Func<TC1, Func<TC2, Action<T1, T2, T3, T4, T5, T6>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T5 value5) => New((T1 value1, T2 value2, T3 value3, T4 value4, T6 value6) => action(valueC1)(valueC2)(value1, value2, value3, value4, value5, value6)))));
        }

        public static Func<TC1, Func<TC2, Func<T6, Action<T1, T2, T3, T4, T5>>>> Curry6th<TC1, TC2, T1, T2, T3, T4, T5, T6>(this Func<TC1, Func<TC2, Action<T1, T2, T3, T4, T5, T6>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T6 value6) => New((T1 value1, T2 value2, T3 value3, T4 value4, T5 value5) => action(valueC1)(valueC2)(value1, value2, value3, value4, value5, value6)))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T1, Action<T2, T3, T4, T5>>>>> Curry1st<TC1, TC2, TC3, T1, T2, T3, T4, T5>(this Func<TC1, Func<TC2, Func<TC3, Action<T1, T2, T3, T4, T5>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T1 value1) => New((T2 value2, T3 value3, T4 value4, T5 value5) => action(valueC1)(valueC2)(valueC3)(value1, value2, value3, value4, value5))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T2, Action<T1, T3, T4, T5>>>>> Curry2nd<TC1, TC2, TC3, T1, T2, T3, T4, T5>(this Func<TC1, Func<TC2, Func<TC3, Action<T1, T2, T3, T4, T5>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T2 value2) => New((T1 value1, T3 value3, T4 value4, T5 value5) => action(valueC1)(valueC2)(valueC3)(value1, value2, value3, value4, value5))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T3, Action<T1, T2, T4, T5>>>>> Curry3rd<TC1, TC2, TC3, T1, T2, T3, T4, T5>(this Func<TC1, Func<TC2, Func<TC3, Action<T1, T2, T3, T4, T5>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T3 value3) => New((T1 value1, T2 value2, T4 value4, T5 value5) => action(valueC1)(valueC2)(valueC3)(value1, value2, value3, value4, value5))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T4, Action<T1, T2, T3, T5>>>>> Curry4th<TC1, TC2, TC3, T1, T2, T3, T4, T5>(this Func<TC1, Func<TC2, Func<TC3, Action<T1, T2, T3, T4, T5>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T4 value4) => New((T1 value1, T2 value2, T3 value3, T5 value5) => action(valueC1)(valueC2)(valueC3)(value1, value2, value3, value4, value5))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T5, Action<T1, T2, T3, T4>>>>> Curry5th<TC1, TC2, TC3, T1, T2, T3, T4, T5>(this Func<TC1, Func<TC2, Func<TC3, Action<T1, T2, T3, T4, T5>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T5 value5) => New((T1 value1, T2 value2, T3 value3, T4 value4) => action(valueC1)(valueC2)(valueC3)(value1, value2, value3, value4, value5))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T1, Action<T2, T3, T4>>>>>> Curry1st<TC1, TC2, TC3, TC4, T1, T2, T3, T4>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Action<T1, T2, T3, T4>>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((T1 value1) => New((T2 value2, T3 value3, T4 value4) => action(valueC1)(valueC2)(valueC3)(valueC4)(value1, value2, value3, value4)))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T2, Action<T1, T3, T4>>>>>> Curry2nd<TC1, TC2, TC3, TC4, T1, T2, T3, T4>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Action<T1, T2, T3, T4>>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((T2 value2) => New((T1 value1, T3 value3, T4 value4) => action(valueC1)(valueC2)(valueC3)(valueC4)(value1, value2, value3, value4)))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T3, Action<T1, T2, T4>>>>>> Curry3rd<TC1, TC2, TC3, TC4, T1, T2, T3, T4>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Action<T1, T2, T3, T4>>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((T3 value3) => New((T1 value1, T2 value2, T4 value4) => action(valueC1)(valueC2)(valueC3)(valueC4)(value1, value2, value3, value4)))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T4, Action<T1, T2, T3>>>>>> Curry4th<TC1, TC2, TC3, TC4, T1, T2, T3, T4>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Action<T1, T2, T3, T4>>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((T4 value4) => New((T1 value1, T2 value2, T3 value3) => action(valueC1)(valueC2)(valueC3)(valueC4)(value1, value2, value3, value4)))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<T1, Action<T2, T3>>>>>>> Curry1st<TC1, TC2, TC3, TC4, TC5, T1, T2, T3>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Action<T1, T2, T3>>>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((TC5 valueC5) => New((T1 value1) => New((T2 value2, T3 value3) => action(valueC1)(valueC2)(valueC3)(valueC4)(valueC5)(value1, value2, value3))))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<T2, Action<T1, T3>>>>>>> Curry2nd<TC1, TC2, TC3, TC4, TC5, T1, T2, T3>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Action<T1, T2, T3>>>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((TC5 valueC5) => New((T2 value2) => New((T1 value1, T3 value3) => action(valueC1)(valueC2)(valueC3)(valueC4)(valueC5)(value1, value2, value3))))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<T3, Action<T1, T2>>>>>>> Curry3rd<TC1, TC2, TC3, TC4, TC5, T1, T2, T3>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Action<T1, T2, T3>>>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((TC5 valueC5) => New((T3 value3) => New((T1 value1, T2 value2) => action(valueC1)(valueC2)(valueC3)(valueC4)(valueC5)(value1, value2, value3))))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<TC6, Func<T1, Action<T2>>>>>>>> Curry1st<TC1, TC2, TC3, TC4, TC5, TC6, T1, T2>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<TC6, Action<T1, T2>>>>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((TC5 valueC5) => New((TC6 valueC6) => New((T1 value1) => New((T2 value2) => action(valueC1)(valueC2)(valueC3)(valueC4)(valueC5)(valueC6)(value1, value2)))))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<TC6, Func<T2, Action<T1>>>>>>>> Curry2nd<TC1, TC2, TC3, TC4, TC5, TC6, T1, T2>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<TC6, Action<T1, T2>>>>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((TC5 valueC5) => New((TC6 valueC6) => New((T2 value2) => New((T1 value1) => action(valueC1)(valueC2)(valueC3)(valueC4)(valueC5)(valueC6)(value1, value2)))))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<TC6, Func<TC7, Func<T1, Action>>>>>>>> Curry<TC1, TC2, TC3, TC4, TC5, TC6, TC7, T1>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<TC6, Func<TC7, Action<T1>>>>>>>> action)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((TC5 valueC5) => New((TC6 valueC6) => New((TC7 valueC7) => New((T1 value1) => New(() => action(valueC1)(valueC2)(valueC3)(valueC4)(valueC5)(valueC6)(valueC7)(value1))))))))));
        }



        public static Func<T1, Func<T2, TResult>> Curry1st<T1, T2, TResult>(this Func<T1, T2, TResult> func)
        {
            return New((T1 value1) => New((T2 value2) => func(value1, value2)));
        }

        public static Func<T2, Func<T1, TResult>> Curry2nd<T1, T2, TResult>(this Func<T1, T2, TResult> func)
        {
            return New((T2 value2) => New((T1 value1) => func(value1, value2)));
        }

        public static Func<TC1, Func<T1, Func<TResult>>> Curry<TC1, T1, TResult>(this Func<TC1, Func<T1, TResult>> func)
        {
            return New((TC1 valueC1) => New((T1 value1) => New(() => func(valueC1)(value1))));
        }

        public static Func<T1, Func<T2, T3, TResult>> Curry1st<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func)
        {
            return New((T1 value1) => New((T2 value2, T3 value3) => func(value1, value2, value3)));
        }

        public static Func<T2, Func<T1, T3, TResult>> Curry2nd<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func)
        {
            return New((T2 value2) => New((T1 value1, T3 value3) => func(value1, value2, value3)));
        }

        public static Func<T3, Func<T1, T2, TResult>> Curry3rd<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func)
        {
            return New((T3 value3) => New((T1 value1, T2 value2) => func(value1, value2, value3)));
        }

        public static Func<TC1, Func<T1, Func<T2, TResult>>> Curry1st<TC1, T1, T2, TResult>(this Func<TC1, Func<T1, T2, TResult>> func)
        {
            return New((TC1 valueC1) => New((T1 value1) => New((T2 value2) => func(valueC1)(value1, value2))));
        }

        public static Func<TC1, Func<T2, Func<T1, TResult>>> Curry2nd<TC1, T1, T2, TResult>(this Func<TC1, Func<T1, T2, TResult>> func)
        {
            return New((TC1 valueC1) => New((T2 value2) => New((T1 value1) => func(valueC1)(value1, value2))));
        }

        public static Func<TC1, Func<TC2, Func<T1, Func<TResult>>>> Curry<TC1, TC2, T1, TResult>(this Func<TC1, Func<TC2, Func<T1, TResult>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T1 value1) => New(() => func(valueC1)(valueC2)(value1)))));
        }

        public static Func<T1, Func<T2, T3, T4, TResult>> Curry1st<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func)
        {
            return New((T1 value1) => New((T2 value2, T3 value3, T4 value4) => func(value1, value2, value3, value4)));
        }

        public static Func<T2, Func<T1, T3, T4, TResult>> Curry2nd<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func)
        {
            return New((T2 value2) => New((T1 value1, T3 value3, T4 value4) => func(value1, value2, value3, value4)));
        }

        public static Func<T3, Func<T1, T2, T4, TResult>> Curry3rd<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func)
        {
            return New((T3 value3) => New((T1 value1, T2 value2, T4 value4) => func(value1, value2, value3, value4)));
        }

        public static Func<T4, Func<T1, T2, T3, TResult>> Curry4th<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func)
        {
            return New((T4 value4) => New((T1 value1, T2 value2, T3 value3) => func(value1, value2, value3, value4)));
        }

        public static Func<TC1, Func<T1, Func<T2, T3, TResult>>> Curry1st<TC1, T1, T2, T3, TResult>(this Func<TC1, Func<T1, T2, T3, TResult>> func)
        {
            return New((TC1 valueC1) => New((T1 value1) => New((T2 value2, T3 value3) => func(valueC1)(value1, value2, value3))));
        }

        public static Func<TC1, Func<T2, Func<T1, T3, TResult>>> Curry2nd<TC1, T1, T2, T3, TResult>(this Func<TC1, Func<T1, T2, T3, TResult>> func)
        {
            return New((TC1 valueC1) => New((T2 value2) => New((T1 value1, T3 value3) => func(valueC1)(value1, value2, value3))));
        }

        public static Func<TC1, Func<T3, Func<T1, T2, TResult>>> Curry3rd<TC1, T1, T2, T3, TResult>(this Func<TC1, Func<T1, T2, T3, TResult>> func)
        {
            return New((TC1 valueC1) => New((T3 value3) => New((T1 value1, T2 value2) => func(valueC1)(value1, value2, value3))));
        }

        public static Func<TC1, Func<TC2, Func<T1, Func<T2, TResult>>>> Curry1st<TC1, TC2, T1, T2, TResult>(this Func<TC1, Func<TC2, Func<T1, T2, TResult>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T1 value1) => New((T2 value2) => func(valueC1)(valueC2)(value1, value2)))));
        }

        public static Func<TC1, Func<TC2, Func<T2, Func<T1, TResult>>>> Curry2nd<TC1, TC2, T1, T2, TResult>(this Func<TC1, Func<TC2, Func<T1, T2, TResult>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T2 value2) => New((T1 value1) => func(valueC1)(valueC2)(value1, value2)))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T1, Func<TResult>>>>> Curry<TC1, TC2, TC3, T1, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<T1, TResult>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T1 value1) => New(() => func(valueC1)(valueC2)(valueC3)(value1))))));
        }

        public static Func<T1, Func<T2, T3, T4, T5, TResult>> Curry1st<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> func)
        {
            return New((T1 value1) => New((T2 value2, T3 value3, T4 value4, T5 value5) => func(value1, value2, value3, value4, value5)));
        }

        public static Func<T2, Func<T1, T3, T4, T5, TResult>> Curry2nd<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> func)
        {
            return New((T2 value2) => New((T1 value1, T3 value3, T4 value4, T5 value5) => func(value1, value2, value3, value4, value5)));
        }

        public static Func<T3, Func<T1, T2, T4, T5, TResult>> Curry3rd<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> func)
        {
            return New((T3 value3) => New((T1 value1, T2 value2, T4 value4, T5 value5) => func(value1, value2, value3, value4, value5)));
        }

        public static Func<T4, Func<T1, T2, T3, T5, TResult>> Curry4th<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> func)
        {
            return New((T4 value4) => New((T1 value1, T2 value2, T3 value3, T5 value5) => func(value1, value2, value3, value4, value5)));
        }

        public static Func<T5, Func<T1, T2, T3, T4, TResult>> Curry5th<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> func)
        {
            return New((T5 value5) => New((T1 value1, T2 value2, T3 value3, T4 value4) => func(value1, value2, value3, value4, value5)));
        }

        public static Func<TC1, Func<T1, Func<T2, T3, T4, TResult>>> Curry1st<TC1, T1, T2, T3, T4, TResult>(this Func<TC1, Func<T1, T2, T3, T4, TResult>> func)
        {
            return New((TC1 valueC1) => New((T1 value1) => New((T2 value2, T3 value3, T4 value4) => func(valueC1)(value1, value2, value3, value4))));
        }

        public static Func<TC1, Func<T2, Func<T1, T3, T4, TResult>>> Curry2nd<TC1, T1, T2, T3, T4, TResult>(this Func<TC1, Func<T1, T2, T3, T4, TResult>> func)
        {
            return New((TC1 valueC1) => New((T2 value2) => New((T1 value1, T3 value3, T4 value4) => func(valueC1)(value1, value2, value3, value4))));
        }

        public static Func<TC1, Func<T3, Func<T1, T2, T4, TResult>>> Curry3rd<TC1, T1, T2, T3, T4, TResult>(this Func<TC1, Func<T1, T2, T3, T4, TResult>> func)
        {
            return New((TC1 valueC1) => New((T3 value3) => New((T1 value1, T2 value2, T4 value4) => func(valueC1)(value1, value2, value3, value4))));
        }

        public static Func<TC1, Func<T4, Func<T1, T2, T3, TResult>>> Curry4th<TC1, T1, T2, T3, T4, TResult>(this Func<TC1, Func<T1, T2, T3, T4, TResult>> func)
        {
            return New((TC1 valueC1) => New((T4 value4) => New((T1 value1, T2 value2, T3 value3) => func(valueC1)(value1, value2, value3, value4))));
        }

        public static Func<TC1, Func<TC2, Func<T1, Func<T2, T3, TResult>>>> Curry1st<TC1, TC2, T1, T2, T3, TResult>(this Func<TC1, Func<TC2, Func<T1, T2, T3, TResult>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T1 value1) => New((T2 value2, T3 value3) => func(valueC1)(valueC2)(value1, value2, value3)))));
        }

        public static Func<TC1, Func<TC2, Func<T2, Func<T1, T3, TResult>>>> Curry2nd<TC1, TC2, T1, T2, T3, TResult>(this Func<TC1, Func<TC2, Func<T1, T2, T3, TResult>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T2 value2) => New((T1 value1, T3 value3) => func(valueC1)(valueC2)(value1, value2, value3)))));
        }

        public static Func<TC1, Func<TC2, Func<T3, Func<T1, T2, TResult>>>> Curry3rd<TC1, TC2, T1, T2, T3, TResult>(this Func<TC1, Func<TC2, Func<T1, T2, T3, TResult>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T3 value3) => New((T1 value1, T2 value2) => func(valueC1)(valueC2)(value1, value2, value3)))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T1, Func<T2, TResult>>>>> Curry1st<TC1, TC2, TC3, T1, T2, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<T1, T2, TResult>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T1 value1) => New((T2 value2) => func(valueC1)(valueC2)(valueC3)(value1, value2))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T2, Func<T1, TResult>>>>> Curry2nd<TC1, TC2, TC3, T1, T2, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<T1, T2, TResult>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T2 value2) => New((T1 value1) => func(valueC1)(valueC2)(valueC3)(value1, value2))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T1, Func<TResult>>>>>> Curry<TC1, TC2, TC3, TC4, T1, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T1, TResult>>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((T1 value1) => New(() => func(valueC1)(valueC2)(valueC3)(valueC4)(value1)))))));
        }

        public static Func<T1, Func<T2, T3, T4, T5, T6, TResult>> Curry1st<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> func)
        {
            return New((T1 value1) => New((T2 value2, T3 value3, T4 value4, T5 value5, T6 value6) => func(value1, value2, value3, value4, value5, value6)));
        }

        public static Func<T2, Func<T1, T3, T4, T5, T6, TResult>> Curry2nd<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> func)
        {
            return New((T2 value2) => New((T1 value1, T3 value3, T4 value4, T5 value5, T6 value6) => func(value1, value2, value3, value4, value5, value6)));
        }

        public static Func<T3, Func<T1, T2, T4, T5, T6, TResult>> Curry3rd<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> func)
        {
            return New((T3 value3) => New((T1 value1, T2 value2, T4 value4, T5 value5, T6 value6) => func(value1, value2, value3, value4, value5, value6)));
        }

        public static Func<T4, Func<T1, T2, T3, T5, T6, TResult>> Curry4th<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> func)
        {
            return New((T4 value4) => New((T1 value1, T2 value2, T3 value3, T5 value5, T6 value6) => func(value1, value2, value3, value4, value5, value6)));
        }

        public static Func<T5, Func<T1, T2, T3, T4, T6, TResult>> Curry5th<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> func)
        {
            return New((T5 value5) => New((T1 value1, T2 value2, T3 value3, T4 value4, T6 value6) => func(value1, value2, value3, value4, value5, value6)));
        }

        public static Func<T6, Func<T1, T2, T3, T4, T5, TResult>> Curry6th<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> func)
        {
            return New((T6 value6) => New((T1 value1, T2 value2, T3 value3, T4 value4, T5 value5) => func(value1, value2, value3, value4, value5, value6)));
        }

        public static Func<TC1, Func<T1, Func<T2, T3, T4, T5, TResult>>> Curry1st<TC1, T1, T2, T3, T4, T5, TResult>(this Func<TC1, Func<T1, T2, T3, T4, T5, TResult>> func)
        {
            return New((TC1 valueC1) => New((T1 value1) => New((T2 value2, T3 value3, T4 value4, T5 value5) => func(valueC1)(value1, value2, value3, value4, value5))));
        }

        public static Func<TC1, Func<T2, Func<T1, T3, T4, T5, TResult>>> Curry2nd<TC1, T1, T2, T3, T4, T5, TResult>(this Func<TC1, Func<T1, T2, T3, T4, T5, TResult>> func)
        {
            return New((TC1 valueC1) => New((T2 value2) => New((T1 value1, T3 value3, T4 value4, T5 value5) => func(valueC1)(value1, value2, value3, value4, value5))));
        }

        public static Func<TC1, Func<T3, Func<T1, T2, T4, T5, TResult>>> Curry3rd<TC1, T1, T2, T3, T4, T5, TResult>(this Func<TC1, Func<T1, T2, T3, T4, T5, TResult>> func)
        {
            return New((TC1 valueC1) => New((T3 value3) => New((T1 value1, T2 value2, T4 value4, T5 value5) => func(valueC1)(value1, value2, value3, value4, value5))));
        }

        public static Func<TC1, Func<T4, Func<T1, T2, T3, T5, TResult>>> Curry4th<TC1, T1, T2, T3, T4, T5, TResult>(this Func<TC1, Func<T1, T2, T3, T4, T5, TResult>> func)
        {
            return New((TC1 valueC1) => New((T4 value4) => New((T1 value1, T2 value2, T3 value3, T5 value5) => func(valueC1)(value1, value2, value3, value4, value5))));
        }

        public static Func<TC1, Func<T5, Func<T1, T2, T3, T4, TResult>>> Curry5th<TC1, T1, T2, T3, T4, T5, TResult>(this Func<TC1, Func<T1, T2, T3, T4, T5, TResult>> func)
        {
            return New((TC1 valueC1) => New((T5 value5) => New((T1 value1, T2 value2, T3 value3, T4 value4) => func(valueC1)(value1, value2, value3, value4, value5))));
        }

        public static Func<TC1, Func<TC2, Func<T1, Func<T2, T3, T4, TResult>>>> Curry1st<TC1, TC2, T1, T2, T3, T4, TResult>(this Func<TC1, Func<TC2, Func<T1, T2, T3, T4, TResult>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T1 value1) => New((T2 value2, T3 value3, T4 value4) => func(valueC1)(valueC2)(value1, value2, value3, value4)))));
        }

        public static Func<TC1, Func<TC2, Func<T2, Func<T1, T3, T4, TResult>>>> Curry2nd<TC1, TC2, T1, T2, T3, T4, TResult>(this Func<TC1, Func<TC2, Func<T1, T2, T3, T4, TResult>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T2 value2) => New((T1 value1, T3 value3, T4 value4) => func(valueC1)(valueC2)(value1, value2, value3, value4)))));
        }

        public static Func<TC1, Func<TC2, Func<T3, Func<T1, T2, T4, TResult>>>> Curry3rd<TC1, TC2, T1, T2, T3, T4, TResult>(this Func<TC1, Func<TC2, Func<T1, T2, T3, T4, TResult>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T3 value3) => New((T1 value1, T2 value2, T4 value4) => func(valueC1)(valueC2)(value1, value2, value3, value4)))));
        }

        public static Func<TC1, Func<TC2, Func<T4, Func<T1, T2, T3, TResult>>>> Curry4th<TC1, TC2, T1, T2, T3, T4, TResult>(this Func<TC1, Func<TC2, Func<T1, T2, T3, T4, TResult>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T4 value4) => New((T1 value1, T2 value2, T3 value3) => func(valueC1)(valueC2)(value1, value2, value3, value4)))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T1, Func<T2, T3, TResult>>>>> Curry1st<TC1, TC2, TC3, T1, T2, T3, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<T1, T2, T3, TResult>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T1 value1) => New((T2 value2, T3 value3) => func(valueC1)(valueC2)(valueC3)(value1, value2, value3))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T2, Func<T1, T3, TResult>>>>> Curry2nd<TC1, TC2, TC3, T1, T2, T3, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<T1, T2, T3, TResult>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T2 value2) => New((T1 value1, T3 value3) => func(valueC1)(valueC2)(valueC3)(value1, value2, value3))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T3, Func<T1, T2, TResult>>>>> Curry3rd<TC1, TC2, TC3, T1, T2, T3, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<T1, T2, T3, TResult>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T3 value3) => New((T1 value1, T2 value2) => func(valueC1)(valueC2)(valueC3)(value1, value2, value3))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T1, Func<T2, TResult>>>>>> Curry1st<TC1, TC2, TC3, TC4, T1, T2, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T1, T2, TResult>>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((T1 value1) => New((T2 value2) => func(valueC1)(valueC2)(valueC3)(valueC4)(value1, value2)))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T2, Func<T1, TResult>>>>>> Curry2nd<TC1, TC2, TC3, TC4, T1, T2, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T1, T2, TResult>>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((T2 value2) => New((T1 value1) => func(valueC1)(valueC2)(valueC3)(valueC4)(value1, value2)))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<T1, Func<TResult>>>>>>> Curry<TC1, TC2, TC3, TC4, TC5, T1, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<T1, TResult>>>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((TC5 valueC5) => New((T1 value1) => New(() => func(valueC1)(valueC2)(valueC3)(valueC4)(valueC5)(value1))))))));
        }

        public static Func<T1, Func<T2, T3, T4, T5, T6, T7, TResult>> Curry1st<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> func)
        {
            return New((T1 value1) => New((T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7) => func(value1, value2, value3, value4, value5, value6, value7)));
        }

        public static Func<T2, Func<T1, T3, T4, T5, T6, T7, TResult>> Curry2nd<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> func)
        {
            return New((T2 value2) => New((T1 value1, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7) => func(value1, value2, value3, value4, value5, value6, value7)));
        }

        public static Func<T3, Func<T1, T2, T4, T5, T6, T7, TResult>> Curry3rd<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> func)
        {
            return New((T3 value3) => New((T1 value1, T2 value2, T4 value4, T5 value5, T6 value6, T7 value7) => func(value1, value2, value3, value4, value5, value6, value7)));
        }

        public static Func<T4, Func<T1, T2, T3, T5, T6, T7, TResult>> Curry4th<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> func)
        {
            return New((T4 value4) => New((T1 value1, T2 value2, T3 value3, T5 value5, T6 value6, T7 value7) => func(value1, value2, value3, value4, value5, value6, value7)));
        }

        public static Func<T5, Func<T1, T2, T3, T4, T6, T7, TResult>> Curry5th<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> func)
        {
            return New((T5 value5) => New((T1 value1, T2 value2, T3 value3, T4 value4, T6 value6, T7 value7) => func(value1, value2, value3, value4, value5, value6, value7)));
        }

        public static Func<T6, Func<T1, T2, T3, T4, T5, T7, TResult>> Curry6th<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> func)
        {
            return New((T6 value6) => New((T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T7 value7) => func(value1, value2, value3, value4, value5, value6, value7)));
        }

        public static Func<T7, Func<T1, T2, T3, T4, T5, T6, TResult>> Curry7th<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> func)
        {
            return New((T7 value7) => New((T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6) => func(value1, value2, value3, value4, value5, value6, value7)));
        }

        public static Func<TC1, Func<T1, Func<T2, T3, T4, T5, T6, TResult>>> Curry1st<TC1, T1, T2, T3, T4, T5, T6, TResult>(this Func<TC1, Func<T1, T2, T3, T4, T5, T6, TResult>> func)
        {
            return New((TC1 valueC1) => New((T1 value1) => New((T2 value2, T3 value3, T4 value4, T5 value5, T6 value6) => func(valueC1)(value1, value2, value3, value4, value5, value6))));
        }

        public static Func<TC1, Func<T2, Func<T1, T3, T4, T5, T6, TResult>>> Curry2nd<TC1, T1, T2, T3, T4, T5, T6, TResult>(this Func<TC1, Func<T1, T2, T3, T4, T5, T6, TResult>> func)
        {
            return New((TC1 valueC1) => New((T2 value2) => New((T1 value1, T3 value3, T4 value4, T5 value5, T6 value6) => func(valueC1)(value1, value2, value3, value4, value5, value6))));
        }

        public static Func<TC1, Func<T3, Func<T1, T2, T4, T5, T6, TResult>>> Curry3rd<TC1, T1, T2, T3, T4, T5, T6, TResult>(this Func<TC1, Func<T1, T2, T3, T4, T5, T6, TResult>> func)
        {
            return New((TC1 valueC1) => New((T3 value3) => New((T1 value1, T2 value2, T4 value4, T5 value5, T6 value6) => func(valueC1)(value1, value2, value3, value4, value5, value6))));
        }

        public static Func<TC1, Func<T4, Func<T1, T2, T3, T5, T6, TResult>>> Curry4th<TC1, T1, T2, T3, T4, T5, T6, TResult>(this Func<TC1, Func<T1, T2, T3, T4, T5, T6, TResult>> func)
        {
            return New((TC1 valueC1) => New((T4 value4) => New((T1 value1, T2 value2, T3 value3, T5 value5, T6 value6) => func(valueC1)(value1, value2, value3, value4, value5, value6))));
        }

        public static Func<TC1, Func<T5, Func<T1, T2, T3, T4, T6, TResult>>> Curry5th<TC1, T1, T2, T3, T4, T5, T6, TResult>(this Func<TC1, Func<T1, T2, T3, T4, T5, T6, TResult>> func)
        {
            return New((TC1 valueC1) => New((T5 value5) => New((T1 value1, T2 value2, T3 value3, T4 value4, T6 value6) => func(valueC1)(value1, value2, value3, value4, value5, value6))));
        }

        public static Func<TC1, Func<T6, Func<T1, T2, T3, T4, T5, TResult>>> Curry6th<TC1, T1, T2, T3, T4, T5, T6, TResult>(this Func<TC1, Func<T1, T2, T3, T4, T5, T6, TResult>> func)
        {
            return New((TC1 valueC1) => New((T6 value6) => New((T1 value1, T2 value2, T3 value3, T4 value4, T5 value5) => func(valueC1)(value1, value2, value3, value4, value5, value6))));
        }

        public static Func<TC1, Func<TC2, Func<T1, Func<T2, T3, T4, T5, TResult>>>> Curry1st<TC1, TC2, T1, T2, T3, T4, T5, TResult>(this Func<TC1, Func<TC2, Func<T1, T2, T3, T4, T5, TResult>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T1 value1) => New((T2 value2, T3 value3, T4 value4, T5 value5) => func(valueC1)(valueC2)(value1, value2, value3, value4, value5)))));
        }

        public static Func<TC1, Func<TC2, Func<T2, Func<T1, T3, T4, T5, TResult>>>> Curry2nd<TC1, TC2, T1, T2, T3, T4, T5, TResult>(this Func<TC1, Func<TC2, Func<T1, T2, T3, T4, T5, TResult>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T2 value2) => New((T1 value1, T3 value3, T4 value4, T5 value5) => func(valueC1)(valueC2)(value1, value2, value3, value4, value5)))));
        }

        public static Func<TC1, Func<TC2, Func<T3, Func<T1, T2, T4, T5, TResult>>>> Curry3rd<TC1, TC2, T1, T2, T3, T4, T5, TResult>(this Func<TC1, Func<TC2, Func<T1, T2, T3, T4, T5, TResult>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T3 value3) => New((T1 value1, T2 value2, T4 value4, T5 value5) => func(valueC1)(valueC2)(value1, value2, value3, value4, value5)))));
        }

        public static Func<TC1, Func<TC2, Func<T4, Func<T1, T2, T3, T5, TResult>>>> Curry4th<TC1, TC2, T1, T2, T3, T4, T5, TResult>(this Func<TC1, Func<TC2, Func<T1, T2, T3, T4, T5, TResult>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T4 value4) => New((T1 value1, T2 value2, T3 value3, T5 value5) => func(valueC1)(valueC2)(value1, value2, value3, value4, value5)))));
        }

        public static Func<TC1, Func<TC2, Func<T5, Func<T1, T2, T3, T4, TResult>>>> Curry5th<TC1, TC2, T1, T2, T3, T4, T5, TResult>(this Func<TC1, Func<TC2, Func<T1, T2, T3, T4, T5, TResult>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T5 value5) => New((T1 value1, T2 value2, T3 value3, T4 value4) => func(valueC1)(valueC2)(value1, value2, value3, value4, value5)))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T1, Func<T2, T3, T4, TResult>>>>> Curry1st<TC1, TC2, TC3, T1, T2, T3, T4, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<T1, T2, T3, T4, TResult>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T1 value1) => New((T2 value2, T3 value3, T4 value4) => func(valueC1)(valueC2)(valueC3)(value1, value2, value3, value4))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T2, Func<T1, T3, T4, TResult>>>>> Curry2nd<TC1, TC2, TC3, T1, T2, T3, T4, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<T1, T2, T3, T4, TResult>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T2 value2) => New((T1 value1, T3 value3, T4 value4) => func(valueC1)(valueC2)(valueC3)(value1, value2, value3, value4))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T3, Func<T1, T2, T4, TResult>>>>> Curry3rd<TC1, TC2, TC3, T1, T2, T3, T4, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<T1, T2, T3, T4, TResult>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T3 value3) => New((T1 value1, T2 value2, T4 value4) => func(valueC1)(valueC2)(valueC3)(value1, value2, value3, value4))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T4, Func<T1, T2, T3, TResult>>>>> Curry4th<TC1, TC2, TC3, T1, T2, T3, T4, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<T1, T2, T3, T4, TResult>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T4 value4) => New((T1 value1, T2 value2, T3 value3) => func(valueC1)(valueC2)(valueC3)(value1, value2, value3, value4))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T1, Func<T2, T3, TResult>>>>>> Curry1st<TC1, TC2, TC3, TC4, T1, T2, T3, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T1, T2, T3, TResult>>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((T1 value1) => New((T2 value2, T3 value3) => func(valueC1)(valueC2)(valueC3)(valueC4)(value1, value2, value3)))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T2, Func<T1, T3, TResult>>>>>> Curry2nd<TC1, TC2, TC3, TC4, T1, T2, T3, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T1, T2, T3, TResult>>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((T2 value2) => New((T1 value1, T3 value3) => func(valueC1)(valueC2)(valueC3)(valueC4)(value1, value2, value3)))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T3, Func<T1, T2, TResult>>>>>> Curry3rd<TC1, TC2, TC3, TC4, T1, T2, T3, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T1, T2, T3, TResult>>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((T3 value3) => New((T1 value1, T2 value2) => func(valueC1)(valueC2)(valueC3)(valueC4)(value1, value2, value3)))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<T1, Func<T2, TResult>>>>>>> Curry1st<TC1, TC2, TC3, TC4, TC5, T1, T2, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<T1, T2, TResult>>>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((TC5 valueC5) => New((T1 value1) => New((T2 value2) => func(valueC1)(valueC2)(valueC3)(valueC4)(valueC5)(value1, value2))))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<T2, Func<T1, TResult>>>>>>> Curry2nd<TC1, TC2, TC3, TC4, TC5, T1, T2, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<T1, T2, TResult>>>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((TC5 valueC5) => New((T2 value2) => New((T1 value1) => func(valueC1)(valueC2)(valueC3)(valueC4)(valueC5)(value1, value2))))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<TC6, Func<T1, Func<TResult>>>>>>>> Curry<TC1, TC2, TC3, TC4, TC5, TC6, T1, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<TC6, Func<T1, TResult>>>>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((TC5 valueC5) => New((TC6 valueC6) => New((T1 value1) => New(() => func(valueC1)(valueC2)(valueC3)(valueC4)(valueC5)(valueC6)(value1)))))))));
        }

        public static Func<T1, Func<T2, T3, T4, T5, T6, T7, T8, TResult>> Curry1st<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func)
        {
            return New((T1 value1) => New((T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8) => func(value1, value2, value3, value4, value5, value6, value7, value8)));
        }

        public static Func<T2, Func<T1, T3, T4, T5, T6, T7, T8, TResult>> Curry2nd<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func)
        {
            return New((T2 value2) => New((T1 value1, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8) => func(value1, value2, value3, value4, value5, value6, value7, value8)));
        }

        public static Func<T3, Func<T1, T2, T4, T5, T6, T7, T8, TResult>> Curry3rd<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func)
        {
            return New((T3 value3) => New((T1 value1, T2 value2, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8) => func(value1, value2, value3, value4, value5, value6, value7, value8)));
        }

        public static Func<T4, Func<T1, T2, T3, T5, T6, T7, T8, TResult>> Curry4th<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func)
        {
            return New((T4 value4) => New((T1 value1, T2 value2, T3 value3, T5 value5, T6 value6, T7 value7, T8 value8) => func(value1, value2, value3, value4, value5, value6, value7, value8)));
        }

        public static Func<T5, Func<T1, T2, T3, T4, T6, T7, T8, TResult>> Curry5th<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func)
        {
            return New((T5 value5) => New((T1 value1, T2 value2, T3 value3, T4 value4, T6 value6, T7 value7, T8 value8) => func(value1, value2, value3, value4, value5, value6, value7, value8)));
        }

        public static Func<T6, Func<T1, T2, T3, T4, T5, T7, T8, TResult>> Curry6th<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func)
        {
            return New((T6 value6) => New((T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T7 value7, T8 value8) => func(value1, value2, value3, value4, value5, value6, value7, value8)));
        }

        public static Func<T7, Func<T1, T2, T3, T4, T5, T6, T8, TResult>> Curry7th<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func)
        {
            return New((T7 value7) => New((T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T8 value8) => func(value1, value2, value3, value4, value5, value6, value7, value8)));
        }

        public static Func<T8, Func<T1, T2, T3, T4, T5, T6, T7, TResult>> Curry8th<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func)
        {
            return New((T8 value8) => New((T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7) => func(value1, value2, value3, value4, value5, value6, value7, value8)));
        }

        public static Func<TC1, Func<T1, Func<T2, T3, T4, T5, T6, T7, TResult>>> Curry1st<TC1, T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<TC1, Func<T1, T2, T3, T4, T5, T6, T7, TResult>> func)
        {
            return New((TC1 valueC1) => New((T1 value1) => New((T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7) => func(valueC1)(value1, value2, value3, value4, value5, value6, value7))));
        }

        public static Func<TC1, Func<T2, Func<T1, T3, T4, T5, T6, T7, TResult>>> Curry2nd<TC1, T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<TC1, Func<T1, T2, T3, T4, T5, T6, T7, TResult>> func)
        {
            return New((TC1 valueC1) => New((T2 value2) => New((T1 value1, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7) => func(valueC1)(value1, value2, value3, value4, value5, value6, value7))));
        }

        public static Func<TC1, Func<T3, Func<T1, T2, T4, T5, T6, T7, TResult>>> Curry3rd<TC1, T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<TC1, Func<T1, T2, T3, T4, T5, T6, T7, TResult>> func)
        {
            return New((TC1 valueC1) => New((T3 value3) => New((T1 value1, T2 value2, T4 value4, T5 value5, T6 value6, T7 value7) => func(valueC1)(value1, value2, value3, value4, value5, value6, value7))));
        }

        public static Func<TC1, Func<T4, Func<T1, T2, T3, T5, T6, T7, TResult>>> Curry4th<TC1, T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<TC1, Func<T1, T2, T3, T4, T5, T6, T7, TResult>> func)
        {
            return New((TC1 valueC1) => New((T4 value4) => New((T1 value1, T2 value2, T3 value3, T5 value5, T6 value6, T7 value7) => func(valueC1)(value1, value2, value3, value4, value5, value6, value7))));
        }

        public static Func<TC1, Func<T5, Func<T1, T2, T3, T4, T6, T7, TResult>>> Curry5th<TC1, T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<TC1, Func<T1, T2, T3, T4, T5, T6, T7, TResult>> func)
        {
            return New((TC1 valueC1) => New((T5 value5) => New((T1 value1, T2 value2, T3 value3, T4 value4, T6 value6, T7 value7) => func(valueC1)(value1, value2, value3, value4, value5, value6, value7))));
        }

        public static Func<TC1, Func<T6, Func<T1, T2, T3, T4, T5, T7, TResult>>> Curry6th<TC1, T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<TC1, Func<T1, T2, T3, T4, T5, T6, T7, TResult>> func)
        {
            return New((TC1 valueC1) => New((T6 value6) => New((T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T7 value7) => func(valueC1)(value1, value2, value3, value4, value5, value6, value7))));
        }

        public static Func<TC1, Func<T7, Func<T1, T2, T3, T4, T5, T6, TResult>>> Curry7th<TC1, T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<TC1, Func<T1, T2, T3, T4, T5, T6, T7, TResult>> func)
        {
            return New((TC1 valueC1) => New((T7 value7) => New((T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6) => func(valueC1)(value1, value2, value3, value4, value5, value6, value7))));
        }

        public static Func<TC1, Func<TC2, Func<T1, Func<T2, T3, T4, T5, T6, TResult>>>> Curry1st<TC1, TC2, T1, T2, T3, T4, T5, T6, TResult>(this Func<TC1, Func<TC2, Func<T1, T2, T3, T4, T5, T6, TResult>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T1 value1) => New((T2 value2, T3 value3, T4 value4, T5 value5, T6 value6) => func(valueC1)(valueC2)(value1, value2, value3, value4, value5, value6)))));
        }

        public static Func<TC1, Func<TC2, Func<T2, Func<T1, T3, T4, T5, T6, TResult>>>> Curry2nd<TC1, TC2, T1, T2, T3, T4, T5, T6, TResult>(this Func<TC1, Func<TC2, Func<T1, T2, T3, T4, T5, T6, TResult>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T2 value2) => New((T1 value1, T3 value3, T4 value4, T5 value5, T6 value6) => func(valueC1)(valueC2)(value1, value2, value3, value4, value5, value6)))));
        }

        public static Func<TC1, Func<TC2, Func<T3, Func<T1, T2, T4, T5, T6, TResult>>>> Curry3rd<TC1, TC2, T1, T2, T3, T4, T5, T6, TResult>(this Func<TC1, Func<TC2, Func<T1, T2, T3, T4, T5, T6, TResult>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T3 value3) => New((T1 value1, T2 value2, T4 value4, T5 value5, T6 value6) => func(valueC1)(valueC2)(value1, value2, value3, value4, value5, value6)))));
        }

        public static Func<TC1, Func<TC2, Func<T4, Func<T1, T2, T3, T5, T6, TResult>>>> Curry4th<TC1, TC2, T1, T2, T3, T4, T5, T6, TResult>(this Func<TC1, Func<TC2, Func<T1, T2, T3, T4, T5, T6, TResult>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T4 value4) => New((T1 value1, T2 value2, T3 value3, T5 value5, T6 value6) => func(valueC1)(valueC2)(value1, value2, value3, value4, value5, value6)))));
        }

        public static Func<TC1, Func<TC2, Func<T5, Func<T1, T2, T3, T4, T6, TResult>>>> Curry5th<TC1, TC2, T1, T2, T3, T4, T5, T6, TResult>(this Func<TC1, Func<TC2, Func<T1, T2, T3, T4, T5, T6, TResult>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T5 value5) => New((T1 value1, T2 value2, T3 value3, T4 value4, T6 value6) => func(valueC1)(valueC2)(value1, value2, value3, value4, value5, value6)))));
        }

        public static Func<TC1, Func<TC2, Func<T6, Func<T1, T2, T3, T4, T5, TResult>>>> Curry6th<TC1, TC2, T1, T2, T3, T4, T5, T6, TResult>(this Func<TC1, Func<TC2, Func<T1, T2, T3, T4, T5, T6, TResult>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((T6 value6) => New((T1 value1, T2 value2, T3 value3, T4 value4, T5 value5) => func(valueC1)(valueC2)(value1, value2, value3, value4, value5, value6)))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T1, Func<T2, T3, T4, T5, TResult>>>>> Curry1st<TC1, TC2, TC3, T1, T2, T3, T4, T5, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<T1, T2, T3, T4, T5, TResult>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T1 value1) => New((T2 value2, T3 value3, T4 value4, T5 value5) => func(valueC1)(valueC2)(valueC3)(value1, value2, value3, value4, value5))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T2, Func<T1, T3, T4, T5, TResult>>>>> Curry2nd<TC1, TC2, TC3, T1, T2, T3, T4, T5, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<T1, T2, T3, T4, T5, TResult>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T2 value2) => New((T1 value1, T3 value3, T4 value4, T5 value5) => func(valueC1)(valueC2)(valueC3)(value1, value2, value3, value4, value5))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T3, Func<T1, T2, T4, T5, TResult>>>>> Curry3rd<TC1, TC2, TC3, T1, T2, T3, T4, T5, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<T1, T2, T3, T4, T5, TResult>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T3 value3) => New((T1 value1, T2 value2, T4 value4, T5 value5) => func(valueC1)(valueC2)(valueC3)(value1, value2, value3, value4, value5))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T4, Func<T1, T2, T3, T5, TResult>>>>> Curry4th<TC1, TC2, TC3, T1, T2, T3, T4, T5, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<T1, T2, T3, T4, T5, TResult>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T4 value4) => New((T1 value1, T2 value2, T3 value3, T5 value5) => func(valueC1)(valueC2)(valueC3)(value1, value2, value3, value4, value5))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<T5, Func<T1, T2, T3, T4, TResult>>>>> Curry5th<TC1, TC2, TC3, T1, T2, T3, T4, T5, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<T1, T2, T3, T4, T5, TResult>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((T5 value5) => New((T1 value1, T2 value2, T3 value3, T4 value4) => func(valueC1)(valueC2)(valueC3)(value1, value2, value3, value4, value5))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T1, Func<T2, T3, T4, TResult>>>>>> Curry1st<TC1, TC2, TC3, TC4, T1, T2, T3, T4, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T1, T2, T3, T4, TResult>>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((T1 value1) => New((T2 value2, T3 value3, T4 value4) => func(valueC1)(valueC2)(valueC3)(valueC4)(value1, value2, value3, value4)))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T2, Func<T1, T3, T4, TResult>>>>>> Curry2nd<TC1, TC2, TC3, TC4, T1, T2, T3, T4, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T1, T2, T3, T4, TResult>>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((T2 value2) => New((T1 value1, T3 value3, T4 value4) => func(valueC1)(valueC2)(valueC3)(valueC4)(value1, value2, value3, value4)))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T3, Func<T1, T2, T4, TResult>>>>>> Curry3rd<TC1, TC2, TC3, TC4, T1, T2, T3, T4, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T1, T2, T3, T4, TResult>>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((T3 value3) => New((T1 value1, T2 value2, T4 value4) => func(valueC1)(valueC2)(valueC3)(valueC4)(value1, value2, value3, value4)))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T4, Func<T1, T2, T3, TResult>>>>>> Curry4th<TC1, TC2, TC3, TC4, T1, T2, T3, T4, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<T1, T2, T3, T4, TResult>>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((T4 value4) => New((T1 value1, T2 value2, T3 value3) => func(valueC1)(valueC2)(valueC3)(valueC4)(value1, value2, value3, value4)))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<T1, Func<T2, T3, TResult>>>>>>> Curry1st<TC1, TC2, TC3, TC4, TC5, T1, T2, T3, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<T1, T2, T3, TResult>>>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((TC5 valueC5) => New((T1 value1) => New((T2 value2, T3 value3) => func(valueC1)(valueC2)(valueC3)(valueC4)(valueC5)(value1, value2, value3))))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<T2, Func<T1, T3, TResult>>>>>>> Curry2nd<TC1, TC2, TC3, TC4, TC5, T1, T2, T3, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<T1, T2, T3, TResult>>>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((TC5 valueC5) => New((T2 value2) => New((T1 value1, T3 value3) => func(valueC1)(valueC2)(valueC3)(valueC4)(valueC5)(value1, value2, value3))))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<T3, Func<T1, T2, TResult>>>>>>> Curry3rd<TC1, TC2, TC3, TC4, TC5, T1, T2, T3, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<T1, T2, T3, TResult>>>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((TC5 valueC5) => New((T3 value3) => New((T1 value1, T2 value2) => func(valueC1)(valueC2)(valueC3)(valueC4)(valueC5)(value1, value2, value3))))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<TC6, Func<T1, Func<T2, TResult>>>>>>>> Curry1st<TC1, TC2, TC3, TC4, TC5, TC6, T1, T2, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<TC6, Func<T1, T2, TResult>>>>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((TC5 valueC5) => New((TC6 valueC6) => New((T1 value1) => New((T2 value2) => func(valueC1)(valueC2)(valueC3)(valueC4)(valueC5)(valueC6)(value1, value2)))))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<TC6, Func<T2, Func<T1, TResult>>>>>>>> Curry2nd<TC1, TC2, TC3, TC4, TC5, TC6, T1, T2, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<TC6, Func<T1, T2, TResult>>>>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((TC5 valueC5) => New((TC6 valueC6) => New((T2 value2) => New((T1 value1) => func(valueC1)(valueC2)(valueC3)(valueC4)(valueC5)(valueC6)(value1, value2)))))))));
        }

        public static Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<TC6, Func<TC7, Func<T1, Func<TResult>>>>>>>>> Curry<TC1, TC2, TC3, TC4, TC5, TC6, TC7, T1, TResult>(this Func<TC1, Func<TC2, Func<TC3, Func<TC4, Func<TC5, Func<TC6, Func<TC7, Func<T1, TResult>>>>>>>> func)
        {
            return New((TC1 valueC1) => New((TC2 valueC2) => New((TC3 valueC3) => New((TC4 valueC4) => New((TC5 valueC5) => New((TC6 valueC6) => New((TC7 valueC7) => New((T1 value1) => New(() => func(valueC1)(valueC2)(valueC3)(valueC4)(valueC5)(valueC6)(valueC7)(value1))))))))));
        }
    }
}
