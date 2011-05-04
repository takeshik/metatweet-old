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

using System.IO;

namespace XSpect.MetaTweet
{
    /// <summary>
    /// MetaTweet システムのディレクトリ構造を表します。
    /// </summary>
    public interface IDirectoryStructure
    {
        /// <summary>
        /// MetaTweet システムのベースディレクトリを取得します。
        /// </summary>
        /// <value>
        /// MetaTweet システムのベースディレクトリ。
        /// </value>
        DirectoryInfo BaseDirectory
        {
            get;
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
        DirectoryInfo BinaryDirectory
        {
            get;
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
        DirectoryInfo PrivilegedBinaryDirectory
        {
            get;
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
        DirectoryInfo CacheDirectory
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
        DirectoryInfo ConfigDirectory
        {
            get;
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
        DirectoryInfo LibraryDirectory
        {
            get;
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
        DirectoryInfo LogDirectory
        {
            get;
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
        DirectoryInfo ModuleDirectory
        {
            get;
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
        DirectoryInfo RuntimeDirectory
        {
            get;
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
        DirectoryInfo TempDirectory
        {
            get;
        }

        /// <summary>
        /// <see cref="BaseDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="BaseDirectory"/> を監視するコンポーネント。
        /// </value>
        FileSystemWatcher BaseDirectoryWatcher
        {
            get;
        }

        /// <summary>
        /// <see cref="BinaryDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="BinaryDirectory"/> を監視するコンポーネント。
        /// </value>
        FileSystemWatcher BinaryDirectoryWatcher
        {
            get;
        }

        /// <summary>
        /// <see cref="PrivilegedBinaryDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="PrivilegedBinaryDirectory"/> を監視するコンポーネント。
        /// </value>
        FileSystemWatcher PrivilegedBinaryDirectoryWatcher
        {
            get;
        }

        /// <summary>
        /// <see cref="CacheDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="CacheDirectory"/> を監視するコンポーネント。
        /// </value>
        FileSystemWatcher CacheDirectoryWatcher
        {
            get;
        }

        /// <summary>
        /// <see cref="ConfigDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="ConfigDirectory"/> を監視するコンポーネント。
        /// </value>
        FileSystemWatcher ConfigDirectoryWatcher
        {
            get;
        }

        /// <summary>
        /// <see cref="LibraryDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="LibraryDirectory"/> を監視するコンポーネント。
        /// </value>
        FileSystemWatcher LibraryDirectoryWatcher
        {
            get;
        }

        /// <summary>
        /// <see cref="LogDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="LogDirectory"/> を監視するコンポーネント。
        /// </value>
        FileSystemWatcher LogDirectoryWatcher
        {
            get;
        }

        /// <summary>
        /// <see cref="ModuleDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="ModuleDirectory"/> を監視するコンポーネント。
        /// </value>
        FileSystemWatcher ModuleDirectoryWatcher
        {
            get;
        }

        /// <summary>
        /// <see cref="RuntimeDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="RuntimeDirectory"/> を監視するコンポーネント。
        /// </value>
        FileSystemWatcher RuntimeDirectoryWatcher
        {
            get;
        }

        /// <summary>
        /// <see cref="TempDirectory"/> を監視するコンポーネントを取得します。
        /// </summary>
        /// <value>
        /// <see cref="TempDirectory"/> を監視するコンポーネント。
        /// </value>
        FileSystemWatcher TempDirectoryWatcher
        {
            get;
        }
    }
}