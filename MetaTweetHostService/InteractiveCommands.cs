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
    internal static class InteractiveCommands
    {
        public static void Help()
        {
            Console.WriteLine("## Available Commands are:");
            typeof(InteractiveCommands).GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Select(m => "##   " + m.Name + " " + String.Join(" ", m.GetParameters().Select(p => "<" + p.Name + ">")))
                .OrderBy(_ => _)
                .ToList()
                .ForEach(Console.WriteLine);
            Console.WriteLine("## All commands are case-insensitive. Each parameters are separated by one space.");
        }

        public static void Stop()
        {
            Console.WriteLine("## MetaTweet Server requested to shutdown.");
            ServerLauncher.Instance.StopServerGracefully();
            Console.WriteLine("## MetaTweet Server shutdown successfully.");
            Environment.Exit(0);
        }

        public static void Gc()
        {
            Console.WriteLine("## Collecting garbages... (Working Set Size: {0})", Environment.WorkingSet);
            GC.Collect();
            Console.WriteLine("## Done. (Working Set Size: {0})", Environment.WorkingSet);
        }

        public static void Clear()
        {
            Console.Clear();
        }

        public static void Version()
        {
            Console.WriteLine("## Version:");
            Console.WriteLine("##   " + ThisAssembly.EntireVersionInfo);
        }

        public static void Debug()
        {
            if (Debugger.IsAttached)
            {
                Console.WriteLine("## This process is already attached to the debugger.");
            }
            else
            {
                Console.WriteLine("## Attaching to the debugger...");
                Debugger.Launch();
                Console.WriteLine("## Done.");
            }
        }
    }
}