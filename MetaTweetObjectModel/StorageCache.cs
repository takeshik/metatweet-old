// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetObjectModel
 *   Object model and Storage interface for MetaTweet and other systems
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetObjectModel.
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
using System.Runtime.Serialization.Formatters.Binary;

namespace XSpect.MetaTweet.Objects
{
    [Serializable()]
    public partial class StorageCache
        : Object
    {
        [NonSerialized()]
        private FileInfo _cacheFile;

        [NonSerialized()]
        private Storage _storage;

        /// <summary>
        /// キャッシュのソースとなるストレージを取得します。
        /// </summary>
        /// <value>
        /// キャッシュのソースとなるストレージ。
        /// </value>
        public Storage Storage
        {
            get
            {
                return this._storage;
            }
            private set
            {
                this._storage = value;
            }
        }

        /// <summary>
        /// キャッシュ ファイルを保存または読み込んだファイルを取得します。
        /// </summary>
        /// <value>
        /// キャッシュ ファイルを保存または読み込んだファイル。
        /// </value>
        public FileInfo CacheFile
        {
            get
            {
                return this._cacheFile;
            }
            private set
            {
                this._cacheFile = value;
            }
        }

        /// <summary>
        /// <see cref="ActivityCache"/> を取得します。
        /// </summary>
        /// <value>
        /// <see cref="ActivityCache"/>。
        /// </value>
        public ActivityCache Activities
        {
            get;
            private set;
        }

        /// <summary>
        /// <see cref="StorageCache"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="storage">キャッシュのソースとなるストレージ。</param>
        public StorageCache(Storage storage)
        {
            this.Storage = storage;
            this.Activities = new ActivityCache(this);
        }


        /// <summary>
        /// ファイルからキャッシュ データを読み込みます。
        /// </summary>
        /// <param name="file">読み込むファイル。</param>
        /// <param name="storage">キャッシュのソースとなるストレージ。</param>
        /// <returns>ファイルから読み込まれた <see cref="StorageCache"/>。</returns>
        public static StorageCache Load(FileInfo file, Storage storage)
        {
            using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                StorageCache cache = new BinaryFormatter().Deserialize(stream) as StorageCache;
                cache.CacheFile = file;
                cache.Storage = storage;
                // TODO: Is below Correct? Is it works?
                // NOTE: There is no longer Connect(), Storage is non-serialized.
                foreach (Activity activity in cache.Activities)
                {
                    activity.Storage = cache.Storage;
                }
                return cache;
            }
        }

        /// <summary>
        /// キャッシュ データをファイルに保存します。
        /// </summary>
        /// <param name="file">保存先のファイル。</param>
        public void Save(FileInfo file)
        {
            using (FileStream stream = file.Open(FileMode.Create, FileAccess.Write, FileShare.None))
            {
                new BinaryFormatter().Serialize(stream, this);
            }
            this.CacheFile = file;
        }

        /// <summary>
        /// キャッシュ データを保存または読み込んだファイルに保存します。
        /// </summary>
        public void Save()
        {
            this.Save(this.CacheFile);
        }
    }
}