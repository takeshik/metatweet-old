// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetInterface
 *   Common interface library to communicate with MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetInterface.
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
using XSpect.MetaTweet.Modules;
using XSpect.MetaTweet.Requesting;

namespace XSpect.MetaTweet
{
    /// <summary>
    /// サーバ オブジェクトを表します。サーバ オブジェクトは、MetaTweet サーバ全体を表すオブジェクトで、他の全てのサーバ構造へのアクセスを提供します。このクラスは継承できません。
    /// </summary>
    public interface IServerCore
        : IDisposable,
          ILoggable
    {
        /// <summary>
        /// このサーバ オブジェクトが存在するアプリケーション ドメインを取得します。
        /// </summary>
        AppDomain MainAppDomain
        {
            get;
        }

        /// <summary>
        /// サーバ オブジェクトの生成時に渡されたパラメータのリストを取得します。
        /// </summary>
        /// <value>
        /// サーバ オブジェクトの生成時に渡されたパラメータのリスト。
        /// </value>
        IDictionary<String, String> Parameters
        {
            get;
        }

        /// <summary>
        /// MetaTweet システム全体の設定を保持するオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// MetaTweet システム全体の設定を保持するオブジェクト。
        /// </value>
        dynamic GlobalConfiguration
        {
            get;
        }

        /// <summary>
        /// MetaTweet サーバの設定を保持するオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// MetaTweet サーバの設定を保持するオブジェクト。
        /// </value>
        dynamic Configuration
        {
            get;
        }

        /// <summary>
        /// MetaTweet システムの特別なディレクトリを取得するためのオブジェクトを取得します。
        /// </summary>
        /// <value>
        /// MetaTweet システムの特別なディレクトリを取得するためのオブジェクト。
        /// </value>
        IDirectoryStructure Directories
        {
            get;
        }

        /// <summary>
        /// このサーバ オブジェクトのモジュール マネージャを取得します。
        /// </summary>
        /// <value>
        /// このサーバ オブジェクトのモジュール マネージャ。
        /// </value>
        IModuleManager ModuleManager
        {
            get;
        }

        /// <summary>
        /// このサーバ オブジェクトのリクエスト マネージャを取得します。
        /// </summary>
        /// <value>このサーバ オブジェクトのリクエスト マネージャ。</value>
        IRequestManager RequestManager
        {
            get;
        }

        /// <summary>
        /// このサーバ オブジェクトのストアド リクエスト マネージャを取得します。
        /// </summary>
        /// <value>このサーバ オブジェクトのストアド リクエスト マネージャ。</value>
        IStoredRequestManager StoredRequestManager
        {
            get;
        }

        /// <summary>
        /// このサーバ オブジェクトのログ マネージャを取得します。
        /// </summary>
        /// <value>
        /// このサーバ オブジェクトのログ マネージャ。
        /// </value>
        ILogManager LogManager
        {
            get;
        }

        /// <summary>
        /// 外部のコードを実行する際に与える既定のパラメータを取得します。
        /// </summary>
        /// <value>外部のコードを実行する際に与える既定のパラメータ。</value>
        IDictionary<String, Object> DefaultArgumentDictionary
        {
            get;
        }

        /// <summary>
        /// MetaTweet システムのバージョン情報を表す文字列を取得します。
        /// </summary>
        /// <value>MetaTweet システムのバージョン情報を表す文字列。</value>
        /// <remarks>このプロパティはバージョン情報の取得の他に、クライアントがサーバとの接続の確立を確認するために用いられることが想定されています。</remarks>
        String Version
        {
            get;
        }

        /// <summary>
        /// サーバ オブジェクトを初期化します。
        /// </summary>
        /// <param name="arguments">サーバ ホストからサーバ オブジェクトに渡す引数のリスト。</param>
        void Initialize(IDictionary<String, String> arguments);

        /// <summary>
        /// サーバおよびサーバント オブジェクトを開始します。
        /// </summary>
        void Start();

        /// <summary>
        /// サーバおよびサーバント オブジェクトを停止します。
        /// </summary>
        void Stop();

        /// <summary>
        /// サーバおよびサーバント オブジェクトを安全に停止します。
        /// </summary>
        void StopGracefully();
    }
}