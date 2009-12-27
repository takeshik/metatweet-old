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
using System.Data.Objects;
using XSpect.MetaTweet.Objects;

namespace XSpect.MetaTweet.Objects
{
    /// <summary>
    /// ストレージ オブジェクトを管理するクラスに共通の機能を提供します。
    /// </summary>
    public abstract class Storage
        : MarshalByRefObject,
          IDisposable
    {
        private Boolean _disposed;

        /// <summary>
        /// 派生クラスで実装された場合、このストレージのキャッシュを取得または設定します。
        /// </summary>
        /// <value>このストレージのキャッシュ。</value>
        public abstract StorageCache Cache
        {
            get;
            set;
        }

        /// <summary>
        /// <see cref="Storage"/> の新しいインスタンスを初期化します。
        /// </summary>
        protected Storage()
        {
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
        public abstract IEnumerable<Account> GetAccounts(
            Nullable<Guid> accountId,
            String realm
        );

        /// <summary>
        /// 値を指定してアカウントを検索します。
        /// </summary>
        /// <param name="accountId">アカウントの ID。指定しない場合は <c>null</c>。</param>
        /// <returns>指定した条件に合致するアカウントのシーケンス。</returns>
        public IEnumerable<Account> GetAccounts(
            Nullable<Guid> accountId
        )
        {
            return this.GetAccounts(accountId, null);
        }

        /// <summary>
        /// 全てのアカウントを取得します。
        /// </summary>
        /// <returns>オブジェクト コンテキストに存在する全てのアカウントのシーケンス。</returns>
        public IEnumerable<Account> GetAccounts()
        {
            return this.GetAccounts(null, null);
        }

        /// <summary>
        /// 新しいアカウントを生成します。
        /// </summary>
        /// <param name="accountId">アカウントの ID。</param>
        /// <param name="realm">アカウントのレルム。</param>
        /// <returns>生成されたアカウント。</returns>
        public abstract Account NewAccount(Guid accountId, String realm);

        /// <summary>
        /// 指定したアカウントをこのストレージにマージします。
        /// </summary>
        /// <param name="account">マージするアカウント。</param>
        /// <returns>マージされた、このストレージに所属するアカウント。</returns>
        public Account Merge(IAccount account)
        {
            return this.NewAccount(account.AccountId, account.Realm);
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
        protected internal abstract IEnumerable<Activity> GetActivities(
            Nullable<Guid> accountId,
            Nullable<DateTime> timestamp,
            String category,
            String subId,
            String userAgent,
            Object value,
            Object data
        );

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
        public IEnumerable<Activity> GetActivities(
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
        public IEnumerable<Activity> GetActivities(
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
        public IEnumerable<Activity> GetActivities(
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
        public IEnumerable<Activity> GetActivities(
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
        public IEnumerable<Activity> GetActivities()
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
        public abstract Activity NewActivity(Account account, DateTime timestamp, String category, String subId, String userAgent, String value, Byte[] data);

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

        /// <summary>
        /// 指定したアクティビティをこのストレージにマージします。
        /// </summary>
        /// <param name="activity">マージするアクティビティ。</param>
        /// <returns>マージされた、このストレージに所属するアクティビティ。</returns>
        public Activity Merge(IActivity activity)
        {
            return this.NewActivity(
                this.Merge(activity.Account),
                activity.Timestamp,
                activity.Category,
                activity.SubId
            );
        }


        #endregion

        #region Annotation

        /// <summary>
        /// 値を指定してアノテーションを検索します。
        /// </summary>
        /// <param name="accountId">アノテーションが関連付けられているアカウントの ID。指定しない場合は <c>null</c>。</param>
        /// <param name="name">アノテーションの意味。指定しない場合は <c>null</c>。</param>
        /// <returns>指定した条件に合致するアノテーションのシーケンス。</returns>
        protected internal abstract IEnumerable<Annotation> GetAnnotations(
            Nullable<Guid> accountId,
            String name
        );

        /// <summary>
        /// 値を指定してアノテーションを検索します。
        /// </summary>
        /// <param name="accountId">アノテーションが関連付けられているアカウントの ID。</param>
        /// <param name="name">アノテーションの意味。指定しない場合は <c>null</c>。</param>
        /// <returns>指定した条件に合致するアノテーションのシーケンス。</returns>
        public IEnumerable<Annotation> GetAnnotations(
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
        public IEnumerable<Annotation> GetAnnotations(
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
        public IEnumerable<Annotation> GetAnnotations()
        {
            return this.GetAnnotations((Nullable<Guid>) null, null);
        }

        /// <summary>
        /// 新しいアノテーションを生成します。
        /// </summary>
        /// <param name="account">アノテーションが関連付けられるアカウント。</param>
        /// <param name="name">アノテーションの意味。</param>
        /// <returns>生成されたアノテーション。</returns>
        public abstract Annotation NewAnnotation(Account account, String name);

        /// <summary>
        /// 指定したアノテーションをこのストレージにマージします。
        /// </summary>
        /// <param name="annotation">マージするアノテーション。</param>
        /// <returns>マージされた、このストレージに所属するアノテーション。</returns>
        public Annotation Merge(IAnnotation annotation)
        {
            return this.NewAnnotation(
                this.Merge(annotation.Account),
                annotation.Name
            );
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
        protected internal abstract IEnumerable<Relation> GetRelations(
            Nullable<Guid> accountId,
            String name,
            Nullable<Guid> relatingAccountId
        );

        /// <summary>
        /// 値を指定してリレーションを検索します。
        /// </summary>
        /// <param name="accountId">リレーションが関連付けられているアカウントの ID。</param>
        /// <param name="name">リレーションの意味。</param>
        /// <param name="relatingAccountId">リレーションが関連付けられる先のアカウントの ID。</param>
        /// <returns>指定した条件に合致するリレーションのシーケンス。</returns>
        public IEnumerable<Relation> GetRelations(
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
        public IEnumerable<Relation> GetRelations(
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
        public IEnumerable<Relation> GetRelations()
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
        public abstract Relation NewRelation(Account account, String name, Account relatingAccount);

        /// <summary>
        /// 指定したリレーションをこのストレージにマージします。
        /// </summary>
        /// <param name="relation">マージするリレーション。</param>
        /// <returns>マージされた、このストレージに所属するリレーション。</returns>
        public Relation Merge(IRelation relation)
        {
            return this.NewRelation(
                this.Merge(relation.Account),
                relation.Name,
                this.Merge(relation.RelatingAccount)
            );
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
        public abstract IEnumerable<Mark> GetMarks(
            Nullable<Guid> accountId,
            String name,
            Nullable<Guid> markingAccountId,
            Nullable<DateTime> markingTimestamp,
            String markingCategory,
            String markingSubId
        );

        /// <summary>
        /// 値を指定してマークを検索します。
        /// </summary>
        /// <param name="accountId">マークが関連付けられているアカウントの ID。</param>
        /// <param name="name">マークの意味。指定しない場合は <c>null</c>。</param>
        /// <param name="markingActivity">マークが関連付けられる先のアクティビティ。指定しない場合は <c>null</c>。</param>
        /// <returns>指定した条件に合致するマークのシーケンス。</returns>
        public IEnumerable<Mark> GetMarks(
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
        public IEnumerable<Mark> GetMarks(
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
        public IEnumerable<Mark> GetMarks()
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
        public abstract Mark NewMark(Account account, String name, Activity markingActivity);

        /// <summary>
        /// 指定したマークをこのストレージにマージします。
        /// </summary>
        /// <param name="mark">マージするマーク。</param>
        /// <returns>マージされた、このストレージに所属するマーク。</returns>
        public Mark Merge(IMark mark)
        {
            return this.NewMark(
                this.Merge(mark.Account),
                mark.Name,
                this.Merge(mark.MarkingActivity)
            );
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
        public abstract IEnumerable<Reference> GetReferences(
            Nullable<Guid> accountId,
            Nullable<DateTime> timestamp,
            String category,
            String subId,
            String name,
            Nullable<Guid> referringAccountId,
            Nullable<DateTime> referringTimestamp,
            String referringCategory,
            String referringSubId
        );

        /// <summary>
        /// 値を指定してリファレンスを検索します。
        /// </summary>
        /// <param name="activity">リファレンスが関連付けられているアクティビティ。指定しない場合は <c>null</c>。</param>
        /// <param name="name">リファレンスの意味。指定しない場合は <c>null</c>。</param>
        /// <param name="referringActivity">リファレンスが関連付けられる先のアクティビティ。指定しない場合は <c>null</c>。</param>
        /// <returns>指定した条件に合致するリファレンスのシーケンス。</returns>
        public IEnumerable<Reference> GetReferences(
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
        public IEnumerable<Reference> GetReferences()
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
        public abstract Reference NewReference(Activity activity, String name, Activity referringActivity);

        /// <summary>
        /// 指定したリファレンスをこのストレージにマージします。
        /// </summary>
        /// <param name="reference">マージするリファレンス。</param>
        /// <returns>マージされた、このストレージに所属するリファレンス。</returns>
        public Reference Merge(IReference reference)
        {
            return this.NewReference(
                this.Merge(reference.Activity),
                reference.Name,
                this.Merge(reference.ReferringActivity)
            );
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
        public abstract IEnumerable<Tag> GetTags(
            Nullable<Guid> accountId,
            Nullable<DateTime> timestamp,
            String category,
            String subId,
            String name
        );

        /// <summary>
        /// 値を指定してタグを検索します。
        /// </summary>
        /// <param name="activity">タグが関連付けられているアクティビティ。指定しない場合は <c>null</c>。</param>
        /// <param name="name">タグの意味。指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するタグのシーケンス。</returns>
        public IEnumerable<Tag> GetTags(
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
        public IEnumerable<Tag> GetTags()
        {
            return this.GetTags(null, null, null, null, null);
        }

        /// <summary>
        /// 新しいタグを生成します。
        /// </summary>
        /// <param name="activity">タグが関連付けられるアクティビティ。</param>
        /// <param name="name">タグの意味。</param>
        /// <returns>生成されたタグ。</returns>
        public abstract Tag NewTag(Activity activity, String name);

        /// <summary>
        /// 指定したタグをこのストレージにマージします。
        /// </summary>
        /// <param name="tag">マージするタグ。</param>
        /// <returns>マージされた、このストレージに所属するタグ。</returns>
        public Tag Merge(ITag tag)
        {
            return this.NewTag(
                this.Merge(tag.Activity),
                tag.Name
            );
        }

        #endregion

        /// <summary>
        /// ストレージ オブジェクトをストレージにアタッチします。
        /// </summary>
        /// <param name="obj">アタッチするストレージ オブジェクト。</param>
        public abstract void AttachObject(StorageObject obj);

        /// <summary>
        /// ストレージ オブジェクトをストレージからデタッチします。
        /// </summary>
        /// <param name="obj">デタッチするストレージ オブジェクト。</param>
        public abstract void DetachObject(StorageObject obj);

        /// <summary>
        /// ストレージ オブジェクトを削除の対象としてマークします。
        /// </summary>
        /// <param name="obj">削除の対象としてマークするストレージ オブジェクト。</param>
        public abstract void DeleteObject(StorageObject obj);

        /// <summary>
        /// ストレージ オブジェクトをデータ ソース内のデータで更新します。
        /// </summary>
        /// <param name="refreshMode">更新モードを表す値。</param>
        /// <param name="obj">更新するストレージ オブジェクト。</param>
        public abstract void RefreshObject(RefreshMode refreshMode, StorageObject obj);

        /// <summary>
        /// ストレージ オブジェクトの変更をデータ ソースに保存します。
        /// </summary>
        /// <returns>データ ソースにおいて処理が行われた行数。</returns>
        public abstract Int32 Update();
    }
}