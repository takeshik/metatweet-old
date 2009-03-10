// -*- mode: csharp; encoding: utf-8; -*-
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
using XSpect.MetaTweet.ObjectModel;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using XSpect.Configuration;
using log4net;

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
        /// <see cref="LoadAccountsDataTable"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="LoadAccountsDataTable"/> のフック リスト。
        /// </value>
        public Hook<StorageModule, Nullable<Guid>, String> LoadAccountsDataTableHook
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
        public Hook<StorageModule, Nullable<Guid>, Nullable<DateTime>, String, Nullable<Int32>, Object, Object> LoadActivitiesDataTableHook
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
        public Hook<StorageModule, Nullable<Guid>, Nullable<Guid>, Nullable<DateTime>, String, Nullable<Int32>> LoadFavorMapDataTableHook
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
        public Hook<StorageModule, Nullable<Guid>, Nullable<Guid>> LoadFollowMapDataTableHook
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
        public Hook<StorageModule, Nullable<Guid>, String, Object, Object> LoadPostsDataTableHook
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
        public Hook<StorageModule, Nullable<Guid>, String, Nullable<Guid>, String> LoadReplyMapDataTableHook
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
        public Hook<StorageModule, Nullable<Guid>, Nullable<DateTime>, String, Nullable<Int32>, String> LoadTagMapDataTableHook
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

        public StorageModule()
        {
            this.LoadAccountsDataTableHook = new Hook<StorageModule, Nullable<Guid>, String>();
            this.LoadActivitiesDataTableHook = new Hook<StorageModule, Nullable<Guid>, Nullable<DateTime>, String, Nullable<Int32>, Object, Object>();
            this.LoadFavorMapDataTableHook = new Hook<StorageModule, Nullable<Guid>, Nullable<Guid>, Nullable<DateTime>, String, Nullable<Int32>>();
            this.LoadFollowMapDataTableHook = new Hook<StorageModule, Nullable<Guid>, Nullable<Guid>>();
            this.LoadPostsDataTableHook = new Hook<StorageModule, Nullable<Guid>, String, Object, Object>();
            this.LoadReplyMapDataTableHook = new Hook<StorageModule, Nullable<Guid>, String, Nullable<Guid>, String>();
            this.LoadTagMapDataTableHook = new Hook<StorageModule, Nullable<Guid>, Nullable<DateTime>, String, Nullable<Int32>, String>();
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
            this.Initialize();
        }

        /// <summary>
        /// このモジュールを初期化します。
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// 列の値を指定してバックエンドのデータソースからアカウントのデータ表を読み出し、データセット上のデータ表とマージします。
        /// </summary>
        /// <param name="accountId">アカウントを一意に識別するグローバル一意識別子 (GUID) 値。指定しない場合は <c>null</c>。</param>
        /// <param name="realm">アカウントに関連付けられているサービスを表す文字列。指定しない場合は <c>null</c>。</param>
        /// <returns>データソースから読み出したデータ表。</returns>
        public override StorageDataSet.AccountsDataTable LoadAccountsDataTable(
            Nullable<Guid> accountId,
            String realm
        )
        {
            return this.LoadAccountsDataTableHook.Execute(
                (self, accountId_, realm_) => this._LoadAccountsDataTable(accountId_, realm_),
                this, accountId, realm
            );
        }

        /// <summary>
        /// 列の値を指定してバックエンドのデータソースからアカウントのデータ表を読み出し、データセット上のデータ表とマージします。
        /// </summary>
        /// <param name="accountId">アクティビティの主体であるアカウントを一意に識別するグローバル一意識別子 (GUID) 値。指定しない場合は <c>null</c>。</param>
        /// <param name="timestamp">アクティビティの行われた日時。指定しない場合は <c>null</c>。</param>
        /// <param name="category">アクティビティの種別を表す文字列。指定しない場合は <c>null</c>。</param>
        /// <param name="subindex">アクティビティのサブインデックス。指定しない場合は <c>null</c>。</param>
        /// <param name="value">アクティビティに関連付けられている文字列の値。値は <see cref="String"/> として扱われます。値が存在しない状態を指定するには <see cref="DBNull"/> 値を指定してください。指定しない場合は <c>null</c>。</param>
        /// <param name="data">アクティビティに関連付けられているバイト列の値。値は <see cref="Byte"/> 配列として扱われます。値が存在しない状態を指定するには <see cref="DBNull"/> 値を指定してください。指定しない場合は <c>null</c>。</param>
        /// <returns>データソースから読み出したデータ表。</returns>
        public override StorageDataSet.ActivitiesDataTable LoadActivitiesDataTable(
            Nullable<Guid> accountId,
            Nullable<DateTime> timestamp,
            String category,
            Nullable<Int32> subindex,
            Object value,
            Object data
        )
        {
            return this.LoadActivitiesDataTableHook.Execute(
                (self, accountId_, timestamp_, category_, subindex_, value_, data_)
                    => this._LoadActivitiesDataTable(accountId_, timestamp_, category_, subindex_, value_, data_),
                this, accountId, timestamp, category, subindex, value, data
            );
        }

        /// <summary>
        /// 主キーを指定してバックエンドのデータソースからお気に入りの関係のデータ表を読み出し、データセット上のデータ表とマージします。
        /// </summary>
        /// <param name="accountId">お気に入りとしてマークしている主体であるアカウントを一意に識別するグローバル一意識別子 (GUID) 値。指定しない場合は <c>null</c>。</param>
        /// <param name="favoringAccountId">お気に入りとしてマークしているアクティビティの主体であるアカウントを一意に識別するグローバル一意識別子 (GUID) 値。指定しない場合は <c>null</c>。</param>
        /// <param name="favoringTimestamp">お気に入りとしてマークしているアクティビティの行われた日時。指定しない場合は <c>null</c>。</param>
        /// <param name="favoringCategory">お気に入りとしてマークしているアクティビティの種別を表す文字列。指定しない場合は <c>null</c>。</param>
        /// <param name="favoringSubindex">お気に入りとしてマークしているアクティビティのサブインデックス。指定しない場合は <c>null</c>。</param>
        /// <returns>データソースから読み出したデータ表。</returns>
        public override StorageDataSet.FavorMapDataTable LoadFavorMapDataTable(
            Nullable<Guid> accountId,
            Nullable<Guid> favoringAccountId,
            Nullable<DateTime> favoringTimestamp,
            String favoringCategory,
            Nullable<Int32> favoringSubindex
        )
        {
            return this.LoadFavorMapDataTableHook.Execute(
                (self, accountId_, favoringAccountId_, favoringTimestamp_, favoringCategory_, favoringSubindex_)
                    => this._LoadFavorMapDataTable(accountId_, favoringAccountId_, favoringTimestamp_, favoringCategory_, favoringSubindex_),
                this, accountId, favoringAccountId, favoringTimestamp, favoringCategory, favoringSubindex
            );
        }

        /// <summary>
        /// 主キーを指定してバックエンドのデータソースからフォローの関係のデータ表を読み出し、データセット上のデータ表とマージします。
        /// </summary>
        /// <param name="accountId">フォローしている主体であるアカウントを一意に識別するグローバル一意識別子 (GUID) 値。指定しない場合は <c>null</c>。</param>
        /// <param name="followingAccountId">アカウントがフォローしているアカウントを一意に識別するグローバル一意識別子 (GUID) 値。指定しない場合は <c>null</c>。</param>
        /// <returns>データソースから読み出したデータ表。</returns>
        public override StorageDataSet.FollowMapDataTable LoadFollowMapDataTable(
            Nullable<Guid> accountId,
            Nullable<Guid> followingAccountId
        )
        {
            return this.LoadFollowMapDataTableHook.Execute(
                (self, accountId_, followingAccountId_)
                    => this._LoadFollowMapDataTable(accountId_, followingAccountId_),
                this, accountId, followingAccountId
            );
        }

        /// <summary>
        /// 列の値を指定してバックエンドのデータソースからポストのデータ表を読み出し、データセット上のデータ表とマージします。
        /// </summary>
        /// <param name="accountId">ポストを投稿した主体であるアカウントを一意に識別するグローバル一意識別子 (GUID) 値。指定しない場合は <c>null</c>。</param>
        /// <param name="postId">任意のサービス内においてポストを一意に識別する文字列。指定しない場合は <c>null</c>。</param>
        /// <param name="text">ポストの本文。値は <see cref="String"/> として扱われます。値が存在しない状態を指定するには <see cref="DBNull"/> 値を指定してください。指定しない場合は <c>null</c>。</param>
        /// <param name="source">ポストの投稿に使用されたクライアントを表す文字列。値は <see cref="String"/> として扱われます。値が存在しない状態を指定するには <see cref="DBNull"/> 値を指定してください。指定しない場合は <c>null</c>。</param>
        /// <returns>データソースから読み出したデータ表。</returns>
        public override StorageDataSet.PostsDataTable LoadPostsDataTable(
            Nullable<Guid> accountId,
            String postId,
            Object text,
            Object source
        )
        {
            return this.LoadPostsDataTableHook.Execute(
                (self, accountId_, postId_, text_, source_)
                    => this._LoadPostsDataTable(accountId_, postId_, text_, source_),
                this, accountId, postId, text, source
            );
        }

        /// <summary>
        /// 主キーを指定してバックエンドのデータソースから返信の関係のデータ表を読み出し、データセット上のデータ表とマージします。
        /// </summary>
        /// <param name="accountId">返信している主体であるポストを投稿した主体であるアカウントを一意に識別するグローバル一意識別子 (GUID) 値。指定しない場合は <c>null</c>。</param>
        /// <param name="postId">任意のサービス内において返信している主体であるポストを一意に識別する文字列。指定しない場合は <c>null</c>。</param>
        /// <param name="inReplyToAccountId">ポストの返信元のポストを投稿した主体であるアカウントを一意に識別するグローバル一意識別子 (GUID) 値。指定しない場合は <c>null</c>。</param>
        /// <param name="inReplyToPostId">任意のサービス内においてポストを一意に識別する文字列。ポストの返信元の指定しない場合は <c>null</c>。</param>
        /// <returns>データソースから読み出したデータ表。</returns>
        public override StorageDataSet.ReplyMapDataTable LoadReplyMapDataTable(
            Nullable<Guid> accountId,
            String postId,
            Nullable<Guid> inReplyToAccountId,
            String inReplyToPostId
        )
        {
            return this.LoadReplyMapDataTableHook.Execute(
                (self, accountId_, postId_, inReplyToAccountId_, inReplyToPostId_)
                    => this._LoadReplyMapDataTable(accountId_, postId_, inReplyToAccountId_, inReplyToPostId_),
                this, accountId, postId, inReplyToAccountId, inReplyToPostId
            );

        }

        /// <summary>
        /// 主キーを指定してバックエンドのデータソースからタグの関係のデータ表を読み出し、データセット上のデータ表とマージします。
        /// </summary>
        /// <param name="accountId">タグを付与されている主体であるアクティビティの主体であるアカウントを一意に識別するグローバル一意識別子 (GUID) 値。指定しない場合は <c>null</c>。</param>
        /// <param name="timestamp">タグを付与されている主体であるアクティビティの行われた日時。指定しない場合は <c>null</c>。</param>
        /// <param name="category">タグを付与されている主体であるアクティビティの種別を表す文字列。指定しない場合は <c>null</c>。</param>
        /// <param name="subindex">タグを付与されている主体であるアクティビティのサブインデックス。指定しない場合は <c>null</c>。</param>
        /// <param name="tag">タグの文字列。指定しない場合は <c>null</c>。</param>
        /// <returns>データソースから読み出したデータ表。</returns>
        public override StorageDataSet.TagMapDataTable LoadTagMapDataTable(
            Nullable<Guid> accountId,
            Nullable<DateTime> timestamp,
            String category,
            Nullable<Int32> subindex,
            String tag
        )
        {
            return this.LoadTagMapDataTableHook.Execute(
                (self, accountId_, timestamp_, category_, subindex_, tag_)
                    => this._LoadTagMapDataTable(accountId_, timestamp_, category_, subindex_, tag_),
                this, accountId, timestamp, category, subindex, tag
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

        #region Private Helper Methods
        private StorageDataSet.AccountsDataTable _LoadAccountsDataTable(
            Nullable<Guid> accountId,
            String realm
        )
        {
            return base.LoadAccountsDataTable(accountId, realm);
        }

        private StorageDataSet.ActivitiesDataTable _LoadActivitiesDataTable(
            Nullable<Guid> accountId,
            Nullable<DateTime> timestamp,
            String category,
            Nullable<Int32> subindex,
            Object value,
            Object data
        )
        {
            return base.LoadActivitiesDataTable(accountId, timestamp, category, subindex, value, data);
        }

        private StorageDataSet.FavorMapDataTable _LoadFavorMapDataTable(
            Nullable<Guid> accountId,
            Nullable<Guid> favoringAccountId,
            Nullable<DateTime> favoringTimestamp,
            String favoringCategory,
            Nullable<Int32> favoringSubindex
        )
        {
            return base.LoadFavorMapDataTable(accountId, favoringAccountId, favoringTimestamp, favoringCategory, favoringSubindex);
        }

        private StorageDataSet.FollowMapDataTable _LoadFollowMapDataTable(
            Nullable<Guid> accountId,
            Nullable<Guid> followingAccountId
        )
        {
            return base.LoadFollowMapDataTable(accountId, followingAccountId);
        }

        private StorageDataSet.PostsDataTable _LoadPostsDataTable(
            Nullable<Guid> accountId,
            String postId,
            Object text,
            Object source
        )
        {
            return base.LoadPostsDataTable(accountId, postId, text, source);
        }

        private StorageDataSet.ReplyMapDataTable _LoadReplyMapDataTable(
            Nullable<Guid> accountId,
            String postId,
            Nullable<Guid> inReplyToAccountId,
            String inReplyToPostId
        )
        {
            return base.LoadReplyMapDataTable(accountId, postId, inReplyToAccountId, inReplyToPostId);
        }

        private StorageDataSet.TagMapDataTable _LoadTagMapDataTable(
            Nullable<Guid> accountId,
            Nullable<DateTime> timestamp,
            String category,
            Nullable<Int32> subindex,
            String tag
        )
        {
            return base.LoadTagMapDataTable(accountId, timestamp, category, subindex, tag);
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