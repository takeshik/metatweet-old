﻿// -*- mode: csharp; encoding: utf-8; -*-
/* MetaTweet
 *   Hub system for micro-blog communication services
 * SQLiteStorage
 *   MetaTweet Storage which is provided by SQLite3 RDBMS.
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
using System.Data;
using System.Data.SQLite;
using XSpect.MetaTweet.StorageDataSetTableAdapters;
using XSpect.MetaTweet.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSpect.MetaTweet
{
    public class SQLiteStorage
        : Storage
    {
        private String _connectionString;

        private AccountsTableAdapter _accounts;

        private ActivitiesTableAdapter _activities;
        
        private FollowMapTableAdapter _followMap;
        
        private PostsTableAdapter _posts;
        
        private ReplyMapTableAdapter _replyMap;
        
        private TagMapTableAdapter _tagMap;

        public AccountsTableAdapter Accounts
        {
            get
            {
                return this._accounts;
            }
        }

        public ActivitiesTableAdapter Activities
        {
            get
            {
                return this._activities;
            }
        }

        public FollowMapTableAdapter FollowMap
        {
            get
            {
                return this._followMap;
            }
        }

        public PostsTableAdapter Posts
        {
            get
            {
                return this._posts;
            }
        }

        public ReplyMapTableAdapter ReplyMap
        {
            get
            {
                return this._replyMap;
            }
        }

        public TagMapTableAdapter TagMap
        {
            get
            {
                return this._tagMap;
            }
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
            this._accounts = new AccountsTableAdapter(this._connectionString);
            this._activities = new ActivitiesTableAdapter(this._connectionString);
            this._followMap = new FollowMapTableAdapter(this._connectionString);
            this._posts = new PostsTableAdapter(this._connectionString);
            this._replyMap = new ReplyMapTableAdapter(this._connectionString);
            this._tagMap = new TagMapTableAdapter(this._connectionString);
        }

        public override void Disconnect()
        {
            this._tagMap.Dispose();
            this._replyMap.Dispose();
            this._posts.Dispose();
            this._followMap.Dispose();
            this._activities.Dispose();
            this._accounts.Dispose();
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
                            "FollowingAccountId GUID NOT NULL" +
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
                        "CREATE TABLE IF NOT EXISTS TagMap (" +
                            "AccountId GUID NOT NULL," +
                            "Timestamp DATETIME NOT NULL," +
                            "Category TEXT NOT NULL," +
                            "Tag TEXT NOT NULL" +
                        ")";
                    command.ExecuteNonQuery();

                    command.CommandText =
                        "CREATE TABLE IF NOT EXISTS Posts (" +
                            "AccountId GUID NOT NULL," +
                            "PostId TEXT NOT NULL," +
                            "Timestamp DATETIME NOT NULL," +
                            "Text TEXT NOT NULL," +
                            "Source TEXT NOT NULL," +
                            "FavoriteCount INT NULL," +
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
                            "InReplyToPostId TEXT NOT NULL" +
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

        public override ICollection<Account> GetAccounts(Nullable<Guid> accountId)
        {
            StringBuilder whereClause = new StringBuilder();
            if (accountId.HasValue)
            {
                whereClause.AppendFormat("[AccountId] == '{0}' ", accountId.Value.ToString("D").ToLower());
            }
            return this.Accounts.GetDataBy(
                "SELECT [Accounts].* FROM [Accounts] " + (whereClause.Length > 0
                    ? "WHERE " + whereClause.ToString()
                    : String.Empty
                )
            ).Select(r => new Account()
            {
                Storage = this,
                UnderlyingDataRow = r,
            }).ToArray();
        }

        public override ICollection<Activity> GetActivities(Nullable<Guid> accountId, Nullable<DateTime> timestamp, String category)
        {
            StringBuilder whereClause = new StringBuilder();
            if (accountId.HasValue)
            {
                whereClause.AppendFormat("[AccountId] == '{0}' ", accountId.Value.ToString("D").ToLower());
            }
            if (timestamp.HasValue)
            {
                whereClause.AppendFormat(
                    "{0}[Timestamp] == datetime('{1}') ",
                    whereClause.Length > 0 ? String.Empty : "AND ",
                    timestamp.Value.ToString("s")
                );
            }
            if (category != null)
            {
                whereClause.AppendFormat(
                    "{0}[Category] == '{1}' ",
                    whereClause.Length > 0 ? String.Empty : "AND ",
                    category
                );
            }
            return this.Activities.GetDataBy(
                "SELECT [Activities].* FROM [Activities] " + (whereClause.Length > 0
                    ? "WHERE " + whereClause.ToString()
                    : String.Empty
                )
            ).Select(r => new Activity()
            {
                Storage = this,
                UnderlyingDataRow = r,
            }).ToArray();
        }

        public override ICollection<FollowElement> GetFollowElements(Nullable<Guid> accountId)
        {
            StringBuilder whereClause = new StringBuilder();
            if (accountId.HasValue)
            {
                whereClause.AppendFormat(
                    "[AccountId] == '{0}' OR [FollowingAccountId] == '{0}' ",
                    accountId.Value.ToString("D").ToLower()
                );
            }
            return this.FollowMap.GetDataBy(
                "SELECT [FollowMap].* FROM [FollowMap] " + (whereClause.Length > 0
                    ? "WHERE " + whereClause.ToString()
                    : String.Empty
                )
            ).Select(r =>  new FollowElement()
            {
                Storage = this,
                UnderlyingDataRow = r,
            }).ToArray();
        }

        public override ICollection<FollowElement> GetFollowElements(Nullable<Guid> accountId, Nullable<Guid> followingAccountId)
        {
            StringBuilder whereClause = new StringBuilder();
            if (accountId.HasValue)
            {
                whereClause.AppendFormat("[AccountId] == '{0}' ", accountId.Value.ToString("D").ToLower());
            }
            if (followingAccountId.HasValue)
            {
                whereClause.AppendFormat(
                    "{0}[FollowingAccountId] == '{1}' ",
                    whereClause.Length > 0 ? String.Empty : "AND ",
                    accountId.Value.ToString("D").ToLower()
                );
            }
            return this.FollowMap.GetDataBy(
                "SELECT [FollowMap].* FROM [FollowMap] " + (whereClause.Length > 0
                    ? "WHERE " + whereClause.ToString()
                    : String.Empty
                )
            ).Select(r => new FollowElement()
            {
                Storage = this,
                UnderlyingDataRow = r,
            }).ToArray();
        }

        public override ICollection<Post> GetPosts(Nullable<Guid> accountId, String postId, Nullable<DateTime> timestamp)
        {
            StringBuilder whereClause = new StringBuilder();
            if (accountId.HasValue)
            {
                whereClause.AppendFormat("[AccountId] == '{0}' ", accountId.Value.ToString("D").ToLower());
            }
            if (postId != null)
            {
                whereClause.AppendFormat(
                    "{0}[PostId] == '{1}' ",
                    whereClause.Length > 0 ? String.Empty : "AND ",
                    postId
                );
            }
            if (timestamp.HasValue)
            {
                whereClause.AppendFormat(
                    "{0}[Timestamp] == datetime('{1}') ",
                    whereClause.Length > 0 ? String.Empty : "AND ",
                    timestamp.Value.ToString("s")
                );
            }
            return this.Posts.GetDataBy(
                "SELECT [Posts].* FROM [Posts] " + (whereClause.Length > 0
                    ? "WHERE " + whereClause.ToString()
                    : String.Empty
                )
            ).Select(r => new Post()
            {
                Storage = this,
                UnderlyingDataRow = r,
            }).ToArray();
        }

        public override ICollection<ReplyElement> GetReplyElements(Nullable<Guid> accountId, String postId)
        {
            StringBuilder whereClause = new StringBuilder();
            if (accountId.HasValue)
            {
                whereClause.AppendFormat(
                    "([AccountId] == '{0}' OR [InReplyToAccountId] == '{0}') ",
                    accountId.Value.ToString("D").ToLower()
                );
            }
            if (postId != null)
            {
                whereClause.AppendFormat(
                    "{0}([PostId] == '{1}' OR [InReplyToPostId] == '{1}') ",
                    whereClause.Length > 0 ? String.Empty : "AND ",
                    postId
                );
            }
            return this.ReplyMap.GetDataBy(
                "SELECT [ReplyMap].* FROM [ReplyMap] " + (whereClause.Length > 0
                    ? "WHERE " + whereClause.ToString()
                    : String.Empty
                )
            ).Select(r => new ReplyElement()
            {
                Storage = this,
                UnderlyingDataRow = r,
            }).ToArray();
        }

        public override ICollection<ReplyElement> GetReplyElements(Nullable<Guid> accountId, String postId, Nullable<Guid> inReplyToAccountId, String inReplyTopostId)
        {
            StringBuilder whereClause = new StringBuilder();
            if (accountId.HasValue)
            {
                whereClause.AppendFormat(
                    "[AccountId] == '{0}' ",
                    accountId.Value.ToString("D").ToLower()
                );
            }
            if (postId != null)
            {
                whereClause.AppendFormat(
                    "{0}[PostId] == '{1}' ",
                    whereClause.Length > 0 ? String.Empty : "AND ",
                    postId
                );
            }
            if (accountId.HasValue)
            {
                whereClause.AppendFormat(
                    "{0}[InReplyToAccountId] == '{1}' ",
                    whereClause.Length > 0 ? String.Empty : "AND ",
                    accountId.Value.ToString("D").ToLower()
                );
            }
            if (inReplyTopostId != null)
            {
                whereClause.AppendFormat(
                    "{0}[InReplyToPostId] == '{1}' ",
                    whereClause.Length > 0 ? String.Empty : "AND ",
                    postId
                );
            }
            return this.ReplyMap.GetDataBy(
                "SELECT [ReplyMap].* FROM [ReplyMap] " + (whereClause.Length > 0
                    ? "WHERE " + whereClause.ToString()
                    : String.Empty
                )
            ).Select(r => new ReplyElement()
            {
                Storage = this,
                UnderlyingDataRow = r,
            }).ToArray();
        }

        public override ICollection<TagElement> GetTagElements(Nullable<Guid> accountId, Nullable<DateTime> timestamp, String category, String tag)
        {
            StringBuilder whereClause = new StringBuilder();
            if (accountId.HasValue)
            {
                whereClause.AppendFormat("[AccountId] == '{0}' ", accountId.Value.ToString("D").ToLower());
            }
            if (timestamp.HasValue)
            {
                whereClause.AppendFormat(
                    "{0}[Timestamp] == datetime('{1}') ",
                    whereClause.Length > 0 ? String.Empty : "AND ",
                    timestamp.Value.ToString("s")
                );
            }
            if (category != null)
            {
                whereClause.AppendFormat(
                    "{0}[Category] == '{1}' ",
                    whereClause.Length > 0 ? String.Empty : "AND ",
                    category
                );
            }
            if (tag != null)
            {
                whereClause.AppendFormat(
                    "{0}[Tag] == '{1}' ",
                    whereClause.Length > 0 ? String.Empty : "AND ",
                    tag
                );
            }
            return this.TagMap.GetDataBy(
                "SELECT [TagMap].* FROM [TagMap] " + (whereClause.Length > 0
                    ? "WHERE " + whereClause.ToString()
                    : String.Empty
                )
            ).Select(r => new TagElement()
            {
                Storage = this,
                UnderlyingDataRow = r,
            }).ToArray();
        }

        public override void Update(params StorageDataSet.AccountsRow[] rows)
        {
            this.Accounts.Update(rows);
        }

        public override void Update(params StorageDataSet.ActivitiesRow[] rows)
        {
            this.Activities.Update(rows);
        }

        public override void Update(params StorageDataSet.FollowMapRow[] rows)
        {
            this.FollowMap.Update(rows);
        }

        public override void Update(params StorageDataSet.PostsRow[] rows)
        {
            this.Posts.Update(rows);
        }

        public override void Update(params StorageDataSet.ReplyMapRow[] rows)
        {
            this.ReplyMap.Update(rows);
        }

        public override void Update(params StorageDataSet.TagMapRow[] rows)
        {
            this.TagMap.Update(rows);
        }
    }
}