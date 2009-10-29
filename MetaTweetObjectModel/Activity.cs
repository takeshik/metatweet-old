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
using System.Collections.Generic;
using System.Linq;

namespace XSpect.MetaTweet.Objects
{
    partial class Activity
        : IActivity
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
                return this.References.ToList().Select(r => new KeyValuePair<String, Activity>(r.Name, r.ReferringActivity));
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
                return this.ReverseReferences.ToList().Select(r => new KeyValuePair<String, Activity>(r.Name, r.Activity));
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
                return this.Marks.ToList().Select(m => new KeyValuePair<String, Account>(m.Name, m.Account));
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
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override String ToString()
        {
            return String.Format(
                "Act [{0}] @ {1}: {2}{3}{4}{5}{6}",
                this.Account,
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
        /// 削除後の処理を完了した後に <see cref="StorageObject.Deleted"/> イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している <see cref="EventArgs"/>。</param>
        protected override void OnDeleted(EventArgs e)
        {
            // NOTE: Alternative implementation.
            foreach (Reference reference in this.ReverseReferences)
            {
                reference.Delete();
            }
            base.OnDeleted(e);
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

        #region Implicit Implementations

        /// <summary>
        /// このアクティビティに関連付けられたタグのシーケンスを取得します。
        /// </summary>
        /// <value>このアクティビティに関連付けられたタグのシーケンス。</value>
        IEnumerable<Tag> IActivity.Tags
        {
            get
            {
                return this.Tags;
            }
        }

        /// <summary>
        /// このアクティビティに関連付けられたリファレンスのシーケンスを取得します。
        /// </summary>
        /// <value>このアクティビティに関連付けられたリファレンスのシーケンス。</value>
        IEnumerable<Reference> IActivity.References
        {
            get
            {
                return this.References;
            }
        }

        /// <summary>
        /// このアクティビティが対象として関連付けられたリファレンスのシーケンスを取得します。
        /// </summary>
        /// <value>このアクティビティが対象として関連付けられたリファレンスのシーケンス。</value>
        IEnumerable<Reference> IActivity.ReverseReferences
        {
            get
            {
                return this.ReverseReferences;
            }
        }

        /// <summary>
        /// このアクティビティに関連付けられたマークのシーケンスを取得します。
        /// </summary>
        /// <value>このアクティビティに関連付けられたマークのシーケンス。</value>
        IEnumerable<Mark> IActivity.Marks
        {
            get
            {
                return this.Marks;
            }
        }

        #endregion

        // NOTE: Alternative implementation.
        /// <summary>
        /// このアクティビティが対象として関連付けられたリファレンスのシーケンスを取得します。
        /// </summary>
        /// <value>このアクティビティが対象として関連付けられたリファレンスのシーケンス。</value>
        public IQueryable<Reference> ReverseReferences
        {
            get
            {
                return this.Storage.GetReferences(null, null, this);
            }
        }
    }
}
