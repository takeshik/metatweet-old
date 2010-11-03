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
using System.Data.Objects;

namespace XSpect.MetaTweet.Objects
{
    /// <summary>
    /// 他のストレージに間接的にアクセスする機能を提供します。
    /// </summary>
    [Serializable()]
    public class ProxyStorage
        : Storage
    {
        /// <summary>
        /// このストレージが委譲のために参照するストレージを取得します。
        /// </summary>
        /// <value>このストレージが委譲のために参照するストレージ。</value>
        public Storage Target
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="ProxyStorage"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="target">委譲のために参照するストレージ。</param>
        public ProxyStorage(Storage target)
        {
            this.Target = target;
        }

        #region Account

        /// <summary>
        /// 値を指定してアカウントを検索します。
        /// </summary>
        /// <returns>指定した条件に合致するアカウントのシーケンス。</returns>
        public override IEnumerable<Account> GetAccounts(StorageObjectQuery<Account, AccountTuple> query)
        {
            return this.Target.GetAccounts(query);
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
            return this.Target.NewAccount(accountId, realm, seeds, out created);
        }

        #endregion

        #region Activity

        /// <summary>
        /// 値を指定してアクティビティを検索します。
        /// </summary>
        /// <returns>指定した条件に合致するアクティビティのシーケンス。</returns>
        public override IEnumerable<Activity> GetActivities(StorageObjectQuery<Activity, ActivityTuple> query)
        {
            return this.Target.GetActivities(query);
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
            return this.Target.NewActivity(account, timestamp, category, subId, userAgent, value, data, out created);
        }

        #endregion

        #region Annotation

        /// <summary>
        /// 値を指定してアノテーションを検索します。
        /// </summary>
        /// <returns>指定した条件に合致するアノテーションのシーケンス。</returns>
        public override IEnumerable<Annotation> GetAnnotations(StorageObjectQuery<Annotation, AnnotationTuple> query)
        {
            return this.Target.GetAnnotations(query);
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
            return this.Target.NewAnnotation(account, name, value, out created);
        }

        #endregion

        #region Relation

        /// <summary>
        /// 値を指定してリレーションを検索します。
        /// </summary>
        /// <returns>指定した条件に合致するリレーションのシーケンス。</returns>
        public override IEnumerable<Relation> GetRelations(StorageObjectQuery<Relation, RelationTuple> query)
        {
            return this.Target.GetRelations(query);
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
            return this.Target.NewRelation(account, name, relatingAccount, out created);
        }

        #endregion

        #region Mark

        /// <summary>
        /// 値を指定してマークを検索します。
        /// </summary>
        /// <returns>指定した条件に合致するマークのシーケンス。</returns>
        public override IEnumerable<Mark> GetMarks(StorageObjectQuery<Mark, MarkTuple> query)
        {
            return this.Target.GetMarks(query);
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
            return this.Target.NewMark(account, name, markingActivity, out created);
        }

        #endregion

        #region Reference

        /// <summary>
        /// 値を指定してリファレンスを検索します。
        /// </summary>
        /// <returns>指定した条件に合致するリファレンスのシーケンス。</returns>
        public override IEnumerable<Reference> GetReferences(StorageObjectQuery<Reference, ReferenceTuple> query)
        {
            return this.Target.GetReferences(query);
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
            return this.Target.NewReference(activity, name, referringActivity, out created);
        }

        #endregion

        #region Tag

        /// <summary>
        /// 値を指定してタグを検索します。
        /// </summary>
        /// <returns>条件に合致するタグのシーケンス。</returns>
        public override IEnumerable<Tag> GetTags(StorageObjectQuery<Tag, TagTuple> query)
        {
            return this.Target.GetTags(query);
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
            return this.Target.NewTag(activity, name, value, out created);
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