// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
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
using System.Dynamic;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Transactions;
using log4net;
using System.Threading;
using Achiral;
using Achiral.Extension;
using System.Linq;
using XSpect.Extension;
using XSpect.MetaTweet.Objects;
using XSpect.MetaTweet.Properties;

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
        /// このモジュールの設定を保持するオブジェクトを取得します。
        /// </summary>
        /// <value>このモジュールの設定を保持するオブジェクト。</value>
        public dynamic Configuration
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
        }

        /// <summary>
        /// このモジュールを表す文字列を返します。
        /// </summary>
        /// <returns>このモジュールを表す文字列。</returns>
        public override String ToString()
        {
            return Module.ToStringImpl(this);
        }

        protected override void Dispose(Boolean disposing)
        {
            this.Log.Info(Resources.ModuleObjectDisposing, this.Name);
            base.Dispose(disposing);
            this.Log.Info(Resources.ModuleObjectDisposed, this.Name);
        }

        public override IEnumerable<Account> GetAccounts(IStorageObjectQuery<Account> query)
        {
            IEnumerable<Account> ret = base.GetAccounts(query);
            this.Log.Verbose(
                Resources.StorageGotAccounts,
                this.Name,
                query.ToString().Indent(2),
                ret.Count().If(i => i > 1, i => i + " objects", i => i + " object")
            );
            return ret;
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
            Tuple<Account, Boolean> result = Tuple.Create(base.NewAccount(accountId, realm, seeds, out created), created);
            created = result.Item2;
            if (created)
            {
                this._objectCreated.OnNext(result.Item1);
            }
            this.Log.Verbose(
                created ? Resources.StorageAddedAccount : Resources.StorageAddedExistingAccount,
                this.Name,
                result.Item1
            );
            return result.Item1;
        }

        public override IEnumerable<Activity> GetActivities(IStorageObjectQuery<Activity> query)
        {
            IEnumerable<Activity> ret = base.GetActivities(query);
            this.Log.Verbose(
                Resources.StorageGotActivities,
                this.Name,
                query.ToString().Indent(2),
                ret.Count().If(i => i > 1, i => i + " objects", i => i + " object")
            );
            return ret;
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
            Tuple<Activity, Boolean> result = Tuple.Create(base.NewActivity(account, timestamp, category, subId, userAgent, value, data, out created), created);
            created = result.Item2;
            if (created)
            {
                this._objectCreated.OnNext(result.Item1);
            }
            this.Log.Verbose(
                created ? Resources.StorageAddedActivity : Resources.StorageAddedExistingActivity,
                this.Name,
                result.Item1
            );
            return result.Item1;
        }

        public override IEnumerable<Annotation> GetAnnotations(IStorageObjectQuery<Annotation> query)
        {
            IEnumerable<Annotation> ret = base.GetAnnotations(query);
            this.Log.Verbose(
                Resources.StorageGotAnnotations,
                this.Name,
                query.ToString().Indent(2),
                ret.Count().If(i => i > 1, i => i + " objects", i => i + " object")
            );
            return ret;
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
            Tuple<Annotation, Boolean> result = Tuple.Create(base.NewAnnotation(account, name, value, out created), created);
            created = result.Item2;
            if (created)
            {
                this._objectCreated.OnNext(result.Item1);
            }
            this.Log.Verbose(
                created ? Resources.StorageAddedAnnotation : Resources.StorageAddedExistingAnnotation,
                this.Name,
                result.Item1
            );
            return result.Item1;
        }

        public override IEnumerable<Relation> GetRelations(IStorageObjectQuery<Relation> query)
        {
            IEnumerable<Relation> ret = base.GetRelations(query);
            this.Log.Verbose(
                Resources.StorageGotRelations,
                this.Name,
                query.ToString().Indent(2),
                ret.Count().If(i => i > 1, i => i + " objects", i => i + " object")
            );
            return ret;
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
            Tuple<Relation, Boolean> result = Tuple.Create(base.NewRelation(account, name, relatingAccount, out created), created);
            created = result.Item2;
            if (created)
            {
                this._objectCreated.OnNext(result.Item1);
            }
            this.Log.Verbose(
                created ? Resources.StorageAddedRelation : Resources.StorageAddedExistingRelation,
                this.Name,
                result.Item1
            );
            return result.Item1;
        }

        public override IEnumerable<Mark> GetMarks(IStorageObjectQuery<Mark> query)
        {
            IEnumerable<Mark> ret = base.GetMarks(query);
            this.Log.Verbose(
                Resources.StorageGotMarks,
                this.Name,
                query.ToString().Indent(2),
                ret.Count().If(i => i > 1, i => i + " objects", i => i + " object")
            );
            return ret;
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
            Tuple<Mark, Boolean> result = Tuple.Create(base.NewMark(account, name, markingActivity, out created), created);
            created = result.Item2;
            if (created)
            {
                this._objectCreated.OnNext(result.Item1);
            }
            this.Log.Verbose(
                created ? Resources.StorageAddedMark : Resources.StorageAddedExistingMark,
                this.Name,
                result.Item1
            );
            return result.Item1;
        }

        public override IEnumerable<Reference> GetReferences(IStorageObjectQuery<Reference> query)
        {
            IEnumerable<Reference> ret = base.GetReferences(query);
            this.Log.Verbose(
                Resources.StorageGotReferences,
                this.Name,
                query.ToString().Indent(2),
                ret.Count().If(i => i > 1, i => i + " objects", i => i + " object")
            );
            return ret;
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
            Tuple<Reference, Boolean> result = Tuple.Create(base.NewReference(activity, name, referringActivity, out created), created);
            created = result.Item2;
            if (created)
            {
                this._objectCreated.OnNext(result.Item1);
            }
            this.Log.Verbose(
                created ? Resources.StorageAddedReference : Resources.StorageAddedExistingReference,
                this.Name,
                result.Item1
            );
            return result.Item1;
        }

        public override IEnumerable<Tag> GetTags(IStorageObjectQuery<Tag> query)
        {
            IEnumerable<Tag> ret = base.GetTags(query);
            this.Log.Verbose(
                Resources.StorageGotTags,
                this.Name,
                query.ToString().Indent(2),
                ret.Count().If(i => i > 1, i => i + " objects", i => i + " object")
            );
            return ret;
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
            Tuple<Tag, Boolean> result = Tuple.Create(base.NewTag(activity, name, value, out created), created);
            created = result.Item2;
            if (created)
            {
                this._objectCreated.OnNext(result.Item1);
            }
            this.Log.Verbose(
                created ? Resources.StorageAddedTag : Resources.StorageAddedExistingTag,
                this.Name,
                result.Item1
            );
            return result.Item1;
        }

        public override Int32 Update()
        {
            Int32 ret = base.Update();
            this.Log.Debug(
                Resources.StorageUpdated,
                this.Name,
                ret.If(i => i > 1, i => i + " objects", i => i + " object")
            );
            return ret;
        }

        /// <summary>
        /// バックエンドのデータソースとの接続を初期化します。既に接続が存在する場合は、新たに接続を初期化し直します。
        /// </summary>
        public new void InitializeContext()
        {
            this.InitializeContext(this.Configuration.Connection);
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
            this.CheckIfDisposed();
            this.Log.Info(Resources.ModuleObjectInitializing, this.Name);
            this.InitializeImpl();
            this.Log.Info(Resources.ModuleObjectInitialized, this.Name);
        }

        /// <summary>
        /// このモジュールを初期化します。
        /// </summary>
        protected virtual void InitializeImpl()
        {
            this.InitializeContext();
        }

        /// <summary>
        /// このモジュールの設定を行います。
        /// </summary>
        /// <param name="configFile">設定ファイル。</param>
        public void Configure(FileInfo configFile)
        {
            this.CheckIfDisposed();
            ConfigureImpl(configFile);
            this.Log.Info(Resources.ModuleObjectInitializing, this.Name);
        }

        /// <summary>
        /// 派生クラスで実装された場合、実際の設定処理を行います。
        /// </summary>
        /// <param name="configFile">設定ファイル。</param>
        protected virtual void ConfigureImpl(FileInfo configFile)
        {
            this.Configuration = this.Domain.Execute(configFile, self => this, host => this.Host);
        }

        /// <summary>
        /// リモート オブジェクトとの通信に使用するプロキシの生成に必要な情報をすべて格納しているオブジェクトを作成します。
        /// </summary>
        /// <returns>プロキシを生成するのに必要な情報。</returns>
        public ObjRef CreateObjRef()
        {
            return this.Domain.AppDomain.Invoke(() => this.CreateObjRef(this.GetType()));
        }
    }
}