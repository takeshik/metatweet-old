// -*- mode: csharp; encoding: utf-8; -*-
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
using System.Collections.Generic;
using System.Linq;

namespace XSpect.MetaTweet.ObjectModel
{
    /// <summary>
    /// <see cref="String"/> クラスに対する <see cref="XSpect.MetaTweet.StorageDataSet.TagMapDataTable"/> クラスに対する
    /// 操作のための拡張メソッドを定義します。これは静的クラスです。
    /// </summary>
    public static class StringTagMapExtensions
        : Object
    {
        /// <summary>
        /// 指定された文字列をタグとして付与されているアクティビティの一覧を取得します。
        /// </summary>
        /// <param name="tag">検索するタグの文字列。</param>
        /// <param name="storage">検索に使用するストレージ。</param>
        /// <returns>指定された文字列をタグとして付与されているアクティビティの一覧。</returns>
        public static IEnumerable<Activity> GetTaggedActivities(this String tag, Storage storage)
        {
            // TODO: Consider to write more smart
            return storage.GetTagElements(r => r.Tag == tag).Select(e => e.Activity);
        }
    }
}