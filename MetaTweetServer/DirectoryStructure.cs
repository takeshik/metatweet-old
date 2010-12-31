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
using System.Diagnostics;
using System.IO;
using XSpect.Extension;

namespace XSpect.MetaTweet
{
    /// <summary>
    /// MetaTweet システムのディレクトリ構造を表します。
    /// </summary>
    public sealed class DirectoryStructure
        : MarshalByRefObject
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
        /// 設定が提供されるまでの暫定的なディレクトリ構成によって <see cref="DirectoryStructure"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="tempConfigDir">仮の <see cref="ConfigDirectory"/> のパス。</param>
        internal DirectoryStructure(String tempConfigDir)
        {
            this.BaseDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            this.PrivilegedBinaryDirectory = new FileInfo(Process.GetCurrentProcess().MainModule.FileName).Directory;
            this.BinaryDirectory = this.PrivilegedBinaryDirectory;
            this.CacheDirectory = this.BaseDirectory;
            this.ConfigDirectory = new DirectoryInfo(tempConfigDir);
            this.LibraryDirectory = new FileInfo(typeof(ServerCore).Assembly.Location).Directory;
            this.LogDirectory = this.BaseDirectory;
            this.ModuleDirectory = this.BaseDirectory;
            this.RuntimeDirectory = this.BaseDirectory;
            this.TempDirectory = new DirectoryInfo(Path.GetTempPath());
        }

        /// <summary>
        /// <see cref="DirectoryStructure"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="configuration">設定を保持するオブジェクト。</param>
        public DirectoryStructure(dynamic configuration)
        {
            this.BaseDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            Environment.SetEnvironmentVariable(
                "BaseDir",
                this.BaseDirectory.FullName,
                EnvironmentVariableTarget.Process
            );
            Environment.SetEnvironmentVariable(
                "DataDir",
                new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))
                    .CreateSubdirectory("MetaTweet")
                    .FullName,
                EnvironmentVariableTarget.Process
            );
            this.BinaryDirectory = this.GetDirectory(configuration.Binary);
            this.PrivilegedBinaryDirectory = this.GetDirectory(configuration.PrivBinary);
            this.CacheDirectory = this.GetDirectory(configuration.Cache);
            this.ConfigDirectory = this.GetDirectory(configuration.Config);
            this.LibraryDirectory = this.GetDirectory(configuration.Library);
            this.LogDirectory = this.GetDirectory(configuration.Log);
            this.ModuleDirectory = this.GetDirectory(configuration.Module);
            this.RuntimeDirectory = this.GetDirectory(configuration.Runtime);
            this.TempDirectory = this.GetDirectory(configuration.Temp);

            this.BaseDirectoryWatcher = new FileSystemWatcher(this.BaseDirectory.FullName)
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = true,
            };
            this.BinaryDirectoryWatcher = new FileSystemWatcher(this.BinaryDirectory.FullName)
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = true,
            };
            this.PrivilegedBinaryDirectoryWatcher = new FileSystemWatcher(this.PrivilegedBinaryDirectory.FullName)
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = true,
            };
            this.CacheDirectoryWatcher = new FileSystemWatcher(this.CacheDirectory.FullName)
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = true,
            };
            this.ConfigDirectoryWatcher = new FileSystemWatcher(this.ConfigDirectory.FullName)
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = true,
            };
            this.LibraryDirectoryWatcher = new FileSystemWatcher(this.LibraryDirectory.FullName)
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = true,
            };
            this.LogDirectoryWatcher = new FileSystemWatcher(this.LogDirectory.FullName)
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = true,
            };
            this.ModuleDirectoryWatcher = new FileSystemWatcher(this.ModuleDirectory.FullName)
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = true,
            };
            this.RuntimeDirectoryWatcher = new FileSystemWatcher(this.RuntimeDirectory.FullName)
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = true,
            };
            this.TempDirectoryWatcher = new FileSystemWatcher(this.TempDirectory.FullName)
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = true,
            };
        }

        private DirectoryInfo GetDirectory(String str)
        {
            return str.StartsWith("%") || str.Contains(":")
                ? new DirectoryInfo(Environment.ExpandEnvironmentVariables(str))
                      .Apply(d => d.Create())
                : this.BaseDirectory.CreateSubdirectory(Environment.ExpandEnvironmentVariables(str));
        }
    }
}