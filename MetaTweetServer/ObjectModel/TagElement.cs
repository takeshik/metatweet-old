// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@xspect.org>
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
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace XSpect.MetaTweet.ObjectModel
{
    [Serializable()]
    public class TagElement
        : StorageObject<StorageDataSet.TagMapDataTable, StorageDataSet.TagMapRow>
    {
        private Activity _activity;

        private String _tag;

        public Activity Activity
        {
            get
            {
                return this._activity ?? (this._activity = this.Storage.GetActivities(
                    this.UnderlyingDataRow.AccountId,
                    this.UnderlyingDataRow.Timestamp,
                    this.UnderlyingDataRow.Category
                ).Single());
            }
            set
            {
                this.UnderlyingDataRow.AccountId = value.Account.AccountId;
                this.UnderlyingDataRow.Timestamp = value.Timestamp;
                this.UnderlyingDataRow.Category = value.Category;
                this._activity = value;
            }
        }

        public String Tag
        {
            get
            {
                return this._tag ?? (this._tag = this.UnderlyingDataRow.Tag);
            }
            set
            {
                this.UnderlyingDataRow.Tag = value;
                this._tag = value;
            }
        }

        protected override void UpdateImpl()
        {
            this.Storage.Update(this.UnderlyingDataRow);
        }
    }
}