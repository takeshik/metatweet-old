// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetObjectModel
 *   Object model and Storage interface for MetaTweet and other systems
 *   Part of MetaTweet
 * Copyright c 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
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
using System.Runtime.Serialization;

namespace XSpect.MetaTweet.Objects
{
    partial class Annotation
        : IAnnotation,
          IComparable<Annotation>,
          IEquatable<Annotation>
    {
        /// <summary>
        /// <see cref="Annotation"/> の新しいインスタンスを初期化します。
        /// </summary>
        private Annotation()
        {
        }

        /// <summary>
        /// <see cref="Annotation"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="storage">オブジェクトが追加されるストレージ。</param>
        internal Annotation(Storage storage)
            : base(storage)
        {
        }

        protected Annotation(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.Account = (Account) info.GetValue("Account", typeof(Account));
            this.Name = (String) info.GetValue("Name", typeof(String));
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override Boolean Equals(Object obj)
        {
            return
                !ReferenceEquals(null, obj) &&
                ReferenceEquals(this, obj) ||
                ((obj is IAnnotation) && this.Equals(obj as IAnnotation));
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override Int32 GetHashCode()
        {
            return unchecked(
                this._AccountId.GetHashCode() * 397 ^
                (this._Name != null ? this._Name.GetHashCode() : 0)
            );
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override String ToString()
        {
            return String.Format(
                "Ann [{0}]: {1}",
                this.Account,
                this.Name
            );
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
        /// Value
        /// Meaning
        /// Less than zero
        /// This object is less than the <paramref name="other"/> parameter.
        /// Zero
        /// This object is equal to <paramref name="other"/>.
        /// Greater than zero
        /// This object is greater than <paramref name="other"/>.
        /// </returns>
        public override Int32 CompareTo(StorageObject other)
        {
            if (!(other is Annotation))
            {
                throw new ArgumentException("other");
            }
            return this.CompareTo(other as Annotation);
        }

        /// <summary>
        /// 指定したストレージ オブジェクトが、このストレージ オブジェクトと完全に等しいかどうかを判断します。
        /// </summary>
        /// <param name="other">このストレージ オブジェクトと比較するストレージ オブジェクト。</param>
        /// <returns>
        /// 指定したストレージ オブジェクトがこのストレージ オブジェクトと完全に等しい場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        public override Boolean EqualsExact(StorageObject other)
        {
            return other is Annotation
                ? this.EqualsExact(other as Annotation)
                : other is IAnnotation
                      ? this.EqualsExact(other as IAnnotation)
                      : false;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Account", this.Account);
            info.AddValue("Name", this.Name);
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
        /// Value
        /// Meaning
        /// Less than zero
        /// This object is less than the <paramref name="other"/> parameter.
        /// Zero
        /// This object is equal to <paramref name="other"/>.
        /// Greater than zero
        /// This object is greater than <paramref name="other"/>.
        /// </returns>
        public Int32 CompareTo(IAnnotation other)
        {
            // Account -> Name
            Int32 result;
            return other == null
                ? 1
                : (result = this.Account.CompareTo(other.Account)) != 0
                      ? result
                      : this.Name.CompareTo(other.Name);
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
        /// Value
        /// Meaning
        /// Less than zero
        /// This object is less than the <paramref name="other"/> parameter.
        /// Zero
        /// This object is equal to <paramref name="other"/>.
        /// Greater than zero
        /// This object is greater than <paramref name="other"/>.
        /// </returns>
        public Int32 CompareTo(Annotation other)
        {
            return this.CompareTo(other as IAnnotation);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public Boolean Equals(IAnnotation other)
        {
            return !ReferenceEquals(other, null) && (
                ReferenceEquals(this, other)
                || this.Account.Equals(other.Account)
                && this.Name.Equals(other.Name)
            );
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public Boolean Equals(Annotation other)
        {
            return this.Equals(other as IAnnotation);
        }

        /// <summary>
        /// 指定したアノテーションが、このアノテーションと完全に等しいかどうかを判断します。
        /// </summary>
        /// <param name="other">このアノテーションと比較するアノテーション。</param>
        /// <returns>
        /// 指定したアノテーションがこのアノテーションと完全に等しい場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        public Boolean EqualsExact(IAnnotation other)
        {
            return !ReferenceEquals(other, null) && (
                ReferenceEquals(this, other)
                || this.Account.EqualsExact(other.Account)
                && this.Name.Equals(other.Name)
            );
        }

        /// <summary>
        /// 指定したアノテーションが、所属するストレージを含め、このアノテーションと完全に等しいかどうかを判断します。
        /// </summary>
        /// <param name="other">このアノテーションと比較するアノテーション。</param>
        /// <returns>
        /// 指定したアノテーションが、所属するストレージを含め、このアノテーションと完全に等しい場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        public Boolean EqualsExact(Annotation other)
        {
            return this.Storage == other.Storage
                && this.EqualsExact(other as IAnnotation);
        }

        #region Implicit Implementations

        /// <summary>
        /// このアノテーションが関連付けられているアカウントを取得または設定します。
        /// </summary>
        /// <value>このアノテーションが関連付けられているアカウント。</value>
        IAccount IAnnotation.Account
        {
            get
            {
                return this.Account;
            }
            set
            {
                this.Account = (Account) value;
            }
        }

        #endregion

    }
}