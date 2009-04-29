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
using System.IO;
using Achiral.Extension;
using Achiral;

namespace XSpect.Extension
{
    public static class ObjectUtil
    {
        public static Boolean If<TReceiver>(this TReceiver self, Func<TReceiver, Boolean> predicate)
        {
            return predicate(self);
        }

        public static TResult If<TReceiver, TResult>(this TReceiver self, Func<TReceiver, Boolean> predicate, TResult valueIfTrue, TResult valueIfFalse)
        {
            if (self == null)
            {
                return default(TResult);
            }
            else if (self == null || predicate(self))
            {
                return valueIfTrue;
            }
            else
            {
                return valueIfFalse;
            }
        }

        public static TReceiver If<TReceiver>(this TReceiver self, Func<TReceiver, Boolean> predicate, TReceiver valueIfTrue)
        {
            return self.If(predicate, valueIfTrue, self);
        }

        public static TResult If<TReceiver, TResult>(this TReceiver self, Func<TReceiver, Boolean> predicate, Func<TReceiver, TResult> funcIfTrue, Func<TReceiver, TResult> funcIfFalse)
        {
            if (self == null)
            {
                return default(TResult);
            }
            else if (self == null || predicate(self))
            {
                return funcIfTrue(self);
            }
            else
            {
                return funcIfFalse(self);
            }
        }

        public static TReceiver If<TReceiver>(this TReceiver self, Func<TReceiver, Boolean> predicate, Func<TReceiver, TReceiver> funcIfTrue)
        {
            return self.If(predicate, funcIfTrue, Lambda.Id<TReceiver>());
        }

        public static TResult Null<TReceiver, TResult>(this TReceiver self, Func<TReceiver, TResult> func, TResult valueIfNull)
            where TReceiver : class
        {
            if (self == null)
            {
                return valueIfNull;
            }
            else
            {
                return func(self);
            }
        }

        public static TResult Null<TReceiver, TResult>(this TReceiver self, Func<TReceiver, TResult> func)
            where TReceiver : class
        {
            return Null(self, func, default(TResult));
        }

        public static void Null<TReceiver>(this TReceiver self, Action<TReceiver> action)
        {
            if (self != null)
            {
                action(self);
            }
        }

        public static Nullable<TResult> Nullable<TReceiver, TResult>(this TReceiver self, Func<TReceiver, TResult> func)
            where TResult : struct
        {
            if (self == null)
            {
                return null;
            }
            else
            {
                return func(self);
            }
        }

        public static Nullable<TReceiver> NullIf<TReceiver>(this TReceiver self, Func<TReceiver, Boolean> predicate)
            where TReceiver : struct
        {
            if (predicate(self))
            {
                return null;
            }
            else
            {
                return self;
            }
        }

        public static Boolean IsDefault<TReceiver>(this TReceiver self)
        {
            return self.Equals(default(TReceiver));
        }

        public static TResult Do<TReceiver, TResult>(this TReceiver self, Func<TReceiver, TResult> func)
        {
            return func(self);
        }

        public static TResult[] Do<TReceiver, TResult>(this TReceiver self, params Func<TReceiver, TResult>[] funcs)
        {
            return funcs.Select(f => f(self)).ToArray();
        }

        public static TReceiver Do<TReceiver>(this TReceiver self, params Action<TReceiver>[] actions)
        {
            actions.ForEach(a => a(self));
            return self;
        }

        public static TReceiver Write<TReceiver>(this TReceiver self)
        {
            return self.Write(Console.Out);
        }

        public static TReceiver WriteLine<TReceiver>(this TReceiver self)
        {
            return self.WriteLine(Console.Out);
        }

        public static TReceiver Write<TReceiver>(this TReceiver self, TextWriter writer)
        {
            return self.Do(o => writer.Write(o));
        }

        public static TReceiver WriteLine<TReceiver>(this TReceiver self, TextWriter writer)
        {
            return self.Do(o => writer.WriteLine(o));
        }

        public static void Void(this Object self)
        {
            return;
        }

        public static Boolean True(this Object self)
        {
            return true;
        }

        public static Boolean False(this Object self)
        {
            return false;
        }

        public static Boolean Try<TReceiver, TResult>(this TReceiver self, Func<TReceiver, TResult> func, out TResult result)
        {
            try
            {
                result = func(self);
                return true;
            }
            catch (Exception)
            {
                result = default(TResult);
                return false;
            }
        }

        public static Boolean Try<TReceiver>(this TReceiver self, Action<TReceiver> action)
        {
            try
            {
                action(self);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static TResult Scope<TReceiver, TResult>(
            this TReceiver self,
            Action<TReceiver> begin,
            Func<TReceiver, TResult> body,
            Action<TReceiver> end
        )
        {
            if (begin != null)
            {
                begin(self);
            }
            TResult ret = body(self);
            if (begin != null)
            {
                end(self);
            }
            return ret;
        }

        public static void Scope<TReceiver>(
            this TReceiver self,
            Action<TReceiver> begin,
            Action<TReceiver> body,
            Action<TReceiver> end
        )
        {
            if (begin != null)
            {
                begin(self);
            }
            body(self);
            if (begin != null)
            {
                end(self);
            }
        }

        public static Boolean EqualsAny(this Object self, params Object[] objects)
        {
            return objects.Any(self.Equals);
        }

        public static Boolean EqualsAll(this Object self, params Object[] objects)
        {
            return objects.All(self.Equals);
        }

    }
}