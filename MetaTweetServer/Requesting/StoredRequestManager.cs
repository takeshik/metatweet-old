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
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Achiral.Extension;
using log4net;
using XSpect.Collections;
using XSpect.Extension;

namespace XSpect.MetaTweet.Requesting
{
    /// <summary>
    /// <see cref="StoredRequest"/> を管理し、実行する機能を提供します。
    /// </summary>
    public class StoredRequestManager
        : MarshalByRefObject
    {
        /// <summary>
        /// このオブジェクトを保持する <see cref="ServerCore"/> オブジェクトを取得します。
        /// </summary>
        /// <value>
        /// このオブジェクトを保持する <see cref="ServerCore"/> オブジェクト。
        /// </value>
        public ServerCore Parent
        {
            get;
            private set;
        }

        /// <summary>
        /// このオブジェクトの設定を保持するオブジェクトを取得します。
        /// </summary>
        /// <value>このオブジェクトの設定を保持するオブジェクト。</value>
        public dynamic Configuration
        {
            get;
            private set;
        }

        /// <summary>
        /// 定義されている <see cref="StoredRequest"/> の一覧を取得します。
        /// </summary>
        public HybridDictionary<String, StoredRequest> StoredRequests
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="DirectoryStructure"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="parent">このオブジェクトを生成する、親となるオブジェクト。</param>
        /// <param name="configFile">設定ファイル。</param>
        public StoredRequestManager(ServerCore parent, FileInfo configFile)
        {
            this.Parent = parent;
            this.Configuration = this.Parent.ModuleManager.Execute(configFile, self => this, host => this.Parent);
            this.StoredRequests = new HybridDictionary<string, StoredRequest>((i, e) => e.Name);
            this.StoredRequests.AddRange(((IList<Object>) this.Configuration.StoredRequests).Cast<StoredRequest>());
        }

        public override Object InitializeLifetimeService()
        {
            return null;
        }

        /// <summary>
        /// <see cref="StoredRequest"/> を実行します。
        /// </summary>
        /// <typeparam name="TOutput">実行する <see cref="StoredRequest"/> の出力の型。</typeparam>
        /// <param name="name">実行する <see cref="StoredRequest"/> の名前。</param>
        /// <param name="args">実行する <see cref="StoredRequest"/> に与える引数。</param>
        /// <returns><see cref="StoredRequest"/> の結果となる出力。</returns>
        public TOutput Execute<TOutput>(String name, IDictionary<String, String> args)
        {
            return this.Parent.RequestManager.Execute<TOutput>(this.StoredRequests[name].Apply(args));
        }

        /// <summary>
        /// <see cref="StoredRequest"/> を実行します。
        /// </summary>
        /// <param name="name">実行する <see cref="StoredRequest"/> の名前。</param>
        /// <param name="args">実行する <see cref="StoredRequest"/> に与える引数。</param>
        /// <param name="outputType">実行する <see cref="StoredRequest"/> の出力の型を表すオブジェクト。</param>
        /// <returns><see cref="StoredRequest"/> の結果となる出力。</returns>
        public Object Execute(String name, IDictionary<String, String> args, Type outputType)
        {
            return this.Parent.RequestManager.Execute(this.StoredRequests[name].Apply(args), outputType);
        }

        /// <summary>
        /// <see cref="StoredRequest"/> を実行します。
        /// </summary>
        /// <param name="name">実行する <see cref="StoredRequest"/> の名前。</param>
        /// <param name="args">実行する <see cref="StoredRequest"/> に与える引数。</param>
        /// <returns><see cref="StoredRequest"/> の結果となる出力。</returns>
        public Object Execute(String name, IDictionary<String, String> args)
        {
            return this.Execute(name, args, null);
        }
    }
}