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
using XSpect.MetaTweet.ObjectModel;
using XSpect.Configuration;

namespace XSpect.MetaTweet.Modules
{
    /// <summary>
    /// フロー モジュールに共通の機能を提供します。
    /// </summary>
    /// <remarks>
    /// フロー モジュールとは、このクラスを継承する型、即ち <see cref="InputFlowModule"/>、<see cref="FilterFlowModule"/> および <see cref="OutputFlowModule"/> を指します。このクラスは、これらフロー モジュールに共通の操作を実装し、提供します。
    /// </remarks>
    public abstract class FlowModule
        : MarshalByRefObject,
          IModule
    {
        /// <summary>
        /// このモジュールがホストされているサーバ オブジェクトを取得します。
        /// </summary>
        /// <value>このモジュールがホストされているサーバ オブジェクト。</value>
        public ServerCore Host
        {
            get;
            private set;
        }

        /// <summary>
        /// このモジュールに設定された名前を取得します。
        /// </summary>
        /// <value>このモジュールに設定された名前を取得します。</value>
        public String Name
        {
            get;
            private set;
        }

        /// <summary>
        /// このモジュールの設定を管理するオブジェクトを取得します。
        /// </summary>
        /// <value>このモジュールの設定を管理するオブジェクト。</value>
        public XmlConfiguration Configuration
        {
            get;
            private set;
        }

        /// <summary>
        /// ストレージに対し提示する、サービスを表す文字列 (Realm) を取得または設定します。
        /// </summary>
        /// <value>
        /// ストレージに対し提示する、サービスを表す文字列 (Realm)。
        /// </value>
        /// <remarks>
        /// このプロパティは <see cref="XSpect.MetaTweet.ObjectModel.Account.Realm"/> に対応します。
        /// </remarks>
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

        /// <summary>
        ///<see cref="FlowModule"/> によって使用されているすべてのリソースを解放します。
        /// </summary>
        public virtual void Dispose()
        {
        }

        /// <summary>
        /// このモジュールをサーバ オブジェクトに登録します。
        /// </summary>
        /// <param name="host">登録されるサーバ オブジェクト。</param>
        /// <param name="name">モジュールに設定する名前。</param>
        public virtual void Register(ServerCore host, String name)
        {
            this.Host = host;
            this.Name = name;
        }

        /// <summary>
        /// このモジュールに設定を適用し、初期化を行います。
        /// </summary>
        /// <param name="configuration">適用する設定。</param>
        public void Initialize(XmlConfiguration configuration)
        {
            this.Configuration = configuration;
            this.Initialize();
        }

        /// <summary>
        /// このモジュールを初期化します。
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// このモジュールに定義されているフロー インターフェイスを検索します。
        /// </summary>
        /// <returns>全てのフロー インターフェイスの一覧。</returns>
        /// <remarks>
        /// 返り値の型が <see cref="XSpect.MetaTweet.ObjectModel.StorageObject"/> のフロー インターフェイスを検索します。<see cref="XSpect.MetaTweet.ObjectModel.StorageObject"/> 型以外の値を返すフロー インターフェイスを検索する場合は、型パラメタを指定するオーバーロードを使用してください。
        /// </remarks>
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

        /// <summary>
        /// このモジュールに定義されているフロー インターフェイスを検索します。
        /// </summary>
        /// <typeparam name="TOutput">フロー インターフェイスの返り値の型。</typeparam>
        /// <returns>指定した条件に合致するフロー インターフェイスの一覧。</returns>
        public IEnumerable<FlowInterfaceInfo> GetFlowInterfaces<TOutput>()
        {
            return this.GetFlowInterfaces()
                .Where(ii => ii.OutputType == typeof(TOutput));
        }

        /// <summary>
        /// このモジュールに定義されているフロー インターフェイスを検索します。
        /// </summary>
        /// <param name="selector">フロー インターフェイスに対し照合を行うセレクタ文字列。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>指定した条件に合致するフロー インターフェイスと、セレクタ照合で得られたパラメータの組の一覧。</returns>
        /// <remarks>
        /// 返り値の型が <see cref="XSpect.MetaTweet.ObjectModel.StorageObject"/> のフロー インターフェイスを検索します。<see cref="XSpect.MetaTweet.ObjectModel.StorageObject"/> 型以外の値を返すフロー インターフェイスを検索する場合は、型パラメタを指定するオーバーロードを使用してください。
        /// </remarks>
        public IEnumerable<KeyValuePair<FlowInterfaceInfo, String>> GetFlowInterfaces(String selector)
        {
            return this.GetFlowInterfaces()
                .Where(ii => selector.StartsWith(ii != null ? ii.Id : selector))
                .Select(ii =>
                    new KeyValuePair<FlowInterfaceInfo, String>(ii, ii.GetParameter(selector))
                )
                .OrderBy(p => p.Value.Length);
        }

        /// <summary>
        /// このモジュールに定義されているフロー インターフェイスを検索します。
        /// </summary>
        /// <typeparam name="TOutput">フロー インターフェイスの返り値の型。</typeparam>
        /// <param name="selector">フロー インターフェイスに対し照合を行うセレクタ文字列。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>指定した条件に合致するフロー インターフェイスと、セレクタ照合で得られたパラメータの組の一覧。</returns>
        public IEnumerable<KeyValuePair<FlowInterfaceInfo, String>> GetFlowInterfaces<TOutput>(String selector)
        {
            return this.GetFlowInterfaces(selector)
                .Where(p => p.Key.OutputType == typeof(TOutput))
                .OrderBy(p => p.Value.Length);
        }

        /// <summary>
        /// このモジュールに定義されているフロー インターフェイスを取得します。
        /// </summary>
        /// <typeparam name="TOutput">フロー インターフェイスの返り値の型。</typeparam>
        /// <param name="selector">フロー インターフェイスに対し照合を行うセレクタ文字列。</param>
        /// <param name="parameter">セレクタ照合で得られたパラメータ。このパラメータは初期化せずに渡されます。</param>
        /// <returns>セレクタ条件に合致する中で、最も適合するフロー インターフェイス。</returns>
        public FlowInterfaceInfo GetFlowInterface<TOutput>(String selector, out String parameter)
        {
            KeyValuePair<FlowInterfaceInfo, String> selected
                = this.GetFlowInterfaces<TOutput>(selector).First();
            parameter = selected.Value;
            return selected.Key;
        }

        /// <summary>
        /// このモジュールに定義されているフロー インターフェイスを取得します。
        /// </summary>
        /// <param name="selector">フロー インターフェイスに対し照合を行うセレクタ文字列。</param>
        /// <param name="parameter">セレクタ照合で得られたパラメータ。このパラメータは初期化せずに渡されます。</param>
        /// <returns>セレクタ条件に合致する中で、最も適合するフロー インターフェイス。</returns>
        /// <remarks>
        /// 返り値の型が <see cref="XSpect.MetaTweet.ObjectModel.StorageObject"/> のフロー インターフェイスを検索します。<see cref="XSpect.MetaTweet.ObjectModel.StorageObject"/> 型以外の値を返すフロー インターフェイスを検索する場合は、型パラメタを指定するオーバーロードを使用してください。
        /// </remarks>
        public FlowInterfaceInfo GetFlowInterface(String selector, out String parameter)
        {
            return this.GetFlowInterface<IEnumerable<StorageObject>>(selector, out parameter);
        }
    }
}