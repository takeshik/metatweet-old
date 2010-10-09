// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetServer.
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
using System.IO;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Transactions;
using XSpect.Configuration;
using log4net;
using System.Threading;
using Achiral;
using Achiral.Extension;
using System.Linq;
using XSpect.Hooking;
using XSpect.MetaTweet.Objects;

namespace XSpect.MetaTweet.Modules
{
    /// <summary>
    /// ストレージ モジュールの抽象基本クラスを提供します。
    /// </summary>
    /// <remarks>
    /// ストレージ モジュールとは、ストレージの機能を提供するモジュールです。即ち、<see cref="Storage"/> にモジュールに必要な機能を実装したクラスです。
    /// </remarks>
    [Serializable()]
    public abstract class StorageModule
        : ObjectContextStorage,
          IModule
    {
        private readonly Subject<StorageObject> _objectCreated;

        /// <summary>
        /// このモジュールがホストされているサーバ オブジェクトを取得します。
        /// </summary>
        /// <value>このモジュールがホストされているサーバ オブジェクト。</value>
        public ServerCore Host
        {
            get;
            private set;
        }

        /// <summary>
        /// このモジュールが生成されたドメインを取得します。
        /// </summary>
        /// <value>
        /// このモジュールが生成されたドメイン。
        /// </value>
        public ModuleDomain Domain
        {
            get;
            private set;
        }

        /// <summary>
        /// このモジュールに設定された名前を取得します。
        /// </summary>
        /// <value>このモジュールに設定された名前。</value>
        public String Name
        {
            get;
            private set;
        }

        /// <summary>
        /// このモジュールに渡されたオプションのリストを取得します。
        /// </summary>
        /// <value>このモジュールに渡されたオプションのリスト。</value>
        public IList<String> Options
        {
            get;
            private set;
        }

        /// <summary>
        /// このモジュールの設定を管理するオブジェクトを取得します。
        /// </summary>
        /// <value>このモジュールの設定を管理するオブジェクト。</value>
        public XmlConfiguration Configuration
        {
            get;
            private set;
        }

        /// <summary>
        /// イベントを記録するログ ライタを取得します。
        /// </summary>
        /// <value>
        /// イベントを記録するログ ライタ。
        /// </value>
        public Log Log
        {
            get
            {
                return this.Host.Log;
            }
        }

        /// <summary>
        /// <see cref="Initialize()"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="Initialize()"/> のフック リスト。
        /// </value>
        public ActionHook<IModule> InitializeHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="Configure(XmlConfiguration)"/> のフック リストを取得します。
        /// </summary>
        /// <value><see cref="Configure(XmlConfiguration)"/> のフック リスト。</value>
        public ActionHook<IModule, XmlConfiguration> ConfigureHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="Dispose()"/> のフック リストを取得します。
        /// </summary>
        /// <value><see cref="Dispose()"/> のフック リスト。</value>
        public ActionHook<IModule> DisposeHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="GetAccounts"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="GetAccounts"/> のフック リスト。
        /// </value>
        public FuncHook<StorageModule, String, String, String, IEnumerable<Account>> GetAccountsHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="NewAccount"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="NewAccount"/> のフック リスト。
        /// </value>
        public FuncHook<StorageModule, String, String, IDictionary<String, String>, Tuple<Account, Boolean>> NewAccountHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="GetActivities"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="GetActivities"/> のフック リスト。
        /// </value>
        public FuncHook<StorageModule, String, Nullable<DateTime>, String, String, String, Object, Object, IEnumerable<Activity>> GetActivitiesHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="NewActivity"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="NewActivity"/> のフック リスト。
        /// </value>
        public FuncHook<StorageModule, Account, DateTime, String, String, String, String, Byte[], Tuple<Activity, Boolean>> NewActivityHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="GetAnnotations"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="GetAnnotations"/> のフック リスト。
        /// </value>
        public FuncHook<StorageModule, String, String, String, IEnumerable<Annotation>> GetAnnotationsHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="NewAnnotation"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="NewAnnotation"/> のフック リスト。
        /// </value>
        public FuncHook<StorageModule, Account, String, String, Tuple<Annotation, Boolean>> NewAnnotationHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="GetRelations"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="GetRelations"/> のフック リスト。
        /// </value>
        public FuncHook<StorageModule, String, String, String, IEnumerable<Relation>> GetRelationsHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="NewRelation"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="NewRelation"/> のフック リスト。
        /// </value>
        public FuncHook<StorageModule, Account, String, Account, Tuple<Relation, Boolean>> NewRelationHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="GetMarks"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="GetMarks"/> のフック リスト。
        /// </value>
        public FuncHook<StorageModule, String, String, String, Nullable<DateTime>, String, String, IEnumerable<Mark>> GetMarksHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="NewMark"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="NewMark"/> のフック リスト。
        /// </value>
        public FuncHook<StorageModule, Account, String, Activity, Tuple<Mark, Boolean>> NewMarkHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="GetReferences"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="GetReferences"/> のフック リスト。
        /// </value>
        public FuncHook<StorageModule, String, Nullable<DateTime>, String, String, String, String, Nullable<DateTime>, String, String, IEnumerable<Reference>> GetReferencesHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="NewReference"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="NewReference"/> のフック リスト。
        /// </value>
        public FuncHook<StorageModule, Activity, String, Activity, Tuple<Reference, Boolean>> NewReferenceHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="GetTags"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="GetTags"/> のフック リスト。
        /// </value>
        public FuncHook<StorageModule, String, Nullable<DateTime>, String, String, String, String, IEnumerable<Tag>> GetTagsHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="NewTag"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="NewTag"/> のフック リスト。
        /// </value>
        public FuncHook<StorageModule, Activity, String, String, Tuple<Tag, Boolean>> NewTagHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="Update"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="Update"/> のフック リスト。
        /// </value>
        public FuncHook<StorageModule, Int32> UpdateHook
        {
            get;
            private set;
        }

        /// <summary>
        /// 新しいストレージ オブジェクトの生成を監視するためのオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// 新しいストレージ オブジェクトの生成を監視するためのオブジェクト。
        /// </value>
        public IObservable<StorageObject> ObjectCreated
        {
            get
            {
                return this._objectCreated.Remotable();
            }
        }

        /// <summary>
        /// <see cref="StorageModule"/> の新しいインスタンスを初期化します。
        /// </summary>
        protected StorageModule()
        {
            this._objectCreated = new Subject<StorageObject>();
            this.InitializeHook = new ActionHook<IModule>(this.InitializeImpl);
            this.ConfigureHook = new ActionHook<IModule, XmlConfiguration>(c => this.ConfigureImpl());
            this.DisposeHook = new ActionHook<IModule>(base.Dispose);
            this.GetAccountsHook = new FuncHook<StorageModule, String, String, String, IEnumerable<Account>>(this._GetAccounts);
            this.GetActivitiesHook = new FuncHook<StorageModule, String, Nullable<DateTime>, String, String, String, Object, Object, IEnumerable<Activity>>(this._GetActivities);
            this.GetAnnotationsHook = new FuncHook<StorageModule, String, String, String, IEnumerable<Annotation>>(this._GetAnnotations);
            this.GetRelationsHook = new FuncHook<StorageModule, String, String, String, IEnumerable<Relation>>(this._GetRelations);
            this.GetMarksHook = new FuncHook<StorageModule, String, String, String, Nullable<DateTime>, String, String, IEnumerable<Mark>>(this._GetMarks);
            this.GetReferencesHook = new FuncHook<StorageModule, String, Nullable<DateTime>, String, String, String, String, Nullable<DateTime>, String, String, IEnumerable<Reference>>(this._GetReferences);
            this.GetTagsHook = new FuncHook<StorageModule, String, Nullable<DateTime>, String, String, String, String, IEnumerable<Tag>>(this._GetTags);
            this.NewAccountHook = new FuncHook<StorageModule, String, String, IDictionary<String, String>, Tuple<Account, Boolean>>(this._NewAccount);
            this.NewActivityHook = new FuncHook<StorageModule, Account, DateTime, String, String, String, String, Byte[], Tuple<Activity, Boolean>>(this._NewActivity);
            this.NewAnnotationHook = new FuncHook<StorageModule, Account, String, String, Tuple<Annotation, Boolean>>(this._NewAnnotation);
            this.NewRelationHook = new FuncHook<StorageModule, Account, String, Account, Tuple<Relation, Boolean>>(this._NewRelation);
            this.NewMarkHook = new FuncHook<StorageModule, Account, String, Activity, Tuple<Mark, Boolean>>(this._NewMark);
            this.NewReferenceHook = new FuncHook<StorageModule, Activity, String, Activity, Tuple<Reference, Boolean>>(this._NewReference);
            this.NewTagHook = new FuncHook<StorageModule, Activity, String, String, Tuple<Tag, Boolean>>(this._NewTag);
            this.UpdateHook = new FuncHook<StorageModule, Int32>(this._Update);
        }

        /// <summary>
        /// <see cref="Storage"/> によって使用されているすべてのリソースを解放します。
        /// </summary>
        public new void Dispose()
        {
            this.DisposeHook.Execute();
        }

        /// <summary>
        /// このモジュールを表す文字列を返します。
        /// </summary>
        /// <returns>このモジュールを表す文字列。</returns>
        public override String ToString()
        {
            return this.GetType().Name + "-" + this.Name;
        }

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
            return this.GetAccountsHook.Execute(accountId, realm, seedString);
        }

        /// <summary>
        /// 新しいアカウントを生成します。
        /// </summary>
        /// <param name="accountId">アカウントの ID。</param>
        /// <param name="realm">アカウントのレルム。</param>
        /// <param name="seeds">アカウントのシード値。</param>
        /// <param name="created">アカウントが新規に生成された場合は <c>true</c>。それ以外の場合、つまり既存のアカウントが取得された場合は <c>false</c> が返されます。このパラメータは初期化せずに渡されます。</param>
        /// <returns>生成されたアカウント。</returns>
        public override Account NewAccount(
            String accountId,
            String realm,
            IDictionary<String, String> seeds,
            out Boolean created
        )
        {
            Tuple<Account, Boolean> result = this.NewAccountHook.Execute(accountId, realm, seeds);
            created = result.Item2;
            if (created)
            {
                this._objectCreated.OnNext(result.Item1);
            }
            return result.Item1;
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
            return this.GetActivitiesHook.Execute(accountId, timestamp, category, subId, userAgent, value, data);
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
        public override Activity NewActivity(
            Account account,
            DateTime timestamp,
            String category,
            String subId,
            String userAgent,
            String value,
            Byte[] data,
            out Boolean created
        )
        {
            Tuple<Activity, Boolean> result = this.NewActivityHook.Execute(account, timestamp, category, subId, userAgent, value, data);
            created = result.Item2;
            if (created)
            {
                this._objectCreated.OnNext(result.Item1);
            }
            return result.Item1;
        }

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
            return this.GetAnnotationsHook.Execute(accountId, name, value);
        }

        /// <summary>
        /// 新しいアノテーションを生成します。
        /// </summary>
        /// <param name="account">アノテーションが関連付けられるアカウント。</param>
        /// <param name="name">アノテーションの意味。</param>
        /// <param name="value">アノテーションの値。</param>
        /// <param name="created">アノテーションが新規に生成された場合は <c>true</c>。それ以外の場合、つまり既存のアノテーションが取得された場合は <c>false</c> が返されます。このパラメータは初期化せずに渡されます。</param>
        /// <returns>生成されたアノテーション。</returns>
        public override Annotation NewAnnotation(
            Account account,
            String name,
            String value,
            out Boolean created
        )
        {
            Tuple<Annotation, Boolean> result = this.NewAnnotationHook.Execute(account, name, value);
            created = result.Item2;
            if (created)
            {
                this._objectCreated.OnNext(result.Item1);
            }
            return result.Item1;
        }

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
            return this.GetRelationsHook.Execute(accountId, name, relatingAccountId);
        }

        /// <summary>
        /// 新しいリレーションを生成します。
        /// </summary>
        /// <param name="account">リレーションが関連付けられるアカウント。</param>
        /// <param name="name">リレーションの意味。</param>
        /// <param name="relatingAccount">リレーションが関連付けられる先のアカウント。</param>
        /// <param name="created">リレーションが新規に生成された場合は <c>true</c>。それ以外の場合、つまり既存のリレーションが取得された場合は <c>false</c> が返されます。このパラメータは初期化せずに渡されます。</param>
        /// <returns>生成されたリレーション。</returns>
        public override Relation NewRelation(
            Account account,
            String name,
            Account relatingAccount,
            out Boolean created
        )
        {
            Tuple<Relation, Boolean> result = this.NewRelationHook.Execute(account, name, relatingAccount);
            created = result.Item2;
            if (created)
            {
                this._objectCreated.OnNext(result.Item1);
            }
            return result.Item1;
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
        public override IEnumerable<Mark> GetMarks(
            String accountId,
            String name,
            String markingAccountId,
            Nullable<DateTime> markingTimestamp,
            String markingCategory,
            String markingSubId
        )
        {
            return this.GetMarksHook.Execute(accountId, name, markingAccountId, markingTimestamp, markingCategory, markingSubId);
        }

        /// <summary>
        /// 新しいマークを生成します。
        /// </summary>
        /// <param name="account">マークが関連付けられるアカウント。</param>
        /// <param name="name">マークの意味。</param>
        /// <param name="markingActivity">マークが関連付けられる先のアカウント。</param>
        /// <param name="created">マークが新規に生成された場合は <c>true</c>。それ以外の場合、つまり既存のマークが取得された場合は <c>false</c> が返されます。このパラメータは初期化せずに渡されます。</param>
        /// <returns>生成されたマーク。</returns>
        public override Mark NewMark(
            Account account,
            String name,
            Activity markingActivity,
            out Boolean created
        )
        {
            Tuple<Mark, Boolean> result = this.NewMarkHook.Execute(account, name, markingActivity);
            created = result.Item2;
            if (created)
            {
                this._objectCreated.OnNext(result.Item1);
            }
            return result.Item1;
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
            return this.GetReferencesHook.Execute(accountId, timestamp, category, subId, name, referringAccountId, referringTimestamp, referringCategory, referringSubId);
        }

        /// <summary>
        /// 新しいリファレンスを生成します。
        /// </summary>
        /// <param name="activity">リファレンスが関連付けられているアクティビティ。</param>
        /// <param name="name">リファレンスの意味。</param>
        /// <param name="referringActivity">リファレンスが関連付けられる先のアクティビティ。</param>
        /// <param name="created">リファレンスが新規に生成された場合は <c>true</c>。それ以外の場合、つまり既存のリファレンスが取得された場合は <c>false</c> が返されます。このパラメータは初期化せずに渡されます。</param>
        /// <returns>生成されたリファレンス。</returns>
        public override Reference NewReference(
            Activity activity,
            String name,
            Activity referringActivity,
            out Boolean created
        )
        {
            Tuple<Reference, Boolean> result = this.NewReferenceHook.Execute(activity, name, referringActivity);
            created = result.Item2;
            if (created)
            {
                this._objectCreated.OnNext(result.Item1);
            }
            return result.Item1;
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
        public override IEnumerable<Tag> GetTags(
            String accountId,
            Nullable<DateTime> timestamp,
            String category,
            String subId,
            String name,
            String value
        )
        {
            return this.GetTagsHook.Execute(accountId, timestamp, category, subId, name, value);
        }

        /// <summary>
        /// 新しいタグを生成します。
        /// </summary>
        /// <param name="activity">タグが関連付けられるアクティビティ。</param>
        /// <param name="name">タグの意味。</param>
        /// <param name="value">タグの値。</param>
        /// <param name="created">タグが新規に生成された場合は <c>true</c>。それ以外の場合、つまり既存のタグが取得された場合は <c>false</c> が返されます。このパラメータは初期化せずに渡されます。</param>
        /// <returns>生成されたタグ。</returns>
        public override Tag NewTag(
            Activity activity,
            String name,
            String value,
            out Boolean created
        )
        {
            Tuple<Tag, Boolean> result = this.NewTagHook.Execute(activity, name, value);
            created = result.Item2;
            if (created)
            {
                this._objectCreated.OnNext(result.Item1);
            }
            return result.Item1;
        }

        /// <summary>
        /// バックエンドのデータソースとの接続を初期化します。既に接続が存在する場合は、新たに接続を初期化し直します。
        /// </summary>
        public new void InitializeContext()
        {
            this.InitializeContext(this.Configuration.ResolveValue<String>("connection"));
        }

        /// <summary>
        /// このモジュールをサーバ オブジェクトに登録します。
        /// </summary>
        /// <param name="domain">登録されるモジュール ドメイン。</param>
        /// <param name="name">モジュールに設定する名前。</param>
        /// <param name="options">モジュールに渡されたオプションのリスト。</param>
        public virtual void Register(ModuleDomain domain, String name, IList<String> options)
        {
            this.Domain = domain;
            this.Host = domain.Parent.Parent;
            this.Name = name;
            this.Options = options;
        }

        /// <summary>
        /// このモジュールを初期化します。
        /// </summary>
        /// <remarks>
        /// このメソッドはモジュールの寿命中、複数回呼び出される可能性があります。
        /// </remarks>
        public void Initialize()
        {
            this.InitializeHook.Execute();
        }

        /// <summary>
        /// このモジュールを初期化します。
        /// </summary>
        protected virtual void InitializeImpl()
        {
            if (this.Configuration.Exists("connection"))
            {
                this.InitializeContext();
            }
        }

        /// <summary>
        /// このモジュールの設定を行います。
        /// </summary>
        /// <param name="configuration">設定を取得する <see cref="XmlConfiguration"/> オブジェクト。</param>
        public void Configure(XmlConfiguration configuration)
        {
            this.Configuration = configuration;
            this.ConfigureHook.Execute(configuration);
        }

        /// <summary>
        /// 派生クラスで実装された場合、実際の設定処理を行います。
        /// </summary>
        protected virtual void ConfigureImpl()
        {
        }

        /// <summary>
        /// リモート オブジェクトとの通信に使用するプロキシの生成に必要な情報をすべて格納しているオブジェクトを作成します。
        /// </summary>
        /// <returns>プロキシを生成するのに必要な情報。</returns>
        public ObjRef CreateObjRef()
        {
            return this.Domain.DoCallback(() => this.CreateObjRef(this.GetType()));
        }

        /// <summary>
        /// ロックが解除されるのを待って、ストレージ オブジェクトの変更をデータ ソースに保存します。
        /// </summary>
        /// <returns>データ ソースにおいて処理が行われた行数。</returns>
        public override Int32 Update()
        {
            return this.UpdateHook.Execute();
        }

        #region Helper Methods

        private IEnumerable<Account> _GetAccounts(
            String accountId,
            String realm,
            String seedString
        )
        {
            return base.GetAccounts(accountId, realm, seedString);
        }

        private Tuple<Account, Boolean> _NewAccount(
            String accountId,
            String realm,
            IDictionary<String, String> seeds
        )
        {
            Boolean created;
            return new Tuple<Account, Boolean>(base.NewAccount(accountId, realm, seeds, out created), created);
        }

        private IEnumerable<Activity> _GetActivities(
            String accountId,
            Nullable<DateTime> timestamp,
            String category,
            String subId,
            String userAgent,
            Object value,
            Object data
        )
        {
            return base.GetActivities(accountId, timestamp, category, subId, userAgent, value, data);
        }

        private Tuple<Activity, Boolean> _NewActivity(
            Account account,
            DateTime timestamp,
            String category,
            String subId,
            String userAgent,
            String value,
            Byte[] data
        )
        {
            Boolean created;
            return new Tuple<Activity, Boolean>(base.NewActivity(account, timestamp, category, subId, userAgent, value, data, out created), created);
        }

        private IEnumerable<Annotation> _GetAnnotations(
            String accountId,
            String name,
            String value
        )
        {
            return base.GetAnnotations(accountId, name, value);
        }

        private Tuple<Annotation, Boolean> _NewAnnotation(
            Account account,
            String name,
            String value
        )
        {
            Boolean created;
            return new Tuple<Annotation, Boolean>(base.NewAnnotation(account, name, value, out created), created);
        }

        private IEnumerable<Relation> _GetRelations(
            String accountId,
            String name,
            String relatingAccountId
        )
        {
            return base.GetRelations(accountId, name, relatingAccountId);
        }

        private Tuple<Relation, Boolean> _NewRelation(
            Account account,
            String name,
            Account relatingAccount
        )
        {
            Boolean created;
            return new Tuple<Relation, Boolean>(base.NewRelation(account, name, relatingAccount, out created), created);
        }

        private IEnumerable<Mark> _GetMarks(
            String accountId,
            String name,
            String markingAccountId,
            Nullable<DateTime> markingTimestamp,
            String markingCategory,
            String markingSubId
        )
        {
            return base.GetMarks(accountId, name, markingAccountId, markingTimestamp, markingCategory, markingSubId);
        }

        private Tuple<Mark, Boolean> _NewMark(
            Account account,
            String name,
            Activity markingActivity
        )
        {
            Boolean created;
            return new Tuple<Mark, Boolean>(base.NewMark(account, name, markingActivity, out created), created);
        }

        private IEnumerable<Reference> _GetReferences(
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
            return base.GetReferences(accountId, timestamp, category, subId, name, referringAccountId, referringTimestamp, referringCategory, referringSubId);
        }

        private Tuple<Reference, Boolean> _NewReference(
            Activity activity,
            String name,
            Activity referringActivity
        )
        {
            Boolean created;
            return new Tuple<Reference, Boolean>(base.NewReference(activity, name, referringActivity, out created), created);
        }

        private IEnumerable<Tag> _GetTags(
            String accountId,
            Nullable<DateTime> timestamp,
            String category,
            String subId,
            String name,
            String value
        )
        {
            return base.GetTags(accountId, timestamp, category, subId, name, value);
        }

        private Tuple<Tag, Boolean> _NewTag(
            Activity activity,
            String name,
            String value
        )
        {
            Boolean created;
            return new Tuple<Tag, Boolean>(base.NewTag(activity, name, value, out created), created);
        }

        private Int32 _Update()
        {
            return base.Update();
        }

        #endregion
    }
}