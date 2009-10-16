﻿// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
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
using System.Linq;

namespace XSpect.MetaTweet.Objects
{
    partial class Reference
        : IReference
    {
        private Reference()
        {
        }

        internal Reference(Storage storage)
            : base(storage)
        {
        }

        public override String ToString()
        {
            return String.Format(
                "Ref [{0}]: {1} -> [{2}]",
                this.Activity,
                this.Name,
                this.ReferringActivity
            );
        }

        public override Int32 CompareTo(StorageObject other)
        {
            if (!(other is Reference))
            {
                throw new ArgumentException("other");
            }
            return this.CompareTo(other as Reference);
        }

        public Int32 CompareTo(IReference other)
        {
            // Activity -> Name -> ReferringActivity
            Int32 result;
            return other == null
                ? 1
                : (result = this.Activity.CompareTo(other.Activity)) != 0
                      ? result
                      : (result = this.Name.CompareTo(other.Name)) != 0
                            ? result
                            : this.ReferringActivity.CompareTo(other.ReferringActivity);
        }

        // NOTE: Alternative implementation.
        public Activity ReferringActivity
        {
            get
            {
                return this.Storage.GetActivities(this.ReferringAccountId, this.ReferringTimestamp, this.ReferringCategory, this.ReferringSubId).SingleOrDefault();
            }
            set
            {
                this.ReferringAccountId = value.AccountId;
                this.ReferringTimestamp = value.Timestamp;
                this.ReferringCategory = value.Category;
                this.ReferringSubId = value.SubId;
            }
        }
    }
}