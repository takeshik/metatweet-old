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
                return Module.GetLogImpl(this);
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
        public FuncHook<StorageModule, StorageObjectQuery<Account, AccountTuple>, IEnumerable<Account>> GetAccountsHook
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
        public FuncHook<StorageModule, StorageObjectQuery<Activity, ActivityTuple>, IEnumerable<Activity>> GetActivitiesHook
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
        public FuncHook<StorageModule, StorageObjectQuery<Annotation, AnnotationTuple>, IEnumerable<Annotation>> GetAnnotationsHook
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
        public FuncHook<StorageModule, StorageObjectQuery<Relation, RelationTuple>, IEnumerable<Relation>> GetRelationsHook
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
        public FuncHook<StorageModule, StorageObjectQuery<Mark, MarkTuple>, IEnumerable<Mark>> GetMarksHook
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
        public FuncHook<StorageModule, StorageObjectQuery<Reference, ReferenceTuple>, IEnumerable<Reference>> GetReferencesHook
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
        public FuncHook<StorageModule, StorageObjectQuery<Tag, TagTuple>, IEnumerable<Tag>> GetTagsHook
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
            this.GetAccountsHook = new FuncHook<StorageModule, StorageObjectQuery<Account, AccountTuple>, IEnumerable<Account>>(this._GetAccounts);
            this.GetActivitiesHook = new FuncHook<StorageModule, StorageObjectQuery<Activity, ActivityTuple>, IEnumerable<Activity>>(this._GetActivities);
            this.GetAnnotationsHook = new FuncHook<StorageModule, StorageObjectQuery<Annotation, AnnotationTuple>, IEnumerable<Annotation>>(this._GetAnnotations);
            this.GetRelationsHook = new FuncHook<StorageModule, StorageObjectQuery<Relation, RelationTuple>, IEnumerable<Relation>>(this._GetRelations);
            this.GetMarksHook = new FuncHook<StorageModule, StorageObjectQuery<Mark, MarkTuple>, IEnumerable<Mark>>(this._GetMarks);
            this.GetReferencesHook = new FuncHook<StorageModule, StorageObjectQuery<Reference, ReferenceTuple>, IEnumerable<Reference>>(this._GetReferences);
            this.GetTagsHook = new FuncHook<StorageModule, StorageObjectQuery<Tag, TagTuple>, IEnumerable<Tag>>(this._GetTags);
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
            return Module.ToStringImpl(this);
        }

        public override IEnumerable<Account> GetAccounts(StorageObjectQuery<Account, AccountTuple> query)
        {
            return this.GetAccountsHook.Execute(query);
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

        public override IEnumerable<Activity> GetActivities(StorageObjectQuery<Activity, ActivityTuple> query)
        {
            return this.GetActivitiesHook.Execute(query);
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

        public override IEnumerable<Annotation> GetAnnotations(StorageObjectQuery<Annotation, AnnotationTuple> query)
        {
            return this.GetAnnotationsHook.Execute(query);
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

        public override IEnumerable<Relation> GetRelations(StorageObjectQuery<Relation, RelationTuple> query)
        {
            return this.GetRelationsHook.Execute(query);
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

        public override IEnumerable<Mark> GetMarks(StorageObjectQuery<Mark, MarkTuple> query)
        {
            return this.GetMarksHook.Execute(query);
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

        public override IEnumerable<Reference> GetReferences(StorageObjectQuery<Reference, ReferenceTuple> query)
        {
            return this.GetReferencesHook.Execute(query);
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

        public override IEnumerable<Tag> GetTags(StorageObjectQuery<Tag, TagTuple> query)
        {
            return this.GetTagsHook.Execute(query);
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

        private IEnumerable<Account> _GetAccounts(StorageObjectQuery<Account, AccountTuple> query)
        {
            return base.GetAccounts(query);
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

        private IEnumerable<Activity> _GetActivities(StorageObjectQuery<Activity, ActivityTuple> query)
        {
            return base.GetActivities(query);
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

        private IEnumerable<Annotation> _GetAnnotations(StorageObjectQuery<Annotation, AnnotationTuple> query)
        {
            return base.GetAnnotations(query);
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

        private IEnumerable<Relation> _GetRelations(StorageObjectQuery<Relation, RelationTuple> query)
        {
            return base.GetRelations(query);
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

        private IEnumerable<Mark> _GetMarks(StorageObjectQuery<Mark, MarkTuple> query)
        {
            return base.GetMarks(query);
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

        private IEnumerable<Reference> _GetReferences(StorageObjectQuery<Reference, ReferenceTuple> query)
        {
            return base.GetReferences(query);
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

        private IEnumerable<Tag> _GetTags(StorageObjectQuery<Tag, TagTuple> query)
        {
            return base.GetTags(query);
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