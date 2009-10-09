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

namespace XSpect.MetaTweet.Objects
{
    partial class Mark
        : IMark
    {
        internal Mark(Storage storage)
            : base(storage)
        {
        }

        public override String ToString()
        {
            return String.Format(
                "Mrk [{0}]: {1} -> [{2}]",
                this.Account,
                this.Name,
                this.MarkingActivity
            );
        }

        public override Int32 CompareTo(StorageObject other)
        {
            if (!(other is Mark))
            {
                throw new ArgumentException("other");
            }
            return this.CompareTo(other as Mark);
        }

        public Int32 CompareTo(IMark other)
        {
            // Account -> Name -> MarkingActivity
            Int32 result;
            return (result = this.Account.CompareTo(other.Account)) != 0
                ? result
                : (result = this.Name.CompareTo(other.Name)) != 0
                      ? result
                      : this.MarkingActivity.CompareTo(other.MarkingActivity);
        }
    }
}