// -*- mode: csharp; encoding: utf-8; -*-
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
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
using XSpect.MetaTweet.ObjectModel;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using XSpect.Configuration;
using log4net;
using System.Threading;
using Achiral;
using Achiral.Extension;
using System.Linq;

namespace XSpect.MetaTweet.Modules
{
    /// <summary>
    /// ストレージ モジュールの抽象基本クラスを提供します。
    /// </summary>
    /// <remarks>
    /// ストレージ モジュールとは、ストレージの機能を提供するモジュールです。即ち、<see cref="XSpect.MetaTweet.Storage"/> にモジュールに必要な機能を実装したクラスです。
    /// </remarks>
    public abstract class StorageModule
        : Storage,
          IModule
    {
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
        /// このモジュールに設定された名前を取得します。
        /// </summary>
        /// <value>このモジュールに設定された名前を取得します。</value>
        public String Name
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
        public ILog Log
        {
            get
            {
                return this.Host.Log;
            }
        }

        /// <summary>
        /// <see cref="StorageDataSet.AccountsDataTable"/> をロックするためのオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// <see cref="StorageDataSet.AccountsDataTable"/> をロックするためのオブジェクト。
        /// </value>
        internal Mutex AccountsLock
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="StorageDataSet.ActivitiesDataTable"/> をロックするためのオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// <see cref="StorageDataSet.ActivitiesDataTable"/> をロックするためのオブジェクト。
        /// </value>
        internal Mutex ActivitiesLock
        {
            get;
            private set;
        }
        
        /// <summary>
        /// <see cref="StorageDataSet.PostsDataTable"/> をロックするためのオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// <see cref="StorageDataSet.PostsDataTable"/> をロックするためのオブジェクト。
        /// </value>
        internal Mutex PostsLock
        {
            get;
            private set;
        }
        
        /// <summary>
        /// <see cref="StorageDataSet.FollowMapDataTable"/> をロックするためのオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// <see cref="StorageDataSet.FollowMapDataTable"/> をロックするためのオブジェクト。
        /// </value>
        internal Mutex FollowMapLock
        {
            get;
            private set;
        }
        
        /// <summary>
        /// <see cref="StorageDataSet.FavorMapDataTable"/> をロックするためのオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// <see cref="StorageDataSet.FavorMapDataTable"/> をロックするためのオブジェクト。
        /// </value>
        internal Mutex FavorMapLock
        {
            get;
            private set;
        }
        
        /// <summary>
        /// <see cref="StorageDataSet.TagMapDataTable"/> をロックするためのオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// <see cref="StorageDataSet.TagMapDataTable"/> をロックするためのオブジェクト。
        /// </value>
        internal Mutex TagMapLock
        {
            get;
            private set;
        }
        
        /// <summary>
        /// <see cref="StorageDataSet.ReplyMapDataTable"/> をロックするためのオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// <see cref="StorageDataSet.ReplyMapDataTable"/> をロックするためのオブジェクト。
        /// </value>
        internal Mutex ReplyMapLock
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="Initialize(XmlConfiguration)"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="Initialize(XmlConfiguration)"/> のフック リスト。
        /// </value>
        public Hook<IModule, XmlConfiguration> InitializeHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="LoadAccountsDataTable"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="LoadAccountsDataTable"/> のフック リスト。
        /// </value>
        public Hook<StorageModule, String> LoadAccountsDataTableHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="LoadActivitiesDataTable"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="LoadActivitiesDataTable"/> のフック リスト。
        /// </value>
        public Hook<StorageModule, String> LoadActivitiesDataTableHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="LoadFavorMapDataTable"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="LoadFavorMapDataTable"/> のフック リスト。
        /// </value>
        public Hook<StorageModule, String> LoadFavorMapDataTableHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="LoadFollowMapDataTable"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="LoadFollowMapDataTable"/> のフック リスト。
        /// </value>
        public Hook<StorageModule, String> LoadFollowMapDataTableHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="LoadPostsDataTable"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="LoadPostsDataTable"/> のフック リスト。
        /// </value>
        public Hook<StorageModule, String> LoadPostsDataTableHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="LoadReplyMapDataTable"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="LoadReplyMapDataTable"/> のフック リスト。
        /// </value>
        public Hook<StorageModule, String> LoadReplyMapDataTableHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="LoadTagMapDataTable"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="LoadTagMapDataTable"/> のフック リスト。
        /// </value>
        public Hook<StorageModule, String> LoadTagMapDataTableHook
        {
            get;
            set;
        }

        /// <summary>
        /// <see cref="GetAccount"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="GetAccount"/> のフック リスト。
        /// </value>
        public Hook<StorageModule, StorageDataSet.AccountsRow> GetAccountHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="GetActivity"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="GetActivity"/> のフック リスト。
        /// </value>
        public Hook<StorageModule, StorageDataSet.ActivitiesRow> GetActivityHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="GetFavorElement"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="GetFavorElement"/> のフック リスト。
        /// </value>
        public Hook<StorageModule, StorageDataSet.FavorMapRow> GetFavorElementHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="GetFollowElement"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="GetFollowElement"/> のフック リスト。
        /// </value>
        public Hook<StorageModule, StorageDataSet.FollowMapRow> GetFollowElementHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="GetPost"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="GetPost"/> のフック リスト。
        /// </value>
        public Hook<StorageModule, StorageDataSet.PostsRow> GetPostHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="GetReplyElement"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="GetReplyElement"/> のフック リスト。
        /// </value>
        public Hook<StorageModule, StorageDataSet.ReplyMapRow> GetReplyElementHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="GetTagElement"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="GetTagElement"/> のフック リスト。
        /// </value>
        public Hook<StorageModule, StorageDataSet.TagMapRow> GetTagElementHook
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
        public Hook<StorageModule, Guid, String> NewAccountHook
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
        public Hook<StorageModule, Account, DateTime, String, Int32> NewActivityHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="NewFavorElement"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="NewFavorElement"/> のフック リスト。
        /// </value>
        public Hook<StorageModule, Account, Activity> NewFavorElementHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="NewFollowElement"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="NewFollowElement"/> のフック リスト。
        /// </value>
        public Hook<StorageModule, Account, Account> NewFollowElementHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="NewPost"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="NewPost"/> のフック リスト。
        /// </value>
        public Hook<StorageModule, Activity> NewPostHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="NewReplyElement"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="NewReplyElement"/> のフック リスト。
        /// </value>
        public Hook<StorageModule, Post, Post> NewReplyElementHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="NewTagElement"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="NewTagElement"/> のフック リスト。
        /// </value>
        public Hook<StorageModule, Activity, String> NewTagElementHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="StorageModule"/> の新しいインスタンスを初期化します。
        /// </summary>
        public StorageModule()
        {
            this.AccountsLock = new Mutex();
            this.ActivitiesLock = new Mutex();
            this.PostsLock = new Mutex();
            this.FollowMapLock = new Mutex();
            this.FavorMapLock = new Mutex();
            this.TagMapLock = new Mutex();
            this.ReplyMapLock = new Mutex();
            this.InitializeHook = new Hook<IModule, XmlConfiguration>();
            this.LoadAccountsDataTableHook = new Hook<StorageModule, String>();
            this.LoadActivitiesDataTableHook = new Hook<StorageModule, String>();
            this.LoadFavorMapDataTableHook = new Hook<StorageModule, String>();
            this.LoadFollowMapDataTableHook = new Hook<StorageModule, String>();
            this.LoadPostsDataTableHook = new Hook<StorageModule, String>();
            this.LoadReplyMapDataTableHook = new Hook<StorageModule, String>();
            this.LoadTagMapDataTableHook = new Hook<StorageModule, String>();
            this.GetAccountHook = new Hook<StorageModule, StorageDataSet.AccountsRow>();
            this.GetActivityHook = new Hook<StorageModule, StorageDataSet.ActivitiesRow>();
            this.GetFavorElementHook = new Hook<StorageModule, StorageDataSet.FavorMapRow>();
            this.GetFollowElementHook = new Hook<StorageModule, StorageDataSet.FollowMapRow>();
            this.GetPostHook = new Hook<StorageModule, StorageDataSet.PostsRow>();
            this.GetReplyElementHook = new Hook<StorageModule, StorageDataSet.ReplyMapRow>();
            this.GetTagElementHook = new Hook<StorageModule, StorageDataSet.TagMapRow>();
            this.NewAccountHook = new Hook<StorageModule, Guid, String>();
            this.NewActivityHook = new Hook<StorageModule, Account, DateTime, String, Int32>();
            this.NewFavorElementHook = new Hook<StorageModule, Account, Activity>();
            this.NewFollowElementHook = new Hook<StorageModule, Account, Account>();
            this.NewPostHook = new Hook<StorageModule, Activity>();
            this.NewReplyElementHook = new Hook<StorageModule, Post, Post>();
            this.NewTagElementHook = new Hook<StorageModule, Activity, String>();
        }

        /// <summary>
        /// <see cref="StorageModule"/> によって使用されているアンマネージ リソースを解放し、オプションでマネージ リソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 <c>true</c>、破棄されない場合は <c>false</c>。</param>
        protected override void Dispose(Boolean disposing)
        {
            this.AccountsLock.Close();
            this.ActivitiesLock.Close();
            this.PostsLock.Close();
            this.FollowMapLock.Close();
            this.FavorMapLock.Close();
            this.TagMapLock.Close();
            this.ReplyMapLock.Close();
            this.Cache.Save();
            base.Dispose(disposing);
        }

        /// <summary>
        /// このモジュールをサーバ オブジェクトに登録します。
        /// </summary>
        /// <param name="host">登録されるサーバ オブジェクト。</param>
        /// <param name="name">モジュールに設定する名前。</param>
        public virtual void Register(ServerCore host, String name)
        {
            this.Host = host;
            this.Name = name;
        }

        /// <summary>
        /// このモジュールに設定を適用し、初期化を行います。
        /// </summary>
        /// <param name="configuration">適用する設定。</param>
        public void Initialize(XmlConfiguration configuration)
        {
            this.Configuration = configuration;
            if (configuration.ContainsKey("connection"))
            {
                this.Initialize(configuration.GetValue<String>("connection"));
            }
            FileInfo file = new FileInfo(configuration.GetValueOrDefault(
                "cachePath",
                this.Name + ".cache"
            ));
            try
            {
                this.Cache = StorageCache.Load(file, this);
            }
            catch (Exception)
            {
                if (file.Exists)
                {
                    file.Delete();
                }
                this.Cache = new StorageCache(this);
                // Create the cache file and set CacheFile.
                this.Cache.Save(file);
            }

            this.InitializeHook.Execute((self, configuration_) =>
            {
                self.Initialize();
            }, this, configuration);
        }

        /// <summary>
        /// このモジュールを初期化します。
        /// </summary>
        /// <remarks>
        /// このメソッドはモジュールの寿命中、複数回呼び出される可能性があります。
        /// </remarks>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// 選択を行う文に後続するクエリ節を指定してバックエンドのデータソースからアカウントのデータ表を読み出し、データセット上のデータ表とマージします。
        /// </summary>
        /// <param name="clauses">読み出しに使用する、データ表内に存在する全てのデータを取得する文に続くクエリ節文字列。</param>
        /// <returns>データソースから読み出したデータ表。</returns>
        public override StorageDataSet.AccountsDataTable LoadAccountsDataTable(String clauses)
        {
            return this.LoadAccountsDataTableHook.Execute(
                (self, clauses_) => this._LoadAccountsDataTable(clauses_),
                this, clauses
            );
        }

        /// <summary>
        /// 選択を行う文に後続するクエリ節を指定してバックエンドのデータソースからアカウントのデータ表を読み出し、データセット上のデータ表とマージします。
        /// </summary>
        /// <param name="clauses">読み出しに使用する、データ表内に存在する全てのデータを取得する文に続くクエリ節文字列。</param>
        /// <returns>データソースから読み出したデータ表。</returns>
        public override StorageDataSet.ActivitiesDataTable LoadActivitiesDataTable(String clauses)
        {
            return this.LoadActivitiesDataTableHook.Execute(
                (self, clauses_) => this._LoadActivitiesDataTable(clauses_),
                this, clauses
            );
        }

        /// <summary>
        /// 選択を行う文に後続するクエリ節を指定してバックエンドのデータソースからお気に入りの関係のデータ表を読み出し、データセット上のデータ表とマージします。
        /// </summary>
        /// <param name="clauses">読み出しに使用する、データ表内に存在する全てのデータを取得する文に続くクエリ節文字列。</param>
        /// <returns>データソースから読み出したデータ表。</returns>
        public override StorageDataSet.FavorMapDataTable LoadFavorMapDataTable(String clauses)
        {
            return this.LoadFavorMapDataTableHook.Execute(
                (self, clauses_) => this._LoadFavorMapDataTable(clauses_),
                this, clauses
            );
        }

        /// <summary>
        /// 選択を行う文に後続するクエリ節を指定してバックエンドのデータソースからフォローの関係のデータ表を読み出し、データセット上のデータ表とマージします。
        /// </summary>
        /// <param name="clauses">読み出しに使用する、データ表内に存在する全てのデータを取得する文に続くクエリ節文字列。</param>
        /// <returns>データソースから読み出したデータ表。</returns>
        public override StorageDataSet.FollowMapDataTable LoadFollowMapDataTable(String clauses)
        {
            return this.LoadFollowMapDataTableHook.Execute(
                (self, clauses_) => this._LoadFollowMapDataTable(clauses_),
                this, clauses
            );
        }

        /// <summary>
        /// 選択を行う文に後続するクエリ節を指定してバックエンドのデータソースからポストのデータ表を読み出し、データセット上のデータ表とマージします。
        /// </summary>
        /// <param name="clauses">読み出しに使用する、データ表内に存在する全てのデータを取得する文に続くクエリ節文字列。</param>
        /// <returns>データソースから読み出したデータ表。</returns>
        public override StorageDataSet.PostsDataTable LoadPostsDataTable(String clauses)
        {
            return this.LoadPostsDataTableHook.Execute(
                (self, clauses_) => this._LoadPostsDataTable(clauses_),
                this, clauses
            );
        }

        /// <summary>
        /// 選択を行う文に後続するクエリ節を指定してバックエンドのデータソースから返信の関係のデータ表を読み出し、データセット上のデータ表とマージします。
        /// </summary>
        /// <param name="clauses">読み出しに使用する、データ表内に存在する全てのデータを取得する文に続くクエリ節文字列。</param>
        /// <returns>データソースから読み出したデータ表。</returns>
        public override StorageDataSet.ReplyMapDataTable LoadReplyMapDataTable(String clauses)
        {
            return this.LoadReplyMapDataTableHook.Execute(
                (self, clauses_) => this._LoadReplyMapDataTable(clauses_),
                this, clauses
            );
        }

        /// <summary>
        /// 選択を行う文に後続するクエリ節を指定してバックエンドのデータソースからタグの関係のデータ表を読み出し、データセット上のデータ表とマージします。
        /// </summary>
        /// <param name="clauses">読み出しに使用する、データ表内に存在する全てのデータを取得する文に続くクエリ節文字列。</param>
        /// <returns>データソースから読み出したデータ表。</returns>
        public override StorageDataSet.TagMapDataTable LoadTagMapDataTable(String clauses)
        {
            return this.LoadTagMapDataTableHook.Execute(
                (self, clauses_) => this._LoadTagMapDataTable(clauses_),
                this, clauses
            );
        }

        /// <summary>
        /// データ行からアカウントを生成します。
        /// </summary>
        /// <param name="row">生成に用いるデータ行。</param>
        /// <returns>生成されたアカウント。</returns>
        public override Account GetAccount(StorageDataSet.AccountsRow row)
        {
            return this.GetAccountHook.Execute(
                (self, row_) => this._GetAccount(row_),
                this, row
            );
        }

        /// <summary>
        /// データ行からアクティビティを生成します。
        /// </summary>
        /// <param name="row">生成に用いるデータ行。</param>
        /// <returns>生成されたアクティビティ。</returns>
        public override Activity GetActivity(StorageDataSet.ActivitiesRow row)
        {
            return this.GetActivityHook.Execute(
                (self, row_) => this._GetActivity(row_),
                this, row
            );
        }

        /// <summary>
        /// データ行からお気に入りの関係を生成します。
        /// </summary>
        /// <param name="row">生成に用いるデータ行。</param>
        /// <returns>生成されたお気に入りの関係。</returns>
        public override FavorElement GetFavorElement(StorageDataSet.FavorMapRow row)
        {
            return this.GetFavorElementHook.Execute(
                (self, row_) => this._GetFavorElement(row_),
                this, row
            );
        }

        /// <summary>
        /// データ行からフォローの関係を生成します。
        /// </summary>
        /// <param name="row">生成に用いるデータ行。</param>
        /// <returns>生成されたフォローの関係。</returns>
        public override FollowElement GetFollowElement(StorageDataSet.FollowMapRow row)
        {
            return this.GetFollowElementHook.Execute(
                (self, row_) => this._GetFollowElement(row_),
                this, row
            );
        }

        /// <summary>
        /// データ行からポストを生成します。
        /// </summary>
        /// <param name="row">生成に用いるデータ行。</param>
        /// <returns>生成されたポスト。</returns>
        public override Post GetPost(StorageDataSet.PostsRow row)
        {
            return this.GetPostHook.Execute(
                (self, row_) => this._GetPost(row_),
                this, row
            );
        }

        /// <summary>
        /// データ行から返信の関係を生成します。
        /// </summary>
        /// <param name="row">生成に用いるデータ行。</param>
        /// <returns>生成された返信の関係。</returns>
        public override ReplyElement GetReplyElement(StorageDataSet.ReplyMapRow row)
        {
            return this.GetReplyElementHook.Execute(
                (self, row_) => this._GetReplyElement(row_),
                this, row
            );
        }

        /// <summary>
        /// データ行からタグの関係を生成します。
        /// </summary>
        /// <param name="row">生成に用いるデータ行。</param>
        /// <returns>生成されたタグの関係。</returns>
        public override TagElement GetTagElement(StorageDataSet.TagMapRow row)
        {
            return this.GetTagElementHook.Execute(
                (self, row_) => this._GetTagElement(row_),
                this, row
            );
        }

        /// <summary>
        /// 値を指定して、このストレージを使用するアカウントを初期化します。既にバックエンドのデータソースに対応するデータ行が存在する場合は、データセットにロードされ、そこから生成されたアカウントを返します。
        /// </summary>
        /// <param name="accountId">アカウントを一意に識別するグローバル一意識別子 (GUID) 値。</param>
        /// <param name="realm">アカウントに関連付けられるサービスを表す文字列。</param>
        /// <returns>
        /// 新しいアカウント。既にバックエンドのデータソースに存在する場合は、生成されたアカウント。
        /// </returns>
        public override Account NewAccount(
            Guid accountId,
            String realm
        )
        {
            return this.NewAccountHook.Execute(
                (self, accountId_, realm_) => this._NewAccount(accountId_, realm_),
                this, accountId, realm
            );
        }

        /// <summary>
        /// 値を指定して、このストレージを使用するアクティビティを初期化します。既にバックエンドのデータソースに対応するデータ行が存在する場合は、データセットにロードされ、そこから生成されたアクティビティを返します。
        /// </summary>
        /// <param name="account">アクティビティの主体となるアカウント。</param>
        /// <param name="timestamp">アクティビティの行われた日時。</param>
        /// <param name="category">アクティビティの種別を表す文字列。</param>
        /// <param name="subindex">アクティビティのサブインデックス。</param>
        /// <returns>
        /// 新しいアクティビティ。既にバックエンドのデータソースに存在する場合は、生成されたアクティビティ。
        /// </returns>
        public override Activity NewActivity(
            Account account,
            DateTime timestamp,
            String category,
            Int32 subindex
        )
        {
            return this.NewActivityHook.Execute(
                (self, account_, timestamp_, category_, subindex_)
                    => this._NewActivity(account_, timestamp_, category_, subindex_),
                this, account, timestamp, category, subindex
            );
        }

        /// <summary>
        /// 値を指定して、このストレージを使用するお気に入りの関係を初期化します。既にバックエンドのデータソースに対応するデータ行が存在する場合は、データセットにロードされ、そこから生成されたお気に入りの関係を返します。
        /// </summary>
        /// <param name="account">お気に入りとしてマークする主体となるアカウント。</param>
        /// <param name="favoringActivity">お気に入りとしてマークするアクティビティ。</param>
        /// <returns>
        /// 新しいお気に入りの関係。既にバックエンドのデータソースに存在する場合は、生成されたお気に入りの関係。
        /// </returns>
        public override FavorElement NewFavorElement(
            Account account,
            Activity favoringActivity
        )
        {
            return this.NewFavorElementHook.Execute(
                (self, account_, favoringActivity_) => this._NewFavorElement(account_, favoringActivity_),
                this, account, favoringActivity
            );
        }

        /// <summary>
        /// 値を指定して、このストレージを使用するフォローの関係を初期化します。既にバックエンドのデータソースに対応するデータ行が存在する場合は、データセットにロードされ、そこから生成されたフォローの関係を返します。
        /// </summary>
        /// <param name="account">フォローする主体となるアカウント。</param>
        /// <param name="followingAccount">フォローするアカウント。</param>
        /// <returns>
        /// 新しいフォローの関係。既にバックエンドのデータソースに存在する場合は、生成されたフォローの関係。
        /// </returns>
        public override FollowElement NewFollowElement(
            Account account,
            Account followingAccount
        )
        {
            return this.NewFollowElementHook.Execute(
                (self, account_, followingAccount_) => this._NewFollowElement(account_, followingAccount_),
                this, account, followingAccount
            );
        }

        /// <summary>
        /// 値を指定して、このストレージを使用するポストを初期化します。既にバックエンドのデータソースに対応するデータ行が存在する場合は、データセットにロードされ、そこから生成されたポストを返します。
        /// </summary>
        /// <param name="activity">ポストと一対一で対応するアクティビティ。</param>
        /// <returns>新しいポスト。既にバックエンドのデータソースに存在する場合は、生成されたポスト。</returns>
        public override Post NewPost(
            Activity activity
        )
        {
            return this.NewPostHook.Execute(
                (self, activity_) => this._NewPost(activity_),
                this, activity
            );
        }

        /// <summary>
        /// 値を指定して、このストレージを使用する返信の関係を初期化します。既にバックエンドのデータソースに対応するデータ行が存在する場合は、データセットにロードされ、そこから生成された返信の関係を返します。
        /// </summary>
        /// <param name="post">返信する主体となるポスト</param>
        /// <param name="inReplyToPost">返信元のポスト。</param>
        /// <returns>
        /// 新しい返信の関係。既にバックエンドのデータソースに存在する場合は、生成された返信の関係。
        /// </returns>
        public override ReplyElement NewReplyElement(
            Post post,
            Post inReplyToPost
        )
        {
            return this.NewReplyElementHook.Execute(
                (self, post_, inReplyToPost_) => this._NewReplyElement(post_, inReplyToPost_),
                this, post, inReplyToPost
            );
        }

        /// <summary>
        /// 値を指定して、このストレージを使用するタグの関係を初期化します。既にバックエンドのデータソースに対応するデータ行が存在する場合は、データセットにロードされ、そこから生成されたタグの関係を返します。
        /// </summary>
        /// <param name="activity">タグを付与される主体となるアクティビティ。</param>
        /// <param name="tag">付与されるタグの文字列。</param>
        /// <returns>
        /// 新しいタグの関係。既にバックエンドのデータソースに存在する場合は、生成されたタグの関係。
        /// </returns>
        public override TagElement NewTagElement(
            Activity activity,
            String tag
        )
        {
            return this.NewTagElementHook.Execute(
                (self, activity_, tag_) => this._NewTagElement(activity_, tag_),
                this, activity, tag
            );
        }

        /// <summary>
        /// 指定されたデータ表へのロックが解除されるまで待機します。
        /// </summary>
        /// <param name="waitingLocks">解除されるのを待機するロック。</param>
        public void Wait(StorageDataTypes waitingLocks)
        {
            this.CheckIfDisposed();
            if (waitingLocks == StorageDataTypes.None)
            {
                return;
            }
            WaitHandle.WaitAll(this.GetMutexes(waitingLocks).ToArray());
        }

        /// <summary>
        /// <see cref="Wait"/> で取得したロックを解放します。
        /// </summary>
        /// <param name="waitedLocks"><see cref="Wait"/> で取得したロック。</param>
        public void Release(StorageDataTypes waitedLocks)
        {
            this.CheckIfDisposed();
            if (waitedLocks == StorageDataTypes.None)
            {
                return;
            }
            this.GetMutexes(waitedLocks).ForEach(m => m.ReleaseMutex());
        }

        /// <summary>
        /// データ表へのロックが全て解放されている場合のみ <see cref="Storage.Update"/> を実行します。
        /// </summary>
        public void TryUpdate()
        {
            this.CheckIfDisposed();
            // Test or get whether all mutexes is free.
            if (WaitHandle.WaitAll(this.GetMutexes(StorageDataTypes.All).ToArray(), 0))
            {
                this.Update();
                this.Release(StorageDataTypes.All);
            }
        }

        private IEnumerable<Mutex> GetMutexes(StorageDataTypes locks)
        {
            LinkedList<Mutex> mutexes = new LinkedList<Mutex>();
            if ((locks & StorageDataTypes.Account) == StorageDataTypes.Account)
            {
                mutexes.AddLast(this.AccountsLock);
            }
            if ((locks & StorageDataTypes.Activity) == StorageDataTypes.Activity)
            {
                mutexes.AddLast(this.ActivitiesLock);
            }
            if ((locks & StorageDataTypes.Post) == StorageDataTypes.Post)
            {
                mutexes.AddLast(this.PostsLock);
            }
            if ((locks & StorageDataTypes.Follow) == StorageDataTypes.Follow)
            {
                mutexes.AddLast(this.FollowMapLock);
            }
            if ((locks & StorageDataTypes.Favor) == StorageDataTypes.Favor)
            {
                mutexes.AddLast(this.FavorMapLock);
            }
            if ((locks & StorageDataTypes.Tag) == StorageDataTypes.Tag)
            {
                mutexes.AddLast(this.TagMapLock);
            }
            if ((locks & StorageDataTypes.Reply) == StorageDataTypes.Reply)
            {
                mutexes.AddLast(this.ReplyMapLock);
            }
            return mutexes;
        }

        #region Private Helper Methods
        private StorageDataSet.AccountsDataTable _LoadAccountsDataTable(String clauses)
        {
            return base.LoadAccountsDataTable(clauses);
        }

        private StorageDataSet.ActivitiesDataTable _LoadActivitiesDataTable(String clauses)
        {
            return base.LoadActivitiesDataTable(clauses);
        }

        private StorageDataSet.FavorMapDataTable _LoadFavorMapDataTable(String clauses)
        {
            return base.LoadFavorMapDataTable(clauses);
        }

        private StorageDataSet.FollowMapDataTable _LoadFollowMapDataTable(String clauses)
        {
            return base.LoadFollowMapDataTable(clauses);
        }

        private StorageDataSet.PostsDataTable _LoadPostsDataTable(String clauses)
        {
            return base.LoadPostsDataTable(clauses);
        }

        private StorageDataSet.ReplyMapDataTable _LoadReplyMapDataTable(String clauses)
        {
            return base.LoadReplyMapDataTable(clauses);
        }

        private StorageDataSet.TagMapDataTable _LoadTagMapDataTable(String clauses)
        {
            return base.LoadTagMapDataTable(clauses);
        }

        private Account _GetAccount(StorageDataSet.AccountsRow row)
        {
            return base.GetAccount(row);
        }

        private Activity _GetActivity(StorageDataSet.ActivitiesRow row)
        {
            return base.GetActivity(row);
        }

        private FavorElement _GetFavorElement(StorageDataSet.FavorMapRow row)
        {
            return base.GetFavorElement(row);
        }

        private FollowElement _GetFollowElement(StorageDataSet.FollowMapRow row)
        {
            return base.GetFollowElement(row);
        }

        private Post _GetPost(StorageDataSet.PostsRow row)
        {
            return base.GetPost(row);
        }

        private ReplyElement _GetReplyElement(StorageDataSet.ReplyMapRow row)
        {
            return base.GetReplyElement(row);
        }

        private TagElement _GetTagElement(StorageDataSet.TagMapRow row)
        {
            return base.GetTagElement(row);
        }

        private Account _NewAccount(
            Guid accountId,
            String realm
        )
        {
            return base.NewAccount(accountId, realm);
        }

        private Activity _NewActivity(
            Account account,
            DateTime timestamp,
            String category,
            Int32 subindex
        )
        {
            return base.NewActivity(account, timestamp, category, subindex);
        }

        private FavorElement _NewFavorElement(
            Account account,
            Activity favoringActivity
        )
        {
            return base.NewFavorElement(account, favoringActivity);
        }

        private FollowElement _NewFollowElement(
            Account account,
            Account followingAccount
        )
        {
            return base.NewFollowElement(account, followingAccount);
        }

        private Post _NewPost(
            Activity activity
        )
        {
            return base.NewPost(activity);
        }

        private ReplyElement _NewReplyElement(
            Post post,
            Post inReplyToPost
        )
        {
            return base.NewReplyElement(post, inReplyToPost);
        }

        private TagElement _NewTagElement(
            Activity activity,
            String tag
        )
        {
            return base.NewTagElement(activity, tag);
        }
        #endregion
    }
}