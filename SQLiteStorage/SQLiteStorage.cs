// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * SQLiteStorage
 *   MetaTweet Storage module which is provided by SQLite3 RDBMS.
 *   Part of MetaTweet
 * Copyright c 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
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
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using XSpect.Extension;

namespace XSpect.MetaTweet.Modules
{
    public class SQLiteStorage
        : StorageModule
    {
        private String _connectionString;

        public String ConnectionString
        {
            get
            {
                return this.Entities != null
                    ? this.Entities.Connection.ConnectionString
                    : this._connectionString;
            }
            set
            {
                if (this.Entities == null)
                {
                    this._connectionString = value;
                }
            }
        }

        public String ProviderConnectionString
        {
            get
            {
                return Regex.Match(
                    this.ConnectionString,
                    "provider connection string=\"(.*)\""
                ).Groups[1].Value;
            }
        }

        public FileInfo DataSource
        {
            get
            {
                return this.Host.Directories.BaseDirectory.File(Regex.Match(
                    this.ProviderConnectionString,
                    "data source=\"?(.+?)\"?;"
                ).Groups[1].Value);
            }
        }

        public override void Initialize(String connectionString)
        {
            this.ConnectionString = connectionString;
            if (!this.ConnectionString.ToLowerInvariant().Contains("binaryguid=false"))
            {
                throw new ArgumentException("Parameter \"BinaryGUID=false\" is required in connection string.");
            }
            this.CreateTables();
            base.Initialize(this.ConnectionString);
        }

        public virtual void CreateTables()
        {
            this.CheckIfDisposed();
            this.DataSource.Directory.Create();
            using (SQLiteConnection connection = new SQLiteConnection(this.ProviderConnectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = String.Concat(
                        "CREATE TABLE IF NOT EXISTS [accounts] (",
                            "[account_id] GUID NOT NULL,",
                            "[realm] TEXT NOT NULL,",
                            "PRIMARY KEY (",
                                "[account_id]",
                            ")",
                        ")"
                    );
                    command.ExecuteNonQuery();

                    command.CommandText = String.Concat(
                        "CREATE TABLE IF NOT EXISTS [activities] (",
                            "[account_id] GUID NOT NULL,",
                            "[timestamp] DATETIME NOT NULL,",
                            "[category] TEXT NOT NULL,",
                            "[sub_id] TEXT NOT NULL,",
                            "[user_agent] TEXT NULL,",
                            "[value] TEXT NULL,",
                            "[data] BLOB NULL,",
                            "PRIMARY KEY (",
                                "[account_id],",
                                "[timestamp],",
                                "[category],",
                                "[sub_id]",
                            ")",
                        ")"
                    );
                    command.ExecuteNonQuery();

                    command.CommandText = String.Concat(
                        "CREATE TABLE IF NOT EXISTS [annotations] (",
                            "[account_id] GUID NOT NULL,",
                            "[name] TEXT NOT NULL,",
                            "PRIMARY KEY (",
                                "[account_id],",
                                "[name]",
                            ")",
                        ")"
                    );
                    command.ExecuteNonQuery();

                    command.CommandText = String.Concat(
                        "CREATE TABLE IF NOT EXISTS [relations] (",
                            "[account_id] GUID NOT NULL,",
                            "[name] TEXT NOT NULL,",
                            "[relating_account_id] GUID NOT NULL,",
                            "PRIMARY KEY (",
                                "[account_id],",
                                "[name],",
                                "[relating_account_id]",
                            ")",
                        ")"
                    );
                    command.ExecuteNonQuery();

                    command.CommandText = String.Concat(
                        "CREATE TABLE IF NOT EXISTS [marks] (",
                            "[account_id] GUID NOT NULL,",
                            "[name] TEXT NOT NULL,",
                            "[marking_account_id] GUID NOT NULL,",
                            "[marking_timestamp] DATETIME NOT NULL,",
                            "[marking_category] TEXT NOT NULL,",
                            "[marking_sub_id] TEXT NOT NULL,",
                            "PRIMARY KEY (",
                                "[account_id],",
                                "[name],",
                                "[marking_account_id],",
                                "[marking_timestamp],",
                                "[marking_category],",
                                "[marking_sub_id]",
                            ")",
                        ")"
                    );
                    command.ExecuteNonQuery();

                    command.CommandText = String.Concat(
                        "CREATE TABLE IF NOT EXISTS [references] (",
                            "[account_id] GUID NOT NULL,",
                            "[timestamp] DATETIME NOT NULL,",
                            "[category] TEXT NOT NULL,",
                            "[sub_id] TEXT NOT NULL,",
                            "[name] TEXT NOT NULL,",
                            "[referring_account_id] GUID NOT NULL,",
                            "[referring_timestamp] DATETIME NOT NULL,",
                            "[referring_category] TEXT NOT NULL,",
                            "[referring_sub_id] TEXT NOT NULL,",
                            "PRIMARY KEY (",
                                "[account_id],",
                                "[timestamp],",
                                "[category],",
                                "[sub_id],",
                                "[name],",
                                "[referring_account_id],",
                                "[referring_timestamp],",
                                "[referring_category],",
                                "[referring_sub_id]",
                            ")",
                        ")"
                    );
                    command.ExecuteNonQuery();

                    command.CommandText = String.Concat(
                        "CREATE TABLE IF NOT EXISTS [tags] (",
                            "[account_id] GUID NOT NULL,",
                            "[timestamp] DATETIME NOT NULL,",
                            "[category] TEXT NOT NULL,",
                            "[sub_id] TEXT NOT NULL,",
                            "[name] TEXT NOT NULL,",
                            "PRIMARY KEY (",
                                "[account_id],",
                                "[timestamp],",
                                "[category],",
                                "[sub_id],",
                                "[name]",
                            ")",
                        ")"
                    );
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public virtual void DropTables()
        {
            this.CheckIfDisposed();
            using (SQLiteConnection connection = new SQLiteConnection(this.ConnectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"DROP TABLE IF EXISTS Tags";
                    command.ExecuteNonQuery();

                    command.CommandText = @"DROP TABLE IF EXISTS References";
                    command.ExecuteNonQuery();

                    command.CommandText = @"DROP TABLE IF EXISTS Marks";
                    command.ExecuteNonQuery();

                    command.CommandText = @"DROP TABLE IF EXISTS Relations";
                    command.ExecuteNonQuery();

                    command.CommandText = @"DROP TABLE IF EXISTS Annotations";
                    command.ExecuteNonQuery();

                    command.CommandText = @"DROP TABLE IF EXISTS Activities";
                    command.ExecuteNonQuery();

                    command.CommandText = @"DROP TABLE IF EXISTS Accounts";
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public virtual void Vacuum()
        {
            this.CheckIfDisposed();
            using (SQLiteConnection connection = new SQLiteConnection(this.ConnectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"VACUUM";
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public virtual void Attach(String name, String path)
        {
            this.CheckIfDisposed();
            using (SQLiteConnection connection = new SQLiteConnection(this.ConnectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = String.Format(@"ATTACH DATABASE {0} AS {1}", path, name);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public virtual void Detach(String name)
        {
            this.CheckIfDisposed();
            using (SQLiteConnection connection = new SQLiteConnection(this.ConnectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = String.Format(@"DETACH DATABASE {0}", name);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}