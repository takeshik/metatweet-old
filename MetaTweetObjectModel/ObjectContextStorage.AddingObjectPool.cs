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
using System.Data;
using System.Linq;

namespace XSpect.MetaTweet.Objects
{
    partial class ObjectContextStorage
    {
        /// <summary>
        /// 生成され、まだデータベースに格納されていないストレージ オブジェクトを格納します。
        /// </summary>
        public class AddingObjectPool
            : Object,
              IEnumerable<StorageObject>
        {
            /// <summary>
            /// 生成され、まだデータベースに格納されていないアカウントのリストを取得します。
            /// </summary>
            /// <value>
            /// 生成され、まだデータベースに格納されていないアカウントのリスト。
            /// </value>
            public HashSet<Account> Accounts
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
            public HashSet<Activity> Activities
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
            public HashSet<Annotation> Annotations
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
            public HashSet<Relation> Relations
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
            public HashSet<Mark> Marks
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
            public HashSet<Reference> References
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
            public HashSet<Tag> Tags
            {
                get;
                private set;
            }

            /// <summary>
            /// <see cref="AddingObjectPool"/> の新しいインスタンスを初期化します。
            /// </summary>
            public AddingObjectPool()
            {
                this.Accounts = new HashSet<Account>();
                this.Activities = new HashSet<Activity>();
                this.Annotations = new HashSet<Annotation>();
                this.Relations = new HashSet<Relation>();
                this.Marks = new HashSet<Mark>();
                this.References = new HashSet<Reference>();
                this.Tags = new HashSet<Tag>();
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
            /// </returns>
            public IEnumerator<StorageObject> GetEnumerator()
            {
                return ((IEnumerable<StorageObject>) this.Accounts)
                    .Concat(this.Activities)
                    .Concat(this.Annotations)
                    .Concat(this.Relations)
                    .Concat(this.Marks)
                    .Concat(this.References)
                    .Concat(this.Tags)
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
                        return this.Add((Account) obj);
                    case StorageObjectTypes.Activity:
                        return this.Add((Activity) obj);
                    case StorageObjectTypes.Annotation:
                        return this.Add((Annotation) obj);
                    case StorageObjectTypes.Relation:
                        return this.Add((Relation) obj);
                    case StorageObjectTypes.Mark:
                        return this.Add((Mark) obj);
                    case StorageObjectTypes.Reference:
                        return this.Add((Reference) obj);
                    default: // case StorageObjectTypes.Tag:
                        return this.Add((Tag) obj);
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
                        return this.Remove((Account) obj);
                    case StorageObjectTypes.Activity:
                        return this.Remove((Activity) obj);
                    case StorageObjectTypes.Annotation:
                        return this.Remove((Annotation) obj);
                    case StorageObjectTypes.Relation:
                        return this.Remove((Relation) obj);
                    case StorageObjectTypes.Mark:
                        return this.Remove((Mark) obj);
                    case StorageObjectTypes.Reference:
                        return this.Remove((Reference) obj);
                    default: // case StorageObjectTypes.Tag:
                        return this.Remove((Tag) obj);
                }
            }

            /// <summary>
            /// 指定したシーケンスと比較して、このプールと重複するアカウントを消去します。
            /// </summary>
            /// <param name="accounts">比較するアカウントのシーケンス。</param>
            public void RemoveDuplicates(IEnumerable<Account> accounts)
            {
                this.Accounts.ExceptWith(accounts);
            }

            /// <summary>
            /// 指定したシーケンスと比較して、このプールと重複するアクティビティを消去します。
            /// </summary>
            /// <param name="activities">比較するのシーケンス。</param>
            public void RemoveDuplicates(IEnumerable<Activity> activities)
            {
                this.Activities.ExceptWith(activities);
            }

            /// <summary>
            /// 指定したシーケンスと比較して、このプールと重複するアノテーションを消去します。
            /// </summary>
            /// <param name="annotations">比較するアノテーションのシーケンス。</param>
            public void RemoveDuplicates(IEnumerable<Annotation> annotations)
            {
                this.Annotations.ExceptWith(annotations);
            }

            /// <summary>
            /// 指定したシーケンスと比較して、このプールと重複するリレーションを消去します。
            /// </summary>
            /// <param name="relations">比較するリレーションのシーケンス。</param>
            public void RemoveDuplicates(IEnumerable<Relation> relations)
            {
                this.Relations.ExceptWith(relations);
            }

            /// <summary>
            /// 指定したシーケンスと比較して、このプールと重複するマークを消去します。
            /// </summary>
            /// <param name="marks">比較するマークのシーケンス。</param>
            public void RemoveDuplicates(IEnumerable<Mark> marks)
            {
                this.Marks.ExceptWith(marks);
            }

            /// <summary>
            /// 指定したシーケンスと比較して、このプールと重複するリファレンスを消去します。
            /// </summary>
            /// <param name="relations">比較するリファレンスのシーケンス。</param>
            public void RemoveDuplicates(IEnumerable<Reference> relations)
            {
                this.References.ExceptWith(relations);
            }

            /// <summary>
            /// 指定したシーケンスと比較して、このプールと重複するタグを消去します。
            /// </summary>
            /// <param name="tags">比較するタグのシーケンス。</param>
            public void RemoveDuplicates(IEnumerable<Tag> tags)
            {
                this.Tags.ExceptWith(tags);
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
                    this.Accounts.RemoveWhere(a => a.EntityState != EntityState.Added);
                    this.Activities.RemoveWhere(a => a.EntityState != EntityState.Added);
                    this.Annotations.RemoveWhere(a => a.EntityState != EntityState.Added);
                    this.Relations.RemoveWhere(r => r.EntityState != EntityState.Added);
                    this.Marks.RemoveWhere(m => m.EntityState != EntityState.Added);
                    this.References.RemoveWhere(r => r.EntityState != EntityState.Added);
                    this.Tags.RemoveWhere(t => t.EntityState != EntityState.Added);
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

            public IEnumerable<Account> GetAccounts(IStorageObjectQuery<Account> query)
            {
                return query.Evaluate(this.Accounts.AsQueryable());
            }

            public IEnumerable<Activity> GetActivities(IStorageObjectQuery<Activity> query)
            {
                return query.Evaluate(this.Activities.AsQueryable());
            }

            public IEnumerable<Annotation> GetAnnotations(IStorageObjectQuery<Annotation> query)
            {
                return query.Evaluate(this.Annotations.AsQueryable());
            }

            public IEnumerable<Relation> GetRelations(IStorageObjectQuery<Relation> query)
            {
                return query.Evaluate(this.Relations.AsQueryable());
            }

            public IEnumerable<Mark> GetMarks(IStorageObjectQuery<Mark> query)
            {
                return query.Evaluate(this.Marks.AsQueryable());
            }

            public IEnumerable<Reference> GetReferences(IStorageObjectQuery<Reference> query)
            {
                return query.Evaluate(this.References.AsQueryable());
            }

            public IEnumerable<Tag> GetTags(IStorageObjectQuery<Tag> query)
            {
                return query.Evaluate(this.Tags.AsQueryable());
            }
        }
    }
}