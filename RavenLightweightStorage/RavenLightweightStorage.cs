// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * RavenLightweightStorage
 *   MetaTweet storage which is provided by Raven Document Database (lightweight client)
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of RavenLightweightStorage.
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
using Raven.Client;
using Raven.Client.Document;

namespace XSpect.MetaTweet.Objects
{
    public class RavenLightweightStorage
        : Storage
    {
        private IDocumentStore _store;

        public override void Initialize(IDictionary<String, Object> connectionSettings)
        {
            this._store = new DocumentStore()
            {
                Url = (String) connectionSettings["uri"],
            };
            this._store.Initialize();
            this._store.Conventions.IdentityTypeConvertors.Add(new RavenStorageObjectIdConverter());
        }

        protected override StorageSession InitializeSession()
        {
            return new RavenLightweightStorageSession(this, this._store.OpenSession());
        }
    }
}