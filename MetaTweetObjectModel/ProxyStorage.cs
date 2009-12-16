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

namespace XSpect.MetaTweet.Objects
{
    /// <summary>
    /// 他のストレージに間接的にアクセスする機能を提供します。
    /// </summary>
    public class ProxyStorage
        : Storage
    {
        public override StorageCache Cache
        {
            get
            {
                return this.Target.Cache;
            }
            set
            {
                this.Target.Cache = value;
            }
        }

        public Storage Target
        {
            get;
            private set;
        }

        public ProxyStorage(Storage target)
        {
            this.Target = target;
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
            return this.Target.GetAccounts(accountId, realm);
        }

        /// <summary>
        /// 新しいアカウントを生成します。
        /// </summary>
        /// <param name="accountId">アカウントの ID。</param>
        /// <param name="realm">アカウントのレルム。</param>
        /// <returns>生成されたアカウント。</returns>
        public override Account NewAccount(Guid accountId, String realm)
        {
            return this.Target.NewAccount(accountId, realm);
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
            return this.Target.GetActivities(accountId, timestamp, category, subId, userAgent, value, data);
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
            return this.Target.NewActivity(account, timestamp, category, subId, userAgent, value, data);
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
            return this.Target.GetAnnotations(accountId, name);
        }

        /// <summary>
        /// 新しいアノテーションを生成します。
        /// </summary>
        /// <param name="account">アノテーションが関連付けられるアカウント。</param>
        /// <param name="name">アノテーションの意味。</param>
        /// <returns>生成されたアノテーション。</returns>
        public override Annotation NewAnnotation(Account account, String name)
        {
            return this.Target.NewAnnotation(account, name);
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
            return this.Target.GetRelations(accountId, name, relatingAccountId);
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
            return this.Target.NewRelation(account, name, relatingAccount);
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
            return this.Target.GetMarks(accountId, name, markingAccountId, markingTimestamp, markingCategory, markingSubId);
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
            return this.Target.NewMark(account, name, markingActivity);
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
            return this.Target.GetReferences(accountId, timestamp, category, subId, name, referringAccountId, referringTimestamp, referringCategory, referringSubId);
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
            return this.Target.NewReference(activity, name, referringActivity);
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
            return this.Target.GetTags(accountId, timestamp, category, subId, name);
        }

        /// <summary>
        /// 新しいタグを生成します。
        /// </summary>
        /// <param name="activity">タグが関連付けられるアクティビティ。</param>
        /// <param name="name">タグの意味。</param>
        /// <returns>生成されたタグ。</returns>
        public override Tag NewTag(Activity activity, String name)
        {
            return this.Target.NewTag(activity, name);
        }

        #endregion

        /// <summary>
        /// ストレージ オブジェクトをストレージにアタッチします。
        /// </summary>
        /// <param name="obj">アタッチするストレージ オブジェクト。</param>
        public override void AttachObject(StorageObject obj)
        {
            this.Target.AttachObject(obj);
        }

        /// <summary>
        /// ストレージ オブジェクトをストレージからデタッチします。
        /// </summary>
        /// <param name="obj">デタッチするストレージ オブジェクト。</param>
        public override void DetachObject(StorageObject obj)
        {
            this.Target.DetachObject(obj);
        }

        /// <summary>
        /// ストレージ オブジェクトを削除の対象としてマークします。
        /// </summary>
        /// <param name="obj">削除の対象としてマークするストレージ オブジェクト。</param>
        public override void DeleteObject(StorageObject obj)
        {
            this.Target.DeleteObject(obj);
        }

        /// <summary>
        /// ストレージ オブジェクトをデータ ソース内のデータで更新します。
        /// </summary>
        /// <param name="refreshMode">更新モードを表す値。</param>
        /// <param name="obj">更新するストレージ オブジェクト。</param>
        public override void RefreshObject(RefreshMode refreshMode, StorageObject obj)
        {
            this.Target.RefreshObject(refreshMode, obj);
        }

        /// <summary>
        /// ストレージ オブジェクトの変更をデータ ソースに保存します。
        /// </summary>
        /// <returns>データ ソースにおいて処理が行われた行数。</returns>
        public override Int32 Update()
        {
            return this.Target.Update();
        }
    }
}