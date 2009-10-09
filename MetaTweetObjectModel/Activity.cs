// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
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
using System.Collections.Generic;
using System.Linq;

namespace XSpect.MetaTweet.Objects
{
    partial class Activity
        : IActivity
    {
        public IEnumerable<String> Tagging
        {
            get
            {
                return this.Tags.Select(t => t.Name);
            }
        }

        public IEnumerable<KeyValuePair<String, Activity>> Referring
        {
            get
            {
                return this.References.Select(r => new KeyValuePair<String, Activity>(r.Name, r.ReferringActivity));
            }
        }

        public IEnumerable<KeyValuePair<String, Activity>> Referrers
        {
            get
            {
                return this.ReverseReferences.Select(r => new KeyValuePair<String, Activity>(r.Name, r.Activity));
            }
        }

        public IEnumerable<KeyValuePair<String, Account>> Markers
        {
            get
            {
                return this.Marks.Select(m => new KeyValuePair<String, Account>(m.Name, m.Account));
            }
        }

        internal Activity(Storage storage)
            : base(storage)
        {
        }

        public override String ToString()
        {
            return String.Format(
                "Acc [{0}] @ {1}: {2}({3}) = \"{4}\"{5}{6}",
                this.Account,
                this.Timestamp.ToString("s"),
                this.Category,
                this.SubId,
                this.Value,
                this.Data != null ? "+" : String.Empty,
                String.IsNullOrEmpty(this.UserAgent)
                    ? " (" + this.UserAgent + ")"
                    : String.Empty
            );
        }

        public override Int32 CompareTo(StorageObject other)
        {
            if (!(other is Activity))
            {
                throw new ArgumentException("other");
            }
            return this.CompareTo(other as Activity);
        }

        public Int32 CompareTo(IActivity other)
        {
            // Timestamp -> Category -> SubId(numeric || text) -> Account
            Int32 result;
            Int64 x;
            Int64 y;
            return (result = this.Timestamp.CompareTo(other.Timestamp)) != 0
                ? result
                : (result = this.Category.CompareTo(other.Category)) != 0
                      ? result
                      : (result = (Int64.TryParse(this.SubId, out x) && Int64.TryParse(other.SubId, out y))
                            ? x.CompareTo(y)
                            : this.SubId.CompareTo(other.SubId)
                        ) != 0
                            ? result
                            : this.Account.CompareTo(other.Account);
        }

        public IEnumerable<Activity> ReferringOf(String name)
        {
            return this.Referring.Where(p => p.Key == name).Select(p => p.Value);
        }

        public IEnumerable<Activity> ReferrersOf(String name)
        {
            return this.Referrers.Where(p => p.Key == name).Select(p => p.Value);
        }

        public IEnumerable<Account> MarkersOf(String name)
        {
            return this.Markers.Where(p => p.Key == name).Select(p => p.Value);
        }

        public Boolean IsTagging(String name)
        {
            return this.Tagging.Contains(name);
        }

        public Boolean IsReferringOf(String name, Activity activity)
        {
            return this.ReferringOf(name).Contains(activity);
        }

        public Boolean IsReferredOf(String name, Activity activity)
        {
            return this.ReferrersOf(name).Contains(activity);
        }

        public Boolean IsMarkedOf(String name, Account account)
        {
            return this.MarkersOf(name).Contains(account);
        }

        public Tag Tag(String name)
        {
            return this.Storage.NewTag(this, name);
        }

        public Reference Refer(String name, Activity referTo)
        {
            return this.Storage.NewReference(this, name, referTo);
        }

        public Reference Referred(String name, Activity referredFrom)
        {
            return this.Storage.NewReference(referredFrom, name, this);
        }

        public Mark Marked(String name, Account markedFrom)
        {
            return this.Storage.NewMark(markedFrom, name, this);
        }

        #region Implicit Implementations

        IEnumerable<Tag> IActivity.Tags
        {
            get
            {
                return this.Tags;
            }
        }

        IEnumerable<Reference> IActivity.References
        {
            get
            {
                return this.References;
            }
        }

        IEnumerable<Reference> IActivity.ReverseReferences
        {
            get
            {
                return this.ReverseReferences;
            }
        }

        IEnumerable<Mark> IActivity.Marks
        {
            get
            {
                return this.Marks;
            }
        }

        #endregion
    }
}
