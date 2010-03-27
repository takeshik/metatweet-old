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
using System.ComponentModel;
using XSpect.MetaTweet.Objects;

namespace XSpect.MetaTweet.Clients.Client
{
    [Serializable()]
    public class Configuration
        : Object
    {
        [DisplayName("Server Address")]
        [Description("Address to MetaTweet Server for fetching data.")]
        public String ServerAddress
        {
            get;
            set;
        }

        [DisplayName("Sending Request")]
        [Description("String which represents Server Request to send periodically.")]
        public String SendingRequest
        {
            get;
            set;
        }

        [DisplayName("Posing Request")]
        [Description("String which represents Server Request to send posts to the remote service; '{0}' replaces to the .")]
        public String PostingRequest
        {
            get;
            set;
        }

        [DisplayName("Request Interval")]
        [Description("Interval (milliseconds) to send the request.")]
        public Double RequestInterval
        {
            get;
            set;
        }
    }
}