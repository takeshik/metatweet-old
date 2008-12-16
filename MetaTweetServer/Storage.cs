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
using XSpect.MetaTweet.Properties;

namespace XSpect.MetaTweet
{
    public class Storage
        : IDisposable
    {
		private readonly ServerCore _parent;

        private readonly String _connectionString;

        private AccountsTableAdapter _accountsTableAdapter;

        private ActivitiesTableAdapter _activitiesTableAdapter;

        private FollowMapTableAdapter _followMapTableAdapter;

        private PicturesTableAdapter _picturesTableAdapter;

        private PostsTableAdapter _postsTableAdapter;

        private ReplyMapTableAdapter _replyMapTableAdapter;

		public ServerCore Parent
		{
			get
			{
				return this._parent;
			}
		}

		public String ConnectionString
		{
			get
			{
				return this._connectionString;
			}
		}

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
        }

		public virtual void Activate()
		{
			this._accountsTableAdapter = new AccountsTableAdapter(this._connectionString);
			this._activitiesTableAdapter = new ActivitiesTableAdapter(this._connectionString);
			this._followMapTableAdapter = new FollowMapTableAdapter(this._connectionString);
			this._picturesTableAdapter = new PicturesTableAdapter(this._connectionString);
			this._postsTableAdapter = new PostsTableAdapter(this._connectionString);
			this._replyMapTableAdapter = new ReplyMapTableAdapter(this._connectionString);
			this._parent.Log.Info(Resources.StorageActivated);
		}

		public virtual void Inactivate()
		{
			this._replyMapTableAdapter.Dispose();
			this._postsTableAdapter.Dispose();
			this._replyMapTableAdapter.Dispose();
			this._picturesTableAdapter.Dispose();
			this._followMapTableAdapter.Dispose();
			this._activitiesTableAdapter.Dispose();
			this._accountsTableAdapter.Dispose();
			this._parent.Log.Info(Resources.StorageInactivated);
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
					this._parent.Log.Info(Resources.StorageAccountsTableCreated);

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
					this._parent.Log.Info(Resources.StorageActivitiesTableCreated);


                    command.CommandText =
                        "CREATE TABLE IF NOT EXISTS FollowMap (" +
                            "AccountId GUID NOT NULL," +
                            "FollowingAccountId GUID NOT NULL" +
                        ")";
                    command.ExecuteNonQuery();
					this._parent.Log.Info(Resources.StorageFollowMapCreated);

					command.CommandText =
					"CREATE TABLE IF NOT EXISTS Pictures (" +
						"ImageId TEXT NOT NULL," +
						"Width INT NOT NULL," +
						"Height INT NOT NULL," +
						"Image BLOB NOT NULL," +
						"PRIMARY KEY (ImageId)" +
					")";
					command.ExecuteNonQuery();
					this._parent.Log.Info(Resources.StoragePicturesTableCreated);

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
					this._parent.Log.Info(Resources.StoragePostsTableCreated);

                    command.CommandText =
                        "CREATE TABLE IF NOT EXISTS ReplyMap (" +
                            "AccountId GUID NOT NULL," +
                            "PostId TEXT NOT NULL," +
                            "InReplyToAccountId GUID NOT NULL," +
                            "InReplyToPostId INTEGER NOT NULL" +
                        ")";
                    command.ExecuteNonQuery();
					this._parent.Log.Info(Resources.StorageReplyMapCreated);

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
                    command.CommandText = @"DROP TABLE IF EXISTS ReplyMap";
                    command.ExecuteNonQuery();
					this._parent.Log.Info(Resources.StorageReplyMapDropped);

					command.CommandText = @"DROP TABLE IF EXISTS Posts";
					command.ExecuteNonQuery();
					this._parent.Log.Info(Resources.StoragePostsTableDropped);

					command.CommandText = @"DROP TABLE IF EXISTS Pictures";
					command.ExecuteNonQuery();
					this._parent.Log.Info(Resources.StoragePicturesTableDropped);

                    command.CommandText = @"DROP TABLE IF EXISTS FollowMap";
                    command.ExecuteNonQuery();
					this._parent.Log.Info(Resources.StorageFollowMapDropped);

					command.CommandText = @"DROP TABLE IF EXISTS Activities";
					command.ExecuteNonQuery();
					this._parent.Log.Info(Resources.StorageActivitiesTableDropped);

                    command.CommandText = @"DROP TABLE IF EXISTS Accounts";
                    command.ExecuteNonQuery();
					this._parent.Log.Info(Resources.StorageAccountsTableDropped);
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
					this._parent.Log.Info(Resources.StorageTableVacuumed);
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
			this.Inactivate();
        }
    }
}
