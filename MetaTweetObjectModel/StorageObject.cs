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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;

namespace XSpect.MetaTweet.Objects
{
    /// <summary>
    /// 全ての MetaTweet ストレージ オブジェクトの基本クラスを表します。
    /// </summary>
    [Serializable()]
    [DataContract(IsReference = true)]
    public abstract class StorageObject
        : EntityObject,
          IComparable<StorageObject>,
          IEquatable<StorageObject>,
          ISerializable,
          ISupportInitialize
    {
        private Storage _storage;

        [NonSerialized()]
        private Boolean _isInitializing;

        /// <summary>
        /// このオブジェクトがアタッチされているコンテキストを保持するストレージを取得または設定します。
        /// </summary>
        /// <value>このオブジェクトがアタッチされているコンテキストを保持するストレージ。</value>
        /// <remarks>
        /// <para>プロパティに新たなストレージを設定することで、このオブジェクトをストレージが保持するコンテキストにアタッチすることができます。</para>
        /// <para>プロパティの値が <c>null</c> 以外の時に新たなストレージを設定すると、現在設定されているストレージが保持するコンテキストからこのオブジェクトがデタッチされます。</para>
        /// </remarks>
        public Storage Storage
        {
            get
            {
                return this._storage;
            }
            set
            {
                if (value == this.Storage)
                {
                    return;
                }
                if (this._storage != null && this.EntityState != System.Data.EntityState.Detached)
                {
                    throw new InvalidOperationException("This object cannot change the Storage because of the EntityState.");
                }
                this._storage = value;
            }
        }

        /// <summary>
        /// オブジェクトが初期化中かどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// インスタンスが初期化中の場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </value>
        /// <remarks>
        /// オブジェクトが初期化中の場合の挙動は実装により異なります。
        /// </remarks>
        protected Boolean IsInitializing
        {
            get
            {
                return this._isInitializing;
            }
        }

        /// <summary>
        /// オブジェクトをストレージにアタッチしたときに発生します。
        /// </summary>
        public event EventHandler<ObjectStateEventArgs> Attached;

        /// <summary>
        /// オブジェクトをストレージからデタッチしたときに発生します。
        /// </summary>
        public event EventHandler<ObjectStateEventArgs> Detached;

        /// <summary>
        /// オブジェクトを削除の対象としてマークしたときに発生します。
        /// </summary>
        public event EventHandler<ObjectStateEventArgs> Deleted;

        /// <summary>
        /// <see cref="StorageObject"/> の新しいインスタンスを初期化します。
        /// </summary>
        protected StorageObject()
        {
            this._isInitializing = true;
        }

        /// <summary>
        /// インフラストラクチャ。
        /// </summary>
        /// <param name="info">オブジェクトのシリアル化または逆シリアル化に必要なデータ。</param>
        /// <param name="context">指定したシリアル化ストリームの転送元と転送先。</param>
        protected StorageObject(SerializationInfo info, StreamingContext context)
            : this()
        {
            Storage storage;
            this.Storage = context.State != StreamingContextStates.File
                ? (storage = (Storage) info.GetValue("Storage", typeof(Storage)))
                : null;
        }

        /// <summary>
        /// <see cref="StorageObject"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="storage">オブジェクトが追加されるストレージ。</param>
        protected StorageObject(Storage storage)
            : this()
        {
            this._storage = storage;
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
        /// Value
        /// Meaning
        /// Less than zero
        /// This object is less than the <paramref name="other"/> parameter.
        /// Zero
        /// This object is equal to <paramref name="other"/>.
        /// Greater than zero
        /// This object is greater than <paramref name="other"/>.
        /// </returns>
        public abstract Int32 CompareTo(StorageObject other);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public Boolean Equals(StorageObject other)
        {
            return this.CompareTo(other) == 0;
        }

        /// <summary>
        /// 指定したストレージ オブジェクトが、このストレージ オブジェクトと完全に等しいかどうかを判断します。
        /// </summary>
        /// <param name="other">このストレージ オブジェクトと比較するストレージ オブジェクト。</param>
        /// <returns>
        /// 指定したストレージ オブジェクトがこのストレージ オブジェクトと完全に等しい場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        public abstract Boolean EqualsExact(StorageObject other);

        /// <summary>
        /// オブジェクトをシリアル化するために必要なデータをシリアル化情報オブジェクトに設定します。  
        /// </summary>
        /// <param name="info">オブジェクトと関連付けられているシリアル化データを保持する <see cref="SerializationInfo"/>。</param>
        /// <param name="context">オブジェクトに関連付けられているシリアル化ストリームのソースおよびデスティネーションを格納している <see cref="StreamingContext"/>。</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Storage", context.State != StreamingContextStates.File
                ? this._storage is ProxyStorage
                      ? this._storage
                      : new ProxyStorage(this._storage)
                : null
            );
        }

        /// <summary>
        /// 初期化の開始を通知するシグナルをオブジェクトに送信します。
        /// </summary>
        public virtual void BeginInit()
        {
            this._isInitializing = true;
        }

        /// <summary>
        /// 初期化の完了を通知するシグナルをオブジェクトに送信します。
        /// </summary>
        public virtual void EndInit()
        {
            this._isInitializing = false;
        }

        /// <summary>
        /// このオブジェクトの完全な内容を表す <see cref="String"/> を返します。
        /// </summary>
        /// <returns>このオブジェクトの完全な内容を表す <see cref="String"/>。</returns>
        public abstract String Describe();

        /// <summary>
        /// アタッチ後の処理を完了した後に <see cref="Attached"/> イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している <see cref="ObjectStateEventArgs"/>。</param>
        protected virtual void OnAttached(ObjectStateEventArgs e)
        {
            if (this.Attached != null)
            {
                this.Attached(this, e);
            }
        }

        /// <summary>
        /// デタッチ後の処理を完了した後に <see cref="Detached"/> イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している <see cref="ObjectStateEventArgs"/>。</param>
        protected virtual void OnDetached(ObjectStateEventArgs e)
        {
            if (this.Detached != null)
            {
                this.Detached(this, e);
            }
        }

        /// <summary>
        /// 削除後の処理を完了した後に <see cref="Deleted"/> イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している <see cref="ObjectStateEventArgs"/>。</param>
        protected virtual void OnDeleted(ObjectStateEventArgs e)
        {
            if (this.Deleted != null)
            {
                this.Deleted(this, e);
            }
        }

        /// <summary>
        /// オブジェクトをストレージにアタッチします。
        /// </summary>
        public void Attach()
        {
            System.Data.EntityState previous = this.EntityState;
            this.Storage.AttachObject(this);
            this.OnAttached(new ObjectStateEventArgs(previous));
        }

        /// <summary>
        /// オブジェクトをストレージからデタッチします。
        /// </summary>
        public void Detach()
        {
            if (this.EntityState == System.Data.EntityState.Added)
            {
                this.Storage.Cache.AddingObjects.Remove(this);
            }
            System.Data.EntityState previous = this.EntityState;
            this.Storage.DetachObject(this);
            this.OnDetached(new ObjectStateEventArgs(previous));
        }

        /// <summary>
        /// オブジェクトを削除の対象としてマークします。
        /// </summary>
        public void Delete()
        {
            if (this.EntityState == System.Data.EntityState.Added)
            {
                this.Storage.Cache.AddingObjects.Remove(this);
            }
            System.Data.EntityState previous = this.EntityState;
            this.Storage.DeleteObject(this);
            this.OnDeleted(new ObjectStateEventArgs(previous));
        }

        /// <summary>
        /// オブジェクトをデータ ソース内のデータで更新します。
        /// </summary>
        /// <param name="refreshMode">更新の方法を示す値。</param>
        public void Refresh(RefreshMode refreshMode)
        {
            this.Storage.RefreshObject(refreshMode, this);
        }
    }
}