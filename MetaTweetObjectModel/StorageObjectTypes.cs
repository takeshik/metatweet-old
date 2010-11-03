// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetObjectModel
 *   Object model and Storage interface for MetaTweet and other systems
 *   Part of MetaTweet
 * Copyright © 2008-2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
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

namespace XSpect.MetaTweet.Objects
{
    /// <summary>
    /// ストレージ オブジェクトの種類を表します。
    /// </summary>
    [Flags()]
    public enum StorageObjectTypes
        : int
    {
        /// <summary>
        /// どのストレージ オブジェクトの種類も示しません。
        /// </summary>
        None = 0x0,

        /// <summary>
        /// アカウント、即ち <see cref="IAccount"/> (略称: Acc) を示します。
        /// </summary>
        Account = 0x100,
        
        /// <summary>
        /// アクティビティ、即ち <see cref="IActivity"/> (略称: Act) を示します。
        /// </summary>
        Activity = 0x200,
        
        /// <summary>
        /// アノテーション、即ち <see cref="IAnnotation"/> (略称: Ann) を示します。
        /// </summary>
        Annotation = 0x10,
        
        /// <summary>
        /// リレーション、即ち <see cref="IRelation"/> (略称: Rel) を示します。
        /// </summary>
        Relation = 0x1,
        
        /// <summary>
        /// マーク、即ち <see cref="IMark"/> (略称: Mrk) を示します。
        /// </summary>
        Mark = 0x4,
        
        /// <summary>
        /// リファレンス、即ち <see cref="IReference"/> (略称: Ref) を示します。
        /// </summary>
        Reference = 0x2,
        
        /// <summary>
        /// タグ、即ち <see cref="ITag"/> (略称: Tag) を示します。
        /// </summary>
        Tag = 0x20,
        
        /// <summary>
        /// 全てのストレージ オブジェクトの種類を示します。
        /// </summary>
        All = Account | Activity | Annotation | Relation | Mark | Reference | Tag,
    }
}