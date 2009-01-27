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
using System.Data;
using System.Linq;

namespace XSpect.MetaTweet.ObjectModel
{
    /// <summary>
    /// MetaTweet オブジェクトモデルの抽象基本クラスを提供します。
    /// </summary>
    [Serializable()]
    public abstract class StorageObject
        : Object
    {
        private Storage _storage;

        /// <summary>
        /// このオブジェクトが探索および操作に使用するストレージを取得または設定します。
        /// </summary>
        /// <value>
        /// このオブジェクトが探索および操作に使用するストレージ。
        /// </value>
        public Storage Storage
        {
            get
            {
                return this._storage;
            }
            set
            {
                this._storage = value;
            }
        }

        /// <summary>
        /// 派生クラスで実装された場合、このオブジェクトのデータのバックエンドとなるデータ行を取得または設定します。
        /// </summary>
        /// <value>
        /// このオブジェクトのデータのバックエンドとなるデータ行。
        /// </value>
        public abstract DataRow UnderlyingUntypedDataRow
        {
            get;
            set;
        }

        /// <summary>
        /// このオブジェクトが変更されているかどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// このオブジェクトが変更されている場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </value>
        public virtual Boolean IsModified
        {
            get
            {
                return this.UnderlyingUntypedDataRow.RowState == DataRowState.Modified;
            }
        }

        /// <summary>
        /// 指定した <see cref="T:System.Object"/> が、現在の <see cref="T:System.Object"/> と等しいかどうかを判断します。
        /// </summary>
        /// <param name="obj">現在の <see cref="T:System.Object"/> と比較する <see cref="T:System.Object"/>。</param>
        /// <returns>
        /// 指定した <see cref="T:System.Object"/> が現在の <see cref="T:System.Object"/> と等しい場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// <paramref name="obj"/> パラメータが <c>null</c> です。
        /// </exception>
        public override Boolean Equals(Object obj)
        {
            return this.UnderlyingUntypedDataRow == (obj as StorageObject).UnderlyingUntypedDataRow;
        }

        /// <summary>
        /// 特定の型のハッシュ関数として機能します。
        /// </summary>
        /// <returns>
        /// このインスタンスのハッシュ コード。
        /// </returns>
        public override Int32 GetHashCode()
        {
            return this.UnderlyingUntypedDataRow.GetHashCode();
        }

        /// <summary>
        /// このオブジェクトの参照するデータ行を新たなデータ表に所属させます。
        /// </summary>
        protected virtual void Store()
        {
            this.UnderlyingUntypedDataRow.Table.Rows.Add(this.UnderlyingUntypedDataRow);
        }

        /// <summary>
        /// このオブジェクトの参照するデータ行を削除するようマークします。
        /// </summary>
        public virtual void Delete()
        {
            this.UnderlyingUntypedDataRow.Delete();
        }

        /// <summary>
        /// このオブジェクトに対する変更を差し戻します。
        /// </summary>
        public virtual void Revert()
        {
            this.UnderlyingUntypedDataRow.RejectChanges();
        }
    }

    /// <summary>
    /// 厳密に型付けされた MetaTweet オブジェクトモデルの抽象基本クラスを提供します。
    /// </summary>
    /// <typeparam name="TTable">このオブジェクトのバックエンドのデータ行を含むデータ表の型。</typeparam>
    /// <typeparam name="TRow">このオブジェクトのバックエンドのデータ行の型。</typeparam>
    [Serializable()]
    public abstract class StorageObject<TTable, TRow>
        : StorageObject
        where TTable
            : TypedTableBase<TRow>,
              new()
        where TRow
            : DataRow
    {
        private TRow _underlyingDataRow;

        /// <summary>
        /// このオブジェクトのデータのバックエンドとなる、厳密に型付けされていないデータ行を取得または設定します。
        /// </summary>
        /// <value>
        /// このオブジェクトのデータのバックエンドとなる、厳密に型付けされていないデータ行。
        /// </value>
        public override DataRow UnderlyingUntypedDataRow
        {
            get
            {
                return this._underlyingDataRow;
            }
            set
            {
                this._underlyingDataRow = (TRow) value;
            }
        }
        
        /// <summary>
        /// このオブジェクトのデータのバックエンドとなるデータ行を取得または設定します。
        /// </summary>
        /// <value>
        /// このオブジェクトのデータのバックエンドとなるデータ行。
        /// </value>
        public TRow UnderlyingDataRow
        {
            get
            {
                if (this._underlyingDataRow == null)
                {
                    this._underlyingDataRow = (TRow) this.Storage.UnderlyingDataSet.Tables
                        .OfType<TTable>()
                        .Single()
                        .NewRow();
                }
                return this._underlyingDataRow;
            }
            set
            {
                // Suppress re-setting.
                if (this._underlyingDataRow != null)
                {
                    // TODO: Exception string resource
                    throw new InvalidOperationException();
                }
                this._underlyingDataRow = value;
            }
        }

        /// <summary>
        /// このオブジェクトが変更されているかどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// このオブジェクトが変更されている場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </value>
        public override Boolean IsModified
        {
            get
            {
                return this.UnderlyingDataRow.RowState == DataRowState.Modified;
            }
        }

        /// <summary>
        /// 指定した <see cref="T:System.Object"/> が、現在の <see cref="T:System.Object"/> と等しいかどうかを判断します。
        /// </summary>
        /// <param name="obj">現在の <see cref="T:System.Object"/> と比較する <see cref="T:System.Object"/>。</param>
        /// <returns>
        /// 指定した <see cref="T:System.Object"/> が現在の <see cref="T:System.Object"/> と等しい場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// <paramref name="obj"/> パラメータが <c>null</c> です。
        /// </exception>
        public override Boolean Equals(Object obj)
        {
            return this.UnderlyingDataRow == (obj as StorageObject<TTable, TRow>).UnderlyingDataRow;
        }

        /// <summary>
        /// 特定の型のハッシュ関数として機能します。
        /// </summary>
        /// <returns>
        /// このインスタンスのハッシュ コード。
        /// </returns>
        public override Int32 GetHashCode()
        {
            return this.UnderlyingDataRow.GetHashCode();
        }

        /// <summary>
        /// このオブジェクトの参照するデータ行を新たなデータ表に所属させます。
        /// </summary>
        protected override void Store()
        {
            this.UnderlyingDataRow.Table.Rows.Add(this.UnderlyingDataRow);
        }

        /// <summary>
        /// このオブジェクトの参照するデータ行を削除するようマークします。
        /// </summary>
        public override void Delete()
        {
            this.UnderlyingDataRow.Delete();
        }

        /// <summary>
        /// このオブジェクトに対する変更を差し戻します。
        /// </summary>
        public override void Revert()
        {
            this.UnderlyingDataRow.RejectChanges();
        }
    }
}