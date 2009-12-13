// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright c 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
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
using XSpect.Configuration;

namespace XSpect.MetaTweet.Modules
{
    /// <summary>
    /// 全てのモジュールによって要求される機能を提供します。
    /// </summary>
    /// <remarks>
    /// モジュールとは、MetaTweet サーバにおける機能の単位で、動的にロードおよびアンロードできるオブジェクトです。全てのモジュールは MetaTweet サーバへの完全なアクセスを持ちます。
    /// </remarks>
    public interface IModule
        : IDisposable,
          ILoggable
    {
        /// <summary>
        /// このモジュールがホストされているサーバ オブジェクトを取得します。
        /// </summary>
        /// <value>
        /// このモジュールがホストされているサーバ オブジェクト。
        /// </value>
        ServerCore Host
        {
            get;
        }

        /// <summary>
        /// このモジュールに設定された名前を取得します。
        /// </summary>
        /// <value>
        /// このモジュールに設定された名前を取得します。
        /// </value>
        String Name
        {
            get;
        }

        /// <summary>
        /// <see cref="Initialize(XmlConfiguration)"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="Initialize(XmlConfiguration)"/> のフック リスト。
        /// </value>
        Hook<IModule, XmlConfiguration> InitializeHook
        {
            get;
        }

        /// <summary>
        /// このモジュールの設定を管理するオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// このモジュールの設定を管理するオブジェクト。
        /// </value>
        XmlConfiguration Configuration
        {
            get;
        }

        /// <summary>
        /// このモジュールをサーバ オブジェクトに登録します。
        /// </summary>
        /// <param name="host">登録されるサーバ オブジェクト。</param>
        /// <param name="name">モジュールに設定する名前。</param>
        void Register(ServerCore host, String name);

        /// <summary>
        /// このモジュールに設定を適用し、初期化を行います。
        /// </summary>
        /// <param name="configuration">適用する設定。</param>
        void Initialize(XmlConfiguration configuration);

        /// <summary>
        /// このモジュールを初期化します。
        /// </summary>
        /// <remarks>
        /// このメソッドはモジュールの寿命中、複数回呼び出される可能性があります。
        /// </remarks>
        void Initialize();
    }
}