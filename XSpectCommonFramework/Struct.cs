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
using Achiral;

namespace XSpect
{
    public struct Struct<T1, T2>
        : IEquatable<Struct<T1, T2>>
    {
        public static Struct<T1, T2> Default
        {
            get
            {
                return new Struct<T1, T2>();
            }
        }

        public T1 Item1
        {
            get;
            set;
        }

        public T2 Item2
        {
            get;
            set;
        }

        public Struct(T1 item1, T2 item2)
            : this()
        {
            this.Item1 = item1;
            this.Item2 = item2;
        }

        public static Boolean operator ==(Struct<T1, T2> self, Struct<T1, T2> other)
        {
            return self.Equals(other);
        }

        public static Boolean operator !=(Struct<T1, T2> self, Struct<T1, T2> other)
        {
            return !self.Equals(other);
        }

        public Boolean Equals(Struct<T1, T2> other)
        {
            return this.Item1.Equals(other.Item1) && this.Item2.Equals(other.Item2);
        }

        public override Boolean Equals(Object obj)
        {
            return obj is Struct<T1, T2> && this.Equals((Struct<T1, T2>) obj);
        }

        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        public TResult Select<TResult>(Func<T1, T2, TResult> selector)
        {
            return selector(this.Item1, this.Item2);
        }
    }

    public struct Struct<T1, T2, T3>
        : IEquatable<Struct<T1, T2, T3>>
    {
        public static Struct<T1, T2, T3> Default
        {
            get
            {
                return new Struct<T1, T2, T3>();
            }
        }

        public T1 Item1
        {
            get;
            set;
        }

        public T2 Item2
        {
            get;
            set;
        }

        public T3 Item3
        {
            get;
            set;
        }

        public Struct(T1 item1, T2 item2, T3 item3)
            : this()
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
        }

        public static Boolean operator ==(Struct<T1, T2, T3> self, Struct<T1, T2, T3> other)
        {
            return self.Equals(other);
        }

        public static Boolean operator !=(Struct<T1, T2, T3> self, Struct<T1, T2, T3> other)
        {
            return !self.Equals(other);
        }

        public Boolean Equals(Struct<T1, T2, T3> other)
        {
            return
                this.Item1.Equals(other.Item1) &&
                this.Item2.Equals(other.Item2) &&
                this.Item3.Equals(other.Item3);
        }

        public override Boolean Equals(Object obj)
        {
            return obj is Struct<T1, T2, T3> && this.Equals((Struct<T1, T2, T3>) obj);
        }

        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        public TResult Select<TResult>(Func<T1, T2, T3, TResult> selector)
        {
            return selector(this.Item1, this.Item2, this.Item3);
        }
    }

    public struct Struct<T1, T2, T3, T4>
        : IEquatable<Struct<T1, T2, T3, T4>>
    {
        public static Struct<T1, T2, T3, T4> Default
        {
            get
            {
                return new Struct<T1, T2, T3, T4>();
            }
        }

        public T1 Item1
        {
            get;
            set;
        }

        public T2 Item2
        {
            get;
            set;
        }

        public T3 Item3
        {
            get;
            set;
        }

        public T4 Item4
        {
            get;
            set;
        }

        public Struct(T1 item1, T2 item2, T3 item3, T4 item4)
            : this()
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
            this.Item4 = item4;
        }

        public static Boolean operator ==(Struct<T1, T2, T3, T4> self, Struct<T1, T2, T3, T4> other)
        {
            return self.Equals(other);
        }

        public static Boolean operator !=(Struct<T1, T2, T3, T4> self, Struct<T1, T2, T3, T4> other)
        {
            return !self.Equals(other);
        }

        public Boolean Equals(Struct<T1, T2, T3, T4> other)
        {
            return
                this.Item1.Equals(other.Item1) &&
                this.Item2.Equals(other.Item2) &&
                this.Item3.Equals(other.Item3) &&
                this.Item4.Equals(other.Item4);
        }

        public override Boolean Equals(Object obj)
        {
            return obj is Struct<T1, T2, T3, T4> && this.Equals((Struct<T1, T2, T3, T4>) obj);
        }

        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        public TResult Select<TResult>(Func<T1, T2, T3, T4, TResult> selector)
        {
            return selector(this.Item1, this.Item2, this.Item3, this.Item4);
        }
    }
}