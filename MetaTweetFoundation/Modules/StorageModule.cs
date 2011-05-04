// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetServer.
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
using System.IO;
using System.Linq;
using System.Collections.Generic;
using XSpect.MetaTweet.Objects;
using XSpect.MetaTweet.Properties;

namespace XSpect.MetaTweet.Modules
{
    /// <summary>
    /// ストレージ モジュールの抽象基本クラスを提供します。
    /// </summary>
    /// <remarks>
    /// ストレージ モジュールとは、ストレージの機能を提供するモジュールです。即ち、<see cref="Storage"/> にモジュールに必要な機能を実装したクラスです。
    /// </remarks>
    [Serializable()]
    public class StorageModule
        : Module
    {
        private IDictionary<String, Object> _connectionSettings;

        public Storage Storage
        {
            get;
            protected set;
        }

        public event EventHandler<StorageSessionEventArgs> Opened;

        public event EventHandler<StorageSessionEventArgs> Closed;

        public event EventHandler<StorageObjectEventArgs> Queried;

        public event EventHandler<StorageObjectEventArgs> Loaded;

        public event EventHandler<StorageObjectEventArgs> Created;

        public event EventHandler<StorageObjectEventArgs> Deleted;

        public event EventHandler<StorageObjectEventArgs> Updated;

        public StorageModule(Storage storage)
        {
            this.Storage = storage;
            this.Storage.Opened += (sender, e) =>
            {
                if (this.Opened != null)
                {
                    this.Opened(sender, e);
                }
            };
            this.Storage.Closed += (sender, e) =>
            {
                if (this.Closed != null)
                {
                    this.Closed(sender, e);
                }
            };
            this.Storage.Queried += (sender, e) =>
            {
                if (this.Queried != null)
                {
                    this.Queried(sender, e);
                }
            };
            this.Storage.Loaded += (sender, e) =>
            {
                if (this.Loaded != null)
                {
                    this.Loaded(sender, e);
                }
            };
            this.Storage.Created += (sender, e) =>
            {
                if (this.Created != null)
                {
                    this.Created(sender, e);
                }
            };
            this.Storage.Deleted += (sender, e) =>
            {
                if (this.Deleted != null)
                {
                    this.Deleted(sender, e);
                }
            };
            this.Storage.Updated += (sender, e) =>
            {
                if (this.Updated != null)
                {
                    this.Updated(sender, e);
                }
            };
        }

        /// <summary>
        /// 派生クラスで実装された場合、実際の設定処理を行います。
        /// </summary>
        /// <param name="configFile">設定ファイル。</param>
        protected override void ConfigureImpl(FileInfo configFile)
        {
            base.ConfigureImpl(configFile);
            this._connectionSettings = this.Configuration.ConnectionSettings;
        }

        protected override void Dispose(Boolean disposing)
        {
            this.Storage.Dispose();
            base.Dispose(disposing);
        }

        protected override void InitializeImpl()
        {
            base.InitializeImpl();
            this.Opened += (sender, e) => this.Log.Debug(
                Resources.StorageOpenedSession,
                this.Name,
                e.SessionId.ToString("d")
            );
            this.Closed += (sender, e) => this.Log.Debug(
                Resources.StorageClosedSession,
                this.Name,
                e.SessionId.ToString("d")
            );
            this.Storage.Initialize(this._connectionSettings);
        }

        public virtual StorageSession OpenSession()
        {
            StorageSession session = this.Storage.OpenSession();
            session.Queried += (sender, e) => this.Log.Trace(
                Resources.StorageQueried,
                this.Name,
                e.SessionId.ToString("d"),
                Indent(e.Description),
                e.Objects.Count
            );
            session.Loaded += (sender, e) => this.Log.Verbose(
                Resources.StorageLoaded,
                this.Name,
                e.SessionId.ToString("d"),
                Indent(e.Description),
                e.Objects.Count
            );
            session.Created += (sender, e) => this.Log.Trace(
                Resources.StorageCreated,
                this.Name,
                e.SessionId.ToString("d"),
                Indent(e.Description)
            );
            session.Deleted += (sender, e) => this.Log.Verbose(
                Resources.StorageDeleted,
                this.Name,
                e.SessionId.ToString("d"),
                Indent(e.Description)
            );
            session.Updated += (sender, e) => this.Log.Debug(
                Resources.StorageUpdated,
                this.Name,
                e.SessionId.ToString("d")
            );
            return session;
        }

        public virtual void CloseSession(Guid id)
        {
            this.Storage.CloseSession(id);
        }

        private static String Indent(String str)
        {
            return String.Join(Environment.NewLine, str.Split(new String[] { Environment.NewLine, }, StringSplitOptions.None).Select(l => "    " + l));
        }
    }
}