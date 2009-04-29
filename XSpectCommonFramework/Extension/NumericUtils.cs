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

namespace XSpect.Extension
{
    public static class NumericUtil
    {
        public static void Times(this Int32 self, Action action)
        {
            if (self < 0)
            {
                return;
            }
            for (Int32 i = 0; i < self; ++i)
            {
                action();
            }
        }

        public static void Times(this Int32 self, Action<Int32> action)
        {
            if (self < 0)
            {
                return;
            }
            for (Int32 i = 0; i < self; ++i)
            {
                action(i);
            }
        }

        public static IEnumerable<Int32> Step(this Int32 self, Int32 limit)
        {
            return Step(self, limit, 1);
        }

        public static IEnumerable<Int32> Step(this Int32 self, Int32 limit, Int32 step)
        {
            for (Int32 i = self; i <= limit; i += step)
            {
                yield return i;
            }
        }

        public static void Times(this Int64 self, Action action)
        {
            if (self < 0)
            {
                return;
            }
            for (Int64 i = 0; i < self; ++i)
            {
                action();
            }
        }

        public static void Times(this Int64 self, Action<Int64> action)
        {
            if (self < 0)
            {
                return;
            }
            for (Int64 i = 0; i < self; ++i)
            {
                action(i);
            }
        }

        public static IEnumerable<Int64> Step(this Int64 self, Int64 limit)
        {
            return Step(self, limit, 1);
        }

        public static IEnumerable<Int64> Step(this Int64 self, Int64 limit, Int64 step)
        {
            for (Int64 i = self; i <= limit; i += step)
            {
                yield return i;
            }
        }
    }
}
