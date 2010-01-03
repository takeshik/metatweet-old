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
    public abstract class StorageModule
        : ObjectContextStorage,
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
        /// <see cref="StorageEntities.AccountSet"/> をロックするためのオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// <see cref="StorageEntities.AccountSet"/> をロックするためのオブジェクト。
        /// </value>
        internal Mutex AccountsLock
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="StorageEntities.ActivitySet"/> をロックするためのオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// <see cref="StorageEntities.ActivitySet"/> をロックするためのオブジェクト。
        /// </value>
        internal Mutex ActivitiesLock
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="StorageEntities.AnnotationSet"/> をロックするためのオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// <see cref="StorageEntities.AnnotationSet"/> をロックするためのオブジェクト。
        /// </value>
        internal Mutex AnnotationsLock
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="StorageEntities.RelationSet"/> をロックするためのオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// <see cref="StorageEntities.RelationSet"/> をロックするためのオブジェクト。
        /// </value>
        internal Mutex RelationsLock
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="StorageEntities.MarkSet"/> をロックするためのオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// <see cref="StorageEntities.MarkSet"/> をロックするためのオブジェクト。
        /// </value>
        internal Mutex MarksLock
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="StorageEntities.ReferenceSet"/> をロックするためのオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// <see cref="StorageEntities.ReferenceSet"/> をロックするためのオブジェクト。
        /// </value>
        internal Mutex ReferencesLock
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="StorageEntities.TagSet"/> をロックするためのオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// <see cref="StorageEntities.TagSet"/> をロックするためのオブジェクト。
        /// </value>
        internal Mutex TagsLock
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
        public ActionHook<IModule> InitializeHook
        {
            get;
            private set;
        }

        public FuncHook<StorageModule, Nullable<Guid>, String, IEnumerable<Account>> GetAccountsHook
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
        public FuncHook<StorageModule, Guid, String, Account> NewAccountHook
        {
            get;
            private set;
        }

        public FuncHook<StorageModule, Nullable<Guid>, Nullable<DateTime>, String, String, String, Object, Object, IEnumerable<Activity>> GetActivitiesHook
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
        public FuncHook<StorageModule, Account, DateTime, String, String, String, String, Byte[], Activity> NewActivityHook
        {
            get;
            private set;
        }

        public FuncHook<StorageModule, Nullable<Guid>, String, IEnumerable<Annotation>> GetAnnotationsHook
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
        public FuncHook<StorageModule, Account, String, Annotation> NewAnnotationHook
        {
            get;
            private set;
        }

        public FuncHook<StorageModule, Nullable<Guid>, String, Nullable<Guid>, IEnumerable<Relation>> GetRelationsHook
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
        public FuncHook<StorageModule, Account, String, Account, Relation> NewRelationHook
        {
            get;
            private set;
        }

        public FuncHook<StorageModule, Nullable<Guid>, String, Nullable<Guid>, Nullable<DateTime>, String, String, IEnumerable<Mark>> GetMarksHook
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
        public FuncHook<StorageModule, Account, String, Activity, Mark> NewMarkHook
        {
            get;
            private set;
        }

        public FuncHook<StorageModule, Nullable<Guid>, Nullable<DateTime>, String, String, String, Nullable<Guid>, Nullable<DateTime>, String, String, IEnumerable<Reference>> GetReferencesHook
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
        public FuncHook<StorageModule, Activity, String, Activity, Reference> NewReferenceHook
        {
            get;
            private set;
        }

        public FuncHook<StorageModule, Nullable<Guid>, Nullable<DateTime>, String, String, String, IEnumerable<Tag>> GetTagsHook
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
        public FuncHook<StorageModule, Activity, String, Tag> NewTagHook
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="StorageModule"/> の新しいインスタンスを初期化します。
        /// </summary>
        protected StorageModule()
        {
            this.AccountsLock = new Mutex();
            this.ActivitiesLock = new Mutex();
            this.AnnotationsLock = new Mutex();
            this.RelationsLock = new Mutex();
            this.MarksLock = new Mutex();
            this.ReferencesLock = new Mutex();
            this.TagsLock = new Mutex();
            this.InitializeHook = new ActionHook<IModule>(this._Initialize);
            this.GetAccountsHook = new FuncHook<StorageModule, Nullable<Guid>, String, IEnumerable<Account>>(this._GetAccounts);
            this.GetActivitiesHook = new FuncHook<StorageModule, Nullable<Guid>, Nullable<DateTime>, String, String, String, Object, Object, IEnumerable<Activity>>(this._GetActivities);
            this.GetAnnotationsHook = new FuncHook<StorageModule, Nullable<Guid>, String, IEnumerable<Annotation>>(this._GetAnnotations);
            this.GetRelationsHook = new FuncHook<StorageModule, Nullable<Guid>, String, Nullable<Guid>, IEnumerable<Relation>>(this._GetRelations);
            this.GetMarksHook = new FuncHook<StorageModule, Nullable<Guid>, String, Nullable<Guid>, Nullable<DateTime>, String, String, IEnumerable<Mark>>(this._GetMarks);
            this.GetReferencesHook = new FuncHook<StorageModule, Nullable<Guid>, Nullable<DateTime>, String, String, String, Nullable<Guid>, Nullable<DateTime>, String, String, IEnumerable<Reference>>(this._GetReferences);
            this.GetTagsHook = new FuncHook<StorageModule, Nullable<Guid>, Nullable<DateTime>, String, String, String, IEnumerable<Tag>>(this._GetTags);
            this.NewAccountHook = new FuncHook<StorageModule, Guid, String, Account>(this._NewAccount);
            this.NewActivityHook = new FuncHook<StorageModule, Account, DateTime, String, String, String, String, Byte[], Activity>(this._NewActivity);
            this.NewAnnotationHook = new FuncHook<StorageModule, Account, String, Annotation>(this._NewAnnotation);
            this.NewRelationHook = new FuncHook<StorageModule, Account, String, Account, Relation>(this._NewRelation);
            this.NewMarkHook = new FuncHook<StorageModule, Account, String, Activity, Mark>(this._NewMark);
            this.NewReferenceHook = new FuncHook<StorageModule, Activity, String, Activity, Reference>(this._NewReference);
            this.NewTagHook = new FuncHook<StorageModule, Activity, String, Tag>(this._NewTag);
        }

        /// <summary>
        /// <see cref="StorageModule"/> によって使用されているアンマネージ リソースを解放し、オプションでマネージ リソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 <c>true</c>、破棄されない場合は <c>false</c>。</param>
        protected override void Dispose(Boolean disposing)
        {
            this.AccountsLock.Close();
            this.ActivitiesLock.Close();
            this.AnnotationsLock.Close();
            this.RelationsLock.Close();
            this.MarksLock.Close();
            this.ReferencesLock.Close();
            this.TagsLock.Close();
            base.Dispose(disposing);
        }

        public override IEnumerable<Account> GetAccounts(
            Nullable<Guid> accountId,
            String realm
        )
        {
            return this.GetAccountsHook.Execute(accountId, realm);
        }

        /// <summary>
        /// 新しいアカウントを生成します。
        /// </summary>
        /// <param name="accountId">アカウントの ID。</param>
        /// <param name="realm">アカウントのレルム。</param>
        /// <returns>生成されたアカウント。</returns>
        public override Account NewAccount(
            Guid accountId,
            String realm
        )
        {
            return this.NewAccountHook.Execute(accountId, realm);
        }

        protected override IEnumerable<Activity> GetActivities(
            Nullable<Guid> accountId,
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
        /// <returns>生成されたアクティビティ。</returns>
        public override Activity NewActivity(
            Account account,
            DateTime timestamp,
            String category,
            String subId,
            String userAgent,
            String value,
            Byte[] data
        )
        {
            return this.NewActivityHook.Execute(account, timestamp, category, subId, userAgent, value, data);
        }

        protected override IEnumerable<Annotation> GetAnnotations(
            Nullable<Guid> accountId,
            String name
        )
        {
            return this.GetAnnotationsHook.Execute(accountId, name);
        }

        /// <summary>
        /// 新しいアノテーションを生成します。
        /// </summary>
        /// <param name="account">アノテーションが関連付けられるアカウント。</param>
        /// <param name="name">アノテーションの意味。</param>
        /// <returns>生成されたアノテーション。</returns>
        public override Annotation NewAnnotation(
            Account account,
            String name
        )
        {
            return this.NewAnnotationHook.Execute(account, name);
        }

        protected override IEnumerable<Relation> GetRelations(
            Nullable<Guid> accountId,
            String name,
            Nullable<Guid> relatingAccountId
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
        /// <returns>生成されたリレーション。</returns>
        public override Relation NewRelation(
            Account account,
            String name,
            Account relatingAccount
        )
        {
            return this.NewRelationHook.Execute(account, name, relatingAccount);
        }

        public override IEnumerable<Mark> GetMarks(
            Nullable<Guid> accountId,
            String name,
            Nullable<Guid> markingAccountId,
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
        /// <returns>生成されたマーク。</returns>
        public override Mark NewMark(
            Account account,
            String name,
            Activity markingActivity
        )
        {
            return this.NewMarkHook.Execute(account, name, markingActivity);
        }

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
            return this.GetReferencesHook.Execute(accountId, timestamp, category, subId, name, referringAccountId, referringTimestamp, referringCategory, referringSubId);
        }

        /// <summary>
        /// 新しいリファレンスを生成します。
        /// </summary>
        /// <param name="activity">リファレンスが関連付けられているアクティビティ。</param>
        /// <param name="name">リファレンスの意味。</param>
        /// <param name="referringActivity">リファレンスが関連付けられる先のアクティビティ。</param>
        /// <returns>生成されたリファレンス。</returns>
        public override Reference NewReference(
            Activity activity,
            String name,
            Activity referringActivity
        )
        {
            return this.NewReferenceHook.Execute(activity, name, referringActivity);
        }

        public override IEnumerable<Tag> GetTags(
            Nullable<Guid> accountId,
            Nullable<DateTime> timestamp,
            String category,
            String subId,
            String name
        )
        {
            return this.GetTagsHook.Execute(accountId, timestamp, category, subId, name);
        }

        /// <summary>
        /// 新しいタグを生成します。
        /// </summary>
        /// <param name="activity">タグが関連付けられるアクティビティ。</param>
        /// <param name="name">タグの意味。</param>
        /// <returns>生成されたタグ。</returns>
        public override Tag NewTag(
            Activity activity,
            String name
        )
        {
            return this.NewTagHook.Execute(activity, name);
        }

        /// <summary>
        /// このモジュールをサーバ オブジェクトに登録します。
        /// </summary>
        /// <param name="host">登録されるサーバ オブジェクト。</param>
        /// <param name="name">モジュールに設定する名前。</param>
        /// <param name="configuration">モジュールが参照する設定。</param>
        public virtual void Register(ServerCore host, String name, XmlConfiguration configuration)
        {
            this.Host = host;
            this.Name = name;
            this.Configuration = configuration;
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

        private void _Initialize()
        {
            if (this.Configuration.Exists("connection"))
            {
                this.InitializeContext(this.Configuration.ResolveValue<String>("connection"));
            }

            FileInfo file = new FileInfo(Path.Combine(
                this.Host.Directories.CacheDirectory.FullName,
                String.Format("{0}-{1}.cache", this.GetType().Name, this.Name)
            ));
            try
            {
                this.Cache = StorageCache.Load(file, this);
            }
            catch (Exception)
            {
                Cache.Save(file);
            }
        }

        /// <summary>
        /// 指定されたデータ表へのロックが解除されるまで待機します。
        /// </summary>
        /// <param name="waitingLocks">解除されるのを待機するロック。</param>
        public void Wait(StorageObjectTypes waitingLocks)
        {
            this.CheckIfDisposed();
            if (waitingLocks == StorageObjectTypes.None)
            {
                return;
            }
            WaitHandle.WaitAll(this.GetMutexes(waitingLocks).ToArray());
        }

        /// <summary>
        /// <see cref="Wait"/> で取得したロックを解放します。
        /// </summary>
        /// <param name="waitedLocks"><see cref="Wait"/> で取得したロック。</param>
        public void Release(StorageObjectTypes waitedLocks)
        {
            this.CheckIfDisposed();
            if (waitedLocks == StorageObjectTypes.None)
            {
                return;
            }
            this.GetMutexes(waitedLocks).ForEach(m => m.ReleaseMutex());
        }

        /// <summary>
        /// ロックが全て解放されている場合のみ、ストレージ オブジェクトの変更をデータ ソースに保存します。
        /// </summary>
        /// <returns>ロックが全て解放されていた場合、データ ソースにおいて処理が行われた行数。それ以外の場合、0 未満の値。</returns>
        public Int32 TryUpdate()
        {
            this.CheckIfDisposed();
            // Test or get whether all mutexes is free.
            if (WaitHandle.WaitAll(this.GetMutexes(StorageObjectTypes.All).ToArray(), 0))
            {
                Int32 ret = base.Update();
                this.Release(StorageObjectTypes.All);
                return ret;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// ロックが解除されるのを待って、ストレージ オブジェクトの変更をデータ ソースに保存します。
        /// </summary>
        /// <returns>データ ソースにおいて処理が行われた行数。</returns>
        public override Int32 Update()
        {
            this.Wait(StorageObjectTypes.All);
            Int32 ret = base.Update();
            this.Release(StorageObjectTypes.All);
            return ret;
        }

        private IEnumerable<Mutex> GetMutexes(StorageObjectTypes locks)
        {
            LinkedList<Mutex> mutexes = new LinkedList<Mutex>();
            if ((locks & StorageObjectTypes.Account) == StorageObjectTypes.Account)
            {
                mutexes.AddLast(this.AccountsLock);
            }
            if ((locks & StorageObjectTypes.Activity) == StorageObjectTypes.Activity)
            {
                mutexes.AddLast(this.ActivitiesLock);
            }
            if ((locks & StorageObjectTypes.Annotation) == StorageObjectTypes.Annotation)
            {
                mutexes.AddLast(this.AnnotationsLock);
            }
            if ((locks & StorageObjectTypes.Relation) == StorageObjectTypes.Relation)
            {
                mutexes.AddLast(this.RelationsLock);
            }
            if ((locks & StorageObjectTypes.Mark) == StorageObjectTypes.Mark)
            {
                mutexes.AddLast(this.MarksLock);
            }
            if ((locks & StorageObjectTypes.Reference) == StorageObjectTypes.Reference)
            {
                mutexes.AddLast(this.ReferencesLock);
            }
            if ((locks & StorageObjectTypes.Tag) == StorageObjectTypes.Tag)
            {
                mutexes.AddLast(this.TagsLock);
            }
            return mutexes;
        }

        #region Helper Methods

        private IEnumerable<Account> _GetAccounts(
            Nullable<Guid> accountId,
            String realm
        )
        {
            return base.GetAccounts(accountId, realm);
        }

        private Account _NewAccount(
            Guid accountId,
            String realm
        )
        {
            return base.NewAccount(accountId, realm);
        }

        private IEnumerable<Activity> _GetActivities(
            Nullable<Guid> accountId,
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

        private Activity _NewActivity(
            Account account,
            DateTime timestamp,
            String category,
            String subId,
            String userAgent,
            String value,
            Byte[] data
        )
        {
            return base.NewActivity(account, timestamp, category, subId, userAgent, value, data);
        }

        private IEnumerable<Annotation> _GetAnnotations(
            Nullable<Guid> accountId,
            String name
        )
        {
            return base.GetAnnotations(accountId, name);
        }

        private Annotation _NewAnnotation(
            Account account,
            String name
        )
        {
            return base.NewAnnotation(account, name);
        }

        private IEnumerable<Relation> _GetRelations(
            Nullable<Guid> accountId,
            String name,
            Nullable<Guid> relatingAccountId
        )
        {
            return base.GetRelations(accountId, name, relatingAccountId);
        }

        private Relation _NewRelation(
            Account account,
            String name,
            Account relatingAccount
        )
        {
            return base.NewRelation(account, name, relatingAccount);
        }

        private IEnumerable<Mark> _GetMarks(
            Nullable<Guid> accountId,
            String name,
            Nullable<Guid> markingAccountId,
            Nullable<DateTime> markingTimestamp,
            String markingCategory,
            String markingSubId
        )
        {
            return base.GetMarks(accountId, name, markingAccountId, markingTimestamp, markingCategory, markingSubId);
        }

        private Mark _NewMark(
            Account account,
            String name,
            Activity markingActivity
        )
        {
            return base.NewMark(account, name, markingActivity);
        }

        private IEnumerable<Reference> _GetReferences(
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
            return base.GetReferences(accountId, timestamp, category, subId, name, referringAccountId, referringTimestamp, referringCategory, referringSubId);
        }

        private Reference _NewReference(
            Activity activity,
            String name,
            Activity referringActivity
        )
        {
            return base.NewReference(activity, name, referringActivity);
        }

        private IEnumerable<Tag> _GetTags(
            Nullable<Guid> accountId,
            Nullable<DateTime> timestamp,
            String category,
            String subId,
            String name
        )
        {
            return base.GetTags(accountId, timestamp, category, subId, name);
        }

        private Tag _NewTag(
            Activity activity,
            String name
        )
        {
            return base.NewTag(activity, name);
        }

        #endregion
    }
}