// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * DataFetcherServant
 *   MetaTweet Servant to fetch Web resources for filling data of activity
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
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
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Achiral;
using Achiral.Extension;
using XSpect.MetaTweet.Objects;
using XSpect.Extension;

namespace XSpect.MetaTweet.Modules
{
    using Target = MutableTuple<String, String>;

    public class DataFetcherServant
        : ServantModule
    {
        private readonly ConcurrentQueue<Tuple<Activity, Uri>> _queue;

        private readonly ConcurrentQueue<Tuple<Activity, Uri>> _immediateQueue;

        private readonly Thread _streamingThread;

        private List<Thread> _workers;

        private readonly Mutex _retrieveMutex;

        public StorageModule Storage
        {
            get;
            private set;
        }

        public Int32 WorkerCount
        {
            get;
            private set;
        }

        public IEnumerable<Target> Targets
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
            this.Storage = this.Host.ModuleManager.GetModule<StorageModule>(this.Configuration.StorageName);
            this.WorkerCount = this.Configuration.WorkerCount;
            this.Targets = ((IList<Object>) this.Configuration.Targets).Cast<Target>();
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
            Observable.FromEvent<StorageObjectEventArgs>(this.Storage, "Created")
                .SelectMany(e => e.EventArgs.Objects)
                .OfType<Activity>()
                .Select(GetUnit)
                .Where(t => t != null)
                .Run(this._immediateQueue.Enqueue);
        }

        private IEnumerable<Tuple<Activity, Uri>> GetUnits()
        {
            using (StorageSession session = this.Storage.OpenSession())
            {
                return this.Targets.SelectMany(t => session.Query(StorageObjectDynamicQuery.Activity(t.Item1))
                    .Select(a => Tuple.Create(a, new Uri(TriDQL.ParseLambda<Activity, String>(t.Item2).Compile()(a))))
                );
            }
        }

        private Tuple<Activity, Uri> GetUnit(Activity activity)
        {
            return this.Targets
                .Where(t => StorageObjectDynamicQuery.Activity(t.Item1).Evaluate(EnumerableEx.Return(activity).AsQueryable()).Any())
                .Select(t => Tuple.Create(activity, new Uri(TriDQL.ParseLambda<Activity, String>(t.Item2).Compile()(activity))))
                .FirstOrDefault();
        }

        private void Fetch()
        {
            using (WebClient client = new WebClient())
            {
                while (true)
                {
                    Tuple<Activity, Uri> unit;
                    using (StorageSession session = this.Storage.OpenSession())
                    {
                        while (this._immediateQueue.TryDequeue(out unit) || this._queue.TryDequeue(out unit))
                        {
                            if (!unit.Item1["Data"].Any())
                            {
                                try
                                {
                                    // Activity.Act is not usable since unit.Item1.Context might be invalid.
                                    session.Create(unit.Item1.Account, unit.Item1.SelfAndAncestorIds, "Data", client.DownloadData(unit.Item2));
                                    this.Log.Debug("Fetched activity data resource: {0}", unit.Item2.AbsoluteUri);
                                }
                                catch (WebException ex)
                                {
                                    // Write invalid data not to refetch
                                    session.Create(unit.Item1.Account, unit.Item1.SelfAndAncestorIds, "Data", new Byte[0]);
                                    this.Log.Debug("Failed to fetch activity data resource: " + unit.Item2.AbsoluteUri, ex);
                                }
                            }
                        }
                        session.Update();
                    }
                    if (this._retrieveMutex.WaitOne(0))
                    {
                        this.GetUnits().ForEach(this._queue.Enqueue);
                        this._retrieveMutex.ReleaseMutex();
                    }
                    Thread.Sleep(30000);
                }
            }
        }
    }
}
