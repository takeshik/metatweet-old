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

namespace XSpect.MetaTweet.Requesting
{
    /// <summary>
    /// リクエスト タスクの状態を表します。
    /// </summary>
    [Flags()]
    public enum RequestTaskState
        : int
    {
        /// <summary>
        /// リクエストの状態は不明です。
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// リクエストは初期化され、まだ開始要求が行われていません。
        /// </summary>
        Initialized = 1,
        /// <summary>
        /// リクエストは開始が要求され、実際に開始されるまで待機しています。
        /// </summary>
        WaitForStart = 10,
        /// <summary>
        /// リクエストは現在実行されています。
        /// </summary>
        Running = 2,
        /// <summary>
        /// リクエストは一時停止が要求され、現在一時停止が可能な地点まで実行が到達するのを待っています。
        /// </summary>
        WaitForPause = 20,
        /// <summary>
        /// リクエストは一時停止しています。
        /// </summary>
        Paused = 4,
        /// <summary>
        /// リクエストは再開が要求され、実際に再開されるまで待機しています。
        /// </summary>
        WaitForContinue = 40,
        /// <summary>
        /// リクエストは成功して終了しました。
        /// </summary>
        Succeeded = 100,
        /// <summary>
        /// リクエストは失敗して終了しました。
        /// </summary>
        Failed = 200,
        /// <summary>
        /// リクエストは中断されて終了しました。
        /// </summary>
        Canceled = 400,
    }
}