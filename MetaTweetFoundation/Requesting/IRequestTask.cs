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
    /// リクエストを実行し、処理を行うタスクを表します。
    /// </summary>
    public interface IRequestTask
        : ILoggable
    {
        /// <summary>
        /// 監査用のイベントを記録するアクセス ログ ライタを取得します。
        /// </summary>
        /// <value>監査用のイベントを記録するアクセス ログ ライタを取得します。</value>
        ILog AccessLog
        {
            get;
        }

        /// <summary>
        /// このタスクが所属する <see cref="RequestManager"/> を取得します。
        /// </summary>
        /// <value>このタスクが所属する <see cref="RequestManager"/>。</value>
        IRequestManager Parent
        {
            get;
        }

        /// <summary>
        /// このタスクの ID を取得します。
        /// </summary>
        /// <value>このタスクの ID。</value>
        Int32 Id
        {
            get;
        }

        /// <summary>
        /// このタスクが実行するリクエストを表すオブジェクトを取得します。
        /// </summary>
        /// <value>このタスクが実行するリクエストを表すオブジェクト</value>
        Request Request
        {
            get;
        }

        /// <summary>
        /// 現在実行が行われている部分を表す <see cref="Request"/> の断片を取得します。
        /// </summary>
        /// <value>現在実行が行われている部分を表す <see cref="Request"/> の断片。</value>
        Fragment CurrentFragment
        {
            get;
        }

        /// <summary>
        /// このタスクの実行結果の型を表すオブジェクトを取得します。
        /// </summary>
        /// <value>このタスクの実行結果の型を表すオブジェクト。</value>
        Type OutputType
        {
            get;
        }

        /// <summary>
        /// このタスクの追加の実行結果を取得します。
        /// </summary>
        /// <value>このタスクの追加の実行結果。</value>
        IDictionary<String, Object> Variables
        {
            get;
        }

        /// <summary>
        /// このタスクの現在の状態を取得します。
        /// </summary>
        /// <value>このタスクの現在の状態。</value>
        RequestTaskState State
        {
            get;
        }

        /// <summary>
        /// このタスクが実行するリクエストが現在実行している断片の位置を取得します。
        /// </summary>
        /// <value>このタスクが実行するリクエストが現在実行している断片の位置。</value>
        Int32 StepCount
        {
            get;
        }

        /// <summary>
        /// このタスクの実行が開始された時刻を取得します。
        /// </summary>
        /// <value>このタスクの実行が開始された時刻。</value>
        Nullable<DateTime> StartTime
        {
            get;
        }

        /// <summary>
        /// このタスクの実行が終了した時刻を取得します。
        /// </summary>
        /// <value>このタスクの実行が終了した時刻。</value>
        Nullable<DateTime> ExitTime
        {
            get;
        }

        /// <summary>
        /// このタスクの実行時間を取得します。
        /// </summary>
        /// <valu>このタスクの実行時間。</valu>
        TimeSpan ElapsedTime
        {
            get;
        }

        /// <summary>
        /// このタスクが終了しているかどうかを表す値を取得します。
        /// </summary>
        /// <value>このタスクが終了している場合は <c>true</c>。それ以外の場合は <c>false</c>。</value>
        Boolean HasExited
        {
            get;
        }

        /// <summary>
        /// タスクの開始を要求します。
        /// </summary>
        /// <param name="outputType">このタスクの出力の型を表すオブジェクト。</param>
        void Start(Type outputType);

        /// <summary>
        /// タスクの開始を要求します。
        /// </summary>
        /// <typeparam name="TOutput">このタスクの出力の型。</typeparam>
        void Start<TOutput>();

        /// <summary>
        /// タスクの一時停止を要求します。
        /// </summary>
        void Pause();

        /// <summary>
        /// タスクの再開を要求します。
        /// </summary>
        void Continue();

        /// <summary>
        /// タスクが終了するまで待機します。
        /// </summary>
        void Wait();

        /// <summary>
        /// タスクが終了するまで、または指定された時間が経過するまで待機します。
        /// </summary>
        /// <param name="millisecondsTimeout">タスクが終了するまでの待機時間を表すミリ秒。</param>
        /// <returns>スレッドが終了した場合は <c>true</c>。<paramref name="millisecondsTimeout"/> パラメータで指定した時間が経過してもスレッドが終了していない場合は <c>false</c>。</returns>
        Boolean Wait(Int32 millisecondsTimeout);

        /// <summary>
        /// タスクが終了するまで、または指定された時間が経過するまで待機します。
        /// </summary>
        /// <param name="timeout">タスクが終了するまでの待機時間に設定される <see cref="TimeSpan"/>。</param>
        /// <returns>スレッドが終了した場合は <c>true</c>。<paramref name="timeout"/> パラメータで指定した時間が経過してもスレッドが終了していない場合は <c>false</c>。</returns>
        Boolean Wait(TimeSpan timeout);

        /// <summary>
        /// タスクを中断します。
        /// </summary>
        void Cancel();

        /// <summary>
        /// タスクを破棄します。
        /// </summary>
        void Kill();

        /// <summary>
        /// タスクの結果となる出力を返します。。
        /// </summary>
        /// <param name="outputType">タスクの出力の型を表すオブジェクト。</param>
        /// <returns>タスクの結果となる出力。</returns>
        Object GetOutput(Type outputType);

        /// <summary>
        /// タスクの結果となる出力を返します。
        /// </summary>
        /// <typeparam name="TOutput">タスクの出力の型。</typeparam>
        /// <returns>タスクの結果となる出力。</returns>
        TOutput GetOutput<TOutput>();

        /// <summary>
        /// タスクの開始を要求し、終了するまで待機し、結果を返します。
        /// </summary>
        /// <param name="outputType">タスクの出力の型を表すオブジェクト。</param>
        /// <returns>タスクの結果となる出力。</returns>
        Object Execute(Type outputType);

        /// <summary>
        /// タスクの開始を要求し、終了するまで待機し、結果を返します。
        /// </summary>
        /// <typeparam name="TOutput">タスクの出力の型。</typeparam>
        /// <returns>タスクの結果となる出力。</returns>
        TOutput Execute<TOutput>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputType"></param>
        /// <returns></returns>
        Object End(Type outputType);

        /// <summary>
        /// タスクの開始を要求し、終了するまで待機し、結果を返します。終了したタスクは <see cref="RequestManager"/> から削除されます。
        /// </summary>
        /// <typeparam name="TOutput">タスクの出力の型。</typeparam>
        /// <returns>タスクの結果となる出力。</returns>
        TOutput End<TOutput>();

        /// <summary>
        /// このタスクを <see cref="RequestManager"/> から削除します。
        /// </summary>
        void Clean();
    }
}