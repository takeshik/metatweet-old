// -*- mode: csharp; encoding: utf-8; -*-
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetConsole
 *   Bandled CLI client / manager for MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetConsole.
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
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace XSpect.MetaTweet.Clients
{
    internal class Program
    {
        [STAThread()]
        static void Main(String[] args)
        {
            Environment.CurrentDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            if (AppDomain.CurrentDomain.IsDefaultAppDomain())
            {
                Dictionary<String, String> arguments = ConfigurationManager.AppSettings.AllKeys
                    .ToDictionary(k => k, k => ConfigurationManager.AppSettings[k]);
                foreach (Match match in args
                    .Select(s => Regex.Match(s, "(-(?<key>[a-zA-Z0-9_]*)(=(?<value>(\"[^\"]*\")|('[^']*')|(.*)))?)*"))
                    .Where(m => m.Success)
                    )
                {
                    arguments[match.Groups["key"].Value] = match.Groups["value"].Success
                        ? match.Groups["value"].Value
                        : "true";
                }
                AppDomain domain = AppDomain.CreateDomain(
                    "MetaTweetConsole.exe:run",
                    AppDomain.CurrentDomain.Evidence,
                    new AppDomainSetup()
                    {
                        ApplicationBase = Path.GetFullPath(arguments.ContainsKey("init_base")
                            ? arguments["init_base"]
                            : ".."
                        ),
                        ConfigurationFile = Path.Combine(arguments.ContainsKey("init_config")
                            ? arguments["init_config"]
                            : "etc",
                            "MetaTweetConsole.exe.config"
                        ),
                        PrivateBinPath = arguments.ContainsKey("init_probe")
                            ? arguments["init_probe"]
                            : "lib;sbin",
                        PrivateBinPathProbe = "true",
                        ApplicationName = "MetaTweetConsole",
                        LoaderOptimization = LoaderOptimization.MultiDomainHost,
                    }
                );
                domain.ExecuteAssembly(Assembly.GetExecutingAssembly().Location);
                return;
            }

            Shell shell = new Shell();
            while (true)
            {
                Console.WriteLine(shell.Evaluate(Console.ReadLine()));
            }
        }
    }
}
