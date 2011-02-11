// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetObjectModel
 *   Object model and Storage interface for MetaTweet and other systems
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetObjectModel.
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
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace XSpect.MetaTweet.Objects
{
    [Serializable()]
    [DataContract()]
    [JsonConverter(typeof(StorageObjectIdConverter))]
    public struct AccountId
        : IStorageObjectId<Account>,
          IComparable<AccountId>,
          IEquatable<AccountId>
    {
        public const Int32 ByteLength = 20;

        public const Int32 HexStringLength = ByteLength * 2;

        private static readonly SHA1 _hash = SHA1CryptoServiceProvider.Create();

        private readonly Byte[] _value;

        public StorageObjectTypes ObjectType
        {
            get
            {
                return StorageObjectTypes.Account;
            }
        }

        [DataMember()]
        public Byte[] Value
        {
            get
            {
                return this._value ?? new Byte[ByteLength];
            }
        }

        public String HexString
        {
            get
            {
                return String.Join("", Array.ConvertAll(this.Value, b => b.ToString("x2")));
            }
        }

        public String Base64String
        {
            get
            {
                return Convert.ToBase64String(this.Value, Base64FormattingOptions.None);
            }
        }

        public static Boolean operator ==(AccountId left, AccountId right)
        {
            return left.Equals(right);
        }

        public static Boolean operator !=(AccountId left, AccountId right)
        {
            return !left.Equals(right);
        }

        public static Boolean operator <(AccountId left, AccountId right)
        {
            return left.CompareTo(right) < 0;
        }

        public static Boolean operator <=(AccountId left, AccountId right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static Boolean operator >(AccountId left, AccountId right)
        {
            return left.CompareTo(right) > 0;
        }

        public static Boolean operator >=(AccountId left, AccountId right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static implicit operator AccountId(String hexString)
        {
            return new AccountId(hexString);
        }

        public static implicit operator String(AccountId id)
        {
            return id.HexString;
        }

        public AccountId(Byte[] id)
            : this()
        {
            if (id.Length != ByteLength)
            {
                throw new ArgumentException("id");
            }
            this._value = id;
        }

        public AccountId(String hexString)
            : this()
        {
            if (hexString.Length != HexStringLength)
            {
                throw new ArgumentException("hexString");
            }
            this._value = hexString
                .Zip(hexString.Skip(1), (x, y) => new String(new Char[] { x, y }))
                .Where((_, i) => i % 2 == 0)
                .Select(_ => Convert.ToByte(_, 16))
                .ToArray();
        }

        public static AccountId Create(String realm, IDictionary<String, String> seeds)
        {
            return Create(realm, Account.GetSeed(seeds));
        }

        public static AccountId Create(String realm, String seed)
        {
            return new AccountId(_hash.ComputeHash(Encoding.UTF32.GetBytes(seed + "@" + realm)));
        }

        public override Boolean Equals(Object obj)
        {
            return obj is AccountId && this.Equals((AccountId) obj);
        }

        public override Int32 GetHashCode()
        {
            return this.Base64String.GetHashCode();
        }

        public override String ToString()
        {
            return this.HexString;
        }

        public String ToString(Int32 head, Int32 tail)
        {
            return this.HexString.Substring(0, head) + ".." + this.HexString.Substring(HexStringLength - tail);
        }

        public String ToString(Boolean shorten)
        {
            return shorten ? this.ToString(4, 4) : this.ToString();
        }

        public Int32 CompareTo(AccountId other)
        {
            return this.HexString.CompareTo(other.HexString);
        }

        public Boolean Equals(IStorageObjectId other)
        {
            return other is AccountId && this.Equals((AccountId) other);
        }

        public Boolean Equals(AccountId other)
        {
            return this.Value.SequenceEqual(other.Value);
        }
    }
}