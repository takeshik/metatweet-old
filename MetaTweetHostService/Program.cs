// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetHostService
 *   Windows Service which hosts MetaTweetServer
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetHostService.
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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text.RegularExpressions;
using System.Threading;

namespace XSpect.MetaTweet
{
    internal static class Program
    {
        private static Exception _lastException;

        private static void Main(String[] args)
        {
            if (args.TakeWhile(s => s == "-").Any(s => s == "-d" || s == "-debug"))
            {
                Debugger.Launch();
            }
            if (Environment.UserInteractive)
            {
                RunServerInConsole(args.SkipWhile(s => s == "-"));
            }
            else
            {
                ServiceBase.Run(new ServerHost());
            }
        }

        private static void RunServerInConsole(IEnumerable<String> args)
        {
            Console.WriteLine("## MetaTweet Server Hosting Process (interactive mode).");
            Console.CancelKeyPress += (sender, e) =>
            {
                if (e.SpecialKey == ConsoleSpecialKey.ControlC)
                {
                    Console.WriteLine("## Type 'stop [ENTER]' to start shutdown the server.");
                    e.Cancel = true;
                }
                else
                {
                    Environment.Exit(127);
                }
            };
            Debug.Listeners.Add(new TextWriterTraceListener(Console.Error));
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Error));
            Environment.CurrentDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            foreach (Match match in args
                .Select(s => Regex.Match(s, "(-(?<key>[a-zA-Z0-9_]*)(=(?<value>(\"[^\"]*\")|('[^']*')|(.*)))?)*"))
                .Where(m => m.Success)
            )
            {
                ServerLauncher.Instance.Arguments[match.Groups["key"].Value] = match.Groups["value"].Success
                    ? match.Groups["value"].Value
                    : "true";
            }
            ServerLauncher.Instance.Arguments[".pid"] = Process.GetCurrentProcess().Id.ToString();
            ServerLauncher.Instance.Arguments[".svc_id"] = String.Empty;
            Console.WriteLine("## Loading...");
            ServerLauncher.Instance.StartServer();
            Console.WriteLine("## MetaTweet Server was started successfully. Enter in interactive mode.");
            Console.WriteLine("## Type 'help [ENTER]' to show help.");
            while (true)
            {
                String input = Console.ReadLine();
                if (String.IsNullOrWhiteSpace(input))
                {
                    continue;
                }
                String[] inputs = input.Split(' ');
                if (inputs[0].ToLower() == "stop")
                {
                    InteractiveCommands.Stop();
                }
                try
                {
                    typeof(InteractiveCommands).GetMethod(
                        inputs[0],
                        BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase,
                        null,
                        Enumerable.Repeat(typeof(String), inputs.Length - 1).ToArray(),
                        null
                    ).Invoke(null, inputs.Skip(1).ToArray());
                }
                catch (Exception ex)
                {
                    _lastException = ex;
                    Console.WriteLine("## Exception was thrown in interactive command:");
                    Console.WriteLine("##   {0}: {1}", ex.GetType().FullName, ex.Message);
                }
            }
        }
    }
}

