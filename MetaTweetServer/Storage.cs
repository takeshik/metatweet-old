// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system of Twitter-like communication service
 * MetaTweetServer
 *   Server library of MetaTweet
 *   Part of MetaTweet
 * Copyright © 2008 Takeshi KIRIYA, XSpect Project <takeshik@xspect.org>
 * All rights reserved.
 * 
 * This file is part of MetaTweetServer.
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
using XSpect.MetaTweet.StorageDataSetTableAdapters;

namespace XSpect.MetaTweet
{
    public class Storage
        : IDisposable
    {
        private readonly String _connectionString;

        private readonly AccountsTableAdapter _accountsTableAdapter;

        private readonly ActivitiesTableAdapter _activitiesTableAdapter;

        private readonly FollowMapTableAdapter _followMapTableAdapter;

        private readonly PicturesTableAdapter _picturesTableAdapter;

        private readonly PostsTableAdapter _postsTableAdapter;

        private readonly ReplyMapTableAdapter _replyMapTableAdapter;

        public AccountsTableAdapter Accounts
        {
            get
            {
                return this._accountsTableAdapter;
            }
        }

        public ActivitiesTableAdapter Activities
        {
            get
            {
                return this._activitiesTableAdapter;
            }
        }

        public FollowMapTableAdapter FollowMap
        {
            get
            {
                return this._followMapTableAdapter;
            }
        }

        public PicturesTableAdapter Pictures
        {
            get
            {
                return this._picturesTableAdapter;
            }
        }

        public PostsTableAdapter Posts
        {
            get
            {
                return this._postsTableAdapter;
            }
        }

        public ReplyMapTableAdapter ReplyMap
        {
            get
            {
                return this._replyMapTableAdapter;
            }
        }

        public Storage(String connectionString)
        {
            this._connectionString = connectionString;
            this._accountsTableAdapter = new AccountsTableAdapter(connectionString);
            this._activitiesTableAdapter = new ActivitiesTableAdapter(connectionString);
            this._followMapTableAdapter = new FollowMapTableAdapter(connectionString);
            this._picturesTableAdapter = new PicturesTableAdapter(connectionString);
            this._postsTableAdapter = new PostsTableAdapter(connectionString);
            this._replyMapTableAdapter = new ReplyMapTableAdapter(connectionString);
        }

        public virtual void CreateTable()
        {
            using (SQLiteConnection connection = new SQLiteConnection(this._connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        "CREATE TABLE IF NOT EXISTS Accounts (" +
                            "AccountId GUID," +
                            "Realm TEXT NOT NULL," +
                            "Tags TEXT NOT NULL," +
                            "PRIMARY KEY (AccountId)" +
                        ")";
                    command.ExecuteNonQuery();

                    command.CommandText =
                        "CREATE TABLE IF NOT EXISTS FollowMap (" +
                            "AccountId GUID NOT NULL," +
                            "FollowingAccountId GUID NOT NULL" +
                        ")";
                    command.ExecuteNonQuery();

                    command.CommandText =
                        "CREATE TABLE IF NOT EXISTS Activities (" +
                            "AccountId GUID," +
                            "Timestamp DATETIME," +
                            "Category TEXT NOT NULL," +
                            "Value TEXT," +
                            "Data BLOB," +
                            "Tags TEXT NOT NULL," +
                            "PRIMARY KEY (AccountId, TimeStamp, Category)" +
                        ")";
                    command.ExecuteNonQuery();

                    command.CommandText =
                        "CREATE TABLE IF NOT EXISTS Posts (" +
                            "AccountId GUID NOT NULL," +
                            "PostId TEXT NOT NULL," +
                            "Text TEXT NOT NULL," +
                            "Source TEXT NOT NULL," +
                            "FavoriteCount INT," +
                            "IsRead BIT NOT NULL," +
                            "IsFavorited BIT NOT NULL," +
                            "IsReply BIT NOT NULL," +
                            "IsRestricted BIT NOT NULL," +
                            "PRIMARY KEY (AccountId, PostId)" +
                        ")";
                    command.ExecuteNonQuery();

                    command.CommandText =
                        "CREATE TABLE IF NOT EXISTS ReplyMap (" +
                            "AccountId GUID NOT NULL," +
                            "PostId TEXT NOT NULL," +
                            "InReplyToAccountId GUID NOT NULL," +
                            "InReplyToPostId INTEGER NOT NULL" +
                        ")";
                    command.ExecuteNonQuery();

                    command.CommandText =
                        "CREATE TABLE IF NOT EXISTS Pictures (" +
                            "ImageId TEXT NOT NULL," +
                            "Width INT NOT NULL," +
                            "Height INT NOT NULL," +
                            "Image BLOB NOT NULL," +
                            "PRIMARY KEY (ImageId)" +
                        ")";
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public virtual void DropTable()
        {
            using (SQLiteConnection connection = new SQLiteConnection(this._connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"DROP TABLE IF EXISTS Pictures";
                    command.ExecuteNonQuery();

                    command.CommandText = @"DROP TABLE IF EXISTS ReplyMap";
                    command.ExecuteNonQuery();

                    command.CommandText = @"DROP TABLE IF EXISTS Posts";
                    command.ExecuteNonQuery();

                    command.CommandText = @"DROP TABLE IF EXISTS Events";
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

        public virtual void Dispose()
        {
            this._replyMapTableAdapter.Dispose();
            this._postsTableAdapter.Dispose();
            this._replyMapTableAdapter.Dispose();
            this._picturesTableAdapter.Dispose();
            this._followMapTableAdapter.Dispose();
            this._activitiesTableAdapter.Dispose();
            this._accountsTableAdapter.Dispose();
        }
    }
}
