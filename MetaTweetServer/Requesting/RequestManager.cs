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
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Achiral.Extension;
using XSpect.Extension;
using XSpect.MetaTweet.Modules;
using XSpect.MetaTweet.Properties;

namespace XSpect.MetaTweet.Requesting
{
    /// <summary>
    /// <see cref="Request"/> および <see cref="IRequestTask"/> の管理を行ないます。
    /// </summary>
    public class RequestManager
        : MarshalByRefObject,
          IRequestManager
    {
        private readonly ConcurrentDictionary<Int32, IRequestTask> _dictionary;

        /// <summary>
        /// このオブジェクトを保持する <see cref="ServerCore"/> オブジェクトを取得します。
        /// </summary>
        /// <value>
        /// このオブジェクトを保持する <see cref="ServerCore"/> オブジェクト。
        /// </value>
        public IServerCore Parent
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="IRequestTask"/> の ID の最大値を取得します。
        /// </summary>
        /// <value>
        /// <see cref="IRequestTask"/> の ID の最大値。
        /// </value>
        public Int32 MaxRequestId
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="RequestManager"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="parent">親となる <see cref="ModuleManager"/>。</param>
        public RequestManager(ServerCore parent)
        {
            this._dictionary = new ConcurrentDictionary<Int32, IRequestTask>();
            this.Parent = parent;
            this.MaxRequestId = 65536;
        }

        public override Object InitializeLifetimeService()
        {
            return null;
        }

        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<IRequestTask> GetEnumerator()
        {
            return this._dictionary.Values.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<IRequestTask>

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        void ICollection<IRequestTask>.Add(IRequestTask item)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
        void ICollection<IRequestTask>.Clear()
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        public Boolean Contains(IRequestTask item)
        {
            return this._dictionary.Values.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
        public void CopyTo(IRequestTask[] array, Int32 arrayIndex)
        {
            this._dictionary.Values.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        Boolean ICollection<IRequestTask>.Remove(IRequestTask item)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public Int32 Count
        {
            get
            {
                return this._dictionary.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        Boolean ICollection<IRequestTask>.IsReadOnly
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region Implementation of IList<IRequestTask>

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </summary>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        public Int32 IndexOf(IRequestTask item)
        {
            return this.Contains(item) ? item.Id : -1;
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param><param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        void IList<IRequestTask>.Insert(Int32 index, IRequestTask item)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        void IList<IRequestTask>.RemoveAt(Int32 index)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        /// <param name="index">The zero-based index of the element to get or set.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public IRequestTask this[Int32 index]
        {
            get
            {
                return this._dictionary[index];
            }
            set
            {
                throw new InvalidOperationException();
            }
        }

        #endregion

        /// <summary>
        /// <see cref="RequestManager"/> によって使用されているすべてのリソースを解放します。
        /// </summary>
        public void Dispose()
        {
            this.ForEach(t => t.Cancel());
        }

        /// <summary>
        /// イベントを記録するログ ライタを取得します。
        /// </summary>
        /// <value>イベントを記録するログ ライタ。</value>
        public ILog Log
        {
            get
            {
                return this.Parent.Let(
                    s => s.LogManager[s.Configuration.Loggers.RequestManager]
                );
            }
        }

        /// <summary>
        /// <see cref="IRequestTask"/> を作成し、登録します。
        /// </summary>
        /// <param name="request">実行する <see cref="Request"/>。</param>
        /// <returns>作成され、登録された <see cref="IRequestTask"/>。</returns>
        public IRequestTask Register(Request request)
        {
            IRequestTask task = new RequestTask(this, request);
            this._dictionary.GetOrAdd(task.Id, task);
            this.Log.Info(Resources.ServerRequestExecuting, request);
            return task;
        }

        /// <summary>
        /// <see cref="IRequestTask"/> を作成、登録し、開始します。
        /// </summary>
        /// <typeparam name="TOutput">このタスクの出力の型。</typeparam>
        /// <param name="request">実行する <see cref="Request"/>。</param>
        /// <returns>作成、登録し、開始された <see cref="IRequestTask"/>。</returns>
        public IRequestTask Start<TOutput>(Request request)
        {
            return this.Register(request).Apply(t => t.Start<TOutput>());
        }

        /// <summary>
        /// <see cref="IRequestTask"/> を作成、登録し、開始します。
        /// </summary>
        /// <param name="request">実行する <see cref="Request"/>。</param>
        /// <param name="outputType">このタスクの出力の型を表すオブジェクト。</param>
        /// <returns>作成、登録し、開始された <see cref="IRequestTask"/>。</returns>
        public IRequestTask Start(Request request, Type outputType)
        {
            return this.Register(request).Apply(t => t.Start(outputType));
        }

        /// <summary>
        /// <see cref="IRequestTask"/> を作成、登録し、開始します。
        /// </summary>
        /// <param name="request">実行する <see cref="Request"/>。</param>
        /// <returns>作成、登録し、開始された <see cref="IRequestTask"/>。</returns>
        public IRequestTask Start(Request request)
        {
            return this.Start(request, null);
        }

        /// <summary>
        /// <see cref="IRequestTask"/> を作成、登録、開始し、終了するまで待機します。
        /// </summary>
        /// <typeparam name="TOutput">このタスクの出力の型。</typeparam>
        /// <param name="request">実行する <see cref="Request"/>。</param>
        /// <returns>タスクの結果となる出力。</returns>
        public TOutput Execute<TOutput>(Request request)
        {
            return this.Start<TOutput>(request).Execute<TOutput>();
        }

        /// <summary>
        /// <see cref="IRequestTask"/> を作成、登録、開始し、終了するまで待機します。
        /// </summary>
        /// <param name="outputType">このタスクの出力の型を表すオブジェクト。</param>
        /// <param name="request">実行する <see cref="Request"/>。</param>
        /// <returns>タスクの結果となる出力。</returns>
        public Object Execute(Request request, Type outputType)
        {
            return this.Start(request, outputType).Execute(outputType);
        }

        /// <summary>
        /// <see cref="IRequestTask"/> を作成、登録、開始し、終了するまで待機します。
        /// </summary>
        /// <param name="request">実行する <see cref="Request"/>。</param>
        /// <returns>タスクの結果となる出力。</returns>
        public Object Execute(Request request)
        {
            return this.Execute(request, null);
        }

        /// <summary>
        /// 登録されている <see cref="IRequestTask"/> を削除します。
        /// </summary>
        /// <param name="task">削除する <see cref="IRequestTask"/>。</param>
        public void Clean(IRequestTask task)
        {
            IRequestTask value;
            this._dictionary.TryRemove(task.Id, out value);
        }

        /// <summary>
        /// 登録されている <see cref="IRequestTask"/> を全て削除します。
        /// </summary>
        /// <param name="cleanAll">終了していないタスクも含めて削除する場合は <c>true</c>。それ以外の場合は <c>false</c>。</param>
        public void Clean(Boolean cleanAll)
        {
            if (cleanAll)
            {
                this._dictionary.Clear();
            }
            else
            {
                this._dictionary.Values
                    .Where(t => t.HasExited)
                    .ForEach(this.Clean);
            }
        }

        /// <summary>
        /// 終了した <see cref="IRequestTask"/> を全て削除します。
        /// </summary>
        public void Clean()
        {
            this.Clean(false);
        }

        internal Int32 GetNewId()
        {
            if (this.Count == this.MaxRequestId)
            {
                this.Clean(false);
            }
            return 1.UpTo(this.MaxRequestId).Except(this._dictionary.Keys).First();
        }
    }
}