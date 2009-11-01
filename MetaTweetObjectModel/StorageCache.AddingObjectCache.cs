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
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Objects;
using System.Linq;

namespace XSpect.MetaTweet.Objects
{
    partial class StorageCache
    {
        /// <summary>
        /// 生成され、まだデータベースに格納されていないストレージ オブジェクトを管理する機能を提供します。
        /// </summary>
        [Serializable()]
        public class AddingObjectCache
            : Object,
              IEnumerable<StorageObject>
        {
            /// <summary>
            /// 親となる <see cref="StorageCache"/> を取得します。
            /// </summary>
            /// <value>
            /// 親となる <see cref="StorageCache"/>。
            /// </value>
            public StorageCache Cache
            {
                get;
                private set;
            }

            public List<Account> Accounts
            {
                get;
                private set;
            }

            public List<Activity> Activities
            {
                get;
                private set;
            }

            public List<Annotation> Annotations
            {
                get;
                private set;
            }

            public List<Relation> Relations
            {
                get;
                private set;
            }

            public List<Mark> Marks
            {
                get;
                private set;
            }

            public List<Reference> References
            {
                get;
                private set;
            }

            public List<Tag> Tags
            {
                get;
                private set;
            }

            /// <summary>
            /// <see cref="AddingObjectCache"/> の新しいインスタンスを初期化します。
            /// </summary>
            /// <param name="cache">親となる <see cref="StorageCache"/>。</param>
            public AddingObjectCache(StorageCache cache)
            {
                this.Cache = cache;
                this.Accounts = new List<Account>();
                this.Activities = new List<Activity>();
                this.Annotations = new List<Annotation>();
                this.Relations = new List<Relation>();
                this.Marks = new List<Mark>();
                this.References = new List<Reference>();
                this.Tags = new List<Tag>();
            }

            public IEnumerator<StorageObject> GetEnumerator()
            {
                return this.Accounts.Cast<StorageObject>()
                    .Concat(this.Activities.Cast<StorageObject>())
                    .Concat(this.Annotations.Cast<StorageObject>())
                    .Concat(this.Relations.Cast<StorageObject>())
                    .Concat(this.Marks.Cast<StorageObject>())
                    .Concat(this.References.Cast<StorageObject>())
                    .Concat(this.Tags.Cast<StorageObject>())
                    .GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public void Add(Account account)
            {
                this.Accounts.Add(account);
            }

            public void Add(Activity activity)
            {
                this.Activities.Add(activity);
            }

            public void Add(Annotation annotation)
            {
                this.Annotations.Add(annotation);
            }

            public void Add(Relation relation)
            {
                this.Relations.Add(relation);
            }

            public void Add(Mark mark)
            {
                this.Marks.Add(mark);
            }

            public void Add(Reference reference)
            {
                this.References.Add(reference);
            }

            public void Add(Tag tag)
            {
                this.Tags.Add(tag);
            }

            public void Add(StorageObject obj)
            {
                if (obj is Account)
                {
                    this.Add(obj as Account);
                }
                else if (obj is Activity)
                {
                    this.Add(obj as Activity);
                }
                else if (obj is Annotation)
                {
                    this.Add(obj as Annotation);
                }
                else if (obj is Relation)
                {
                    this.Add(obj as Relation);
                }
                else if (obj is Mark)
                {
                    this.Add(obj as Mark);
                }
                else if (obj is Reference)
                {
                    this.Add(obj as Reference);
                }
                else
                {
                    this.Add(obj as Tag);
                }
            }

            public void Remove(Account account)
            {
                this.Accounts.Remove(account);
            }

            public void Remove(Activity activity)
            {
                this.Activities.Remove(activity);
            }

            public void Remove(Annotation annotation)
            {
                this.Annotations.Remove(annotation);
            }

            public void Remove(Relation relation)
            {
                this.Relations.Remove(relation);
            }

            public void Remove(Mark mark)
            {
                this.Marks.Remove(mark);
            }

            public void Remove(Reference reference)
            {
                this.References.Remove(reference);
            }

            public void Remove(Tag tag)
            {
                this.Tags.Remove(tag);
            }

            public void Remove(StorageObject obj)
            {
                if (obj is Account)
                {
                    this.Remove(obj as Account);
                }
                else if (obj is Activity)
                {
                    this.Remove(obj as Activity);
                }
                else if (obj is Annotation)
                {
                    this.Remove(obj as Annotation);
                }
                else if (obj is Relation)
                {
                    this.Remove(obj as Relation);
                }
                else if (obj is Mark)
                {
                    this.Remove(obj as Mark);
                }
                else if (obj is Reference)
                {
                    this.Remove(obj as Reference);
                }
                else
                {
                    this.Remove(obj as Tag);
                }
            }

            public void Clear()
            {
                this.Accounts.Clear();
                this.Activities.Clear();
                this.Annotations.Clear();
                this.Relations.Clear();
                this.Marks.Clear();
                this.References.Clear();
                this.Tags.Clear();
            }
        }
    }
}