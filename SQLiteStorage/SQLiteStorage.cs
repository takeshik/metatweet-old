// -*- mode: csharp; encoding: utf-8; -*-
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * SQLiteStorage
 *   MetaTweet Storage module which is provided by SQLite3 RDBMS.
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
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
using System.Data.SQLite;
using XSpect.MetaTweet.StorageDataSetTableAdapters;
using XSpect.MetaTweet.Modules;

namespace XSpect.MetaTweet
{
    public class SQLiteStorage
        : StorageModule
    {
        private String _connectionString;

        public TableAdapterManager TableAdapters
        {
            get;
            private set;
        }

        public SQLiteStorage()
        {
            this.TableAdapters = new TableAdapterManager();
        }

        public override void Initialize(String connectionString)
        {
            if (!connectionString.ToLowerInvariant().Contains("binaryguid=false"))
            {
                throw new ArgumentException("Parameter \"binaryguid=false\" is required in connection string.");
            }
            this._connectionString = connectionString;
            this.CreateTables();

            this.TableAdapters.AccountsTableAdapter = new AccountsTableAdapter(this._connectionString);
            this.TableAdapters.ActivitiesTableAdapter = new ActivitiesTableAdapter(this._connectionString);
            this.TableAdapters.FavorMapTableAdapter = new FavorMapTableAdapter(this._connectionString);
            this.TableAdapters.FollowMapTableAdapter = new FollowMapTableAdapter(this._connectionString);
            this.TableAdapters.PostsTableAdapter = new PostsTableAdapter(this._connectionString);
            this.TableAdapters.ReplyMapTableAdapter = new ReplyMapTableAdapter(this._connectionString);
            this.TableAdapters.TagMapTableAdapter = new TagMapTableAdapter(this._connectionString);
            
            base.Initialize();
        }

        protected override void Dispose(Boolean disposing)
        {
 	        this.TableAdapters.Dispose();
            base.Dispose(disposing);
        }

        public virtual void CreateTables()
        {
            this.CheckIfDisposed();
            using (SQLiteConnection connection = new SQLiteConnection(this._connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        "CREATE TABLE IF NOT EXISTS Accounts (" +
                            "AccountId GUID NOT NULL," +
                            "Realm TEXT NOT NULL," +
                            "PRIMARY KEY (AccountId)" +
                        ")";
                    command.ExecuteNonQuery();

                    command.CommandText =
                        "CREATE TABLE IF NOT EXISTS FollowMap (" +
                            "AccountId GUID NOT NULL," +
                            "FollowingAccountId GUID NOT NULL," +
                            "PRIMARY KEY (AccountId, FollowingAccountId)" +
                        ")";
                    command.ExecuteNonQuery();

                    command.CommandText =
                        "CREATE TABLE IF NOT EXISTS Activities (" +
                            "AccountId GUID NOT NULL," +
                            "Timestamp DATETIME NOT NULL," +
                            "Category TEXT NOT NULL," +
                            "Subindex INT NOT NULL," +
                            "Value TEXT NULL," +
                            "Data BLOB NULL," +
                            "PRIMARY KEY (AccountId, TimeStamp, Category, Subindex)" +
                        ")";
                    command.ExecuteNonQuery();

                    command.CommandText =
                        "CREATE TABLE IF NOT EXISTS FavorMap (" +
                            "AccountId GUID NOT NULL," +
                            "FavoringAccountId GUID NOT NULL," +
                            "FavoringTimestamp DATETIME NOT NULL," +
                            "FavoringCategory TEXT NOT NULL," +
                            "FavoringSubindex INT NOT NULL," +
                            "PRIMARY KEY (AccountId, FavoringAccountId, FavoringTimestamp, FavoringCategory, FavoringSubindex)" +
                        ")";
                    command.ExecuteNonQuery();

                    command.CommandText =
                        "CREATE TABLE IF NOT EXISTS TagMap (" +
                            "AccountId GUID NOT NULL," +
                            "Timestamp DATETIME NOT NULL," +
                            "Category TEXT NOT NULL," +
                            "Subindex INT NOT NULL," +
                            "Tag TEXT NOT NULL," +
                            "PRIMARY KEY (AccountId, Timestamp, Category, Subindex, Tag)" +
                        ")";
                    command.ExecuteNonQuery();

                    command.CommandText =
                        "CREATE TABLE IF NOT EXISTS Posts (" +
                            "AccountId GUID NOT NULL," +
                            "PostId TEXT NOT NULL," +
                            "Text TEXT NULL," +
                            "Source TEXT NULL," +
                            "PRIMARY KEY (AccountId, PostId)" +
                        ")";
                    command.ExecuteNonQuery();

                    command.CommandText =
                        "CREATE TABLE IF NOT EXISTS ReplyMap (" +
                            "AccountId GUID NOT NULL," +
                            "PostId TEXT NOT NULL," +
                            "InReplyToAccountId GUID NOT NULL," +
                            "InReplyToPostId TEXT NOT NULL," +
                            "PRIMARY KEY (AccountId, PostId, InReplyToAccountId, InReplyToPostId)" +
                        ")";
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public virtual void DropTables()
        {
            this.CheckIfDisposed();
            using (SQLiteConnection connection = new SQLiteConnection(this._connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"DROP TABLE IF EXISTS TagMap";
                    command.ExecuteNonQuery();

                    command.CommandText = @"DROP TABLE IF EXISTS FavorMap";
                    command.ExecuteNonQuery();

                    command.CommandText = @"DROP TABLE IF EXISTS ReplyMap";
                    command.ExecuteNonQuery();

                    command.CommandText = @"DROP TABLE IF EXISTS Posts";
                    command.ExecuteNonQuery();

                    command.CommandText = @"DROP TABLE IF EXISTS Activities";
                    command.ExecuteNonQuery();

                    command.CommandText = @"DROP TABLE IF EXISTS FollowMap";
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
            using (SQLiteConnection connection = new SQLiteConnection(this._connectionString))
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
            using (SQLiteConnection connection = new SQLiteConnection(this._connectionString))
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
            using (SQLiteConnection connection = new SQLiteConnection(this._connectionString))
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

        protected override StorageDataSet.AccountsDataTable LoadAccountsDataTableImpl(String clauses)
        {
            StorageDataSet.AccountsDataTable table
                = this.TableAdapters.AccountsTableAdapter.GetDataBy("SELECT [Accounts].* FROM [Accounts] " + clauses);
            if (table.Count > 0)
            {
                this.UnderlyingDataSet.Accounts.Merge(table, true);
            }
            return table;
        }

        protected override StorageDataSet.ActivitiesDataTable LoadActivitiesDataTableImpl(String clauses)
        {
            StorageDataSet.ActivitiesDataTable table
                = this.TableAdapters.ActivitiesTableAdapter.GetDataBy("SELECT [Activities].* FROM [Activities] " + clauses);
            if (table.Count > 0)
            {
                this.UnderlyingDataSet.Activities.Merge(table, true);
            }
            return table;
        }

        protected override StorageDataSet.FavorMapDataTable LoadFavorMapDataTableImpl(String clauses)
        {
            StorageDataSet.FavorMapDataTable table
                = this.TableAdapters.FavorMapTableAdapter.GetDataBy("SELECT [FavorMap].* FROM [FavorMap] " + clauses);
            if (table.Count > 0)
            {
                this.UnderlyingDataSet.FavorMap.Merge(table, true);
            }
            return table;
        }

        protected override StorageDataSet.FollowMapDataTable LoadFollowMapDataTableImpl(String clauses)
        {
            StorageDataSet.FollowMapDataTable table
                = this.TableAdapters.FollowMapTableAdapter.GetDataBy("SELECT [FollowMap].* FROM [FollowMap] " + clauses);
            if (table.Count > 0)
            {
                this.UnderlyingDataSet.FollowMap.Merge(table, true);
            }
            return table;
        }

        protected override StorageDataSet.PostsDataTable LoadPostsDataTableImpl(String clauses)
        {
            StorageDataSet.PostsDataTable table
                = this.TableAdapters.PostsTableAdapter.GetDataBy("SELECT [Posts].* FROM [Posts] " + clauses);
            if (table.Count > 0)
            {
                this.UnderlyingDataSet.Posts.Merge(table, true);
            }
            return table;
        }

        protected override StorageDataSet.ReplyMapDataTable LoadReplyMapDataTableImpl(String clauses)
        {
            StorageDataSet.ReplyMapDataTable table
                = this.TableAdapters.ReplyMapTableAdapter.GetDataBy("SELECT [ReplyMap].* FROM [ReplyMap] " + clauses);
            if (table.Count > 0)
            {
                this.UnderlyingDataSet.ReplyMap.Merge(table, true);
            }
            return table;
        }

        protected override StorageDataSet.TagMapDataTable LoadTagMapDataTableImpl(String clauses)
        {
            StorageDataSet.TagMapDataTable table
                = this.TableAdapters.TagMapTableAdapter.GetDataBy("SELECT [TagMap].* FROM [TagMap] " + clauses);
            if (table.Count > 0)
            {
                this.UnderlyingDataSet.TagMap.Merge(table, true);
            }
            return table;
        }

        public override void Update()
        {
            this.CheckIfDisposed();
            this.TableAdapters.UpdateAll(this.UnderlyingDataSet);
        }
    }
}