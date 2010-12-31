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
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace XSpect.MetaTweet.Objects
{
    [Serializable()]
    public class StorageObjectEntityQuery<TObject, TTuple>
        : StorageObjectQuery<TObject, TTuple>
        where TObject : StorageObject
        where TTuple : StorageObjectTuple<TObject>
    {
        public String EntitySqlQuery
        {
            get;
            set;
        }

        public Expression<Func<IQueryable<TObject>, IQueryable<TObject>>> QueryExpression
        {
            get;
            set;
        }

        public override String ToString()
        {
            return String.Format(
                "Scalar: {1}{0}EntitySql: {2}{0}QueryExpression: {3}{0}PostExpression: {4}",
                Environment.NewLine,
                this.ScalarMatch,
                this.EntitySqlQuery,
                this.QueryExpression,
                this.PostExpression
            );
        }

        public override IQueryable<TObject> Evaluate(IQueryable<TObject> source)
        {
            return this.ExecutePostExpression(
                this.ExecuteQueryExpression(
                    this.ExecuteScalarMatch(
                        this.ExecuteEntitySqlQuery(source)
                    )
                )
            );
        }

        public virtual IQueryable<TObject> Evaluate(StorageObjectContext context)
        {
            return base.Evaluate(this.EntitySqlQuery != null
                ? context.CreateQuery<TObject>(this.EntitySqlQuery)
                : context.GetObjectSet<TObject>()
            );
        }

        protected IQueryable<TObject> ExecuteEntitySqlQuery(IQueryable<TObject> source)
        {
            return this.EntitySqlQuery != null && source is ObjectQuery
                ? ((ObjectQuery) source).Context.CreateQuery<TObject>(this.EntitySqlQuery)
                : source;
        }

        protected IQueryable<TObject> ExecuteQueryExpression(IQueryable<TObject> source)
        {
            return this.QueryExpression != null
                ? this.QueryExpression.Compile()(source)
                : source;
        }
    }

    public static class StorageObjectEntityQuery
    {
        public static StorageObjectEntityQuery<Account, AccountTuple> Account(
            String entitySqlQuery = null,
            AccountTuple scalarMatch = null,
            Expression<Func<IQueryable<Account>, IQueryable<Account>>> queryExpression = null,
            Expression<Func<IQueryable<Account>, IQueryable<Account>>> postExpression = null
        )
        {
            return new StorageObjectEntityQuery<Account, AccountTuple>()
            {
                EntitySqlQuery = entitySqlQuery,
                ScalarMatch = scalarMatch,
                QueryExpression = queryExpression,
                PostExpression = postExpression,
            };
        }

        public static StorageObjectEntityQuery<Activity, ActivityTuple> Activity(
            String entitySqlQuery = null,
            ActivityTuple scalarMatch = null,
            Expression<Func<IQueryable<Activity>, IQueryable<Activity>>> queryExpression = null,
            Expression<Func<IQueryable<Activity>, IQueryable<Activity>>> postExpression = null
        )
        {
            return new StorageObjectEntityQuery<Activity, ActivityTuple>()
            {
                EntitySqlQuery = entitySqlQuery,
                ScalarMatch = scalarMatch,
                QueryExpression = queryExpression,
                PostExpression = postExpression,
            };
        }

        public static StorageObjectEntityQuery<Annotation, AnnotationTuple> Annotation(
            String entitySqlQuery = null,
            AnnotationTuple scalarMatch = null,
            Expression<Func<IQueryable<Annotation>, IQueryable<Annotation>>> queryExpression = null,
            Expression<Func<IQueryable<Annotation>, IQueryable<Annotation>>> postExpression = null
        )
        {
            return new StorageObjectEntityQuery<Annotation, AnnotationTuple>()
            {
                EntitySqlQuery = entitySqlQuery,
                ScalarMatch = scalarMatch,
                QueryExpression = queryExpression,
                PostExpression = postExpression,
            };
        }

        public static StorageObjectEntityQuery<Relation, RelationTuple> Relation(
            String entitySqlQuery = null,
            RelationTuple scalarMatch = null,
            Expression<Func<IQueryable<Relation>, IQueryable<Relation>>> queryExpression = null,
            Expression<Func<IQueryable<Relation>, IQueryable<Relation>>> postExpression = null
        )
        {
            return new StorageObjectEntityQuery<Relation, RelationTuple>()
            {
                EntitySqlQuery = entitySqlQuery,
                ScalarMatch = scalarMatch,
                QueryExpression = queryExpression,
                PostExpression = postExpression,
            };
        }

        public static StorageObjectEntityQuery<Mark, MarkTuple> Mark(
            String entitySqlQuery = null,
            MarkTuple scalarMatch = null,
            Expression<Func<IQueryable<Mark>, IQueryable<Mark>>> queryExpression = null,
            Expression<Func<IQueryable<Mark>, IQueryable<Mark>>> postExpression = null
        )
        {
            return new StorageObjectEntityQuery<Mark, MarkTuple>()
            {
                EntitySqlQuery = entitySqlQuery,
                ScalarMatch = scalarMatch,
                QueryExpression = queryExpression,
                PostExpression = postExpression,
            };
        }

        public static StorageObjectEntityQuery<Reference, ReferenceTuple> Reference(
            String entitySqlQuery = null,
            ReferenceTuple scalarMatch = null,
            Expression<Func<IQueryable<Reference>, IQueryable<Reference>>> queryExpression = null,
            Expression<Func<IQueryable<Reference>, IQueryable<Reference>>> postExpression = null
        )
        {
            return new StorageObjectEntityQuery<Reference, ReferenceTuple>()
            {
                EntitySqlQuery = entitySqlQuery,
                ScalarMatch = scalarMatch,
                QueryExpression = queryExpression,
                PostExpression = postExpression,
            };
        }

        public static StorageObjectEntityQuery<Tag, TagTuple> Tag(
            String entitySqlQuery = null,
            TagTuple scalarMatch = null,
            Expression<Func<IQueryable<Tag>, IQueryable<Tag>>> queryExpression = null,
            Expression<Func<IQueryable<Tag>, IQueryable<Tag>>> postExpression = null
        )
        {
            return new StorageObjectEntityQuery<Tag, TagTuple>()
            {
                EntitySqlQuery = entitySqlQuery,
                ScalarMatch = scalarMatch,
                QueryExpression = queryExpression,
                PostExpression = postExpression,
            };
        }
    }
}