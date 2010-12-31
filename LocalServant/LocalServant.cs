// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * LocalServant
 *   MetaTweet Servant module which provides scheduled operation
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of LocalServant.
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
using System.Linq;
using System.Net;
using System.Reflection;
using Achiral;
using Achiral.Extension;
using XSpect.Extension;
using XSpect.MetaTweet.Modules;
using System.Timers;
using XSpect.MetaTweet.Requesting;

namespace XSpect.MetaTweet.Modules
{
    public class LocalServant
        : ServantModule
    {
        private List<Timer> _timers;

        public IEnumerable<MutableTuple<Double, String>> TimerJobs
        {
            get;
            private set;
        }

        protected override void InitializeImpl()
        {
            this._timers = this.TimerJobs
                .Where(j => j.Item1 > 0.0)
                .OrderBy(j => j.Item1)
                .Select(j =>
                {
                    Timer timer = new Timer(j.Item1);
                    timer.Elapsed += (sender, e) =>
                    {
                        timer.Stop();
                        try
                        {
                            this.Host.RequestManager.Execute<Object>(Request.Parse(j.Item2));
                        }
                        finally
                        {
                            timer.Start();
                        }
                    };
                    return timer;
                }).ToList();
            this.RunInitializingJobs();
            base.InitializeImpl();
        }

        protected override void ConfigureImpl(System.IO.FileInfo configFile)
        {
            base.ConfigureImpl(configFile);
            this.TimerJobs = ((IList<Object>) this.Configuration.TimerJobs)
                .Cast<MutableTuple<Double, String>>();
        }

        protected override void StartImpl()
        {
            this._timers.ForEach(t => t.Start());
        }

        protected override void StopImpl()
        {
            this._timers.ForEach(t => t.Stop());
        }

        protected override void Dispose(Boolean disposing)
        {
            this._timers.ForEach(t => t.Dispose());
            base.Dispose(disposing);
        }

        private void RunInitializingJobs()
        {
            this.TimerJobs
                .Where(j => j.Item1 < 0.0)
                .ForEach(j =>
                {
                    try
                    {
                        this.Host.RequestManager.Execute<Object>(Request.Parse(j.Item2));
                    }
                    // Provision for problems of remote services
                    catch (WebException)
                    {
                    }
                });
        }
    }
}