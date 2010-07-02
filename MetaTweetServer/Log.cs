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
using System.Globalization;
using System.IO;
using System.Runtime.Remoting;
using log4net;
using log4net.Config;
using log4net.Core;
using XSpect.Extension;

namespace XSpect.MetaTweet
{
    [Serializable()]
    public class Log
        : MarshalByRefObject
    {
        private static readonly Type _thisDeclaringType = typeof(Log);

        private readonly ILogger _logger;

        private readonly Level _levelVerbose;

        private readonly Level _levelTrace;
        
        private readonly Level _levelDebug;
        
        private readonly Level _levelInfo;
        
        private readonly Level _levelNotice;
        
        private readonly Level _levelWarn;
        
        private readonly Level _levelError;
        
        private readonly Level _levelSevere;
        
        private readonly Level _levelCritical;
        
        private readonly Level _levelAlert;
        
        private readonly Level _levelFatal;
        
        private readonly Level _levelEmergency;

        public ServerCore Parent
        {
            get;
            private set;
        }

        public Log(ServerCore parent, FileInfo configFile)
        {
            this.Parent = parent;
            this._logger = LogManager.GetLogger(typeof(ServerCore)).Logger;
            XmlConfigurator.ConfigureAndWatch(this._logger.Repository, configFile);

            LevelMap map = this._logger.Repository.LevelMap;
            this._levelEmergency = map.LookupWithDefault(Level.Emergency);
            this._levelFatal = map.LookupWithDefault(Level.Fatal);
            this._levelAlert = map.LookupWithDefault(Level.Alert);
            this._levelCritical = map.LookupWithDefault(Level.Critical);
            this._levelSevere = map.LookupWithDefault(Level.Severe);
            this._levelError = map.LookupWithDefault(Level.Error);
            this._levelWarn = map.LookupWithDefault(Level.Warn);
            this._levelNotice = map.LookupWithDefault(Level.Notice);
            this._levelInfo = map.LookupWithDefault(Level.Info);
            this._levelDebug = map.LookupWithDefault(Level.Debug);
            this._levelTrace = map.LookupWithDefault(Level.Trace);
            this._levelVerbose = map.LookupWithDefault(Level.Verbose);
        }

        public void Verbose(String format, params Object[] args)
        {
            this.WriteLog(this._levelVerbose, String.Format(CultureInfo.InvariantCulture, format, args), null);
        }

        public void Verbose(String message, Exception exception)
        {
            this.WriteLog(this._levelVerbose, message, exception);
        }
        
        public void Trace(String format, params Object[] args)
        {
            this.WriteLog(this._levelTrace, String.Format(CultureInfo.InvariantCulture, format, args), null);
        }
        
        public void Trace(String message, Exception exception)
        {
            this.WriteLog(this._levelTrace, message, exception);
        }
        
        public void Debug(String format, params Object[] args)
        {
            this.WriteLog(this._levelDebug, String.Format(CultureInfo.InvariantCulture, format, args), null);
        }
        
        public void Debug(String message, Exception exception)
        {
            this.WriteLog(this._levelDebug, message, exception);
        }
        
        public void Info(String format, params Object[] args)
        {
            this.WriteLog(this._levelInfo, String.Format(CultureInfo.InvariantCulture, format, args), null);
        }
        
        public void Info(String message, Exception exception)
        {
            this.WriteLog(this._levelInfo, message, exception);
        }
        
        public void Notice(String format, params Object[] args)
        {
            this.WriteLog(this._levelNotice, String.Format(CultureInfo.InvariantCulture, format, args), null);
        }
        
        public void Notice(String message, Exception exception)
        {
            this.WriteLog(this._levelNotice, message, exception);
        }
        
        public void Warn(String format, params Object[] args)
        {
            this.WriteLog(this._levelWarn, String.Format(CultureInfo.InvariantCulture, format, args), null);
        }
        
        public void Warn(String message, Exception exception)
        {
            this.WriteLog(this._levelWarn, message, exception);
        }
        
        public void Error(String format, params Object[] args)
        {
            this.WriteLog(this._levelError, String.Format(CultureInfo.InvariantCulture, format, args), null);
        }
        
        public void Error(String message, Exception exception)
        {
            this.WriteLog(this._levelError, message, exception);
        }
        
        public void Severe(String format, params Object[] args)
        {
            this.WriteLog(this._levelSevere, String.Format(CultureInfo.InvariantCulture, format, args), null);
        }
        
        public void Severe(String message, Exception exception)
        {
            this.WriteLog(this._levelSevere, message, exception);
        }
        
        public void Critical(String format, params Object[] args)
        {
            this.WriteLog(this._levelCritical, String.Format(CultureInfo.InvariantCulture, format, args), null);
        }
        
        public void Critical(String message, Exception exception)
        {
            this.WriteLog(this._levelCritical, message, exception);
        }
        
        public void Alert(String format, params Object[] args)
        {
            this.WriteLog(this._levelAlert, String.Format(CultureInfo.InvariantCulture, format, args), null);
        }
        
        public void Alert(String message, Exception exception)
        {
            this.WriteLog(this._levelAlert, message, exception);
        }
        
        public void Fatal(String format, params Object[] args)
        {
            this.WriteLog(this._levelFatal, String.Format(CultureInfo.InvariantCulture, format, args), null);
        }
        
        public void Fatal(String message, Exception exception)
        {
            this.WriteLog(this._levelFatal, message, exception);
        }
        
        public void Emergency(String format, params Object[] args)
        {
            this.WriteLog(this._levelEmergency, String.Format(CultureInfo.InvariantCulture, format, args), null);
        }
        
        public void Emergency(String message, Exception exception)
        {
            this.WriteLog(this._levelEmergency, message, exception);
        }

        protected virtual void WriteLog(Level level, String message, Exception exception)
        {
            this._logger.Log(_thisDeclaringType, level, message, exception);
        }
    }
}