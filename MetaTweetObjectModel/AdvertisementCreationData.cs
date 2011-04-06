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
    [Serializable()]
    public class AdvertisementCreationData
        : StorageObjectCreationData
    {
        private readonly Lazy<AdvertisementId> _id;

        private Activity _activity;

        private ActivityId _activityId;

        public AdvertisementId Id
        {
            get
            {
                if (this.ActivityId == default(ActivityId)
                    || this.Timestamp == default(DateTime)
                    || this.Flags == default(AdvertisementFlags)
                )
                {
                    throw new InvalidOperationException();
                }
                return this._id.Value;
            }
        }

            public Activity Activity
        {
            get
            {
                return this._activity;
            }
            set
            {
                this._activity = value;
                this._activityId = value.Id;
            }
        }

        public ActivityId ActivityId
        {
            get
            {
                return this._activityId;
            }
            set
            {
                this._activity = null;
                this._activityId = value;
            }
        }

        public DateTime Timestamp
        {
            get;
            set;
        }

        public AdvertisementFlags Flags
        {
            get;
            set;
        }

        public AdvertisementCreationData()
        {
            this._id = new Lazy<AdvertisementId>(() => AdvertisementId.Create(this.ActivityId, this.Timestamp, this.Flags));
        }

        public Activity GetActivity(StorageSession context)
        {
            return this._activity != null && this._activity.Context == context
                ? this._activity
                : this._activity = context.Load(this._activityId);
        }
    }
}