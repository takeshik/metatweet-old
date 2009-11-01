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
using System.Data;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Linq;
using XSpect.MetaTweet.Objects;

namespace XSpect.MetaTweet.Objects
{
    /// <summary>
    /// オブジェクト コンテキストを保持し、ストレージ オブジェクトを操作する機能を提供すします。
    /// </summary>
    public class Storage
        : MarshalByRefObject,
          IDisposable
    {
        private Boolean _disposed;

        /// <summary>
        /// このストレージのキャッシュを取得または設定します。
        /// </summary>
        /// <value>このストレージのキャッシュ。</value>
        public StorageCache Cache
        {
            get;
            set;
        }

        /// <summary>
        /// このストレージが保持しているオブジェクト コンテキストを取得します。
        /// </summary>
        /// <value>The entities.</value>
        public StorageEntities Entities
        {
            get;
            private set;
        }

        public IQueryable<Account> Accounts
        {
            get
            {
                return this.Entities.AccountSet.Concat(this.Cache.AddingObjects.Accounts);
            }
        }

        public IQueryable<Activity> Activities
        {
            get
            {
                return this.Entities.ActivitySet.Concat(this.Cache.AddingObjects.Activities);
            }
        }

        public IQueryable<Annotation> Annotations
        {
            get
            {
                return this.Entities.AnnotationSet.Concat(this.Cache.AddingObjects.Annotations);
            }
        }

        public IQueryable<Relation> Relations
        {
            get
            {
                return this.Entities.RelationSet.Concat(this.Cache.AddingObjects.Relations);
            }
        }

        public IQueryable<Mark> Marks
        {
            get
            {
                return this.Entities.MarkSet.Concat(this.Cache.AddingObjects.Marks);
            }
        }

        public IQueryable<Reference> References
        {
            get
            {
                return this.Entities.ReferenceSet.Concat(this.Cache.AddingObjects.References);
            }
        }

        public IQueryable<Tag> Tags
        {
            get
            {
                return this.Entities.TagSet.Concat(this.Cache.AddingObjects.Tags);
            }
        }

        /// <summary>
        /// <see cref="Storage"/> の新しいインスタンスを初期化します。
        /// </summary>
        public Storage()
        {
            this.Cache = new StorageCache(this);
        }

        /// <summary>
        /// <see cref="Storage"/> がガベージ コレクションによってクリアされる前に、アンマネージ リソースを解放し、その他のクリーンアップ操作を実行します。
        /// </summary>
        ~Storage()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// 対象のインスタンスの有効期間ポリシーを制御する、有効期間サービス オブジェクトを取得します。
        /// </summary>
        /// <returns>
        /// 対象のインスタンスの有効期間ポリシーを制御するときに使用する、<see cref="T:System.Runtime.Remoting.Lifetime.ILease"/> 型のオブジェクト。存在する場合は、このインスタンスの現在の有効期間サービス オブジェクトです。それ以外の場合は、<see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime"/> プロパティの値に初期化された新しい有効期間サービス オブジェクトです。
        /// </returns>
        /// <exception cref="T:System.Security.SecurityException">直前の呼び出し元に、インフラストラクチャ アクセス許可がありません。</exception>
        /// <PermissionSet>
        /// <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure"/>
        /// </PermissionSet>
        public override Object InitializeLifetimeService()
        {
            return null;
        }

        /// <summary>
        /// バックエンドのデータソースとの接続を初期化します。
        /// </summary>
        public virtual void Initialize()
        {
            this.Entities = new StorageEntities();
        }

        /// <summary>
        /// バックエンドのデータソースとの接続を初期化します。
        /// </summary>
        /// <param name="connectionString">接続に使用する文字列。</param>
        public virtual void Initialize(String connectionString)
        {
            this.Entities = new StorageEntities(connectionString);
        }

        /// <summary>
        /// バックエンドのデータソースとの接続を初期化します。
        /// </summary>
        /// <param name="connection">使用する接続。</param>
        public virtual void Initialize(EntityConnection connection)
        {
            this.Entities = new StorageEntities(connection);
        }

        /// <summary>
        /// <see cref="Storage"/> によって使用されているすべてのリソースを解放します。
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// <see cref="Storage"/> によって使用されているアンマネージ リソースを解放し、オプションでマネージ リソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 <c>true</c>、破棄されない場合は <c>false</c>。</param>
        protected virtual void Dispose(Boolean disposing)
        {
            this.Entities.Dispose();
            this._disposed = true;
        }

        /// <summary>
        /// オブジェクトが破棄されているかどうかを確認し、破棄されている場合は例外を送出します。
        /// </summary>
        protected void CheckIfDisposed()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }

        #region Account

        /// <summary>
        /// 値を指定してアカウントを検索します。
        /// </summary>
        /// <param name="accountId">アカウントの ID。指定しない場合は <c>null</c>。</param>
        /// <param name="realm">アカウントのレルム。指定しない場合は <c>null</c>。</param>
        /// <returns>指定した条件に合致するアカウントのシーケンス。</returns>
        public virtual IQueryable<Account> GetAccounts(
            Nullable<Guid> accountId,
            String realm
        )
        {
            IQueryable<Account> accounts = this.Accounts;
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
                account.Storage = this;
            }
            return accounts;
        }

        /// <summary>
        /// 値を指定してアカウントを検索します。
        /// </summary>
        /// <param name="accountId">アカウントの ID。指定しない場合は <c>null</c>。</param>
        /// <returns>指定した条件に合致するアカウントのシーケンス。</returns>
        public IQueryable<Account> GetAccounts(
            Nullable<Guid> accountId
        )
        {
            return this.GetAccounts(accountId, null);
        }

        /// <summary>
        /// 全てのアカウントを取得します。
        /// </summary>
        /// <returns>オブジェクト コンテキストに存在する全てのアカウントのシーケンス。</returns>
        public IQueryable<Account> GetAccounts()
        {
            return this.GetAccounts(null, null);
        }

        /// <summary>
        /// 新しいアカウントを生成します。
        /// </summary>
        /// <param name="accountId">アカウントの ID。</param>
        /// <param name="realm">アカウントのレルム。</param>
        /// <returns>生成されたアカウント。</returns>
        public virtual Account NewAccount(Guid accountId, String realm)
        {
            Account account = new Account(this)
            {
                AccountId = accountId,
                Realm = realm,
            };
            // BeginInit() must be called at StorageObject#.ctor(Storage).
            account.EndInit();
            this.Entities.AddToAccountSet(account);
            this.Cache.AddingObjects.Add(account);
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
        protected virtual IQueryable<Activity> GetActivities(
            Nullable<Guid> accountId,
            Nullable<DateTime> timestamp,
            String category,
            String subId,
            String userAgent,
            Object value,
            Object data
        )
        {
            IQueryable<Activity> activities = this.Activities;
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
                activity.Storage = this;
            }
            return activities;
        }

        /// <summary>
        /// 値を指定してアクティビティを検索します。
        /// </summary>
        /// <param name="accountId">アクティビティを行ったアカウントの ID。</param>
        /// <param name="timestamp">アクティビティのタイムスタンプ。指定しない場合は <c>null</c>。</param>
        /// <param name="category">アクティビティのカテゴリ。指定しない場合は <c>null</c>。</param>
        /// <param name="subId">アクティビティのサブ ID。指定しない場合は <c>null</c>。</param>
        /// <param name="userAgent">アクティビティのユーザ エージェント。指定しない場合は <c>null</c>。</param>
        /// <param name="value">アクティビティの値。指定しない場合は <c>null</c>。条件として <c>null</c> 値を指定する場合は <see cref="DBNull"/> 値。</param>
        /// <param name="data">アクティビティのデータ。指定しない場合は <c>null</c>。条件として <c>null</c> 値を指定する場合は <see cref="DBNull"/> 値。</param>
        /// <returns>指定した条件に合致するアクティビティのシーケンス。</returns>
        public IQueryable<Activity> GetActivities(
            Guid accountId,
            Nullable<DateTime> timestamp,
            String category,
            String subId,
            String userAgent,
            Object value,
            Object data
        )
        {
            return this.GetActivities(
                (Nullable<Guid>) accountId,
                timestamp,
                category,
                subId,
                userAgent,
                value,
                data
            );
        }

        /// <summary>
        /// 値を指定してアクティビティを検索します。
        /// </summary>
        /// <param name="account">アクティビティを行ったアカウント。指定しない場合は <c>null</c>。</param>
        /// <param name="timestamp">アクティビティのタイムスタンプ。指定しない場合は <c>null</c>。</param>
        /// <param name="category">アクティビティのカテゴリ。指定しない場合は <c>null</c>。</param>
        /// <param name="subId">アクティビティのサブ ID。指定しない場合は <c>null</c>。</param>
        /// <param name="userAgent">アクティビティのユーザ エージェント。指定しない場合は <c>null</c>。</param>
        /// <param name="value">アクティビティの値。指定しない場合は <c>null</c>。条件として <c>null</c> 値を指定する場合は <see cref="DBNull"/> 値。</param>
        /// <param name="data">アクティビティのデータ。指定しない場合は <c>null</c>。条件として <c>null</c> 値を指定する場合は <see cref="DBNull"/> 値。</param>
        /// <returns>指定した条件に合致するアクティビティのシーケンス。</returns>
        public IQueryable<Activity> GetActivities(
            Account account,
            Nullable<DateTime> timestamp,
            String category,
            String subId,
            String userAgent,
            Object value,
            Object data
        )
        {
            return this.GetActivities(
                account != null ? account.AccountId : default(Nullable<Guid>),
                timestamp,
                category,
                subId,
                userAgent,
                value,
                data
            );
        }

        /// <summary>
        /// 値を指定してアクティビティを検索します。
        /// </summary>
        /// <param name="accountId">アクティビティを行ったアカウントの ID。</param>
        /// <param name="timestamp">アクティビティのタイムスタンプ。指定しない場合は <c>null</c>。</param>
        /// <param name="category">アクティビティのカテゴリ。指定しない場合は <c>null</c>。</param>
        /// <param name="subId">アクティビティのサブ ID。指定しない場合は <c>null</c>。</param>
        /// <returns>指定した条件に合致するアクティビティのシーケンス。</returns>
        public IQueryable<Activity> GetActivities(
            Guid accountId,
            Nullable<DateTime> timestamp,
            String category,
            String subId
        )
        {
            return this.GetActivities(
                (Nullable<Guid>) accountId,
                timestamp,
                category,
                subId,
                null,
                null,
                null
            );
        }

        /// <summary>
        /// 値を指定してアクティビティを検索します。
        /// </summary>
        /// <param name="account">アクティビティを行ったアカウント。指定しない場合は <c>null</c>。</param>
        /// <param name="timestamp">アクティビティのタイムスタンプ。指定しない場合は <c>null</c>。</param>
        /// <param name="category">アクティビティのカテゴリ。指定しない場合は <c>null</c>。</param>
        /// <param name="subId">アクティビティのサブ ID。指定しない場合は <c>null</c>。</param>
        /// <returns>指定した条件に合致するアクティビティのシーケンス。</returns>
        public IQueryable<Activity> GetActivities(
            Account account,
            Nullable<DateTime> timestamp,
            String category,
            String subId
        )
        {
            return this.GetActivities(
                account != null ? account.AccountId : default(Nullable<Guid>),
                timestamp,
                category,
                subId,
                null,
                null,
                null
            );
        }

        /// <summary>
        /// 全てのアクティビティを取得します。
        /// </summary>
        /// <returns>オブジェクト コンテキストに存在する全てのアクティビティのシーケンス。</returns>
        public IQueryable<Activity> GetActivities()
        {
            return this.GetActivities((Nullable<Guid>) null, null, null, null, null, null, null);
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
        public virtual Activity NewActivity(Account account, DateTime timestamp, String category, String subId, String userAgent, String value, Byte[] data)
        {
            Activity activity = new Activity(this)
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
            return activity;
        }

        /// <summary>
        /// 新しいアクティビティを生成します。
        /// </summary>
        /// <param name="account">アクティビティを行うアカウント。</param>
        /// <param name="timestamp">アクティビティのタイムスタンプ。</param>
        /// <param name="category">アクティビティのカテゴリ。</param>
        /// <param name="subId">アクティビティのサブ ID。</param>
        /// <returns>生成されたアクティビティ。</returns>
        public Activity NewActivity(Account account, DateTime timestamp, String category, String subId)
        {
            return this.NewActivity(account, timestamp, category, subId, null, null, null);
        }

        #endregion

        #region Annotation

        /// <summary>
        /// 値を指定してアノテーションを検索します。
        /// </summary>
        /// <param name="accountId">アノテーションが関連付けられているアカウントの ID。指定しない場合は <c>null</c>。</param>
        /// <param name="name">アノテーションの意味。指定しない場合は <c>null</c>。</param>
        /// <returns>指定した条件に合致するアノテーションのシーケンス。</returns>
        protected virtual IQueryable<Annotation> GetAnnotations(
            Nullable<Guid> accountId,
            String name
        )
        {
            IQueryable<Annotation> annotations = this.Annotations;
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
                annotation.Storage = this;
            }
            return annotations;
        }

        /// <summary>
        /// 値を指定してアノテーションを検索します。
        /// </summary>
        /// <param name="accountId">アノテーションが関連付けられているアカウントの ID。</param>
        /// <param name="name">アノテーションの意味。指定しない場合は <c>null</c>。</param>
        /// <returns>指定した条件に合致するアノテーションのシーケンス。</returns>
        public IQueryable<Annotation> GetAnnotations(
            Guid accountId,
            String name
        )
        {
            return this.GetAnnotations((Nullable<Guid>) accountId, name);
        }

        /// <summary>
        /// 値を指定してアノテーションを検索します。
        /// </summary>
        /// <param name="account">アノテーションが関連付けられているアカウント。指定しない場合は <c>null</c>。</param>
        /// <param name="name">アノテーションの意味。指定しない場合は <c>null</c>。</param>
        /// <returns>指定した条件に合致するアノテーションのシーケンス。</returns>
        public IQueryable<Annotation> GetAnnotations(
            Account account,
            String name
        )
        {
            return this.GetAnnotations(
                account != null ? account.AccountId : default(Nullable<Guid>),
                name
            );
        }

        /// <summary>
        /// 全てのアノテーションを取得します。
        /// </summary>
        /// <returns>オブジェクト コンテキストに存在する全てのアノテーションのシーケンス。</returns>
        public IQueryable<Annotation> GetAnnotations()
        {
            return this.GetAnnotations((Nullable<Guid>) null, null);
        }

        /// <summary>
        /// 新しいアノテーションを生成します。
        /// </summary>
        /// <param name="account">アノテーションが関連付けられるアカウント。</param>
        /// <param name="name">アノテーションの意味。</param>
        /// <returns>生成されたアノテーション。</returns>
        public virtual Annotation NewAnnotation(Account account, String name)
        {
            Annotation annotation = new Annotation(this)
            {
                Account = account,
                Name = name,
            };
            // BeginInit() must be called at StorageObject#.ctor(Storage).
            annotation.EndInit();
            this.Entities.AddToAnnotationSet(annotation);
            account.Annotations.Add(annotation);
            this.Cache.AddingObjects.Add(annotation);
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
        protected virtual IQueryable<Relation> GetRelations(
            Nullable<Guid> accountId,
            String name,
            Nullable<Guid> relatingAccountId
        )
        {
            IQueryable<Relation> relations = this.Relations;
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
                relation.Storage = this;
            }
            return relations;
        }

        /// <summary>
        /// 値を指定してリレーションを検索します。
        /// </summary>
        /// <param name="accountId">リレーションが関連付けられているアカウントの ID。</param>
        /// <param name="name">リレーションの意味。</param>
        /// <param name="relatingAccountId">リレーションが関連付けられる先のアカウントの ID。</param>
        /// <returns>指定した条件に合致するリレーションのシーケンス。</returns>
        public IQueryable<Relation> GetRelations(
            Guid accountId,
            String name,
            Guid relatingAccountId
        )
        {
            return this.GetRelations(
                (Nullable<Guid>) accountId,
                name,
                relatingAccountId
            );
        }

        /// <summary>
        /// 値を指定してリレーションを検索します。
        /// </summary>
        /// <param name="account">リレーションが関連付けられているアカウント。指定しない場合は <c>null</c>。</param>
        /// <param name="name">リレーションの意味。</param>
        /// <param name="relatingAccount">リレーションが関連付けられる先のアカウント。指定しない場合は <c>null</c>。</param>
        /// <returns>指定した条件に合致するリレーションのシーケンス。</returns>
        public IQueryable<Relation> GetRelations(
            Account account,
            String name,
            Account relatingAccount
        )
        {
            return this.GetRelations(
                account != null ? account.AccountId : default(Nullable<Guid>),
                name,
                relatingAccount != null ? relatingAccount.AccountId : default(Nullable<Guid>)
            );
        }

        /// <summary>
        /// 全てのリレーションを取得します。
        /// </summary>
        /// <returns>オブジェクト コンテキストに存在する全てのリレーションのシーケンス。</returns>
        public IQueryable<Relation> GetRelations()
        {
            return this.GetRelations((Nullable<Guid>) null, null, null);
        }

        /// <summary>
        /// 新しいリレーションを生成します。
        /// </summary>
        /// <param name="account">リレーションが関連付けられるアカウント。</param>
        /// <param name="name">リレーションの意味。</param>
        /// <param name="relatingAccount">リレーションが関連付けられる先のアカウント。</param>
        /// <returns>生成されたリレーション。</returns>
        public virtual Relation NewRelation(Account account, String name, Account relatingAccount)
        {
            Relation relation = new Relation(this)
            {
                Account = account,
                Name = name,
                RelatingAccount = relatingAccount,
            };
            // BeginInit() must be called at StorageObject#.ctor(Storage).
            relation.EndInit();
            this.Entities.AddToRelationSet(relation);
            account.Relations.Add(relation);
            this.Cache.AddingObjects.Add(relation);
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
        public virtual IQueryable<Mark> GetMarks(
            Nullable<Guid> accountId,
            String name,
            Nullable<Guid> markingAccountId,
            Nullable<DateTime> markingTimestamp,
            String markingCategory,
            String markingSubId
        )
        {
            IQueryable<Mark> marks = this.Marks;
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
                mark.Storage = this;
            }
            return marks;
        }

        /// <summary>
        /// 値を指定してマークを検索します。
        /// </summary>
        /// <param name="accountId">マークが関連付けられているアカウントの ID。</param>
        /// <param name="name">マークの意味。指定しない場合は <c>null</c>。</param>
        /// <param name="markingActivity">マークが関連付けられる先のアクティビティ。指定しない場合は <c>null</c>。</param>
        /// <returns>指定した条件に合致するマークのシーケンス。</returns>
        public IQueryable<Mark> GetMarks(
            Guid accountId,
            String name,
            Activity markingActivity
        )
        {
            return markingActivity != null
                ? this.GetMarks(
                      (Nullable<Guid>) accountId,
                      name,
                      markingActivity.AccountId,
                      markingActivity.Timestamp,
                      markingActivity.Category,
                      markingActivity.SubId
                  )
                : this.GetMarks(
                      (Nullable<Guid>) accountId,
                      name,
                      null,
                      null,
                      null,
                      null
                );

        }

        /// <summary>
        /// 値を指定してマークを検索します。
        /// </summary>
        /// <param name="account">マークが関連付けられているアカウント。指定しない場合は <c>null</c>。</param>
        /// <param name="name">マークの意味。指定しない場合は <c>null</c>。</param>
        /// <param name="markingActivity">マークが関連付けられる先のアクティビティ。指定しない場合は <c>null</c>。</param>
        /// <returns>指定した条件に合致するマークのシーケンス。</returns>
        public IQueryable<Mark> GetMarks(
            Account account,
            String name,
            Activity markingActivity
        )
        {
            return markingActivity != null
                ? this.GetMarks(
                      account != null ? account.AccountId : default(Nullable<Guid>),
                      name,
                      markingActivity.AccountId,
                      markingActivity.Timestamp,
                      markingActivity.Category,
                      markingActivity.SubId
                  )
                : this.GetMarks(
                      account != null ? account.AccountId : default(Nullable<Guid>),
                      name,
                      null,
                      null,
                      null,
                      null
                );
        }

        /// <summary>
        /// 全てのマークを取得します。
        /// </summary>
        /// <returns>オブジェクト コンテキストに存在する全てのマークのシーケンス。</returns>
        public IQueryable<Mark> GetMarks()
        {
            return this.GetMarks(null, null, null, null, null, null);
        }

        /// <summary>
        /// 新しいマークを生成します。
        /// </summary>
        /// <param name="account">マークが関連付けられるアカウント。</param>
        /// <param name="name">マークの意味。</param>
        /// <param name="markingActivity">マークが関連付けられる先のアクティビティ。</param>
        /// <returns>生成されたマーク。</returns>
        public virtual Mark NewMark(Account account, String name, Activity markingActivity)
        {
            Mark mark = new Mark(this)
            {
                Account = account,
                Name = name,
                MarkingActivity = markingActivity,
            };
            // BeginInit() must be called at StorageObject#.ctor(Storage).
            mark.EndInit();
            this.Entities.AddToMarkSet(mark);
            account.Marks.Add(mark);
            this.Cache.AddingObjects.Add(mark);
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
        public virtual IQueryable<Reference> GetReferences(
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
            IQueryable<Reference> references = this.References;
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
                reference.Storage = this;
            }
            return references;
        }

        /// <summary>
        /// 値を指定してリファレンスを検索します。
        /// </summary>
        /// <param name="activity">リファレンスが関連付けられているアクティビティ。指定しない場合は <c>null</c>。</param>
        /// <param name="name">リファレンスの意味。指定しない場合は <c>null</c>。</param>
        /// <param name="referringActivity">リファレンスが関連付けられる先のアクティビティ。指定しない場合は <c>null</c>。</param>
        /// <returns>指定した条件に合致するリファレンスのシーケンス。</returns>
        public IQueryable<Reference> GetReferences(
            Activity activity,
            String name,
            Activity referringActivity
        )
        {
            return activity != null
                ? referringActivity != null
                      ? this.GetReferences(
                            activity.AccountId,
                            activity.Timestamp,
                            activity.Category,
                            activity.SubId,
                            name,
                            referringActivity.AccountId,
                            referringActivity.Timestamp,
                            referringActivity.Category,
                            referringActivity.SubId
                        )
                      : this.GetReferences(
                            activity.AccountId,
                            activity.Timestamp,
                            activity.Category,
                            activity.SubId,
                            name,
                            null,
                            null,
                            null,
                            null
                        )
                : referringActivity != null
                      ? this.GetReferences(
                            null,
                            null,
                            null,
                            null,
                            name,
                            referringActivity.AccountId,
                            referringActivity.Timestamp,
                            referringActivity.Category,
                            referringActivity.SubId
                        )
                      : this.GetReferences(
                            null,
                            null,
                            null,
                            null,
                            name,
                            null,
                            null,
                            null,
                            null
                        );
        }

        /// <summary>
        /// 全てのリファレンスを取得します。
        /// </summary>
        /// <returns>オブジェクト コンテキストに存在する全てのリファレンスのシーケンス。</returns>
        public IQueryable<Reference> GetReferences()
        {
            return this.GetReferences(null, null, null, null, null, null, null, null, null);
        }

        /// <summary>
        /// 新しいリファレンスを生成します。
        /// </summary>
        /// <param name="activity">リファレンスが関連付けられているアクティビティ。</param>
        /// <param name="name">リファレンスの意味。</param>
        /// <param name="referringActivity">リファレンスが関連付けられる先のアクティビティ。</param>
        /// <returns>生成されたリファレンス。</returns>
        public virtual Reference NewReference(Activity activity, String name, Activity referringActivity)
        {
            Reference reference = new Reference(this)
            {
                Activity = activity,
                Name = name,
                ReferringActivity = referringActivity,
            };
            // BeginInit() must be called at StorageObject#.ctor(Storage).
            reference.EndInit();
            this.Entities.AddToReferenceSet(reference);
            activity.References.Add(reference);
            this.Cache.AddingObjects.Add(reference);
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
        public virtual IQueryable<Tag> GetTags(
            Nullable<Guid> accountId,
            Nullable<DateTime> timestamp,
            String category,
            String subId,
            String name
        )
        {
            IQueryable<Tag> tags = this.Tags;
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
                tag.Storage = this;
            }
            return tags;
        }

        /// <summary>
        /// 値を指定してタグを検索します。
        /// </summary>
        /// <param name="activity">タグが関連付けられているアクティビティ。指定しない場合は <c>null</c>。</param>
        /// <param name="name">タグの意味。指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するタグのシーケンス。</returns>
        public IQueryable<Tag> GetTags(
            Activity activity,
            String name
        )
        {
            return activity != null
                ? this.GetTags(
                      activity.AccountId,
                      activity.Timestamp,
                      activity.Category,
                      activity.SubId,
                      name
                  )
                : this.GetTags(
                      null,
                      null,
                      null,
                      null,
                      name
                  );
        }

        /// <summary>
        /// 全てのタグを取得します。
        /// </summary>
        /// <returns>オブジェクト コンテキストに存在する全てのタグのシーケンス。</returns>
        public IQueryable<Tag> GetTags()
        {
            return this.GetTags(null, null, null, null, null);
        }

        /// <summary>
        /// 新しいタグを生成します。
        /// </summary>
        /// <param name="activity">タグが関連付けられるアクティビティ。</param>
        /// <param name="name">タグの意味。</param>
        /// <returns>生成されたタグ。</returns>
        public virtual Tag NewTag(Activity activity, String name)
        {
            Tag tag = new Tag(this)
            {
                Activity = activity,
                Name = name,
            };
            // BeginInit() must be called at StorageObject#.ctor(Storage).
            tag.EndInit();
            this.Entities.AddToTagSet(tag);
            activity.Tags.Add(tag);
            this.Cache.AddingObjects.Add(tag);
            return tag;
        }

        #endregion

        /// <summary>
        /// ストレージ オブジェクトの変更をデータ ソースに保存します。
        /// </summary>
        /// <returns>データ ソースにおいて処理が行われた行数。</returns>
        public Int32 Update()
        {
            Int32 ret = this.Entities.SaveChanges();
            this.Cache.AddingObjects.Clear();
            return ret;
        }
    }
}