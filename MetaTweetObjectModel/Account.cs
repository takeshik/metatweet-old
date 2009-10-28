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
    partial class Account
        : IAccount
    {
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
                    .Where(a => a.Timestamp < baseline)
                    .ToList()
                    .OrderByDescending(a => a)
                    .FirstOrDefault();
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
                return this.Relations.ToList().Select(r => new KeyValuePair<String, Account>(r.Name, r.RelatingAccount));
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
                return this.ReverseRelations.ToList().Select(r => new KeyValuePair<String, Account>(r.Name, r.Account));
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
                return this.Marks.ToList().Select(m => new KeyValuePair<String, Activity>(m.Name, m.MarkingActivity));
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
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override String ToString()
        {
            return String.Format(
                "Acc {0}@{1}",
                this.AccountId.ToString("D"),
                this.Realm
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
            return this.CompareTo(other as Account);
        }

        /// <summary>
        /// 削除後の処理を完了した後に <see cref="StorageObject.Deleted"/> イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している <see cref="EventArgs"/>。</param>
        protected override void OnDeleted(EventArgs e)
        {
            // NOTE: Alternative implementation.
            foreach (Relation relation in this.ReverseRelations)
            {
                relation.Delete();
            }
            base.OnDeleted(e);
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
                .Where(a => a.Category == category && subId == (subId ?? String.Empty));
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
        /// <returns>関連付けられたアノテーション。</returns>
        public Annotation Annotate(String name)
        {
            return this.Storage.NewAnnotation(this, name);
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

        #region Implicit Implementations

        /// <summary>
        /// このアカウントによって行われたアクティビティのシーケンスを取得します。
        /// </summary>
        /// <value>このアカウントによって行われたアクティビティのシーケンス。</value>
        IEnumerable<Activity> IAccount.Activities
        {
            get
            {
                return this.Activities;
            }
        }

        /// <summary>
        /// このアカウントに関連付けられたアノテーションのシーケンスを取得します。
        /// </summary>
        /// <value>このアカウントに関連付けられたアノテーションのシーケンス。</value>
        IEnumerable<Annotation> IAccount.Annotations
        {
            get
            {
                return this.Annotations;
            }
        }

        /// <summary>
        /// このアカウントに関連付けられたリレーションのシーケンスを取得します。
        /// </summary>
        /// <value>このアカウントに関連付けられたリレーションのシーケンス。</value>
        IEnumerable<Relation> IAccount.Relations
        {
            get
            {
                return this.Relations;
            }
        }

        /// <summary>
        /// このアカウントが対象として関連付けられたリレーションのシーケンスを取得します。
        /// </summary>
        /// <value>このアカウントが対象として関連付けられたリレーションのシーケンス。</value>
        IEnumerable<Relation> IAccount.ReverseRelations
        {
            get
            {
                return this.ReverseRelations;
            }
        }

        /// <summary>
        /// このアカウントに関連付けられたマークのシーケンスを取得します。
        /// </summary>
        /// <value>このアカウントに関連付けられたマークのシーケンス。</value>
        IEnumerable<Mark> IAccount.Marks
        {
            get
            {
                return this.Marks;
            }
        }

        #endregion

        // NOTE: Alternative implementation.
        /// <summary>
        /// このアカウントが対象として関連付けられたリレーションのシーケンスを取得します。
        /// </summary>
        /// <value>このアカウントが対象として関連付けられたリレーションのシーケンス。</value>
        public IQueryable<Relation> ReverseRelations
        {
            get
            {
                return this.Storage.GetRelations(null, null, this);
            }
        }
    }
}