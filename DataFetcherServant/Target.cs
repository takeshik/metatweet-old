// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * DataFetcherServant
 *   MetaTweet Servant to fetch Web resources for filling data of activity
 *   Part of MetaTweet
 * Copyright © 2008-2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of DataFetcherServant.
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Dynamic;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Serialization;
using XSpect.MetaTweet.Objects;
using XSpect.Extension;

namespace XSpect.MetaTweet.Modules
{
    [Serializable()]
    public class Target
        : MarshalByRefObject
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

        public String PredicateExpression
        {
            get;
            set;
        }

        public String UriSelectorExpression
        {
            get;
            set;
        }

        [XmlIgnore()]
        public Func<Activity, Boolean> Predicate
        {
            get
            {
                return ExpressionGenerator.ParseLambda<Activity, Boolean>(this.PredicateExpression)
                    .Compile();
            }
        }

        [XmlIgnore()]
        public Func<Activity, Uri> UriSelector
        {
            get
            {
                return Lambda.New((Activity act) => new Uri(
                    ExpressionGenerator.ParseLambda<Activity, String>(this.UriSelectorExpression)
                        .Compile()(act)
                ));
            }
        }
    }
}