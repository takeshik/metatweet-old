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
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace XSpect.MetaTweet.Objects
{
    [Serializable()]
    public class StorageObjectExpressionQuery<TObject, TTuple>
        : IStorageObjectQuery<TObject>
        where TObject : StorageObject
        where TTuple : StorageObjectTuple<TObject>
    {
        public TTuple ScalarMatch
        {
            get;
            set;
        }

        public Expression<Func<IQueryable<TObject>, IQueryable<TObject>>> QueryExpression
        {
            get;
            set;
        }

        public Expression<Func<IQueryable<TObject>, IQueryable<TObject>>> PostExpression
        {
            get;
            set;
        }

        public override String ToString()
        {
            return String.Format(
                "Scalar: {1}{0}QueryExpression: {2}{0}PostExpression: {3}",
                Environment.NewLine,
                this.ScalarMatch,
                this.QueryExpression,
                this.PostExpression
            );
        }

        public virtual ICollection<TObject> Evaluate(IQueryable<TObject> source)
        {
            return this.ExecutePostExpression(
                this.ExecuteQueryExpression(
                    this.ExecuteScalarMatch(source)
                )
            ).ToArray();
        }

        protected IQueryable<TObject> ExecuteScalarMatch(IQueryable<TObject> source)
        {
            return this.ScalarMatch != null
                ? source.Where(this.ScalarMatch.GetMatchExpression())
                : source;
        }

        protected IQueryable<TObject> ExecuteQueryExpression(IQueryable<TObject> source)
        {
            return this.QueryExpression != null
                ? this.QueryExpression.Compile()(source)
                : source;
        }

        protected IQueryable<TObject> ExecutePostExpression(IQueryable<TObject> source)
        {
            return this.PostExpression != null
                ? this.PostExpression.Compile()(source.ToArray().AsQueryable())
                : source;
        }
    }

    public static class StorageObjectExpressionQuery
    {
        public static StorageObjectExpressionQuery<Account, AccountTuple> Account(
            AccountTuple scalarMatch = null,
            Expression<Func<IQueryable<Account>, IQueryable<Account>>> queryExpression = null,
            Expression<Func<IQueryable<Account>, IQueryable<Account>>> postExpression = null
        )
        {
            return new StorageObjectExpressionQuery<Account, AccountTuple>()
            {
                ScalarMatch = scalarMatch,
                QueryExpression = queryExpression,
                PostExpression = postExpression,
            };
        }

        public static StorageObjectExpressionQuery<Activity, ActivityTuple> Activity(
            ActivityTuple scalarMatch = null,
            Expression<Func<IQueryable<Activity>, IQueryable<Activity>>> queryExpression = null,
            Expression<Func<IQueryable<Activity>, IQueryable<Activity>>> postExpression = null
        )
        {
            return new StorageObjectExpressionQuery<Activity, ActivityTuple>()
            {
                ScalarMatch = scalarMatch,
                QueryExpression = queryExpression,
                PostExpression = postExpression,
            };
        }

        public static StorageObjectExpressionQuery<Advertisement, AdvertisementTuple> Advertisement(
            AdvertisementTuple scalarMatch = null,
            Expression<Func<IQueryable<Advertisement>, IQueryable<Advertisement>>> queryExpression = null,
            Expression<Func<IQueryable<Advertisement>, IQueryable<Advertisement>>> postExpression = null
        )
        {
            return new StorageObjectExpressionQuery<Advertisement, AdvertisementTuple>()
            {
                ScalarMatch = scalarMatch,
                QueryExpression = queryExpression,
                PostExpression = postExpression,
            };
        }
    }
}