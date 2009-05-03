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
using XSpect.Extension;
using Achiral;
using Achiral.Extension;

namespace XSpect
{
    partial class Lambda
    {
        public static void Invoke<T1, T2>(this Action<T1, T2> action, Tuple<T1, T2> args)
        {
            action.Invoke(args.Item1, args.Item2);
        }

        public static void Invoke<T1, T2, T3>(this Action<T1, T2, T3> action, Tuple<T1, T2, T3> args)
        {
            action.Invoke(args.Item1, args.Item2, args.Item3);
        }
        
        public static void Invoke<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, Tuple<T1, T2, T3, T4> args)
        {
            action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4);
        }
        
        public static void Invoke<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action, Tuple<T1, T2, T3, T4, T5> args)
        {
            action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5);
        }
        
        public static void Invoke<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> action, Tuple<T1, T2, T3, T4, T5, T6> args)
        {
            action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6);
        }
        
        public static void Invoke<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action, Tuple<T1, T2, T3, T4, T5, T6, T7> args)
        {
            action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7);
        }
        
        public static void Invoke<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action, Tuple<T1, T2, T3, T4, T5, T6, T7, T8> args)
        {
            action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8);
        }

        public static TReturn Invoke<T1, T2, TReturn>(this Func<T1, T2, TReturn> func, Tuple<T1, T2> args)
        {
            return func.Invoke(args.Item1, args.Item2);
        }

        public static TReturn Invoke<T1, T2, T3, TReturn>(this Func<T1, T2, T3, TReturn> func, Tuple<T1, T2, T3> args)
        {
            return func.Invoke(args.Item1, args.Item2, args.Item3);
        }

        public static TReturn Invoke<T1, T2, T3, T4, TReturn>(this Func<T1, T2, T3, T4, TReturn> func, Tuple<T1, T2, T3, T4> args)
        {
            return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4);
        }

        public static TReturn Invoke<T1, T2, T3, T4, T5, TReturn>(this Func<T1, T2, T3, T4, T5, TReturn> func, Tuple<T1, T2, T3, T4, T5> args)
        {
            return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5);
        }

        public static TReturn Invoke<T1, T2, T3, T4, T5, T6, TReturn>(this Func<T1, T2, T3, T4, T5, T6, TReturn> func, Tuple<T1, T2, T3, T4, T5, T6> args)
        {
            return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6);
        }

        public static TReturn Invoke<T1, T2, T3, T4, T5, T6, T7, TReturn>(this Func<T1, T2, T3, T4, T5, T6, T7, TReturn> func, Tuple<T1, T2, T3, T4, T5, T6, T7> args)
        {
            return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7);
        }

        public static TReturn Invoke<T1, T2, T3, T4, T5, T6, T7, T8, TReturn>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TReturn> func, Tuple<T1, T2, T3, T4, T5, T6, T7, T8> args)
        {
            return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8);
        }
    }
}