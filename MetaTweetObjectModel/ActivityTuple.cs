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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace XSpect.MetaTweet.Objects
{
    public class ActivityTuple
        : StorageObjectTuple<Activity>
    {
        private readonly static MethodInfo _sequenceEqual = typeof(Enumerable).GetMethods()
            .Single(m => m.Name == "SequenceEqual" && m.GetParameters().Length == 2);

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

        public String UserAgent
        {
            get;
            set;
        }

        public Object Value
        {
            get;
            set;
        }

        public Object Data
        {
            get;
            set;
        }

        public override Expression<Func<Activity, Boolean>> GetMatchExpression()
        {
            BinaryExpression expr = null;
            ParameterExpression param = Expression.Parameter(typeof(Activity));
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
            if (this.UserAgent != null)
            {
                expr = AndAlso(expr, Expression.Equal(
                    Expression.Property(param, "UserAgent"),
                    Expression.Property(self, "UserAgent")
                ));
            }
            if (this.Value != null)
            {
                expr = AndAlso(expr, this.Value != DBNull.Value
                    ? Expression.Equal(
                          Expression.Property(param, "Value"),
                          Expression.Convert(Expression.Property(self, "Value"), typeof(String))
                      )
                    : Expression.Equal(
                          Expression.Property(param, "Value"),
                          Expression.Constant(default(String))
                      )
                );
            }
            if (this.Data != null)
            {
                expr = AndAlso(expr, this.Data != DBNull.Value
                    ? (Expression) Expression.Call(
                          _sequenceEqual,
                          Expression.Property(self, "Data"),
                          Expression.Property(param, "Data")
                      )
                    : (Expression) Expression.Equal(
                          Expression.Property(param, "Data"),
                          Expression.Constant(default(Byte[]))
                      )
                );
            }
            return Expression.Lambda<Func<Activity, Boolean>>(expr, param);
        }
    }
}