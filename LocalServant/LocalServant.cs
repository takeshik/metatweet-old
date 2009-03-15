// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * LocalServant
 *   MetaTweet Servant module which provides scheduled operation.
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
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
using Achiral;
using Achiral.Extension;
using XSpect.MetaTweet.Modules;
using XSpect.Configuration;
using System.Timers;
using System.Runtime.Remoting.Messaging;

namespace XSpect.MetaTweet
{
    public class LocalServant
        : ServantModule
    {
        private List<Timer> _timers;

        public override void Initialize()
        {
            new Action(this.RunInitializingJobs).BeginInvoke(
                r => ((r as AsyncResult).AsyncDelegate as Action).EndInvoke(r),
                null
            );

            this._timers = this.Configuration.GetValueOrDefault<
                List<Struct<Double, String>>
            >("jobs")
                .Where(j => j.Item1 > 0.0)
                .Select(j =>
                {
                    Timer timer = new Timer(j.Item1);
                    timer.Elapsed += (sender, e) =>
                    {
                        timer.Stop();
                        this.Host.Request<String>(j.Item2);
                        timer.Start();
                    };
                    return timer;
                }).ToList();
        }

        protected override void StartImpl()
        {
            this._timers.ForEach(t => t.Start());
        }

        protected override void StopImpl()
        {
            this._timers.ForEach(t => t.Stop());
        }

        public override void Dispose()
        {
            this._timers.ForEach(t => t.Dispose());
        }

        private void RunInitializingJobs()
        {
            this.Configuration.GetValueOrDefault<
                List<Struct<Double, String>>
            >("jobs")
                .Where(j => j.Item1 < 0.0)
                .ForEach(j => this.Host.Request<String>(j.Item2));
        }
    }
}