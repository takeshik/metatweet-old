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

using System.IO;
using log4net;
using log4net.Config;

namespace XSpect.MetaTweet
{
    /// <summary>
    /// オブジェクトがイベントの記録に使用できるログ ライタを提供します。
    /// </summary>
    public static class Loggable
    {
        /// <summary>
        /// ログ ライタを初期化します。
        /// </summary>
        /// <typeparam name="T">ログの採取を行う型。</typeparam>
        /// <param name="configFile">ログ ライタを構成するための設定ファイル。</param>
        /// <returns>生成されたログ ライタ。</returns>
        public static ILog Initialize<T>(FileInfo configFile)
        {
            ILog log = LogManager.GetLogger(typeof(T));
            XmlConfigurator.ConfigureAndWatch(log.Logger.Repository, configFile);
            return log;
        }
    }
}