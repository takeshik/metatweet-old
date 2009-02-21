// -*- mode: csharp; encoding: utf-8; -*-
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
        }

        public override void Dispose()
        {
 	        this.TableAdapters.Dispose();
            base.Dispose();
        }

        public virtual void CreateTables()
        {
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
                            "Timestamp DATETIME NOT NULL," +
                            "PostId TEXT NOT NULL," +
                            "Text TEXT NULL," +
                            "Source TEXT NULL," +
                            "PRIMARY KEY (AccountId, Timestamp, PostId)" +
                        ")";
                    command.ExecuteNonQuery();

                    command.CommandText =
                        "CREATE TABLE IF NOT EXISTS ReplyMap (" +
                            "AccountId GUID NOT NULL," +
                            "Timestamp DATETIME NOT NULL," +
                            "PostId TEXT NOT NULL," +
                            "InReplyToAccountId GUID NOT NULL," +
                            "InReplyToTimestamp DATETIME NOT NULL," +
                            "InReplyToPostId TEXT NOT NULL," +
                            "PRIMARY KEY (AccountId, Timestamp, PostId, InReplyToAccountId, InReplyToTimestamp, InReplyToPostId)" +
                        ")";
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public virtual void DropTables()
        {
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

        public override StorageDataSet.AccountsDataTable LoadAccountsDataTable()
        {
            return this.LoadAccountsDataTableBy("SELECT [Accounts].* FROM [Accounts]");
        }

        public override StorageDataSet.AccountsDataTable LoadAccountsDataTableBy(String query, params Object[] args)
        {
            StorageDataSet.AccountsDataTable table
                = this.TableAdapters.AccountsTableAdapter.GetDataBy(query, args);
            this.UnderlyingDataSet.Accounts.Merge(table);
            return table;
        }

        public override StorageDataSet.ActivitiesDataTable LoadActivitiesDataTable()
        {
            return this.LoadActivitiesDataTableBy("SELECT [Activities].* FROM [Activities]");
        }

        public override StorageDataSet.ActivitiesDataTable LoadActivitiesDataTableBy(String query, params Object[] args)
        {
            StorageDataSet.ActivitiesDataTable table
                = this.TableAdapters.ActivitiesTableAdapter.GetDataBy(query, args);
            this.UnderlyingDataSet.Activities.Merge(table);
            return table;
        }

        public override StorageDataSet.FavorMapDataTable LoadFavorMapDataTable()
        {
            return this.LoadFavorMapDataTableBy("SELECT [FavorMap].* FROM [FavorMap]");
        }

        public override StorageDataSet.FavorMapDataTable LoadFavorMapDataTableBy(String query, params Object[] args)
        {
            StorageDataSet.FavorMapDataTable table
                = this.TableAdapters.FavorMapTableAdapter.GetDataBy(query, args);
            this.UnderlyingDataSet.FavorMap.Merge(table);
            return table;
        }

        public override StorageDataSet.FollowMapDataTable LoadFollowMapDataTable()
        {
            return this.LoadFollowMapDataTableBy("SELECT [FollowMap].* FROM [FollowMap]");
        }

        public override StorageDataSet.FollowMapDataTable LoadFollowMapDataTableBy(String query, params Object[] args)
        {
            StorageDataSet.FollowMapDataTable table
                = this.TableAdapters.FollowMapTableAdapter.GetDataBy(query, args);
            this.UnderlyingDataSet.FollowMap.Merge(table);
            return table;
        }

        public override StorageDataSet.PostsDataTable LoadPostsDataTable()
        {
            return this.LoadPostsDataTableBy("SELECT [Posts].* FROM [Posts]");
        }

        public override StorageDataSet.PostsDataTable LoadPostsDataTableBy(String query, params Object[] args)
        {
            StorageDataSet.PostsDataTable table
                = this.TableAdapters.PostsTableAdapter.GetDataBy(query, args);
            this.UnderlyingDataSet.Posts.Merge(table);
            return table;
        }

        public override StorageDataSet.ReplyMapDataTable LoadReplyMapDataTable()
        {
            return this.LoadReplyMapDataTableBy("SELECT [ReplyMap].* FROM [ReplyMap]");
        }

        public override StorageDataSet.ReplyMapDataTable LoadReplyMapDataTableBy(String query, params Object[] args)
        {
            StorageDataSet.ReplyMapDataTable table
                = this.TableAdapters.ReplyMapTableAdapter.GetDataBy(query, args);
            this.UnderlyingDataSet.ReplyMap.Merge(table);
            return table;
        }

        public override StorageDataSet.TagMapDataTable LoadTagMapDataTable()
        {
            return this.LoadTagMapDataTableBy("SELECT [TagMap].* FROM [TagMap]");
        }

        public override StorageDataSet.TagMapDataTable LoadTagMapDataTableBy(String query, params Object[] args)
        {
            StorageDataSet.TagMapDataTable table
                = this.TableAdapters.TagMapTableAdapter.GetDataBy(query, args);
            this.UnderlyingDataSet.TagMap.Merge(table);
            return table;
        }

        public override void Update()
        {
            this.TableAdapters.UpdateAll(this.UnderlyingDataSet);
        }
    }
}