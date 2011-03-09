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
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace XSpect.MetaTweet.Objects
{
    [Serializable()]
    public class ActivityTuple
        : StorageObjectTuple<Activity>
    {
        private readonly static MethodInfo _sequenceEqual = typeof(Enumerable).GetMethods()
            .Single(m => m.Name == "SequenceEqual" && m.GetParameters().Length == 2)
            .MakeGenericMethod(new Type[] { typeof(ActivityId), });

        public ActivityId Id
        {
            get;
            set;
        }

        public AccountId AccountId
        {
            get;
            set;
        }

        public ActivityId[] AncestorIds
        {
            get;
            set;
        }

        public String Name
        {
            get;
            set;
        }

        public Object Value
        {
            get;
            set;
        }

        public override String ToString()
        {
            return "[Act" +
                (this.Id != default(ActivityId) ? " Id=" + this.Id.ToString(true) : "") +
                (this.AccountId != default(AccountId) ? " AccountId=" + this.AccountId.ToString(true) : "") +
                (this.AncestorIds != null ? " AncestorIds=" + String.Join(",", this.AncestorIds.Select(i => i.ToString(true))) : "") +
                (this.Name != null ? " Name=" + this.Name : "") +
                (this.Value != null ? " Value=" + this.Value : "") +
                "]";
        }

        public override Expression<Func<Activity, Boolean>> GetMatchExpression()
        {
            BinaryExpression expr = null;
            ParameterExpression param = Expression.Parameter(typeof(Activity));
            ConstantExpression self = Expression.Constant(this);

            if (this.Id != default(ActivityId))
            {
                expr = AndAlso(expr, Expression.Equal(
                    Expression.Property(param, "Id"),
                    Expression.Property(self, "Id")
                ));
            }
            if (this.AccountId != default(AccountId))
            {
                expr = AndAlso(expr, Expression.Equal(
                    Expression.Property(param, "AccountId"),
                    Expression.Property(self, "AccountId")
                ));
            }
            if (this.AncestorIds != null)
            {
                expr = AndAlso(expr, Expression.Call(
                        _sequenceEqual,
                        Expression.Property(self, "AncestorIds"),
                        Expression.Property(param, "AncestorIds")
                    )
                );
            }
            if (this.Name != null)
            {
                expr = AndAlso(expr, Expression.Equal(
                    Expression.Property(param, "Name"),
                    Expression.Property(self, "Name")
                ));
            }
            if (this.Value != null)
            {
                expr = AndAlso(expr, Expression.Equal(
                    Expression.Property(param, "Value"),
                    Expression.Constant(this.Value is JObject
                        ? this.Value
                        : new JObject(new JProperty("_", this.Value is IStorageObjectId
                              ? ((IStorageObjectId) this.Value).HexString
                              : this.Value
                          ))
                    )
                ));
            }
            return Expression.Lambda<Func<Activity, Boolean>>(expr, param);
        }
    }
}
