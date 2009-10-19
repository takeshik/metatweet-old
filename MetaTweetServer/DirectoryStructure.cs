// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
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
using System.IO;
using XSpect.Configuration;

namespace XSpect.MetaTweet
{
    /// <summary>
    /// MetaTweet システムのディレクトリ構造を表します。
    /// </summary>
    public sealed class DirectoryStructure
        : Object
    {
        /// <summary>
        /// MetaTweet システムのベースディレクトリを取得します。
        /// </summary>
        /// <value>
        /// MetaTweet システムのベースディレクトリ。
        /// </value>
        public DirectoryInfo BaseDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// システムの、またはシステム全体で使用される実行ファイルが配置されているディレクトリを取得します。
        /// </summary>
        /// <value>
        /// システムの、またはシステム全体で使用される実行ファイルが配置されているディレクトリ。
        /// </value>
        /// <remarks>
        /// 指定されているディレクトリが存在しない場合、新規に作成されます。
        /// </remarks>
        public DirectoryInfo BinaryDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// システムの、またはシステム全体で使用される特権を要する実行ファイルが配置されているディレクトリを取得します。
        /// </summary>
        /// <value>
        /// システムの、またはシステム全体で使用される特権を要する実行ファイルが配置されているディレクトリ。
        /// </value>
        /// <remarks>
        /// 指定されているディレクトリが存在しない場合、新規に作成されます。
        /// </remarks>
        public DirectoryInfo PrivilegedBinaryDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// キャッシュ ファイルが生成されるディレクトリを取得します。
        /// </summary>
        /// <value>
        /// キャッシュ ファイルが生成されるディレクトリ。
        /// </value>
        /// <remarks>
        /// 指定されているディレクトリが存在しない場合、新規に作成されます。
        /// </remarks>
        public DirectoryInfo CacheDirectory
        {
            get;
            set;
        }

        /// <summary>
        /// システムの、またはシステム全体で使用される設定ファイルが配置されているディレクトリを取得します。
        /// </summary>
        /// <value>
        /// システムの、またはシステム全体で使用される設定ファイルが配置されているディレクトリ。
        /// </value>
        /// <remarks>
        /// 指定されているディレクトリが存在しない場合、新規に作成されます。
        /// </remarks>
        public DirectoryInfo ConfigDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// システムの、またはシステム全体で使用されるライブラリが配置されているディレクトリを取得します。
        /// </summary>
        /// <value>
        /// システムの、またはシステム全体で使用されるライブラリが配置されているディレクトリ。
        /// </value>
        /// <remarks>
        /// 指定されているディレクトリが存在しない場合、新規に作成されます。
        /// </remarks>
        public DirectoryInfo LibraryDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// ログ ファイルが生成されるディレクトリを取得します。
        /// </summary>
        /// <value>
        /// ログ ファイルが生成されるディレクトリ。
        /// </value>
        /// <remarks>
        /// 指定されているディレクトリが存在しない場合、新規に作成されます。
        /// </remarks>
        public DirectoryInfo LogDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// モジュールおよびモジュールの使用する各種ファイルが配置されているディレクトリを取得します。
        /// </summary>
        /// <value>
        /// モジュールおよびモジュールの使用する各種ファイルが配置されているディレクトリ。
        /// </value>
        /// <remarks>
        /// 指定されているディレクトリが存在しない場合、新規に作成されます。
        /// </remarks>
        public DirectoryInfo ModuleDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// システムの状態を記録するファイルが生成されるディレクトリを取得します。
        /// </summary>
        /// <value>
        /// システムの状態を記録するファイルが生成されるディレクトリ。
        /// </value>
        /// <remarks>
        /// 指定されているディレクトリが存在しない場合、新規に作成されます。
        /// </remarks>
        public DirectoryInfo RuntimeDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// 一時ファイルが生成されるディレクトリを取得します。
        /// </summary>
        /// <value>
        /// 一時ファイルが生成されるディレクトリ。
        /// </value>
        /// <remarks>
        /// 指定されているディレクトリが存在しない場合、新規に作成されます。
        /// </remarks>
        public DirectoryInfo TempDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="BaseDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="BaseDirectory"/> を監視するコンポーネント。
        /// </value>
        public FileSystemWatcher BaseDirectoryWatcher
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="BinaryDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="BinaryDirectory"/> を監視するコンポーネント。
        /// </value>
        public FileSystemWatcher BinaryDirectoryWatcher
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="PrivilegedBinaryDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="PrivilegedBinaryDirectory"/> を監視するコンポーネント。
        /// </value>
        public FileSystemWatcher PrivilegedBinaryDirectoryWatcher
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="CacheDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="CacheDirectory"/> を監視するコンポーネント。
        /// </value>
        public FileSystemWatcher CacheDirectoryWatcher
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="ConfigDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="ConfigDirectory"/> を監視するコンポーネント。
        /// </value>
        public FileSystemWatcher ConfigDirectoryWatcher
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="LibraryDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="LibraryDirectory"/> を監視するコンポーネント。
        /// </value>
        public FileSystemWatcher LibraryDirectoryWatcher
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="LogDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="LogDirectory"/> を監視するコンポーネント。
        /// </value>
        public FileSystemWatcher LogDirectoryWatcher
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="ModuleDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="ModuleDirectory"/> を監視するコンポーネント。
        /// </value>
        public FileSystemWatcher ModuleDirectoryWatcher
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="RuntimeDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="RuntimeDirectory"/> を監視するコンポーネント。
        /// </value>
        public FileSystemWatcher RuntimeDirectoryWatcher
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="TempDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="TempDirectory"/> を監視するコンポーネント。
        /// </value>
        public FileSystemWatcher TempDirectoryWatcher
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="DirectoryStructure"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="configuration">設定を取得する <see cref="XmlConfiguration"/> オブジェクト。</param>
        public DirectoryStructure(XmlConfiguration configuration)
        {
            this.BaseDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            this.BinaryDirectory = this.BaseDirectory.CreateSubdirectory(
                configuration.ResolveValue<String>("binary")
            );
            this.PrivilegedBinaryDirectory = this.BaseDirectory.CreateSubdirectory(
                configuration.ResolveValue<String>("privbinary")
            );
            this.CacheDirectory = this.BaseDirectory.CreateSubdirectory(
                configuration.ResolveValue<String>("cache")
            );
            this.ConfigDirectory = this.BaseDirectory.CreateSubdirectory(
                configuration.ResolveValue<String>("config")
            );
            this.LibraryDirectory = this.BaseDirectory.CreateSubdirectory(
                configuration.ResolveValue<String>("library")
            );
            this.LogDirectory = this.BaseDirectory.CreateSubdirectory(
                configuration.ResolveValue<String>("log")
            );
            this.ModuleDirectory = this.BaseDirectory.CreateSubdirectory(
                configuration.ResolveValue<String>("module")
            );
            this.RuntimeDirectory = this.BaseDirectory.CreateSubdirectory(
                configuration.ResolveValue<String>("runtime")
            );
            this.TempDirectory = this.BaseDirectory.CreateSubdirectory(
                configuration.ResolveValue<String>("temp")
            );

            this.BaseDirectoryWatcher = new FileSystemWatcher(this.BaseDirectory.FullName);
            this.BinaryDirectoryWatcher = new FileSystemWatcher(this.BinaryDirectory.FullName);
            this.PrivilegedBinaryDirectoryWatcher = new FileSystemWatcher(this.PrivilegedBinaryDirectory.FullName);
            this.CacheDirectoryWatcher = new FileSystemWatcher(this.CacheDirectory.FullName);
            this.ConfigDirectoryWatcher = new FileSystemWatcher(this.ConfigDirectory.FullName);
            this.LibraryDirectoryWatcher = new FileSystemWatcher(this.LibraryDirectory.FullName);
            this.LogDirectoryWatcher = new FileSystemWatcher(this.LogDirectory.FullName);
            this.ModuleDirectoryWatcher = new FileSystemWatcher(this.ModuleDirectory.FullName);
            this.RuntimeDirectoryWatcher = new FileSystemWatcher(this.RuntimeDirectory.FullName);
            this.TempDirectoryWatcher = new FileSystemWatcher(this.TempDirectory.FullName);
        }
    }
}