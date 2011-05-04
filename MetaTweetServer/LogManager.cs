﻿// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
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
using System.Collections.Generic;
using System.IO;
using log4net;
using log4net.Config;
using log4net.Core;
using log4net.Repository;
using XSpect.Extension;

namespace XSpect.MetaTweet
{
    /// <summary>
    /// ログ出力の機能を提供します。
    /// </summary>
    [Serializable()]
    public class LogManager
        : MarshalByRefObject,
          ILogManager
    {
        private readonly ILoggerRepository _repository;

        private readonly Dictionary<String, Log> _logs;

        public IServerCore Parent
        {
            get;
            private set;
        }

        public ILog this[String name]
        {
            get
            {
                return this._logs.ContainsKey(name)
                    ? this._logs[name]
                    : this._repository.Exists(name)
                          .Null(l => new Log(this, l).Apply(_ => this._logs.Add(name, _)));
            }
        }

        public LogManager(ServerCore parent, FileInfo configFile)
        {
            this._logs = new Dictionary<String, Log>();
            this.Parent = parent;
            XmlConfigurator.ConfigureAndWatch(configFile);
            this._repository = log4net.LogManager.GetRepository();
        }

        public override Object InitializeLifetimeService()
        {
            return null;
        }
    }
}