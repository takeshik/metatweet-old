// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetClient
 *   Simple GUI client for MetaTweet
 *   Part of MetaTweet
 * Copyright © 2009-2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetClient.
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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Windows.Forms;
using Achiral;
using Achiral.Extension;
using XSpect.Collections;
using XSpect.Configuration;
using XSpect.Extension;
using XSpect.MetaTweet.Objects;

namespace XSpect.MetaTweet.Clients.Client
{
    public sealed class ClientCore
        : MarshalByRefObject,
          IDisposable
    {
        private Boolean _disposed;

        private IChannel _channel;

        /// <summary>
        /// クライアント オブジェクトの生成時に渡されたパラメータのリストを取得します。
        /// </summary>
        /// <value>
        /// クライアント オブジェクトの生成時に渡されたパラメータのリスト。
        /// </value>
        public IDictionary<String, String> Parameters
        {
            get;
            private set;
        }

        /// <summary>
        /// MetaTweet システム全体の設定を管理するオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// MetaTweet システム全体の設定を管理するオブジェクト。
        /// </value>
        public XmlConfiguration GlobalConfiguration
        {
            get;
            private set;
        }

        public XmlConfiguration Configuration
        {
            get;
            private set;
        }

        public Configuration ConfigurationObject
        {
            get;
            private set;
        }

        /// <summary>
        /// MetaTweet システムの特別なディレクトリを取得するためのオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// MetaTweet システムの特別なディレクトリを取得するためのオブジェクト。
        /// </value>
        public DirectoryStructure Directories
        {
            get;
            private set;
        }

        public ServerCore Host
        {
            get;
            private set;
        }

        public IList<Activity> DataCache
        {
            get;
            private set;
        }

        public HybridDictionary<String, String> Filters
        {
            get;
            private set;
        }

        public MainForm MainForm
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="ClientCore"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public ClientCore()
        {
            this._channel = new TcpClientChannel();
            this.DataCache = new List<Activity>();
        }

        /// <summary>
        /// 対象のインスタンスの有効期間ポリシーを制御する、有効期間サービス オブジェクトを取得します。
        /// </summary>
        /// <returns>
        /// 対象のインスタンスの有効期間ポリシーを制御するときに使用する、<see cref="T:System.Runtime.Remoting.Lifetime.ILease"/> 型のオブジェクト。存在する場合は、このインスタンスの現在の有効期間サービス オブジェクトです。それ以外の場合は、<see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime"/> プロパティの値に初期化された新しい有効期間サービス オブジェクトです。
        /// </returns>
        /// <exception cref="T:System.Security.SecurityException">直前の呼び出し元に、インフラストラクチャ アクセス許可がありません。</exception>
        /// <PermissionSet>
        /// <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure"/>
        /// </PermissionSet>
        public override Object InitializeLifetimeService()
        {
            return null;
        }

        /// <summary>
        /// <see cref="ClientCore"/> によって使用されているすべてのリソースを解放します。
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// <see cref="ClientCore"/> によって使用されているアンマネージ リソースを解放し、オプションでマネージ リソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 <c>true</c>、破棄されない場合は <c>false</c>。</param>
        private void Dispose(Boolean disposing)
        {
            // FIXME: Modifies of ConfigurationObject reflects to default configuration file
            // TODO: Fix XmlConfiguration behavior or change configuration data structure
            this.Configuration.Save();
            this._disposed = true;
        }

        /// <summary>
        /// オブジェクトが破棄されているかどうかを確認し、破棄されている場合は例外を送出します。
        /// </summary>
        private void CheckIfDisposed()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }

        /// <summary>
        /// クライアント オブジェクトを初期化します。
        /// </summary>
        /// <param name="arguments">サーバ ホストからサーバ オブジェクトに渡す引数のリスト。</param>
        public void Initialize(IDictionary<String, String> arguments)
        {
            this.CheckIfDisposed();
            this.Parameters = arguments;
            if (this.Parameters.Contains(Create.KeyValuePair("debug", "true")))
            {
                Debugger.Launch();
            }

            this.GlobalConfiguration = XmlConfiguration.Load(this.Parameters["config"]);
            this.Directories = new DirectoryStructure(this.GlobalConfiguration.ResolveChild("directories"));
            this.Configuration = XmlConfiguration.Load(
                this.Directories.ConfigDirectory.CreateSubdirectory("Client").File("MetaTweetClient.conf.xml")
            );
            this.ConfigurationObject = this.Configuration.ResolveValue<Configuration>("configuration");
            this.Filters = this.Configuration.ResolveValue<HybridDictionary<String, String>>("filters");
        }

        public void Run()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            this.MainForm = new MainForm(this);
            Application.Run(this.MainForm);
        }

        public void Connect()
        {
            if (this.Host == null)
            {
                ChannelServices.RegisterChannel(this._channel, false);
                RemotingConfiguration.RegisterWellKnownClientType(typeof(ServerCore), this.ConfigurationObject.ServerAddress);
                this.Host = Activator.CreateInstance<ServerCore>();
            }
        }

        public void Disconnect()
        {
            if (this.Host != null)
            {
                this.Host = null;
                ChannelServices.UnregisterChannel(this._channel);
            }
        }

        public String TestConnection()
        {
            try
            {
                return this.Host.Version;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<Activity> FetchData()
        {
            return this.Host.RequestManager.Execute<IEnumerable<StorageObject>>(Request.Parse(this.ConfigurationObject.SendingRequest))
                .OfType<Activity>()
                .Except(this.DataCache)
                .Let(this.DataCache.AddRange);
        }

        public IList<Activity> ExecuteQuery(String query)
        {
            try
            {
                return query.Split(Environment.NewLine.ToCharArray())
                    .Select(s => "(?<method>where|select|orderby|take|skip): *(?<body>.+?)(?: params: *(?<params>.+))?$".RegexMatch(s))
                    .Select(m => new
                    {
                        Method = m.Groups["method"].Value.ToLower(),
                        Body = m.Groups["body"].Value,
                        Parameters = m.Groups["params"].Value.Split(',').Select(s => s.Trim(' ')).ToArray()
                    })
                    .ZipWith(Make.Repeat(this.DataCache.AsQueryable()), (_, q) =>
                    {
                        switch (_.Method)
                        {
                            case "where":
                                return q.Where(_.Body, _.Parameters);
                            case "select":
                                return q.Select(_.Body, _.Parameters);
                            case "orderby":
                                return q.OrderBy(_.Body, _.Parameters);
                            case "take":
                                return q.Take(_.Body, _.Parameters);
                            case "skip":
                                return q.Skip(_.Body, _.Parameters);
                            default:
                                throw new ArgumentException("Invalid method: " + _.Method);
                        }
                    })
                    .First()
                    .Cast<Activity>()
                    .ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
