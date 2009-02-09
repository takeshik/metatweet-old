// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
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
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace XSpect.MetaTweet.Modules
{
    public abstract class FlowModule
        : MarshalByRefObject,
          IModule
    {
        public ServerCore Host
        {
            get;
            private set;
        }

        public String Name
        {
            get;
            private set;
        }

        public IDictionary<String, String> Arguments
        {
            get;
            private set;
        }

        public String Realm
        {
            get;
            set;
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
            return base.InitializeLifetimeService();
        }

        public virtual void Dispose()
        {
        }

        public virtual void Register(ServerCore host, String name)
        {
            this.Host = host;
            this.Name = name;
        }

        public virtual void Initialize(IDictionary<String, String> args)
        {
            this.Arguments = args;
        }

        public IEnumerable<KeyValuePair<FlowInterfaceInfo, String>> GetFlowInterfaces(String selector)
        {
            return this.GetFlowInterfaces()
                .Where(ii => selector.StartsWith(ii != null ? ii.Id : selector))
                .Select(ii =>
                    new KeyValuePair<FlowInterfaceInfo, String>(ii, ii.GetParameter(selector))
                )
                .OrderBy(p => p.Value.Length);
        }

        public IEnumerable<FlowInterfaceInfo> GetFlowInterfaces()
        {
            return this.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .SelectMany(
                    m => m.GetCustomAttributes(typeof(FlowInterfaceAttribute), true)
                        .Cast<FlowInterfaceAttribute>()
                        .Select(a => new FlowInterfaceInfo(m, a))
                );
        }

        public FlowInterfaceInfo GetFlowInterface(String selector, out String parameter)
        {
            KeyValuePair<FlowInterfaceInfo, String> selected
                = this.GetFlowInterfaces(selector).First();
            parameter = selected.Value;
            return selected.Key;
        }
    }
}