// -*- mode: csharp; encoding: utf-8; -*-
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
using System.Data;
using System.Linq;
using System.Runtime.Serialization;

namespace XSpect.MetaTweet.ObjectModel
{
    /// <summary>
    /// MetaTweet オブジェクト モデルの抽象基本クラスを提供します。
    /// </summary>
    [Serializable()]
    public abstract class StorageObject
        : MarshalByRefObject,
          ISupportInitialize,
          IComparable<StorageObject>,
          IEquatable<StorageObject>
    {
        [NonSerialized()]
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
                if (this.IsConnected)
                {
                    throw new InvalidOperationException("This object is already connecting to storage.");
                }
                this._storage = value;
            }
        }

        /// <summary>
        /// 派生クラスで実装された場合、このオブジェクトのデータのバックエンドとなる行を取得または設定します。
        /// </summary>
        /// <value>
        /// このオブジェクトのデータのバックエンドとなる行。
        /// </value>
        public abstract DataRow UnderlyingUntypedDataRow
        {
            get;
            set;
        }

        /// <summary>
        /// 派生クラスで実装された場合、このオブジェクトがストレージに接続しているかどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// このオブジェクトがストレージに接続している場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </value>
        public abstract Boolean IsConnected
        {
            get;
        }

        /// <summary>
        /// 派生クラスで実装された場合、このオブジェクトが変更されているかどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// このオブジェクトが変更されている場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </value>
        public abstract Boolean IsModified
        {
            get;
        }

        /// <summary>
        /// 派生クラスで実装された場合、このオブジェクトのデータのバックエンドとなる行の主キーのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// このオブジェクトのデータのバックエンドとなる行の主キーのシーケンス。
        /// </value>
        public abstract IList<Object> PrimaryKeyList
        {
            get;
        }
        
        /// <summary>
        /// 派生クラスで実装された場合、データセット内に存在する、このオブジェクトの親オブジェクトのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このオブジェクトの親オブジェクトのシーケンス。
        /// </value>
        public abstract IEnumerable<StorageObject> Parents
        {
            get;
        }

        /// <summary>
        /// 派生クラスで実装された場合、データセット内に存在する、このオブジェクトの子オブジェクトのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このオブジェクトの子オブジェクトのシーケンス。
        /// </value>
        public abstract IEnumerable<StorageObject> Children
        {
            get;
        }

        /// <summary>
        /// データセット内に存在する、このオブジェクトの祖先オブジェクトのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このオブジェクトの祖先オブジェクトのシーケンス。
        /// </value>
        public IEnumerable<StorageObject> Ancestors
        {
            get
            {
                return this.Cascade(obj => obj.Parents);
            }
        }

        /// <summary>
        /// データセット内に存在する、このオブジェクトの子孫オブジェクトのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このオブジェクトの子孫オブジェクトのシーケンス。
        /// </value>
        public IEnumerable<StorageObject> Descendants
        {
            get
            {
                return this.Cascade(obj => obj.Children);
            }
        }

        /// <summary>
        /// データセット内に存在する、このオブジェクトに関連する親族オブジェクトのシーケンスを取得します。
        /// </summary>
        /// <value>
        /// データセット内に存在する、このオブジェクトの関連する親族オブジェクトのシーケンス。
        /// </value>
        public IEnumerable<StorageObject> Relatives
        {
            get
            {
                return this.Cascade(obj => obj.Parents.Union(obj.Children));
            }
        }

        /// <summary>
        /// 2 つのオブジェクトが等しいかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較するオブジェクト。</param>
        /// <param name="right">比較されるオブジェクト。</param>
        /// <returns><paramref name="left"/> と <paramref name="right"/> が等しい場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator ==(StorageObject left, StorageObject right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }
            else if ((Object) left == null || (Object) right == null)
            {
                return false;
            }
            else
            {
                return left.Equals(right);
            }
        }

        /// <summary>
        /// 2 つのオブジェクトが等しくないかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較するオブジェクト。</param>
        /// <param name="right">比較されるオブジェクト。</param>
        /// <returns><paramref name="left"/> と <paramref name="right"/> が等しくない場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator !=(StorageObject left, StorageObject right)
        {
            return !(left == right);
        }

        /// <summary>
        /// 一方のオブジェクトが、他方のオブジェクトより前に位置するかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較するオブジェクト。</param>
        /// <param name="right">比較されるオブジェクト。</param>
        /// <returns><paramref name="left"/> が <paramref name="right"/> より前に位置する場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator <(StorageObject left, StorageObject right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// 一方のオブジェクトが、他方のオブジェクトより後ろに位置するかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較するオブジェクト。</param>
        /// <param name="right">比較されるオブジェクト。</param>
        /// <returns><paramref name="left"/> が <paramref name="right"/> より後ろに位置する場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator >(StorageObject left, StorageObject right)
        {
            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// 一方のオブジェクトが、他方のオブジェクトと等しいか、または前に位置するかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較するオブジェクト。</param>
        /// <param name="right">比較されるオブジェクト。</param>
        /// <returns><paramref name="left"/> が <paramref name="right"/> と等しい、または前に位置する場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator <=(StorageObject left, StorageObject right)
        {
            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// 一方のオブジェクトが、他方のオブジェクトと等しいか、または後ろに位置するかどうかを示す値を返します。
        /// </summary>
        /// <param name="left">比較するオブジェクト。</param>
        /// <param name="right">比較されるオブジェクト。</param>
        /// <returns><paramref name="left"/> が <paramref name="right"/> と等しい、または後ろに位置する場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public static Boolean operator >=(StorageObject left, StorageObject right)
        {
            return left.CompareTo(right) >= 0;
        }

        /// <summary>
        /// 指定した <see cref="T:System.Object"/> が、現在の <see cref="T:System.Object"/> と等しいかどうかを判断します。
        /// </summary>
        /// <param name="obj">現在の <see cref="T:System.Object"/> と比較する <see cref="T:System.Object"/>。</param>
        /// <returns>
        /// 指定した <see cref="T:System.Object"/> が現在の <see cref="T:System.Object"/> と等しい場合は <c>true</c>。それ以外の場合は <c>false。</c>
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// <paramref name="obj"/> パラメータが <c>null</c> です。
        /// </exception>
        public override Boolean Equals(Object obj)
        {
            return obj is StorageObject && this.Equals(obj as StorageObject);
        }

        /// <summary>
        /// 特定の型のハッシュ関数として機能します。
        /// </summary>
        /// <returns>
        /// 現在の <see cref="T:System.Object"/> のハッシュ コード。
        /// </returns>
        public override Int32 GetHashCode()
        {
            return this.PrimaryKeyList.GetHashCode();
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
        /// 派生クラスで実装された場合、現在のオブジェクトを同じ型の別のオブジェクトと比較します。
        /// </summary>
        /// <param name="other">このオブジェクトと比較するオブジェクト。</param>
        /// <returns>
        /// 比較対象オブジェクトの相対順序を示す 32 ビット符号付き整数。戻り値の意味は次のとおりです。
        /// 値
        /// 意味
        /// 0 より小さい値
        /// このオブジェクトが <paramref name="other"/> パラメータより小さいことを意味します。
        /// 0
        /// このオブジェクトが <paramref name="other"/> と等しいことを意味します。
        /// 0 より大きい値
        /// このオブジェクトが <paramref name="other"/> よりも大きいことを意味します。
        /// </returns>
        public abstract Int32 CompareTo(StorageObject other);

        /// <summary>
        /// 現在のオブジェクトが、同じ型の別のオブジェクトと等しいかどうかを示します。
        /// </summary>
        /// <param name="other">このオブジェクトと比較するオブジェクト。</param>
        /// <returns>
        /// 現在のオブジェクトが <paramref name="other"/> パラメータと等しい場合は <c>true</c>。それ以外の場合は <c>false</c>。
        /// </returns>
        public Boolean Equals(StorageObject other)
        {
            return this.CompareTo(other) == 0;
        }

        /// <summary>
        /// このオブジェクトと、指定した別のオブジェクトが同一のデータソースを参照し、かつ、同一の値を持つかどうかを判断します。
        /// </summary>
        /// <param name="other">このオブジェクトと比較するオブジェクト。</param>
        /// <returns><paramref name="other"/> パラメータの主キーの値がこのオブジェクトと同じで、なおかつ <see cref="Storage"/> も同じ場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public Boolean ExactlyEquals(StorageObject other)
        {
            return this.Storage == other.Storage && this.Equals(other);
        }

        /// <summary>
        /// 派生クラスで実装された場合、このオブジェクトをストレージに接続します。
        /// </summary>
        public abstract void Connect();

        /// <summary>
        /// 派生クラスで実装された場合、このオブジェクトをストレージから切断します。
        /// </summary>
        public abstract void Disconnect();

        /// <summary>
        /// 派生クラスで実装された場合、このオブジェクトの参照するデータ行を削除するようマークします。
        /// </summary>
        public abstract void Delete();

        /// <summary>
        /// 派生クラスで実装された場合、このオブジェクトに対する変更を差し戻します。
        /// </summary>
        public abstract void Revert();

        /// <summary>
        /// 派生クラスで実装された場合、初期化の開始を通知するシグナルをオブジェクトに送信します。
        /// </summary>
        public abstract void BeginInit();

        /// <summary>
        /// 派生クラスで実装された場合、初期化の完了を通知するシグナルをオブジェクトに送信します。
        /// </summary>
        public abstract void EndInit();

        /// <summary>
        /// 派生クラスで実装された場合、このオブジェクトの親オブジェクトのシーケンスを取得します。
        /// </summary>
        /// <returns>
        /// このオブジェクトの親オブジェクトのシーケンス。
        /// </returns>
        public abstract IEnumerable<StorageObject> GetParents();

        /// <summary>
        /// 派生クラスで実装された場合、このオブジェクトの子オブジェクトのシーケンスを取得します。
        /// </summary>
        /// <returns>
        /// このオブジェクトの子オブジェクトのシーケンス。
        /// </returns>
        public abstract IEnumerable<StorageObject> GetChildren();

        /// <summary>
        /// このオブジェクトの祖先オブジェクトのシーケンスを取得します。
        /// </summary>
        /// <returns>
        /// このオブジェクトの祖先オブジェクトのシーケンス。
        /// </returns>
        public IEnumerable<StorageObject> GetAncestors()
        {
            return this.Cascade(obj => obj.GetParents());
        }

        /// <summary>
        /// このオブジェクトの子孫オブジェクトのシーケンスを取得します。
        /// </summary>
        /// <returns>
        /// このオブジェクトの子孫オブジェクトのシーケンス。
        /// </returns>
        public IEnumerable<StorageObject> GetDescendants()
        {
            return this.Cascade(obj => obj.GetChildren());
        }

        /// <summary>
        /// このオブジェクトに関連する親族オブジェクトのシーケンスを取得します。
        /// </summary>
        /// <returns>
        /// このオブジェクトに関連する親族オブジェクトのシーケンス。
        /// </returns>
        public IEnumerable<StorageObject> GetRelatives()
        {
            return this.Cascade(obj => obj.GetParents().Union(obj.GetChildren()));
        }

        private IEnumerable<StorageObject> Cascade(Func<StorageObject, IEnumerable<StorageObject>> selector)
        {
            IEnumerable<StorageObject> result = Enumerable.Empty<StorageObject>();
            for (
                IEnumerable<StorageObject> objects = selector(this);
                objects.Any();
                objects = objects.SelectMany(selector)
            )
            {
                result = result.Concat(objects.ToArray());
            }
            return result;
        }
    }

    /// <summary>
    /// 厳密に型付けされた MetaTweet オブジェクト モデルの抽象基本クラスを提供します。
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
        [NonSerialized()]
        private TRow _underlyingDataRow;

        [NonSerialized()]
        private Boolean _isConnected;

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
        /// このオブジェクトのデータのバックエンドとなる行を取得または設定します。
        /// </summary>
        /// <value>
        /// このオブジェクトのデータのバックエンドとなる行。
        /// </value>
        public TRow UnderlyingDataRow
        {
            get
            {
                if (this._underlyingDataRow == null)
                {
                    TTable table = this.Storage.UnderlyingDataSet.Tables
                        .OfType<TTable>()
                        .Single();
                    this._underlyingDataRow = table.SingleOrDefault(
                        r => this.PrimaryKeyList.SequenceEqual(r.ItemArray.Take(this.PrimaryKeyList.Count))
                    ) ?? table.NewRow() as TRow;
                }
                return this._underlyingDataRow;
            }
            set
            {
                if (this.IsConnected)
                {
                    throw new InvalidOperationException("This object is already connecting to storage.");
                }
                this._underlyingDataRow = value;
            }
        }

        /// <summary>
        /// このオブジェクトがストレージに接続しているかどうかを示す値を取得します。
        /// </summary>
        /// <value>このオブジェクトがストレージに接続している場合は <c>true</c>。それ以外の場合は <c>false</c>。</value>
        public override Boolean IsConnected
        {
            get
            {
                return this._isConnected;
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
            return obj is StorageObject<TTable,TRow>
                && this.UnderlyingDataRow == (obj as StorageObject<TTable, TRow>).UnderlyingDataRow;
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
        /// このオブジェクトをストレージに接続します。
        /// </summary>
        public override void Connect()
        {
            this.Synchronize();
            if (this.UnderlyingDataRow.RowState == DataRowState.Detached)
            {
                this.UnderlyingDataRow.Table.Rows.Add(this.UnderlyingDataRow);
            }
            this._isConnected = true;
        }

        /// <summary>
        /// このオブジェクトをストレージから切断します。
        /// </summary>
        public override void Disconnect()
        {
            this.Synchronize();
            this._isConnected = false;
        }

        /// <summary>
        /// 派生クラスで実装された場合、このオブジェクトが現在参照している列の内容で、このオブジェクトが他に参照する可能性のある列の内容を上書きします。
        /// </summary>
        protected abstract void Synchronize();

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
            if (this.IsConnected)
            {
                this.UnderlyingDataRow.RejectChanges();
            }
            else
            {
                // Reset all elements' modified-flags to false.
                this.BeginInit();
                this.EndInit();
            }
        }

        [OnSerializing()]
        private void OnSerializing(StreamingContext context)
        {
            if (this.IsConnected)
            {
                this.Synchronize();
            }
        }

        [OnDeserialized()]
        private void OnDeserialized(StreamingContext context)
        {
            // HACK: Call get_UnderlyingDataRow().
            #pragma warning disable 168
            TRow dummy = this.UnderlyingDataRow;
            #pragma warning restore 168
        }
    }
}