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
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Objects;
using System.Linq;

namespace XSpect.MetaTweet.Objects
{
    partial class StorageCache
    {
        /// <summary>
        /// 生成され、まだデータベースに格納されていないストレージ オブジェクトを管理する機能を提供します。
        /// </summary>
        [Serializable()]
        public class AddingObjectCache
            : Object,
              IEnumerable<StorageObject>
        {
            /// <summary>
            /// 親となる <see cref="StorageCache"/> を取得します。
            /// </summary>
            /// <value>
            /// 親となる <see cref="StorageCache"/>。
            /// </value>
            public StorageCache Cache
            {
                get;
                private set;
            }

            public List<Account> Accounts
            {
                get;
                private set;
            }

            public List<Activity> Activities
            {
                get;
                private set;
            }

            public List<Annotation> Annotations
            {
                get;
                private set;
            }

            public List<Relation> Relations
            {
                get;
                private set;
            }

            public List<Mark> Marks
            {
                get;
                private set;
            }

            public List<Reference> References
            {
                get;
                private set;
            }

            public List<Tag> Tags
            {
                get;
                private set;
            }

            /// <summary>
            /// <see cref="AddingObjectCache"/> の新しいインスタンスを初期化します。
            /// </summary>
            /// <param name="cache">親となる <see cref="StorageCache"/>。</param>
            public AddingObjectCache(StorageCache cache)
            {
                this.Cache = cache;
                this.Accounts = new List<Account>();
                this.Activities = new List<Activity>();
                this.Annotations = new List<Annotation>();
                this.Relations = new List<Relation>();
                this.Marks = new List<Mark>();
                this.References = new List<Reference>();
                this.Tags = new List<Tag>();
            }

            public IEnumerator<StorageObject> GetEnumerator()
            {
                return this.Accounts.Cast<StorageObject>()
                    .Concat(this.Activities.Cast<StorageObject>())
                    .Concat(this.Annotations.Cast<StorageObject>())
                    .Concat(this.Relations.Cast<StorageObject>())
                    .Concat(this.Marks.Cast<StorageObject>())
                    .Concat(this.References.Cast<StorageObject>())
                    .Concat(this.Tags.Cast<StorageObject>())
                    .GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public void Add(Account account)
            {
                this.Accounts.Add(account);
            }

            public void Add(Activity activity)
            {
                this.Activities.Add(activity);
            }

            public void Add(Annotation annotation)
            {
                this.Annotations.Add(annotation);
            }

            public void Add(Relation relation)
            {
                this.Relations.Add(relation);
            }

            public void Add(Mark mark)
            {
                this.Marks.Add(mark);
            }

            public void Add(Reference reference)
            {
                this.References.Add(reference);
            }

            public void Add(Tag tag)
            {
                this.Tags.Add(tag);
            }

            public void Add(StorageObject obj)
            {
                if (obj is Account)
                {
                    this.Add(obj as Account);
                }
                else if (obj is Activity)
                {
                    this.Add(obj as Activity);
                }
                else if (obj is Annotation)
                {
                    this.Add(obj as Annotation);
                }
                else if (obj is Relation)
                {
                    this.Add(obj as Relation);
                }
                else if (obj is Mark)
                {
                    this.Add(obj as Mark);
                }
                else if (obj is Reference)
                {
                    this.Add(obj as Reference);
                }
                else
                {
                    this.Add(obj as Tag);
                }
            }

            public void Remove(Account account)
            {
                this.Accounts.Remove(account);
            }

            public void Remove(Activity activity)
            {
                this.Activities.Remove(activity);
            }

            public void Remove(Annotation annotation)
            {
                this.Annotations.Remove(annotation);
            }

            public void Remove(Relation relation)
            {
                this.Relations.Remove(relation);
            }

            public void Remove(Mark mark)
            {
                this.Marks.Remove(mark);
            }

            public void Remove(Reference reference)
            {
                this.References.Remove(reference);
            }

            public void Remove(Tag tag)
            {
                this.Tags.Remove(tag);
            }

            public void Remove(StorageObject obj)
            {
                if (obj is Account)
                {
                    this.Remove(obj as Account);
                }
                else if (obj is Activity)
                {
                    this.Remove(obj as Activity);
                }
                else if (obj is Annotation)
                {
                    this.Remove(obj as Annotation);
                }
                else if (obj is Relation)
                {
                    this.Remove(obj as Relation);
                }
                else if (obj is Mark)
                {
                    this.Remove(obj as Mark);
                }
                else if (obj is Reference)
                {
                    this.Remove(obj as Reference);
                }
                else
                {
                    this.Remove(obj as Tag);
                }
            }

            public void Clear()
            {
                this.Accounts.Clear();
                this.Activities.Clear();
                this.Annotations.Clear();
                this.Relations.Clear();
                this.Marks.Clear();
                this.References.Clear();
                this.Tags.Clear();
            }

            /// <summary>
            /// 値を指定してアカウントを検索します。
            /// </summary>
            /// <param name="accountId">アカウントの ID。指定しない場合は <c>null</c>。</param>
            /// <param name="realm">アカウントのレルム。指定しない場合は <c>null</c>。</param>
            /// <returns>指定した条件に合致するアカウントのシーケンス。</returns>
            public IEnumerable<Account> GetAccounts(
                Nullable<Guid> accountId,
                String realm
            )
            {
                IEnumerable<Account> accounts = this.Accounts;
                if (accountId.HasValue)
                {
                    accounts = accounts.Where(a => a.AccountId == accountId);
                }
                if (realm != null)
                {
                    accounts = accounts.Where(a => a.Realm == realm);
                }
                return accounts;
            }

            /// <summary>
            /// 値を指定してアクティビティを検索します。
            /// </summary>
            /// <param name="accountId">アクティビティを行ったアカウントの ID。指定しない場合は <c>null</c>。</param>
            /// <param name="timestamp">アクティビティのタイムスタンプ。指定しない場合は <c>null</c>。</param>
            /// <param name="category">アクティビティのカテゴリ。指定しない場合は <c>null</c>。</param>
            /// <param name="subId">アクティビティのサブ ID。指定しない場合は <c>null</c>。</param>
            /// <param name="userAgent">アクティビティのユーザ エージェント。指定しない場合は <c>null</c>。</param>
            /// <param name="value">アクティビティの値。指定しない場合は <c>null</c>。条件として <c>null</c> 値を指定する場合は <see cref="DBNull"/> 値。</param>
            /// <param name="data">アクティビティのデータ。指定しない場合は <c>null</c>。条件として <c>null</c> 値を指定する場合は <see cref="DBNull"/> 値。</param>
            /// <returns>指定した条件に合致するアクティビティのシーケンス。</returns>
            public IEnumerable<Activity> GetActivities(
                Nullable<Guid> accountId,
                Nullable<DateTime> timestamp,
                String category,
                String subId,
                String userAgent,
                Object value,
                Object data
            )
            {
                IEnumerable<Activity> activities = this.Activities;
                if (accountId.HasValue)
                {
                    activities = activities.Where(a => a.AccountId == accountId);
                }
                if (timestamp.HasValue)
                {
                    DateTime rvalue = timestamp.Value.ToUniversalTime();
                    activities = activities.Where(a => a.Timestamp == rvalue);
                }
                if (category != null)
                {
                    activities = activities.Where(a => a.Category == category);
                }
                if (subId != null)
                {
                    activities = activities.Where(a => a.SubId == subId);
                }
                if (userAgent != null)
                {
                    activities = activities.Where(a => a.UserAgent == userAgent);
                }
                if (value is DBNull)
                {
                    activities = activities.Where(a => a.Value == null);
                }
                else if (value != null)
                {
                    String rvalue = value.ToString();
                    activities = activities.Where(a => a.Value == rvalue);
                }
                if (data is DBNull)
                {
                    activities = activities.Where(a => a.Data == null);
                }
                else if (data != null)
                {
                    activities = activities.Where(a => a.Data == data);
                }
                return activities;
            }

            /// <summary>
            /// 値を指定してアノテーションを検索します。
            /// </summary>
            /// <param name="accountId">アノテーションが関連付けられているアカウントの ID。指定しない場合は <c>null</c>。</param>
            /// <param name="name">アノテーションの意味。指定しない場合は <c>null</c>。</param>
            /// <returns>指定した条件に合致するアノテーションのシーケンス。</returns>
            public IEnumerable<Annotation> GetAnnotations(
                Nullable<Guid> accountId,
                String name
            )
            {
                IEnumerable<Annotation> annotations = this.Annotations;
                if (accountId.HasValue)
                {
                    annotations = annotations.Where(a => a.AccountId == accountId);
                }
                if (name != null)
                {
                    annotations = annotations.Where(a => a.Name == name);
                }
                return annotations;
            }

            /// <summary>
            /// 値を指定してリレーションを検索します。
            /// </summary>
            /// <param name="accountId">リレーションが関連付けられているアカウントの ID。指定しない場合は <c>null</c>。</param>
            /// <param name="name">リレーションの意味。</param>
            /// <param name="relatingAccountId">リレーションが関連付けられる先のアカウントの ID。指定しない場合は <c>null</c>。</param>
            /// <returns>指定した条件に合致するリレーションのシーケンス。</returns>
            public IEnumerable<Relation> GetRelations(
                Nullable<Guid> accountId,
                String name,
                Nullable<Guid> relatingAccountId
            )
            {
                IEnumerable<Relation> relations = this.Relations;
                if (accountId.HasValue)
                {
                    relations = relations.Where(r => r.AccountId == accountId);
                }
                if (name != null)
                {
                    relations = relations.Where(r => r.Name == name);
                }
                if (relatingAccountId.HasValue)
                {
                    relations = relations.Where(r => r.RelatingAccountId == relatingAccountId);
                }
                return relations;
            }

            /// <summary>
            /// 値を指定してマークを検索します。
            /// </summary>
            /// <param name="accountId">マークが関連付けられているアカウントの ID。指定しない場合は <c>null</c>。</param>
            /// <param name="name">マークの意味。指定しない場合は <c>null</c>。</param>
            /// <param name="markingAccountId">マークが関連付けられる先のアクティビティを行ったアカウントの ID。指定しない場合は <c>null</c>。</param>
            /// <param name="markingTimestamp">マークが関連付けられる先のアクティビティのタイムスタンプ。指定しない場合は <c>null</c>。</param>
            /// <param name="markingCategory">マークが関連付けられる先のアクティビティのカテゴリ。指定しない場合は <c>null</c>。</param>
            /// <param name="markingSubId">マークが関連付けられる先のアクティビティのサブ ID。指定しない場合は <c>null</c>。</param>
            /// <returns>指定した条件に合致するマークのシーケンス。</returns>
            public IEnumerable<Mark> GetMarks(
                Nullable<Guid> accountId,
                String name,
                Nullable<Guid> markingAccountId,
                Nullable<DateTime> markingTimestamp,
                String markingCategory,
                String markingSubId
            )
            {
                IEnumerable<Mark> marks = this.Marks;
                if (accountId.HasValue)
                {
                    marks = marks.Where(m => m.AccountId == accountId);
                }
                if (name != null)
                {
                    marks = marks.Where(m => m.Name == name);
                }
                if (markingAccountId.HasValue)
                {
                    marks = marks.Where(m => m.MarkingAccountId == markingAccountId);
                }
                if (markingTimestamp.HasValue)
                {
                    marks = marks.Where(m => m.MarkingTimestamp == markingTimestamp);
                }
                if (markingCategory != null)
                {
                    marks = marks.Where(m => m.MarkingCategory == markingCategory);
                }
                if (markingSubId != null)
                {
                    marks = marks.Where(m => m.MarkingSubId == markingSubId);
                }
                return marks;
            }

            /// <summary>
            /// 値を指定してリファレンスを検索します。
            /// </summary>
            /// <param name="accountId">リファレンスが関連付けられているアクティビティを行ったアカウントの ID。指定しない場合は <c>null</c>。</param>
            /// <param name="timestamp">リファレンスが関連付けられているアクティビティのタイムスタンプ。指定しない場合は <c>null</c>。</param>
            /// <param name="category">リファレンスが関連付けられているアクティビティのカテゴリ。指定しない場合は <c>null</c>。</param>
            /// <param name="subId">リファレンスが関連付けられているアクティビティのサブ ID。指定しない場合は <c>null</c>。</param>
            /// <param name="name">リファレンスの意味。指定しない場合は <c>null</c>。</param>
            /// <param name="referringAccountId">リファレンスが関連付けられる先のアクティビティを行ったアカウントの ID。指定しない場合は <c>null</c>。</param>
            /// <param name="referringTimestamp">リファレンスが関連付けられる先のアクティビティのタイムスタンプ。指定しない場合は <c>null</c>。</param>
            /// <param name="referringCategory">リファレンスが関連付けられる先のアクティビティのカテゴリ。指定しない場合は <c>null</c>。</param>
            /// <param name="referringSubId">リファレンスが関連付けられる先のアクティビティのサブ ID。指定しない場合は <c>null</c>。</param>
            /// <returns>指定した条件に合致するリファレンスのシーケンス。</returns>
            public IEnumerable<Reference> GetReferences(
                Nullable<Guid> accountId,
                Nullable<DateTime> timestamp,
                String category,
                String subId,
                String name,
                Nullable<Guid> referringAccountId,
                Nullable<DateTime> referringTimestamp,
                String referringCategory,
                String referringSubId
            )
            {
                IEnumerable<Reference> references = this.References;
                if (accountId.HasValue)
                {
                    references = references.Where(r => r.AccountId == accountId);
                }
                if (timestamp.HasValue)
                {
                    references = references.Where(r => r.Timestamp == timestamp);
                }
                if (category != null)
                {
                    references = references.Where(r => r.Category == category);
                }
                if (subId != null)
                {
                    references = references.Where(r => r.SubId == subId);
                }
                if (referringAccountId.HasValue)
                {
                    references = references.Where(r => r.ReferringAccountId == referringAccountId);
                }
                if (referringTimestamp.HasValue)
                {
                    references = references.Where(r => r.ReferringTimestamp == referringTimestamp);
                }
                if (referringCategory != null)
                {
                    references = references.Where(r => r.ReferringCategory == referringCategory);
                }
                if (referringSubId != null)
                {
                    references = references.Where(r => r.ReferringSubId == referringSubId);
                }
                return references;
            }

            /// <summary>
            /// 値を指定してタグを検索します。
            /// </summary>
            /// <param name="accountId">タグが関連付けられているアクティビティを行ったアカウントの ID。指定しない場合は <c>null</c>。</param>
            /// <param name="timestamp">タグが関連付けられているアクティビティのタイムスタンプ。指定しない場合は <c>null</c>。</param>
            /// <param name="category">タグが関連付けられているアクティビティのカテゴリ。指定しない場合は <c>null</c>。</param>
            /// <param name="subId">タグが関連付けられているアクティビティのサブ ID。指定しない場合は <c>null</c>。</param>
            /// <param name="name">タグの意味。指定しない場合は <c>null</c>。</param>
            /// <returns>条件に合致するタグのシーケンス。</returns>
            public IEnumerable<Tag> GetTags(
                Nullable<Guid> accountId,
                Nullable<DateTime> timestamp,
                String category,
                String subId,
                String name
            )
            {
                IEnumerable<Tag> tags = this.Tags;
                if (accountId.HasValue)
                {
                    tags = tags.Where(t => t.AccountId == accountId);
                }
                if (timestamp.HasValue)
                {
                    tags = tags.Where(t => t.Timestamp == timestamp);
                }
                if (category != null)
                {
                    tags = tags.Where(t => t.Category == category);
                }
                if (subId != null)
                {
                    tags = tags.Where(t => t.SubId == subId);
                }
                if (name != null)
                {
                    tags = tags.Where(t => t.Name == name);
                }
                return tags;
            }
        }
    }
}