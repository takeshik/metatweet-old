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

    public struct Struct<T1, T2, T3, T4, T5>
    : IEquatable<Struct<T1, T2, T3, T4, T5>>
    {
        public static Struct<T1, T2, T3, T4, T5> Default
        {
            get
            {
                return new Struct<T1, T2, T3, T4, T5>();
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

        public T5 Item5
        {
            get;
            set;
        }

        public Struct(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
            : this()
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
            this.Item4 = item4;
            this.Item5 = item5;
        }

        public static Boolean operator ==(Struct<T1, T2, T3, T4, T5> self, Struct<T1, T2, T3, T4, T5> other)
        {
            return self.Equals(other);
        }

        public static Boolean operator !=(Struct<T1, T2, T3, T4, T5> self, Struct<T1, T2, T3, T4, T5> other)
        {
            return !self.Equals(other);
        }

        public Boolean Equals(Struct<T1, T2, T3, T4, T5> other)
        {
            return
                this.Item1.Equals(other.Item1) &&
                this.Item2.Equals(other.Item2) &&
                this.Item3.Equals(other.Item3) &&
                this.Item4.Equals(other.Item4) &&
                this.Item5.Equals(other.Item5);
        }
        public override Boolean Equals(Object obj)
        {
            return obj is Struct<T1, T2, T3, T4, T5> && this.Equals((Struct<T1, T2, T3, T4, T5>) obj);
        }

        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        public TResult Select<TResult>(Func<T1, T2, T3, T4, T5, TResult> selector)
        {
            return selector(this.Item1, this.Item2, this.Item3, this.Item4, this.Item5);
        }
    }

    public struct Struct<T1, T2, T3, T4, T5, T6>
        : IEquatable<Struct<T1, T2, T3, T4, T5, T6>>
    {
        public static Struct<T1, T2, T3, T4, T5, T6> Default
        {
            get
            {
                return new Struct<T1, T2, T3, T4, T5, T6>();
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

        public T5 Item5
        {
            get;
            set;
        }

        public T6 Item6
        {
            get;
            set;
        }

        public Struct(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
            : this()
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
            this.Item4 = item4;
            this.Item5 = item5;
            this.Item6 = item6;
        }

        public static Boolean operator ==(Struct<T1, T2, T3, T4, T5, T6> self, Struct<T1, T2, T3, T4, T5, T6> other)
        {
            return self.Equals(other);
        }

        public static Boolean operator !=(Struct<T1, T2, T3, T4, T5, T6> self, Struct<T1, T2, T3, T4, T5, T6> other)
        {
            return !self.Equals(other);
        }

        public Boolean Equals(Struct<T1, T2, T3, T4, T5, T6> other)
        {
            return
                this.Item1.Equals(other.Item1) &&
                this.Item2.Equals(other.Item2) &&
                this.Item3.Equals(other.Item3) &&
                this.Item4.Equals(other.Item4) &&
                this.Item5.Equals(other.Item5) &&
                this.Item6.Equals(other.Item6);
        }
        public override Boolean Equals(Object obj)
        {
            return obj is Struct<T1, T2, T3, T4, T5, T6> && this.Equals((Struct<T1, T2, T3, T4, T5, T6>) obj);
        }

        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        public TResult Select<TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> selector)
        {
            return selector(this.Item1, this.Item2, this.Item3, this.Item4, this.Item5, this.Item6);
        }
    }

    public struct Struct<T1, T2, T3, T4, T5, T6, T7>
        : IEquatable<Struct<T1, T2, T3, T4, T5, T6, T7>>
    {
        public static Struct<T1, T2, T3, T4, T5, T6, T7> Default
        {
            get
            {
                return new Struct<T1, T2, T3, T4, T5, T6, T7>();
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

        public T5 Item5
        {
            get;
            set;
        }

        public T6 Item6
        {
            get;
            set;
        }

        public T7 Item7
        {
            get;
            set;
        }

        public Struct(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
            : this()
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
            this.Item4 = item4;
            this.Item5 = item5;
            this.Item6 = item6;
            this.Item7 = item7;
        }

        public static Boolean operator ==(Struct<T1, T2, T3, T4, T5, T6, T7> self, Struct<T1, T2, T3, T4, T5, T6, T7> other)
        {
            return self.Equals(other);
        }

        public static Boolean operator !=(Struct<T1, T2, T3, T4, T5, T6, T7> self, Struct<T1, T2, T3, T4, T5, T6, T7> other)
        {
            return !self.Equals(other);
        }

        public Boolean Equals(Struct<T1, T2, T3, T4, T5, T6, T7> other)
        {
            return
                this.Item1.Equals(other.Item1) &&
                this.Item2.Equals(other.Item2) &&
                this.Item3.Equals(other.Item3) &&
                this.Item4.Equals(other.Item4) &&
                this.Item5.Equals(other.Item5) &&
                this.Item6.Equals(other.Item6) &&
                this.Item7.Equals(other.Item7);
        }
        public override Boolean Equals(Object obj)
        {
            return obj is Struct<T1, T2, T3, T4, T5, T6, T7> && this.Equals((Struct<T1, T2, T3, T4, T5, T6, T7>) obj);
        }

        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        public TResult Select<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> selector)
        {
            return selector(this.Item1, this.Item2, this.Item3, this.Item4, this.Item5, this.Item6, this.Item7);
        }
    }

    public struct Struct<T1, T2, T3, T4, T5, T6, T7, T8>
        : IEquatable<Struct<T1, T2, T3, T4, T5, T6, T7, T8>>
    {
        public static Struct<T1, T2, T3, T4, T5, T6, T7, T8> Default
        {
            get
            {
                return new Struct<T1, T2, T3, T4, T5, T6, T7, T8>();
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

        public T5 Item5
        {
            get;
            set;
        }

        public T6 Item6
        {
            get;
            set;
        }

        public T7 Item7
        {
            get;
            set;
        }

        public T8 Item8
        {
            get;
            set;
        }

        public Struct(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
            : this()
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
            this.Item4 = item4;
            this.Item5 = item5;
            this.Item6 = item6;
            this.Item7 = item7;
            this.Item8 = item8;
        }

        public static Boolean operator ==(Struct<T1, T2, T3, T4, T5, T6, T7, T8> self, Struct<T1, T2, T3, T4, T5, T6, T7, T8> other)
        {
            return self.Equals(other);
        }

        public static Boolean operator !=(Struct<T1, T2, T3, T4, T5, T6, T7, T8> self, Struct<T1, T2, T3, T4, T5, T6, T7, T8> other)
        {
            return !self.Equals(other);
        }

        public Boolean Equals(Struct<T1, T2, T3, T4, T5, T6, T7, T8> other)
        {
            return
                this.Item1.Equals(other.Item1) &&
                this.Item2.Equals(other.Item2) &&
                this.Item3.Equals(other.Item3) &&
                this.Item4.Equals(other.Item4) &&
                this.Item5.Equals(other.Item5) &&
                this.Item6.Equals(other.Item6) &&
                this.Item7.Equals(other.Item7) &&
                this.Item8.Equals(other.Item8);
        }
        public override Boolean Equals(Object obj)
        {
            return obj is Struct<T1, T2, T3, T4, T5, T6, T7, T8> && this.Equals((Struct<T1, T2, T3, T4, T5, T6, T7, T8>) obj);
        }

        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        public TResult Select<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> selector)
        {
            return selector(this.Item1, this.Item2, this.Item3, this.Item4, this.Item5, this.Item6, this.Item7, this.Item8);
        }
    }

    public struct Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9>
    : IEquatable<Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9>>
    {
        public static Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9> Default
        {
            get
            {
                return new Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9>();
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

        public T5 Item5
        {
            get;
            set;
        }

        public T6 Item6
        {
            get;
            set;
        }

        public T7 Item7
        {
            get;
            set;
        }

        public T8 Item8
        {
            get;
            set;
        }

        public T9 Item9
        {
            get;
            set;
        }

        public Struct(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9)
            : this()
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
            this.Item4 = item4;
            this.Item5 = item5;
            this.Item6 = item6;
            this.Item7 = item7;
            this.Item8 = item8;
            this.Item9 = item9;
        }

        public static Boolean operator ==(Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9> other)
        {
            return self.Equals(other);
        }

        public static Boolean operator !=(Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9> other)
        {
            return !self.Equals(other);
        }

        public Boolean Equals(Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9> other)
        {
            return
                this.Item1.Equals(other.Item1) &&
                this.Item2.Equals(other.Item2) &&
                this.Item3.Equals(other.Item3) &&
                this.Item4.Equals(other.Item4) &&
                this.Item5.Equals(other.Item5) &&
                this.Item6.Equals(other.Item6) &&
                this.Item7.Equals(other.Item7) &&
                this.Item8.Equals(other.Item8) &&
                this.Item9.Equals(other.Item9);
        }
        public override Boolean Equals(Object obj)
        {
            return obj is Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9> && this.Equals((Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9>) obj);
        }

        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        public TResult Select<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> selector)
        {
            return selector(this.Item1, this.Item2, this.Item3, this.Item4, this.Item5, this.Item6, this.Item7, this.Item8, this.Item9);
        }
    }

    public struct Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        : IEquatable<Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>
    {
        public static Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Default
        {
            get
            {
                return new Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>();
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

        public T5 Item5
        {
            get;
            set;
        }

        public T6 Item6
        {
            get;
            set;
        }

        public T7 Item7
        {
            get;
            set;
        }

        public T8 Item8
        {
            get;
            set;
        }

        public T9 Item9
        {
            get;
            set;
        }

        public T10 Item10
        {
            get;
            set;
        }

        public Struct(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10)
            : this()
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
            this.Item4 = item4;
            this.Item5 = item5;
            this.Item6 = item6;
            this.Item7 = item7;
            this.Item8 = item8;
            this.Item9 = item9;
            this.Item10 = item10;
        }

        public static Boolean operator ==(Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> other)
        {
            return self.Equals(other);
        }

        public static Boolean operator !=(Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> other)
        {
            return !self.Equals(other);
        }

        public Boolean Equals(Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> other)
        {
            return
                this.Item1.Equals(other.Item1) &&
                this.Item2.Equals(other.Item2) &&
                this.Item3.Equals(other.Item3) &&
                this.Item4.Equals(other.Item4) &&
                this.Item5.Equals(other.Item5) &&
                this.Item6.Equals(other.Item6) &&
                this.Item7.Equals(other.Item7) &&
                this.Item8.Equals(other.Item8) &&
                this.Item9.Equals(other.Item9) &&
                this.Item10.Equals(other.Item10);
        }
        public override Boolean Equals(Object obj)
        {
            return obj is Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> && this.Equals((Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>) obj);
        }

        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        public TResult Select<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> selector)
        {
            return selector(this.Item1, this.Item2, this.Item3, this.Item4, this.Item5, this.Item6, this.Item7, this.Item8, this.Item9, this.Item10);
        }
    }

    public struct Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        : IEquatable<Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>
    {
        public static Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Default
        {
            get
            {
                return new Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>();
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

        public T5 Item5
        {
            get;
            set;
        }

        public T6 Item6
        {
            get;
            set;
        }

        public T7 Item7
        {
            get;
            set;
        }

        public T8 Item8
        {
            get;
            set;
        }

        public T9 Item9
        {
            get;
            set;
        }

        public T10 Item10
        {
            get;
            set;
        }

        public T11 Item11
        {
            get;
            set;
        }

        public Struct(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11)
            : this()
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
            this.Item4 = item4;
            this.Item5 = item5;
            this.Item6 = item6;
            this.Item7 = item7;
            this.Item8 = item8;
            this.Item9 = item9;
            this.Item10 = item10;
            this.Item11 = item11;
        }

        public static Boolean operator ==(Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> other)
        {
            return self.Equals(other);
        }

        public static Boolean operator !=(Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> other)
        {
            return !self.Equals(other);
        }

        public Boolean Equals(Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> other)
        {
            return
                this.Item1.Equals(other.Item1) &&
                this.Item2.Equals(other.Item2) &&
                this.Item3.Equals(other.Item3) &&
                this.Item4.Equals(other.Item4) &&
                this.Item5.Equals(other.Item5) &&
                this.Item6.Equals(other.Item6) &&
                this.Item7.Equals(other.Item7) &&
                this.Item8.Equals(other.Item8) &&
                this.Item9.Equals(other.Item9) &&
                this.Item10.Equals(other.Item10) &&
                this.Item11.Equals(other.Item11);
        }
        public override Boolean Equals(Object obj)
        {
            return obj is Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> && this.Equals((Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>) obj);
        }

        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        public TResult Select<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> selector)
        {
            return selector(this.Item1, this.Item2, this.Item3, this.Item4, this.Item5, this.Item6, this.Item7, this.Item8, this.Item9, this.Item10, this.Item11);
        }
    }

    public struct Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        : IEquatable<Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>
    {
        public static Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Default
        {
            get
            {
                return new Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>();
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

        public T5 Item5
        {
            get;
            set;
        }

        public T6 Item6
        {
            get;
            set;
        }

        public T7 Item7
        {
            get;
            set;
        }

        public T8 Item8
        {
            get;
            set;
        }

        public T9 Item9
        {
            get;
            set;
        }

        public T10 Item10
        {
            get;
            set;
        }

        public T11 Item11
        {
            get;
            set;
        }

        public T12 Item12
        {
            get;
            set;
        }

        public Struct(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12)
            : this()
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
            this.Item4 = item4;
            this.Item5 = item5;
            this.Item6 = item6;
            this.Item7 = item7;
            this.Item8 = item8;
            this.Item9 = item9;
            this.Item10 = item10;
            this.Item11 = item11;
            this.Item12 = item12;
        }

        public static Boolean operator ==(Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> other)
        {
            return self.Equals(other);
        }

        public static Boolean operator !=(Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> other)
        {
            return !self.Equals(other);
        }

        public Boolean Equals(Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> other)
        {
            return
                this.Item1.Equals(other.Item1) &&
                this.Item2.Equals(other.Item2) &&
                this.Item3.Equals(other.Item3) &&
                this.Item4.Equals(other.Item4) &&
                this.Item5.Equals(other.Item5) &&
                this.Item6.Equals(other.Item6) &&
                this.Item7.Equals(other.Item7) &&
                this.Item8.Equals(other.Item8) &&
                this.Item9.Equals(other.Item9) &&
                this.Item10.Equals(other.Item10) &&
                this.Item11.Equals(other.Item11) &&
                this.Item12.Equals(other.Item12);
        }
        public override Boolean Equals(Object obj)
        {
            return obj is Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> && this.Equals((Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>) obj);
        }

        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        public TResult Select<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> selector)
        {
            return selector(this.Item1, this.Item2, this.Item3, this.Item4, this.Item5, this.Item6, this.Item7, this.Item8, this.Item9, this.Item10, this.Item11, this.Item12);
        }
    }

    public struct Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        : IEquatable<Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>
    {
        public static Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Default
        {
            get
            {
                return new Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>();
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

        public T5 Item5
        {
            get;
            set;
        }

        public T6 Item6
        {
            get;
            set;
        }

        public T7 Item7
        {
            get;
            set;
        }

        public T8 Item8
        {
            get;
            set;
        }

        public T9 Item9
        {
            get;
            set;
        }

        public T10 Item10
        {
            get;
            set;
        }

        public T11 Item11
        {
            get;
            set;
        }

        public T12 Item12
        {
            get;
            set;
        }

        public T13 Item13
        {
            get;
            set;
        }

        public Struct(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13)
            : this()
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
            this.Item4 = item4;
            this.Item5 = item5;
            this.Item6 = item6;
            this.Item7 = item7;
            this.Item8 = item8;
            this.Item9 = item9;
            this.Item10 = item10;
            this.Item11 = item11;
            this.Item12 = item12;
            this.Item13 = item13;
        }

        public static Boolean operator ==(Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> other)
        {
            return self.Equals(other);
        }

        public static Boolean operator !=(Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> other)
        {
            return !self.Equals(other);
        }

        public Boolean Equals(Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> other)
        {
            return
                this.Item1.Equals(other.Item1) &&
                this.Item2.Equals(other.Item2) &&
                this.Item3.Equals(other.Item3) &&
                this.Item4.Equals(other.Item4) &&
                this.Item5.Equals(other.Item5) &&
                this.Item6.Equals(other.Item6) &&
                this.Item7.Equals(other.Item7) &&
                this.Item8.Equals(other.Item8) &&
                this.Item9.Equals(other.Item9) &&
                this.Item10.Equals(other.Item10) &&
                this.Item11.Equals(other.Item11) &&
                this.Item12.Equals(other.Item12) &&
                this.Item13.Equals(other.Item13);
        }
        public override Boolean Equals(Object obj)
        {
            return obj is Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> && this.Equals((Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>) obj);
        }

        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        public TResult Select<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> selector)
        {
            return selector(this.Item1, this.Item2, this.Item3, this.Item4, this.Item5, this.Item6, this.Item7, this.Item8, this.Item9, this.Item10, this.Item11, this.Item12, this.Item13);
        }
    }

    public struct Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        : IEquatable<Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>
    {
        public static Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Default
        {
            get
            {
                return new Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>();
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

        public T5 Item5
        {
            get;
            set;
        }

        public T6 Item6
        {
            get;
            set;
        }

        public T7 Item7
        {
            get;
            set;
        }

        public T8 Item8
        {
            get;
            set;
        }

        public T9 Item9
        {
            get;
            set;
        }

        public T10 Item10
        {
            get;
            set;
        }

        public T11 Item11
        {
            get;
            set;
        }

        public T12 Item12
        {
            get;
            set;
        }

        public T13 Item13
        {
            get;
            set;
        }

        public T14 Item14
        {
            get;
            set;
        }

        public Struct(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14)
            : this()
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
            this.Item4 = item4;
            this.Item5 = item5;
            this.Item6 = item6;
            this.Item7 = item7;
            this.Item8 = item8;
            this.Item9 = item9;
            this.Item10 = item10;
            this.Item11 = item11;
            this.Item12 = item12;
            this.Item13 = item13;
            this.Item14 = item14;
        }

        public static Boolean operator ==(Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> other)
        {
            return self.Equals(other);
        }

        public static Boolean operator !=(Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> other)
        {
            return !self.Equals(other);
        }

        public Boolean Equals(Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> other)
        {
            return
                this.Item1.Equals(other.Item1) &&
                this.Item2.Equals(other.Item2) &&
                this.Item3.Equals(other.Item3) &&
                this.Item4.Equals(other.Item4) &&
                this.Item5.Equals(other.Item5) &&
                this.Item6.Equals(other.Item6) &&
                this.Item7.Equals(other.Item7) &&
                this.Item8.Equals(other.Item8) &&
                this.Item9.Equals(other.Item9) &&
                this.Item10.Equals(other.Item10) &&
                this.Item11.Equals(other.Item11) &&
                this.Item12.Equals(other.Item12) &&
                this.Item13.Equals(other.Item13) &&
                this.Item14.Equals(other.Item14);
        }
        public override Boolean Equals(Object obj)
        {
            return obj is Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> && this.Equals((Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>) obj);
        }

        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        public TResult Select<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> selector)
        {
            return selector(this.Item1, this.Item2, this.Item3, this.Item4, this.Item5, this.Item6, this.Item7, this.Item8, this.Item9, this.Item10, this.Item11, this.Item12, this.Item13, this.Item14);
        }
    }

    public struct Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        : IEquatable<Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>
    {
        public static Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Default
        {
            get
            {
                return new Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>();
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

        public T5 Item5
        {
            get;
            set;
        }

        public T6 Item6
        {
            get;
            set;
        }

        public T7 Item7
        {
            get;
            set;
        }

        public T8 Item8
        {
            get;
            set;
        }

        public T9 Item9
        {
            get;
            set;
        }

        public T10 Item10
        {
            get;
            set;
        }

        public T11 Item11
        {
            get;
            set;
        }

        public T12 Item12
        {
            get;
            set;
        }

        public T13 Item13
        {
            get;
            set;
        }

        public T14 Item14
        {
            get;
            set;
        }

        public T15 Item15
        {
            get;
            set;
        }

        public Struct(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15)
            : this()
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
            this.Item4 = item4;
            this.Item5 = item5;
            this.Item6 = item6;
            this.Item7 = item7;
            this.Item8 = item8;
            this.Item9 = item9;
            this.Item10 = item10;
            this.Item11 = item11;
            this.Item12 = item12;
            this.Item13 = item13;
            this.Item14 = item14;
            this.Item15 = item15;
        }

        public static Boolean operator ==(Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> other)
        {
            return self.Equals(other);
        }

        public static Boolean operator !=(Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> other)
        {
            return !self.Equals(other);
        }

        public Boolean Equals(Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> other)
        {
            return
                this.Item1.Equals(other.Item1) &&
                this.Item2.Equals(other.Item2) &&
                this.Item3.Equals(other.Item3) &&
                this.Item4.Equals(other.Item4) &&
                this.Item5.Equals(other.Item5) &&
                this.Item6.Equals(other.Item6) &&
                this.Item7.Equals(other.Item7) &&
                this.Item8.Equals(other.Item8) &&
                this.Item9.Equals(other.Item9) &&
                this.Item10.Equals(other.Item10) &&
                this.Item11.Equals(other.Item11) &&
                this.Item12.Equals(other.Item12) &&
                this.Item13.Equals(other.Item13) &&
                this.Item14.Equals(other.Item14) &&
                this.Item15.Equals(other.Item15);
        }
        public override Boolean Equals(Object obj)
        {
            return obj is Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> && this.Equals((Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>) obj);
        }

        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        public TResult Select<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> selector)
        {
            return selector(this.Item1, this.Item2, this.Item3, this.Item4, this.Item5, this.Item6, this.Item7, this.Item8, this.Item9, this.Item10, this.Item11, this.Item12, this.Item13, this.Item14, this.Item15);
        }
    }

    public struct Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        : IEquatable<Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>>
    {
        public static Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> Default
        {
            get
            {
                return new Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>();
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

        public T5 Item5
        {
            get;
            set;
        }

        public T6 Item6
        {
            get;
            set;
        }

        public T7 Item7
        {
            get;
            set;
        }

        public T8 Item8
        {
            get;
            set;
        }

        public T9 Item9
        {
            get;
            set;
        }

        public T10 Item10
        {
            get;
            set;
        }

        public T11 Item11
        {
            get;
            set;
        }

        public T12 Item12
        {
            get;
            set;
        }

        public T13 Item13
        {
            get;
            set;
        }

        public T14 Item14
        {
            get;
            set;
        }

        public T15 Item15
        {
            get;
            set;
        }

        public T16 Item16
        {
            get;
            set;
        }

        public Struct(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8, T9 item9, T10 item10, T11 item11, T12 item12, T13 item13, T14 item14, T15 item15, T16 item16)
            : this()
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
            this.Item4 = item4;
            this.Item5 = item5;
            this.Item6 = item6;
            this.Item7 = item7;
            this.Item8 = item8;
            this.Item9 = item9;
            this.Item10 = item10;
            this.Item11 = item11;
            this.Item12 = item12;
            this.Item13 = item13;
            this.Item14 = item14;
            this.Item15 = item15;
            this.Item16 = item16;
        }

        public static Boolean operator ==(Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> self, Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> other)
        {
            return self.Equals(other);
        }

        public static Boolean operator !=(Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> self, Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> other)
        {
            return !self.Equals(other);
        }

        public Boolean Equals(Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> other)
        {
            return
                this.Item1.Equals(other.Item1) &&
                this.Item2.Equals(other.Item2) &&
                this.Item3.Equals(other.Item3) &&
                this.Item4.Equals(other.Item4) &&
                this.Item5.Equals(other.Item5) &&
                this.Item6.Equals(other.Item6) &&
                this.Item7.Equals(other.Item7) &&
                this.Item8.Equals(other.Item8) &&
                this.Item9.Equals(other.Item9) &&
                this.Item10.Equals(other.Item10) &&
                this.Item11.Equals(other.Item11) &&
                this.Item12.Equals(other.Item12) &&
                this.Item13.Equals(other.Item13) &&
                this.Item14.Equals(other.Item14) &&
                this.Item15.Equals(other.Item15) &&
                this.Item16.Equals(other.Item16);
        }
        public override Boolean Equals(Object obj)
        {
            return obj is Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> && this.Equals((Struct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>) obj);
        }

        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        public TResult Select<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> selector)
        {
            return selector(this.Item1, this.Item2, this.Item3, this.Item4, this.Item5, this.Item6, this.Item7, this.Item8, this.Item9, this.Item10, this.Item11, this.Item12, this.Item13, this.Item14, this.Item15, this.Item16);
        }
    }
}