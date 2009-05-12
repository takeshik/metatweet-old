// -*- mode: csharp; encoding: utf-8; -*-
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetConsole
 *   Bandled CLI client / manager for MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetConsole.
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
using System.Linq;
using Achiral;
using Achiral.Extension;
using XSpect;
using XSpect.Extension;
using XSpect.MetaTweet.ObjectModel;

namespace XSpect.MetaTweet.Clients
{
    public sealed class Shell
        : Object
    {
        private readonly MetaTweetClient _client;

        public Shell()
        {
            this._client = new MetaTweetClient();
        }

        public String Evaluate(String str)
        {
            if (str.StartsWith("connect "))
            {
                this._client.Connect(str.Substring("connect ".Length));
            }
            if (str == "disconnect")
            {
                this._client.Disconnect();
            }
            if (str.StartsWith("/"))
            {
                IEnumerable<StorageObject> objects = this._client.Host.Request<IEnumerable<StorageObject>>(Request.Parse(str));
                return objects.Select(o => o.ToString()).Join(Environment.NewLine);
            }
            return String.Empty;
        }
    }
}
