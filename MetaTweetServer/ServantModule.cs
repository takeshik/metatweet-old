// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@xspect.org>
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
using System.Runtime.Remoting.Messaging;
using System.Collections.Generic;

namespace XSpect.MetaTweet
{
    public abstract class ServantModule
        : MarshalByRefObject,
          IModule
    {
        public const String ModuleTypeString = "servant";

        private ServerCore _host;

        private String _name;

        public ServerCore Host
        {
            get
            {
                return this._host;
            }
            set
            {
                if (this._host != null)
                {
                    throw new InvalidOperationException();
                }
                this._host = value;
            }
        }

        public String Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (this._name != null)
                {
                    throw new InvalidOperationException();
                }
                this._name = value;
            }
        }

        public String ModuleType
        {
            get
            {
                return ModuleTypeString;
            }
        }

        public Hook<ServantModule> StartHook
        {
            get;
            private set;
        }

        public Hook<ServantModule> StopHook
        {
            get;
            private set;
        }

        public Hook<ServantModule> PauseHook
        {
            get;
            private set;
        }

        public Hook<ServantModule> ContinueHook
        {
            get;
            private set;
        }

        public Hook<ServantModule> AbortHook
        {
            get;
            private set;
        }

        public Hook<ServantModule> WaitHook
        {
            get;
            private set;
        }

        public ServantModule()
        {
            this.StartHook = new Hook<ServantModule>();
            this.StopHook = new Hook<ServantModule>();
            this.PauseHook = new Hook<ServantModule>();
            this.ContinueHook = new Hook<ServantModule>();
            this.AbortHook = new Hook<ServantModule>();
            this.WaitHook = new Hook<ServantModule>();
        }

        /// <summary>
        /// 対象のインスタンスの有効期間ポリシーを制御する、有効期間サービス オブジェクトを取得します。
        /// </summary>
        /// <returns>
        /// 対象のインスタンスの有効期間ポリシーを制御するときに使用する、<see cref="T:System.Runtime.Remoting.Lifetime.ILease"/> 型のオブジェクト。存在する場合は、このインスタンスの現在の有効期間サービス オブジェクトです。それ以外の場合は、<see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime"/> プロパティの値に初期化された新しい有効期間サービス オブジェクトです。
        /// </returns>
        /// <exception cref="T:System.Security.SecurityException">直前の呼び出し元に、インフラストラクチャ アクセス許可がありません。</exception>
        /// <PermissionSet>
        /// <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure"/>
        /// </PermissionSet>
        public override Object InitializeLifetimeService()
        {
            return null;
        }

        public virtual void Initialize(IDictionary<String, String> args)
        {
        }

        public virtual void Dispose()
        {
            this.Abort();
        }

        public void Start()
        {
            this.StartHook.Execute(self =>
            {
                self.StartImpl();
            }, this);
        }

        protected abstract void StartImpl();

        public void Stop()
        {
            this.StopHook.Execute(self =>
            {
                self.StopImpl();
            }, this);
        }

        protected abstract void StopImpl();

        public void Pause()
        {
            this.PauseHook.Execute(self =>
            {
                self.PauseImpl();
            }, this);
        }

        protected virtual void PauseImpl()
        {
            this.StopImpl();
        }

        public void Continue()
        {
            this.ContinueHook.Execute(self =>
            {
                self.ContinueImpl();
            }, this);
        }

        protected virtual void ContinueImpl()
        {
            this.StartImpl();
        }

        public void Abort()
        {
            this.AbortHook.Execute(self =>
            {
                self.AbortImpl();
            }, this);
        }

        protected virtual void AbortImpl()
        {
        }

        public void Wait()
        {
            this.WaitHook.Execute(self =>
            {
                self.WaitImpl();
            }, this);
        }

        protected virtual void WaitImpl()
        {
        }

        public IAsyncResult BeginStart(AsyncCallback callback, Object state)
        {
            return new Action(this.Start).BeginInvoke(callback, state);
        }

        public void EndStart(IAsyncResult asyncResult)
        {
            ((asyncResult as AsyncResult).AsyncDelegate as Action).EndInvoke(asyncResult);
        }

        public IAsyncResult BeginStop(AsyncCallback callback, Object state)
        {
            return new Action(this.Stop).BeginInvoke(callback, state);
        }

        public void EndStop(IAsyncResult asyncResult)
        {
            ((asyncResult as AsyncResult).AsyncDelegate as Action).EndInvoke(asyncResult);
        }

        public IAsyncResult BeginPause(AsyncCallback callback, Object state)
        {
            return new Action(this.Pause).BeginInvoke(callback, state);
        }

        public void EndPause(IAsyncResult asyncResult)
        {
            ((asyncResult as AsyncResult).AsyncDelegate as Action).EndInvoke(asyncResult);
        }

        public IAsyncResult BeginContinue(AsyncCallback callback, Object state)
        {
            return new Action(this.Continue).BeginInvoke(callback, state);
        }

        public void EndContinue(IAsyncResult asyncResult)
        {
            ((asyncResult as AsyncResult).AsyncDelegate as Action).EndInvoke(asyncResult);
        }

        public IAsyncResult BeginAbort(AsyncCallback callback, Object state)
        {
            return new Action(this.Abort).BeginInvoke(callback, state);
        }

        public void EndAbort(IAsyncResult asyncResult)
        {
            ((asyncResult as AsyncResult).AsyncDelegate as Action).EndInvoke(asyncResult);
        }

        public IAsyncResult BeginWait(AsyncCallback callback, Object state)
        {
            return new Action(this.Wait).BeginInvoke(callback, state);
        }

        public void EndWait(IAsyncResult asyncResult)
        {
            ((asyncResult as AsyncResult).AsyncDelegate as Action).EndInvoke(asyncResult);
        }
    }
}
