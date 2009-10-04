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

namespace XSpect.MetaTweet.Objects
{
    public interface IActivity
        : IComparable<IActivity>
    {
        Guid AccountId
        {
            get;
            set;
        }

        DateTime Timestamp
        {
            get;
            set;
        }

        String Category
        {
            get;
            set;
        }

        String SubId
        {
            get;
            set;
        }

        String UserAgent
        {
            get;
            set;
        }

        String Value
        {
            get;
            set;
        }

        Byte[] Data
        {
            get;
            set;
        }

        Account Account
        {
            get;
            set;
        }

        IEnumerable<Tag> Tags
        {
            get;
        }

        IEnumerable<String> Tagging
        {
            get;
        }

        IEnumerable<Reference> References
        {
            get;
        }

        IEnumerable<Reference> ReverseReferences
        {
            get;
        }

        IEnumerable<KeyValuePair<String, Activity>> Referring
        {
            get;
        }

        IEnumerable<KeyValuePair<String, Activity>> Referrers
        {
            get;
        }

        IEnumerable<Mark> Marks
        {
            get;
        }

        IEnumerable<KeyValuePair<String, Account>> Markers
        {
            get;
        }

        Boolean IsTagging(String name);

        IEnumerable<Activity> ReferringOf(String name);

        IEnumerable<Activity> ReferrersOf(String name);

        IEnumerable<Account> MarkersOf(String name);

        Tag Tag(String name);

        Reference Refer(String name, Activity referTo);

        Reference Referred(String name, Activity referredFrom);

        Mark Marked(String name, Account markedFrom);
    }
}