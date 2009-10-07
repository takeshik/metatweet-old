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
using XSpect.MetaTweet.Objects;

namespace XSpect.MetaTweet.Objects
{
    public class Storage
        : MarshalByRefObject,
          IDisposable
    {
        private Boolean _disposed;

        public StorageEntities Entities
        {
            get;
            private set;
        }

        public ObjectQuery<Account> Accounts
        {
            get
            {
                return this.Entities.AccountSet;
            }
        }

        public ObjectQuery<Activity> Activities
        {
            get
            {
                return this.Entities.ActivitySet;
            }
        }
        
        public ObjectQuery<Annotation> Annotations
        {
            get
            {
                return this.Entities.AnnotationSet;
            }
        }
        
        public ObjectQuery<Relation> Relations
        {
            get
            {
                return this.Entities.RelationSet;
            }
        }
        
        public ObjectQuery<Mark> Marks
        {
            get
            {
                return this.Entities.MarkSet;
            }
        }
        
        public ObjectQuery<Reference> References
        {
            get
            {
                return this.Entities.ReferenceSet;
            }
        }
        
        public ObjectQuery<Tag> Tags
        {
            get
            {
                return this.Entities.TagSet;
            }
        }

        /// <summary>
        /// <see cref="Storage"/> の新しいインスタンスを初期化します。
        /// </summary>
        public Storage()
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
        /// バックエンドのデータソースとの接続を初期化します。
        /// </summary>
        public virtual void Initialize()
        {
            this.Entities = new StorageEntities();
        }

        /// <summary>
        /// バックエンドのデータソースとの接続を初期化します。
        /// </summary>
        /// <param name="connectionString">接続に使用する文字列。</param>
        public virtual void Initialize(String connectionString)
        {
            this.Entities = new StorageEntities(connectionString);
        }

        /// <summary>
        /// バックエンドのデータソースとの接続を初期化します。
        /// </summary>
        /// <param name="connection">使用する接続。</param>
        public virtual void Initialize(EntityConnection connection)
        {
            this.Entities = new StorageEntities(connection);
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
            this.Entities.Dispose();
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

        public virtual IQueryable<Account> GetAccounts(
            Nullable<Guid> accountId,
            String realm
        )
        {
            IQueryable<Account> accounts = this.Accounts;
            if (accountId.HasValue)
            {
                accounts = accounts.Where(a => a.AccountId == accountId);
            }
            if (realm != null)
            {
                accounts = accounts.Where(a => a.Realm == realm);
            }
            return accounts;
        }

        public virtual IQueryable<Account> GetAccounts(
            Nullable<Guid> accountId
        )
        {
            return this.GetAccounts(accountId, null);
        }

        public virtual Account NewAccount(Guid accountId, String realm)
        {
            Account account = new Account(this)
            {
                AccountId = accountId,
                Realm = realm,
            };
            this.Entities.AddToAccountSet(account);
            return account;
        }

        #endregion

        #region Activity

        public virtual IQueryable<Activity> GetActivities(
            Nullable<Guid> accountId,
            Nullable<DateTime> timestamp,
            String category,
            String subId,
            String userAgent,
            Object value,
            Object data
        )
        {
            IQueryable<Activity> activities = this.Activities;
            if (accountId.HasValue)
            {
                activities = activities.Where(a => a.AccountId == accountId);
            }
            if (timestamp.HasValue)
            {
                activities = activities.Where(a => a.Timestamp == timestamp.Value.ToUniversalTime());
            }
            if (category != null)
            {
                activities = activities.Where(a => a.Category == category);
            }
            if (subId != null)
            {
                activities = activities.Where(a => a.SubId == subId);
            }
            if (userAgent != null)
            {
                activities = activities.Where(a => a.UserAgent == userAgent);
            }
            if (value != null && value is DBNull)
            {
                activities = activities.Where(a => a.Value == null);
            }
            else
            {
                activities = activities.Where(a => a.Value == value.ToString());
            }
            if (value != null && value is DBNull)
            {
                activities = activities.Where(a => a.Data == null);
            }
            else
            {
                activities = activities.Where(a => a.Data == value);
            }
            return activities;
        }

        public IQueryable<Activity> GetActivities(
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

        public IQueryable<Activity> GetActivities(
            Nullable<Guid> accountId,
            Nullable<DateTime> timestamp,
            String category,
            String subId
        )
        {
            return this.GetActivities(accountId, timestamp, category, subId, null, null, null);
        }

        public IQueryable<Activity> GetActivities(
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
                subId
            );
        }

        public virtual Activity NewActivity(Account account, DateTime timestamp, String category, String subId, String userAgent, String value, Byte[] data)
        {
            Activity activity = new Activity(this)
            {
                Account = account,
                Timestamp = timestamp,
                Category = category,
                SubId = subId ?? String.Empty,
                UserAgent = userAgent,
                Value = value,
                Data = data,
            };
            this.Entities.AddToActivitySet(activity);
            return activity;
        }

        public Activity NewActivity(Account account, DateTime timestamp, String category, String subId)
        {
            return this.NewActivity(account, timestamp, category, subId, null, null, null);
        }

        #endregion

        #region Annotation

        public virtual IQueryable<Annotation> GetAnnotations(
            Nullable<Guid> accountId,
            String name
        )
        {
            IQueryable<Annotation> annotations = this.Annotations;
            if (accountId.HasValue)
            {
                annotations = annotations.Where(a => a.AccountId == accountId);
            }
            if (name != null)
            {
                annotations = annotations.Where(a => a.Name == name);
            }
            return annotations;
        }

        public IQueryable<Annotation> GetAnnotations(
            Account account,
            String name
        )
        {
            return this.GetAnnotations(
                account != null ? account.AccountId : default(Nullable<Guid>),
                name
            );
        }

        public virtual Annotation NewAnnotation(Account account, String name)
        {
            Annotation annotation = new Annotation(this)
            {
                Account = account,
                Name = name,
            };
            this.Entities.AddToAnnotationSet(annotation);
            account.Annotations.Add(annotation);
            return annotation;
        }

        #endregion

        #region Relation

        public virtual IQueryable<Relation> GetRelations(
            Nullable<Guid> accountId,
            String name,
            Nullable<Guid> relatingAccountId
        )
        {
            IQueryable<Relation> relations = this.Relations;
            if (accountId.HasValue)
            {
                relations = relations.Where(r => r.AccountId == accountId);
            }
            if (name != null)
            {
                relations = relations.Where(r => r.Name == name);
            }
            if (relatingAccountId.HasValue)
            {
                relations = relations.Where(r => r.RelatingAccountId == relatingAccountId);
            }
            return relations;
        }

        public IQueryable<Relation> GetRelations(
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

        public virtual Relation NewRelation(Account account, String name, Account relatingAccount)
        {
            Relation relation = new Relation(this)
            {
                Account = account,
                Name = name,
                RelatingAccount = relatingAccount,
            };
            this.Entities.AddToRelationSet(relation);
            account.Relations.Add(relation);
            return relation;
        }

        #endregion

        #region Mark

        public virtual IQueryable<Mark> GetMarks(
            Nullable<Guid> accountId,
            String name,
            Nullable<Guid> markingAccountId,
            Nullable<DateTime> markingTimestamp,
            String markingCategory,
            String markingSubId
        )
        {
            IQueryable<Mark> marks = this.Marks;
            if (accountId.HasValue)
            {
                marks = marks.Where(m => m.AccountId == accountId);
            }
            if (name != null)
            {
                marks = marks.Where(m => m.Name == name);
            }
            if (markingAccountId.HasValue)
            {
                marks = marks.Where(m => m.MarkingAccountId == markingAccountId);
            }
            if (markingTimestamp.HasValue)
            {
                marks = marks.Where(m => m.MarkingTimestamp == markingTimestamp);
            }
            if (markingCategory != null)
            {
                marks = marks.Where(m => m.MarkingCategory == markingCategory);
            }
            if (markingSubId != null)
            {
                marks = marks.Where(m => m.MarkingSubId == markingSubId);
            }
            return marks;
        }

        public IQueryable<Mark> GetMarks(
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

        public virtual Mark NewMark(Account account, String name, Activity markingActivity)
        {
            Mark mark = new Mark(this)
            {
                Account = account,
                Name = name,
                MarkingActivity = markingActivity,
            };
            this.Entities.AddToMarkSet(mark);
            account.Marks.Add(mark);
            return mark;
        }

        #endregion

        #region Reference

        public virtual IQueryable<Reference> GetReferences(
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
            IQueryable<Reference> references = this.References;
            if (accountId.HasValue)
            {
                references = references.Where(r => r.AccountId == accountId);
            }
            if (timestamp.HasValue)
            {
                references = references.Where(r => r.Timestamp == timestamp);
            }
            if (category != null)
            {
                references = references.Where(r => r.Category == category);
            }
            if (subId != null)
            {
                references = references.Where(r => r.SubId == subId);
            }
            if (referringAccountId.HasValue)
            {
                references = references.Where(r => r.ReferringAccountId == referringAccountId);
            }
            if (referringTimestamp.HasValue)
            {
                references = references.Where(r => r.ReferringTimestamp == referringTimestamp);
            }
            if (referringCategory != null)
            {
                references = references.Where(r => r.ReferringCategory == referringCategory);
            }
            if (referringSubId != null)
            {
                references = references.Where(r => r.ReferringSubId == referringSubId);
            }
            return references;
        }

        public IQueryable<Reference> GetReferences(
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

        public virtual Reference NewReference(Activity activity, String name, Activity referringActivity)
        {
            Reference reference = new Reference(this)
            {
                Activity = activity,
                Name = name,
                ReferringActivity = referringActivity,
            };
            this.Entities.AddToReferenceSet(reference);
            activity.References.Add(reference);
            return reference;
        }

        #endregion

        #region Tag

        public virtual IQueryable<Tag> GetTags(
            Nullable<Guid> accountId,
            Nullable<DateTime> timestamp,
            String category,
            String subId,
            String name
        )
        {
            IQueryable<Tag> tags = this.Tags;
            if (accountId.HasValue)
            {
                tags = tags.Where(t => t.AccountId == accountId);
            }
            if (timestamp.HasValue)
            {
                tags = tags.Where(t => t.Timestamp == timestamp);
            }
            if (category != null)
            {
                tags = tags.Where(t => t.Category == category);
            }
            if (subId != null)
            {
                tags = tags.Where(t => t.SubId == subId);
            }
            if (name != null)
            {
                tags = tags.Where(t => t.Name == name);
            }
            return tags;
        }

        public IQueryable<Tag> GetTags(
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

        public virtual Tag NewTag(Activity activity, String name)
        {
            Tag tag = new Tag(this)
            {
                Activity = activity,
                Name = name,
            };
            this.Entities.AddToTagSet(tag);
            activity.Tags.Add(tag);
            return tag;
        }

        #endregion

        public Int32 Update()
        {
            return this.Entities.SaveChanges();
        }
    }
}