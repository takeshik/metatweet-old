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
    partial class Lambda
    {
        public static Action Unbind(this Func<Action> func)
        {
            return New(() => func()());
        }

        public static Action<TB1> Unbind<TB1>(this Func<Action<TB1>> func)
        {
            return New((TB1 valueB1) => func()(valueB1));
        }

        public static Action<TB1, TB2> Unbind<TB1, TB2>(this Func<Action<TB1, TB2>> func)
        {
            return New((TB1 valueB1, TB2 valueB2) => func()(valueB1, valueB2));
        }

        public static Action<TB1, TB2, TB3> Unbind<TB1, TB2, TB3>(this Func<Action<TB1, TB2, TB3>> func)
        {
            return New((TB1 valueB1, TB2 valueB2, TB3 valueB3) => func()(valueB1, valueB2, valueB3));
        }

        public static Action<TB1, TB2, TB3, TB4> Unbind<TB1, TB2, TB3, TB4>(this Func<Action<TB1, TB2, TB3, TB4>> func)
        {
            return New((TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4) => func()(valueB1, valueB2, valueB3, valueB4));
        }

        public static Action<TB1, TB2, TB3, TB4, TB5> Unbind<TB1, TB2, TB3, TB4, TB5>(this Func<Action<TB1, TB2, TB3, TB4, TB5>> func)
        {
            return New((TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5) => func()(valueB1, valueB2, valueB3, valueB4, valueB5));
        }

        public static Action<TB1, TB2, TB3, TB4, TB5, TB6> Unbind<TB1, TB2, TB3, TB4, TB5, TB6>(this Func<Action<TB1, TB2, TB3, TB4, TB5, TB6>> func)
        {
            return New((TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6) => func()(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6));
        }

        public static Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7> Unbind<TB1, TB2, TB3, TB4, TB5, TB6, TB7>(this Func<Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7>> func)
        {
            return New((TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7) => func()(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7));
        }

        public static Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8> Unbind<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8>(this Func<Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8>> func)
        {
            return New((TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8) => func()(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8));
        }

        public static Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9> Unbind<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9>(this Func<Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9>> func)
        {
            return New((TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9) => func()(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9));
        }

        public static Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10> Unbind<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10>(this Func<Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10>> func)
        {
            return New((TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10) => func()(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10));
        }

        public static Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11> Unbind<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11>(this Func<Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11>> func)
        {
            return New((TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11) => func()(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11));
        }

        public static Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12> Unbind<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12>(this Func<Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12>> func)
        {
            return New((TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12) => func()(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12));
        }

        public static Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13> Unbind<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13>(this Func<Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13>> func)
        {
            return New((TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12, TB13 valueB13) => func()(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12, valueB13));
        }

        public static Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14> Unbind<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14>(this Func<Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14>> func)
        {
            return New((TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12, TB13 valueB13, TB14 valueB14) => func()(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12, valueB13, valueB14));
        }

        public static Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14, TB15> Unbind<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14, TB15>(this Func<Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14, TB15>> func)
        {
            return New((TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12, TB13 valueB13, TB14 valueB14, TB15 valueB15) => func()(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12, valueB13, valueB14, valueB15));
        }

        public static Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14, TB15, TB16> Unbind<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14, TB15, TB16>(this Func<Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14, TB15, TB16>> func)
        {
            return New((TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12, TB13 valueB13, TB14 valueB14, TB15 valueB15, TB16 valueB16) => func()(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12, valueB13, valueB14, valueB15, valueB16));
        }

        public static Action<TA1> Unbind<TA1>(this Func<TA1, Action> func)
        {
            return New((TA1 valueA1) => func(valueA1)());
        }

        public static Action<TA1, TB1> Unbind<TA1, TB1>(this Func<TA1, Action<TB1>> func)
        {
            return New((TA1 valueA1, TB1 valueB1) => func(valueA1)(valueB1));
        }

        public static Action<TA1, TB1, TB2> Unbind<TA1, TB1, TB2>(this Func<TA1, Action<TB1, TB2>> func)
        {
            return New((TA1 valueA1, TB1 valueB1, TB2 valueB2) => func(valueA1)(valueB1, valueB2));
        }

        public static Action<TA1, TB1, TB2, TB3> Unbind<TA1, TB1, TB2, TB3>(this Func<TA1, Action<TB1, TB2, TB3>> func)
        {
            return New((TA1 valueA1, TB1 valueB1, TB2 valueB2, TB3 valueB3) => func(valueA1)(valueB1, valueB2, valueB3));
        }

        public static Action<TA1, TB1, TB2, TB3, TB4> Unbind<TA1, TB1, TB2, TB3, TB4>(this Func<TA1, Action<TB1, TB2, TB3, TB4>> func)
        {
            return New((TA1 valueA1, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4) => func(valueA1)(valueB1, valueB2, valueB3, valueB4));
        }

        public static Action<TA1, TB1, TB2, TB3, TB4, TB5> Unbind<TA1, TB1, TB2, TB3, TB4, TB5>(this Func<TA1, Action<TB1, TB2, TB3, TB4, TB5>> func)
        {
            return New((TA1 valueA1, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5) => func(valueA1)(valueB1, valueB2, valueB3, valueB4, valueB5));
        }

        public static Action<TA1, TB1, TB2, TB3, TB4, TB5, TB6> Unbind<TA1, TB1, TB2, TB3, TB4, TB5, TB6>(this Func<TA1, Action<TB1, TB2, TB3, TB4, TB5, TB6>> func)
        {
            return New((TA1 valueA1, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6) => func(valueA1)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6));
        }

        public static Action<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7> Unbind<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7>(this Func<TA1, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7>> func)
        {
            return New((TA1 valueA1, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7) => func(valueA1)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7));
        }

        public static Action<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8> Unbind<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8>(this Func<TA1, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8>> func)
        {
            return New((TA1 valueA1, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8) => func(valueA1)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8));
        }

        public static Action<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9> Unbind<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9>(this Func<TA1, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9>> func)
        {
            return New((TA1 valueA1, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9) => func(valueA1)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9));
        }

        public static Action<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10> Unbind<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10>(this Func<TA1, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10>> func)
        {
            return New((TA1 valueA1, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10) => func(valueA1)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10));
        }

        public static Action<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11> Unbind<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11>(this Func<TA1, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11>> func)
        {
            return New((TA1 valueA1, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11) => func(valueA1)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11));
        }

        public static Action<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12> Unbind<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12>(this Func<TA1, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12>> func)
        {
            return New((TA1 valueA1, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12) => func(valueA1)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12));
        }

        public static Action<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13> Unbind<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13>(this Func<TA1, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13>> func)
        {
            return New((TA1 valueA1, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12, TB13 valueB13) => func(valueA1)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12, valueB13));
        }

        public static Action<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14> Unbind<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14>(this Func<TA1, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14>> func)
        {
            return New((TA1 valueA1, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12, TB13 valueB13, TB14 valueB14) => func(valueA1)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12, valueB13, valueB14));
        }

        public static Action<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14, TB15> Unbind<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14, TB15>(this Func<TA1, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14, TB15>> func)
        {
            return New((TA1 valueA1, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12, TB13 valueB13, TB14 valueB14, TB15 valueB15) => func(valueA1)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12, valueB13, valueB14, valueB15));
        }

        public static Action<TA1, TA2> Unbind<TA1, TA2>(this Func<TA1, TA2, Action> func)
        {
            return New((TA1 valueA1, TA2 valueA2) => func(valueA1, valueA2)());
        }

        public static Action<TA1, TA2, TB1> Unbind<TA1, TA2, TB1>(this Func<TA1, TA2, Action<TB1>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TB1 valueB1) => func(valueA1, valueA2)(valueB1));
        }

        public static Action<TA1, TA2, TB1, TB2> Unbind<TA1, TA2, TB1, TB2>(this Func<TA1, TA2, Action<TB1, TB2>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TB1 valueB1, TB2 valueB2) => func(valueA1, valueA2)(valueB1, valueB2));
        }

        public static Action<TA1, TA2, TB1, TB2, TB3> Unbind<TA1, TA2, TB1, TB2, TB3>(this Func<TA1, TA2, Action<TB1, TB2, TB3>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TB1 valueB1, TB2 valueB2, TB3 valueB3) => func(valueA1, valueA2)(valueB1, valueB2, valueB3));
        }

        public static Action<TA1, TA2, TB1, TB2, TB3, TB4> Unbind<TA1, TA2, TB1, TB2, TB3, TB4>(this Func<TA1, TA2, Action<TB1, TB2, TB3, TB4>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4) => func(valueA1, valueA2)(valueB1, valueB2, valueB3, valueB4));
        }

        public static Action<TA1, TA2, TB1, TB2, TB3, TB4, TB5> Unbind<TA1, TA2, TB1, TB2, TB3, TB4, TB5>(this Func<TA1, TA2, Action<TB1, TB2, TB3, TB4, TB5>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5) => func(valueA1, valueA2)(valueB1, valueB2, valueB3, valueB4, valueB5));
        }

        public static Action<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6> Unbind<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6>(this Func<TA1, TA2, Action<TB1, TB2, TB3, TB4, TB5, TB6>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6) => func(valueA1, valueA2)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6));
        }

        public static Action<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7> Unbind<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7>(this Func<TA1, TA2, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7) => func(valueA1, valueA2)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7));
        }

        public static Action<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8> Unbind<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8>(this Func<TA1, TA2, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8) => func(valueA1, valueA2)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8));
        }

        public static Action<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9> Unbind<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9>(this Func<TA1, TA2, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9) => func(valueA1, valueA2)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9));
        }

        public static Action<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10> Unbind<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10>(this Func<TA1, TA2, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10) => func(valueA1, valueA2)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10));
        }

        public static Action<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11> Unbind<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11>(this Func<TA1, TA2, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11) => func(valueA1, valueA2)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11));
        }

        public static Action<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12> Unbind<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12>(this Func<TA1, TA2, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12) => func(valueA1, valueA2)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12));
        }

        public static Action<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13> Unbind<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13>(this Func<TA1, TA2, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12, TB13 valueB13) => func(valueA1, valueA2)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12, valueB13));
        }

        public static Action<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14> Unbind<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14>(this Func<TA1, TA2, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12, TB13 valueB13, TB14 valueB14) => func(valueA1, valueA2)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12, valueB13, valueB14));
        }

        public static Action<TA1, TA2, TA3> Unbind<TA1, TA2, TA3>(this Func<TA1, TA2, TA3, Action> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3) => func(valueA1, valueA2, valueA3)());
        }

        public static Action<TA1, TA2, TA3, TB1> Unbind<TA1, TA2, TA3, TB1>(this Func<TA1, TA2, TA3, Action<TB1>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TB1 valueB1) => func(valueA1, valueA2, valueA3)(valueB1));
        }

        public static Action<TA1, TA2, TA3, TB1, TB2> Unbind<TA1, TA2, TA3, TB1, TB2>(this Func<TA1, TA2, TA3, Action<TB1, TB2>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TB1 valueB1, TB2 valueB2) => func(valueA1, valueA2, valueA3)(valueB1, valueB2));
        }

        public static Action<TA1, TA2, TA3, TB1, TB2, TB3> Unbind<TA1, TA2, TA3, TB1, TB2, TB3>(this Func<TA1, TA2, TA3, Action<TB1, TB2, TB3>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TB1 valueB1, TB2 valueB2, TB3 valueB3) => func(valueA1, valueA2, valueA3)(valueB1, valueB2, valueB3));
        }

        public static Action<TA1, TA2, TA3, TB1, TB2, TB3, TB4> Unbind<TA1, TA2, TA3, TB1, TB2, TB3, TB4>(this Func<TA1, TA2, TA3, Action<TB1, TB2, TB3, TB4>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4) => func(valueA1, valueA2, valueA3)(valueB1, valueB2, valueB3, valueB4));
        }

        public static Action<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5> Unbind<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5>(this Func<TA1, TA2, TA3, Action<TB1, TB2, TB3, TB4, TB5>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5) => func(valueA1, valueA2, valueA3)(valueB1, valueB2, valueB3, valueB4, valueB5));
        }

        public static Action<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6> Unbind<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6>(this Func<TA1, TA2, TA3, Action<TB1, TB2, TB3, TB4, TB5, TB6>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6) => func(valueA1, valueA2, valueA3)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6));
        }

        public static Action<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TB7> Unbind<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TB7>(this Func<TA1, TA2, TA3, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7) => func(valueA1, valueA2, valueA3)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7));
        }

        public static Action<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8> Unbind<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8>(this Func<TA1, TA2, TA3, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8) => func(valueA1, valueA2, valueA3)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8));
        }

        public static Action<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9> Unbind<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9>(this Func<TA1, TA2, TA3, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9) => func(valueA1, valueA2, valueA3)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9));
        }

        public static Action<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10> Unbind<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10>(this Func<TA1, TA2, TA3, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10) => func(valueA1, valueA2, valueA3)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10));
        }

        public static Action<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11> Unbind<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11>(this Func<TA1, TA2, TA3, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11) => func(valueA1, valueA2, valueA3)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11));
        }

        public static Action<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12> Unbind<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12>(this Func<TA1, TA2, TA3, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12) => func(valueA1, valueA2, valueA3)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12));
        }

        public static Action<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13> Unbind<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13>(this Func<TA1, TA2, TA3, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12, TB13 valueB13) => func(valueA1, valueA2, valueA3)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12, valueB13));
        }

        public static Action<TA1, TA2, TA3, TA4> Unbind<TA1, TA2, TA3, TA4>(this Func<TA1, TA2, TA3, TA4, Action> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4) => func(valueA1, valueA2, valueA3, valueA4)());
        }

        public static Action<TA1, TA2, TA3, TA4, TB1> Unbind<TA1, TA2, TA3, TA4, TB1>(this Func<TA1, TA2, TA3, TA4, Action<TB1>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TB1 valueB1) => func(valueA1, valueA2, valueA3, valueA4)(valueB1));
        }

        public static Action<TA1, TA2, TA3, TA4, TB1, TB2> Unbind<TA1, TA2, TA3, TA4, TB1, TB2>(this Func<TA1, TA2, TA3, TA4, Action<TB1, TB2>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TB1 valueB1, TB2 valueB2) => func(valueA1, valueA2, valueA3, valueA4)(valueB1, valueB2));
        }

        public static Action<TA1, TA2, TA3, TA4, TB1, TB2, TB3> Unbind<TA1, TA2, TA3, TA4, TB1, TB2, TB3>(this Func<TA1, TA2, TA3, TA4, Action<TB1, TB2, TB3>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TB1 valueB1, TB2 valueB2, TB3 valueB3) => func(valueA1, valueA2, valueA3, valueA4)(valueB1, valueB2, valueB3));
        }

        public static Action<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4> Unbind<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4>(this Func<TA1, TA2, TA3, TA4, Action<TB1, TB2, TB3, TB4>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4) => func(valueA1, valueA2, valueA3, valueA4)(valueB1, valueB2, valueB3, valueB4));
        }

        public static Action<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5> Unbind<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5>(this Func<TA1, TA2, TA3, TA4, Action<TB1, TB2, TB3, TB4, TB5>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5) => func(valueA1, valueA2, valueA3, valueA4)(valueB1, valueB2, valueB3, valueB4, valueB5));
        }

        public static Action<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TB6> Unbind<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TB6>(this Func<TA1, TA2, TA3, TA4, Action<TB1, TB2, TB3, TB4, TB5, TB6>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6) => func(valueA1, valueA2, valueA3, valueA4)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6));
        }

        public static Action<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TB6, TB7> Unbind<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TB6, TB7>(this Func<TA1, TA2, TA3, TA4, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7) => func(valueA1, valueA2, valueA3, valueA4)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7));
        }

        public static Action<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8> Unbind<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8>(this Func<TA1, TA2, TA3, TA4, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8) => func(valueA1, valueA2, valueA3, valueA4)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8));
        }

        public static Action<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9> Unbind<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9>(this Func<TA1, TA2, TA3, TA4, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9) => func(valueA1, valueA2, valueA3, valueA4)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9));
        }

        public static Action<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10> Unbind<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10>(this Func<TA1, TA2, TA3, TA4, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10) => func(valueA1, valueA2, valueA3, valueA4)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10));
        }

        public static Action<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11> Unbind<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11>(this Func<TA1, TA2, TA3, TA4, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11) => func(valueA1, valueA2, valueA3, valueA4)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11));
        }

        public static Action<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12> Unbind<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12>(this Func<TA1, TA2, TA3, TA4, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12) => func(valueA1, valueA2, valueA3, valueA4)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5> Unbind<TA1, TA2, TA3, TA4, TA5>(this Func<TA1, TA2, TA3, TA4, TA5, Action> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5) => func(valueA1, valueA2, valueA3, valueA4, valueA5)());
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TB1> Unbind<TA1, TA2, TA3, TA4, TA5, TB1>(this Func<TA1, TA2, TA3, TA4, TA5, Action<TB1>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TB1 valueB1) => func(valueA1, valueA2, valueA3, valueA4, valueA5)(valueB1));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TB1, TB2> Unbind<TA1, TA2, TA3, TA4, TA5, TB1, TB2>(this Func<TA1, TA2, TA3, TA4, TA5, Action<TB1, TB2>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TB1 valueB1, TB2 valueB2) => func(valueA1, valueA2, valueA3, valueA4, valueA5)(valueB1, valueB2));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3> Unbind<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3>(this Func<TA1, TA2, TA3, TA4, TA5, Action<TB1, TB2, TB3>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TB1 valueB1, TB2 valueB2, TB3 valueB3) => func(valueA1, valueA2, valueA3, valueA4, valueA5)(valueB1, valueB2, valueB3));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4> Unbind<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4>(this Func<TA1, TA2, TA3, TA4, TA5, Action<TB1, TB2, TB3, TB4>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4) => func(valueA1, valueA2, valueA3, valueA4, valueA5)(valueB1, valueB2, valueB3, valueB4));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TB5> Unbind<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TB5>(this Func<TA1, TA2, TA3, TA4, TA5, Action<TB1, TB2, TB3, TB4, TB5>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5) => func(valueA1, valueA2, valueA3, valueA4, valueA5)(valueB1, valueB2, valueB3, valueB4, valueB5));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TB5, TB6> Unbind<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TB5, TB6>(this Func<TA1, TA2, TA3, TA4, TA5, Action<TB1, TB2, TB3, TB4, TB5, TB6>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6) => func(valueA1, valueA2, valueA3, valueA4, valueA5)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TB5, TB6, TB7> Unbind<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TB5, TB6, TB7>(this Func<TA1, TA2, TA3, TA4, TA5, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7) => func(valueA1, valueA2, valueA3, valueA4, valueA5)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8> Unbind<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8>(this Func<TA1, TA2, TA3, TA4, TA5, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8) => func(valueA1, valueA2, valueA3, valueA4, valueA5)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9> Unbind<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9>(this Func<TA1, TA2, TA3, TA4, TA5, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9) => func(valueA1, valueA2, valueA3, valueA4, valueA5)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10> Unbind<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10>(this Func<TA1, TA2, TA3, TA4, TA5, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10) => func(valueA1, valueA2, valueA3, valueA4, valueA5)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11> Unbind<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11>(this Func<TA1, TA2, TA3, TA4, TA5, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11) => func(valueA1, valueA2, valueA3, valueA4, valueA5)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6> Unbind<TA1, TA2, TA3, TA4, TA5, TA6>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, Action> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6)());
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TB1> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TB1>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, Action<TB1>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TB1 valueB1) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6)(valueB1));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, Action<TB1, TB2>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TB1 valueB1, TB2 valueB2) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6)(valueB1, valueB2));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, Action<TB1, TB2, TB3>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TB1 valueB1, TB2 valueB2, TB3 valueB3) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6)(valueB1, valueB2, valueB3));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TB4> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TB4>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, Action<TB1, TB2, TB3, TB4>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6)(valueB1, valueB2, valueB3, valueB4));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TB4, TB5> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TB4, TB5>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, Action<TB1, TB2, TB3, TB4, TB5>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6)(valueB1, valueB2, valueB3, valueB4, valueB5));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TB4, TB5, TB6> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TB4, TB5, TB6>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, Action<TB1, TB2, TB3, TB4, TB5, TB6>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TB4, TB5, TB6, TB7> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TB4, TB5, TB6, TB7>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, Action> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7)());
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, Action<TB1>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TB1 valueB1) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7)(valueB1));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, Action<TB1, TB2>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TB1 valueB1, TB2 valueB2) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7)(valueB1, valueB2));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TB3> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TB3>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, Action<TB1, TB2, TB3>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TB1 valueB1, TB2 valueB2, TB3 valueB3) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7)(valueB1, valueB2, valueB3));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TB3, TB4> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TB3, TB4>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, Action<TB1, TB2, TB3, TB4>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7)(valueB1, valueB2, valueB3, valueB4));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TB3, TB4, TB5> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TB3, TB4, TB5>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, Action<TB1, TB2, TB3, TB4, TB5>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7)(valueB1, valueB2, valueB3, valueB4, valueB5));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TB3, TB4, TB5, TB6> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TB3, TB4, TB5, TB6>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, Action<TB1, TB2, TB3, TB4, TB5, TB6>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TB3, TB4, TB5, TB6, TB7> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TB3, TB4, TB5, TB6, TB7>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, Action> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8)());
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, Action<TB1>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TB1 valueB1) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8)(valueB1));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TB2> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TB2>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, Action<TB1, TB2>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TB1 valueB1, TB2 valueB2) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8)(valueB1, valueB2));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TB2, TB3> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TB2, TB3>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, Action<TB1, TB2, TB3>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TB1 valueB1, TB2 valueB2, TB3 valueB3) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8)(valueB1, valueB2, valueB3));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TB2, TB3, TB4> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TB2, TB3, TB4>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, Action<TB1, TB2, TB3, TB4>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8)(valueB1, valueB2, valueB3, valueB4));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TB2, TB3, TB4, TB5> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TB2, TB3, TB4, TB5>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, Action<TB1, TB2, TB3, TB4, TB5>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8)(valueB1, valueB2, valueB3, valueB4, valueB5));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TB2, TB3, TB4, TB5, TB6> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TB2, TB3, TB4, TB5, TB6>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, Action<TB1, TB2, TB3, TB4, TB5, TB6>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TB2, TB3, TB4, TB5, TB6, TB7> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TB2, TB3, TB4, TB5, TB6, TB7>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, Action> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9)());
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TB1> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TB1>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, Action<TB1>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TB1 valueB1) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9)(valueB1));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TB1, TB2> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TB1, TB2>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, Action<TB1, TB2>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TB1 valueB1, TB2 valueB2) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9)(valueB1, valueB2));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TB1, TB2, TB3> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TB1, TB2, TB3>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, Action<TB1, TB2, TB3>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TB1 valueB1, TB2 valueB2, TB3 valueB3) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9)(valueB1, valueB2, valueB3));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TB1, TB2, TB3, TB4> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TB1, TB2, TB3, TB4>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, Action<TB1, TB2, TB3, TB4>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9)(valueB1, valueB2, valueB3, valueB4));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TB1, TB2, TB3, TB4, TB5> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TB1, TB2, TB3, TB4, TB5>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, Action<TB1, TB2, TB3, TB4, TB5>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9)(valueB1, valueB2, valueB3, valueB4, valueB5));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TB1, TB2, TB3, TB4, TB5, TB6> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TB1, TB2, TB3, TB4, TB5, TB6>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, Action<TB1, TB2, TB3, TB4, TB5, TB6>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TB1, TB2, TB3, TB4, TB5, TB6, TB7> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TB1, TB2, TB3, TB4, TB5, TB6, TB7>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, Action<TB1, TB2, TB3, TB4, TB5, TB6, TB7>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, Action> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10)());
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TB1> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TB1>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, Action<TB1>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TB1 valueB1) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10)(valueB1));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TB1, TB2> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TB1, TB2>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, Action<TB1, TB2>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TB1 valueB1, TB2 valueB2) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10)(valueB1, valueB2));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TB1, TB2, TB3> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TB1, TB2, TB3>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, Action<TB1, TB2, TB3>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TB1 valueB1, TB2 valueB2, TB3 valueB3) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10)(valueB1, valueB2, valueB3));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TB1, TB2, TB3, TB4> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TB1, TB2, TB3, TB4>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, Action<TB1, TB2, TB3, TB4>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10)(valueB1, valueB2, valueB3, valueB4));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TB1, TB2, TB3, TB4, TB5> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TB1, TB2, TB3, TB4, TB5>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, Action<TB1, TB2, TB3, TB4, TB5>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10)(valueB1, valueB2, valueB3, valueB4, valueB5));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TB1, TB2, TB3, TB4, TB5, TB6> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TB1, TB2, TB3, TB4, TB5, TB6>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, Action<TB1, TB2, TB3, TB4, TB5, TB6>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, Action> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11)());
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TB1> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TB1>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, Action<TB1>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TB1 valueB1) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11)(valueB1));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TB1, TB2> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TB1, TB2>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, Action<TB1, TB2>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TB1 valueB1, TB2 valueB2) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11)(valueB1, valueB2));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TB1, TB2, TB3> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TB1, TB2, TB3>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, Action<TB1, TB2, TB3>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TB1 valueB1, TB2 valueB2, TB3 valueB3) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11)(valueB1, valueB2, valueB3));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TB1, TB2, TB3, TB4> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TB1, TB2, TB3, TB4>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, Action<TB1, TB2, TB3, TB4>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11)(valueB1, valueB2, valueB3, valueB4));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TB1, TB2, TB3, TB4, TB5> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TB1, TB2, TB3, TB4, TB5>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, Action<TB1, TB2, TB3, TB4, TB5>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11)(valueB1, valueB2, valueB3, valueB4, valueB5));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, Action> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12)());
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TB1> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TB1>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, Action<TB1>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12, TB1 valueB1) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12)(valueB1));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TB1, TB2> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TB1, TB2>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, Action<TB1, TB2>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12, TB1 valueB1, TB2 valueB2) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12)(valueB1, valueB2));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TB1, TB2, TB3> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TB1, TB2, TB3>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, Action<TB1, TB2, TB3>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12, TB1 valueB1, TB2 valueB2, TB3 valueB3) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12)(valueB1, valueB2, valueB3));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TB1, TB2, TB3, TB4> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TB1, TB2, TB3, TB4>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, Action<TB1, TB2, TB3, TB4>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12)(valueB1, valueB2, valueB3, valueB4));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, Action> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12, TA13 valueA13) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12, valueA13)());
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TB1> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TB1>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, Action<TB1>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12, TA13 valueA13, TB1 valueB1) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12, valueA13)(valueB1));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TB1, TB2> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TB1, TB2>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, Action<TB1, TB2>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12, TA13 valueA13, TB1 valueB1, TB2 valueB2) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12, valueA13)(valueB1, valueB2));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TB1, TB2, TB3> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TB1, TB2, TB3>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, Action<TB1, TB2, TB3>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12, TA13 valueA13, TB1 valueB1, TB2 valueB2, TB3 valueB3) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12, valueA13)(valueB1, valueB2, valueB3));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, Action> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12, TA13 valueA13, TA14 valueA14) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12, valueA13, valueA14)());
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, TB1> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, TB1>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, Action<TB1>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12, TA13 valueA13, TA14 valueA14, TB1 valueB1) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12, valueA13, valueA14)(valueB1));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, TB1, TB2> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, TB1, TB2>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, Action<TB1, TB2>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12, TA13 valueA13, TA14 valueA14, TB1 valueB1, TB2 valueB2) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12, valueA13, valueA14)(valueB1, valueB2));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, TA15> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, TA15>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, TA15, Action> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12, TA13 valueA13, TA14 valueA14, TA15 valueA15) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12, valueA13, valueA14, valueA15)());
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, TA15, TB1> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, TA15, TB1>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, TA15, Action<TB1>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12, TA13 valueA13, TA14 valueA14, TA15 valueA15, TB1 valueB1) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12, valueA13, valueA14, valueA15)(valueB1));
        }

        public static Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, TA15, TA16> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, TA15, TA16>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, TA15, TA16, Action> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12, TA13 valueA13, TA14 valueA14, TA15 valueA15, TA16 valueA16) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12, valueA13, valueA14, valueA15, valueA16)());
        }

        public static Func<TResult> Unbind<TResult>(this Func<Func<TResult>> func)
        {
            return New(() => func()());
        }

        public static Func<TB1, TResult> Unbind<TB1, TResult>(this Func<Func<TB1, TResult>> func)
        {
            return New((TB1 valueB1) => func()(valueB1));
        }

        public static Func<TB1, TB2, TResult> Unbind<TB1, TB2, TResult>(this Func<Func<TB1, TB2, TResult>> func)
        {
            return New((TB1 valueB1, TB2 valueB2) => func()(valueB1, valueB2));
        }

        public static Func<TB1, TB2, TB3, TResult> Unbind<TB1, TB2, TB3, TResult>(this Func<Func<TB1, TB2, TB3, TResult>> func)
        {
            return New((TB1 valueB1, TB2 valueB2, TB3 valueB3) => func()(valueB1, valueB2, valueB3));
        }

        public static Func<TB1, TB2, TB3, TB4, TResult> Unbind<TB1, TB2, TB3, TB4, TResult>(this Func<Func<TB1, TB2, TB3, TB4, TResult>> func)
        {
            return New((TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4) => func()(valueB1, valueB2, valueB3, valueB4));
        }

        public static Func<TB1, TB2, TB3, TB4, TB5, TResult> Unbind<TB1, TB2, TB3, TB4, TB5, TResult>(this Func<Func<TB1, TB2, TB3, TB4, TB5, TResult>> func)
        {
            return New((TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5) => func()(valueB1, valueB2, valueB3, valueB4, valueB5));
        }

        public static Func<TB1, TB2, TB3, TB4, TB5, TB6, TResult> Unbind<TB1, TB2, TB3, TB4, TB5, TB6, TResult>(this Func<Func<TB1, TB2, TB3, TB4, TB5, TB6, TResult>> func)
        {
            return New((TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6) => func()(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6));
        }

        public static Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult> Unbind<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult>(this Func<Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult>> func)
        {
            return New((TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7) => func()(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7));
        }

        public static Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TResult> Unbind<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TResult>(this Func<Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TResult>> func)
        {
            return New((TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8) => func()(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8));
        }

        public static Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TResult> Unbind<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TResult>(this Func<Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TResult>> func)
        {
            return New((TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9) => func()(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9));
        }

        public static Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TResult> Unbind<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TResult>(this Func<Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TResult>> func)
        {
            return New((TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10) => func()(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10));
        }

        public static Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TResult> Unbind<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TResult>(this Func<Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TResult>> func)
        {
            return New((TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11) => func()(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11));
        }

        public static Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TResult> Unbind<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TResult>(this Func<Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TResult>> func)
        {
            return New((TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12) => func()(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12));
        }

        public static Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TResult> Unbind<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TResult>(this Func<Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TResult>> func)
        {
            return New((TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12, TB13 valueB13) => func()(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12, valueB13));
        }

        public static Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14, TResult> Unbind<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14, TResult>(this Func<Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14, TResult>> func)
        {
            return New((TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12, TB13 valueB13, TB14 valueB14) => func()(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12, valueB13, valueB14));
        }

        public static Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14, TB15, TResult> Unbind<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14, TB15, TResult>(this Func<Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14, TB15, TResult>> func)
        {
            return New((TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12, TB13 valueB13, TB14 valueB14, TB15 valueB15) => func()(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12, valueB13, valueB14, valueB15));
        }

        public static Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14, TB15, TB16, TResult> Unbind<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14, TB15, TB16, TResult>(this Func<Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14, TB15, TB16, TResult>> func)
        {
            return New((TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12, TB13 valueB13, TB14 valueB14, TB15 valueB15, TB16 valueB16) => func()(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12, valueB13, valueB14, valueB15, valueB16));
        }

        public static Func<TA1, TResult> Unbind<TA1, TResult>(this Func<TA1, Func<TResult>> func)
        {
            return New((TA1 valueA1) => func(valueA1)());
        }

        public static Func<TA1, TB1, TResult> Unbind<TA1, TB1, TResult>(this Func<TA1, Func<TB1, TResult>> func)
        {
            return New((TA1 valueA1, TB1 valueB1) => func(valueA1)(valueB1));
        }

        public static Func<TA1, TB1, TB2, TResult> Unbind<TA1, TB1, TB2, TResult>(this Func<TA1, Func<TB1, TB2, TResult>> func)
        {
            return New((TA1 valueA1, TB1 valueB1, TB2 valueB2) => func(valueA1)(valueB1, valueB2));
        }

        public static Func<TA1, TB1, TB2, TB3, TResult> Unbind<TA1, TB1, TB2, TB3, TResult>(this Func<TA1, Func<TB1, TB2, TB3, TResult>> func)
        {
            return New((TA1 valueA1, TB1 valueB1, TB2 valueB2, TB3 valueB3) => func(valueA1)(valueB1, valueB2, valueB3));
        }

        public static Func<TA1, TB1, TB2, TB3, TB4, TResult> Unbind<TA1, TB1, TB2, TB3, TB4, TResult>(this Func<TA1, Func<TB1, TB2, TB3, TB4, TResult>> func)
        {
            return New((TA1 valueA1, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4) => func(valueA1)(valueB1, valueB2, valueB3, valueB4));
        }

        public static Func<TA1, TB1, TB2, TB3, TB4, TB5, TResult> Unbind<TA1, TB1, TB2, TB3, TB4, TB5, TResult>(this Func<TA1, Func<TB1, TB2, TB3, TB4, TB5, TResult>> func)
        {
            return New((TA1 valueA1, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5) => func(valueA1)(valueB1, valueB2, valueB3, valueB4, valueB5));
        }

        public static Func<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TResult> Unbind<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TResult>(this Func<TA1, Func<TB1, TB2, TB3, TB4, TB5, TB6, TResult>> func)
        {
            return New((TA1 valueA1, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6) => func(valueA1)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6));
        }

        public static Func<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult> Unbind<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult>(this Func<TA1, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult>> func)
        {
            return New((TA1 valueA1, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7) => func(valueA1)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7));
        }

        public static Func<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TResult> Unbind<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TResult>(this Func<TA1, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TResult>> func)
        {
            return New((TA1 valueA1, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8) => func(valueA1)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8));
        }

        public static Func<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TResult> Unbind<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TResult>(this Func<TA1, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TResult>> func)
        {
            return New((TA1 valueA1, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9) => func(valueA1)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9));
        }

        public static Func<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TResult> Unbind<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TResult>(this Func<TA1, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TResult>> func)
        {
            return New((TA1 valueA1, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10) => func(valueA1)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10));
        }

        public static Func<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TResult> Unbind<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TResult>(this Func<TA1, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TResult>> func)
        {
            return New((TA1 valueA1, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11) => func(valueA1)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11));
        }

        public static Func<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TResult> Unbind<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TResult>(this Func<TA1, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TResult>> func)
        {
            return New((TA1 valueA1, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12) => func(valueA1)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12));
        }

        public static Func<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TResult> Unbind<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TResult>(this Func<TA1, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TResult>> func)
        {
            return New((TA1 valueA1, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12, TB13 valueB13) => func(valueA1)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12, valueB13));
        }

        public static Func<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14, TResult> Unbind<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14, TResult>(this Func<TA1, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14, TResult>> func)
        {
            return New((TA1 valueA1, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12, TB13 valueB13, TB14 valueB14) => func(valueA1)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12, valueB13, valueB14));
        }

        public static Func<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14, TB15, TResult> Unbind<TA1, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14, TB15, TResult>(this Func<TA1, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14, TB15, TResult>> func)
        {
            return New((TA1 valueA1, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12, TB13 valueB13, TB14 valueB14, TB15 valueB15) => func(valueA1)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12, valueB13, valueB14, valueB15));
        }

        public static Func<TA1, TA2, TResult> Unbind<TA1, TA2, TResult>(this Func<TA1, TA2, Func<TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2) => func(valueA1, valueA2)());
        }

        public static Func<TA1, TA2, TB1, TResult> Unbind<TA1, TA2, TB1, TResult>(this Func<TA1, TA2, Func<TB1, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TB1 valueB1) => func(valueA1, valueA2)(valueB1));
        }

        public static Func<TA1, TA2, TB1, TB2, TResult> Unbind<TA1, TA2, TB1, TB2, TResult>(this Func<TA1, TA2, Func<TB1, TB2, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TB1 valueB1, TB2 valueB2) => func(valueA1, valueA2)(valueB1, valueB2));
        }

        public static Func<TA1, TA2, TB1, TB2, TB3, TResult> Unbind<TA1, TA2, TB1, TB2, TB3, TResult>(this Func<TA1, TA2, Func<TB1, TB2, TB3, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TB1 valueB1, TB2 valueB2, TB3 valueB3) => func(valueA1, valueA2)(valueB1, valueB2, valueB3));
        }

        public static Func<TA1, TA2, TB1, TB2, TB3, TB4, TResult> Unbind<TA1, TA2, TB1, TB2, TB3, TB4, TResult>(this Func<TA1, TA2, Func<TB1, TB2, TB3, TB4, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4) => func(valueA1, valueA2)(valueB1, valueB2, valueB3, valueB4));
        }

        public static Func<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TResult> Unbind<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TResult>(this Func<TA1, TA2, Func<TB1, TB2, TB3, TB4, TB5, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5) => func(valueA1, valueA2)(valueB1, valueB2, valueB3, valueB4, valueB5));
        }

        public static Func<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TResult> Unbind<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TResult>(this Func<TA1, TA2, Func<TB1, TB2, TB3, TB4, TB5, TB6, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6) => func(valueA1, valueA2)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6));
        }

        public static Func<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult> Unbind<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult>(this Func<TA1, TA2, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7) => func(valueA1, valueA2)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7));
        }

        public static Func<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TResult> Unbind<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TResult>(this Func<TA1, TA2, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8) => func(valueA1, valueA2)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8));
        }

        public static Func<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TResult> Unbind<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TResult>(this Func<TA1, TA2, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9) => func(valueA1, valueA2)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9));
        }

        public static Func<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TResult> Unbind<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TResult>(this Func<TA1, TA2, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10) => func(valueA1, valueA2)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10));
        }

        public static Func<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TResult> Unbind<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TResult>(this Func<TA1, TA2, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11) => func(valueA1, valueA2)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11));
        }

        public static Func<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TResult> Unbind<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TResult>(this Func<TA1, TA2, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12) => func(valueA1, valueA2)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12));
        }

        public static Func<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TResult> Unbind<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TResult>(this Func<TA1, TA2, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12, TB13 valueB13) => func(valueA1, valueA2)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12, valueB13));
        }

        public static Func<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14, TResult> Unbind<TA1, TA2, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14, TResult>(this Func<TA1, TA2, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TB14, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12, TB13 valueB13, TB14 valueB14) => func(valueA1, valueA2)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12, valueB13, valueB14));
        }

        public static Func<TA1, TA2, TA3, TResult> Unbind<TA1, TA2, TA3, TResult>(this Func<TA1, TA2, TA3, Func<TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3) => func(valueA1, valueA2, valueA3)());
        }

        public static Func<TA1, TA2, TA3, TB1, TResult> Unbind<TA1, TA2, TA3, TB1, TResult>(this Func<TA1, TA2, TA3, Func<TB1, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TB1 valueB1) => func(valueA1, valueA2, valueA3)(valueB1));
        }

        public static Func<TA1, TA2, TA3, TB1, TB2, TResult> Unbind<TA1, TA2, TA3, TB1, TB2, TResult>(this Func<TA1, TA2, TA3, Func<TB1, TB2, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TB1 valueB1, TB2 valueB2) => func(valueA1, valueA2, valueA3)(valueB1, valueB2));
        }

        public static Func<TA1, TA2, TA3, TB1, TB2, TB3, TResult> Unbind<TA1, TA2, TA3, TB1, TB2, TB3, TResult>(this Func<TA1, TA2, TA3, Func<TB1, TB2, TB3, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TB1 valueB1, TB2 valueB2, TB3 valueB3) => func(valueA1, valueA2, valueA3)(valueB1, valueB2, valueB3));
        }

        public static Func<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TResult> Unbind<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TResult>(this Func<TA1, TA2, TA3, Func<TB1, TB2, TB3, TB4, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4) => func(valueA1, valueA2, valueA3)(valueB1, valueB2, valueB3, valueB4));
        }

        public static Func<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TResult> Unbind<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TResult>(this Func<TA1, TA2, TA3, Func<TB1, TB2, TB3, TB4, TB5, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5) => func(valueA1, valueA2, valueA3)(valueB1, valueB2, valueB3, valueB4, valueB5));
        }

        public static Func<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TResult> Unbind<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TResult>(this Func<TA1, TA2, TA3, Func<TB1, TB2, TB3, TB4, TB5, TB6, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6) => func(valueA1, valueA2, valueA3)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6));
        }

        public static Func<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult> Unbind<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult>(this Func<TA1, TA2, TA3, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7) => func(valueA1, valueA2, valueA3)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7));
        }

        public static Func<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TResult> Unbind<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TResult>(this Func<TA1, TA2, TA3, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8) => func(valueA1, valueA2, valueA3)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8));
        }

        public static Func<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TResult> Unbind<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TResult>(this Func<TA1, TA2, TA3, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9) => func(valueA1, valueA2, valueA3)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9));
        }

        public static Func<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TResult> Unbind<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TResult>(this Func<TA1, TA2, TA3, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10) => func(valueA1, valueA2, valueA3)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10));
        }

        public static Func<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TResult> Unbind<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TResult>(this Func<TA1, TA2, TA3, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11) => func(valueA1, valueA2, valueA3)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11));
        }

        public static Func<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TResult> Unbind<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TResult>(this Func<TA1, TA2, TA3, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12) => func(valueA1, valueA2, valueA3)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12));
        }

        public static Func<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TResult> Unbind<TA1, TA2, TA3, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TResult>(this Func<TA1, TA2, TA3, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TB13, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12, TB13 valueB13) => func(valueA1, valueA2, valueA3)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12, valueB13));
        }

        public static Func<TA1, TA2, TA3, TA4, TResult> Unbind<TA1, TA2, TA3, TA4, TResult>(this Func<TA1, TA2, TA3, TA4, Func<TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4) => func(valueA1, valueA2, valueA3, valueA4)());
        }

        public static Func<TA1, TA2, TA3, TA4, TB1, TResult> Unbind<TA1, TA2, TA3, TA4, TB1, TResult>(this Func<TA1, TA2, TA3, TA4, Func<TB1, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TB1 valueB1) => func(valueA1, valueA2, valueA3, valueA4)(valueB1));
        }

        public static Func<TA1, TA2, TA3, TA4, TB1, TB2, TResult> Unbind<TA1, TA2, TA3, TA4, TB1, TB2, TResult>(this Func<TA1, TA2, TA3, TA4, Func<TB1, TB2, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TB1 valueB1, TB2 valueB2) => func(valueA1, valueA2, valueA3, valueA4)(valueB1, valueB2));
        }

        public static Func<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TResult> Unbind<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TResult>(this Func<TA1, TA2, TA3, TA4, Func<TB1, TB2, TB3, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TB1 valueB1, TB2 valueB2, TB3 valueB3) => func(valueA1, valueA2, valueA3, valueA4)(valueB1, valueB2, valueB3));
        }

        public static Func<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TResult> Unbind<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TResult>(this Func<TA1, TA2, TA3, TA4, Func<TB1, TB2, TB3, TB4, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4) => func(valueA1, valueA2, valueA3, valueA4)(valueB1, valueB2, valueB3, valueB4));
        }

        public static Func<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TResult> Unbind<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TResult>(this Func<TA1, TA2, TA3, TA4, Func<TB1, TB2, TB3, TB4, TB5, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5) => func(valueA1, valueA2, valueA3, valueA4)(valueB1, valueB2, valueB3, valueB4, valueB5));
        }

        public static Func<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TB6, TResult> Unbind<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TB6, TResult>(this Func<TA1, TA2, TA3, TA4, Func<TB1, TB2, TB3, TB4, TB5, TB6, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6) => func(valueA1, valueA2, valueA3, valueA4)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6));
        }

        public static Func<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult> Unbind<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult>(this Func<TA1, TA2, TA3, TA4, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7) => func(valueA1, valueA2, valueA3, valueA4)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7));
        }

        public static Func<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TResult> Unbind<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TResult>(this Func<TA1, TA2, TA3, TA4, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8) => func(valueA1, valueA2, valueA3, valueA4)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8));
        }

        public static Func<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TResult> Unbind<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TResult>(this Func<TA1, TA2, TA3, TA4, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9) => func(valueA1, valueA2, valueA3, valueA4)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9));
        }

        public static Func<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TResult> Unbind<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TResult>(this Func<TA1, TA2, TA3, TA4, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10) => func(valueA1, valueA2, valueA3, valueA4)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10));
        }

        public static Func<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TResult> Unbind<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TResult>(this Func<TA1, TA2, TA3, TA4, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11) => func(valueA1, valueA2, valueA3, valueA4)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11));
        }

        public static Func<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TResult> Unbind<TA1, TA2, TA3, TA4, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TResult>(this Func<TA1, TA2, TA3, TA4, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TB12, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11, TB12 valueB12) => func(valueA1, valueA2, valueA3, valueA4)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11, valueB12));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, Func<TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5) => func(valueA1, valueA2, valueA3, valueA4, valueA5)());
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TB1, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TB1, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, Func<TB1, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TB1 valueB1) => func(valueA1, valueA2, valueA3, valueA4, valueA5)(valueB1));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, Func<TB1, TB2, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TB1 valueB1, TB2 valueB2) => func(valueA1, valueA2, valueA3, valueA4, valueA5)(valueB1, valueB2));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, Func<TB1, TB2, TB3, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TB1 valueB1, TB2 valueB2, TB3 valueB3) => func(valueA1, valueA2, valueA3, valueA4, valueA5)(valueB1, valueB2, valueB3));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, Func<TB1, TB2, TB3, TB4, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4) => func(valueA1, valueA2, valueA3, valueA4, valueA5)(valueB1, valueB2, valueB3, valueB4));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TB5, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TB5, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, Func<TB1, TB2, TB3, TB4, TB5, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5) => func(valueA1, valueA2, valueA3, valueA4, valueA5)(valueB1, valueB2, valueB3, valueB4, valueB5));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TB5, TB6, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TB5, TB6, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, Func<TB1, TB2, TB3, TB4, TB5, TB6, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6) => func(valueA1, valueA2, valueA3, valueA4, valueA5)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7) => func(valueA1, valueA2, valueA3, valueA4, valueA5)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8) => func(valueA1, valueA2, valueA3, valueA4, valueA5)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9) => func(valueA1, valueA2, valueA3, valueA4, valueA5)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10) => func(valueA1, valueA2, valueA3, valueA4, valueA5)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TB11, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10, TB11 valueB11) => func(valueA1, valueA2, valueA3, valueA4, valueA5)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10, valueB11));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, Func<TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6)());
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, Func<TB1, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TB1 valueB1) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6)(valueB1));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, Func<TB1, TB2, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TB1 valueB1, TB2 valueB2) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6)(valueB1, valueB2));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, Func<TB1, TB2, TB3, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TB1 valueB1, TB2 valueB2, TB3 valueB3) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6)(valueB1, valueB2, valueB3));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TB4, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TB4, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, Func<TB1, TB2, TB3, TB4, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6)(valueB1, valueB2, valueB3, valueB4));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TB4, TB5, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TB4, TB5, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, Func<TB1, TB2, TB3, TB4, TB5, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6)(valueB1, valueB2, valueB3, valueB4, valueB5));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TB4, TB5, TB6, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TB4, TB5, TB6, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, Func<TB1, TB2, TB3, TB4, TB5, TB6, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TB10, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9, TB10 valueB10) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9, valueB10));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, Func<TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7)());
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, Func<TB1, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TB1 valueB1) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7)(valueB1));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, Func<TB1, TB2, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TB1 valueB1, TB2 valueB2) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7)(valueB1, valueB2));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TB3, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TB3, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, Func<TB1, TB2, TB3, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TB1 valueB1, TB2 valueB2, TB3 valueB3) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7)(valueB1, valueB2, valueB3));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TB3, TB4, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TB3, TB4, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, Func<TB1, TB2, TB3, TB4, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7)(valueB1, valueB2, valueB3, valueB4));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TB3, TB4, TB5, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TB3, TB4, TB5, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, Func<TB1, TB2, TB3, TB4, TB5, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7)(valueB1, valueB2, valueB3, valueB4, valueB5));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TB3, TB4, TB5, TB6, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TB3, TB4, TB5, TB6, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, Func<TB1, TB2, TB3, TB4, TB5, TB6, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TB9, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8, TB9 valueB9) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8, valueB9));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, Func<TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8)());
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, Func<TB1, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TB1 valueB1) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8)(valueB1));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TB2, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TB2, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, Func<TB1, TB2, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TB1 valueB1, TB2 valueB2) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8)(valueB1, valueB2));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TB2, TB3, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TB2, TB3, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, Func<TB1, TB2, TB3, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TB1 valueB1, TB2 valueB2, TB3 valueB3) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8)(valueB1, valueB2, valueB3));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TB2, TB3, TB4, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TB2, TB3, TB4, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, Func<TB1, TB2, TB3, TB4, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8)(valueB1, valueB2, valueB3, valueB4));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TB2, TB3, TB4, TB5, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TB2, TB3, TB4, TB5, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, Func<TB1, TB2, TB3, TB4, TB5, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8)(valueB1, valueB2, valueB3, valueB4, valueB5));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TB2, TB3, TB4, TB5, TB6, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TB2, TB3, TB4, TB5, TB6, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, Func<TB1, TB2, TB3, TB4, TB5, TB6, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TB8, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7, TB8 valueB8) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7, valueB8));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, Func<TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9)());
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TB1, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TB1, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, Func<TB1, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TB1 valueB1) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9)(valueB1));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TB1, TB2, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TB1, TB2, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, Func<TB1, TB2, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TB1 valueB1, TB2 valueB2) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9)(valueB1, valueB2));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TB1, TB2, TB3, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TB1, TB2, TB3, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, Func<TB1, TB2, TB3, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TB1 valueB1, TB2 valueB2, TB3 valueB3) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9)(valueB1, valueB2, valueB3));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TB1, TB2, TB3, TB4, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TB1, TB2, TB3, TB4, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, Func<TB1, TB2, TB3, TB4, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9)(valueB1, valueB2, valueB3, valueB4));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TB1, TB2, TB3, TB4, TB5, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TB1, TB2, TB3, TB4, TB5, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, Func<TB1, TB2, TB3, TB4, TB5, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9)(valueB1, valueB2, valueB3, valueB4, valueB5));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TB1, TB2, TB3, TB4, TB5, TB6, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TB1, TB2, TB3, TB4, TB5, TB6, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, Func<TB1, TB2, TB3, TB4, TB5, TB6, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, Func<TB1, TB2, TB3, TB4, TB5, TB6, TB7, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6, TB7 valueB7) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6, valueB7));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, Func<TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10)());
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TB1, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TB1, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, Func<TB1, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TB1 valueB1) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10)(valueB1));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TB1, TB2, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TB1, TB2, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, Func<TB1, TB2, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TB1 valueB1, TB2 valueB2) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10)(valueB1, valueB2));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TB1, TB2, TB3, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TB1, TB2, TB3, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, Func<TB1, TB2, TB3, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TB1 valueB1, TB2 valueB2, TB3 valueB3) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10)(valueB1, valueB2, valueB3));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TB1, TB2, TB3, TB4, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TB1, TB2, TB3, TB4, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, Func<TB1, TB2, TB3, TB4, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10)(valueB1, valueB2, valueB3, valueB4));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TB1, TB2, TB3, TB4, TB5, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TB1, TB2, TB3, TB4, TB5, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, Func<TB1, TB2, TB3, TB4, TB5, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10)(valueB1, valueB2, valueB3, valueB4, valueB5));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TB1, TB2, TB3, TB4, TB5, TB6, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TB1, TB2, TB3, TB4, TB5, TB6, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, Func<TB1, TB2, TB3, TB4, TB5, TB6, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5, TB6 valueB6) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10)(valueB1, valueB2, valueB3, valueB4, valueB5, valueB6));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, Func<TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11)());
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TB1, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TB1, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, Func<TB1, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TB1 valueB1) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11)(valueB1));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TB1, TB2, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TB1, TB2, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, Func<TB1, TB2, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TB1 valueB1, TB2 valueB2) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11)(valueB1, valueB2));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TB1, TB2, TB3, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TB1, TB2, TB3, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, Func<TB1, TB2, TB3, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TB1 valueB1, TB2 valueB2, TB3 valueB3) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11)(valueB1, valueB2, valueB3));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TB1, TB2, TB3, TB4, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TB1, TB2, TB3, TB4, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, Func<TB1, TB2, TB3, TB4, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11)(valueB1, valueB2, valueB3, valueB4));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TB1, TB2, TB3, TB4, TB5, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TB1, TB2, TB3, TB4, TB5, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, Func<TB1, TB2, TB3, TB4, TB5, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4, TB5 valueB5) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11)(valueB1, valueB2, valueB3, valueB4, valueB5));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, Func<TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12)());
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TB1, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TB1, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, Func<TB1, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12, TB1 valueB1) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12)(valueB1));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TB1, TB2, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TB1, TB2, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, Func<TB1, TB2, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12, TB1 valueB1, TB2 valueB2) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12)(valueB1, valueB2));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TB1, TB2, TB3, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TB1, TB2, TB3, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, Func<TB1, TB2, TB3, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12, TB1 valueB1, TB2 valueB2, TB3 valueB3) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12)(valueB1, valueB2, valueB3));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TB1, TB2, TB3, TB4, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TB1, TB2, TB3, TB4, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, Func<TB1, TB2, TB3, TB4, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12, TB1 valueB1, TB2 valueB2, TB3 valueB3, TB4 valueB4) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12)(valueB1, valueB2, valueB3, valueB4));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, Func<TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12, TA13 valueA13) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12, valueA13)());
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TB1, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TB1, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, Func<TB1, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12, TA13 valueA13, TB1 valueB1) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12, valueA13)(valueB1));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TB1, TB2, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TB1, TB2, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, Func<TB1, TB2, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12, TA13 valueA13, TB1 valueB1, TB2 valueB2) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12, valueA13)(valueB1, valueB2));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TB1, TB2, TB3, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TB1, TB2, TB3, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, Func<TB1, TB2, TB3, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12, TA13 valueA13, TB1 valueB1, TB2 valueB2, TB3 valueB3) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12, valueA13)(valueB1, valueB2, valueB3));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, Func<TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12, TA13 valueA13, TA14 valueA14) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12, valueA13, valueA14)());
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, TB1, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, TB1, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, Func<TB1, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12, TA13 valueA13, TA14 valueA14, TB1 valueB1) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12, valueA13, valueA14)(valueB1));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, TB1, TB2, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, TB1, TB2, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, Func<TB1, TB2, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12, TA13 valueA13, TA14 valueA14, TB1 valueB1, TB2 valueB2) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12, valueA13, valueA14)(valueB1, valueB2));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, TA15, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, TA15, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, TA15, Func<TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12, TA13 valueA13, TA14 valueA14, TA15 valueA15) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12, valueA13, valueA14, valueA15)());
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, TA15, TB1, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, TA15, TB1, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, TA15, Func<TB1, TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12, TA13 valueA13, TA14 valueA14, TA15 valueA15, TB1 valueB1) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12, valueA13, valueA14, valueA15)(valueB1));
        }

        public static Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, TA15, TA16, TResult> Unbind<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, TA15, TA16, TResult>(this Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TA11, TA12, TA13, TA14, TA15, TA16, Func<TResult>> func)
        {
            return New((TA1 valueA1, TA2 valueA2, TA3 valueA3, TA4 valueA4, TA5 valueA5, TA6 valueA6, TA7 valueA7, TA8 valueA8, TA9 valueA9, TA10 valueA10, TA11 valueA11, TA12 valueA12, TA13 valueA13, TA14 valueA14, TA15 valueA15, TA16 valueA16) => func(valueA1, valueA2, valueA3, valueA4, valueA5, valueA6, valueA7, valueA8, valueA9, valueA10, valueA11, valueA12, valueA13, valueA14, valueA15, valueA16)());
        }
    }
}