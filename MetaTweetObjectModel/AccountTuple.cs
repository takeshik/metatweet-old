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
using System.Linq.Expressions;

namespace XSpect.MetaTweet.Objects
{
    [Serializable()]
    public class AccountTuple
        : StorageObjectTuple<Account>
    {
        public AccountId Id
        {
            get;
            set;
        }

        public String Realm
        {
            get;
            set;
        }

        public String Seed
        {
            get;
            set;
        }

        public override String ToString()
        {
            return "[Acc" +
                (this.Id != default(AccountId) ? " Id=" + this.Id.ToString(true) : "") +
                (this.Realm != null ? " Realm=" + this.Realm : "") +
                (this.Seed != null ? " Seed=" + this.Seed : "") +
                "]";
        }

        public override Expression<Func<Account, Boolean>> GetMatchExpression()
        {
            BinaryExpression expr = null;
            ParameterExpression param = Expression.Parameter(typeof(Account));
            ConstantExpression self = Expression.Constant(this);

            if (this.Id != default(AccountId))
            {
                expr = AndAlso(expr, Expression.Equal(
                    Expression.Property(param, "Id"),
                    Expression.Property(self, "Id")
                ));
            }
            if (this.Realm != null)
            {
                expr = AndAlso(expr, Expression.Equal(
                    Expression.Property(param, "Realm"),
                    Expression.Property(self, "Realm")
                ));
            }
            if (this.Seed != null)
            {
                expr = AndAlso(expr, Expression.Equal(
                    Expression.Property(param, "Seed"),
                    Expression.Property(self, "Seed")
                ));
            }
            return Expression.Lambda<Func<Account, Boolean>>(expr, param);
        }

    }
}
