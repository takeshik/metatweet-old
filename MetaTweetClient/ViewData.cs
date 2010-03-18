// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * MetaTweetClient
 *   Simple GUI client for MetaTweet
 *   Part of MetaTweet
 * Copyright © 2009-2010 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of MetaTweetClient.
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
using XSpect.MetaTweet.Objects;

namespace XSpect.MetaTweet.Clients.Client
{
    [Serializable()]
    public class ViewData
        : Object
    {
        internal Activity Activity
        {
            get;
            private set;
        }

        public String Id
        {
            get
            {
                return this.Activity.SubId;
            }
        }

        public DateTime Timestamp
        {
            get
            {
                return this.Activity.Timestamp;
            }
        }

        public String ScreenName
        {
            get
            {
                return this.Activity.Account["ScreenName"].Value;
            }
        }

        public String Name
        {
            get
            {
                return this.Activity.Account["Name"].Value;
            }
        }

        public String UserAgent
        {
            get
            {
                return this.Activity.UserAgent;
            }
        }

        public String Body
        {
            get
            {
                return this.Activity.Value;
            }
        }

        // TODO: Icons, etc...

        public ViewData(Activity activity)
        {
            this.Activity = activity;
        }
    }
}