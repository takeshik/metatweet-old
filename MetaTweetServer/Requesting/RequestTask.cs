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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using XSpect.Extension;
using XSpect.MetaTweet.Modules;
using XSpect.MetaTweet.Objects;
using XSpect.MetaTweet.Properties;

namespace XSpect.MetaTweet.Requesting
{
    /// <summary>
    /// リクエストを実行し、処理を行うタスクを表します。
    /// </summary>
    public class RequestTask
        : MarshalByRefObject,
          ILoggable
    {
        private readonly Thread _thread;

        private readonly AutoResetEvent _signal;

        private WeakReference _outputReference;

        private Object _outputValue;

        private readonly Object _lockObject;

        /// <summary>
        /// イベントを記録するログ ライタを取得します。
        /// </summary>
        /// <value>イベントを記録するログ ライタ。</value>
        public Log Log
        {
            get
            {
                return this.Parent.Parent.Let(
                    s => s.LogManager[s.Configuration.Loggers.RequestTask]
                );
            }
        }

        /// <summary>
        /// 監査用のイベントを記録するアクセス ログ ライタを取得します。
        /// </summary>
        /// <value>監査用のイベントを記録するアクセス ログ ライタを取得します。</value>
        public Log AccessLog
        {
            get
            {
                return this.Parent.Parent.Let(
                    s => s.LogManager[s.Configuration.Loggers.RequestTask_Access]
                );
            }
        }

        /// <summary>
        /// このタスクが所属する <see cref="RequestManager"/> を取得します。
        /// </summary>
        /// <value>このタスクが所属する <see cref="RequestManager"/>。</value>
        public RequestManager Parent
        {
            get;
            private set;
        }

        /// <summary>
        /// このタスクの ID を取得します。
        /// </summary>
        /// <value>このタスクの ID。</value>
        public Int32 Id
        {
            get;
            private set;
        }

        /// <summary>
        /// このタスクが実行するリクエストを表すオブジェクトを取得します。
        /// </summary>
        /// <value>このタスクが実行するリクエストを表すオブジェクト</value>
        public Request Request
        {
            get;
            private set;
        }

        /// <summary>
        /// 現在実行が行われている部分を表す <see cref="Request"/> の断片を取得します。
        /// </summary>
        /// <value>現在実行が行われている部分を表す <see cref="Request"/> の断片。</value>
        public Request CurrentRequestFragment
        {
            get
            {
                return this.Request.ElementAt(this.CurrentPosition);
            }
        }

        /// <summary>
        /// このタスクが実行するリクエストが含む実行単位となる断片の数を取得します。
        /// </summary>
        /// <value>このタスクが実行するリクエストが含む実行単位となる断片の数。</value>
        public Int32 RequestFragmentCount
        {
            get
            {
                return this.Request.Count();
            }
        }

        /// <summary>
        /// このタスクの実行結果の型を表すオブジェクトを取得します。
        /// </summary>
        /// <value>このタスクの実行結果の型を表すオブジェクト。</value>
        public Type OutputType
        {
            get;
            private set;
        }

        /// <summary>
        /// このタスクの追加の実行結果を取得します。
        /// </summary>
        /// <value>このタスクの追加の実行結果。</value>
        public IDictionary<String, Object> AdditionalData
        {
            get;
            private set;
        }

        /// <summary>
        /// このタスクの現在の状態を取得します。
        /// </summary>
        /// <value>このタスクの現在の状態。</value>
        public RequestTaskState State
        {
            get;
            private set;
        }

        /// <summary>
        /// このタスクが実行するリクエストが現在実行している断片の位置を取得します。
        /// </summary>
        /// <value>このタスクが実行するリクエストが現在実行している断片の位置。</value>
        public Int32 CurrentPosition
        {
            get;
            private set;
        }

        /// <summary>
        /// このタスクの実行が開始された時刻を取得します。
        /// </summary>
        /// <value>このタスクの実行が開始された時刻。</value>
        public Nullable<DateTime> StartTime
        {
            get;
            private set;
        }

        /// <summary>
        /// このタスクの実行が終了した時刻を取得します。
        /// </summary>
        /// <value>このタスクの実行が終了した時刻。</value>
        public Nullable<DateTime> ExitTime
        {
            get;
            private set;
        }

        /// <summary>
        /// このタスクの実行時間を取得します。
        /// </summary>
        /// <valu>このタスクの実行時間。</valu>
        public TimeSpan ElapsedTime
        {
            get
            {
                return this.State != RequestTaskState.Initialized
                    ? (this.HasExited
                          ? this.ExitTime.Value
                          : DateTime.UtcNow
                      ).Subtract(this.StartTime.Value)
                    : TimeSpan.Zero;
            }
        }

        /// <summary>
        /// このタスクが終了しているかどうかを表す値を取得します。
        /// </summary>
        /// <value>このタスクが終了している場合は <c>true</c>。それ以外の場合は <c>false</c>。</value>
        public Boolean HasExited
        {
            get
            {
                return
                    this.State == RequestTaskState.Succeeded ||
                    this.State == RequestTaskState.Failed ||
                    this.State == RequestTaskState.Canceled;
            }
        }

        /// <summary>
        /// <see cref="RequestTask"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="parent">所属させる <see cref="RequestManager"/>。</param>
        /// <param name="request">実行する <see cref="Request"/>。</param>
        public RequestTask(RequestManager parent, Request request)
        {
            this._signal = new AutoResetEvent(true);
            this._lockObject = new Object();
            this.Parent = parent;
            this.Id = this.Parent.GetNewId();
            this.Request = request;
            this.State = RequestTaskState.Initialized;
            this._thread = new Thread(() => this.Process())
            {
                Name = "RequestTask#" + this.Id,
                IsBackground = true,
            };
        }

        public override Object InitializeLifetimeService()
        {
            return null;
        }

        /// <summary>
        /// タスクの開始を要求します。
        /// </summary>
        /// <param name="outputType">このタスクの出力の型を表すオブジェクト。</param>
        public void Start(Type outputType)
        {
            lock (this._lockObject)
            {
                if (this.State == RequestTaskState.Initialized)
                {
                    this.OutputType = outputType;
                    this.State = RequestTaskState.WaitForStart;
                    // Stub for blocking situations at start
                    this.StartTime = DateTime.UtcNow;
                    this._thread.Start();
                }
            }
        }

        /// <summary>
        /// タスクの開始を要求します。
        /// </summary>
        /// <typeparam name="TOutput">このタスクの出力の型。</typeparam>
        public void Start<TOutput>()
        {
            this.Start(typeof(TOutput));
        }

        /// <summary>
        /// タスクの一時停止を要求します。
        /// </summary>
        public void Pause()
        {
            lock (this._lockObject)
            {
                if (this.State == RequestTaskState.Running)
                {
                    this.State = RequestTaskState.WaitForPause;
                    this._signal.Reset();
                }
            }
        }

        /// <summary>
        /// タスクの再開を要求します。
        /// </summary>
        public void Continue()
        {
            lock (this._lockObject)
            {
                if (this.State == RequestTaskState.Paused)
                {
                    this.State = RequestTaskState.WaitForContinue;
                    this._signal.Set();
                }
            }
        }

        /// <summary>
        /// タスクが終了するまで待機します。
        /// </summary>
        public void Wait()
        {
            lock (this._lockObject)
            {
                this._thread.Join();
            }
        }

        /// <summary>
        /// タスクが終了するまで、または指定された時間が経過するまで待機します。
        /// </summary>
        /// <param name="millisecondsTimeout">タスクが終了するまでの待機時間を表すミリ秒。</param>
        /// <returns>スレッドが終了した場合は <c>true</c>。<paramref name="millisecondsTimeout"/> パラメータで指定した時間が経過してもスレッドが終了していない場合は <c>false</c>。</returns>
        public Boolean Wait(Int32 millisecondsTimeout)
        {
            lock (this._lockObject)
            {
                return this._thread.Join(millisecondsTimeout);
            }
        }

        /// <summary>
        /// タスクが終了するまで、または指定された時間が経過するまで待機します。
        /// </summary>
        /// <param name="timeout">タスクが終了するまでの待機時間に設定される <see cref="TimeSpan"/>。</param>
        /// <returns>スレッドが終了した場合は <c>true</c>。<paramref name="timeout"/> パラメータで指定した時間が経過してもスレッドが終了していない場合は <c>false</c>。</returns>
        public Boolean Wait(TimeSpan timeout)
        {
            lock (this._lockObject)
            {
                return this._thread.Join(timeout);
            }
        }

        /// <summary>
        /// タスクを中断します。
        /// </summary>
        public void Cancel()
        {
            lock (this._lockObject)
            {
                this._thread.Abort();
            }
        }

        /// <summary>
        /// タスクを破棄します。
        /// </summary>
        public void Kill()
        {
            lock (this._lockObject)
            {
                this.Cancel();
                this.Clean();
            }
        }

        /// <summary>
        /// タスクの結果となる出力を返します。。
        /// </summary>
        /// <param name="outputType">タスクの出力の型を表すオブジェクト。</param>
        /// <returns>タスクの結果となる出力。</returns>
        public Object GetOutput(Type outputType)
        {
            lock (this._lockObject)
            {
                if (this.State != RequestTaskState.Succeeded)
                {
                    throw new InvalidOperationException("The task is not finished, or finished not successfully.", this._outputValue as Exception);
                }
                Object output = this._outputReference.Target;
                // Release strong reference
                this._outputValue = null;
                return output;
            }
        }

        /// <summary>
        /// タスクの結果となる出力を返します。
        /// </summary>
        /// <typeparam name="TOutput">タスクの出力の型。</typeparam>
        /// <returns>タスクの結果となる出力。</returns>
        public TOutput GetOutput<TOutput>()
        {
            return (TOutput) this.GetOutput(typeof(TOutput));
        }

        /// <summary>
        /// タスクの開始を要求し、終了するまで待機し、結果を返します。
        /// </summary>
        /// <param name="outputType">タスクの出力の型を表すオブジェクト。</param>
        /// <returns>タスクの結果となる出力。</returns>
        public Object Execute(Type outputType)
        {
            lock (this._lockObject)
            {
                this.Wait();
                return this.GetOutput(outputType);
            }
        }

        /// <summary>
        /// タスクの開始を要求し、終了するまで待機し、結果を返します。
        /// </summary>
        /// <typeparam name="TOutput">タスクの出力の型。</typeparam>
        /// <returns>タスクの結果となる出力。</returns>
        public TOutput Execute<TOutput>()
        {
            return (TOutput) this.Execute(typeof(TOutput));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputType"></param>
        /// <returns></returns>
        public Object End(Type outputType)
        {
            return this.Execute(outputType).Apply(_ => this.Clean());
        }

        /// <summary>
        /// タスクの開始を要求し、終了するまで待機し、結果を返します。終了したタスクは <see cref="RequestManager"/> から削除されます。
        /// </summary>
        /// <typeparam name="TOutput">タスクの出力の型。</typeparam>
        /// <returns>タスクの結果となる出力。</returns>
        public TOutput End<TOutput>()
        {
            return (TOutput) this.End(typeof(TOutput));
        }

        /// <summary>
        /// このタスクを <see cref="RequestManager"/> から削除します。
        /// </summary>
        public void Clean()
        {
            this.Parent.Clean(this);
        }

        private Object Process()
        {
            this.State = RequestTaskState.Running;
            StorageModule storageModule = null;
            StorageSession session = null;
            try
            {
                this.CurrentPosition = 0;
                this.AdditionalData = new Dictionary<String, Object>();
                Object result = null;
                IDictionary<String, Object> additionalData;

                foreach (Request req in this.Request)
                {
                    if (storageModule == null || storageModule.Name != req.StorageName)
                    {
                        if (session != null)
                        {
                            session.Dispose();
                            session = null;
                        }
                        storageModule = this.Parent.Parent.ModuleManager.GetModule<StorageModule>(req.StorageName);
                    }
                    if (session == null)
                    {
                        session = storageModule.OpenSession();
                    }
                    if (this.CurrentPosition == 0) // Invoking InputFlowModule
                    {
                        InputFlowModule flowModule
                            = this.Parent.Parent.ModuleManager.GetModule<InputFlowModule>(req.FlowName);
                        result = flowModule.Input(
                            req.Selector,
                            session,
                            req.Arguments,
                            out additionalData
                        );
                    }
                    else if (this.CurrentPosition != this.RequestFragmentCount - 1) // Invoking FilterFlowModule
                    {
                        FilterFlowModule flowModule
                            = this.Parent.Parent.ModuleManager.GetModule<FilterFlowModule>(req.FlowName);
                        result = flowModule.Filter(
                            req.Selector,
                            result,
                            session,
                            req.Arguments,
                            out additionalData
                        );
                    }
                    else // Invoking OutputFlowModule (End of flow)
                    {
                        OutputFlowModule flowModule
                            = this.Parent.Parent.ModuleManager.GetModule<OutputFlowModule>(req.FlowName);
                        this._outputValue = flowModule.Output(
                            req.Selector,
                            result,
                            session,
                            req.Arguments,
                            this.OutputType,
                            out additionalData
                        );
                    }
                    if (additionalData != null)
                    {
                        this.AdditionalData.AddRange(additionalData.SelectKey(k => this.CurrentPosition + "_" + k));
                    }

                    if (this.State == RequestTaskState.WaitForPause)
                    {
                        this.State = RequestTaskState.Paused;
                        this._signal.WaitOne();
                        this.State = RequestTaskState.Running;
                    }
                    ++this.CurrentPosition;
                }
                this.ExitTime = DateTime.UtcNow;
                this.State = RequestTaskState.Succeeded;
                this.AccessLog.Info(this.ToLogEntryLine());
                this.Log.Info(Resources.ServerRequestExecuted, this.Request, this.ElapsedTime);
                return this._outputValue;
            }
            catch (ThreadAbortException ex)
            {
                this._outputValue = ex;
                this.ExitTime = DateTime.UtcNow;
                this.State = RequestTaskState.Canceled;
                this.AccessLog.Warn(this.ToLogEntryLine());
                return null;
            }
            catch (Exception ex)
            {
                this._outputValue = ex;
                this.ExitTime = DateTime.UtcNow;
                this.State = RequestTaskState.Failed;
                this.AccessLog.Error(this.ToLogEntryLine());
                this.Log.Error(String.Format(Resources.ServerRequestFailed, this.Request, this.ElapsedTime), ex);
                return null;
            }
            finally
            {
                this._outputReference = new WeakReference(this._outputValue);
                this._signal.Close();
                if (session != null)
                {
                    session.Dispose();
                }
            }
        }

        private String ToLogEntryLine()
        {
            return this.HasExited
                ? String.Format(
                      "{0} +{1} #{2}:{3} {4} {5}",
                      this.ExitTime.Value.ToString("yyyy/MM/dd hh:mm:ss.fff"),
                      this.ElapsedTime.ToString(@"hh\:mm\:ss\.fff"),
                      this.Id,
                      this.State.ToString().Substring(0, 1),
                      "-", // user@host (not supported)
                      this.Request
                  )
                : null;
        }
    }
}