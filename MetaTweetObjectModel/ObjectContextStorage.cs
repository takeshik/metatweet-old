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
using System.Collections;
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

        /// <summary>
        /// 現在のスレッドにおけるワーカー オブジェクトを取得します。
        /// </summary>
        /// <value>
        /// 現在のスレッドにおけるワーカー オブジェクト。
        /// </value>
        public Worker CurrentWorker
        {
            get
            {
                return this._worker.Value;
            }
            private set
            {
                this._worker.Value = value;
            }
        }

        /// <summary>
        /// <see cref="ObjectContextStorage"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
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
            this._workerInitializer = () => new Worker(new StorageObjectContext()
            {
                MergeOption = MergeOption.PreserveChanges,
            });
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
            this._workerInitializer = () => new Worker(new StorageObjectContext(connectionString)
            {
                MergeOption = MergeOption.PreserveChanges,
            });
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
            this._workerInitializer = () => new Worker(new StorageObjectContext(connection)
            {
                MergeOption = MergeOption.PreserveChanges,
            });
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

        public override IEnumerable<Account> GetAccounts(StorageObjectQuery<Account, AccountTuple> query)
        {
            IEnumerable<Account> accounts = (query ?? new StorageObjectQuery<Account, AccountTuple>())
                .Evaluate(this.CurrentWorker.Entities.Accounts).ToArray();
            foreach (Account account in accounts)
            {
                this.InternAll(account);
            }
            this.CurrentWorker.AddingObjects.RemoveDuplicates(accounts);
            return accounts
                .Concat(this.CurrentWorker.AddingObjects.GetAccounts(query))
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
                    Context = this.CurrentWorker.Entities,
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

        /// <summary>
        /// アカウントおよび関連するオブジェクトをこのストレージにアタッチし、管理下に置きます。
        /// </summary>
        /// <param name="account">このストレージに所属させるアカウント。</param>
        public void InternAll(Account account)
        {
            this.Intern(account);
            foreach (StorageObject obj in ((IEnumerable<StorageObject>) account.Activities)
                .Concat(account.Annotations)
                .Concat(account.Relations)
                .Concat(account.Marks)
            )
            {
                this.Intern(obj);
            }
        }
        
        /// <summary>
        /// 指定したアカウントをこのストレージ上にマージし、管理下に置かれた同一内容のオブジェクトを返します。
        /// </summary>
        /// <param name="account">マージするアカウント。</param>
        /// <returns>このストレージの管理下に置かれた同一内容のアカウント。</returns>
        public Account GetInterned(Account account)
        {
            return this.GetInterned(account, this.Merge);
        }

        #endregion

        #region Activity

        public override IEnumerable<Activity> GetActivities(StorageObjectQuery<Activity, ActivityTuple> query)
        {
            IEnumerable<Activity> activities = (query ?? new StorageObjectQuery<Activity, ActivityTuple>())
                .Evaluate(this.CurrentWorker.Entities.Activities).ToArray();
            foreach (Activity activity in activities)
            {
                this.InternAll(activity);
            }
            this.CurrentWorker.AddingObjects.RemoveDuplicates(activities);
            return activities
                .Concat(this.CurrentWorker.AddingObjects.GetActivities(query))
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
            account = this.GetInterned(account);
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
                        // TODO: Below code causes exception; consider the alternative way or necessity
                        // activity.Timestamp = timestamp;
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
                        Context = this.CurrentWorker.Entities,
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

        /// <summary>
        /// アクティビティおよび関連するオブジェクトをこのストレージにアタッチし、管理下に置きます。
        /// </summary>
        /// <param name="activity">このストレージに所属させるアクティビティ。</param>
        public void InternAll(Activity activity)
        {
            this.Intern(activity);
            this.Intern(activity.Account);
            foreach (StorageObject obj in ((IEnumerable<StorageObject>) activity.Tags)
                .Concat(activity.References)
                .Concat(activity.Marks)
            )
            {
                this.Intern(obj);
            }
        }

        /// <summary>
        /// 指定したアクティビティをこのストレージ上にマージし、管理下に置かれた同一内容のオブジェクトを返します。
        /// </summary>
        /// <param name="activity">マージするアクティビティ。</param>
        /// <returns>このストレージの管理下に置かれた同一内容のアクティビティ。</returns>
        public Activity GetInterned(Activity activity)
        {
            return this.GetInterned(activity, this.Merge);
        }

        #endregion

        #region Annotation

        public override IEnumerable<Annotation> GetAnnotations(StorageObjectQuery<Annotation, AnnotationTuple> query)
        {
            IEnumerable<Annotation> annotations = (query ?? new StorageObjectQuery<Annotation, AnnotationTuple>())
                .Evaluate(this.CurrentWorker.Entities.Annotations).ToArray();
            foreach (Annotation annotation in annotations)
            {
                this.InternAll(annotation);
            }
            this.CurrentWorker.AddingObjects.RemoveDuplicates(annotations);
            return annotations
                .Concat(this.CurrentWorker.AddingObjects.GetAnnotations(query))
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
            account = this.GetInterned(account);
            Annotation annotation = this.GetAnnotations(account, name, value).FirstOrDefault();
            if (annotation == null)
            {
                annotation = new Annotation(this)
                {
                    Context = this.CurrentWorker.Entities,
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

        /// <summary>
        /// アノテーションおよび関連するオブジェクトをこのストレージにアタッチし、管理下に置きます。
        /// </summary>
        /// <param name="annotation">このストレージに所属させるアノテーション。</param>
        public void InternAll(Annotation annotation)
        {
            this.Intern(annotation);
            this.Intern(annotation.Account);
        }

        /// <summary>
        /// 指定したアノテーションをこのストレージ上にマージし、管理下に置かれた同一内容のオブジェクトを返します。
        /// </summary>
        /// <param name="annotation">マージするアノテーション。</param>
        /// <returns>このストレージの管理下に置かれた同一内容のアノテーション。</returns>
        public Annotation GetInterned(Annotation annotation)
        {
            return this.GetInterned(annotation, this.Merge);
        }

        #endregion

        #region Relation

        public override IEnumerable<Relation> GetRelations(StorageObjectQuery<Relation, RelationTuple> query)
        {
            IEnumerable<Relation> relations = (query ?? new StorageObjectQuery<Relation, RelationTuple>())
                .Evaluate(this.CurrentWorker.Entities.Relations).ToArray();
            foreach (Relation annotation in relations)
            {
                this.InternAll(annotation);
            }
            this.CurrentWorker.AddingObjects.RemoveDuplicates(relations);
            return relations
                .Concat(this.CurrentWorker.AddingObjects.GetRelations(query))
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
            account = this.GetInterned(account);
            relatingAccount = this.GetInterned(relatingAccount);
            Relation relation = this.GetRelations(account, name, relatingAccount).FirstOrDefault();
            if (relation == null)
            {
                relation = new Relation(this)
                {
                    Context = this.CurrentWorker.Entities,
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

        /// <summary>
        /// リレーションおよび関連するオブジェクトをこのストレージにアタッチし、管理下に置きます。
        /// </summary>
        /// <param name="relation">このストレージに所属させるリレーション。</param>
        public void InternAll(Relation relation)
        {
            this.Intern(relation);
            this.Intern(relation.Account);
            this.Intern(relation.RelatingAccount);
        }

        /// <summary>
        /// 指定したリレーションをこのストレージ上にマージし、管理下に置かれた同一内容のオブジェクトを返します。
        /// </summary>
        /// <param name="relation">マージするリレーション。</param>
        /// <returns>このストレージの管理下に置かれた同一内容のリレーション。</returns>
        public Relation GetInterned(Relation relation)
        {
            return this.GetInterned(relation, this.Merge);
        }

        #endregion

        #region Mark

        public override IEnumerable<Mark> GetMarks(StorageObjectQuery<Mark, MarkTuple> query)
        {
            IEnumerable<Mark> marks = (query ?? new StorageObjectQuery<Mark, MarkTuple>())
                .Evaluate(this.CurrentWorker.Entities.Marks).ToArray();
            foreach (Mark mark in marks)
            {
                this.InternAll(mark);
            }
            this.CurrentWorker.AddingObjects.RemoveDuplicates(marks);
            return marks
                .Concat(this.CurrentWorker.AddingObjects.GetMarks(query))
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
            account = this.GetInterned(account);
            markingActivity = this.GetInterned(markingActivity);
            Mark mark = this.GetMarks(account, name, markingActivity).FirstOrDefault();
            if (mark == null)
            {
                mark = new Mark(this)
                {
                    Context = this.CurrentWorker.Entities,
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

        /// <summary>
        /// マークおよび関連するオブジェクトをこのストレージにアタッチし、管理下に置きます。
        /// </summary>
        /// <param name="mark">このストレージに所属させるマーク。</param>
        public void InternAll(Mark mark)
        {
            this.Intern(mark);
            this.Intern(mark.Account);
            this.Intern(mark.MarkingActivity);
        }

        /// <summary>
        /// 指定したマークをこのストレージ上にマージし、管理下に置かれた同一内容のオブジェクトを返します。
        /// </summary>
        /// <param name="mark">マージするマーク。</param>
        /// <returns>このストレージの管理下に置かれた同一内容のマーク。</returns>
        public Mark GetInterned(Mark mark)
        {
            return this.GetInterned(mark, this.Merge);
        }

        #endregion

        #region Reference

        public override IEnumerable<Reference> GetReferences(StorageObjectQuery<Reference, ReferenceTuple> query)
        {
            IEnumerable<Reference> references = (query ?? new StorageObjectQuery<Reference, ReferenceTuple>())
                .Evaluate(this.CurrentWorker.Entities.References).ToArray();
            foreach (Reference reference in references)
            {
                this.InternAll(reference);
            }
            this.CurrentWorker.AddingObjects.RemoveDuplicates(references);
            return references
                .Concat(this.CurrentWorker.AddingObjects.GetReferences(query))
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
            activity = this.GetInterned(activity);
            referringActivity = this.GetInterned(referringActivity);
            Reference reference = this.GetReferences(activity, name, referringActivity).FirstOrDefault();
            if (reference == null)
            {
                reference = new Reference(this)
                {
                    Context = this.CurrentWorker.Entities,
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

        /// <summary>
        /// リファレンスおよび関連するオブジェクトをこのストレージにアタッチし、管理下に置きます。
        /// </summary>
        /// <param name="reference">このストレージに所属させるリファレンス。</param>
        public void InternAll(Reference reference)
        {
            this.Intern(reference);
            this.Intern(reference.Activity);
            this.Intern(reference.ReferringActivity);
        }

        /// <summary>
        /// 指定したリファレンスをこのストレージ上にマージし、管理下に置かれた同一内容のオブジェクトを返します。
        /// </summary>
        /// <param name="reference">マージするリファレンス。</param>
        /// <returns>このストレージの管理下に置かれた同一内容のリファレンス。</returns>
        public Reference GetInterned(Reference reference)
        {
            return this.GetInterned(reference, this.Merge);
        }

        #endregion

        #region Tag

        public override IEnumerable<Tag> GetTags(StorageObjectQuery<Tag, TagTuple> query)
        {
            IEnumerable<Tag> tags = (query ?? new StorageObjectQuery<Tag, TagTuple>())
                .Evaluate(this.CurrentWorker.Entities.Tags).ToArray();
            foreach (Tag reference in tags)
            {
                this.InternAll(reference);
            }
            this.CurrentWorker.AddingObjects.RemoveDuplicates(tags);
            return tags
                .Concat(this.CurrentWorker.AddingObjects.GetTags(query))
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
            activity = this.GetInterned(activity);
            Tag tag = this.GetTags(activity, name, value).FirstOrDefault();
            if (tag == null)
            {
                tag = new Tag(this)
                {
                    Context = this.CurrentWorker.Entities,
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

        /// <summary>
        /// タグおよび関連するオブジェクトをこのストレージにアタッチし、管理下に置きます。
        /// </summary>
        /// <param name="tag">このストレージに所属させるタグ。</param>

        public void InternAll(Tag tag)
        {
            this.Intern(tag);
            this.Intern(tag.Activity);
        }

        /// <summary>
        /// 指定したタグをこのストレージ上にマージし、管理下に置かれた同一内容のオブジェクトを返します。
        /// </summary>
        /// <param name="tag">マージするタグ。</param>
        /// <returns>このストレージの管理下に置かれた同一内容のタグ。</returns>
        public Tag GetInterned(Tag tag)
        {
            return this.GetInterned(tag, this.Merge);
        }

        #endregion

        /// <summary>
        /// ストレージ オブジェクトをストレージにアタッチします。
        /// </summary>
        /// <param name="obj">アタッチするストレージ オブジェクト。</param>
        public override void AttachObject(StorageObject obj)
        {
            this.CurrentWorker.Entities.AttachTo(GetEntitySetName(obj), obj);
            obj.Context = this.CurrentWorker.Entities;
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
            obj.Context = null;
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
                    Int32 ret = this.CurrentWorker.Entities.SaveChanges(SaveOptions.DetectChangesBeforeSave);
                    this.CurrentWorker.AddingObjects.Clear();
                    this.CurrentWorker.Entities.AcceptAllChanges();
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

        /// <summary>
        /// ストレージ オブジェクトの変更をデータ ソースに保存します。失敗した場合再試行を行います。
        /// </summary>
        /// <returns>データ ソースにおいて処理が行われた行数。</returns>
        public Int32 TryUpdate()
        {
            return this.TryUpdate(3, null, true);
        }

        /// <summary>
        /// ストレージ オブジェクトの変更をデータ ソースに保存します。失敗した場合、指定した回数または時間再試行を行います。
        /// </summary>
        /// <returns>データ ソースにおいて処理が行われた行数。</returns>
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

        /// <summary>
        /// オブジェクトをこのストレージにアタッチし、管理下に置きます。
        /// </summary>
        /// <typeparam name="TEntity">管理下に置くストレージ オブジェクトの型。</typeparam>
        /// <param name="obj">管理下に置くオブジェクト。</param>
        public void Intern<TEntity>(TEntity obj)
            where TEntity : StorageObject
        {
            if (obj.Storage == null)
            {
                obj.Storage = this;
            }
            StorageObjectContext context = obj.Context;
            if (context == null)
            {
                this.AttachObject(obj);
            }
            else if (this.CurrentWorker.Entities != context)
            {
                context.Detach(obj);
                this.AttachObject(obj);
            }
        }

        private TEntity GetInterned<TEntity>(TEntity obj, Func<TEntity, TEntity> merger)
            where TEntity : StorageObject
        {
            if (obj.Storage == null)
            {
                obj.Storage = this;
            }
            StorageObjectContext context = obj.Context;
            if (context == null)
            {
                this.AttachObject(obj);
            }
            else if (context.IsDisposed)
            {
                context.Detach(obj);
                this.AttachObject(obj);
            }
            else if (this.CurrentWorker.Entities != context)
            {
                obj = merger(obj);
            }
            return obj;
        }

        /// <summary>
        /// ワーカー スコープを開始します。
        /// </summary>
        /// <remarks>
        /// <para>開始されたワーカー スコープは、<see cref="CurrentWorker"/> プロパティからアクセスできます。</para>
        /// <para>ワーカー スコープは、<see cref="EndWorkerScope()"/> メソッドによって必ず終了されなければなりません。</para>
        /// </remarks>
        public void BeginWorkerScope()
        {
            this.BeginWorkerScope(true);
        }

        /// <summary>
        /// ワーカー スコープを開始します。
        /// </summary>
        /// <param name="checkState">既にワーカー スコープが開始されているかどうか確認する場合は <c>true</c>。それ以外の場合は <c>false</c>。</param>
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

        /// <summary>
        /// ワーカー スコープを終了します。
        /// </summary>
        public void EndWorkerScope()
        {
            this.EndWorkerScope(true);
        }

        /// <summary>
        /// ワーカー スコープを終了します。
        /// </summary>
        /// <param name="checkState">現在のワーカー スコープが存在していないかどうか確認する場合は <c>true</c>。それ以外の場合は <c>false</c>。</param>
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

        /// <summary>
        /// ワーカー スコープを開始し、コードを実行します。
        /// </summary>
        /// <param name="body">ワーカー スコープ内で実行するコード。</param>
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
                    return "Activities";
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