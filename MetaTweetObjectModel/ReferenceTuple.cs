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
using System.Linq.Expressions;

namespace XSpect.MetaTweet.Objects
{
    [Serializable()]
    public class ReferenceTuple
        : StorageObjectTuple<Reference>
    {
        public String AccountId
        {
            get;
            set;
        }

        public Nullable<DateTime> Timestamp
        {
            get;
            set;
        }

        public String Category
        {
            get;
            set;
        }

        public String SubId
        {
            get;
            set;
        }

        public String Name
        {
            get;
            set;
        }

        public String ReferringAccountId
        {
            get;
            set;
        }

        public Nullable<DateTime> ReferringTimestamp
        {
            get;
            set;
        }

        public String ReferringCategory
        {
            get;
            set;
        }

        public String ReferringSubId
        {
            get;
            set;
        }

        public override Expression<Func<Reference, Boolean>> GetMatchExpression()
        {
            BinaryExpression expr = null;
            ParameterExpression param = Expression.Parameter(typeof(Reference));
            ConstantExpression self = Expression.Constant(this);

            if (this.AccountId != null)
            {
                expr = AndAlso(expr, Expression.Equal(
                    Expression.Property(param, "AccountId"),
                    Expression.Property(self, "AccountId")
                ));
            }
            if (this.Timestamp != null)
            {
                expr = AndAlso(expr, Expression.Equal(
                    Expression.Property(param, "Timestamp"),
                    Expression.Property(Expression.Property(self, "Timestamp"), "Value")
                ));
            }
            if (this.Category != null)
            {
                expr = AndAlso(expr, Expression.Equal(
                    Expression.Property(param, "Category"),
                    Expression.Property(self, "Category")
                ));
            }
            if (this.SubId != null)
            {
                expr = AndAlso(expr, Expression.Equal(
                    Expression.Property(param, "SubId"),
                    Expression.Property(self, "SubId")
                ));
            }
            if (this.Name != null)
            {
                expr = AndAlso(expr, Expression.Equal(
                    Expression.Property(param, "Name"),
                    Expression.Property(self, "Name")
                ));
            }
            if (this.ReferringAccountId != null)
            {
                expr = AndAlso(expr, Expression.Equal(
                    Expression.Property(param, "ReferringAccountId"),
                    Expression.Property(self, "ReferringAccountId")
                ));
            }
            if (this.ReferringTimestamp != null)
            {
                expr = AndAlso(expr, Expression.Equal(
                    Expression.Property(param, "ReferringTimestamp"),
                    Expression.Property(Expression.Property(self, "ReferringTimestamp"), "Value")
                ));
            }
            if (this.ReferringCategory != null)
            {
                expr = AndAlso(expr, Expression.Equal(
                    Expression.Property(param, "ReferringCategory"),
                    Expression.Property(self, "ReferringCategory")
                ));
            }
            if (this.ReferringSubId != null)
            {
                expr = AndAlso(expr, Expression.Equal(
                    Expression.Property(param, "ReferringSubId"),
                    Expression.Property(self, "ReferringSubId")
                ));
            }
            return Expression.Lambda<Func<Reference, Boolean>>(expr, param);
        }
    }
}