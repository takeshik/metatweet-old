// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * RavenLightweightStorage
 *   MetaTweet storage which is provided by Raven Document Database (lightweight client)
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
using Raven.Client.Converters;

namespace XSpect.MetaTweet.Objects
{
    internal sealed class RavenStorageObjectIdConverter
        : ITypeConverter
    {
        public Boolean CanConvertFrom(Type sourceType)
        {
            return typeof(IStorageObjectId).IsAssignableFrom(sourceType);
        }

        public String ConvertFrom(Object value)
        {
            return ((IStorageObjectId) value).HexString;
        }

        public Object ConvertTo(String value)
        {
            switch (value.Length)
            {
                case AccountId.HexStringLength:
                    return new AccountId(value);
                case ActivityId.HexStringLength:
                    return new ActivityId(value);
                case AdvertisementId.HexStringLength:
                    return new AdvertisementId(value);
                default:
                    throw new InvalidOperationException();
            }
        }

        public String ConvertFrom(String tag, Object value, Boolean allowNull)
        {
            throw new NotImplementedException();
        }
    }
}