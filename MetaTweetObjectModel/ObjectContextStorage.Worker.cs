// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetObjectModel
 *   Object model and Storage interface for MetaTweet and other systems
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
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

namespace XSpect.MetaTweet.Objects
{
    partial class ObjectContextStorage
    {
        /// <summary>
        /// ストレージ上で実際にエンティティ オブジェクトの管理を行う、ワーカー オブジェクトを表します。
        /// </summary>
        /// <remarks>
        /// <para>ワーカー オブジェクトは、<see cref="ObjectContextStorage"/> においてスレッド ローカルかつ限定的なスコープによって提供され、<see cref="System.Data.Objects.ObjectContext"/> への間接的なアクセスおよび、作成され、まだデータ ソースに反映されていないオブジェクトの管理機能を提供します。</para>
        /// <para>ワーカーは、<see cref="ObjectContextStorage.Execute"/> メソッド、もしくは <see cref="ObjectContextStorage.BeginWorkerScope()"/> メソッドおよび <see cref="ObjectContextStorage.EndWorkerScope()"/> メソッドの組によって作成することができ、<see cref="ObjectContextStorage.CurrentWorker"/> からアクセスできます。</para>
        /// </remarks>
        public class Worker
            : Object,
              IDisposable
        {
            /// <summary>
            /// ストレージ オブジェクトを管理するためのオブジェクト コンテキストを取得します。
            /// </summary>
            /// <value>
            /// ストレージ オブジェクトを管理するためのオブジェクト コンテキスト。
            /// </value>
            public StorageObjectContext Entities
            {
                get;
                private set;
            }

            /// <summary>
            /// 作成され、まだデータ ソースに反映されていないオブジェクトのコレクションを取得します。
            /// </summary>
            /// <value>
            /// 作成され、まだデータ ソースに反映されていないオブジェクトのコレクション。
            /// </value>
            public AddingObjectPool AddingObjects
            {
                get;
                private set;
            }

            /// <summary>
            /// オブジェクトが破棄されたかどうかを表す値を取得します。
            /// </summary>
            /// <value>
            /// オブジェクトが破棄された場合は <c>true</c>。それ以外の場合は <c>false</c>。
            /// </value>
            public Boolean IsDisposed
            {
                get
                {
                    return this.Entities != null && !this.Entities.IsDisposed;
                }
            }

            /// <summary>
            /// <see cref="Worker"/> クラスの新しいインスタンスを初期化します。
            /// </summary>
            /// <param name="entities">ワーカーに関連付けるオブジェクト コンテキスト。</param>
            public Worker(StorageObjectContext entities)
            {
                this.AddingObjects = new AddingObjectPool();
                this.Entities = entities;
            }

            /// <summary>
            /// <see cref="Worker"/> によって使用されているアンマネージ リソースを解放します。
            /// </summary>
            public void Dispose()
            {
                this.Entities.Dispose();
            }
        }
    }
}