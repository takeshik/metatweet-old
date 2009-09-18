﻿// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetMint
 *   Extensible GUI client for MetaTweet
 *   Part of MetaTweet
 * Copyright © 2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetMint.
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using XSpect.MetaTweet.Clients.Mint;
using XSpect.Windows.Forms;

namespace XSpect.MetaTweet.Clients.Mint
{
    internal static class Program
    {
        private static IDictionary<String, String> _parameters = new Dictionary<String, String>();

        [STAThread()]
        private static void Main(String[] args)
        {
            _parameters = ConfigurationManager.AppSettings.AllKeys
                .ToDictionary(k => k, k => ConfigurationManager.AppSettings[k]);
            foreach (Match match in args
                .TakeWhile(s => s != "--")
                .Select(s => Regex.Match(s, "(-(?<key>[a-zA-Z0-9_]*)(=(?<value>(\"[^\"]*\")|('[^']*')|(.*)))?)*"))
                .Where(m => m.Success)
            )
            {
                _parameters[match.Groups["key"].Value] = match.Groups["value"].Success
                    ? match.Groups["value"].Value
                    : "true";
            }

            if (AppDomain.CurrentDomain.IsDefaultAppDomain())
            {
                Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                AppDomain domain = AppDomain.CreateDomain(
                    "MetaTweetMint.exe:run",
                    AppDomain.CurrentDomain.Evidence,
                    new AppDomainSetup()
                    {
                        ApplicationBase = Path.GetFullPath(_parameters["init_base"]),
                        PrivateBinPath = _parameters["init_probe"],
                        PrivateBinPathProbe = "true",
                        ApplicationName = "MetaTweetMint",
                        LoaderOptimization = LoaderOptimization.MultiDomainHost,
                    }
                );
                domain.ExecuteAssembly(Assembly.GetExecutingAssembly().Location, null, args);
                return;
            }

            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            Application.ThreadException += (sender, e) =>
                new ExceptionForm(e.Exception, new Uri("https://sourceforge.net/tracker/?group_id=248108&atid=1127270"))
                    .Show();

            using (ClientCore client = new ClientCore())
            {
                client.Initialize(_parameters);
                client.Run();
            }
        }
    }
}