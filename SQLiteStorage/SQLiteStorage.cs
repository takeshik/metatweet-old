// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * SQLiteStorage
 *   MetaTweet Storage module which is provided by SQLite3 RDBMS.
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of SQLiteStorage.
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
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using XSpect.MetaTweet.Objects.Properties;

namespace XSpect.MetaTweet.Objects
{
    public class SQLiteStorage
        : Storage
    {
        public String ConnectionString
        {
            get;
            private set;
        }

        public String ProviderConnectionString
        {
            get
            {
                return Regex.Match(this.ConnectionString, "provider connection string=\"(.*)\"").Groups[1].Value;
            }
        }

        public String DataSource
        {
            get
            {
                return Regex.Match(this.ProviderConnectionString, "data source=\"?(.+)\"?").Groups[1].Value;
            }
        }

        public override void Initialize(IDictionary<String, Object> connectionSettings)
        {
            this.ConnectionString = (String) connectionSettings["ConnectionString"];
            new FileInfo(this.DataSource).Directory.Create();
            using (SQLiteConnection connection = new SQLiteConnection(this.ProviderConnectionString))
            {
                connection.Open();
                foreach (String str in Resources.CreateDatabase.Split(';'))
                {
                    using (SQLiteCommand command = new SQLiteCommand(str, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        protected override StorageSession InitializeSession()
        {
            return new SQLiteStorageSession(this, new StorageObjectContext(this.ConnectionString));
        }
    }
}