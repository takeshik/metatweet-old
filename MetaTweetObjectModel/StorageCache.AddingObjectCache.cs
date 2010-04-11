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
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Objects;
using System.Linq;

namespace XSpect.MetaTweet.Objects
{
    partial class StorageCache
    {
        /// <summary>
        /// 生成され、まだデータベースに格納されていないストレージ オブジェクトを格納します。
        /// </summary>
        [Serializable()]
        public class AddingObjectCache
            : MarshalByRefObject,
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

            /// <summary>
            /// 生成され、まだデータベースに格納されていないアカウントのリストを取得します。
            /// </summary>
            /// <value>
            /// 生成され、まだデータベースに格納されていないアカウントのリスト。
            /// </value>
            public List<Account> Accounts
            {
                get;
                private set;
            }

            /// <summary>
            /// 生成され、まだデータベースに格納されていないアクティビティのリストを取得します。
            /// </summary>
            /// <value>
            /// 生成され、まだデータベースに格納されていないアクティビティのリスト。
            /// </value>
            public List<Activity> Activities
            {
                get;
                private set;
            }

            /// <summary>
            /// 生成され、まだデータベースに格納されていないアノテーションのリストを取得します。
            /// </summary>
            /// <value>
            /// 生成され、まだデータベースに格納されていないアノテーションのリスト。
            /// </value>
            public List<Annotation> Annotations
            {
                get;
                private set;
            }

            /// <summary>
            /// 生成され、まだデータベースに格納されていないリレーションのリストを取得します。
            /// </summary>
            /// <value>
            /// 生成され、まだデータベースに格納されていないリレーションのリスト。
            /// </value>
            public List<Relation> Relations
            {
                get;
                private set;
            }

            /// <summary>
            /// 生成され、まだデータベースに格納されていないマークのリストを取得します。
            /// </summary>
            /// <value>
            /// 生成され、まだデータベースに格納されていないマークのリスト。
            /// </value>
            public List<Mark> Marks
            {
                get;
                private set;
            }

            /// <summary>
            /// 生成され、まだデータベースに格納されていないリファレンスのリストを取得します。
            /// </summary>
            /// <value>
            /// 生成され、まだデータベースに格納されていないリファレンスのリスト。
            /// </value>
            public List<Reference> References
            {
                get;
                private set;
            }

            /// <summary>
            /// 生成され、まだデータベースに格納されていないタグのリストを取得します。
            /// </summary>
            /// <value>
            /// 生成され、まだデータベースに格納されていないタグのリスト。
            /// </value>
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

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
            /// </returns>
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

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>
            /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
            /// </returns>
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            /// <summary>
            /// アカウントをキャッシュに追加します。
            /// </summary>
            /// <param name="account">追加するアカウント。</param>
            /// <returns>アカウントがキャッシュに追加された場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
            public Boolean Add(Account account)
            {
                if (this.Accounts.Contains(account) || account.EntityState != EntityState.Added)
                {
                    return false;
                }
                else
                {
                    this.Accounts.Add(account);
                    return true;
                }
            }

            /// <summary>
            /// アクティビティをキャッシュに追加します。
            /// </summary>
            /// <param name="activity">追加するアクティビティ。</param>
            /// <returns>アクティビティがキャッシュに追加された場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
            public Boolean Add(Activity activity)
            {
                if (this.Activities.Contains(activity) || activity.EntityState != EntityState.Added)
                {
                    return false;
                }
                else
                {
                    this.Activities.Add(activity);
                    return true;
                }
            }

            /// <summary>
            /// アノテーションをキャッシュに追加します。
            /// </summary>
            /// <param name="annotation">追加するアノテーション。</param>
            /// <returns>アノテーションがキャッシュに追加された場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
            public Boolean Add(Annotation annotation)
            {
                if (this.Annotations.Contains(annotation) || annotation.EntityState != EntityState.Added)
                {
                    return false;
                }
                else
                {
                    this.Annotations.Add(annotation);
                    return true;
                }
            }

            /// <summary>
            /// リレーションをキャッシュに追加します。
            /// </summary>
            /// <param name="relation">追加するリレーション。</param>
            /// <returns>リレーションがキャッシュに追加された場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
            public Boolean Add(Relation relation)
            {
                if (this.Relations.Contains(relation) || relation.EntityState != EntityState.Added)
                {
                    return false;
                }
                else
                {
                    this.Relations.Add(relation);
                    return true;
                }
            }

            /// <summary>
            /// マークをキャッシュに追加します。
            /// </summary>
            /// <param name="mark">追加するマーク。</param>
            /// <returns>マークがキャッシュに追加された場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
            public Boolean Add(Mark mark)
            {
                if (this.Marks.Contains(mark) || mark.EntityState != EntityState.Added)
                {
                    return false;
                }
                else
                {
                    this.Marks.Add(mark);
                    return true;
                }
            }

            /// <summary>
            /// リファレンスをキャッシュに追加します。
            /// </summary>
            /// <param name="reference">追加するリファレンス。</param>
            /// <returns>リファレンスがキャッシュに追加された場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
            public Boolean Add(Reference reference)
            {
                if (this.References.Contains(reference) || reference.EntityState != EntityState.Added)
                {
                    return false;
                }
                else
                {
                    this.References.Add(reference);
                    return true;
                }
            }

            /// <summary>
            /// タグをキャッシュに追加します。
            /// </summary>
            /// <param name="tag">追加するタグ。</param>
            /// <returns>タグがキャッシュに追加された場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
            public Boolean Add(Tag tag)
            {
                if (this.Tags.Contains(tag) || tag.EntityState != EntityState.Added)
                {
                    return false;
                }
                else
                {
                    this.Tags.Add(tag);
                    return true;
                }
            }

            /// <summary>
            /// ストレージ オブジェクトをキャッシュに追加します。
            /// </summary>
            /// <param name="obj">追加するストレージ オブジェクト。</param>
            /// <returns>ストレージ オブジェクトがキャッシュに追加された場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
            public Boolean Add(StorageObject obj)
            {
                switch (obj.ObjectType)
                {
                    case StorageObjectTypes.Account:
                        return this.Add(obj as Account);
                    case StorageObjectTypes.Activity:
                        return this.Add(obj as Activity);
                    case StorageObjectTypes.Annotation:
                        return this.Add(obj as Annotation);
                    case StorageObjectTypes.Relation:
                        return this.Add(obj as Relation);
                    case StorageObjectTypes.Mark:
                        return this.Add(obj as Mark);
                    case StorageObjectTypes.Reference:
                        return this.Add(obj as Reference);
                    default: // case StorageObjectTypes.Reference:
                        return this.Add(obj as Tag);
                }
            }

            /// <summary>
            /// アカウントをキャッシュから削除します。
            /// </summary>
            /// <param name="account">削除するアカウント。</param>
            /// <returns>アカウントがキャッシュにから削除された場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
            public Boolean Remove(Account account)
            {
                return this.Accounts.Remove(account);
            }

            /// <summary>
            /// アクティビティをキャッシュから削除します。
            /// </summary>
            /// <param name="activity">削除するアクティビティ。</param>
            /// <returns>アクティビティがキャッシュにから削除された場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
            public Boolean Remove(Activity activity)
            {
                return this.Activities.Remove(activity);
            }

            /// <summary>
            /// アノテーションをキャッシュから削除します。
            /// </summary>
            /// <param name="annotation">削除するアノテーション。</param>
            /// <returns>アカウントがキャッシュにから削除された場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
            public Boolean Remove(Annotation annotation)
            {
                return this.Annotations.Remove(annotation);
            }

            /// <summary>
            /// リレーションをキャッシュから削除します。
            /// </summary>
            /// <param name="relation">削除するリレーション。</param>
            /// <returns>リレーションがキャッシュにから削除された場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
            public Boolean Remove(Relation relation)
            {
                return this.Relations.Remove(relation);
            }

            /// <summary>
            /// マークをキャッシュから削除します。
            /// </summary>
            /// <param name="mark">削除するマーク。</param>
            /// <returns>マークがキャッシュにから削除された場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
            public Boolean Remove(Mark mark)
            {
                return this.Marks.Remove(mark);
            }

            /// <summary>
            /// リファレンスををキャッシュから削除します。
            /// </summary>
            /// <param name="reference">削除するリファレンス。</param>
            /// <returns>リファレンスがキャッシュにから削除された場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
            public Boolean Remove(Reference reference)
            {
                return this.References.Remove(reference);
            }

            /// <summary>
            /// タグをキャッシュから削除します。
            /// </summary>
            /// <param name="tag">削除するタグ。</param>
            /// <returns>タグがキャッシュにから削除された場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
            public Boolean Remove(Tag tag)
            {
                return this.Tags.Remove(tag);
            }

            /// <summary>
            /// ストレージ オブジェクトをキャッシュから削除します。
            /// </summary>
            /// <param name="obj">削除するストレージ オブジェクト。</param>
            /// <returns>ストレージ オブジェクトがキャッシュにから削除された場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
            public Boolean Remove(StorageObject obj)
            {
                switch (obj.ObjectType)
                {
                    case StorageObjectTypes.Account:
                        return this.Remove(obj as Account);
                    case StorageObjectTypes.Activity:
                        return this.Remove(obj as Activity);
                    case StorageObjectTypes.Annotation:
                        return this.Remove(obj as Annotation);
                    case StorageObjectTypes.Relation:
                        return this.Remove(obj as Relation);
                    case StorageObjectTypes.Mark:
                        return this.Remove(obj as Mark);
                    case StorageObjectTypes.Reference:
                        return this.Remove(obj as Reference);
                    default: // case StorageObjectTypes.Reference:
                        return this.Remove(obj as Tag);
                }
            }

            /// <summary>
            /// キャッシュから全てのオブジェクトを削除します。
            /// </summary>
            /// <returns>キャッシュの内容が全て消去されたかを表す値。常に <c>true</c>。</returns>
            public Boolean Clear()
            {
                return this.Clear(true);
            }

            /// <summary>
            /// データベースに格納されたオブジェクトのみに限定するかを指定して、キャッシュからオブジェクトを削除します。
            /// </summary>
            /// <param name="clearAll">全てのオブジェクトを削除する場合は <c>true</c>。削除対象をデータベースに格納されたオブジェクトに限定する場合は <c>false</c>。</param>
            /// <returns>キャッシュの内容が全て消去された場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
            public Boolean Clear(Boolean clearAll)
            {
                if (clearAll)
                {
                    this.Accounts.Clear();
                    this.Activities.Clear();
                    this.Annotations.Clear();
                    this.Relations.Clear();
                    this.Marks.Clear();
                    this.References.Clear();
                    this.Tags.Clear();
                    return true;
                }
                else
                {
                    this.Accounts.RemoveAll(a => a.EntityState != EntityState.Added);
                    this.Activities.RemoveAll(a => a.EntityState != EntityState.Added);
                    this.Annotations.RemoveAll(a => a.EntityState != EntityState.Added);
                    this.Relations.RemoveAll(r => r.EntityState != EntityState.Added);
                    this.Marks.RemoveAll(m => m.EntityState != EntityState.Added);
                    this.References.RemoveAll(r => r.EntityState != EntityState.Added);
                    this.Tags.RemoveAll(t => t.EntityState != EntityState.Added);
                    return !(this.Accounts.Any()
                        || this.Activities.Any()
                        || this.Annotations.Any()
                        || this.Relations.Any()
                        || this.Marks.Any()
                        || this.References.Any()
                        || this.Tags.Any()
                    );
                }
            }

            /// <summary>
            /// 値を指定してアカウントを検索します。
            /// </summary>
            /// <param name="accountId">アカウントの ID。指定しない場合は <c>null</c>。</param>
            /// <param name="realm">アカウントのレルム。指定しない場合は <c>null</c>。</param>
            /// <param name="seedString">アカウントのシード文字列。指定しない場合は <c>null</c>。</param>
            /// <returns>指定した条件に合致するアカウントのシーケンス。</returns>
            public IEnumerable<Account> GetAccounts(
                String accountId,
                String realm,
                String seedString
            )
            {
                IEnumerable<Account> accounts = this.Accounts;
                if (accountId != null)
                {
                    accounts = accounts.Where(a => a.AccountId == accountId);
                }
                if (realm != null)
                {
                    accounts = accounts.Where(a => a.Realm == realm);
                }
                if (seedString != null)
                {
                    accounts = accounts.Where(a => a.SeedString == seedString);
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
                String accountId,
                Nullable<DateTime> timestamp,
                String category,
                String subId,
                String userAgent,
                Object value,
                Object data
            )
            {
                IEnumerable<Activity> activities = this.Activities;
                if (accountId != null)
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
            /// <param name="value">アノテーションの値。指定しない場合は <c>null</c>。</param>
            /// <returns>指定した条件に合致するアノテーションのシーケンス。</returns>
            public IEnumerable<Annotation> GetAnnotations(
                String accountId,
                String name,
                String value
            )
            {
                IEnumerable<Annotation> annotations = this.Annotations;
                if (accountId != null)
                {
                    annotations = annotations.Where(a => a.AccountId == accountId);
                }
                if (name != null)
                {
                    annotations = annotations.Where(a => a.Name == name);
                }
                if (value != null)
                {
                    annotations = annotations.Where(a => a.Value == value);
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
                String accountId,
                String name,
                String relatingAccountId
            )
            {
                IEnumerable<Relation> relations = this.Relations;
                if (accountId != null)
                {
                    relations = relations.Where(r => r.AccountId == accountId);
                }
                if (name != null)
                {
                    relations = relations.Where(r => r.Name == name);
                }
                if (relatingAccountId != null)
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
                String accountId,
                String name,
                String markingAccountId,
                Nullable<DateTime> markingTimestamp,
                String markingCategory,
                String markingSubId
            )
            {
                IEnumerable<Mark> marks = this.Marks;
                if (accountId != null)
                {
                    marks = marks.Where(m => m.AccountId == accountId);
                }
                if (name != null)
                {
                    marks = marks.Where(m => m.Name == name);
                }
                if (markingAccountId != null)
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
                String accountId,
                Nullable<DateTime> timestamp,
                String category,
                String subId,
                String name,
                String referringAccountId,
                Nullable<DateTime> referringTimestamp,
                String referringCategory,
                String referringSubId
            )
            {
                IEnumerable<Reference> references = this.References;
                if (accountId != null)
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
                if (referringAccountId != null)
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
            /// <param name="value">タグの値。指定しない場合は <c>null</c>。</param>
            /// <returns>条件に合致するタグのシーケンス。</returns>
            public IEnumerable<Tag> GetTags(
                String accountId,
                Nullable<DateTime> timestamp,
                String category,
                String subId,
                String name,
                String value
            )
            {
                IEnumerable<Tag> tags = this.Tags;
                if (accountId != null)
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
                if (value != null)
                {
                    tags = tags.Where(t => t.Value == value);
                }
                return tags;
            }
        }
    }
}