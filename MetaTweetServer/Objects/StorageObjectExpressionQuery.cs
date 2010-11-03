// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
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
using System.Data.Objects;
using System.Linq;
using System.Linq.Dynamic;

namespace XSpect.MetaTweet.Objects
{
    public class StorageObjectExpressionQuery<TObject, TTuple>
        : StorageObjectEntityQuery<TObject, TTuple>
        where TTuple : StorageObjectTuple<TObject>
        where TObject : StorageObject
    {
        public String ExpressionQuery
        {
            get;
            set;
        }

        public String PostExpressionQuery
        {
            get;
            set;
        }

        public override String ToString()
        {
            return String.Format(
                "Scalar: {1}{0}EntitySql: {2}{0}Expression: {3}{0}PostExpression: {4}",
                Environment.NewLine,
                this.ScalarMatch,
                this.EntitySqlQuery,
                this.ExpressionQuery,
                this.PostExpressionQuery
            );
        }

        public override IQueryable<TObject> Evaluate(StorageObjectContext context)
        {
            return ((ObjectQuery<TObject>) base.Evaluate(context).Execute(this.ExpressionQuery ?? ""))
                .Execute(context.MergeOption)
                .AsQueryable()
                .Execute(this.PostExpressionQuery ?? "")
                .OfType<TObject>();
        }
    }
}