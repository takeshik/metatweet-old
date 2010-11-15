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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Objects;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Threading;
using Achiral;
using Achiral.Extension;
using XSpect.MetaTweet.Objects;
using XSpect.Extension;

namespace XSpect.MetaTweet.Modules
{
    using Target = MutableTuple<MutableTuple<String, Nullable<DateTime>, String, String, String, Object>, String, String>;

    public class DataFetcherServant
        : ServantModule
    {
        private readonly ConcurrentQueue<Tuple<Activity, Uri>> _queue;

        private readonly ConcurrentQueue<Tuple<Activity, Uri>> _immediateQueue;

        private readonly Thread _streamingThread;

        private List<Thread> _workers;

        private readonly Mutex _retrieveMutex;

        public String StorageName
        {
            get;
            private set;
        }

        public Int32 WorkerCount
        {
            get;
            private set;
        }

        public IList<Target> Targets
        {
            get;
            set;
        }

        public DataFetcherServant()
        {
            this._queue = new ConcurrentQueue<Tuple<Activity, Uri>>();
            this._immediateQueue = new ConcurrentQueue<Tuple<Activity, Uri>>();
            this._streamingThread = new Thread(this.EnqueueFromStream);
            this._retrieveMutex = new Mutex();
        }

        protected override void ConfigureImpl(FileInfo configFile)
        {
            base.ConfigureImpl(configFile);
            this.StorageName = this.Configuration.StorageName;
            this.WorkerCount = this.Configuration.WorkerCount;
            this.Targets = this.Configuration.Targets;
        }

        protected override void StartImpl()
        {
            this._streamingThread.Start();
            this._workers = 1.UpTo(this.WorkerCount)
                .Select(_ => new Thread(Fetch).Apply(t => t.IsBackground = true))
                .ToList();
            this._workers.ForEach(t => t.Start());
        }

        protected override void StopImpl()
        {
            this._streamingThread.Abort();
            this._workers.ForEach(t => t.Abort());
        }

        protected override void Dispose(bool disposing)
        {
            this._retrieveMutex.Dispose();
        }

        private void EnqueueFromStream()
        {
            this.Host.ModuleManager.GetModule<StorageModule>(this.StorageName).ObjectCreated
                .OfType<Activity>()
                .Select(GetUnit)
                .Where(t => t != null)
                .Run(this._immediateQueue.Enqueue);
        }

        private IEnumerable<Tuple<Activity, Uri>> GetUnits()
        {
            return this.Targets.SelectMany(t => this.Host.ModuleManager.GetModule<StorageModule>(this.StorageName)
                .GetActivities(
                    t.Item1.Item1,
                    t.Item1.Item2,
                    t.Item1.Item3,
                    t.Item1.Item4,
                    t.Item1.Item5,
                    t.Item1.Item6,
                    DBNull.Value
                )
                .Where(ExpressionGenerator.ParseLambda<Activity, Boolean>(t.Item2).Compile())
                .Select(a => Tuple.Create(a, new Uri(ExpressionGenerator.ParseLambda<Activity, String>(t.Item3).Compile()(a))))
            );
        }

        private Tuple<Activity, Uri> GetUnit(Activity activity)
        {
            return this.Targets
                .Where(t =>
                    (t.Item1.Item1 == null || activity.AccountId == t.Item1.Item1) &&
                    (t.Item1.Item2 == null || activity.Timestamp == t.Item1.Item2) &&
                    (t.Item1.Item3 == null || activity.Category == t.Item1.Item3) &&
                    (t.Item1.Item4 == null || activity.SubId == t.Item1.Item4) &&
                    (t.Item1.Item5 == null || activity.UserAgent == t.Item1.Item5) &&
                    (t.Item1.Item6 == null || activity.Value == (t.Item1.Item6 != DBNull.Value ? (String) t.Item1.Item6 : null)) &&
                    activity.Data == null &&
                    ExpressionGenerator.ParseLambda<Activity, Boolean>(t.Item2).Compile()(activity)
                )
                .Select(t => Tuple.Create(activity, new Uri(ExpressionGenerator.ParseLambda<Activity, String>(t.Item3).Compile()(activity))))
                .FirstOrDefault();
        }

        private void Fetch()
        {
            this.Host.ModuleManager.GetModule<StorageModule>(this.StorageName).Execute(s =>
            {
                using (WebClient client = new WebClient())
                {
                    while (true)
                    {
                        Tuple<Activity, Uri> unit;
                        if (this._immediateQueue.TryDequeue(out unit) || this._queue.TryDequeue(out unit))
                        {
                            if (unit.Item1.Data == null)
                            {
                                try
                                {
                                    unit.Item1.Data = client.DownloadData(unit.Item2);
                                    this.Log.Debug("Fetched activity data resource: {0}", unit.Item2.AbsoluteUri);
                                }
                                catch (WebException ex)
                                {
                                    // Write invalid data not to refetch
                                    unit.Item1.Data = new byte[0];
                                    this.Log.Debug("Failed to fetch activity data resource: " + unit.Item2.AbsoluteUri, ex);
                                }
                                s.TryUpdate();
                            }
                        }
                        else
                        {
                            if (this._retrieveMutex.WaitOne(0))
                            {
                                this.GetUnits().ForEach(this._queue.Enqueue);
                                this._retrieveMutex.ReleaseMutex();
                            }
                            Thread.Sleep(30000);
                        }
                    }
                }
            });
        }
    }
}