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
using System.Data.EntityClient;
using System.Data.Objects;
using System.Linq;

namespace XSpect.MetaTweet.Objects
{
    /// <summary>
    /// オブジェクト コンテキストを保持し、ストレージ オブジェクトを管理する機能を提供します。
    /// </summary>
    public class ObjectContextStorage
        : Storage
    {
        /// <summary>
        /// このストレージのキャッシュを取得または設定します。
        /// </summary>
        /// <value>このストレージのキャッシュ。</value>
        public override StorageCache Cache
        {
            get;
            set;
        }

        /// <summary>
        /// このストレージが保持しているオブジェクト コンテキストを取得します。
        /// </summary>
        /// <value>The entities.</value>
        public StorageObjectContext Entities
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="ObjectContextStorage"/> の新しいインスタンスを初期化します。
        /// </summary>
        public ObjectContextStorage()
        {
            this.Cache = new StorageCache(this);
        }

        /// <summary>
        /// バックエンドのデータソースとの接続を初期化します。
        /// </summary>
        public virtual void Initialize()
        {
            this.Entities = new StorageObjectContext();
        }

        /// <summary>
        /// バックエンドのデータソースとの接続を初期化します。
        /// </summary>
        /// <param name="connectionString">接続に使用する文字列。</param>
        public virtual void Initialize(String connectionString)
        {
            this.Entities = new StorageObjectContext(connectionString);
        }

        /// <summary>
        /// バックエンドのデータソースとの接続を初期化します。
        /// </summary>
        /// <param name="connection">使用する接続。</param>
        public virtual void Initialize(EntityConnection connection)
        {
            this.Entities = new StorageObjectContext(connection);
        }

        /// <summary>
        /// <see cref="Storage"/> によって使用されているアンマネージ リソースを解放し、オプションでマネージ リソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 <c>true</c>、破棄されない場合は <c>false</c>。</param>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                this.Entities.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Account

        /// <summary>
        /// 値を指定してアカウントを検索します。
        /// </summary>
        /// <param name="accountId">アカウントの ID。指定しない場合は <c>null</c>。</param>
        /// <param name="realm">アカウントのレルム。指定しない場合は <c>null</c>。</param>
        /// <returns>指定した条件に合致するアカウントのシーケンス。</returns>
        public override IEnumerable<Account> GetAccounts(
            Nullable<Guid> accountId,
            String realm
        )
        {
            IQueryable<Account> accounts = this.Entities.AccountSet;
            if (accountId.HasValue)
            {
                accounts = accounts.Where(a => a.AccountId == accountId);
            }
            if (realm != null)
            {
                accounts = accounts.Where(a => a.Realm == realm);
            }
            foreach (Account account in accounts)
            {
                if (account.Storage == null)
                {
                    account.Storage = this;
                }
            }
            return accounts
                .AsEnumerable()
                .Concat(this.Cache.AddingObjects.GetAccounts(accountId, realm))
                .ToList();
        }

        /// <summary>
        /// 新しいアカウントを生成します。
        /// </summary>
        /// <param name="accountId">アカウントの ID。</param>
        /// <param name="realm">アカウントのレルム。</param>
        /// <returns>生成されたアカウント。</returns>
        public override Account NewAccount(Guid accountId, String realm)
        {
            Account account = this.GetAccounts(accountId).SingleOrDefault();
            if (account == null)
            {
                account = new Account(this)
                {
                    AccountId = accountId,
                    Realm = realm,
                };
                // BeginInit() must be called at StorageObject#.ctor(Storage).
                account.EndInit();
                this.Entities.AddToAccountSet(account);
                this.Cache.AddingObjects.Add(account);
            }
            else
            {
                if (account.Realm != realm)
                {
                    account.Realm = realm;
                }
            }
            return account;
        }

        #endregion

        #region Activity

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
        protected internal override IEnumerable<Activity> GetActivities(
            Nullable<Guid> accountId,
            Nullable<DateTime> timestamp,
            String category,
            String subId,
            String userAgent,
            Object value,
            Object data
        )
        {
            IQueryable<Activity> activities = this.Entities.ActivitySet;
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
            foreach(Activity activity in activities)
            {
                if (activity.Storage == null)
                {
                    activity.Storage = this;
                }
            }
            return activities
                .AsEnumerable()
                .Concat(this.Cache.AddingObjects.GetActivities(
                    accountId,
                    timestamp,
                    category,
                    subId,
                    userAgent,
                    value,
                    data
                ))
                .ToList();
        }

        /// <summary>
        /// 新しいアクティビティを生成します。
        /// </summary>
        /// <param name="account">アクティビティを行うアカウント。</param>
        /// <param name="timestamp">アクティビティのタイムスタンプ。</param>
        /// <param name="category">アクティビティのカテゴリ。</param>
        /// <param name="subId">アクティビティのサブ ID。</param>
        /// <param name="userAgent">アクティビティのユーザ エージェント。</param>
        /// <param name="value">アクティビティの値。</param>
        /// <param name="data">アクティビティのデータ。</param>
        /// <returns>生成されたアクティビティ。</returns>
        public override Activity NewActivity(Account account, DateTime timestamp, String category, String subId, String userAgent, String value, Byte[] data)
        {
            Activity activity = this.GetActivities(account, timestamp, category, subId).SingleOrDefault();
            if (activity == null)
            {
                activity = new Activity(this)
                {
                    Account = account,
                    Timestamp = timestamp,
                    Category = category,
                    SubId = subId ?? String.Empty,
                    UserAgent = userAgent,
                    Value = value,
                    Data = data,
                };
                // BeginInit() must be called at StorageObject#.ctor(Storage).
                activity.EndInit();
                this.Entities.AddToActivitySet(activity);
                this.Cache.AddingObjects.Add(activity);
            }
            else
            {
                if (activity.UserAgent != userAgent)
                {
                    activity.UserAgent = userAgent;
                }
                if (activity.Value != value)
                {
                    activity.Value = value;
                }
                if (activity.Data != data)
                {
                    activity.Data = data;
                }
            }
            return activity;
        }

        #endregion

        #region Annotation

        /// <summary>
        /// 値を指定してアノテーションを検索します。
        /// </summary>
        /// <param name="accountId">アノテーションが関連付けられているアカウントの ID。指定しない場合は <c>null</c>。</param>
        /// <param name="name">アノテーションの意味。指定しない場合は <c>null</c>。</param>
        /// <returns>指定した条件に合致するアノテーションのシーケンス。</returns>
        protected internal override IEnumerable<Annotation> GetAnnotations(
            Nullable<Guid> accountId,
            String name
        )
        {
            IQueryable<Annotation> annotations = this.Entities.AnnotationSet;
            if (accountId.HasValue)
            {
                annotations = annotations.Where(a => a.AccountId == accountId);
            }
            if (name != null)
            {
                annotations = annotations.Where(a => a.Name == name);
            }
            foreach (Annotation annotation in annotations)
            {
                if (annotation.Storage == null)
                {
                    annotation.Storage = this;
                }
            }
            return annotations
                .AsEnumerable()
                .Concat(this.Cache.AddingObjects.GetAnnotations(accountId, name))
                .ToList();
        }

        /// <summary>
        /// 新しいアノテーションを生成します。
        /// </summary>
        /// <param name="account">アノテーションが関連付けられるアカウント。</param>
        /// <param name="name">アノテーションの意味。</param>
        /// <returns>生成されたアノテーション。</returns>
        public override Annotation NewAnnotation(Account account, String name)
        {
            Annotation annotation = this.GetAnnotations(account, name).SingleOrDefault();
            if (annotation == null)
            {
                annotation = new Annotation(this)
                {
                    Account = account,
                    Name = name,
                };
                // BeginInit() must be called at StorageObject#.ctor(Storage).
                annotation.EndInit();
                this.Entities.AddToAnnotationSet(annotation);
                this.Cache.AddingObjects.Add(annotation);
            }
            return annotation;
        }

        #endregion

        #region Relation

        /// <summary>
        /// 値を指定してリレーションを検索します。
        /// </summary>
        /// <param name="accountId">リレーションが関連付けられているアカウントの ID。指定しない場合は <c>null</c>。</param>
        /// <param name="name">リレーションの意味。</param>
        /// <param name="relatingAccountId">リレーションが関連付けられる先のアカウントの ID。指定しない場合は <c>null</c>。</param>
        /// <returns>指定した条件に合致するリレーションのシーケンス。</returns>
        protected internal override IEnumerable<Relation> GetRelations(
            Nullable<Guid> accountId,
            String name,
            Nullable<Guid> relatingAccountId
        )
        {
            IQueryable<Relation> relations = this.Entities.RelationSet;
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
            foreach (Relation relation in relations)
            {
                if (relation.Storage == null)
                {
                    relation.Storage = this;
                }
            }
            return relations
                .AsEnumerable()
                .Concat(this.Cache.AddingObjects.GetRelations(accountId, name, relatingAccountId))
                .ToList();
        }

        /// <summary>
        /// 新しいリレーションを生成します。
        /// </summary>
        /// <param name="account">リレーションが関連付けられるアカウント。</param>
        /// <param name="name">リレーションの意味。</param>
        /// <param name="relatingAccount">リレーションが関連付けられる先のアカウント。</param>
        /// <returns>生成されたリレーション。</returns>
        public override Relation NewRelation(Account account, String name, Account relatingAccount)
        {
            Relation relation = this.GetRelations(account, name, relatingAccount).SingleOrDefault();
            if (relation == null)
            {
                relation = new Relation(this)
                {
                    Account = account,
                    Name = name,
                    RelatingAccount = relatingAccount,
                };
                // BeginInit() must be called at StorageObject#.ctor(Storage).
                relation.EndInit();
                this.Entities.AddToRelationSet(relation);
                this.Cache.AddingObjects.Add(relation);
            }
            return relation;
        }

        #endregion

        #region Mark

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
        public override IEnumerable<Mark> GetMarks(
            Nullable<Guid> accountId,
            String name,
            Nullable<Guid> markingAccountId,
            Nullable<DateTime> markingTimestamp,
            String markingCategory,
            String markingSubId
        )
        {
            IQueryable<Mark> marks = this.Entities.MarkSet;
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
            foreach (Mark mark in marks)
            {
                if (mark.Storage == null)
                {
                    mark.Storage = this;
                }
            }
            return marks
                .AsEnumerable()
                .Concat(this.Cache.AddingObjects.GetMarks(
                    accountId,
                    name,
                    markingAccountId,
                    markingTimestamp,
                    markingCategory,
                    markingSubId
                ))
                .ToList();
        }

        /// <summary>
        /// 新しいマークを生成します。
        /// </summary>
        /// <param name="account">マークが関連付けられるアカウント。</param>
        /// <param name="name">マークの意味。</param>
        /// <param name="markingActivity">マークが関連付けられる先のアクティビティ。</param>
        /// <returns>生成されたマーク。</returns>
        public override Mark NewMark(Account account, String name, Activity markingActivity)
        {
            Mark mark = this.GetMarks(account, name, markingActivity).SingleOrDefault();
            if (mark == null)
            {
                mark = new Mark(this)
                {
                    Account = account,
                    Name = name,
                    MarkingActivity = markingActivity,
                };
                // BeginInit() must be called at StorageObject#.ctor(Storage).
                mark.EndInit();
                this.Entities.AddToMarkSet(mark);
                this.Cache.AddingObjects.Add(mark);
            }
            return mark;
        }

        #endregion

        #region Reference

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
        public override IEnumerable<Reference> GetReferences(
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
            IQueryable<Reference> references = this.Entities.ReferenceSet;
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
            foreach (Reference reference in references)
            {
                if (reference.Storage == null)
                {
                    reference.Storage = this;
                }
            }
            return references
                .AsEnumerable()
                .Concat(this.Cache.AddingObjects.GetReferences(
                    accountId,
                    timestamp,
                    category,
                    subId,
                    name,
                    referringAccountId,
                    referringTimestamp,
                    referringCategory,
                    referringSubId
                ))
                .ToList();
        }

        /// <summary>
        /// 新しいリファレンスを生成します。
        /// </summary>
        /// <param name="activity">リファレンスが関連付けられているアクティビティ。</param>
        /// <param name="name">リファレンスの意味。</param>
        /// <param name="referringActivity">リファレンスが関連付けられる先のアクティビティ。</param>
        /// <returns>生成されたリファレンス。</returns>
        public override Reference NewReference(Activity activity, String name, Activity referringActivity)
        {
            Reference reference = this.GetReferences(activity, name, referringActivity).SingleOrDefault();
            if (reference == null)
            {
                reference = new Reference(this)
                {
                    Activity = activity,
                    Name = name,
                    ReferringActivity = referringActivity,
                };
                // BeginInit() must be called at StorageObject#.ctor(Storage).
                reference.EndInit();
                this.Entities.AddToReferenceSet(reference);
                this.Cache.AddingObjects.Add(reference);
            }
            return reference;
        }

        #endregion

        #region Tag

        /// <summary>
        /// 値を指定してタグを検索します。
        /// </summary>
        /// <param name="accountId">タグが関連付けられているアクティビティを行ったアカウントの ID。指定しない場合は <c>null</c>。</param>
        /// <param name="timestamp">タグが関連付けられているアクティビティのタイムスタンプ。指定しない場合は <c>null</c>。</param>
        /// <param name="category">タグが関連付けられているアクティビティのカテゴリ。指定しない場合は <c>null</c>。</param>
        /// <param name="subId">タグが関連付けられているアクティビティのサブ ID。指定しない場合は <c>null</c>。</param>
        /// <param name="name">タグの意味。指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するタグのシーケンス。</returns>
        public override IEnumerable<Tag> GetTags(
            Nullable<Guid> accountId,
            Nullable<DateTime> timestamp,
            String category,
            String subId,
            String name
        )
        {
            IQueryable<Tag> tags = this.Entities.TagSet;
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
            foreach (Tag tag in tags)
            {
                if (tag.Storage == null)
                {
                    tag.Storage = this;
                }
            }
            return tags
                .AsEnumerable()
                .Concat(this.Cache.AddingObjects.GetTags(accountId, timestamp, category, subId, name))
                .ToList();
        }

        /// <summary>
        /// 新しいタグを生成します。
        /// </summary>
        /// <param name="activity">タグが関連付けられるアクティビティ。</param>
        /// <param name="name">タグの意味。</param>
        /// <returns>生成されたタグ。</returns>
        public override Tag NewTag(Activity activity, String name)
        {
            Tag tag = this.GetTags(activity, name).SingleOrDefault();
            if (tag == null)
            {
                tag = new Tag(this)
                {
                    Activity = activity,
                    Name = name,
                };
                // BeginInit() must be called at StorageObject#.ctor(Storage).
                tag.EndInit();
                this.Entities.AddToTagSet(tag);
                this.Cache.AddingObjects.Add(tag);
            }
            return tag;
        }

        #endregion

        /// <summary>
        /// ストレージ オブジェクトをストレージにアタッチします。
        /// </summary>
        /// <param name="obj">アタッチするストレージ オブジェクト。</param>
        public override void AttachObject(StorageObject obj)
        {
            this.Entities.Attach(obj);
        }

        /// <summary>
        /// ストレージ オブジェクトをストレージからデタッチします。
        /// </summary>
        /// <param name="obj">デタッチするストレージ オブジェクト。</param>
        public override void DetachObject(StorageObject obj)
        {
            this.Entities.Detach(obj);
        }

        /// <summary>
        /// ストレージ オブジェクトを削除の対象としてマークします。
        /// </summary>
        /// <param name="obj">削除の対象としてマークするストレージ オブジェクト。</param>
        public override void DeleteObject(StorageObject obj)
        {
            this.Entities.DeleteObject(obj);
        }

        /// <summary>
        /// ストレージ オブジェクトをデータ ソース内のデータで更新します。
        /// </summary>
        /// <param name="refreshMode">更新モードを表す値。</param>
        /// <param name="obj">更新するストレージ オブジェクト。</param>
        public override void RefreshObject(RefreshMode refreshMode, StorageObject obj)
        {
            this.Entities.Refresh(refreshMode, obj);
        }

        /// <summary>
        /// ストレージ オブジェクトの変更をデータ ソースに保存します。
        /// </summary>
        /// <returns>データ ソースにおいて処理が行われた行数。</returns>
        public override Int32 Update()
        {
            Int32 ret = this.Entities.SaveChanges();
            this.Cache.AddingObjects.Clear();
            return ret;
        }
    }
}