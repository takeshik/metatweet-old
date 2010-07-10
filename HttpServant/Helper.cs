// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * HttpServant
 *   MetaTweet Servant which provides HTTP service
 *   Part of MetaTweet
 * Copyright © 2008-2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of HttpServant.
 * 
 * This program is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation; either version 3 of the License, or (at your
 * option) any later version.
 * 
 * This program is distributed in the hope that it will be useful, but
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
using System.Linq.Dynamic;
using System.Reflection;

namespace XSpect.MetaTweet.Modules
{
    public static class Helper
    {
        public static String GetThisAssemblyInfo(String name)
        {
            return (String) typeof(ThisAssembly)
                .GetField(name, BindingFlags.Static | BindingFlags.NonPublic)
                .GetValue(null);
        }

        public static String GetFormattedVersionInfo()
        {
            return String.Format(
                "<a href='http://www.metatweet.org/'>MetaTweet</a> version {0} (" + 
                    "<a href='http://github.com/takeshik/metatweet/tree/{1}'>{1}</a>: " +
                    "<a href='http://github.com/takeshik/metatweet/commit/{2}'>{2}</a>) {3}",
                ThisAssembly.EntireVersion,
                ThisAssembly.Branch,
                ThisAssembly.EntireCommitId,
                ThisAssembly.EntireCommittedAt
            );
        }

        public static String Eval(String expr, params Object[] values)
        {
            return ExpressionGenerator.ParseLambda<String>(expr, values).Compile()();
        }
    }
}