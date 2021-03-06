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
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace XSpect.MetaTweet.Objects
{
    public static class StorageObjectExtensions
    {
        public static Object TryGetValue(this Activity activity)
        {
            return activity != null
                ? activity.GetValue()
                : null;
        }

        public static T TryGetValue<T>(this Activity activity)
        {
            return activity != null
                ? activity.GetValue<T>()
                : default(T);
        }

        public static Object Lookup<T>(this StorageObject obj, String name, Nullable<DateTime> maxTimestamp = null)
        {
            return (obj is Account
                ? (Account) obj
                : obj is Activity
                      ? ((Activity) obj).Account
                      : ((Advertisement) obj).Activity.Account
            ).Lookup(name, maxTimestamp).GetValue<T>();
        }

        public static Object Lookup(this StorageObject obj, String name, Nullable<DateTime> maxTimestamp = null)
        {
            return obj.Lookup<Object>(name, maxTimestamp);
        }

        public static Object GetValue<T>(this StorageObject obj, params String[] names)
        {
            Activity activity = (obj is Account
                ? ((Account) obj)[names.First()]
                : (obj is Activity
                      ? ((Activity) obj)
                      : ((Advertisement) obj).Activity
                  )[names.First()]
            ).SingleOrDefault();
            if (activity == null)
            {
                return null;
            }
            foreach (String name in names.Skip(1))
            {
                activity = activity[name].SingleOrDefault();
                if (activity == null)
                {
                    return null;
                }
            }
            return activity.GetValue<T>();
        }

        public static Object GetValue(this StorageObject obj, params String[] names)
        {
            return obj.GetValue<Object>(names);
        }
    }
}