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
using System.Data;
using System.Data.SQLite;

namespace XSpect.MetaTweet.StorageDataSetTableAdapters
{
    partial class AccountsTableAdapter
    {
        private readonly String _connectionString;

        public AccountsTableAdapter(String connectionString)
        {
            this.ClearBeforeFill = true;
            this._connectionString = connectionString;
        }

        public virtual Int32 FillBy(StorageDataSet.AccountsDataTable dataTable, String query)
        {
            SQLiteCommand command = new SQLiteCommand(query, this.Connection);
            command.CommandType = CommandType.Text;
            this.Adapter.SelectCommand = command;
            if (this.ClearBeforeFill)
            {
                dataTable.Clear();
            }
            return this.Adapter.Fill(dataTable);
        }

        public virtual StorageDataSet.AccountsDataTable GetDataBy(String query)
        {
            SQLiteCommand command = new SQLiteCommand(query, this.Connection);
            command.CommandType = CommandType.Text;
            this.Adapter.SelectCommand = command;
            StorageDataSet.AccountsDataTable dataTable = new StorageDataSet.AccountsDataTable();
            this.Adapter.Fill(dataTable);
            return dataTable;
        }
    }

    partial class ActivitiesTableAdapter
    {
        private readonly String _connectionString;

        public ActivitiesTableAdapter(String connectionString)
        {
            this.ClearBeforeFill = true;
            this._connectionString = connectionString;
        }

        public virtual Int32 FillBy(StorageDataSet.ActivitiesDataTable dataTable, String query)
        {
            SQLiteCommand command = new SQLiteCommand(query, this.Connection);
            command.CommandType = CommandType.Text;
            this.Adapter.SelectCommand = command;
            if (this.ClearBeforeFill)
            {
                dataTable.Clear();
            }
            return this.Adapter.Fill(dataTable);
        }

        public virtual StorageDataSet.ActivitiesDataTable GetDataBy(String query)
        {
            SQLiteCommand command = new SQLiteCommand(query, this.Connection);
            command.CommandType = CommandType.Text;
            this.Adapter.SelectCommand = command;
            StorageDataSet.ActivitiesDataTable dataTable = new StorageDataSet.ActivitiesDataTable();
            this.Adapter.Fill(dataTable);
            return dataTable;
        }
    }

    partial class FollowMapTableAdapter
    {
        private readonly String _connectionString;

        public FollowMapTableAdapter(String connectionString)
        {
            this.ClearBeforeFill = true;
            this._connectionString = connectionString;
        }

        public virtual Int32 FillBy(StorageDataSet.FollowMapDataTable dataTable, String query)
        {
            SQLiteCommand command = new SQLiteCommand(query, this.Connection);
            command.CommandType = CommandType.Text;
            this.Adapter.SelectCommand = command;
            if (this.ClearBeforeFill)
            {
                dataTable.Clear();
            }
            return this.Adapter.Fill(dataTable);
        }

        public virtual StorageDataSet.FollowMapDataTable GetDataBy(String query)
        {
            SQLiteCommand command = new SQLiteCommand(query, this.Connection);
            command.CommandType = CommandType.Text;
            this.Adapter.SelectCommand = command;
            StorageDataSet.FollowMapDataTable dataTable = new StorageDataSet.FollowMapDataTable();
            this.Adapter.Fill(dataTable);
            return dataTable;
        }
    }

    partial class PicturesTableAdapter
    {
        private readonly String _connectionString;

        public PicturesTableAdapter(String connectionString)
        {
            this.ClearBeforeFill = true;
            this._connectionString = connectionString;
        }

        public virtual Int32 FillBy(StorageDataSet.PicturesDataTable dataTable, String query)
        {
            SQLiteCommand command = new SQLiteCommand(query, this.Connection);
            command.CommandType = CommandType.Text;
            this.Adapter.SelectCommand = command;
            if (this.ClearBeforeFill)
            {
                dataTable.Clear();
            }
            return this.Adapter.Fill(dataTable);
        }

        public virtual StorageDataSet.PicturesDataTable GetDataBy(String query)
        {
            SQLiteCommand command = new SQLiteCommand(query, this.Connection);
            command.CommandType = CommandType.Text;
            this.Adapter.SelectCommand = command;
            StorageDataSet.PicturesDataTable dataTable = new StorageDataSet.PicturesDataTable();
            this.Adapter.Fill(dataTable);
            return dataTable;
        }
    }

    partial class PostsTableAdapter
    {
        private readonly String _connectionString;

        public PostsTableAdapter(String connectionString)
        {
            this.ClearBeforeFill = true;
            this._connectionString = connectionString;
        }

        public virtual Int32 FillBy(StorageDataSet.PostsDataTable dataTable, String query)
        {
            SQLiteCommand command = new SQLiteCommand(query, this.Connection);
            command.CommandType = CommandType.Text;
            this.Adapter.SelectCommand = command;
            if (this.ClearBeforeFill)
            {
                dataTable.Clear();
            }
            return this.Adapter.Fill(dataTable);
        }

        public virtual StorageDataSet.PostsDataTable GetDataBy(String query)
        {
            SQLiteCommand command = new SQLiteCommand(query, this.Connection);
            command.CommandType = CommandType.Text;
            this.Adapter.SelectCommand = command;
            StorageDataSet.PostsDataTable dataTable = new StorageDataSet.PostsDataTable();
            this.Adapter.Fill(dataTable);
            return dataTable;
        }
    }

    partial class ReplyMapTableAdapter
    {
        private readonly String _connectionString;

        public ReplyMapTableAdapter(String connectionString)
        {
            this.ClearBeforeFill = true;
            this._connectionString = connectionString;
        }

        public virtual Int32 FillBy(StorageDataSet.ReplyMapDataTable dataTable, String query)
        {
            SQLiteCommand command = new SQLiteCommand(query, this.Connection);
            command.CommandType = CommandType.Text;
            this.Adapter.SelectCommand = command;
            if (this.ClearBeforeFill)
            {
                dataTable.Clear();
            }
            return this.Adapter.Fill(dataTable);
        }

        public virtual StorageDataSet.ReplyMapDataTable GetDataBy(String query)
        {
            SQLiteCommand command = new SQLiteCommand(query, this.Connection);
            command.CommandType = CommandType.Text;
            this.Adapter.SelectCommand = command;
            StorageDataSet.ReplyMapDataTable dataTable = new StorageDataSet.ReplyMapDataTable();
            this.Adapter.Fill(dataTable);
            return dataTable;
        }
    }
}
namespace XSpect.MetaTweet {
    
    
    public partial class StorageDataSet {
    }
}
