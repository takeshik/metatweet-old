// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetFoundation
 *   Common library to access MetaTweet platform
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetFoundation.
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

namespace XSpect.MetaTweet.Requesting
{
    /// <summary>
    /// <see cref="Request"/> および <see cref="IRequestTask"/> の管理を行ないます。
    /// </summary>
    public interface IRequestManager
        : IList<IRequestTask>,
          IDisposable,
          ILoggable
    {
        /// <summary>
        /// このオブジェクトを保持する <see cref="IServerCore"/> オブジェクトを取得します。
        /// </summary>
        /// <value>
        /// このオブジェクトを保持する <see cref="IServerCore"/> オブジェクト。
        /// </value>
        IServerCore Parent
        {
            get;
        }

        /// <summary>
        /// <see cref="IRequestTask"/> の ID の最大値を取得します。
        /// </summary>
        /// <value>
        /// <see cref="IRequestTask"/> の ID の最大値。
        /// </value>
        Int32 MaxRequestId
        {
            get;
        }

        /// <summary>
        /// <see cref="IRequestTask"/> を作成し、登録します。
        /// </summary>
        /// <param name="request">実行する <see cref="Request"/>。</param>
        /// <returns>作成され、登録された <see cref="IRequestTask"/>。</returns>
        IRequestTask Register(Request request);

        /// <summary>
        /// <see cref="IRequestTask"/> を作成、登録し、開始します。
        /// </summary>
        /// <typeparam name="TOutput">このタスクの出力の型。</typeparam>
        /// <param name="request">実行する <see cref="Request"/>。</param>
        /// <returns>作成、登録し、開始された <see cref="IRequestTask"/>。</returns>
        IRequestTask Start<TOutput>(Request request);

        /// <summary>
        /// <see cref="IRequestTask"/> を作成、登録し、開始します。
        /// </summary>
        /// <param name="request">実行する <see cref="Request"/>。</param>
        /// <param name="outputType">このタスクの出力の型を表すオブジェクト。</param>
        /// <returns>作成、登録し、開始された <see cref="IRequestTask"/>。</returns>
        IRequestTask Start(Request request, Type outputType);

        /// <summary>
        /// <see cref="IRequestTask"/> を作成、登録し、開始します。
        /// </summary>
        /// <param name="request">実行する <see cref="Request"/>。</param>
        /// <returns>作成、登録し、開始された <see cref="IRequestTask"/>。</returns>
        IRequestTask Start(Request request);

        /// <summary>
        /// <see cref="IRequestTask"/> を作成、登録、開始し、終了するまで待機します。
        /// </summary>
        /// <typeparam name="TOutput">このタスクの出力の型。</typeparam>
        /// <param name="request">実行する <see cref="Request"/>。</param>
        /// <returns>タスクの結果となる出力。</returns>
        TOutput Execute<TOutput>(Request request);

        /// <summary>
        /// <see cref="IRequestTask"/> を作成、登録、開始し、終了するまで待機します。
        /// </summary>
        /// <param name="outputType">このタスクの出力の型を表すオブジェクト。</param>
        /// <param name="request">実行する <see cref="Request"/>。</param>
        /// <returns>タスクの結果となる出力。</returns>
        Object Execute(Request request, Type outputType);

        /// <summary>
        /// <see cref="IRequestTask"/> を作成、登録、開始し、終了するまで待機します。
        /// </summary>
        /// <param name="request">実行する <see cref="Request"/>。</param>
        /// <returns>タスクの結果となる出力。</returns>
        Object Execute(Request request);

        /// <summary>
        /// 登録されている <see cref="IRequestTask"/> を削除します。
        /// </summary>
        /// <param name="task">削除する <see cref="IRequestTask"/>。</param>
        void Clean(IRequestTask task);

        /// <summary>
        /// 登録されている <see cref="IRequestTask"/> を全て削除します。
        /// </summary>
        /// <param name="cleanAll">終了していないタスクも含めて削除する場合は <c>true</c>。それ以外の場合は <c>false</c>。</param>
        void Clean(Boolean cleanAll);

        /// <summary>
        /// 終了した <see cref="IRequestTask"/> を全て削除します。
        /// </summary>
        void Clean();
    }
}