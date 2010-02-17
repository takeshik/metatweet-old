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

namespace XSpect.MetaTweet.Objects
{
    partial class Activity
        : IActivity,
          IComparable<Activity>,
          IEquatable<Activity>
    {
        /// <summary>
        /// このアクティビティに関連付けられたタグの意味となる文字列のシーケンスを取得します。
        /// </summary>
        /// <value>このアクティビティに関連付けられたタグの意味となる文字列のシーケンス。</value>
        public IEnumerable<String> Tagging
        {
            get
            {
                return this.Tags.Select(t => t.Name);
            }
        }

        /// <summary>
        /// このアクティビティに関連付けられたリファレンスの意味と、対象となるアクティビティの組のシーケンスを取得します。
        /// </summary>
        /// <value>このアクティビティに関連付けられたリファレンスの意味と、対象となるアクティビティの組のシーケンス。</value>
        public IEnumerable<KeyValuePair<String, Activity>> Referring
        {
            get
            {
                return this.References.AsEnumerable().Select(r => new KeyValuePair<String, Activity>(r.Name, r.ReferringActivity));
            }
        }

        /// <summary>
        /// このアクティビティが対象として関連付けられたリファレンスの意味と、関連付けたアクティビティの組のシーケンスを取得します。
        /// </summary>
        /// <value>このアクティビティが対象として関連付けられたリファレンスの意味と、関連付けたアクティビティの組のシーケンス。</value>
        public IEnumerable<KeyValuePair<String, Activity>> Referrers
        {
            get
            {
                return this.ReverseReferences.AsEnumerable().Select(r => new KeyValuePair<String, Activity>(r.Name, r.Activity));
            }
        }

        /// <summary>
        /// このアクティビティが対象として関連付けられたマークの意味と、関連付けたアカウントの組のシーケンスを取得します。
        /// </summary>
        /// <value>このアクティビティが対象として関連付けられたマークの意味と、関連付けたアカウントの組のシーケンス。</value>
        public IEnumerable<KeyValuePair<String, Account>> Markers
        {
            get
            {
                return this.Marks.AsEnumerable().Select(m => new KeyValuePair<String, Account>(m.Name, m.Account));
            }
        }

        /// <summary>
        /// <see cref="Activity"/> の新しいインスタンスを初期化します。
        /// </summary>
        private Activity()
        {
        }

        /// <summary>
        /// <see cref="Activity"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="storage">オブジェクトが追加されるストレージ。</param>
        internal Activity(Storage storage)
            : base(storage)
        {
        }

        /// <summary>
        /// シリアル化したデータを使用して、<see cref="Activity"/> クラスの新しいインスタンスを初期化します。 
        /// </summary>
        /// <param name="info">シリアル化済みオブジェクト データを保持している <see cref="SerializationInfo"/>。</param>
        /// <param name="context">転送元または転送先に関するコンテキスト情報を含んでいる <see cref="StreamingContext"/>。</param>
        protected Activity(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.Account = (Account) info.GetValue("Account", typeof(Account));
            this.AccountId = this.Account.AccountId;
            this.Timestamp = (DateTime) info.GetValue("Timestamp", typeof(DateTime));
            this.Category = (String) info.GetValue("Category", typeof(String));
            this.SubId = (String) info.GetValue("SubId", typeof(String));
            this.UserAgent = (String) info.GetValue("UserAgent", typeof(String));
            this.Value = (String) info.GetValue("Value", typeof(String));
            this.Data = (Byte[]) info.GetValue("Data", typeof(Byte[]));
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
                ((obj is IActivity) && this.Equals(obj as IActivity));
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
                (this._SubId != null ? this._SubId.GetHashCode() : 0)
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
                "Act [{0}] @ {1}: {2}{3}",
                this.Account,
                this.Timestamp.ToString("s"),
                this.Category,
                String.IsNullOrEmpty(this.SubId)
                    ? String.Empty
                    : "(" + this.SubId + ")"
            );
        }

        /// <summary>
        /// このオブジェクトの完全な内容を表す <see cref="String"/> を返します。
        /// </summary>
        /// <returns>このオブジェクトの完全な内容を表す <see cref="String"/>。</returns>
        public override String Describe()
        {
            return String.Format(
                "Act [{0}] @ {1}: {2}{3}{4}{5}{6}",
                this.Account.Describe(),
                this.Timestamp.ToString("s"),
                this.Category,
                String.IsNullOrEmpty(this.SubId)
                    ? String.Empty
                    : "(" + this.SubId + ")",
                String.IsNullOrEmpty(this.Value)
                    ? String.Empty
                    : " = \"" + this.Value + "\"",
                this.Data != null ? " +" : String.Empty,
                String.IsNullOrEmpty(this.UserAgent)
                    ? String.Empty
                    : " (" + this.UserAgent + ")"
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
            if (!(other is Activity))
            {
                throw new ArgumentException("other");
            }
            return this.CompareTo(other as Activity);
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
            return other is Activity
                ? this.EqualsExact(other as Activity)
                : other is IActivity
                      ? this.EqualsExact(other as IActivity)
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
            info.AddValue("Timestamp", this.Timestamp, typeof(DateTime));
            info.AddValue("Category", this.Category, typeof(String));
            info.AddValue("SubId", this.SubId, typeof(String));
            info.AddValue("UserAgent", this.UserAgent, typeof(String));
            info.AddValue("Value", this.Value, typeof(String));
            info.AddValue("Data", this.Data, typeof(Byte[]));
        }

        /// <summary>
        /// 初期化の完了を通知するシグナルをオブジェクトに送信します。
        /// </summary>
        public override void EndInit()
        {
            this.Storage.Cache.Activities.Update(this);
            base.EndInit();
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
        public Int32 CompareTo(IActivity other)
        {
            // Timestamp -> Category -> SubId(numeric || text) -> Account
            Int32 result;
            Int64 x;
            Int64 y;
            return other == null
                ? 1
                : (result = this.Timestamp.CompareTo(other.Timestamp)) != 0
                      ? result
                      : (result = this.Category.CompareTo(other.Category)) != 0
                            ? result
                            : (result = (Int64.TryParse(this.SubId, out x) && Int64.TryParse(other.SubId, out y))
                                  ? x.CompareTo(y)
                                  : this.SubId.CompareTo(other.SubId)
                              ) != 0
                                  ? result
                                  : this.Account.CompareTo(other.Account);
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
        public Int32 CompareTo(Activity other)
        {
            return this.CompareTo(other as IActivity);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public Boolean Equals(IActivity other)
        {
            return !ReferenceEquals(other, null) && (
                ReferenceEquals(this, other)
                || this.Account.Equals(other.Account)
                && this.Timestamp.Equals(other.Timestamp)
                && this.Category.Equals(other.Category)
                && this.SubId.Equals(other.SubId)
            );
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public Boolean Equals(Activity other)
        {
            return this.Equals(other as IActivity);
        }

        /// <summary>
        /// 指定したアクティビティが、このアクティビティと完全に等しいかどうかを判断します。
        /// </summary>
        /// <param name="other">このアクティビティと比較するアクティビティ。</param>
        /// <returns>
        /// 指定したアクティビティがこのアクティビティと完全に等しい場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        public Boolean EqualsExact(IActivity other)
        {
            return !ReferenceEquals(other, null) && (
                ReferenceEquals(this, other)
                || this.Account.EqualsExact(other.Account)
                && this.Timestamp.Equals(other.Timestamp)
                && this.Category.Equals(other.Category)
                && this.SubId.Equals(other.SubId)
                && this.UserAgent.Equals(other.UserAgent)
                && this.Value.Equals(other.Value)
                && this.Data.Equals(other.Data)
            );
        }

        /// <summary>
        /// 指定したアクティビティが、所属するストレージを含め、このアクティビティと完全に等しいかどうかを判断します。
        /// </summary>
        /// <param name="other">このアクティビティと比較するアクティビティ。</param>
        /// <returns>
        /// 指定したアクティビティが、所属するストレージを含め、このアクティビティと完全に等しい場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        public Boolean EqualsExact(Activity other)
        {
            return this.Storage == other.Storage
                && this.EqualsExact(other as IActivity);
        }

        /// <summary>
        /// 意味を指定して、このアクティビティに関連付けられたリファレンスの対象となるアクティビティのシーケンスを取得します。
        /// </summary>
        /// <param name="name">リファレンスの意味。</param>
        /// <returns>
        /// このアクティビティに、指定した意味で関連付けられたリファレンスの対象となるアクティビティのシーケンス。
        /// </returns>
        public IEnumerable<Activity> ReferringOf(String name)
        {
            return this.Referring.Where(p => p.Key == name).Select(p => p.Value);
        }

        /// <summary>
        /// 意味を指定して、このアクティビティが対象として関連付けられたリファレンスを関連付けたアクティビティのシーケンスを取得します。
        /// </summary>
        /// <param name="name">リレーションの意味。</param>
        /// <returns>
        /// このアクティビティに、指定した意味でこのアクティビティが対象として関連付けられたリファレンスを関連付けたアクティビティのシーケンス。
        /// </returns>
        public IEnumerable<Activity> ReferrersOf(String name)
        {
            return this.Referrers.Where(p => p.Key == name).Select(p => p.Value);
        }

        /// <summary>
        /// 意味を指定して、このアクティビティが対象として関連付けられたマークを関連付けたアカウントのシーケンスを取得します。
        /// </summary>
        /// <param name="name">マークの意味。</param>
        /// <returns>
        /// このアクティビティに、指定した意味でこのアクティビティが対象として関連付けられたマークを関連付けたアカウントのシーケンス。
        /// </returns>
        public IEnumerable<Account> MarkersOf(String name)
        {
            return this.Markers.Where(p => p.Key == name).Select(p => p.Value);
        }

        /// <summary>
        /// このアクティビティに、指定した意味でタグが関連付けられているかどうかを示す値を取得します。
        /// </summary>
        /// <param name="name">タグの意味。</param>
        /// <returns>
        /// このアクティビティに、指定した意味でタグが関連付けられている場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        public Boolean IsTagging(String name)
        {
            return this.Tagging.Contains(name);
        }

        /// <summary>
        /// このアクティビティに、指定した意味とアクティビティでリファレンスが関連付けられているかどうかを示す値を取得します。
        /// </summary>
        /// <param name="name">リファレンスの意味。</param>
        /// <param name="activity">対象とするアカウント。</param>
        /// <returns>
        /// このアクティビティに、指定した意味とアクティビティでリファレンスが関連付けられている場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        public Boolean IsReferring(String name, Activity activity)
        {
            return this.ReferringOf(name).Contains(activity);
        }

        /// <summary>
        /// 指定したアクティビティに、指定した意味でこのアクティビティを対象としてリファレンスが関連付けられているかどうかを示す値を取得します。
        /// </summary>
        /// <param name="name">リファレンスの意味。</param>
        /// <param name="activity">リファレンスが関連付けられているかどうかを取得するアクティビティ。</param>
        /// <returns>
        /// 指定したアクティビティに、指定した意味でこのアクティビティを対象としてリファレンスが関連付けられている場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        public Boolean IsReferred(String name, Activity activity)
        {
            return this.ReferrersOf(name).Contains(activity);
        }

        /// <summary>
        /// 指定したアカウントに、指定した意味でこのアクティビティを対象としてマークが関連付けられているかどうかを示す値を取得します。
        /// </summary>
        /// <param name="name">マークの意味。</param>
        /// <param name="account">マークが関連付けられているかどうかを取得するアカウント。</param>
        /// <returns>
        /// 指定したアカウントに、指定した意味でこのアクティビティを対象としてマークが関連付けられている場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        public Boolean IsMarked(String name, Account account)
        {
            return this.MarkersOf(name).Contains(account);
        }

        /// <summary>
        /// このアクティビティにタグを関連付けます。
        /// </summary>
        /// <param name="name">タグの意味。</param>
        /// <returns></returns>
        public Tag Tag(String name)
        {
            return this.Storage.NewTag(this, name);
        }

        /// <summary>
        /// このアクティビティにリファレンスを関連付けます。
        /// </summary>
        /// <param name="name">リファレンスの意味。</param>
        /// <param name="referTo">リファレンスの対象となるアクティビティ。</param>
        /// <returns>関連付けられたリファレンス。</returns>
        public Reference Refer(String name, Activity referTo)
        {
            return this.Storage.NewReference(this, name, referTo);
        }

        /// <summary>
        /// このアクティビティを対象として、指定したアクティビティにリファレンスを関連付けます。
        /// </summary>
        /// <param name="name">リファレンスの意味。</param>
        /// <param name="referredFrom">リファレンスを関連付けるアクティビティ。</param>
        /// <returns>関連付けられたリファレンス。</returns>
        public Reference Referred(String name, Activity referredFrom)
        {
            return this.Storage.NewReference(referredFrom, name, this);
        }

        /// <summary>
        /// このアクティビティを対象として、指定したアカウントにマークを関連付けます。
        /// </summary>
        /// <param name="name">マークの意味。</param>
        /// <param name="markedFrom">マークを関連付けるアカウント。</param>
        /// <returns>関連付けられたマーク。</returns>
        public Mark Marked(String name, Account markedFrom)
        {
            return this.Storage.NewMark(markedFrom, name, this);
        }

        #region Alternative Implementations

        /// <summary>
        /// このアクティビティが対象として関連付けられたリファレンスのシーケンスを取得します。
        /// </summary>
        /// <value>このアクティビティが対象として関連付けられたリファレンスのシーケンス。</value>
        public IEnumerable<Reference> ReverseReferences
        {
            get
            {
                return this.Storage.GetReferences(null, null, this);
            }
        }

        #endregion

        #region Implicit Implementations

        /// <summary>
        /// このアクティビティを行ったアカウントを取得または設定します。
        /// </summary>
        /// <value>このアクティビティを行ったアカウント。</value>
        IAccount IActivity.Account
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
        /// このアクティビティに関連付けられたタグのシーケンスを取得します。
        /// </summary>
        /// <value>このアクティビティに関連付けられたタグのシーケンス。</value>
        IEnumerable<ITag> IActivity.Tags
        {
            get
            {
                return this.Tags.Cast<ITag>();
            }
        }

        /// <summary>
        /// このアクティビティに関連付けられたリファレンスのシーケンスを取得します。
        /// </summary>
        /// <value>このアクティビティに関連付けられたリファレンスのシーケンス。</value>
        IEnumerable<IReference> IActivity.References
        {
            get
            {
                return this.References.Cast<IReference>();
            }
        }

        /// <summary>
        /// このアクティビティが対象として関連付けられたリファレンスのシーケンスを取得します。
        /// </summary>
        /// <value>このアクティビティが対象として関連付けられたリファレンスのシーケンス。</value>
        IEnumerable<IReference> IActivity.ReverseReferences
        {
            get
            {
                return this.ReverseReferences.Cast<IReference>();
            }
        }

        /// <summary>
        /// このアクティビティに関連付けられたリファレンスの意味と、対象となるアクティビティの組のシーケンスを取得します。
        /// </summary>
        /// <value>このアクティビティに関連付けられたリファレンスの意味と、対象となるアクティビティの組のシーケンス。</value>
        IEnumerable<KeyValuePair<String, IActivity>> IActivity.Referring
        {
            get
            {
                return this.Referring
                    .Select(p => new KeyValuePair<String, IActivity>(p.Key, p.Value));
            }
        }

        /// <summary>
        /// このアクティビティが対象として関連付けられたリファレンスの意味と、関連付けたアクティビティの組のシーケンスを取得します。
        /// </summary>
        /// <value>このアクティビティが対象として関連付けられたリファレンスの意味と、関連付けたアクティビティの組のシーケンス。</value>
        IEnumerable<KeyValuePair<String, IActivity>> IActivity.Referrers
        {
            get
            {
                return this.Referrers
                    .Select(p => new KeyValuePair<String, IActivity>(p.Key, p.Value));
            }
        }

        /// <summary>
        /// このアクティビティに関連付けられたマークのシーケンスを取得します。
        /// </summary>
        /// <value>このアクティビティに関連付けられたマークのシーケンス。</value>
        IEnumerable<IMark> IActivity.Marks
        {
            get
            {
                return this.Marks.Cast<IMark>();
            }
        }

        /// <summary>
        /// このアクティビティが対象として関連付けられたマークの意味と、関連付けたアカウントの組のシーケンスを取得します。
        /// </summary>
        /// <value>このアクティビティが対象として関連付けられたマークの意味と、関連付けたアカウントの組のシーケンス。</value>
        IEnumerable<KeyValuePair<String, IAccount>> IActivity.Markers
        {
            get
            {
                return this.Markers
                    .Select(p => new KeyValuePair<String, IAccount>(p.Key, p.Value));
            }
        }

        /// <summary>
        /// 意味を指定して、このアクティビティに関連付けられたリファレンスの対象となるアクティビティのシーケンスを取得します。
        /// </summary>
        /// <param name="name">リファレンスの意味。</param>
        /// <returns>
        /// このアクティビティに、指定した意味で関連付けられたリファレンスの対象となるアクティビティのシーケンス。
        /// </returns>
        IEnumerable<IActivity> IActivity.ReferringOf(String name)
        {
            return this.ReferringOf(name).Cast<IActivity>();
        }

        /// <summary>
        /// 意味を指定して、このアクティビティが対象として関連付けられたリファレンスを関連付けたアクティビティのシーケンスを取得します。
        /// </summary>
        /// <param name="name">リレーションの意味。</param>
        /// <returns>
        /// このアクティビティに、指定した意味でこのアクティビティが対象として関連付けられたリファレンスを関連付けたアクティビティのシーケンス。
        /// </returns>
        IEnumerable<IActivity> IActivity.ReferrersOf(String name)
        {
            return this.ReferrersOf(name).Cast<IActivity>();
        }

        /// <summary>
        /// 意味を指定して、このアクティビティが対象として関連付けられたマークを関連付けたアカウントのシーケンスを取得します。
        /// </summary>
        /// <param name="name">マークの意味。</param>
        /// <returns>
        /// このアクティビティに、指定した意味でこのアクティビティが対象として関連付けられたマークを関連付けたアカウントのシーケンス。
        /// </returns>
        IEnumerable<IAccount> IActivity.MarkersOf(String name)
        {
            return this.MarkersOf(name).Cast<IAccount>();
        }

        /// <summary>
        /// このアクティビティに、指定した意味とアクティビティでリファレンスが関連付けられているかどうかを示す値を取得します。
        /// </summary>
        /// <param name="name">リファレンスの意味。</param>
        /// <param name="activity">対象とするアカウント。</param>
        /// <returns>
        /// このアクティビティに、指定した意味とアクティビティでリファレンスが関連付けられている場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        Boolean IActivity.IsReferring(String name, IActivity activity)
        {
            return this.IsReferring(name, (Activity) activity);
        }

        /// <summary>
        /// 指定したアクティビティに、指定した意味でこのアクティビティを対象としてリファレンスが関連付けられているかどうかを示す値を取得します。
        /// </summary>
        /// <param name="name">リファレンスの意味。</param>
        /// <param name="activity">リファレンスが関連付けられているかどうかを取得するアクティビティ。</param>
        /// <returns>
        /// 指定したアクティビティに、指定した意味でこのアクティビティを対象としてリファレンスが関連付けられている場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        Boolean IActivity.IsReferred(String name, IActivity activity)
        {
            return this.IsReferred(name, (Activity) activity);
        }

        /// <summary>
        /// 指定したアカウントに、指定した意味でこのアクティビティを対象としてマークが関連付けられているかどうかを示す値を取得します。
        /// </summary>
        /// <param name="name">マークの意味。</param>
        /// <param name="account">マークが関連付けられているかどうかを取得するアカウント。</param>
        /// <returns>
        /// 指定したアカウントに、指定した意味でこのアクティビティを対象としてマークが関連付けられている場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        Boolean IActivity.IsMarked(String name, IAccount account)
        {
            return this.IsMarked(name, (Account) account);
        }

        /// <summary>
        /// このアクティビティにタグを関連付けます。
        /// </summary>
        /// <param name="name">タグの意味。</param>
        /// <returns></returns>
        ITag IActivity.Tag(String name)
        {
            return this.Tag(name);
        }

        /// <summary>
        /// このアクティビティにリファレンスを関連付けます。
        /// </summary>
        /// <param name="name">リファレンスの意味。</param>
        /// <param name="referTo">リファレンスの対象となるアクティビティ。</param>
        /// <returns>関連付けられたリファレンス。</returns>
        IReference IActivity.Refer(String name, IActivity referTo)
        {
            return this.Refer(name, (Activity) referTo);
        }

        /// <summary>
        /// このアクティビティを対象として、指定したアクティビティにリファレンスを関連付けます。
        /// </summary>
        /// <param name="name">リファレンスの意味。</param>
        /// <param name="referredFrom">リファレンスを関連付けるアクティビティ。</param>
        /// <returns>関連付けられたリファレンス。</returns>
        IReference IActivity.Referred(String name, IActivity referredFrom)
        {
            return this.Referred(name, (Activity) referredFrom);
        }

        /// <summary>
        /// このアクティビティを対象として、指定したアカウントにマークを関連付けます。
        /// </summary>
        /// <param name="name">マークの意味。</param>
        /// <param name="markedFrom">マークを関連付けるアカウント。</param>
        /// <returns>関連付けられたマーク。</returns>
        IMark IActivity.Marked(String name, IAccount markedFrom)
        {
            return this.Marked(name, (Account) markedFrom);
        }

        #endregion

        #region Partial Method Implementations

        partial void OnAccountIdChanging(Guid value)
        {
            if (!this.IsInitializing)
            {
                this.Storage.Cache.Activities.Remove(this);

                foreach (Tag tag in this.Tags)
                {
                    tag.AccountId = value;
                }
                foreach (Reference reference in this.References)
                {
                    reference.AccountId = value;
                }
                foreach (Reference reference in this.ReverseReferences)
                {
                    reference.ReferringAccountId = value;
                }
            }
        }

        partial void OnAccountIdChanged()
        {
            if (!this.IsInitializing)
            {
                this.Storage.Cache.Activities.Update(this);
            }
        }

        partial void OnTimestampChanging(DateTime value)
        {
            value = value.ToUniversalTime();
            if (!this.IsInitializing)
            {
                foreach (Tag tag in this.Tags)
                {
                    tag.Timestamp = value;
                }
                foreach (Reference reference in this.References)
                {
                    reference.Timestamp = value;
                }
                foreach (Reference reference in this.ReverseReferences)
                {
                    reference.ReferringTimestamp = value;
                }
            }
        }

        partial void OnTimestampChanged()
        {
            if (!this.IsInitializing)
            {
                this.Storage.Cache.Activities.Update(this);
            }
        }

        partial void OnCategoryChanging(String value)
        {
            if (!this.IsInitializing)
            {
                this.Storage.Cache.Activities.Remove(this);

                foreach (Tag tag in this.Tags)
                {
                    tag.Category = value;
                }
                foreach (Reference reference in this.References)
                {
                    reference.Category = value;
                }
                foreach (Reference reference in this.ReverseReferences)
                {
                    reference.ReferringCategory = value;
                }
            }
        }

        partial void OnCategoryChanged()
        {
            if (!this.IsInitializing)
            {
                this.Storage.Cache.Activities.Update(this);
            }
        }

        partial void OnSubIdChanging(String value)
        {
            if (!this.IsInitializing)
            {
                foreach (Tag tag in this.Tags)
                {
                    tag.SubId = value;
                }
                foreach (Reference reference in this.References)
                {
                    reference.SubId = value;
                }
                foreach (Reference reference in this.ReverseReferences)
                {
                    reference.ReferringSubId = value;
                }
            }
        }

        partial void OnSubIdChanged()
        {
            if (!this.IsInitializing)
            {
                this.Storage.Cache.Activities.Update(this);
            }
        }

        #endregion
    }
}
