// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetObjectModel
 *   Object model and Storage interface for MetaTweet and other systems
 *   Part of MetaTweet
 * Copyright © 2008-2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
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
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;

namespace XSpect.MetaTweet.Objects
{
    [KnownType(typeof(Account))]
    partial class Relation
        : IRelation,
          IComparable<Relation>,
          IEquatable<Relation>
    {
        /// <summary>
        /// オブジェクトの種類を取得します。
        /// </summary>
        /// <value>オブジェクトの種類を表す <see cref="StorageObjectTypes"/> 値。</value>
        public override StorageObjectTypes ObjectType
        {
            get
            {
                return StorageObjectTypes.Relation;
            }
        }

        /// <summary>
        /// <see cref="System.Data.Objects.ObjectContext"/> を取得するために使用するオブジェクトを取得します。
        /// </summary>
        /// <value><see cref="System.Data.Objects.ObjectContext"/> を取得するために使用するオブジェクト。</value>
        protected override RelatedEnd ContextHolder
        {
            get
            {
                return this.AccountReference;
            }
        }

        /// <summary>
        /// <see cref="Relation"/> の新しいインスタンスを初期化します。
        /// </summary>
        public Relation()
            : this(null)
        {
        }

        /// <summary>
        /// <see cref="Relation"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="storage">The storage.</param>
        internal Relation(Storage storage)
            : base(storage)
        {
        }

        /// <summary>
        /// シリアル化したデータを使用して、<see cref="Relation"/> クラスの新しいインスタンスを初期化します。 
        /// </summary>
        /// <param name="info">シリアル化済みオブジェクト データを保持している <see cref="SerializationInfo"/>。</param>
        /// <param name="context">転送元または転送先に関するコンテキスト情報を含んでいる <see cref="StreamingContext"/>。</param>
        protected Relation(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.Account = (Account) info.GetValue("Account", typeof(Account));
            this.AccountId = this.Account.AccountId;
            this.Name = (String) info.GetValue("Name", typeof(String));
            this.RelatingAccount = (Account) info.GetValue("RelatingAccount", typeof(Account));
            // this.RelatingAccountId must be set at set_RelatingAccount.
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
                ((obj is IRelation) && this.Equals((IRelation) obj));
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
                (this._AccountId != null ? this._AccountId.GetHashCode() * 397 : 0) ^
                (this._Name != null ? this._Name.GetHashCode() * 397 : 0) ^
                (this._RelatingAccountId != null ? this._RelatingAccountId.GetHashCode() : 0)
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
                "Rel [{0}]: {1} -> [{2}]",
                this.Account,
                this.Name,
                this.RelatingAccount
            );
        }

        /// <summary>
        /// このオブジェクトの完全な内容を表す <see cref="String"/> を返します。
        /// </summary>
        /// <returns>このオブジェクトの完全な内容を表す <see cref="String"/>。</returns>
        public override String Describe()
        {
            return String.Format(
                "Rel [{0}]: {1} -> [{2}]",
                this.Account.Describe(),
                this.Name,
                this.RelatingAccount.Describe()
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
            if (!(other is Relation))
            {
                throw new ArgumentException("other");
            }
            return this.CompareTo((Relation) other);
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
            return other is Relation
                ? this.EqualsExact((Relation) other)
                : other is IRelation
                      ? this.EqualsExact((IRelation) other)
                      : false;
        }

        /// <summary>
        /// オブジェクトをシリアル化するために必要なデータをシリアル化情報オブジェクトに設定します。
        /// </summary>
        /// <param name="info">オブジェクトと関連付けられているシリアル化データを保持する <see cref="SerializationInfo"/>。</param>
        /// <param name="context">オブジェクトに関連付けられているシリアル化ストリームのソースおよびデスティネーションを格納している <see cref="StreamingContext"/>。</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Account", this.Account, typeof(Account));
            info.AddValue("Name", this.Name, typeof(String));
            info.AddValue("RelatingAccount", this.RelatingAccount, typeof(Account));
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
        public Int32 CompareTo(IRelation other)
        {
            // Account -> Name -> RelatingAccount
            Int32 result;
            return other == null
                ? 1
                : (result = this.Account.CompareTo(other.Account)) != 0
                      ? result
                      : (result = this.Name.CompareTo(other.Name)) != 0
                            ? result
                            : this.RelatingAccount.CompareTo(other.RelatingAccount);
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
        public Int32 CompareTo(Relation other)
        {
            return this.CompareTo((IRelation) other);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public Boolean Equals(IRelation other)
        {
            return !ReferenceEquals(other, null) && (
                ReferenceEquals(this, other)
                || this.Account.Equals(other.Account)
                && this.Name.Equals(other.Name)
                && this.RelatingAccount.Equals(other.RelatingAccount)
            );
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public Boolean Equals(Relation other)
        {
            return this.Equals((IRelation) other);
        }

        /// <summary>
        /// 指定したリレーションが、このリレーションと完全に等しいかどうかを判断します。
        /// </summary>
        /// <param name="other">このリレーションと比較するリレーション。</param>
        /// <returns>
        /// 指定したリレーションがこのリレーションと完全に等しい場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        public Boolean EqualsExact(IRelation other)
        {
            return !ReferenceEquals(other, null) && (
                ReferenceEquals(this, other)
                || this.Account.EqualsExact(other.Account)
                && this.Name.Equals(other.Name)
                && this.RelatingAccount.EqualsExact(other.RelatingAccount)
            );
        }

        /// <summary>
        /// 指定したリレーションが、所属するストレージを含め、このリレーションと完全に等しいかどうかを判断します。
        /// </summary>
        /// <param name="other">このリレーションと比較するリレーション。</param>
        /// <returns>
        /// 指定したリレーションが、所属するストレージを含め、このリレーションと完全に等しい場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        public Boolean EqualsExact(Relation other)
        {
            return this.Storage == other.Storage
                && this.EqualsExact((IRelation) other);
        }

        #region Alternative Implementations

        /// <summary>
        /// このリレーションが関連付けられる先のアカウントを取得または設定します。
        /// </summary>
        /// <value>このリレーションが関連付けられる先のアカウント。</value>
        public Account RelatingAccount
        {
            get
            {
                return this.Storage.GetAccounts(this.RelatingAccountId)
                    .Single();
            }
            set
            {
                this.RelatingAccountId = value.AccountId;
            }
        }

        #endregion

        #region Implicit Implementations

        /// <summary>
        /// このリレーションが関連付けられているアカウントを取得または設定します。
        /// </summary>
        /// <value>このリレーションが関連付けられているアカウント。</value>
        IAccount IRelation.Account
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

        /// <summary>
        /// このリレーションが関連付けられる先のアカウントを取得または設定します。
        /// </summary>
        /// <value>このリレーションが関連付けられる先のアカウント。</value>
        IAccount IRelation.RelatingAccount
        {
            get
            {
                return this.RelatingAccount;
            }
            set
            {
                this.RelatingAccount = (Account) value;
            }
        }

        #endregion
    }
}