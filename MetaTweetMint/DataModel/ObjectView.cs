// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetMint
 *   Extensible GUI client for MetaTweet
 *   Part of MetaTweet
 * Copyright © 2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetMint.
 * 
 * This program is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation; either version 3 of the License, or (at your
 * option) any later version.
 * 
 * This program is distributed in the hope that it will be useful, but
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
using XSpect.Collections;

namespace XSpect.MetaTweet.Clients.Mint.DataModel
{
    public class ObjectView
        : Object
    {
        public String Name
        {
            get;
            private set;
        }

        public ServerConnector ParentConnector
        {
            get;
            private set;
        }

        public HybridDictionary<String, ObjectFilter> Filters
        {
            get;
            private set;
        }

        public Func<ObjectView, IList<Object>> Generator
        {
            get;
            set;
        }

        public IList<String> Columns
        {
            get;
            private set;
        }

        public IList<IList<Object>> Rows
        {
            get;
            private set;
        }

        public ObjectView(String name, ServerConnector parent)
        {
            this.Name = name;
            this.ParentConnector = parent;
            this.Filters = new HybridDictionary<String, ObjectFilter>((i, f) => f.Name);
            this.Columns = new List<String>();
            this.Rows = new List<IList<Object>>();
        }
    }
}
