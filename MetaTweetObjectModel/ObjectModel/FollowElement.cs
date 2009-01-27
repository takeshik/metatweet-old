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
    /// アカウントと、アカウントがフォローしているアカウントとの関係を表します。
    /// </summary>
    /// <remarks>
    /// このクラスは一方のアカウントと他方のアカウントとの関係表の単一の行要素を表現し、
    /// その集合により多対多の関係を構成します。
    /// </remarks>
    [Serializable()]
    public class FollowElement
        : StorageObject<StorageDataSet.FollowMapDataTable, StorageDataSet.FollowMapRow>
    {
        /// <summary>
        /// フォローしている主体であるアカウントを取得または設定します。
        /// </summary>
        /// <value>
        /// フォローしている主体であるアカウント。
        /// </value>
        public Account Account
        {
            get
            {
                return this.Storage.GetAccount(this.UnderlyingDataRow.AccountsRow);
            }
            set
            {
                this.UnderlyingDataRow.AccountId = value.AccountId;
            }
        }

        /// <summary>
        /// アカウントがフォローしているアカウントを取得または設定します。
        /// </summary>
        /// <value>
        /// アカウントがフォローしているアカウント。
        /// </value>
        public Account FollowingAccount
        {
            get
            {
                return this.Storage.GetAccount(this.UnderlyingDataRow.AccountsRowByFK_AccountsFollowing_FollowMap);
            }
            set
            {
                this.UnderlyingDataRow.FollowingAccountId = value.AccountId;
            }
        }

        public FollowElement(
            Account accoumt,
            Account followingAccount
        )
        {
            this.Account = accoumt;
            this.FollowingAccount = followingAccount;
            this.Store();
        }

        public FollowElement(StorageDataSet.FollowMapRow row)
        {
            this.UnderlyingDataRow = row;
        }

        /// <summary>
        /// この関係を表す <see cref="T:System.String"/> を返します。
        /// </summary>
        /// <returns>
        /// この関係を表す <see cref="T:System.String"/>。
        /// </returns>
        public override String ToString()
        {
            return String.Format("{0} => {1}", this.Account.ToString(), this.FollowingAccount.ToString());
        }
    }
}