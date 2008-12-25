// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
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
using System.Data.SQLite;
using System.Collections.Generic;
using XSpect.MetaTweet.Properties;

namespace XSpect.MetaTweet
{
    public class Storage
        : IDisposable
    {
        private readonly ServerCore _parent;

        private readonly Hook<Storage> _activateHook = new Hook<Storage>();

        private readonly Hook<Storage> _inactivateHook = new Hook<Storage>();

        private readonly String _connectionString;

        public ServerCore Parent
        {
            get
            {
                return this._parent;
            }
        }

        public Hook<Storage> ActivateHook
        {
            get
            {
                return this._activateHook;
            }
        }

        public Hook<Storage> InactivateHook
        {
            get
            {
                return this._inactivateHook;
            }
        } 

        public String ConnectionString
        {
            get
            {
                return this._connectionString;
            }
        }

        public Storage(ServerCore parent, String connectionString)
        {
            this._parent = parent;
            this._connectionString = connectionString;
        }

        public virtual void Activate()
        {
            this._activateHook.Execute(self =>
            {
            }, this);
        }

        public virtual void Inactivate()
        {
            this._inactivateHook.Execute(self =>
            {
            }, this);
        }

        // TODO: Consider to support hooking to below methods

        public virtual void CreateTable()
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
                    this._parent.Log.Info(Resources.StorageAccountsTableCreated);

                    command.CommandText =
                        "CREATE TABLE IF NOT EXISTS Activities (" +
                            "AccountId GUID NOT NULL," +
                            "Timestamp DATETIME NOT NULL," +
                            "Category TEXT NOT NULL," +
                            "Value TEXT," +
                            "Data BLOB," +
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
                            "InReplyToPostId TEXT NOT NULL" +
                        ")";
                    command.ExecuteNonQuery();
                    this._parent.Log.Info(Resources.StorageReplyMapCreated);

                    command.CommandText =
                        "CREATE TABLE IF NOT EXISTS TagMap (" +
                            "AccountId GUID," +
                            "Timestamp DATETIME," +
                            "Category TEXT NOT NULL," +
                            "Tag TEXT NOT NULL" +
                        ")";
                    command.ExecuteNonQuery();
                    this._parent.Log.Info(Resources.StorageTagMapTableCreated);

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
                    this._parent.Log.Info(Resources.StorageTagMapTableDropped);

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
