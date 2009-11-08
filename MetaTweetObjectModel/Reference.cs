// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetObjectModel
 *   Object model and Storage interface for MetaTweet and other systems
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
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
using System.Linq;

namespace XSpect.MetaTweet.Objects
{
    partial class Reference
        : IReference,
          IComparable<Reference>,
          IEquatable<Reference>
    {
        /// <summary>
        /// <see cref="Reference"/> の新しいインスタンスを初期化します。
        /// </summary>
        private Reference()
        {
        }

        /// <summary>
        /// <see cref="Reference"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="storage">オブジェクトが追加されるストレージ。</param>
        internal Reference(Storage storage)
            : base(storage)
        {
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
                ((obj is IReference) && this.Equals(obj as IReference));
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
                this._Timestamp.GetHashCode() * 397 ^
                (this._Category != null ? this._Category.GetHashCode() * 397 : 0) ^
                (this._SubId != null ? this._SubId.GetHashCode() * 397 : 0) ^
                (this._Name != null ? this._Name.GetHashCode() * 397 : 0) ^
                this._ReferringAccountId.GetHashCode() * 397 ^
                this._ReferringTimestamp.GetHashCode() * 397 ^
                (this._ReferringCategory != null ? this._ReferringCategory.GetHashCode() * 397 : 0) ^
                (this._ReferringSubId != null ? this._ReferringSubId.GetHashCode() : 0)
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
                "Ref [{0}]: {1} -> [{2}]",
                this.Activity,
                this.Name,
                this.ReferringActivity
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
            if (!(other is Reference))
            {
                throw new ArgumentException("other");
            }
            return this.CompareTo(other as Reference);
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
            return other is Reference
                ? this.EqualsExact(other as Reference)
                : other is IReference
                      ? this.EqualsExact(other as IReference)
                      : false;
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
        public Int32 CompareTo(IReference other)
        {
            // Activity -> Name -> ReferringActivity
            Int32 result;
            return other == null
                ? 1
                : (result = this.Activity.CompareTo(other.Activity)) != 0
                      ? result
                      : (result = this.Name.CompareTo(other.Name)) != 0
                            ? result
                            : this.ReferringActivity.CompareTo(other.ReferringActivity);
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
        public Int32 CompareTo(Reference other)
        {
            return this.CompareTo(other as IReference);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public Boolean Equals(IReference other)
        {
            return !ReferenceEquals(other, null) && (
                ReferenceEquals(this, other)
                || this.Activity.Equals(other.Activity)
                && this.Name.Equals(other.Name)
                && this.ReferringActivity.Equals(other.ReferringActivity)
            );
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public Boolean Equals(Reference other)
        {
            return this.Equals(other as IReference);
        }

        /// <summary>
        /// 指定したリファレンスが、このリファレンスと完全に等しいかどうかを判断します。
        /// </summary>
        /// <param name="other">このリファレンスと比較するリファレンス。</param>
        /// <returns>
        /// 指定したリファレンスがこのリファレンスと完全に等しい場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        public Boolean EqualsExact(IReference other)
        {
            return !ReferenceEquals(other, null) && (
                ReferenceEquals(this, other)
                || this.Activity.EqualsExact(other.Activity)
                && this.Name.Equals(other.Name)
                && this.ReferringActivity.EqualsExact(other.ReferringActivity)
            );
        }

        /// <summary>
        /// 指定したリファレンスが、所属するストレージを含め、このリファレンスと完全に等しいかどうかを判断します。
        /// </summary>
        /// <param name="other">このリファレンスと比較するリファレンス。</param>
        /// <returns>
        /// 指定したリファレンスが、所属するストレージを含め、このリファレンスと完全に等しい場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        public Boolean EqualsExact(Reference other)
        {
            return this.Storage == other.Storage
                && this.EqualsExact(other as IReference);
        }

        #region Implicit Implementations

        /// <summary>
        /// このリファレンスが関連付けられているアクティビティを取得または設定します。
        /// </summary>
        /// <value>このリファレンスが関連付けられているアクティビティ。</value>
        IActivity IReference.Activity
        {
            get
            {
                return this.Activity;
            }
            set
            {
                this.Activity = (Activity) value;
            }
        }

        /// <summary>
        /// このリファレンスが関連付けられる先のアクティビティを取得または設定します。
        /// </summary>
        /// <value>このリファレンスが関連付けられる先のアクティビティ。</value>
        IActivity IReference.ReferringActivity
        {
            get
            {
                return this.ReferringActivity;
            }
            set
            {
                this.ReferringActivity = (Activity) value;
            }
        }

        #endregion

        // NOTE: Alternative implementation.
        /// <summary>
        /// このリファレンスが関連付けられる先のアクティビティを取得または設定します。
        /// </summary>
        /// <value>このリファレンスが関連付けられる先のアクティビティ。</value>
        public Activity ReferringActivity
        {
            get
            {
                return this.Storage.GetActivities(this.ReferringAccountId, this.ReferringTimestamp, this.ReferringCategory, this.ReferringSubId).FirstOrDefault();
            }
            set
            {
                this.ReferringAccountId = value.AccountId;
                this.ReferringTimestamp = value.Timestamp;
                this.ReferringCategory = value.Category;
                this.ReferringSubId = value.SubId;
            }
        }
    }
}