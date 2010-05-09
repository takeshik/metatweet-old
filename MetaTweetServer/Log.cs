// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
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
using System.Runtime.Remoting;
using log4net;
using log4net.Config;
using XSpect.Extension;

namespace XSpect.MetaTweet
{
    [Serializable()]
    public class Log
        : MarshalByRefObject
    {
        private readonly ILog _log;

        public ServerCore Parent
        {
            get;
            private set;
        }

        public Log(ServerCore parent, FileInfo configFile)
        {
            this.Parent = parent;
            this._log = LogManager.GetLogger(typeof(ServerCore));
            XmlConfigurator.ConfigureAndWatch(this._log.Logger.Repository, configFile);
        }

        public void Debug(String format, params Object[] args)
        {
            this._log.DebugFormat(format, args);
        }

        public void Debug(String message, Exception exception)
        {
            this._log.Debug(message, exception);
        }

        public void Info(String format, params Object[] args)
        {
            this._log.InfoFormat(format, args);
        }

        public void Info(String message, Exception exception)
        {
            this._log.Info(message, exception);
        }

        public void Warn(String format, params Object[] args)
        {
            this._log.WarnFormat(format, args);
        }

        public void Warn(String message, Exception exception)
        {
            this._log.Warn(message, exception);
        }

        public void Error(String format, params Object[] args)
        {
            this._log.ErrorFormat(format, args);
        }

        public void Error(String message, Exception exception)
        {
            this._log.Error(message, exception);
        }

        public void Fatal(String format, params Object[] args)
        {
            this._log.FatalFormat(format, args);
        }

        public void Fatal(String message, Exception exception)
        {
            this._log.Fatal(message, exception);
        }
    }
}