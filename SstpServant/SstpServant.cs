// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * SstpServant
 *   MetaTweet Servant which provides SSTP (Sakura Script Transfer Protocol) client feature
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of SstpServant.
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
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using XSpect.MetaTweet.Objects;
using XSpect.Extension;

namespace XSpect.MetaTweet.Modules
{
    public class SstpServant
        : ServantModule
    {
        private readonly Thread _thread;

        public String ServerAddress
        {
            get;
            private set;
        }

        public Int32 ServerPort
        {
            get;
            private set;
        }

        public StorageModule Storage
        {
            get;
            private set;
        }

        public SstpServant()
        {
            this._thread = new Thread(this.Notify);
        }

        protected override void ConfigureImpl(FileInfo configFile)
        {
            base.ConfigureImpl(configFile);
            this.ServerAddress = this.Configuration.ServerAddress;
            this.ServerPort = this.Configuration.ServerPort;
            this.Storage = this.Host.ModuleManager.GetModule<StorageModule>(this.Configuration.StorageName);
        }

        protected override void StartImpl()
        {
            this._thread.Start();
        }

        protected override void StopImpl()
        {
            this._thread.Abort();
        }

        private void Notify()
        {
            Observable.FromEvent<StorageObjectEventArgs>(this.Storage, "Created")
                .Select(e => e.EventArgs.Object)
                .OfType<Activity>()
                .Where(a => a.Name == "Status")
                .Where(a => (a["Body"].FirstOrDefault().TryGetValue<String>() ?? "").EndsWith(@"\e"))
                .Subscribe(this.Send);
            Thread.Sleep(Timeout.Infinite);
        }

        private void Send(Activity status)
        {
            using (TcpClient client = new TcpClient(this.ServerAddress, this.ServerPort))
            {
                String body = status["Body"].First().GetValue<String>();
                client.GetStream()
                    .Write(Encoding.UTF8.GetBytes(String.Format(
                        // NOTE: Below here document expects newline code of this source is CRLF.
                        #region SSTP
@"SEND SSTP/1.4
Sender: MetaTweet ({0})
{1}
Charset: UTF-8
",
                        #endregion
                        status.Account["ScreenName"].FirstOrDefault().TryGetValue<String>(),
                        Regex.Match(body, @"([\w,]+):\W*(.+)").If(
                            m => m.Success,
                            m => String.Format("IfGhost: {0}\r\nScript: {1}", m.Groups[1].Value, m.Groups[2].Value),
                            m => String.Format("Script: {0}", body)
                        )
                    )));
            }
        }
    }
}
