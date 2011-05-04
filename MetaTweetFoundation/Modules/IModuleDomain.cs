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
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;
using System.IO;

namespace XSpect.MetaTweet.Modules
{
    /// <summary>
    /// モジュール アセンブリを読み込み、モジュール オブジェクトを管理するための、独立した環境を提供します。
    /// </summary>
    /// <remarks>
    /// <para>モジュール ドメインは、<see cref="ModuleManager"/> によって作成される、モジュール アセンブリのための独立した環境です。<see cref="Add(string,string,System.Collections.Generic.IList{string})"/> メソッドを使用して新しいモジュール オブジェクトを生成し、<see cref="Remove{TModule}"/> メソッドを使用してそれを破棄することができます。</para>
    /// <para>モジュール オブジェクトは名前 (キー) と型によって一意に識別されます。型が異なる限りにおいて、同一の名前を使用できます。</para>
    /// <para>モジュール ドメインはドメインの名前と同一のディレクトリに対応します。対応先のディレクトリは <see cref="Directory"/> プロパティで参照できます。</para>
    /// </remarks>
    /// <seealso cref="ModuleManager"/>
    public interface IModuleDomain
        : IDisposable,
          ILoggable
    {
        /// <summary>
        /// このモジュール ドメインの親である <see cref="ModuleManager"/> を取得します。
        /// </summary>
        /// <value>
        /// このモジュール ドメインの親である <see cref="ModuleManager"/>。
        /// </value>
        IModuleManager Parent
        {
            get;
        }

        String Key
        {
            get;
        }

        AppDomain AppDomain
        {
            get;
        }

        IEnumerable<AssemblyName> Assemblies
        {
            get;
        }

        /// <summary>
        /// イベントを記録するログ ライタを取得します。
        /// </summary>
        /// <value>
        /// イベントを記録するログ ライタ。
        /// </value>

        /// <summary>
        /// 生成されたモジュール オブジェクトのコレクションを取得します。
        /// </summary>
        /// <value>
        /// 生成されたモジュール オブジェクトのコレクション。
        /// </value>
        IDictionary<Tuple<String, String>, IModule> Modules
        {
            get;
        }

        /// <summary>
        /// このモジュール ドメイン上に現時点で存在するモジュール オブジェクトの初期か情報のリストを取得します。
        /// </summary>
        /// <value>
        /// このモジュール ドメイン上に現時点で存在するモジュール オブジェクトの初期か情報のリスト。
        /// </value>
        IList<ModuleObjectSetup> Snapshot
        {
            get;
        }

        /// <summary>
        /// このモジュール ドメインに対応するディレクトリを取得します。
        /// </summary>
        /// <value>
        /// このモジュール ドメインに対応するディレクトリ。
        /// </value>
        DirectoryInfo Directory
        {
            get;
        }

        /// <summary>
        /// 既定の動作に従い、モジュール ドメインにアセンブリをロードします。
        /// </summary>
        void Load();

        /// <summary>
        /// モジュール ドメインをアンロードします。
        /// </summary>
        void Unload();

        /// <summary>
        /// モジュール オブジェクトを生成します。
        /// </summary>
        /// <typeparam name="TModule">返り値となるモジュール オブジェクトの型。</typeparam>
        /// <param name="key">モジュール オブジェクトに付ける名前。</param>
        /// <param name="typeName">生成するモジュール オブジェクトの完全な型名。</param>
        /// <param name="options">モジュール オブジェクトに渡されるオプションのリスト。</param>
        /// <returns>生成された型厳密なモジュール オブジェクト。</returns>
        TModule Add<TModule>(String key, String typeName, IList<String> options)
            where TModule : IModule;

        /// <summary>
        /// モジュール オブジェクトを生成します。
        /// </summary>
        /// <param name="key">モジュール オブジェクトに付ける名前。</param>
        /// <param name="typeName">生成するモジュール オブジェクトの完全な型名。</param>
        /// <param name="options">モジュール オブジェクトに渡されるオプションのリスト。</param>
        /// <returns>生成されたモジュール オブジェクト。</returns>
        IModule Add(String key, String typeName, IList<String> options);

        /// <summary>
        /// モジュール オブジェクトを生成します。
        /// </summary>
        /// <param name="setup">モジュール オブジェクトを生成するための情報。</param>
        /// <returns>生成されたモジュール オブジェクト。</returns>
        IModule Add(ModuleObjectSetup setup);

        /// <summary>
        /// モジュール オブジェクトを生成します。
        /// </summary>
        /// <param name="key">モジュール オブジェクトの名前。</param>
        /// <param name="typeName">生成するモジュール オブジェクトの完全な型名。</param>
        /// <param name="options">モジュール オブジェクトに渡されるオプションのリスト。</param>
        /// <param name="configFile">モジュール オブジェクトの初期化時に与える設定ファイル。</param>
        /// <returns>生成されたモジュール オブジェクト。</returns>
        IModule Add(String key, String typeName, IList<String> options, FileInfo configFile);

        /// <summary>
        /// モジュール オブジェクトを破棄します。
        /// </summary>
        /// <typeparam name="TModule">破棄するモジュール オブジェクトの型。</typeparam>
        /// <param name="key">破棄するモジュール オブジェクトの名前。</param>
        void Remove<TModule>(String key)
            where TModule : IModule;

        /// <summary>
        /// モジュール オブジェクトを破棄します。
        /// </summary>
        /// <param name="key">破棄するモジュール オブジェクトの名前。</param>
        /// <param name="type">破棄するモジュール オブジェクトの型を表すオブジェクト。</param>
        void Remove(String key, Type type);

        #region Load / LoadFile / LoadFrom

        AssemblyName Load(AssemblyName assemblyRef);

        AssemblyName Load(String assemblyString);

        AssemblyName Load(Byte[] rawAssembly);

        AssemblyName Load(Byte[] rawAssembly, Byte[] rawSymbolStore);

        AssemblyName LoadFile(String path);

        AssemblyName LoadFrom(String assemblyFile);

        #endregion

        #region GetModules / GetModule

        /// <summary>
        /// モジュール オブジェクトを検索します。
        /// </summary>
        /// <typeparam name="TModule">モジュール オブジェクトの型。</typeparam>
        /// <returns>条件に合致するモジュール オブジェクトのシーケンス。</returns>
        IEnumerable<TModule> GetModules<TModule>()
            where TModule : IModule;

        /// <summary>
        /// モジュール オブジェクトを検索します。
        /// </summary>
        /// <typeparam name="TModule">モジュール オブジェクトの型。</typeparam>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュール オブジェクトのシーケンス。</returns>
        IEnumerable<TModule> GetModules<TModule>(String key)
            where TModule : IModule;

        /// <summary>
        /// 全てのモジュール オブジェクトを取得します。
        /// </summary>
        /// <returns>全てのモジュール オブジェクトのシーケンス。</returns>
        IEnumerable<IModule> GetModules();

        /// <summary>
        /// モジュール オブジェクトを検索します。
        /// </summary>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュール オブジェクトのシーケンス。</returns>
        IEnumerable<IModule> GetModules(String key);

        /// <summary>
        /// モジュール オブジェクトを検索します。
        /// </summary>
        /// <param name="type">モジュール オブジェクトの型を表すオブジェクト。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュール オブジェクトのシーケンス。</returns>
        IEnumerable<IModule> GetModules(Type type);

        /// <summary>
        /// モジュール オブジェクトを検索します。
        /// </summary>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="type">モジュール オブジェクトの型を表すオブジェクト。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>条件に合致するモジュール オブジェクトのシーケンス。</returns>
        IEnumerable<IModule> GetModules(String key, Type type);

        /// <summary>
        /// モジュール オブジェクトを取得します。
        /// </summary>
        /// <typeparam name="TModule">モジュール オブジェクトの型。</typeparam>
        /// <returns>一意に特定されたモジュール オブジェクト。</returns>
        TModule GetModule<TModule>()
            where TModule : IModule;

        /// <summary>
        /// モジュール オブジェクトを取得します。
        /// </summary>
        /// <typeparam name="TModule">モジュール オブジェクトの型。</typeparam>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>一意に特定されたモジュール オブジェクト。</returns>
        TModule GetModule<TModule>(String key)
            where TModule : IModule;

        /// <summary>
        /// モジュール オブジェクトを取得します。
        /// </summary>
        /// <returns>唯一のモジュール オブジェクト。</returns>
        IModule GetModule();

        /// <summary>
        /// モジュール オブジェクトを取得します。
        /// </summary>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>一意に特定されたモジュール オブジェクト。</returns>
        IModule GetModule(String key);

        /// <summary>
        /// モジュール オブジェクトを取得します。
        /// </summary>
        /// <param name="type">モジュール オブジェクトの型を表すオブジェクト。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>一意に特定されたモジュール オブジェクト。</returns>
        IModule GetModule(Type type);

        /// <summary>
        /// モジュール オブジェクトを取得します。
        /// </summary>
        /// <param name="key">モジュール オブジェクトを識別する名前。条件を指定しない場合は <c>null</c>。</param>
        /// <param name="type">モジュール オブジェクトの型を表すオブジェクト。条件を指定しない場合は <c>null</c>。</param>
        /// <returns>一意に特定されたモジュール オブジェクト。</returns>
        IModule GetModule(String key, Type type);

        #endregion

        #region Execute

        /// <summary>
        /// スクリプト コードを実行します。
        /// </summary>
        /// <typeparam name="T">コードの返り値の型。</typeparam>
        /// <param name="language">コードの言語を表す文字列。</param>
        /// <param name="source">実行するコード。</param>
        /// <param name="arguments">コードに与える引数とその値のリスト。</param>
        /// <returns>コードの評価の結果となる返り値。</returns>
        T Execute<T>(String language, String source, params Expression<Func<Object, dynamic>>[] arguments);

        /// <summary>
        /// スクリプト コードを実行します。
        /// </summary>
        /// <param name="language">コードの言語を表す文字列。</param>
        /// <param name="source">実行するコード。</param>
        /// <param name="arguments">コードに与える引数とその値のリスト。</param>
        /// <returns>コードの評価の結果となる返り値。</returns>
        dynamic Execute(String language, String source, params Expression<Func<Object, dynamic>>[] arguments);

        /// <summary>
        /// スクリプト コードを実行します。
        /// </summary>
        /// <typeparam name="T">コードの返り値の型。</typeparam>
        /// <param name="file">実行するコード ファイル。</param>
        /// <param name="arguments">コードに与える引数とその値のリスト。</param>
        /// <returns>コードの評価の結果となる返り値。</returns>
        T Execute<T>(FileInfo file, params Expression<Func<Object, dynamic>>[] arguments);

        /// <summary>
        /// スクリプト コードを実行します。
        /// </summary>
        /// <param name="file">実行するコード ファイル。</param>
        /// <param name="arguments">コードに与える引数とその値のリスト。</param>
        /// <returns>コードの評価の結果となる返り値。</returns>
        dynamic Execute(FileInfo file, params Expression<Func<Object, dynamic>>[] arguments);

        #endregion

        /// <summary>
        /// 型名を表す文字列から、その型が含まれるアセンブリの名前を返します。
        /// </summary>
        /// <param name="typeName">検索する型を表す文字列。</param>
        /// <returns>指定した型が含まれるアセンブリの名前を表すオブジェクト。</returns>
        AssemblyName GetAssemblyByName(String typeName);
    }
}
