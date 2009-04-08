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
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Achiral.Extension;
using Achiral;

namespace XSpect.Extension
{
    public static class ObjectUtil
    {
        public static Boolean If<TSource>(this TSource source, Func<TSource, Boolean> predicate)
        {
            return predicate(source);
        }

        public static TResult If<TSource, TResult>(this TSource source, Func<TSource, Boolean> predicate, TResult valueIfTrue, TResult valueIfFalse)
        {
            if (source == null)
            {
                return default(TResult);
            }
            else if (source == null || predicate(source))
            {
                return valueIfTrue;
            }
            else
            {
                return valueIfFalse;
            }
        }

        public static TSource If<TSource>(this TSource source, Func<TSource, Boolean> predicate, TSource valueIfTrue)
        {
            return source.If(predicate, valueIfTrue, source);
        }

        public static TResult If<TSource, TResult>(this TSource source, Func<TSource, Boolean> predicate, Func<TSource, TResult> funcIfTrue, Func<TSource, TResult> funcIfFalse)
        {
            if (source == null)
            {
                return default(TResult);
            }
            else if (source == null || predicate(source))
            {
                return funcIfTrue(source);
            }
            else
            {
                return funcIfFalse(source);
            }
        }

        public static TSource If<TSource>(this TSource source, Func<TSource, Boolean> predicate, Func<TSource, TSource> funcIfTrue)
        {
            return source.If(predicate, funcIfTrue, Lambda.Id<TSource>());
        }

        public static TResult Null<TSource, TResult>(this TSource source, Func<TSource, TResult> func)
            where TSource : class
        {
            if (source == null)
            {
                return default(TResult);
            }
            else
            {
                return func(source);
            }
        }

        public static void Null<TSource>(this TSource source, Action<TSource> action)
        {
            if (source != null)
            {
                action(source);
            }
        }

        public static Nullable<TResult> Nullable<TSource, TResult>(this TSource source, Func<TSource, TResult> func)
            where TSource : class
            where TResult : struct
        {
            if (source == null)
            {
                return null;
            }
            else
            {
                return func(source);
            }
        }

        public static Boolean IsDefault<TSource>(this TSource source)
        {
            return source.Equals(default(TSource));
        }

        public static TResult Do<TSource, TResult>(this TSource source, Func<TSource, TResult> func)
        {
            return func(source);
        }

        public static TSource Do<TSource>(this TSource source, Action action)
        {
            action();
            return source;
        }

        public static TSource Do<TSource>(this TSource source, Action<TSource> action)
        {
            action(source);
            return source;
        }

        public static TSource Print<TSource>(this TSource source)
        {
            return source.Do(o => Console.WriteLine(o));
        }

        public static void Void<TSource>(this TSource source)
        {
            return;
        }

        public static Boolean Try<TSource, TResult>(this TSource source, Func<TSource, TResult> func, out TResult result)
        {
            try
            {
                result = func(source);
                return true;
            }
            catch (Exception)
            {
                result = default(TResult);
                return false;
            }
        }

        public static Boolean Try<TSource>(this TSource source, Action<TSource> action)
        {
            try
            {
                action(source);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}