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
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace XSpect.MetaTweet.Objects
{
    /// <summary>
    /// <see cref="Storage"/> にキャッシュ機能を提供します。
    /// </summary>
    /// <remarks>
    /// ストレージ キャッシュは、ストレージおよびストレージ オブジェクトにおける高コストのデータ取得処理を高速化するために用意された機構です。
    /// </remarks>
    [Serializable()]
    public partial class StorageCache
        : MarshalByRefObject,
          ISerializable
    {
        /// <summary>
        /// キャッシュのソースとなるストレージを取得します。
        /// </summary>
        /// <value>
        /// キャッシュのソースとなるストレージ。
        /// </value>
        public Storage Storage
        {
            get;
            private set;
        }

        /// <summary>
        /// キャッシュ ファイルを保存または読み込んだファイルを取得します。
        /// </summary>
        /// <value>
        /// キャッシュ ファイルを保存または読み込んだファイル。
        /// </value>
        public FileInfo CacheFile
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="ActivityCache"/> を取得します。
        /// </summary>
        /// <value>
        /// <see cref="ActivityCache"/>。
        /// </value>
        public ActivityCache Activities
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="AddingObjectCache"/> を取得します。
        /// </summary>
        /// <value>
        /// <see cref="AddingObjectCache"/>。
        /// </value>
        public AddingObjectCache AddingObjects
        {
            get;
            private set;
        }

        /// <summary>
        /// インフラストラクチャ。
        /// </summary>
        /// <param name="info">オブジェクトのシリアル化または逆シリアル化に必要なデータ。</param>
        /// <param name="context">指定したシリアル化ストリームの転送元と転送先。</param>
        protected StorageCache(SerializationInfo info, StreamingContext context)
        {
            this.Storage = (Storage) info.GetValue("Storage", typeof(Storage));
            if (this.Storage is ProxyStorage)
            {
                this.Storage = ((ProxyStorage) this.Storage).Target;
            }
            this.Activities = (ActivityCache) info.GetValue("Activities", typeof(ActivityCache));
            this.Activities.GetType().GetProperty("Cache").SetValue(this.Activities, this, null);
            this.AddingObjects = new AddingObjectCache(this);
        }

        /// <summary>
        /// <see cref="StorageCache"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="storage">キャッシュのソースとなるストレージ。</param>
        public StorageCache(Storage storage)
        {
            this.Storage = storage;
            this.Activities = new ActivityCache(this);
            this.AddingObjects = new AddingObjectCache(this);
        }

        /// <summary>
        /// Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns>
        /// An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease"/> used to control the lifetime policy for this instance. This is the current lifetime service object for this instance if one exists; otherwise, a new lifetime service object initialized to the value of the <see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime"/> property.
        /// </returns>
        /// <exception cref="T:System.Security.SecurityException">
        /// The immediate caller does not have infrastructure permission.
        /// </exception>
        /// <PermissionSet>
        /// <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure"/>
        /// </PermissionSet>
        public override Object InitializeLifetimeService()
        {
            return null;
        }

        /// <summary>
        /// オブジェクトをシリアル化するために必要なデータをシリアル化情報オブジェクトに設定します。  
        /// </summary>
        /// <param name="info">オブジェクトと関連付けられているシリアル化データを保持する <see cref="SerializationInfo"/>。</param>
        /// <param name="context">オブジェクトに関連付けられているシリアル化ストリームのソースおよびデスティネーションを格納している <see cref="StreamingContext"/>。</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Storage", (context.State & StreamingContextStates.File) != StreamingContextStates.File
                ? this.Storage is ProxyStorage
                      ? this.Storage
                      : new ProxyStorage(this.Storage)
                : null
            );
            info.AddValue("Activities", this.Activities, typeof(ActivityCache));
        }

        /// <summary>
        /// ファイルからキャッシュ データを読み込みます。
        /// </summary>
        /// <param name="file">読み込むファイル。</param>
        /// <param name="storage">キャッシュのソースとなるストレージ。</param>
        /// <returns>ファイルから読み込まれた <see cref="StorageCache"/>。</returns>
        public static StorageCache Load(FileInfo file, Storage storage)
        {
            using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                StorageCache cache = (StorageCache) new BinaryFormatter().Deserialize(stream);
                cache.CacheFile = file;
                cache.Storage = storage;
                foreach (Activity activity in cache.Activities)
                {
                    activity.Storage = cache.Storage;
                    activity.Attach();
                    if (!activity.AccountReference.IsLoaded)
                    {
                        activity.AccountReference.Load();
                        activity.Account.Storage = cache.Storage;
                    }
                    activity.EndInit();
                }
                return cache;
            }
        }

        /// <summary>
        /// キャッシュ データをファイルに保存します。
        /// </summary>
        /// <param name="file">保存先のファイル。</param>
        public void Save(FileInfo file)
        {
            using (FileStream stream = file.Open(FileMode.Create, FileAccess.Write, FileShare.None))
            {
                new BinaryFormatter().Serialize(stream, this);
            }
            this.CacheFile = file;
        }

        /// <summary>
        /// キャッシュ データを保存または読み込んだファイルに保存します。
        /// </summary>
        public void Save()
        {
            if (this.CacheFile != null)
            {
                this.Save(this.CacheFile);
            }
        }
    }
}