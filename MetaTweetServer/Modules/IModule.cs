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
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Runtime.Remoting;
using XSpect.Hooking;

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
        /// このモジュールが生成されたドメインを取得します。
        /// </summary>
        /// <value>
        /// このモジュールが生成されたドメイン。
        /// </value>
        ModuleDomain Domain
        {
            get;
        }

        /// <summary>
        /// このモジュールに設定された名前を取得します。
        /// </summary>
        /// <value>
        /// このモジュールに設定された名前。
        /// </value>
        String Name
        {
            get;
        }

        /// <summary>
        /// このモジュールに渡されたオプションのリストを取得します。
        /// </summary>
        /// <value>
        /// このモジュールに渡されたオプションのリスト。
        /// </value>
        IList<String> Options
        {
            get;
        }

        /// <summary>
        /// <see cref="Initialize()"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="Initialize()"/> のフック リスト。
        /// </value>
        ActionHook<IModule> InitializeHook
        {
            get;
        }

        /// <summary>
        /// <see cref="Configure(FileInfo)"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="Configure(FileInfo)"/> のフック リスト。
        /// </value>
        ActionHook<IModule, FileInfo> ConfigureHook
        {
            get;
        }

        /// <summary>
        /// <see cref="IDisposable.Dispose()"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="IDisposable.Dispose()"/> のフック リスト。
        /// </value>
        ActionHook<IModule> DisposeHook
        {
            get;
        }

        /// <summary>
        /// このモジュールの設定を保持するオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// このモジュールの設定を保持するオブジェクト。
        /// </value>
        dynamic Configuration
        {
            get;
        }

        /// <summary>
        /// このモジュールをサーバ オブジェクトに登録します。
        /// </summary>
        /// <param name="domain">登録されるモジュール ドメイン。</param>
        /// <param name="name">モジュールに設定する名前。</param>
        /// <param name="options">モジュールに渡されたオプションのリスト。</param>
        void Register(ModuleDomain domain, String name, IList<String> options);

        /// <summary>
        /// このモジュールの設定を行います。
        /// </summary>
        /// <param name="configFile">設定を保持するファイル。</param>
        void Configure(FileInfo configFile);

        /// <summary>
        /// このモジュールを初期化します。
        /// </summary>
        /// <remarks>
        /// このメソッドはモジュールの寿命中、複数回呼び出される可能性があります。
        /// </remarks>
        void Initialize();

        /// <summary>
        /// リモート オブジェクトとの通信に使用するプロキシの生成に必要な情報をすべて格納しているオブジェクトを作成します。
        /// </summary>
        /// <returns>プロキシを生成するのに必要な情報。</returns>
        ObjRef CreateObjRef();
    }
}