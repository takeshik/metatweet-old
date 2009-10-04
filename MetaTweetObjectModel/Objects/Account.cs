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
    partial class Account
        : IAccount
    {
        public Activity this[String category]
        {
            get
            {
                return this.Activities.CreateSourceQuery()
                    .Where(a => a.Category == category)
                    .OrderByDescending(a => a)
                    .First();
            }
        }

        public Activity this[String category, DateTime baseline]
        {
            get
            {
                return this.Activities.CreateSourceQuery()
                    .Where(a => a.Category == category)
                    .Where(a => a.Timestamp < baseline)
                    .OrderByDescending(a => a)
                    .First();
            }
        }

        public IEnumerable<String> Annotating
        {
            get
            {
                return this.Annotations.Select(a => a.Name);
            }
        }

        public IEnumerable<KeyValuePair<String, Account>> Relating
        {
            get
            {
                return this.Relations.Select(r => new KeyValuePair<String, Account>(r.Name, r.RelatingAccount));
            }
        }

        public IEnumerable<KeyValuePair<String, Account>> Relators
        {
            get
            {
                return this.ReverseRelations.Select(r => new KeyValuePair<String, Account>(r.Name, r.Account));
            }
        }

        public IEnumerable<KeyValuePair<String, Activity>> Marking
        {
            get
            {
                return this.Marks.Select(m => new KeyValuePair<String, Activity>(m.Name, m.MarkingActivity));
            }
        }

        public Account(Storage storage)
            : base(storage)
        {
        }

        public override Int32 CompareTo(StorageObject other)
        {
            if (!(other is Account))
            {
                throw new ArgumentException("other");
            }
            return this.CompareTo(other as Account);
        }

        public Int32 CompareTo(IAccount other)
        {
            // AccountId
            return this.AccountId.CompareTo(other.AccountId);
        }

        public Boolean IsAnnotating(String name)
        {
            return this.Annotating.Contains(name);
        }

        public IEnumerable<Account> RelatingOf(String name)
        {
            return this.Relating.Where(p => p.Key == name).Select(p => p.Value);
        }

        public IEnumerable<Account> RelatorsOf(String name)
        {
            return this.Relators.Where(p => p.Key == name).Select(p => p.Value);
        }

        public IEnumerable<Activity> MarkingOf(String name)
        {
            return this.Marking.Where(p => p.Key == name).Select(p => p.Value);
        }

        public Activity Act(DateTime timestamp, String category, String subId, String userAgent, String value, Byte[] data)
        {
            return this.Storage.NewActivity(this, timestamp, category, subId, userAgent, value, data);
        }

        public Activity Act(DateTime timestamp, String category, String subId)
        {
            return this.Storage.NewActivity(this, timestamp, category, subId);
        }

        public Annotation Annotate(String name)
        {
            return this.Storage.NewAnnotation(this, name);
        }

        public Relation Relate(String name, Account relateTo)
        {
            return this.Storage.NewRelation(this, name, relateTo);
        }

        public Relation Related(String name, Account relatedFrom)
        {
            return this.Storage.NewRelation(relatedFrom, name, this);
        }

        public Mark Mark(String name, Activity markTo)
        {
            return this.Storage.NewMark(this, name, markTo);
        }

        #region Implicit Implementations
        IEnumerable<Activity> IAccount.Activities
        {
            get
            {
                return this.Activities;
            }
        }

        IEnumerable<Annotation> IAccount.Annotations
        {
            get
            {
                return this.Annotations;
            }
        }

        IEnumerable<Relation> IAccount.Relations
        {
            get
            {
                return this.Relations;
            }
        }

        IEnumerable<Relation> IAccount.ReverseRelations
        {
            get
            {
                return this.ReverseRelations;
            }
        }

        IEnumerable<Mark> IAccount.Marks
        {
            get
            {
                return this.Marks;
            }
        }
        #endregion
    }
}