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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Data.Objects;

namespace XSpect.MetaTweet.Objects
{
    partial class StorageObjectContext
    {
        public MergeOption MergeOption
        {
            get
            {
                if (
                    this.Accounts.MergeOption == this.Activities.MergeOption &&
                    this.Activities.MergeOption == this.Annotations.MergeOption &&
                    this.Annotations.MergeOption == this.Relations.MergeOption &&
                    this.Relations.MergeOption == this.Marks.MergeOption &&
                    this.Marks.MergeOption == this.References.MergeOption &&
                    this.References.MergeOption == this.Tags.MergeOption &&
                    this.Tags.MergeOption == this.Accounts.MergeOption
                )
                {
                    return this.Accounts.MergeOption;
                }
                else
                {
                    throw new InvalidOperationException("Value of MergeOption properties are not same.");
                }
            }
            set
            {
                this.Accounts.MergeOption = value;
                this.Activities.MergeOption = value;
                this.Annotations.MergeOption = value;
                this.Relations.MergeOption = value;
                this.Marks.MergeOption = value;
                this.References.MergeOption = value;
                this.Tags.MergeOption = value;
            }
        }

        public Boolean IsDisposed
        {
            get
            {
                // HACK: Depends on internal structure, accessing non-public field
                return typeof(ObjectContext)
                    .GetField("_connection", BindingFlags.Instance | BindingFlags.NonPublic)
                    .GetValue(this) == null;
            }
        }
    }
}