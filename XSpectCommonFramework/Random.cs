// -*- mode: csharp; encoding: utf-8; -*-
/* XSpect Common Framework - Generic Utility Class Library
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@xspect.org>
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
 *
 * ACKNOWLEDGEMENT:
 * 
 * This is a port of Mersenne Twister (version mt19937ar, released in 2002).
 * 
 * Original code written in C is:
 *   http://www.math.sci.hiroshima-u.ac.jp/~m-mat/MT/MT2002/CODES/mt19937ar.c
 * Below is the copyright declaration of the code:
 *
 *   A C-program for MT19937, with initialization improved 2002/1/26.
 *   Coded by Takuji Nishimura and Makoto Matsumoto.
 *   
 *   Before using, initialize the state by using init_genrand(seed)  
 *   or init_by_array(init_key, key_length).
 *   
 *   Copyright (C) 1997 - 2002, Makoto Matsumoto and Takuji Nishimura,
 *   All rights reserved.                          
 *   
 *   Redistribution and use in source and binary forms, with or without
 *   modification, are permitted provided that the following conditions
 *   are met:
 *   
 *     1. Redistributions of source code must retain the above copyright
 *        notice, this list of conditions and the following disclaimer.
 * 
 *     2. Redistributions in binary form must reproduce the above copyright
 *        notice, this list of conditions and the following disclaimer in the
 *        documentation and/or other materials provided with the distribution.
 * 
 *     3. The names of its contributors may not be used to endorse or promote 
 *        products derived from this software without specific prior written 
 *        permission.
 *   
 *   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 *   "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 *   LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
 *   A PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE COPYRIGHT OWNER OR
 *   CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 *   EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 *   PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
 *   PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
 *   LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 *   NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 *   SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *   
 *   
 *   Any feedback is very welcome.
 *   http://www.math.sci.hiroshima-u.ac.jp/~m-mat/MT/emt.html
 *   email: m-mat @ math.sci.hiroshima-u.ac.jp (remove space)
 * 
 * And, this code is referred an another port of C# based on version MT19937
 * released on 1998. The code is:
 *   http://takel.jp/mt/MersenneTwister.cs
 * Below is the copyright declaration of the code:
 * 
 *   C# Version Copyright (C) 2001-2004 Akihilo Kramot (Takel).
 *   C# porting from a C-program for MT19937, originaly coded by
 *   Takuji Nishimura, considering the suggestions by
 *   Topher Cooper and Marc Rieffel in July-Aug. 1997.
 *   This library is free software under the Artistic license:
 *   
 *   You can find the original C-program at
 *       http://www.math.keio.ac.jp/~matumoto/mt.html
 * 
 * END OF ACKNOWLEDGEMENT.
 */

using System;
using System.Collections.Generic;

namespace XSpect
{
    public class Random
        : System.Random
    {
        private const Int32 N = 624;

        private const Int32 M = 397;

        /// <summary>
        /// Constant vector A.
        /// </summary>
        private const UInt32 MATRIX_A = 0x9908b0df;

        /// <summary>
        /// Most significant w-r bits.
        /// </summary>
        private const UInt32 UPPER_MASK = 0x80000000U;

        /// <summary>
        /// Least significant r bits.
        /// </summary>
        private const UInt32 LOWER_MASK = 0x7fffffffU;

        /// <summary>
        /// The array for the state vector.
        /// </summary>
        private UInt32[] _mt = new UInt32[N];

        /// <summary>
        /// mti==N+1 means mt[N] is not initialized.
        /// </summary>
        private Int16 _mti = N + 1;

        public Random()
            : this((UInt32) Environment.TickCount)
        {
        }

        /// <summary>
        /// Initializes mt[N] with a seed.
        /// </summary>
        /// <param name="seed">The seed value.</param>
        [CLSCompliant(false)]
        public Random(UInt32 seed)
        {
            this._mt[0] = seed & 0xffffffffU;

            // See Knuth TAOCP Vol2. 3rd Ed. P.106 for multiplier.
            // In the previous versions, MSBs of the seed affect
            // only MSBs of the array mt[].
            // 2002/01/09 modified by Makoto Matsumoto
            for (this._mti = 1; this._mti < N; ++this._mti)
            {
                // The algorithm looks to overflow in below expression.
                unchecked
                {
                    this._mt[this._mti]
                        = 1812433253U * (this._mt[this._mti - 1] ^ (this._mt[this._mti - 1] >> 30)) + (UInt32) this._mti;
                }
                // for >32 bit machines
                this._mt[this._mti] &= 0xffffffffU;
            }
        }

        [CLSCompliant(false)]
        public Random(UInt32[] initKey)
            : this(initKey, initKey.Length)
        {
        }

        /// <summary>
        /// Initialize by an array with array-length.
        /// </summary>
        /// <param name="initKey">The array for initializing keys.</param>
        /// <param name="length">The length of initKey.</param>
        [CLSCompliant(false)]
        public Random(UInt32[] initKey, Int32 length)
            : this(19650218U)
        {
            Int16 i = 1;
            Int16 j = 0;
            Int16 k = (Int16) (N > length ? N : length);

            for (; k != 0; k--)
            {
                unchecked
                {
                    // non linear
                    this._mt[i] =
                        (this._mt[i] ^ ((this._mt[i - 1] ^ (this._mt[i - 1] >> 30)) * 1664525U)) + (UInt32) (initKey[j] + j);
                }
                // for WORDSIZE > 32 machines
                this._mt[i] &= 0xffffffffU;
                i++;
                j++;
                if (i >= N)
                {
                    this._mt[0] = this._mt[N - 1];
                    i = 1;
                }
                if (j >= length)
                {
                    j = 0;
                }
            }

            for (k = N - 1; k != 0; k--)
            {
                unchecked
                {
                    this._mt[i] =
                        (this._mt[i] ^ ((this._mt[i - 1] ^ (this._mt[i - 1] >> 30)) * 1566083941U)) - (UInt32) i;
                }
                // for WORDSIZE > 32 machines
                this._mt[i] &= 0xffffffffU;
                i++;
                if (i >= N)
                {
                    this._mt[0] = this._mt[N - 1];
                    i = 1;
                }
            }

            /* MSB is 1; assuring non-zero initial array */
            this._mt[0] = 0x80000000U;
        }

        /// <summary>
        /// Generates a random number on [0,0xffffffff]-interval
        /// </summary>
        /// <returns>Generated value.</returns>
        [CLSCompliant(false)]
        protected UInt32 GenerateUInt32()
        {
            UInt32 y;
            /* mag01[x] = x * MATRIX_A  for x=0,1 */
            UInt32[] mag01 = new UInt32[] { 0x0U, MATRIX_A, };

            if (this._mti >= N)
            {
                /* generate N words at one time */
                Int32 kk = 0;

                for (; kk < N - M; ++kk)
                {
                    y = (this._mt[kk] & UPPER_MASK) | (this._mt[kk + 1] & LOWER_MASK);
                    this._mt[kk] = this._mt[kk + M] ^ (y >> 1) ^ mag01[y & 0x1U];
                }
                for (; kk < N - 1; ++kk)
                {
                    y = (this._mt[kk] & UPPER_MASK) | (this._mt[kk + 1] & LOWER_MASK);
                    this._mt[kk] = this._mt[kk + (M - N)] ^ (y >> 1) ^ mag01[y & 0x1U];
                }
                y = (this._mt[N - 1] & UPPER_MASK) | (this._mt[0] & LOWER_MASK);
                this._mt[N - 1] = this._mt[M - 1] ^ (y >> 1) ^ mag01[y & 0x1U];
                this._mti = 0;
            }

            // Tempering
            y = _mt[_mti++];
            y ^= (y >> 11);
            y ^= (y << 7) & 0x9d2c5680U;
            y ^= (y << 15) & 0xefc60000U;
            y ^= (y >> 18);

            return y;
        }

        protected override Double Sample()
        {
            return this.NextDouble(false, true, false);
        }

        #region Next
        public override Int32 Next()
        {
            return this.NextInt32();
        }

        public override Int32 Next(Int32 maxValue)
        {
            return this.NextInt32(maxValue);
        }

        public override Int32 Next(Int32 minValue, Int32 maxValue)
        {
            return this.NextInt32(minValue, maxValue);
        }
        #endregion

        #region NextByte
        public Byte NextByte()
        {
            return this.NextByte(Byte.MaxValue);
        }

        public Byte NextByte(Byte maxValue)
        {
            if (maxValue <= 1)
            {
                if (maxValue < 0)
                {
                    throw new ArgumentOutOfRangeException("maxValue");
                }
                return 0;
            }

            return (Byte) (this.NextDouble() * maxValue);
        }

        public Byte NextByte(Byte minValue, Byte maxValue)
        {
            if (maxValue < minValue)
            {
                throw new ArgumentOutOfRangeException("minValue");
            }
            else if (minValue == maxValue)
            {
                return minValue;
            }
            else
            {
                return (Byte) (this.NextDouble(false, true, false) * ((Double) maxValue - minValue) + minValue);
            }
        }

        public override void NextBytes(Byte[] buffer)
        {
            this.NextBytes(buffer, buffer.Length);
        }

        public virtual void NextBytes(Byte[] buffer, Int32 length)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            for (Int32 idx = 0; idx < length; ++idx)
            {
                buffer[idx] = this.NextByte();
            }
        }

        public IEnumerable<Byte> NextBytes()
        {
            while (true)
            {
                yield return this.NextByte();
            }
        }

        public IEnumerable<Byte> NextBytes(Byte maxValue)
        {
            while (true)
            {
                yield return this.NextByte(maxValue);
            }
        }

        public IEnumerable<Byte> NextBytes(Byte minValue, Byte maxValue)
        {
            while (true)
            {
                yield return this.NextByte(minValue, maxValue);
            }
        }
        #endregion

        #region NextSByte
        [CLSCompliant(false)]
        public SByte NextSByte()
        {
            return this.NextSByte(SByte.MaxValue);
        }

        [CLSCompliant(false)]
        public SByte NextSByte(SByte maxValue)
        {
            if (maxValue <= 1)
            {
                if (maxValue < 0)
                {
                    throw new ArgumentOutOfRangeException("maxValue");
                }
                return 0;
            }

            return (SByte) (this.NextDouble() * maxValue);
        }

        [CLSCompliant(false)]
        public SByte NextSByte(SByte minValue, SByte maxValue)
        {
            if (maxValue < minValue)
            {
                throw new ArgumentOutOfRangeException("minValue");
            }
            else if (minValue == maxValue)
            {
                return minValue;
            }
            else
            {
                return (SByte) (this.NextDouble(false, true, false) * ((Double) maxValue - minValue) + minValue);
            }
        }

        [CLSCompliant(false)]
        public void NextSBytes(SByte[] buffer)
        {
            this.NextSBytes(buffer, buffer.Length);
        }

        [CLSCompliant(false)]
        public virtual void NextSBytes(SByte[] buffer, Int32 length)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            for (Int32 idx = 0; idx < length; ++idx)
            {
                buffer[idx] = this.NextSByte(SByte.MinValue, SByte.MaxValue);
            }
        }

        [CLSCompliant(false)]
        public IEnumerable<SByte> NextSBytes()
        {
            while (true)
            {
                yield return this.NextSByte();
            }
        }

        [CLSCompliant(false)]
        public IEnumerable<SByte> NextSBytes(SByte maxValue)
        {
            while (true)
            {
                yield return this.NextSByte(maxValue);
            }
        }

        [CLSCompliant(false)]
        public IEnumerable<SByte> NextSBytes(SByte minValue, SByte maxValue)
        {
            while (true)
            {
                yield return this.NextSByte(minValue, maxValue);
            }
        }
        #endregion

        #region NextChar
        public Char NextChar()
        {
            return this.NextChar(Char.MaxValue);
        }

        public Char NextChar(Char maxValue)
        {
            if (maxValue <= 1)
            {
                if (maxValue < 0)
                {
                    throw new ArgumentOutOfRangeException("maxValue");
                }
                return (Char) 0;
            }

            return (Char) (this.NextDouble() * maxValue);
        }

        public Char NextChar(Char minValue, Char maxValue)
        {
            if (maxValue < minValue)
            {
                throw new ArgumentOutOfRangeException("minValue");
            }
            else if (minValue == maxValue)
            {
                return minValue;
            }
            else
            {
                return (Char) (this.NextDouble(false, true, false) * ((Double) maxValue - minValue) + minValue);
            }
        }

        public void NextChars(Char[] buffer)
        {
            this.NextChars(buffer, buffer.Length);
        }

        public virtual void NextChars(Char[] buffer, Int32 length)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            for (Int32 idx = 0; idx < length; ++idx)
            {
                buffer[idx] = this.NextChar();
            }
        }

        public IEnumerable<Char> NextChars()
        {
            while (true)
            {
                yield return this.NextChar();
            }
        }

        public IEnumerable<Char> NextChars(Char maxValue)
        {
            while (true)
            {
                yield return this.NextChar(maxValue);
            }
        }

        public IEnumerable<Char> NextChars(Char minValue, Char maxValue)
        {
            while (true)
            {
                yield return this.NextChar(minValue, maxValue);
            }
        }
        #endregion

        #region NextInt16
        public Int16 NextInt16()
        {
            return this.NextInt16(Int16.MaxValue);
        }

        public Int16 NextInt16(Int16 maxValue)
        {
            if (maxValue <= 1)
            {
                if (maxValue < 0)
                {
                    throw new ArgumentOutOfRangeException("maxValue");
                }
                return 0;
            }

            return (Int16) (this.NextDouble() * maxValue);
        }

        public Int16 NextInt16(Int16 minValue, Int16 maxValue)
        {
            if (maxValue < minValue)
            {
                throw new ArgumentOutOfRangeException("minValue");
            }
            else if (minValue == maxValue)
            {
                return minValue;
            }
            else
            {
                return (Int16) (this.NextDouble(false, true, false) * ((Double) maxValue - minValue) + minValue);
            }
        }

        public IEnumerable<Int16> NextInt16s()
        {
            while (true)
            {
                yield return this.NextInt16();
            }
        }

        public IEnumerable<Int16> NextInt16s(Int16 maxValue)
        {
            while (true)
            {
                yield return this.NextInt16(maxValue);
            }
        }

        public IEnumerable<Int16> NextInt16s(Int16 minValue, Int16 maxValue)
        {
            while (true)
            {
                yield return this.NextInt16(minValue, maxValue);
            }
        }
        #endregion

        #region NextUInt16
        [CLSCompliant(false)]
        public UInt16 NextUInt16()
        {
            return this.NextUInt16(UInt16.MaxValue);
        }

        [CLSCompliant(false)]
        public UInt16 NextUInt16(UInt16 maxValue)
        {
            if (maxValue <= 1)
            {
                if (maxValue < 0)
                {
                    throw new ArgumentOutOfRangeException("maxValue");
                }
                return 0;
            }

            return (UInt16) (this.NextDouble() * maxValue);
        }

        [CLSCompliant(false)]
        public UInt16 NextUInt16(UInt16 minValue, UInt16 maxValue)
        {
            if (maxValue < minValue)
            {
                throw new ArgumentOutOfRangeException("minValue");
            }
            else if (minValue == maxValue)
            {
                return minValue;
            }
            else
            {
                return (UInt16) (this.NextDouble(false, true, false) * ((Double) maxValue - minValue) + minValue);
            }
        }

        [CLSCompliant(false)]
        public IEnumerable<UInt16> NextUInt16s()
        {
            while (true)
            {
                yield return this.NextUInt16();
            }
        }

        [CLSCompliant(false)]
        public IEnumerable<UInt16> NextUInt16s(UInt16 maxValue)
        {
            while (true)
            {
                yield return this.NextUInt16(maxValue);
            }
        }

        [CLSCompliant(false)]
        public IEnumerable<UInt16> NextUInt16s(UInt16 minValue, UInt16 maxValue)
        {
            while (true)
            {
                yield return this.NextUInt16(minValue, maxValue);
            }
        }
        #endregion

        #region NextInt32
        public Int32 NextInt32()
        {
            return this.NextInt32(Int32.MaxValue);
        }

        public Int32 NextInt32(Int32 maxValue)
        {
            if (maxValue <= 1)
            {
                if (maxValue < 0)
                {
                    throw new ArgumentOutOfRangeException("maxValue");
                }
                return 0;
            }

            return (Int32) (this.NextDouble() * maxValue);
        }

        public Int32 NextInt32(Int32 minValue, Int32 maxValue)
        {
            if (maxValue < minValue)
            {
                throw new ArgumentOutOfRangeException("minValue");
            }
            else if (minValue == maxValue)
            {
                return minValue;
            }
            else
            {
                return (Int32) (this.NextDouble(false, true, false) * ((Double) maxValue - minValue) + minValue);
            }
        }

        public IEnumerable<Int32> NextInt32s()
        {
            while (true)
            {
                yield return this.NextInt32();
            }
        }

        public IEnumerable<Int32> NextInt32s(Int32 maxValue)
        {
            while (true)
            {
                yield return this.NextInt32(maxValue);
            }
        }

        public IEnumerable<Int32> NextInt32s(Int32 minValue, Int32 maxValue)
        {
            while (true)
            {
                yield return this.NextInt32(minValue, maxValue);
            }
        }
        #endregion

        #region NextUInt32
        [CLSCompliant(false)]
        public UInt32 NextUInt32()
        {
            return this.NextUInt32(UInt32.MaxValue);
        }

        [CLSCompliant(false)]
        public UInt32 NextUInt32(UInt32 maxValue)
        {
            if (maxValue <= 1)
            {
                if (maxValue < 0)
                {
                    throw new ArgumentOutOfRangeException("maxValue");
                }
                return 0;
            }

            return (UInt32) (this.NextDouble() * maxValue);
        }

        [CLSCompliant(false)]
        public UInt32 NextUInt32(UInt32 minValue, UInt32 maxValue)
        {
            if (maxValue < minValue)
            {
                throw new ArgumentOutOfRangeException("minValue");
            }
            else if (minValue == maxValue)
            {
                return minValue;
            }
            else
            {
                return (UInt32) (this.NextDouble(false, true, false) * ((Double) maxValue - minValue) + minValue);
            }
        }

        [CLSCompliant(false)]
        public IEnumerable<UInt32> NextUInt32s()
        {
            while (true)
            {
                yield return this.NextUInt32();
            }
        }

        [CLSCompliant(false)]
        public IEnumerable<UInt32> NextUInt32s(UInt32 maxValue)
        {
            while (true)
            {
                yield return this.NextUInt32(maxValue);
            }
        }

        [CLSCompliant(false)]
        public IEnumerable<UInt32> NextUInt32s(UInt32 minValue, UInt32 maxValue)
        {
            while (true)
            {
                yield return this.NextUInt32(minValue, maxValue);
            }
        }
        #endregion

        #region NextInt64
        public Int64 NextInt64()
        {
            return this.NextInt64(Int64.MaxValue);
        }

        public Int64 NextInt64(Int64 maxValue)
        {
            if (maxValue <= 1)
            {
                if (maxValue < 0)
                {
                    throw new ArgumentOutOfRangeException("maxValue");
                }
                return 0;
            }

            return (Int64) (this.NextDouble() * maxValue);
        }

        public Int64 NextInt64(Int64 minValue, Int64 maxValue)
        {
            if (maxValue < minValue)
            {
                throw new ArgumentOutOfRangeException("minValue");
            }
            else if (minValue == maxValue)
            {
                return minValue;
            }
            else
            {
                return (Int64) (this.NextDouble(false, true, false) * ((Double) maxValue - minValue) + minValue);
            }
        }

        public IEnumerable<Int64> NextInt64s()
        {
            while (true)
            {
                yield return this.NextInt64();
            }
        }

        public IEnumerable<Int64> NextInt64s(Int64 maxValue)
        {
            while (true)
            {
                yield return this.NextInt64(maxValue);
            }
        }

        public IEnumerable<Int64> NextInt64s(Int64 minValue, Int64 maxValue)
        {
            while (true)
            {
                yield return this.NextInt64(minValue, maxValue);
            }
        }
        #endregion

        #region NextUInt64
        [CLSCompliant(false)]
        public UInt64 NextUInt64()
        {
            return this.NextUInt64(UInt64.MaxValue);
        }

        [CLSCompliant(false)]
        public UInt64 NextUInt64(UInt64 maxValue)
        {
            if (maxValue <= 1)
            {
                if (maxValue < 0)
                {
                    throw new ArgumentOutOfRangeException("maxValue");
                }
                return 0;
            }

            return (UInt64) (this.NextDouble() * maxValue);
        }

        [CLSCompliant(false)]
        public UInt64 NextUInt64(UInt64 minValue, UInt64 maxValue)
        {
            if (maxValue < minValue)
            {
                throw new ArgumentOutOfRangeException("minValue");
            }
            else if (minValue == maxValue)
            {
                return minValue;
            }
            else
            {
                return (UInt64) (this.NextDouble(false, true, false) * ((Double) maxValue - minValue) + minValue);
            }
        }

        [CLSCompliant(false)]
        public IEnumerable<UInt64> NextUInt64s()
        {
            while (true)
            {
                yield return this.NextUInt64();
            }
        }

        [CLSCompliant(false)]
        public IEnumerable<UInt64> NextUInt64s(UInt64 maxValue)
        {
            while (true)
            {
                yield return this.NextUInt64(maxValue);
            }
        }

        [CLSCompliant(false)]
        public IEnumerable<UInt64> NextUInt64s(UInt64 minValue, UInt64 maxValue)
        {
            while (true)
            {
                yield return this.NextUInt64(minValue, maxValue);
            }
        }
        #endregion

        #region NextDecimal
        public Decimal NextDecimal()
        {
            return this.NextDecimal(Decimal.MaxValue);
        }

        public Decimal NextDecimal(Decimal maxValue)
        {
            if (maxValue <= 1)
            {
                if (maxValue < 0)
                {
                    throw new ArgumentOutOfRangeException("maxValue");
                }
                return 0;
            }

            return (Decimal) ((Decimal) this.NextDouble() * maxValue);
        }

        public Decimal NextDecimal(Decimal minValue, Decimal maxValue)
        {
            if (maxValue < minValue)
            {
                throw new ArgumentOutOfRangeException("minValue");
            }
            else if (minValue == maxValue)
            {
                return minValue;
            }
            else
            {
                return (Decimal) ((Decimal) this.NextDouble(false, true, false) * (maxValue - minValue) + minValue);
            }
        }

        public IEnumerable<Decimal> NextDecimals()
        {
            while (true)
            {
                yield return this.NextDecimal();
            }
        }

        public IEnumerable<Decimal> NextDecimals(Decimal maxValue)
        {
            while (true)
            {
                yield return this.NextDecimal(maxValue);
            }
        }

        public IEnumerable<Decimal> NextDecimals(Decimal minValue, Decimal maxValue)
        {
            while (true)
            {
                yield return this.NextDecimal(minValue, maxValue);
            }
        }
        #endregion

        #region NextSingle
        public Single NextSingle()
        {
            return (Single) this.Sample();
        }

        public virtual Single NextSingle(Boolean isSigned, Boolean isMinClosed, Boolean isMaxClosed)
        {
            return (Single) this.NextDouble(isSigned, isMinClosed, isMaxClosed);
        }
        #endregion

        #region NextDouble
        public override Double NextDouble()
        {
            return this.Sample();
        }

        public virtual Double NextDouble(Boolean isSigned, Boolean isMinClosed, Boolean isMaxClosed)
        {
            UInt64 n = this.GenerateUInt32();
            UInt64 m = isSigned ? (UInt64) Int32.MaxValue : (UInt64) UInt32.MaxValue;
            if (!isMinClosed)
            {
                n++;
            }
            if (!isMaxClosed)
            {
                m++;
            }

            unchecked
            {
                if (isSigned)
                {
                    return (Double) ((Int32) n) / m;
                }
                else
                {
                    return (Double) n / m;
                }
            }
        }
        #endregion
    }
}
