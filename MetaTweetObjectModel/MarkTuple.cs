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
    public class MarkTuple
        : StorageObjectTuple<Mark>
    {
        public String AccountId
        {
            get;
            set;
        }

        public String Name
        {
            get;
            set;
        }

        public String MarkingAccountId
        {
            get;
            set;
        }

        public Nullable<DateTime> MarkingTimestamp
        {
            get;
            set;
        }

        public String MarkingCategory
        {
            get;
            set;
        }

        public String MarkingSubId
        {
            get;
            set;
        }

        public override Expression<Func<Mark, Boolean>> GetMatchExpression()
        {
            BinaryExpression expr = null;
            ParameterExpression param = Expression.Parameter(typeof(Mark));
            ConstantExpression self = Expression.Constant(this);

            if (this.AccountId != null)
            {
                expr = AndAlso(expr, Expression.Equal(
                    Expression.Property(param, "AccountId"),
                    Expression.Property(self, "AccountId")
                ));
            }
            if (this.Name != null)
            {
                expr = AndAlso(expr, Expression.Equal(
                    Expression.Property(param, "Name"),
                    Expression.Property(self, "Name")
                ));
            }
            if (this.MarkingAccountId != null)
            {
                expr = AndAlso(expr, Expression.Equal(
                    Expression.Property(param, "MarkingAccountId"),
                    Expression.Property(self, "MarkingAccountId")
                ));
            }
            if (this.MarkingTimestamp != null)
            {
                expr = AndAlso(expr, Expression.Equal(
                    Expression.Property(param, "MarkingTimestamp"),
                    Expression.Property(Expression.Property(self, "MarkingTimestamp"), "Value")
                ));
            }
            if (this.MarkingCategory != null)
            {
                expr = AndAlso(expr, Expression.Equal(
                    Expression.Property(param, "MarkingCategory"),
                    Expression.Property(self, "MarkingCategory")
                ));
            }
            if (this.MarkingSubId != null)
            {
                expr = AndAlso(expr, Expression.Equal(
                    Expression.Property(param, "MarkingSubId"),
                    Expression.Property(self, "MarkingSubId")
                ));
            }
            return Expression.Lambda<Func<Mark, Boolean>>(expr, param);
        }
    }
}