// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * SQLiteStorage
 *   MetaTweet Storage module which is provided by SQLite3 RDBMS.
 *   Part of MetaTweet
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@xspect.org>
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

namespace XSpect.MetaTweet
{
    public class SQLiteStorage
        : StorageModule
    {
        private String _connectionString;

        private readonly TableAdapterManager _tableAdapterManager;

        public AccountsTableAdapter Accounts
        {
            get
            {
                return this._tableAdapterManager.AccountsTableAdapter;
            }
        }

        public ActivitiesTableAdapter Activities
        {
            get
            {
                return this._tableAdapterManager.ActivitiesTableAdapter;
            }
        }

        public FavorMapTableAdapter FavorMap
        {
            get
            {
                return this._tableAdapterManager.FavorMapTableAdapter;
            }
        }

        public FollowMapTableAdapter FollowMap
        {
            get
            {
                return this._tableAdapterManager.FollowMapTableAdapter;
            }
        }

        public PostsTableAdapter Posts
        {
            get
            {
                return this._tableAdapterManager.PostsTableAdapter;
            }
        }

        public ReplyMapTableAdapter ReplyMap
        {
            get
            {
                return this._tableAdapterManager.ReplyMapTableAdapter;
            }
        }

        public TagMapTableAdapter TagMap
        {
            get
            {
                return this._tableAdapterManager.TagMapTableAdapter;
            }
        }

        public SQLiteStorage()
        {
            this._tableAdapterManager = new TableAdapterManager();
        }

        public override void Initialize(String connectionString)
        {
            if (!connectionString.ToLowerInvariant().Contains("binaryguid=false"))
            {
                // TODO: exception string resource
                throw new ArgumentException();
            }
            this._connectionString = connectionString;
            this.CreateTables();
        }

        public override void Connect()
        {
            this._tableAdapterManager.AccountsTableAdapter = new AccountsTableAdapter(this._connectionString);
            this._tableAdapterManager.ActivitiesTableAdapter = new ActivitiesTableAdapter(this._connectionString);
            this._tableAdapterManager.FavorMapTableAdapter = new FavorMapTableAdapter(this._connectionString);
            this._tableAdapterManager.FollowMapTableAdapter = new FollowMapTableAdapter(this._connectionString);
            this._tableAdapterManager.PostsTableAdapter = new PostsTableAdapter(this._connectionString);
            this._tableAdapterManager.ReplyMapTableAdapter = new ReplyMapTableAdapter(this._connectionString);
            this._tableAdapterManager.TagMapTableAdapter = new TagMapTableAdapter(this._connectionString);
            this.Load();
        }

        public override void Disconnect()
        {
            this._tableAdapterManager.Dispose();
        }

        public override void Dispose()
        {
            this.Disconnect();
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
                            "Value TEXT NULL," +
                            "Data BLOB NULL," +
                            "PRIMARY KEY (AccountId, TimeStamp, Category)" +
                        ")";
                    command.ExecuteNonQuery();

                    command.CommandText =
                        "CREATE TABLE IF NOT EXISTS FavorMap (" +
                            "AccountId GUID NOT NULL," +
                            "FavoringAccountId GUID NOT NULL," +
                            "FavoringTimestamp DATETIME NOT NULL," +
                            "FavoringCategory TEXT NOT NULL," +
                            "PRIMARY KEY (AccountId, FavoringAccountId, FavoringTimestamp, FavoringCategory)" +
                        ")";
                    command.ExecuteNonQuery();

                    command.CommandText =
                        "CREATE TABLE IF NOT EXISTS TagMap (" +
                            "AccountId GUID NOT NULL," +
                            "Timestamp DATETIME NOT NULL," +
                            "Category TEXT NOT NULL," +
                            "Tag TEXT NOT NULL," +
                            "PRIMARY KEY (AccountId, Timestamp, Category, Tag)" +
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

        public override StorageDataSet.AccountsDataTable GetAccountsDataTable()
        {
            this.UnderlyingDataSet.Accounts.Merge(this.Accounts.GetData(), true);
            return this.UnderlyingDataSet.Accounts;
        }

        public override StorageDataSet.ActivitiesDataTable GetActivitiesDataTable()
        {
            this.UnderlyingDataSet.Activities.Merge(this.Activities.GetData(), true);
            return this.UnderlyingDataSet.Activities;
        }

        public override StorageDataSet.FavorMapDataTable GetFavorMapDataTable()
        {
            this.UnderlyingDataSet.FavorMap.Merge(this.FavorMap.GetData(), true);
            return this.UnderlyingDataSet.FavorMap;
        }

        public override StorageDataSet.FollowMapDataTable GetFollowMapDataTable()
        {
            this.UnderlyingDataSet.FollowMap.Merge(this.FollowMap.GetData(), true);
            return this.UnderlyingDataSet.FollowMap;
        }

        public override StorageDataSet.PostsDataTable GetPostsDataTable()
        {
            this.UnderlyingDataSet.Posts.Merge(this.Posts.GetData(), true);
            return this.UnderlyingDataSet.Posts;
        }

        public override StorageDataSet.ReplyMapDataTable GetReplyMapTable()
        {
            this.UnderlyingDataSet.ReplyMap.Merge(this.ReplyMap.GetData(), true);
            return this.UnderlyingDataSet.ReplyMap;
        }

        public override StorageDataSet.TagMapDataTable GetTagMapDataTable()
        {
            this.UnderlyingDataSet.TagMap.Merge(this.TagMap.GetData(), true);
            return this.UnderlyingDataSet.TagMap;
        }

        public override void Update()
        {
            this._tableAdapterManager.UpdateAll(this.UnderlyingDataSet);
        }

        public override void Merge(IStorage destination)
        {
            this.UnderlyingDataSet.Merge(destination.UnderlyingDataSet);
        }
    }
}