// -*- mode: csharp; encoding: utf-8; -*-
// $Id$
/* XSpect Common Framework - Generic utility class library
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of XSpect Common Framework.
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Achiral;
using Achiral.Extension;
using XSpect;
using XSpect.Extension;

namespace XSpect
{
    public class Shell
        : Object
    {
        public static IDictionary<String, String> GetArguments(String[] args, String baseFile)
        {
            Dictionary<String, String> arguments = new Dictionary<String, String>();
            if (!baseFile.IsNullOrEmpty() && new FileInfo(Assembly.GetCallingAssembly().Location).Directory.File(baseFile).Exists)
            {
                args = File.ReadAllLines("MetaTweet.args")
                    .Where(l => !(l.StartsWith("#") || String.IsNullOrEmpty(l)))
                    .Concat(args)
                    .ToArray();
            }
            foreach (Match match in args
                .Select(s => Regex.Match(s, "(-(?<key>[a-zA-Z0-9_]*)(=(?<value>(\"[^\"]*\")|('[^']*')|(.*)))?)*"))
                .Where(m => m.Success)
            )
            {
                arguments[match.Groups["key"].Value] = match.Groups["value"].Success
                    ? match.Groups["value"].Value
                    : "true";
            }
            return arguments;
        }
    }
}