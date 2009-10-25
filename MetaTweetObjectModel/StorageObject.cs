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
using System.ComponentModel;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;

namespace XSpect.MetaTweet.Objects
{
    /// <summary>
    /// 全ての MetaTweet ストレージ オブジェクトの基本クラスを表します。
    /// </summary>
    [Serializable()]
    public abstract class StorageObject
        : EntityObject,
          IComparable<StorageObject>,
          ISupportInitialize
    {
        [NonSerialized()]
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
                if (this.Storage != null)
                {
                    this.Storage.Entities.Detach(this);
                }
                if (value != null)
                {
                    this.Storage.Entities.Attach(this);
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
        /// オブジェクトの削除が完了したときに発生します。
        /// </summary>
        public event EventHandler<EventArgs> Deleted;

        /// <summary>
        /// <see cref="StorageObject"/> の新しいインスタンスを初期化します。
        /// </summary>
        protected StorageObject()
        {
            this.BeginInit();
        }

        /// <summary>
        /// <see cref="StorageObject"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="storage">オブジェクトが追加されるストレージ。</param>
        protected StorageObject(Storage storage)
            : this()
        {
            this.Storage = storage;
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
        /// 削除後の処理を完了した後に <see cref="Deleted"/> イベントを発生させます。
        /// </summary>
        /// <param name="e">イベント データを格納している <see cref="EventArgs"/>。</param>
        protected virtual void OnDeleted(EventArgs e)
        {
            this.Deleted(this, e);
        }

        /// <summary>
        /// オブジェクトをストレージから削除します。
        /// </summary>
        public void Delete()
        {
            this.Storage.Entities.DeleteObject(this);
            this.OnDeleted(EventArgs.Empty);
        }

        /// <summary>
        /// オブジェクトの状態をストレージと同期します。
        /// </summary>
        /// <param name="refreshMode">更新の方法を示す値。</param>
        public void Refresh(RefreshMode refreshMode)
        {
            this.Storage.Entities.Refresh(refreshMode, this);
        }
    }
}