// -*- mode: csharp; encoding: utf-8; -*-
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
        public static Action New(Action action)
        {
            return action;
        }

        public static Action<T1> New<T1>(Action<T1> action)
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

        public static Func<TResult> New<TResult>(Func<TResult> func)
        {
            return func;
        }

        public static Func<T1, TResult> New<T1, TResult>(Func<T1, TResult> func)
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
    }
}