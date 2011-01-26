﻿// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
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
    public class AdvertisementTuple
        : StorageObjectTuple<Advertisement>
    {
        public AdvertisementId Id
        {
            get;
            set;
        }

        public ActivityId ActivityId
        {
            get;
            set;
        }

        public DateTime Timestamp
        {
            get;
            set;
        }

        public AdvertisementFlags Flags
        {
            get;
            set;
        }

        public override Expression<Func<Advertisement, Boolean>> GetMatchExpression()
        {
            BinaryExpression expr = null;
            ParameterExpression param = Expression.Parameter(typeof(Advertisement));
            ConstantExpression self = Expression.Constant(this);

            if (this.Id != default(AdvertisementId))
            {
                expr = AndAlso(expr, Expression.Equal(
                    Expression.Property(param, "Id"),
                    Expression.Property(self, "Id")
                ));
            }
            if (this.ActivityId != default(AccountId))
            {
                expr = AndAlso(expr, Expression.Equal(
                    Expression.Property(param, "ActivityId"),
                    Expression.Property(self, "ActivityId")
                ));
            }
            if (this.Timestamp != default(DateTime))
            {
                expr = AndAlso(expr, Expression.Equal(
                    Expression.Property(param, "Timestamp"),
                    Expression.Property(self, "Timestamp")
                ));
            }
            if (this.Flags != default(AdvertisementFlags))
            {
                expr = AndAlso(expr, Expression.Equal(
                    Expression.Property(param, "Flags"),
                    Expression.Property(self, "Flags")
                ));
            }
            return Expression.Lambda<Func<Advertisement, Boolean>>(expr, param);
        }
    }
}