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
using System.Linq;
using System.Net;
using System.Reflection;
using System.Collections.Generic;
using XSpect.Configuration;
using XSpect.Extension;
using XSpect.Hooking;
using XSpect.MetaTweet.Objects;
using log4net;

namespace XSpect.MetaTweet.Modules
{
    /// <summary>
    /// フロー モジュールに共通の機能を提供します。
    /// </summary>
    /// <remarks>
    /// <para>フロー モジュールとは、このクラスを継承する型、即ち <see cref="InputFlowModule"/>、<see cref="FilterFlowModule"/> および <see cref="OutputFlowModule"/> を指します。このクラスは、これらフロー モジュールに共通の操作を実装し、提供します。</para>
    /// <para>全てのフロー モジュール (<see cref="FlowModule"/> を継承する全てのモジュール) はスカラ値を返すことができます。スカラ値は任意の型の値で、<see cref="FlowInterfaceAttribute.Id"/> が <c>@</c> で始まるインターフェイスで返すことができます。スカラ値の取得は入力を取ることができず、また、スカラ値の型に関わらず取得した時点でフロー パイプラインは終了します。</para>
    /// </remarks>
    public abstract class FlowModule
        : MarshalByRefObject,
          IModule
    {
        private Boolean _disposed;

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
        /// <see cref="Initialize(XmlConfiguration)"/> のフック リストを取得します。
        /// </summary>
        /// <value>
        /// <see cref="Initialize(XmlConfiguration)"/> のフック リスト。
        /// </value>
        public ActionHook<IModule> InitializeHook
        {
            get;
            private set;
        }

        /// <summary>
        /// イベントを記録するログ ライタを取得します。
        /// </summary>
        /// <value>
        /// イベントを記録するログ ライタ。
        /// </value>
        public ILog Log
        {
            get
            {
                return this.Host.Log;
            }
        }

        /// <summary>
        /// ストレージに対し提示する、サービスを表す文字列 (Realm) を取得または設定します。
        /// </summary>
        /// <value>
        /// ストレージに対し提示する、サービスを表す文字列 (Realm)。
        /// </value>
        /// <remarks>
        /// <para>このプロパティの値は <see cref="XSpect.MetaTweet.Objects.Account.Realm"/> に対応します。</para>
        /// <para>このプロパティは <see cref="Configuration"/> の設定エントリ <c>realm</c> へのアクセスを提供します。エントリが存在しない場合は <see cref="DefaultRealm"/> の値が使用されます。</para>
        /// </remarks>
        public String Realm
        {
            get
            {
                return this.Configuration.Exists("realm")
                    ? this.Configuration.ResolveValue<String>("realm")
                    : this.DefaultRealm;
            }
            set
            {
                this.Configuration.Get<String>("realm").Value = value;
            }
        }

        /// <summary>
        /// 派生クラスで実装された場合、既定の <see cref="Realm"/> 値を取得します。
        /// </summary>
        /// <value>派生クラスで実装された場合、既定の <see cref="Realm"/> 値。</value>
        protected abstract String DefaultRealm
        {
            get;
        }

        /// <summary>
        /// 接続に使用するプロキシを取得します。
        /// </summary>
        /// <value>
        /// 接続に使用するプロキシ。
        /// </value>
        /// <remarks>
        /// このプロパティの値は <see cref="ProxyAddress"/> および <see cref="ProxyCredential"/> の値によって構築されます。
        /// </remarks>
        public WebProxy Proxy
        {
            get
            {
                return String.IsNullOrEmpty(this.ProxyAddress)
                    ? new WebProxy(
                          this.ProxyAddress,
                          false,
                          null,
                          this.ProxyCredential
                      )
                    : new WebProxy();
            }
        }

        /// <summary>
        /// 接続に使用するプロキシのアドレスを取得または設定します。
        /// </summary>
        /// <value>
        /// 接続に使用するプロキシのアドレス。
        /// </value>
        /// <remarks>
        /// このプロパティは <see cref="Configuration"/> の設定エントリ <c>proxyAddress</c> へのアクセスを提供します。
        /// </remarks>
        public String ProxyAddress
        {
            get
            {
                return this.Configuration.Exists("proxyHost")
                    ? this.Configuration.ResolveValue<String>("proxyHost")
                    : null;
            }
            set
            {
                this.Configuration.Get<String>("proxyHost").Value = value;
            }
        }

        /// <summary>
        /// 接続に使用するプロキシに与える資格情報を取得または設定します。
        /// </summary>
        /// <value>
        /// 接続に使用するプロキシに与える資格情報。
        /// </value>
        /// <remarks>
        /// このプロパティは <see cref="Configuration"/> の設定エントリ <c>proxyCredential</c> へのアクセスを提供します。
        /// </remarks>
        public NetworkCredential ProxyCredential
        {
            get
            {
                return this.Configuration.Exists("proxyCredential")
                    ? this.Configuration.ResolveValue<NetworkCredential>("proxyCredential")
                    : null;
            }
            set
            {
                this.Configuration.Get<NetworkCredential>("proxyCredential").Value = value;
            }
        }

        /// <summary>
        /// <see cref="FlowModule"/> の新しいインスタンスを初期化します。
        /// </summary>
        protected FlowModule()
        {
            this.InitializeHook = new ActionHook<IModule>(this.InitializeImpl);
        }

        /// <summary>
        /// <see cref="FlowModule"/> がガベージ コレクションによってクリアされる前に、アンマネージ リソースを解放し、その他のクリーンアップ操作を実行します。
        /// </summary>
        ~FlowModule()
        {
            this.Dispose(false);
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

        /// <summary>
        /// <see cref="FlowModule"/> によって使用されているすべてのリソースを解放します。
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// <see cref="FlowModule"/> によって使用されているアンマネージ リソースを解放し、オプションでマネージ リソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 <c>true</c>、破棄されない場合は <c>false</c>。</param>
        protected virtual void Dispose(Boolean disposing)
        {
            this._disposed = true;
        }

        /// <summary>
        /// オブジェクトが破棄されているかどうかを確認し、破棄されている場合は例外を送出します。
        /// </summary>
        protected void CheckIfDisposed()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }

        /// <summary>
        /// このモジュールをサーバ オブジェクトに登録します。
        /// </summary>
        /// <param name="host">登録されるサーバ オブジェクト。</param>
        /// <param name="name">モジュールに設定する名前。</param>
        /// <param name="configuration">モジュールが参照する設定。</param>
        public virtual void Register(ServerCore host, String name, XmlConfiguration configuration)
        {
            this.Host = host;
            this.Name = name;
            this.Configuration = configuration;
        }

        /// <summary>
        /// このモジュールを初期化します。
        /// </summary>
        /// <remarks>
        /// このメソッドはモジュールの寿命中、複数回呼び出される可能性があります。
        /// </remarks>
        public virtual void Initialize()
        {
            this.InitializeHook.Execute();
        }

        protected virtual void InitializeImpl()
        {
        }

        /// <summary>
        /// このモジュールに定義されているフロー インターフェイスを検索します。
        /// </summary>
        /// <returns>全てのフロー インターフェイスのシーケンス。</returns>
        /// <remarks>
        /// 返り値の型が <see cref="XSpect.MetaTweet.Objects.StorageObject"/> のフロー インターフェイスを検索します。<see cref="XSpect.MetaTweet.Objects.StorageObject"/> 型以外の値を返すフロー インターフェイスを検索する場合は、型パラメタを指定するオーバーロードを使用してください。
        /// </remarks>
        public IEnumerable<FlowInterfaceInfo> GetFlowInterfaces()
        {
            this.CheckIfDisposed();
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
        /// <returns>指定した条件に合致するフロー インターフェイスのシーケンス。</returns>
        public IEnumerable<FlowInterfaceInfo> GetFlowInterfaces<TOutput>()
        {
            return this.GetFlowInterfaces()
                .Where(ii => ii.OutputType == typeof(TOutput));
        }

        /// <summary>
        /// このモジュールに定義されているフロー インターフェイスを検索します。
        /// </summary>
        /// <param name="selector">フロー インターフェイスに対し照合を行うセレクタ文字列。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>指定した条件に合致するフロー インターフェイスと、セレクタ照合で得られたパラメータの組のシーケンス。</returns>
        /// <remarks>
        /// 返り値の型が <see cref="XSpect.MetaTweet.Objects.StorageObject"/> のフロー インターフェイスを検索します。<see cref="XSpect.MetaTweet.Objects.StorageObject"/> 型以外の値を返すフロー インターフェイスを検索する場合は、型パラメタを指定するオーバーロードを使用してください。
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
        /// <returns>指定した条件に合致するフロー インターフェイスと、セレクタ照合で得られたパラメータの組のシーケンス。</returns>
        public IEnumerable<KeyValuePair<FlowInterfaceInfo, String>> GetFlowInterfaces<TOutput>(String selector)
        {
            return this.GetFlowInterfaces(selector, typeof(TOutput));
        }

        /// <summary>
        /// このモジュールに定義されているフロー インターフェイスを検索します。
        /// </summary>
        /// <param name="selector">フロー インターフェイスに対し照合を行うセレクタ文字列。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="outputType">フロー インターフェイスの返り値の型。</param>
        /// <returns>指定した条件に合致するフロー インターフェイスと、セレクタ照合で得られたパラメータの組のシーケンス。</returns>
        public IEnumerable<KeyValuePair<FlowInterfaceInfo, String>> GetFlowInterfaces(String selector, Type outputType)
        {
            return this.GetFlowInterfaces(selector)
                .Where(p => outputType.IsAssignableFrom(p.Key.OutputType))
                .OrderBy(p => p.Value.Length);
        }

        /// <summary>
        /// このモジュールに定義されているフロー インターフェイスを取得します。
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="selector">フロー インターフェイスに対し照合を行うセレクタ文字列。</param>
        /// <param name="parameter">セレクタ照合で得られたパラメータ。このパラメータは初期化せずに渡されます。</param>
        /// <returns>セレクタ条件に合致する中で、最も適合するフロー インターフェイス。</returns>
        public FlowInterfaceInfo GetFlowInterface<TOutput>(String selector, out String parameter)
        {
            return this.GetFlowInterface(selector, typeof(TOutput), out parameter);
        }

        /// <summary>
        /// このモジュールに定義されているフロー インターフェイスを取得します。
        /// </summary>
        /// <param name="selector">フロー インターフェイスに対し照合を行うセレクタ文字列。</param>
        /// <param name="outputType">フロー インターフェイスの返り値の型。</param>
        /// <param name="parameter">セレクタ照合で得られたパラメータ。このパラメータは初期化せずに渡されます。</param>
        /// <returns>セレクタ条件に合致する中で、最も適合するフロー インターフェイス。</returns>
        public FlowInterfaceInfo GetFlowInterface(String selector, Type outputType, out String parameter)
        {
            KeyValuePair<FlowInterfaceInfo, String> selected
                = this.GetFlowInterfaces(selector, outputType).First();
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
        /// 返り値の型が <see cref="XSpect.MetaTweet.Objects.StorageObject"/> のフロー インターフェイスを検索します。<see cref="XSpect.MetaTweet.Objects.StorageObject"/> 型以外の値を返すフロー インターフェイスを検索する場合は、型パラメタを指定するオーバーロードを使用してください。
        /// </remarks>
        public FlowInterfaceInfo GetFlowInterface(String selector, out String parameter)
        {
            return this.GetFlowInterface<IEnumerable<StorageObject>>(selector, out parameter);
        }
    }
}