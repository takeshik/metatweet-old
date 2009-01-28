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

namespace XSpect.MetaTweet.ObjectModel
{
    /// <summary>
    /// アカウントと、アカウントがお気に入りとしてマークしているアクティビティとの関係を表します。
    /// </summary>
    /// <remarks>
    /// このクラスはアカウントとアクティビティとの関係表の単一の行要素を表現し、
    /// その集合により多対多の関係を構成します。
    /// </remarks>
    [Serializable()]
    public class FavorElement
        : StorageObject<StorageDataSet.FavorMapDataTable, StorageDataSet.FavorMapRow>
    {
        /// <summary>
        /// お気に入りとしてマークしている主体であるアカウントを取得または設定します。
        /// </summary>
        /// <value>
        /// お気に入りとしてマークしている主体であるアカウント。
        /// </value>
        public Account Account
        {
            get
            {
                return this.Storage.GetAccount(this.UnderlyingDataRow.AccountsRow);
            }
            set
            {
                this.UnderlyingDataRow.AccountsRow = value.UnderlyingDataRow;
            }
        }

        /// <summary>
        /// アカウントがお気に入りとしてマークしているアクティビティを取得または設定します。
        /// </summary>
        /// <value>
        /// アカウントがお気に入りとしてマークしているアクティビティ。
        /// </value>
        public Activity FavoringActivity
        {
            get
            {
                return this.Storage.GetActivity(this.UnderlyingDataRow.ActivitiesRowParent);
            }
            set
            {
                this.UnderlyingDataRow.ActivitiesRowParent = value.UnderlyingDataRow;
            }
        }

        /// <summary>
        /// <see cref="FavorElement"/> の新しいインスタンスを初期化します。
        /// </summary>
        public FavorElement()
        {
        }

        /// <summary>
        /// <see cref="FavorElement"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="row">関係が参照するデータ列。</param>
        public FavorElement(StorageDataSet.FavorMapRow row)
        {
            this.UnderlyingDataRow = row;
        }

        /// <summary>
        /// この関係を表す <see cref="T:System.String"/> を返します。
        /// </summary>
        /// <returns>この関係を表す <see cref="T:System.String"/>。</returns>
        public override String ToString()
        {
            return String.Format("{0}: {1}", this.Account.ToString(), this.FavoringActivity.ToString());
        }
    }
}