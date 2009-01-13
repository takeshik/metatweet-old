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
using System.Collections.Generic;
using System.Linq;

namespace XSpect.MetaTweet.ObjectModel
{
    /// <summary>
    /// Represents user account.
    /// </summary>
    /// <remarks>
    /// User account structure is composed of account ID and realm. Account ID is unique GUID value
    /// in the Storage database. Realm is a string which specifies account's belonging service.
    /// Generally, realm is named by reversed FQDN, for instance, "com.example.service". Each user
    /// account have collection of <see cref="Activity"/> and <see cref="FollowElement"/>.
    /// </remarks>
    [Serializable()]
    public class Account
        : StorageObject<StorageDataSet.AccountsDataTable, StorageDataSet.AccountsRow>,
          IComparable<Account>
    {
        /// <summary>
        /// Gets or sets the ID of the <see cref="Account"/>.
        /// </summary>
        public Guid AccountId
        {
            get
            {
                return this.UnderlyingDataRow.AccountId;
            }
            set
            {
                this.UnderlyingDataRow.AccountId = value;
            }
        }

        /// <summary>
        /// Gets or sets the realm of the <see cref="Account"/>.
        /// </summary>
        public String Realm
        {
            get
            {
                return this.UnderlyingDataRow.Realm;
            }
            set
            {
                this.UnderlyingDataRow.Realm = value;
            }
        }

        /// <summary>
        /// Gets the latest value which is categorized with specified name from activities of
        /// the <see cref="Account"/>.
        /// </summary>
        /// <param name="category">Category name.</param>
        /// <returns>
        /// Latest <see cref="Activity"/>'s value of the <see cref="Account"/> which is categorized
        /// as specified name.
        /// </returns>
        public String this[String category]
        {
            get
            {
                return this.GetActivityOf(category).Value;
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="FollowElement"/> which the <see cref="Account"/> is
        /// following.
        /// </summary>
        public IEnumerable<FollowElement> FollowingMap
        {
            get
            {
                return this.Storage.GetFollowElements(this.UnderlyingDataRow.GetFollowMapRowsByFK_Accounts_FollowMap());
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="Account"/> which the <see cref="Account"/> is
        /// following.
        /// </summary>
        public IEnumerable<Account> Following
        {
            get
            {
                return this.FollowingMap.Select(e => e.FollowingAccount);
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="FollowElement"/> which is following the
        /// <see cref="Account"/>.
        /// </summary>
        public IEnumerable<FollowElement> FollowersMap
        {
            get
            {
                return this.Storage.GetFollowElements(this.UnderlyingDataRow.GetFollowMapRowsByFK_AccountsFollowing_FollowMap());
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="Account"/> which is following the
        /// <see cref="Account"/>.
        /// </summary>
        public IEnumerable<Account> Followers
        {
            get
            {
                return this.FollowersMap.Select(e => e.Account);
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="Activity"/> of the <see cref="Account"/>.
        /// </summary>
        public IEnumerable<Activity> Activities
        {
            get
            {
                return this.Storage.GetActivities(this.UnderlyingDataRow.GetActivitiesRows());
            }
        }

        /// <summary>
        /// Constuctor of the <see cref="Account"/>.
        /// </summary>
        internal Account()
        {
        }

        /// <summary>
        /// Returuns formatted <see cref="String"/> for the <see cref="Account"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> containing formatted data of the <see cref="Account"/>.
        /// </returns>
        public override String ToString()
        {
            return String.Format("{0}@{1}", this.AccountId.ToString("d"), this.Realm);
        }

        /// <summary>
        /// Compares the <see cref="Account"/> with another <see cref="Account"/>.
        /// </summary>
        /// <param name="other">Comparing <see cref="Account"/>.</param>
        /// <returns>
        /// A 32-bit signed value.
        /// Negative value indicates the <see cref="Account"/> is less than <paramref name="other"/>.
        /// Zero indicates the <see cref="Account"/> is equal to <paramref name="other"/>.
        /// Positive value indeicates the <see cref="Account"/> is greater than <paramref name="other"/>.
        /// </returns>
        public Int32 CompareTo(Account other)
        {
            return this.AccountId.CompareTo(other.AccountId);
        }

        /// <summary>
        /// Adds following <see cref="Account"/> of the <see cref="Account"/>.
        /// </summary>
        /// <param name="account">Following <see cref="Account"/>.</param>
        public void AddFollowing(Account account)
        {
            FollowElement element = this.Storage.NewFollowElement();
            element.Account = this;
            element.FollowingAccount = account;
            element.Update();
        }

        /// <summary>
        /// Adds follower <see cref="Account"/> of the <see cref="Account"/>.
        /// </summary>
        /// <param name="account">Followed <see cref="Account"/>.</param>
        public void AddFollower(Account account)
        {
            FollowElement element = this.Storage.NewFollowElement();
            element.Account = account;
            element.FollowingAccount = this;
            element.Update();
        }

        /// <summary>
        /// Removes following <see cref="Account"/> of the <see cref="Account"/>.
        /// </summary>
        /// <param name="account">Following <see cref="Account"/>.</param>
        public void RemoveFollowing(Account account)
        {
            FollowElement element = this.FollowingMap.Where(e => e.FollowingAccount == account).Single();
            element.Delete();
            element.Update();
        }

        /// <summary>
        /// Removes follower <see cref="Account"/> of the <see cref="Account"/>.
        /// </summary>
        /// <param name="account">Followed <see cref="Account"/>.</param>
        public void RemoveFollower(Account account)
        {
            FollowElement element = this.FollowersMap.Where(e => e.Account == account).Single();
            element.Delete();
            element.Update();
        }

        /// <summary>
        /// Adds new <see cref="Activity"/> of the <see cref="Account"/>.
        /// </summary>
        /// <returns>New <see cref="Activity"/>.</returns>
        public Activity NewActivity()
        {
            Activity activity = this.Storage.NewActivity();
            activity.Account = this;
            return activity;
        }

        /// <summary>
        /// Gets the latest <see cref="Activity"/> which is categorized with specified name from
        /// activities of the <see cref="Account"/>.
        /// </summary>
        /// <param name="category">Category name.</param>
        /// <returns>
        /// Latest <see cref="Activity"/> of the <see cref="Account"/> which is categorized as
        /// specified name.
        /// </returns>
        public Activity GetActivityOf(String category)
        {
            return this.Activities.Where(a => a.Category == category).OrderByDescending(a => a.Timestamp).First();
        }
    }
}