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
                // TODO: exception string resource
                throw new ArgumentException();
            }
            this._connectionString = connectionString;
            this.CreateTables();
        }

        public override void Connect()
        {
            this.TableAdapters.AccountsTableAdapter = new AccountsTableAdapter(this._connectionString);
            this.TableAdapters.ActivitiesTableAdapter = new ActivitiesTableAdapter(this._connectionString);
            this.TableAdapters.FavorMapTableAdapter = new FavorMapTableAdapter(this._connectionString);
            this.TableAdapters.FollowMapTableAdapter = new FollowMapTableAdapter(this._connectionString);
            this.TableAdapters.PostsTableAdapter = new PostsTableAdapter(this._connectionString);
            this.TableAdapters.ReplyMapTableAdapter = new ReplyMapTableAdapter(this._connectionString);
            this.TableAdapters.TagMapTableAdapter = new TagMapTableAdapter(this._connectionString);
        }

        public override void Disconnect()
        {
            this.TableAdapters.Dispose();
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

        public override Int32 FillAccountsBy(String query, params Object[] args)
        {
            return this.TableAdapters.AccountsTableAdapter.FillBy(
                this.UnderlyingDataSet.Accounts,
                String.Format(query, args)
            );
        }

        public override Int32 FillActivitiesBy(String query, params Object[] args)
        {
            return this.TableAdapters.ActivitiesTableAdapter.FillBy(
                this.UnderlyingDataSet.Activities,
                String.Format(query, args)
            );
        }

        public override Int32 FillFavorMapBy(String query, params Object[] args)
        {
            return this.TableAdapters.FavorMapTableAdapter.FillBy(
                this.UnderlyingDataSet.FavorMap,
                String.Format(query, args)
            );
        }

        public override Int32 FillFollowMapBy(String query, params Object[] args)
        {
            return this.TableAdapters.FollowMapTableAdapter.FillBy(
                this.UnderlyingDataSet.FollowMap,
                String.Format(query, args)
            );
        }

        public override Int32 FillPostsBy(String query, params Object[] args)
        {
            return this.TableAdapters.PostsTableAdapter.FillBy(
                this.UnderlyingDataSet.Posts,
                String.Format(query, args)
            );
        }

        public override Int32 FillReplyMapBy(String query, params Object[] args)
        {
            return this.TableAdapters.ReplyMapTableAdapter.FillBy(
                this.UnderlyingDataSet.ReplyMap,
                String.Format(query, args)
            );
        }

        public override Int32 FillTagMapBy(String query, params Object[] args)
        {
            return this.TableAdapters.TagMapTableAdapter.FillBy(
                this.UnderlyingDataSet.TagMap,
                String.Format(query, args)
            );
        }

        public override void Update()
        {
            this.TableAdapters.UpdateAll(this.UnderlyingDataSet);
        }

        public override void Merge(Storage destination)
        {
            this.UnderlyingDataSet.Merge(destination.UnderlyingDataSet);
        }
    }
}