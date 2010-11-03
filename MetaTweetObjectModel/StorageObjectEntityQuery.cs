// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetObjectModel
 *   Object model and Storage interface for MetaTweet and other systems
 *   Part of MetaTweet
 * Copyright © 2008-2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
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
using System.Data.Objects;
using System.Linq;

namespace XSpect.MetaTweet.Objects
{
    public class StorageObjectEntityQuery<TObject, TTuple>
        : StorageObjectQuery<TObject, TTuple>
        where TTuple : StorageObjectTuple<TObject>
        where TObject : StorageObject
    {
        public String EntitySqlQuery
        {
            get;
            set;
        }

        public override String ToString()
        {
            return String.Format(
                "Scalar: {1}{0}EntitySql: {2}",
                Environment.NewLine,
                this.ScalarMatch,
                this.EntitySqlQuery
            );
        }

        public override IQueryable<TObject> Evaluate(IQueryable<TObject> source)
        {
            return source is ObjectQuery
                ? this.Evaluate((StorageObjectContext) ((ObjectQuery) source).Context)
                : base.Evaluate(source);
        }

        public virtual IQueryable<TObject> Evaluate(StorageObjectContext context)
        {
            return base.Evaluate(this.EntitySqlQuery != null
                ? context.CreateQuery<TObject>(this.EntitySqlQuery)
                : context.GetObjectSet<TObject>()
            );
        }
    }
}