// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * SystemFlow
 *   MetaTweet Input/Output modules which provides generic system instructions
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
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
using XSpect.Extension;
using XSpect.MetaTweet.Objects;

namespace XSpect.MetaTweet.Modules
{
    public class SystemFilter
        : FlowModule
    {
        [FlowInterface("/resolve")]
        public IEnumerable<StorageObject> ResolveReference(IEnumerable<Activity> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            return input.Select(a =>
            {
                switch (a.GetValue<String>().Length)
                {
                    case AccountId.HexStringLength:
                        return (StorageObject) a.GetValue<Account>();
                    case ActivityId.HexStringLength:
                        return a.GetValue<Activity>();
                    default: // AdvertisementId.HexStringLength:
                        return a.GetValue<Advertisement>();
                }
            }).ToArray();
        }

        [FlowInterface("/download")]
        public IEnumerable<StorageObject> Download(IEnumerable<Activity> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            IDisposable _ = session.SuppressDispose();
            return input
                .Do(a => a.Act("Body", ((HttpWebResponse) WebRequest.Create(a.GetValue<String>()).GetResponse()).If(
                    r => (Int32) r.StatusCode < 300,
                    r => new Byte[r.ContentLength].Apply(b => r.GetResponseStream().Dispose(s => s.Read(b, 0, b.Length))),
                    r => new Byte[0]
                )))
                .Finally(_.Dispose);
        }

        [FlowInterface("/download")]
        public IObservable<StorageObject> Download(IObservable<Activity> input, StorageSession session, String param, IDictionary<String, String> args)
        {
            IDisposable _ = session.SuppressDispose();
            return input
                .Do(a => a.Act("Body", ((HttpWebResponse) WebRequest.Create(a.GetValue<String>()).GetResponse()).If(
                    r => (Int32) r.StatusCode < 300,
                    r => new Byte[r.ContentLength].Apply(b => r.GetResponseStream().Dispose(s => s.Read(b, 0, b.Length))),
                    r => new Byte[0]
                )))
                .Finally(_.Dispose);
        }
    }
}