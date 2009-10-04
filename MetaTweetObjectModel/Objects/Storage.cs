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

        public Account NewAccount(Guid accountId, String realm)
        {
            Account account = new Account(this)
            {
                AccountId = accountId,
                Realm = realm,
            };
            this.Entities.AddToAccountSet(account);
            return account;
        }

        public Account NewAccount(Guid accountId)
        {
            Account account = new Account(this)
            {
                AccountId = accountId,
            };
            this.Entities.AddToAccountSet(account);
            return account;
        }

        public Activity NewActivity(Account account, DateTime timestamp, String category, String subId, String userAgent, String value, Byte[] data)
        {
            Activity activity = new Activity(this)
            {
                Account = account,
                Timestamp = timestamp,
                Category = category,
                SubId = subId,
                UserAgent = userAgent,
                Value = value,
                Data = data,
            };
            this.Entities.AddToActivitySet(activity);
            return activity;
        }

        public Activity NewActivity(Account account, DateTime timestamp, String category, String subId)
        {
            Activity activity = new Activity(this)
            {
                Account = account,
                Timestamp = timestamp,
                Category = category,
                SubId = subId,
            };
            this.Entities.AddToActivitySet(activity);
            return activity;
        }

        public Annotation NewAnnotation(Account account, String name)
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

        public Relation NewRelation(Account account, String name, Account relatingAccount)
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

        public Mark NewMark(Account account, String name, Activity markingActivity)
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

        public Reference NewReference(Activity activity, String name, Activity referringActivity)
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

        public Tag NewTag(Activity activity, String name)
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
    }
}