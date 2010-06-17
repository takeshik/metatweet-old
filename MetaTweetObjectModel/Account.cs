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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace XSpect.MetaTweet.Objects
{
    partial class Account
        : IAccount,
          IComparable<Account>,
          IEquatable<Account>
    {
        private IDictionary<String, String> _seedsCache;

        private static readonly SHA1 _sha1 = new SHA1Cng();

        /// <summary>
        /// このアカウントによる、指定されたカテゴリの最新のアクティビティを取得します。
        /// </summary>
        /// <value></value>
        /// <returns>このアカウントによる、指定されたカテゴリの最新のアクティビティ。</returns>
        public Activity this[String category]
        {
            get
            {
                return this.Storage.Cache.Activities.GetActivity(this.AccountId, category);
            }
        }

        /// <summary>
        /// このアカウントによる、指定されたカテゴリの、指定した時点で最新のアクティビティを取得します。
        /// </summary>
        /// <value></value>
        /// <returns>このアカウントによる、指定されたカテゴリの、指定した時点で最新のアクティビティ。</returns>
        public Activity this[String category, DateTime baseline]
        {
            get
            {
                return this.ActivitiesOf(category)
                    .Where(a => a.Timestamp <= baseline)
                    .AsEnumerable()
                    .OrderByDescending(a => a)
                    .FirstOrDefault();
            }
        }

        /// <summary>
        /// オブジェクトの種類を取得します。
        /// </summary>
        /// <value>オブジェクトの種類を表す <see cref="StorageObjectTypes"/> 値。</value>
        public override StorageObjectTypes ObjectType
        {
            get
            {
                return StorageObjectTypes.Account;
            }
        }

        /// <summary>
        /// このアカウントに関連付けられたアノテーションの意味となる文字列のシーケンスを取得します。
        /// </summary>
        /// <value>このアカウントに関連付けられたアノテーションの意味となる文字列のシーケンス。</value>
        public IEnumerable<String> Annotating
        {
            get
            {
                return this.Annotations.Select(a => a.Name);
            }
        }

        /// <summary>
        /// このアカウントに関連付けられたリレーションの意味と、対象となるアカウントの組のシーケンスを取得します。
        /// </summary>
        /// <value>このアカウントに関連付けられたリレーションの意味と、対象となるアカウントの組のシーケンス。</value>
        public IEnumerable<KeyValuePair<String, Account>> Relating
        {
            get
            {
                return this.Relations.AsEnumerable().Select(r => new KeyValuePair<String, Account>(r.Name, r.RelatingAccount));
            }
        }

        /// <summary>
        /// このアカウントが対象として関連付けられたリレーションの意味と、関連付けたアカウントの組のシーケンスを取得します。
        /// </summary>
        /// <value>このアカウントが対象として関連付けられたリレーションの意味と、関連付けたアカウントの組のシーケンス。</value>
        public IEnumerable<KeyValuePair<String, Account>> Relators
        {
            get
            {
                return this.ReverseRelations.AsEnumerable().Select(r => new KeyValuePair<String, Account>(r.Name, r.Account));
            }
        }

        /// <summary>
        /// このアカウントに関連付けられたマークの意味と、対象となるアクティビティの組のシーケンスを取得します。
        /// </summary>
        /// <value>このアカウントに関連付けられたマークの意味と、対象となるアクティビティの組のシーケンス。</value>
        public IEnumerable<KeyValuePair<String, Activity>> Marking
        {
            get
            {
                return this.Marks.AsEnumerable().Select(m => new KeyValuePair<String, Activity>(m.Name, m.MarkingActivity));
            }
        }

        /// <summary>
        /// <see cref="Account"/> の新しいインスタンスを初期化します。
        /// </summary>
        private Account()
        {
        }

        /// <summary>
        /// <see cref="Account"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="storage">オブジェクトが追加されるストレージ。</param>
        internal Account(Storage storage)
            : base(storage)
        {
        }

        /// <summary>
        /// シリアル化したデータを使用して、<see cref="Account"/> クラスの新しいインスタンスを初期化します。 
        /// </summary>
        /// <param name="info">シリアル化済みオブジェクト データを保持している <see cref="SerializationInfo"/>。</param>
        /// <param name="context">転送元または転送先に関するコンテキスト情報を含んでいる <see cref="StreamingContext"/>。</param>
        protected Account(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.AccountId = (String) info.GetValue("AccountId", typeof(String));
            this.Realm = (String) info.GetValue("Realm", typeof(String));
            this.SeedString = (String) info.GetValue("SeedString", typeof(String));
        }

        /// <summary>
        /// シード文字列からシード値のディクショナリを取得します。
        /// </summary>
        /// <param name="seedString">シード文字列。</param>
        /// <returns>シード値。</returns>
        public static IDictionary<String, String> GetSeeds(String seedString)
        {
            return seedString
                .Split(new Char[] { '!', }, StringSplitOptions.RemoveEmptyEntries)
                .OrderBy(s => s)
                .Select(s => s.Split('='))
                .ToDictionary(a => a[0], a => a[1]);
        }

        /// <summary>
        /// シード値のディクショナリからシード文字列を取得します。
        /// </summary>
        /// <param name="seeds">シード値。</param>
        /// <returns>シード文字列。</returns>
        public static String GetSeedString(IDictionary<String, String> seeds)
        {
            return String.Join(String.Empty, seeds.Select(p => "!" + p.Key + "=" + p.Value).OrderBy(s => s).ToArray());
        }

        /// <summary>
        /// レルム文字列とシード文字列から SHA-1 アカウント ID を生成します。
        /// </summary>
        /// <param name="realm">アカウントのレルム。</param>
        /// <param name="seedString">アカウントのシード文字列。</param>
        /// <returns>指定したレルム文字列とシード文字列から生成された、アカウントの ID。</returns>
        public static String GetAccountId(String realm, String seedString)
        {
            return String.Join(String.Empty, _sha1.ComputeHash(Encoding.BigEndianUnicode.GetBytes(seedString + "@" + realm))
                .Select(b => b.ToString("x2"))
                .ToArray()
            );
        }

        /// <summary>
        /// レルム文字列とシード値から SHA-1 アカウント ID を生成します。
        /// </summary>
        /// <param name="realm">アカウントのレルム。</param>
        /// <param name="seeds">アカウントのシード値。</param>
        /// <returns>指定したレルム文字列とシード値から生成された、アカウントの ID。</returns>
        public static String GetAccountId(String realm, IDictionary<String, String> seeds)
        {
            return GetAccountId(realm, GetSeedString(seeds));
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
                ((obj is IAccount) && this.Equals((IAccount) obj));
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override Int32 GetHashCode()
        {
            return this._AccountId != null ? this._AccountId.GetHashCode() : 0;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override String ToString()
        {
            return "Acc " + this.AccountId;
        }

        /// <summary>
        /// このオブジェクトの完全な内容を表す <see cref="String"/> を返します。
        /// </summary>
        /// <returns>このオブジェクトの完全な内容を表す <see cref="String"/>。</returns>
        public override String Describe()
        {
            return String.Format(
                "Acc {0}@{1}: {2}",
                this.AccountId,
                this.Realm,
                this.SeedString
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
            if (!(other is Account))
            {
                throw new ArgumentException("other");
            }
            return this.CompareTo((Account) other);
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
            return other is Account
                ? this.EqualsExact((Account) other)
                : other is IAccount
                      ? this.EqualsExact((IAccount) other)
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
            info.AddValue("AccountId", this.AccountId, typeof(Guid));
            info.AddValue("Realm", this.Realm, typeof(String));
            info.AddValue("SeedString", this.SeedString, typeof(String));
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
        public Int32 CompareTo(IAccount other)
        {
            // AccountId
            return other == null
                ? 1
                : this.AccountId.CompareTo(other.AccountId);
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
        public Int32 CompareTo(Account other)
        {
            return this.CompareTo((IAccount) other);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public Boolean Equals(IAccount other)
        {
            return !ReferenceEquals(other, null) && (
                ReferenceEquals(this, other)
                || this.AccountId.Equals(other.AccountId)
            );
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public Boolean Equals(Account other)
        {
            return this.Equals((IAccount) other);
        }

        /// <summary>
        /// 指定したアカウントが、このアカウントと完全に等しいかどうかを判断します。
        /// </summary>
        /// <param name="other">このアカウントと比較するアカウント。</param>
        /// <returns>
        /// 指定したアカウントがこのアカウントと完全に等しい場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        public Boolean EqualsExact(IAccount other)
        {
            return !ReferenceEquals(other, null) && (
                ReferenceEquals(this, other)
                || this.AccountId.Equals(other.AccountId)
                && this.Realm.Equals(other.Realm)
            );
        }

        /// <summary>
        /// 指定したアカウントが、所属するストレージを含め、このアカウントと完全に等しいかどうかを判断します。
        /// </summary>
        /// <param name="other">このアカウントと比較するアカウント。</param>
        /// <returns>
        /// 指定したアカウントが、所属するストレージを含め、このアカウントと完全に等しい場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        public Boolean EqualsExact(Account other)
        {
            return this.Storage == other.Storage
                && this.EqualsExact((IAccount) other);
        }

        /// <summary>
        /// サービス内でこのアカウントを一意に識別するための情報となるディクショナリを取得または設定します。
        /// </summary>
        /// <value>サービス内でこのアカウントを一意に識別するための情報となるディクショナリ。</value>
        public IDictionary<String, String> Seeds
        {
            get
            {
                return this._seedsCache ?? (this._seedsCache = GetSeeds(this.SeedString));
            }
        }

        /// <summary>
        /// カテゴリとサブ ID を指定して、このアカウントによるアクティビティのシーケンスを取得します。
        /// </summary>
        /// <param name="category">取得するアクティビティのカテゴリ。</param>
        /// <param name="subId">取得するアクティビティのサブ ID。</param>
        /// <returns>
        /// 指定したカテゴリとサブ ID の、このアカウントによるアクティビティのシーケンス。
        /// </returns>
        public IEnumerable<Activity> ActivitiesOf(String category, String subId)
        {
            return this.Activities
                .Where(a => a.Category == category && a.SubId == (subId ?? String.Empty));
        }

        /// <summary>
        /// カテゴリを指定して、このアカウントによるアクティビティのシーケンスを取得します。
        /// </summary>
        /// <param name="category">取得するアクティビティのカテゴリ。</param>
        /// <returns>指定したカテゴリの、このアカウントによるアクティビティのシーケンス。</returns>
        public IEnumerable<Activity> ActivitiesOf(String category)
        {
            return this.ActivitiesOf(category, null);
        }

        /// <summary>
        /// 意味を指定して、このアカウントに関連付けられたリレーションの対象となるアカウントのシーケンスを取得します。
        /// </summary>
        /// <param name="name">リレーションの意味。</param>
        /// <returns>
        /// このアカウントに、指定した意味で関連付けられたリレーションの対象となるアカウントのシーケンス。
        /// </returns>
        public IEnumerable<Account> RelatingOf(String name)
        {
            return this.Relating.Where(p => p.Key == name).Select(p => p.Value);
        }

        /// <summary>
        /// 意味を指定して、このアカウントが対象として関連付けられたリレーションを関連付けたアカウントのシーケンスを取得します。
        /// </summary>
        /// <param name="name">リレーションの意味。</param>
        /// <returns>
        /// このアカウントに、指定した意味でこのアカウントが対象として関連付けられたリレーションを関連付けたアカウントのシーケンス。
        /// </returns>
        public IEnumerable<Account> RelatorsOf(String name)
        {
            return this.Relators.Where(p => p.Key == name).Select(p => p.Value);
        }

        /// <summary>
        /// 意味を指定して、このアカウントに関連付けられたマークの対象となるアクティビティのシーケンスを取得します。
        /// </summary>
        /// <param name="name">マークの意味。</param>
        /// <returns>
        /// このアカウントに、指定した意味で関連付けられたマークの対象となるアクティビティのシーケンス。
        /// </returns>
        public IEnumerable<Activity> MarkingOf(String name)
        {
            return this.Marking.Where(p => p.Key == name).Select(p => p.Value);
        }

        /// <summary>
        /// このアカウントに、指定した意味でアノテーションが関連付けられているかどうかを示す値を取得します。
        /// </summary>
        /// <param name="name">アノテーションの意味。</param>
        /// <returns>
        /// このアカウントに、指定した意味でアノテーションが関連付けられている場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        public Boolean IsAnnotating(String name)
        {
            return this.Annotating.Contains(name);
        }

        /// <summary>
        /// このアカウントに、指定した意味とアカウントでリレーションが関連付けられているかどうかを示す値を取得します。
        /// </summary>
        /// <param name="name">アノテーションの意味。</param>
        /// <param name="account">対象とするアカウント。</param>
        /// <returns>
        /// このアカウントに、指定した意味とアカウントでリレーションが関連付けられている場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        public Boolean IsRelating(String name, Account account)
        {
            return this.RelatingOf(name).Contains(account);
        }

        /// <summary>
        /// 指定したアカウントに、指定した意味でこのアカウントを対象としてリレーションが関連付けられているかどうかを示す値を取得します。
        /// </summary>
        /// <param name="name">アノテーションの意味。</param>
        /// <param name="account">リレーションが関連付けられているかどうかを取得するアカウント。</param>
        /// <returns>
        /// 指定したアカウントに、指定した意味でこのアカウントを対象としてリレーションが関連付けられている場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        public Boolean IsRelated(String name, Account account)
        {
            return this.RelatorsOf(name).Contains(account);
        }

        /// <summary>
        /// このアカウントが、指定した意味でアカウントでリレーションが関連付けられているかどうかを示す値を取得します。
        /// </summary>
        /// <param name="name">マークの名前。</param>
        /// <param name="activity">対象とするアクティビティ。</param>
        /// <returns>
        /// このアカウントに、指定した意味とアクティビティでリレーションが関連付けられている場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        public Boolean IsMarking(String name, Activity activity)
        {
            return this.MarkingOf(name).Contains(activity);
        }

        /// <summary>
        /// このアカウントによるアクティビティを追加します。
        /// </summary>
        /// <param name="timestamp">アクティビティのタイムスタンプ。</param>
        /// <param name="category">アクティビティのカテゴリ。</param>
        /// <param name="subId">アクティビティのサブ ID。</param>
        /// <param name="userAgent">アクティビティのユーザ エージェント。</param>
        /// <param name="value">アクティビティの値。</param>
        /// <param name="data">アクティビティのデータ。</param>
        /// <returns>追加されたアクティビティ。</returns>
        public Activity Act(DateTime timestamp, String category, String subId, String userAgent, String value, Byte[] data)
        {
            return this.Storage.NewActivity(this, timestamp, category, subId, userAgent, value, data);
        }

        /// <summary>
        /// このアカウントによるアクティビティを追加します。
        /// </summary>
        /// <param name="timestamp">アクティビティのタイムスタンプ。</param>
        /// <param name="category">アクティビティのカテゴリ。</param>
        /// <param name="subId">アクティビティのサブ ID。</param>
        /// <returns>追加されたアクティビティ。</returns>
        public Activity Act(DateTime timestamp, String category, String subId)
        {
            return this.Storage.NewActivity(this, timestamp, category, subId);
        }

        /// <summary>
        /// このアカウントにアノテーションを関連付けます。
        /// </summary>
        /// <param name="name">アノテーションの意味。</param>
        /// <param name="value">アノテーションの値。</param>
        /// <returns>関連付けられたアノテーション。</returns>
        public Annotation Annotate(String name, String value)
        {
            return this.Storage.NewAnnotation(this, name, value);
        }

        /// <summary>
        /// このアカウントにリレーションを関連付けます。
        /// </summary>
        /// <param name="name">リレーションの意味。</param>
        /// <param name="relateTo">リレーションの対象となるアカウント。</param>
        /// <returns>関連付けられたリレーション。</returns>
        public Relation Relate(String name, Account relateTo)
        {
            return this.Storage.NewRelation(this, name, relateTo);
        }

        /// <summary>
        /// このアカウントを対象として、指定したアカウントにリレーションを関連付けます。
        /// </summary>
        /// <param name="name">リレーションの意味。</param>
        /// <param name="relatedFrom">リレーションを関連付けるアカウント。</param>
        /// <returns>関連付けられたリレーション。</returns>
        public Relation Related(String name, Account relatedFrom)
        {
            return this.Storage.NewRelation(relatedFrom, name, this);
        }

        /// <summary>
        /// このアカウントにマークを関連付けます。
        /// </summary>
        /// <param name="name">マークの意味。</param>
        /// <param name="markTo">マークの対象となるアクティビティ。</param>
        /// <returns>関連付けたマーク。</returns>
        public Mark Mark(String name, Activity markTo)
        {
            return this.Storage.NewMark(this, name, markTo);
        }

        #region Alternative Implementations

        /// <summary>
        /// このアカウントが対象として関連付けられたリレーションのシーケンスを取得します。
        /// </summary>
        /// <value>このアカウントが対象として関連付けられたリレーションのシーケンス。</value>
        public IEnumerable<Relation> ReverseRelations
        {
            get
            {
                return this.Storage.GetRelations(null, null, this);
            }
        }

        #endregion

        #region Implicit Implementations

        /// <summary>
        /// このアカウントによる、指定されたカテゴリの最新のアクティビティを取得します。
        /// </summary>
        /// <value></value>
        /// <returns>このアカウントによる、指定されたカテゴリの最新のアクティビティ。</returns>
        IActivity IAccount.this[String category]
        {
            get
            {
                return this[category];
            }
        }

        /// <summary>
        /// このアカウントによる、指定されたカテゴリの、指定した時点で最新のアクティビティを取得します。
        /// </summary>
        /// <value></value>
        /// <returns>このアカウントによる、指定されたカテゴリの、指定した時点で最新のアクティビティ。</returns>
        IActivity IAccount.this[String category, DateTime baseline]
        {
            get
            {
                return this[category, baseline];
            }
        }

        /// <summary>
        /// このアカウントによって行われたアクティビティのシーケンスを取得します。
        /// </summary>
        /// <value>このアカウントによって行われたアクティビティのシーケンス。</value>
        IEnumerable<IActivity> IAccount.Activities
        {
            get
            {
                return this.Activities.Cast<IActivity>();
            }
        }

        /// <summary>
        /// このアカウントに関連付けられたアノテーションのシーケンスを取得します。
        /// </summary>
        /// <value>このアカウントに関連付けられたアノテーションのシーケンス。</value>
        IEnumerable<IAnnotation> IAccount.Annotations
        {
            get
            {
                return this.Annotations.Cast<IAnnotation>();
            }
        }

        /// <summary>
        /// このアカウントに関連付けられたリレーションのシーケンスを取得します。
        /// </summary>
        /// <value>このアカウントに関連付けられたリレーションのシーケンス。</value>
        IEnumerable<IRelation> IAccount.Relations
        {
            get
            {
                return this.Relations.Cast<IRelation>();
            }
        }

        /// <summary>
        /// このアカウントが対象として関連付けられたリレーションのシーケンスを取得します。
        /// </summary>
        /// <value>このアカウントが対象として関連付けられたリレーションのシーケンス。</value>
        IEnumerable<IRelation> IAccount.ReverseRelations
        {
            get
            {
                return this.ReverseRelations.Cast<IRelation>();
            }
        }

        /// <summary>
        /// このアカウントに関連付けられたリレーションの意味と、対象となるアカウントの組のシーケンスを取得します。
        /// </summary>
        /// <value>このアカウントに関連付けられたリレーションの意味と、対象となるアカウントの組のシーケンス。</value>
        IEnumerable<KeyValuePair<String, IAccount>> IAccount.Relating
        {
            get
            {
                return this.Relating
                    .Select(p => new KeyValuePair<String, IAccount>(p.Key, p.Value));
            }
        }

        /// <summary>
        /// このアカウントが対象として関連付けられたリレーションの意味と、関連付けたアカウントの組のシーケンスを取得します。
        /// </summary>
        /// <value>このアカウントが対象として関連付けられたリレーションの意味と、関連付けたアカウントの組のシーケンス。</value>
        IEnumerable<KeyValuePair<String, IAccount>> IAccount.Relators
        {
            get
            {
                return this.Relators
                    .Select(p => new KeyValuePair<String, IAccount>(p.Key, p.Value));
            }
        }

        /// <summary>
        /// このアカウントに関連付けられたマークのシーケンスを取得します。
        /// </summary>
        /// <value>このアカウントに関連付けられたマークのシーケンス。</value>
        IEnumerable<IMark> IAccount.Marks
        {
            get
            {
                return this.Marks.Cast<IMark>();
            }
        }

        /// <summary>
        /// このアカウントに関連付けられたマークの意味と、対象となるアクティビティの組のシーケンスを取得します。
        /// </summary>
        /// <value>このアカウントに関連付けられたマークの意味と、対象となるアクティビティの組のシーケンス。</value>
        IEnumerable<KeyValuePair<String, IActivity>> IAccount.Marking
        {
            get
            {
                return this.Marking
                    .Select(p => new KeyValuePair<String, IActivity>(p.Key, p.Value));
            }
        }

        /// <summary>
        /// カテゴリとサブ ID を指定して、このアカウントによるアクティビティのシーケンスを取得します。
        /// </summary>
        /// <param name="category">取得するアクティビティのカテゴリ。</param>
        /// <param name="subId">取得するアクティビティのサブ ID。</param>
        /// <returns>
        /// 指定したカテゴリとサブ ID の、このアカウントによるアクティビティのシーケンス。
        /// </returns>
        IEnumerable<IActivity> IAccount.ActivitiesOf(String category, String subId)
        {
            return this.ActivitiesOf(category, subId).Cast<IActivity>();
        }

        /// <summary>
        /// カテゴリを指定して、このアカウントによるアクティビティのシーケンスを取得します。
        /// </summary>
        /// <param name="category">取得するアクティビティのカテゴリ。</param>
        /// <returns>指定したカテゴリの、このアカウントによるアクティビティのシーケンス。</returns>
        IEnumerable<IActivity> IAccount.ActivitiesOf(String category)
        {
            return this.ActivitiesOf(category).Cast<IActivity>();
        }

        /// <summary>
        /// 意味を指定して、このアカウントに関連付けられたリレーションの対象となるアカウントのシーケンスを取得します。
        /// </summary>
        /// <param name="name">リレーションの意味。</param>
        /// <returns>
        /// このアカウントに、指定した意味で関連付けられたリレーションの対象となるアカウントのシーケンス。
        /// </returns>
        IEnumerable<IAccount> IAccount.RelatingOf(String name)
        {
            return this.RelatingOf(name).Cast<IAccount>();
        }

        /// <summary>
        /// 意味を指定して、このアカウントが対象として関連付けられたリレーションを関連付けたアカウントのシーケンスを取得します。
        /// </summary>
        /// <param name="name">リレーションの意味。</param>
        /// <returns>
        /// このアカウントに、指定した意味でこのアカウントが対象として関連付けられたリレーションを関連付けたアカウントのシーケンス。
        /// </returns>
        IEnumerable<IAccount> IAccount.RelatorsOf(String name)
        {
            return this.RelatorsOf(name).Cast<IAccount>();
        }

        /// <summary>
        /// 意味を指定して、このアカウントに関連付けられたマークの対象となるアクティビティのシーケンスを取得します。
        /// </summary>
        /// <param name="name">マークの意味。</param>
        /// <returns>
        /// このアカウントに、指定した意味で関連付けられたマークの対象となるアクティビティのシーケンス。
        /// </returns>
        IEnumerable<IActivity> IAccount.MarkingOf(String name)
        {
            return this.MarkingOf(name).Cast<IActivity>();
        }

        /// <summary>
        /// このアカウントに、指定した意味とアカウントでリレーションが関連付けられているかどうかを示す値を取得します。
        /// </summary>
        /// <param name="name">アノテーションの意味。</param>
        /// <param name="account">対象とするアカウント。</param>
        /// <returns>
        /// このアカウントに、指定した意味とアカウントでリレーションが関連付けられている場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        Boolean IAccount.IsRelating(String name, IAccount account)
        {
            return this.IsRelating(name, (Account) account);
        }

        /// <summary>
        /// 指定したアカウントに、指定した意味でこのアカウントを対象としてリレーションが関連付けられているかどうかを示す値を取得します。
        /// </summary>
        /// <param name="name">アノテーションの意味。</param>
        /// <param name="account">リレーションが関連付けられているかどうかを取得するアカウント。</param>
        /// <returns>
        /// 指定したアカウントに、指定した意味でこのアカウントを対象としてリレーションが関連付けられている場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        Boolean IAccount.IsRelated(String name, IAccount account)
        {
            return this.IsRelated(name, (Account) account);
        }

        /// <summary>
        /// このアカウントが、指定した意味でアカウントでリレーションが関連付けられているかどうかを示す値を取得します。
        /// </summary>
        /// <param name="name">マークの名前。</param>
        /// <param name="activity">対象とするアクティビティ。</param>
        /// <returns>
        /// このアカウントに、指定した意味とアクティビティでリレーションが関連付けられている場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        Boolean IAccount.IsMarking(String name, IActivity activity)
        {
            return this.IsMarking(name, (Activity) activity);
        }

        /// <summary>
        /// このアカウントによるアクティビティを追加します。
        /// </summary>
        /// <param name="timestamp">アクティビティのタイムスタンプ。</param>
        /// <param name="category">アクティビティのカテゴリ。</param>
        /// <param name="subId">アクティビティのサブ ID。</param>
        /// <param name="userAgent">アクティビティのユーザ エージェント。</param>
        /// <param name="value">アクティビティの値。</param>
        /// <param name="data">アクティビティのデータ。</param>
        /// <returns>追加されたアクティビティ。</returns>
        IActivity IAccount.Act(DateTime timestamp, String category, String subId, String userAgent, String value, Byte[] data)
        {
            return this.Act(timestamp, category, subId, userAgent, value, data);
        }

        /// <summary>
        /// このアカウントによるアクティビティを追加します。
        /// </summary>
        /// <param name="timestamp">アクティビティのタイムスタンプ。</param>
        /// <param name="category">アクティビティのカテゴリ。</param>
        /// <param name="subId">アクティビティのサブ ID。</param>
        /// <returns>追加されたアクティビティ。</returns>
        IActivity IAccount.Act(DateTime timestamp, String category, String subId)
        {
            return this.Act(timestamp, category, subId);
        }

        /// <summary>
        /// このアカウントにアノテーションを関連付けます。
        /// </summary>
        /// <param name="name">アノテーションの意味。</param>
        /// <param name="value">アノテーションの値。</param>
        /// <returns>関連付けられたアノテーション。</returns>
        IAnnotation IAccount.Annotate(String name, String value)
        {
            return this.Annotate(name, value);
        }

        /// <summary>
        /// このアカウントにリレーションを関連付けます。
        /// </summary>
        /// <param name="name">リレーションの意味。</param>
        /// <param name="relateTo">リレーションの対象となるアカウント。</param>
        /// <returns>関連付けられたリレーション。</returns>
        IRelation IAccount.Relate(String name, IAccount relateTo)
        {
            return this.Relate(name, (Account) relateTo);
        }

        /// <summary>
        /// Relateds the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="relateTo">The relate to.</param>
        /// <returns></returns>
        IRelation IAccount.Related(String name, IAccount relateTo)
        {
            return this.Related(name, (Account) relateTo);
        }

        /// <summary>
        /// このアカウントにマークを関連付けます。
        /// </summary>
        /// <param name="name">マークの意味。</param>
        /// <param name="markTo">マークの対象となるアクティビティ。</param>
        /// <returns>関連付けたマーク。</returns>
        IMark IAccount.Mark(String name, IActivity markTo)
        {
            return this.Mark(name, (Activity) markTo);
        }

        #endregion
    }
}