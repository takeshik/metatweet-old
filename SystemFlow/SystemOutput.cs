// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * SystemFlow
 *   MetaTweet Input/Output modules which provides generic system instructions
 *   Part of MetaTweet
 * Copyright © 2008-2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of TwitterApiFlow.
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
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using XSpect.Configuration;
using XSpect.Extension;
using XSpect.MetaTweet.Modules;
using XSpect.MetaTweet.Objects;
using XSpect.Net;
using Achiral;
using Achiral.Extension;

namespace XSpect.MetaTweet.Modules
{
    public class SystemOutput
        : OutputFlowModule
    {
        protected override String DefaultRealm
        {
            get
            {
                return String.Empty;
            }
        }

        [FlowInterface("/.null", WriteTo = StorageObjectTypes.None)]
        public Object OutputNull(IEnumerable<StorageObject> source, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return null;
        }

        [FlowInterface("/.null", WriteTo = StorageObjectTypes.None)]
        public String OutputNullString(IEnumerable<StorageObject> source, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return String.Empty;
        }

        [FlowInterface("/.obj", WriteTo = StorageObjectTypes.None)]
        public IEnumerable<StorageObject> OutputStorageObjects(IEnumerable<StorageObject> source, StorageModule storage, String param, IDictionary<String, String> args)
        {
            return source;
        }
    }
}