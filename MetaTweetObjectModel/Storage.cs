// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetObjectModel
 *   Object model and Storage interface for MetaTweet and other systems
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@xspect.org>
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
using System.Linq;
using XSpect.MetaTweet.ObjectModel;

namespace XSpect.MetaTweet
{
    /// <summary>
    /// オブジェクトモデルの生成、格納、操作を提供する基本クラスです。
    /// </summary>
    /// <remarks>
    /// <p>MetaTweet のオブジェクトモデルは、ストレージオブジェクト、データセット、バックエンドの三層で
    /// 構成されます。</p>
    /// <p>バックエンドはオブジェクトモデルを効率的に外部記憶に格納し、データセットに対しデータの提供を行います。
    /// 通常、リレーショナルデータベースの使用が期待されています。</p>
    /// <p>データセットはバックエンドから取得したデータ構造を表形式で保持し、追加、修正、および削除を行い、
    /// バックエンドに対し変更点の更新を行います。</p>
    /// <p>ストレージオブジェクトはデータセット上のデータ行を参照し、表形式のデータ構造を通常のオブジェクト
    /// 構造として公開し、データの抽象的な追加、修正、および削除の機能を提供します。</p>
    /// <p>データセットストレージオブジェクトとデータセットはオブジェクトモデルにおいて厳密に定義されて
    /// います。バックエンドと他の層に関してはインターフェイスのみ厳密に定義されています。</p>
    /// <p>ストレージは、バックエンドとの接続、切断、およびデータセット間の入出力のインターフェイス、
    /// データセット内のデータを検索し、また表形式のデータからストレージオブジェクトを生成する機能を
    /// 提供します。</p>
    /// </remarks>
    public abstract class Storage
        : MarshalByRefObject
    {
        private StorageDataSet _underlyingDataSet;

        /// <summary>
        /// バックエンドから取得し、またはストレージオブジェクトにより追加されたデータ行を格納する
        /// データセットを取得または設定します。このプロパティは一度に限り値を設定できます。
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// 値が既に設定されています。
        /// </exception>
        public StorageDataSet UnderlyingDataSet
        {
            get
            {
                return this._underlyingDataSet ?? (this._underlyingDataSet = new StorageDataSet());
            }
            set
            {
                // Suppress re-setting.
                if (this._underlyingDataSet != null)
                {
                    // TODO: Exception string resource
                    throw new InvalidOperationException();
                }
                this._underlyingDataSet = value;
            }
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

        public void Initialize(IDictionary<String, String> args)
        {
            if (args.ContainsKey("connection"))
            {
                this.Initialize(args["connection"]);
            }
        }

        /// <summary>
        /// 派生クラスで実装された場合、バックエンドのデータソースとの接続を初期化します。
        /// </summary>
        /// <param name="connectionString">接続に使用する文字列。</param>
        public abstract void Initialize(String connectionString);

        /// <summary>
        /// 派生クラスで実装された場合、バックエンドのデータソースに接続します。
        /// </summary>
        public abstract void Connect();

        /// <summary>
        /// 派生クラスで実装された場合、バックエンドのデータソースから切断します。
        /// </summary>
        public abstract void Disconnect();

        public void Dispose()
        {
            this.Disconnect();
        }

        #region Accounts
        /// <summary>
        /// バックエンドのデータソースに対しクエリを発行し、アカウントのデータ行を格納します。
        /// </summary>
        /// <param name="query">発行する SQL クエリのフォーマット文字列。</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ <see cref="Object"/> 配列。</param>
        /// <returns>追加されたデータ行数。</returns>
        public abstract Int32 FillAccountsBy(String query, params Object[] args);

        /// <summary>
        /// データセット内で該当するデータ表の全てのデータ行を用いてアカウントを生成します。
        /// </summary>
        /// <returns>
        /// データセット内で該当するデータ表の全てのデータ行を用いて生成されたアカウントの集合。
        /// </returns>
        public IEnumerable<Account> GetAccounts()
        {
            return this.GetAccounts(row => true);
        }

        /// <summary>
        /// 指定された条件に合致するデータ行を用いてアカウントを生成します。
        /// </summary>
        /// <param name="predicate">各データ行が条件に当てはまるかどうかをテストする関数。</param>
        /// <returns>条件に合致したデータ行を用いて生成されたアカウントの集合。</returns>
        public IEnumerable<Account> GetAccounts(Func<StorageDataSet.AccountsRow, Boolean> predicate)
        {
            return this.GetAccounts(this.UnderlyingDataSet.Accounts.Where(predicate));
        }

        /// <summary>
        /// データ行の集合を用いてアカウントを生成します。
        /// </summary>
        /// <param name="rows">生成に用いるデータ行の集合。</param>
        /// <returns>生成されたアカウントの集合。</returns>
        public IEnumerable<Account> GetAccounts(IEnumerable<StorageDataSet.AccountsRow> rows)
        {
            return rows.Select(row => this.GetAccount(row));
        }

        /// <summary>
        /// データ行からアカウントを生成します。
        /// </summary>
        /// <param name="row">生成に用いるデータ行。</param>
        /// <returns>生成されたアカウント。</returns>
        public Account GetAccount(StorageDataSet.AccountsRow row)
        {
            return new Account(row)
            {
                Storage = this,
            };
        }

        /// <summary>
        /// 値を指定して、このストレージを使用するアカウントを初期化します。
        /// </summary>
        /// <param name="accountId">アカウントを一意に識別するグローバル一意識別子 (GUID) 値。</param>
        /// <param name="realm">アカウントに関連付けられるサービスを表す文字列。</param>
        /// <returns>新しいアカウント。</returns>
        public Account NewAccount(Guid accountId, String realm)
        {
            Account account = new Account()
            {
                Storage = this,
                AccountId = accountId,
                Realm  = realm,
            };
            account.Store();
            return account;
        }
        #endregion

        #region Activities
        /// <summary>
        /// バックエンドのデータソースに対しクエリを発行し、アクティビティのデータ行を格納します。
        /// </summary>
        /// <param name="query">発行する SQL クエリのフォーマット文字列。</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ <see cref="Object"/> 配列。</param>
        /// <returns>追加されたデータ行数。</returns>
        public abstract Int32 FillActivitiesBy(String query, params Object[] args);

        /// <summary>
        /// データセット内で該当するデータ表の全てのデータ行を用いてアクティビティを生成します。
        /// </summary>
        /// <returns>データセット内で該当するデータ表の全てのデータ行を用いて生成されたアクティビティの集合。</returns>
        public IEnumerable<Activity> GetActivities()
        {
            return this.GetActivities(row => true);
        }

        /// <summary>
        /// 指定された条件に合致するデータ行を用いてアクティビティを生成します。
        /// </summary>
        /// <param name="predicate">各データ行が条件に当てはまるかどうかをテストする関数。</param>
        /// <returns>条件に合致したデータ行を用いて生成されたアクティビティの集合。</returns>
        public IEnumerable<Activity> GetActivities(Func<StorageDataSet.ActivitiesRow, Boolean> predicate)
        {
            return this.GetActivities(this.UnderlyingDataSet.Activities.Where(predicate));
        }

        /// <summary>
        /// データ行の集合を用いてアクティビティを生成します。
        /// </summary>
        /// <param name="rows">生成に用いるデータ行の集合。</param>
        /// <returns>生成されたアクティビティの集合。</returns>
        public IEnumerable<Activity> GetActivities(IEnumerable<StorageDataSet.ActivitiesRow> rows)
        {
            return rows.Select(row => this.GetActivity(row));
        }

        /// <summary>
        /// データ行からアクティビティを生成します。
        /// </summary>
        /// <param name="row">生成に用いるデータ行。</param>
        /// <returns>生成されたアクティビティ。</returns>
        public Activity GetActivity(StorageDataSet.ActivitiesRow row)
        {
            return new Activity(row)
            {
                Storage = this,
            };
        }

        /// <summary>
        /// 値を指定して、このストレージを使用するアクティビティを初期化します。
        /// </summary>
        /// <param name="account">アクティビティの主体となるアカウント。</param>
        /// <param name="timestamp">アクティビティの行われた日時。</param>
        /// <param name="category">アクティビティの種別を表す文字列。</param>
        /// <returns>新しいアクティビティ。</returns>
        public Activity NewActivity(
            Account account,
            DateTime timestamp,
            String category
        )
        {
            Activity activity = new Activity()
            {
                Storage = this,
                Account = account,
                Timestamp = timestamp,
                Category = category,
            };
            activity.Store();
            return activity;
        }
        #endregion

        #region FavorMap
        /// <summary>
        /// バックエンドのデータソースに対しクエリを発行し、お気に入りの関係のデータ行を格納します。
        /// </summary>
        /// <param name="query">発行する SQL クエリのフォーマット文字列。</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ <see cref="Object"/> 配列。</param>
        /// <returns>追加されたデータ行数。</returns>
        public abstract Int32 FillFavorMapBy(String query, params Object[] args);

        /// <summary>
        /// データセット内で該当するデータ表の全てのデータ行を用いてお気に入りの関係を生成します。
        /// </summary>
        /// <returns>データセット内で該当するデータ表の全てのデータ行を用いて生成されたお気に入りの関係の集合。</returns>
        public IEnumerable<FavorElement> GetFavorElements()
        {
            return this.GetFavorElements(row => true);
        }

        /// <summary>
        /// 指定された条件に合致するデータ行を用いてお気に入りの関係を生成します。
        /// </summary>
        /// <param name="predicate">各データ行が条件に当てはまるかどうかをテストする関数。</param>
        /// <returns>条件に合致したデータ行を用いて生成されたお気に入りの関係の集合。</returns>
        public IEnumerable<FavorElement> GetFavorElements(Func<StorageDataSet.FavorMapRow, Boolean> predicate)
        {
            return this.GetFavorElements(this.UnderlyingDataSet.FavorMap.Where(predicate));
        }

        /// <summary>
        /// データ行の集合を用いてお気に入りの関係を生成します。
        /// </summary>
        /// <param name="rows">生成に用いるデータ行の集合。</param>
        /// <returns>生成されたお気に入りの関係の集合。</returns>
        public IEnumerable<FavorElement> GetFavorElements(IEnumerable<StorageDataSet.FavorMapRow> rows)
        {
            return rows.Select(row => this.GetFavorElement(row));
        }

        /// <summary>
        /// データ行からお気に入りの関係を生成します。
        /// </summary>
        /// <param name="row">生成に用いるデータ行。</param>
        /// <returns>生成されたお気に入りの関係。</returns>
        public FavorElement GetFavorElement(StorageDataSet.FavorMapRow row)
        {
            return new FavorElement(row)
            {
                Storage = this,
            };
        }

        /// <summary>
        /// 値を指定して、このストレージを使用するお気に入りの関係を初期化します。
        /// </summary>
        /// <param name="account">お気に入りとしてマークする主体となるアカウント。</param>
        /// <param name="favoringActivity">お気に入りとしてマークするアクティビティ。</param>
        /// <returns>新しいお気に入りの関係。</returns>
        public FavorElement NewFavorElement(
            Account account,
            Activity favoringActivity
        )
        {
            FavorElement element = new FavorElement()
            {
                Storage = this,
                Account = account,
                FavoringActivity = favoringActivity,
            };
            element.Store();
            return element;
        }
        #endregion

        #region FollowMap
        /// <summary>
        /// バックエンドのデータソースに対しクエリを発行し、フォローの関係のデータ行を格納します。
        /// </summary>
        /// <param name="query">発行する SQL クエリのフォーマット文字列。</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ <see cref="Object"/> 配列。</param>
        /// <returns>追加されたデータ行数。</returns>
        public abstract Int32 FillFollowMapBy(String query, params Object[] args);

        /// <summary>
        /// データセット内で該当するデータ表の全てのデータ行を用いてフォローの関係を生成します。
        /// </summary>
        /// <returns>データセット内で該当するデータ表の全てのデータ行を用いて生成されたフォローの関係の集合。</returns>
        public IEnumerable<FollowElement> GetFollowElements()
        {
            return this.GetFollowElements(row => true);
        }

        /// <summary>
        /// 指定された条件に合致するデータ行を用いてフォローの関係を生成します。
        /// </summary>
        /// <param name="predicate">各データ行が条件に当てはまるかどうかをテストする関数。</param>
        /// <returns>条件に合致したデータ行を用いて生成されたフォローの関係の集合。</returns>
        public IEnumerable<FollowElement> GetFollowElements(Func<StorageDataSet.FollowMapRow, Boolean> predicate)
        {
            return this.GetFollowElements(this.UnderlyingDataSet.FollowMap.Where(predicate));
        }

        /// <summary>
        /// データ行の集合を用いてフォローの関係を生成します。
        /// </summary>
        /// <param name="rows">生成に用いるデータ行の集合。</param>
        /// <returns>生成されたフォローの関係の集合。</returns>
        public IEnumerable<FollowElement> GetFollowElements(IEnumerable<StorageDataSet.FollowMapRow> rows)
        {
            return rows.Select(row => this.GetFollowElement(row));
        }

        /// <summary>
        /// データ行からフォローの関係を生成します。
        /// </summary>
        /// <param name="row">生成に用いるデータ行。</param>
        /// <returns>生成されたフォローの関係。</returns>
        public FollowElement GetFollowElement(StorageDataSet.FollowMapRow row)
        {
            return new FollowElement(row)
            {
                Storage = this,
            };
        }

        /// <summary>
        /// 値を指定して、このストレージを使用するフォローの関係を初期化します。
        /// </summary>
        /// <param name="accoumt">フォローする主体となるアカウント。</param>
        /// <param name="followingAccount">フォローするアカウント。</param>
        /// <returns>新しいフォローの関係。</returns>
        public FollowElement NewFollowElement(
            Account account,
            Account followingAccount
        )
        {
            FollowElement element = new FollowElement()
            {
                Storage = this,
                Account = account,
                FollowingAccount = followingAccount,
            };
            element.Store();
            return element;
        }
        #endregion

        #region Posts
        /// <summary>
        /// バックエンドのデータソースに対しクエリを発行し、ポストのデータ行を格納します。
        /// </summary>
        /// <param name="query">発行する SQL クエリのフォーマット文字列。</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ <see cref="Object"/> 配列。</param>
        /// <returns>追加されたデータ行数。</returns>
        public abstract Int32 FillPostsBy(String query, params Object[] args);

        /// <summary>
        /// データセット内で該当するデータ表の全てのデータ行を用いてポストを生成します。
        /// </summary>
        /// <returns>データセット内で該当するデータ表の全てのデータ行を用いて生成されたポストの集合。</returns>
        public IEnumerable<Post> GetPosts()
        {
            return this.GetPosts(row => true);
        }

        /// <summary>
        /// 指定された条件に合致するデータ行を用いてポストを生成します。
        /// </summary>
        /// <param name="predicate">各データ行が条件に当てはまるかどうかをテストする関数。</param>
        /// <returns>条件に合致したデータ行を用いて生成されたポストの集合。</returns>
        public IEnumerable<Post> GetPosts(Func<StorageDataSet.PostsRow, Boolean> predicate)
        {
            return this.GetPosts(this.UnderlyingDataSet.Posts.Where(predicate));
        }

        /// <summary>
        /// データ行の集合を用いてポストを生成します。
        /// </summary>
        /// <param name="rows">生成に用いるデータ行の集合。</param>
        /// <returns>生成されたポストの集合。</returns>
        public IEnumerable<Post> GetPosts(IEnumerable<StorageDataSet.PostsRow> rows)
        {
            return rows.Select(row => this.GetPost(row));
        }

        /// <summary>
        /// データ行からポストを生成します。
        /// </summary>
        /// <param name="row">生成に用いるデータ行。</param>
        /// <returns>生成されたポスト。</returns>
        public Post GetPost(StorageDataSet.PostsRow row)
        {
            return new Post(row)
            {
                Storage = this,
            };
        }

        /// <summary>
        /// 値を指定して、このストレージを使用するポストを初期化します。
        /// </summary>
        /// <param name="activity">ポストと一対一で対応するアクティビティ。</param>
        /// <returns>新しいポスト。</returns>
        public Post NewPost(
            Activity activity
        )
        {
            // TODO: Check the property setting
            Post post = new Post()
            {
                Storage = this,
                Activity = activity,
            };
            post.Store();
            return post;
        }
        #endregion

        #region ReplyMap
        /// <summary>
        /// バックエンドのデータソースに対しクエリを発行し、返信の関係のデータ行を格納します。
        /// </summary>
        /// <param name="query">発行する SQL クエリのフォーマット文字列。</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ <see cref="Object"/> 配列。</param>
        /// <returns>追加されたデータ行数。</returns>
        public abstract Int32 FillReplyMapBy(String query, params Object[] args);

        /// <summary>
        /// データセット内で該当するデータ表の全てのデータ行を用いて返信の関係を生成します。
        /// </summary>
        /// <returns>データセット内で該当するデータ表の全てのデータ行を用いて生成された返信の関係の集合。</returns>
        public IEnumerable<ReplyElement> GetReplyElements()
        {
            return this.GetReplyElements(row => true);
        }

        /// <summary>
        /// 指定された条件に合致するデータ行を用いて返信の関係を生成します。
        /// </summary>
        /// <param name="predicate">各データ行が条件に当てはまるかどうかをテストする関数。</param>
        /// <returns>条件に合致したデータ行を用いて生成された返信の関係の集合。</returns>
        public IEnumerable<ReplyElement> GetReplyElements(Func<StorageDataSet.ReplyMapRow, Boolean> predicate)
        {
            return this.GetReplyElements(this.UnderlyingDataSet.ReplyMap.Where(predicate));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        public IEnumerable<ReplyElement> GetReplyElements(IEnumerable<StorageDataSet.ReplyMapRow> rows)
        {
            return rows.Select(row => this.GetReplyElement(row));
        }

        /// <summary>
        /// データ行から返信の関係を生成します。
        /// </summary>
        /// <param name="row">生成に用いるデータ行。</param>
        /// <returns>生成された返信の関係。</returns>
        public ReplyElement GetReplyElement(StorageDataSet.ReplyMapRow row)
        {
            return new ReplyElement(row)
            {
                Storage = this,
            };
        }

        /// <summary>
        /// 値を指定して、このストレージを使用する返信の関係を初期化します。
        /// </summary>
        /// <param name="post">返信する主体となるポスト</param>
        /// <param name="inReplyToPost">返信元のポスト。</param>
        /// <returns>新しい返信の関係。</returns>
        public ReplyElement NewReplyElement(
            Post post,
            Post inReplyToPost
        )
        {
            ReplyElement element = new ReplyElement()
            {
                Storage = this,
                Post = post,
                InReplyToPost = inReplyToPost,
            };
            element.Store();
            return element;
        }
        #endregion

        #region TagMap
        /// <summary>
        /// バックエンドのデータソースに対しクエリを発行し、タグの関係のデータ行を格納します。
        /// </summary>
        /// <param name="query">発行する SQL クエリのフォーマット文字列。</param>
        /// <param name="args">0 個以上の書式設定対象オブジェクトを含んだ <see cref="Object"/> 配列。</param>
        /// <returns>追加されたデータ行数。</returns>
        public abstract Int32 FillTagMapBy(String query, params Object[] args);

        /// <summary>
        /// データセット内で該当するデータ表の全てのデータ行を用いてタグの関係を生成します。
        /// </summary>
        /// <returns>データセット内で該当するデータ表の全てのデータ行を用いて生成されたタグの関係の集合。</returns>
        public IEnumerable<TagElement> GetTagElements()
        {
            return this.GetTagElements(row => true);
        }

        /// <summary>
        /// 指定された条件に合致するデータ行を用いてタグの関係を生成します。
        /// </summary>
        /// <param name="predicate">各データ行が条件に当てはまるかどうかをテストする関数。</param>
        /// <returns>条件に合致したデータ行を用いて生成されたタグの関係の集合。</returns>
        public IEnumerable<TagElement> GetTagElements(Func<StorageDataSet.TagMapRow, Boolean> predicate)
        {
            return this.GetTagElements(this.UnderlyingDataSet.TagMap.Where(predicate));
        }

        /// <summary>
        /// データ行の集合を用いてタグの関係を生成します。
        /// </summary>
        /// <param name="rows">生成に用いるデータ行の集合。</param>
        /// <returns>生成されたタグの関係の集合。</returns>
        public IEnumerable<TagElement> GetTagElements(IEnumerable<StorageDataSet.TagMapRow> rows)
        {
            return rows.Select(row => this.GetTagElement(row));
        }

        /// <summary>
        /// データ行からタグの関係を生成します。
        /// </summary>
        /// <param name="row">生成に用いるデータ行。</param>
        /// <returns>生成されたタグの関係。</returns>
        public TagElement GetTagElement(StorageDataSet.TagMapRow row)
        {
            return new TagElement(row)
            {
                Storage = this,
            };
        }

        /// <summary>
        /// 値を指定して、このストレージを使用するタグの関係を初期化します。
        /// </summary>
        /// <param name="activity">タグを付与される主体となるアクティビティ。</param>
        /// <param name="tag">付与されるタグの文字列。</param>
        /// <returns>新しいタグの関係。</returns>
        public TagElement NewTagElement(
            Activity activity,
            String tag
        )
        {
            TagElement element = new TagElement()
            {
                Storage = this,
                Activity = activity,
                Tag = tag,
            };
            element.Store();
            return element;
        }
        #endregion

        /// <summary>
        /// 派生クラスで実装された場合、データセットにおける変更点および関連するその他の
        /// データ行をバックエンドのデータソースに対し追加、更新、削除します。
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// 指定されたストレージとデータセットのデータをマージします。
        /// </summary>
        /// <param name="destination"></param>
        public virtual void Merge(Storage destination)
        {
            this.UnderlyingDataSet.Merge(destination.UnderlyingDataSet);
        }
    }
}