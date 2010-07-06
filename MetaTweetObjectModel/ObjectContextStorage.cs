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
using System.Threading;
using System.Transactions;

namespace XSpect.MetaTweet.Objects
{
    /// <summary>
    /// オブジェクト コンテキストを保持し、ストレージ オブジェクトを管理する機能を提供します。
    /// </summary>
    public partial class ObjectContextStorage
        : Storage
    {
        private readonly ThreadLocal<Worker> _worker;

        private Func<Worker> _workerInitializer;

        public Worker CurrentWorker
        {
            get
            {
                return this._worker.Value;
            }
            set
            {
                this._worker.Value = value;
            }
        }

        public ObjectContextStorage()
        {
            this._worker = new ThreadLocal<Worker>();
        }

        /// <summary>
        /// バックエンドのデータソースとの接続を初期化します。既に接続が存在する場合は、新たに接続を初期化し直します。
        /// </summary>
        public virtual void InitializeContext()
        {
            if (this.CurrentWorker != null)
            {
                this.CurrentWorker.Dispose();
            }
            this._workerInitializer = () => new Worker(new StorageObjectContext());
        }

        /// <summary>
        /// バックエンドのデータソースとの接続を初期化します。既に接続が存在する場合は、新たに接続を初期化し直します。
        /// </summary>
        /// <param name="connectionString">接続に使用する文字列。</param>
        public virtual void InitializeContext(String connectionString)
        {
            if (this.CurrentWorker != null)
            {
                this.CurrentWorker.Dispose();
            }
            this._workerInitializer = () => new Worker(new StorageObjectContext(connectionString));
        }

        /// <summary>
        /// バックエンドのデータソースとの接続を初期化します。既に接続が存在する場合は、新たに接続を初期化し直します。
        /// </summary>
        /// <param name="connection">使用する接続。</param>
        public virtual void InitializeContext(EntityConnection connection)
        {
            if (this.CurrentWorker != null)
            {
                this.CurrentWorker.Dispose();
            }
            this._workerInitializer = () => new Worker(new StorageObjectContext(connection));
        }

        /// <summary>
        /// <see cref="Storage"/> によって使用されているアンマネージ リソースを解放し、オプションでマネージ リソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 <c>true</c>、破棄されない場合は <c>false</c>。</param>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing && this.CurrentWorker != null)
            {
                this.CurrentWorker.Dispose();
            }
            this._worker.Dispose();
            base.Dispose(disposing);
        }

        #region Account

        /// <summary>
        /// 値を指定してアカウントを検索します。
        /// </summary>
        /// <param name="accountId">アカウントの ID。指定しない場合は <c>null</c>。</param>
        /// <param name="realm">アカウントのレルム。指定しない場合は <c>null</c>。</param>
        /// <param name="seedString">アカウントのシード文字列。指定しない場合は <c>null</c>。</param>
        /// <returns>指定した条件に合致するアカウントのシーケンス。</returns>
        public override IEnumerable<Account> GetAccounts(
            String accountId,
            String realm,
            String seedString
        )
        {
            IQueryable<Account> accounts = this.CurrentWorker.Entities.Accounts;
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
            foreach (Account account in accounts)
            {
                if (account.Storage == null)
                {
                    account.Storage = this;
                }
            }
            return accounts
                .AsEnumerable()
                .Concat(this.CurrentWorker.AddingObjects.GetAccounts(accountId, realm, seedString))
                .AsTransparent();
        }

        /// <summary>
        /// 新しいアカウントを生成します。
        /// </summary>
        /// <param name="accountId">アカウントの ID。</param>
        /// <param name="realm">アカウントのレルム。</param>
        /// <param name="seeds">アカウントのシード値。</param>
        /// <param name="created">アカウントが新規に生成された場合は <c>true</c>。それ以外の場合、つまり既存のアカウントが取得された場合は <c>false</c> が返されます。このパラメータは初期化せずに渡されます。</param>
        /// <returns>生成されたアカウント。</returns>
        public override Account NewAccount(String accountId, String realm, IDictionary<String, String> seeds, out Boolean created)
        {
            Account account = this.GetAccounts(accountId).FirstOrDefault();
            if (account == null)
            {
                account = new Account(this)
                {
                    AccountId = accountId,
                    Realm = realm,
                    SeedString = Account.GetSeedString(seeds),
                };
                // BeginInit() must be called at StorageObject#.ctor(Storage).
                account.EndInit();
                this.CurrentWorker.Entities.Accounts.AddObject(account);
                this.CurrentWorker.AddingObjects.Add(account);
                created = true;
            }
            else
            {
                // Realm and SeedString is modifiable in Entity Object layer, but to modify them
                // causes incoherences between AccountId, not modifiable property.
                created = false;
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
        public override IEnumerable<Activity> GetActivities(
            String accountId,
            Nullable<DateTime> timestamp,
            String category,
            String subId,
            String userAgent,
            Object value,
            Object data
        )
        {
            IQueryable<Activity> activities = this.CurrentWorker.Entities.Activities;
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
            foreach(Activity activity in activities)
            {
                if (activity.Storage == null)
                {
                    activity.Storage = this;
                }
                if (!activity.AccountReference.IsLoaded)
                {
                    activity.AccountReference.Load();
                    activity.Account.Storage = this;
                }
            }
            return activities
                .AsEnumerable()
                .Concat(this.CurrentWorker.AddingObjects.GetActivities(
                    accountId,
                    timestamp,
                    category,
                    subId,
                    userAgent,
                    value,
                    data
                ))
                .AsTransparent();
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
        /// <param name="created">アクティビティが新規に生成された場合は <c>true</c>。それ以外の場合、つまり既存のアクティビティがそのまま、あるいは変更されて取得された場合は <c>false</c> が返されます。このパラメータは初期化せずに渡されます。</param>
        /// <returns>生成されたアクティビティ。</returns>
        /// <remarks>
        /// <para>同一の <paramref name="account"/>、<paramref name="category"/> および <paramref name="subId"/> を持つアクティビティが既に存在し、その中で <paramref name="timestamp"/> が最も近い (隣接する) アクティビティにおいて、その値が <paramref name="value"/> および <paramref name="data"/> と一致した場合、そのアクティビティが取得されます。取得されたアクティビティの <see cref="Activity.Timestamp"/> が <paramref name="timestamp"/> より新しい場合、<paramref name="timestamp"/> に書き換えられます。</para>
        /// <para>ここで、値が異なった場合に新しくアクティビティが作られるのは <paramref name="value"/> および <paramref name="data"/> であり (変更が累積される)、<paramref name="userAgent"/> 値が既存のアクティビティの <see cref="Activity.UserAgent"/> 値と異なっていてもそのまま上書きされます。</para>
        /// </remarks>
        public override Activity NewActivity(Account account, DateTime timestamp, String category, String subId, String userAgent, String value, Byte[] data, out Boolean created)
        {
            timestamp = timestamp.ToUniversalTime();
            IEnumerable<Activity> activities = this.GetActivities(account, null, category, subId);
            Activity activity = activities.SingleOrDefault(a => a.Timestamp == timestamp);
            if (activity == null)
            {
                if (activities.Any())
                {
                    Activity previous = activities
                            .Where(a => a.Timestamp < timestamp)
                            .OrderByDescending(a => a.Timestamp)
                            .FirstOrDefault();
                    Activity next = activities
                        .Where(a => a.Timestamp > timestamp)
                        .OrderBy(a => a.Timestamp)
                        .FirstOrDefault();
                    if (previous != null && previous.Value == value && previous.Data == data)
                    {
                        activity = previous;
                    }
                    else if (next != null && next.Value == value && next.Data == data)
                    {
                        activity = next;
                        activity.Timestamp = timestamp;
                    }
                    if (activity != null && activity.UserAgent != userAgent)
                    {
                        activity.UserAgent = userAgent;
                    }
                }
                if (activity == null)
                {
                    activity = new Activity(this)
                    {
                        AccountId = account.AccountId,
                        Timestamp = timestamp,
                        Category = category,
                        SubId = subId ?? String.Empty,
                        UserAgent = userAgent,
                        Value = value,
                        Data = data,
                        Account = account,
                    };
                    // BeginInit() must be called at StorageObject#.ctor(Storage).
                    activity.EndInit();
                    account.Activities.Add(activity);
                    this.CurrentWorker.Entities.Activities.AddObject(activity);
                    this.CurrentWorker.AddingObjects.Add(activity);
                    created = true;
                }
                else
                {
                    created = false;
                }
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
                created = false;
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
        /// <param name="value">アノテーションの値。指定しない場合は <c>null</c>。</param>
        /// <returns>指定した条件に合致するアノテーションのシーケンス。</returns>
        public override IEnumerable<Annotation> GetAnnotations(
            String accountId,
            String name,
            String value
        )
        {
            IQueryable<Annotation> annotations = this.CurrentWorker.Entities.Annotations;
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
            foreach (Annotation annotation in annotations)
            {
                if (annotation.Storage == null)
                {
                    annotation.Storage = this;
                }
                if (!annotation.AccountReference.IsLoaded)
                {
                    annotation.AccountReference.Load();
                    annotation.Account.Storage = this;
                }
            }
            return annotations
                .AsEnumerable()
                .Concat(this.CurrentWorker.AddingObjects.GetAnnotations(accountId, name, value))
                .AsTransparent();
        }

        /// <summary>
        /// 新しいアノテーションを生成します。
        /// </summary>
        /// <param name="account">アノテーションが関連付けられるアカウント。</param>
        /// <param name="name">アノテーションの意味。</param>
        /// <param name="value">アノテーションの値。</param>
        /// <param name="created">アノテーションが新規に生成された場合は <c>true</c>。それ以外の場合、つまり既存のアノテーションが取得された場合は <c>false</c> が返されます。このパラメータは初期化せずに渡されます。</param>
        /// <returns>生成されたアノテーション。</returns>
        public override Annotation NewAnnotation(Account account, String name, String value, out Boolean created)
        {
            Annotation annotation = this.GetAnnotations(account, name, value).FirstOrDefault();
            if (annotation == null)
            {
                annotation = new Annotation(this)
                {
                    AccountId = account.AccountId,
                    Name = name,
                    Value = value,
                    Account = account,
                };
                // BeginInit() must be called at StorageObject#.ctor(Storage).
                annotation.EndInit();
                account.Annotations.Add(annotation);
                this.CurrentWorker.Entities.Annotations.AddObject(annotation);
                this.CurrentWorker.AddingObjects.Add(annotation);
                created = true;
            }
            else
            {
                created = false;
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
        public override IEnumerable<Relation> GetRelations(
            String accountId,
            String name,
            String relatingAccountId
        )
        {
            IQueryable<Relation> relations = this.CurrentWorker.Entities.Relations;
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
            foreach (Relation relation in relations)
            {
                if (relation.Storage == null)
                {
                    relation.Storage = this;
                }
                if (!relation.AccountReference.IsLoaded)
                {
                    relation.AccountReference.Load();
                    relation.Account.Storage = this;
                }
            }
            return relations
                .AsEnumerable()
                .Concat(this.CurrentWorker.AddingObjects.GetRelations(accountId, name, relatingAccountId))
                .AsTransparent();
        }

        /// <summary>
        /// 新しいリレーションを生成します。
        /// </summary>
        /// <param name="account">リレーションが関連付けられるアカウント。</param>
        /// <param name="name">リレーションの意味。</param>
        /// <param name="relatingAccount">リレーションが関連付けられる先のアカウント。</param>
        /// <param name="created">リレーションが新規に生成された場合は <c>true</c>。それ以外の場合、つまり既存のリレーションが取得された場合は <c>false</c> が返されます。このパラメータは初期化せずに渡されます。</param>
        /// <returns>生成されたリレーション。</returns>
        public override Relation NewRelation(Account account, String name, Account relatingAccount, out Boolean created)
        {
            Relation relation = this.GetRelations(account, name, relatingAccount).FirstOrDefault();
            if (relation == null)
            {
                relation = new Relation(this)
                {
                    AccountId = account.AccountId,
                    Name = name,
                    RelatingAccountId = relatingAccount.AccountId,
                    Account = account,
                    RelatingAccount = relatingAccount,
                };
                // BeginInit() must be called at StorageObject#.ctor(Storage).
                relation.EndInit();
                account.Relations.Add(relation);
                this.CurrentWorker.Entities.Relations.AddObject(relation);
                this.CurrentWorker.AddingObjects.Add(relation);
                created = true;
            }
            else
            {
                created = false;
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
            String accountId,
            String name,
            String markingAccountId,
            Nullable<DateTime> markingTimestamp,
            String markingCategory,
            String markingSubId
        )
        {
            IQueryable<Mark> marks = this.CurrentWorker.Entities.Marks;
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
                DateTime rvalue = markingTimestamp.Value.ToUniversalTime();
                marks = marks.Where(m => m.MarkingTimestamp == rvalue);
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
                if (!mark.AccountReference.IsLoaded)
                {
                    mark.AccountReference.Load();
                    mark.Account.Storage = this;
                }
                if (!mark.MarkingActivityReference.IsLoaded)
                {
                    mark.MarkingActivityReference.Load();
                    mark.MarkingActivity.Storage = this;
                }
            }
            return marks
                .AsEnumerable()
                .Concat(this.CurrentWorker.AddingObjects.GetMarks(
                    accountId,
                    name,
                    markingAccountId,
                    markingTimestamp,
                    markingCategory,
                    markingSubId
                ))
                .AsTransparent();
        }

        /// <summary>
        /// 新しいマークを生成します。
        /// </summary>
        /// <param name="account">マークが関連付けられるアカウント。</param>
        /// <param name="name">マークの意味。</param>
        /// <param name="markingActivity">マークが関連付けられる先のアクティビティ。</param>
        /// <param name="created">マークが新規に生成された場合は <c>true</c>。それ以外の場合、つまり既存のマークが取得された場合は <c>false</c> が返されます。このパラメータは初期化せずに渡されます。</param>
        /// <returns>生成されたマーク。</returns>
        public override Mark NewMark(Account account, String name, Activity markingActivity, out Boolean created)
        {
            Mark mark = this.GetMarks(account, name, markingActivity).FirstOrDefault();
            if (mark == null)
            {
                mark = new Mark(this)
                {
                    AccountId = account.AccountId,
                    Name = name,
                    MarkingAccountId = markingActivity.AccountId,
                    MarkingTimestamp = markingActivity.Timestamp,
                    MarkingCategory = markingActivity.Category,
                    MarkingSubId = markingActivity.SubId,
                    Account = account,
                    MarkingActivity = markingActivity,
                };
                // BeginInit() must be called at StorageObject#.ctor(Storage).
                mark.EndInit();
                account.Marks.Add(mark);
                markingActivity.Marks.Add(mark);
                this.CurrentWorker.Entities.Marks.AddObject(mark);
                this.CurrentWorker.AddingObjects.Add(mark);
                created = true;
            }
            else
            {
                created = false;
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
            IQueryable<Reference> references = this.CurrentWorker.Entities.References;
            if (accountId != null)
            {
                references = references.Where(r => r.AccountId == accountId);
            }
            if (timestamp.HasValue)
            {
                DateTime rvalue = timestamp.Value.ToUniversalTime();
                references = references.Where(r => r.Timestamp == rvalue);
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
                DateTime rvalue = referringTimestamp.Value.ToUniversalTime();
                references = references.Where(r => r.ReferringTimestamp == rvalue);
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
                if (!reference.ActivityReference.IsLoaded)
                {
                    reference.ActivityReference.Load();
                    reference.Activity.Storage = this;
                }
            }
            return references
                .AsEnumerable()
                .Concat(this.CurrentWorker.AddingObjects.GetReferences(
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
                .AsTransparent();
        }

        /// <summary>
        /// 新しいリファレンスを生成します。
        /// </summary>
        /// <param name="activity">リファレンスが関連付けられているアクティビティ。</param>
        /// <param name="name">リファレンスの意味。</param>
        /// <param name="referringActivity">リファレンスが関連付けられる先のアクティビティ。</param>
        /// <param name="created">リファレンスが新規に生成された場合は <c>true</c>。それ以外の場合、つまり既存のリファレンスが取得された場合は <c>false</c> が返されます。このパラメータは初期化せずに渡されます。</param>
        /// <returns>生成されたリファレンス。</returns>
        public override Reference NewReference(Activity activity, String name, Activity referringActivity, out Boolean created)
        {
            Reference reference = this.GetReferences(activity, name, referringActivity).FirstOrDefault();
            if (reference == null)
            {
                reference = new Reference(this)
                {
                    AccountId = activity.AccountId,
                    Timestamp = activity.Timestamp,
                    Category = activity.Category,
                    SubId = activity.SubId,
                    Name = name,
                    ReferringAccountId = referringActivity.AccountId,
                    ReferringTimestamp = referringActivity.Timestamp,
                    ReferringCategory = referringActivity.Category,
                    ReferringSubId = referringActivity.SubId,
                    Activity = activity,
                    ReferringActivity = referringActivity,
                };
                // BeginInit() must be called at StorageObject#.ctor(Storage).
                reference.EndInit();
                activity.References.Add(reference);
                this.CurrentWorker.Entities.References.AddObject(reference);
                this.CurrentWorker.AddingObjects.Add(reference);
                created = true;
            }
            else
            {
                created = false;
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
        /// <param name="value">タグの値。指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するタグのシーケンス。</returns>
        public override IEnumerable<Tag> GetTags(
            String accountId,
            Nullable<DateTime> timestamp,
            String category,
            String subId,
            String name,
            String value
        )
        {
            IQueryable<Tag> tags = this.CurrentWorker.Entities.Tags;
            if (accountId != null)
            {
                tags = tags.Where(t => t.AccountId == accountId);
            }
            if (timestamp.HasValue)
            {
                DateTime rvalue = timestamp.Value.ToUniversalTime();
                tags = tags.Where(t => t.Timestamp == rvalue);
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
            if (name != null)
            {
                tags = tags.Where(t => t.Value == value);
            }
            foreach (Tag tag in tags)
            {
                if (tag.Storage == null)
                {
                    tag.Storage = this;
                }
                if (!tag.ActivityReference.IsLoaded)
                {
                    tag.ActivityReference.Load();
                    tag.Activity.Storage = this;
                }
            }
            return tags
                .AsEnumerable()
                .Concat(this.CurrentWorker.AddingObjects.GetTags(accountId, timestamp, category, subId, name, value))
                .AsTransparent();
        }

        /// <summary>
        /// 新しいタグを生成します。
        /// </summary>
        /// <param name="activity">タグが関連付けられるアクティビティ。</param>
        /// <param name="name">タグの意味。</param>
        /// <param name="value">タグの値。</param>
        /// <param name="created">タグが新規に生成された場合は <c>true</c>。それ以外の場合、つまり既存のタグが取得された場合は <c>false</c> が返されます。このパラメータは初期化せずに渡されます。</param>
        /// <returns>生成されたタグ。</returns>
        public override Tag NewTag(Activity activity, String name, String value, out Boolean created)
        {
            Tag tag = this.GetTags(activity, name, value).FirstOrDefault();
            if (tag == null)
            {
                tag = new Tag(this)
                {
                    AccountId = activity.AccountId,
                    Timestamp = activity.Timestamp,
                    Category = activity.Category,
                    SubId = activity.SubId,
                    Name = name,
                    Activity = activity,
                };
                // BeginInit() must be called at StorageObject#.ctor(Storage).
                tag.EndInit();
                activity.Tags.Add(tag);
                this.CurrentWorker.Entities.Tags.AddObject(tag);
                this.CurrentWorker.AddingObjects.Add(tag);
                created = true;
            }
            else
            {
                created = false;
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
            this.CurrentWorker.Entities.AttachTo(GetEntitySetName(obj), obj);
        }

        /// <summary>
        /// ストレージ オブジェクトをストレージからデタッチします。
        /// </summary>
        /// <param name="obj">デタッチするストレージ オブジェクト。</param>
        public override void DetachObject(StorageObject obj)
        {
            if (obj.Storage != this)
            {
                throw new ArgumentException("Invalid StorageObject: Different Storage.", "obj");
            }
            if (obj.EntityState == EntityState.Added)
            {
                this.CurrentWorker.AddingObjects.Remove(obj);
            }
            this.CurrentWorker.Entities.Detach(obj);
        }

        /// <summary>
        /// ストレージ オブジェクトを削除の対象としてマークします。
        /// </summary>
        /// <param name="obj">削除の対象としてマークするストレージ オブジェクト。</param>
        public override void DeleteObject(StorageObject obj)
        {
            if (obj.Storage != this)
            {
                throw new ArgumentException("Invalid StorageObject: Different Storage.", "obj");
            }
            if (obj.EntityState == EntityState.Added)
            {
                this.CurrentWorker.AddingObjects.Remove(obj);
            }
            this.CurrentWorker.Entities.DeleteObject(obj);
        }

        /// <summary>
        /// ストレージ オブジェクトをデータ ソース内のデータで更新します。
        /// </summary>
        /// <param name="refreshMode">更新モードを表す値。</param>
        /// <param name="obj">更新するストレージ オブジェクト。</param>
        public override void RefreshObject(RefreshMode refreshMode, StorageObject obj)
        {
            this.CurrentWorker.Entities.Refresh(refreshMode, obj);
        }

        /// <summary>
        /// ストレージ オブジェクトの変更をデータ ソースに保存します。
        /// </summary>
        /// <returns>データ ソースにおいて処理が行われた行数。</returns>
        public override Int32 Update()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    Int32 ret = this.CurrentWorker.Entities.SaveChanges();
                    this.CurrentWorker.AddingObjects.Clear();
                    scope.Complete();
                    return ret;
                }
                catch (UpdateException ex)
                {
                    // Provision of problem about Added-state entities which is already inserted
                    // by another context (causes constraint violation).
                    foreach (ObjectStateEntry entry in ex.StateEntries)
                    {
                        entry.AcceptChanges();
                    }
                    throw;
                }
            }
        }

        public Int32 TryUpdate()
        {
            return this.TryUpdate(3, null, true);
        }

        public Int32 TryUpdate(Nullable<Int32> tryingCount, Nullable<TimeSpan> tryingTime, Boolean throwIfFailed)
        {
            Int32 ret = -1;
            Exception exception = null;
            DateTime start = DateTime.UtcNow;
            for (
                Int32 i = 0;
                ret < 0 && (
                    (!tryingCount.HasValue || i < tryingCount) ||
                    (!tryingTime.HasValue || tryingTime <= DateTime.Now - start)
                );
                ++i
            )
            {
                try
                {
                    ret = this.Update();
                    exception = null;
                }
                catch (Exception ex)
                {
                    exception = ex;
                    Thread.Sleep(500);
                }
            }
            if (exception != null && throwIfFailed)
            {
                throw exception;
            }
            return ret;
        }

        public void BeginWorkerScope()
        {
            this.BeginWorkerScope(true);
        }

        public void BeginWorkerScope(Boolean checkState)
        {
            if (this.CurrentWorker != null)
            {
                if (checkState)
                {
                    throw new InvalidOperationException("Already in Worker context.");
                }
                return;
            }
            this.CurrentWorker = this._workerInitializer();
        }

        public void EndWorkerScope()
        {
            this.EndWorkerScope(true);
        }

        public void EndWorkerScope(Boolean checkState)
        {
            if (this.CurrentWorker == null)
            {
                if (checkState)
                {
                    throw new InvalidOperationException("Not in worker context.");
                }
                return;
            }
            this.CurrentWorker.Dispose();
            this.CurrentWorker = null;
        }

        public void Execute(Action<ObjectContextStorage> body)
        {
            this.BeginWorkerScope();
            try
            {
                body(this);
            }
            finally
            {
                this.EndWorkerScope();
            }
        }

        private static String GetEntitySetName(StorageObject obj)
        {
            switch (obj.ObjectType)
            {
                case StorageObjectTypes.Account:
                    return "Accounts";
                case StorageObjectTypes.Activity:
                    return "Activitys";
                case StorageObjectTypes.Annotation:
                    return "Annotations";
                case StorageObjectTypes.Relation:
                    return "Relations";
                case StorageObjectTypes.Mark:
                    return "Marks";
                case StorageObjectTypes.Reference:
                    return "References";
                default: // case StorageObjectTypes.Tag:
                    return "Tags";
            }
        }
    }
}