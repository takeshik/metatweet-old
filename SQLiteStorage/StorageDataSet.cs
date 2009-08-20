// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
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
using System.Data;
using System.Data.SQLite;

namespace XSpect.MetaTweet.Modules.StorageDataSetTableAdapters
{
    partial class AccountsTableAdapter
    {
        private readonly String _connectionString;

        public AccountsTableAdapter(String connectionString)
        {
            this._connectionString = connectionString;
            this.ClearBeforeFill = true;
        }

        public virtual Int32 FillBy(StorageDataSet.AccountsDataTable dataTable, String query)
        {
            this.Adapter.SelectCommand = new SQLiteCommand(query, this.Connection);
            this.Adapter.SelectCommand.CommandType = CommandType.Text;
            if (this.ClearBeforeFill)
            {
                dataTable.Clear();
            }
            Int32 returnValue = this.Adapter.Fill(dataTable);
            return returnValue;
        }

        public virtual StorageDataSet.AccountsDataTable GetDataBy(String query)
        {
            this.Adapter.SelectCommand = new SQLiteCommand(query, this.Connection);
            this.Adapter.SelectCommand.CommandType = CommandType.Text;
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
            this._connectionString = connectionString;
            this.ClearBeforeFill = true;
        }

        public virtual Int32 FillBy(StorageDataSet.ActivitiesDataTable dataTable, String query)
        {
            this.Adapter.SelectCommand = new SQLiteCommand(query, this.Connection);
            this.Adapter.SelectCommand.CommandType = CommandType.Text;

            if (this.ClearBeforeFill)
            {
                dataTable.Clear();
            }
            Int32 returnValue = this.Adapter.Fill(dataTable);
            return returnValue;
        }

        public virtual StorageDataSet.ActivitiesDataTable GetDataBy(String query)
        {
            this.Adapter.SelectCommand = new SQLiteCommand(query, this.Connection);
            this.Adapter.SelectCommand.CommandType = CommandType.Text;
            StorageDataSet.ActivitiesDataTable dataTable = new StorageDataSet.ActivitiesDataTable();
            this.Adapter.Fill(dataTable);
            return dataTable;
        }
    }

    partial class FavorMapTableAdapter
    {
        private readonly String _connectionString;

        public FavorMapTableAdapter(String connectionString)
        {
            this._connectionString = connectionString;
            this.ClearBeforeFill = true;
        }

        public virtual Int32 FillBy(StorageDataSet.FavorMapDataTable dataTable, String query)
        {
            this.Adapter.SelectCommand = new SQLiteCommand(query, this.Connection);
            this.Adapter.SelectCommand.CommandType = CommandType.Text;

            if (this.ClearBeforeFill)
            {
                dataTable.Clear();
            }
            Int32 returnValue = this.Adapter.Fill(dataTable);
            return returnValue;
        }

        public virtual StorageDataSet.FavorMapDataTable GetDataBy(String query)
        {
            this.Adapter.SelectCommand = new SQLiteCommand(query, this.Connection);
            this.Adapter.SelectCommand.CommandType = CommandType.Text;
            StorageDataSet.FavorMapDataTable dataTable = new StorageDataSet.FavorMapDataTable();
            this.Adapter.Fill(dataTable);
            return dataTable;
        }
    }

    partial class FollowMapTableAdapter
    {
        private readonly String _connectionString;

        public FollowMapTableAdapter(String connectionString)
        {
            this._connectionString = connectionString;
            this.ClearBeforeFill = true;
        }

        public virtual Int32 FillBy(StorageDataSet.FollowMapDataTable dataTable, String query)
        {
            this.Adapter.SelectCommand = new SQLiteCommand(query, this.Connection);
            this.Adapter.SelectCommand.CommandType = CommandType.Text;

            if (this.ClearBeforeFill)
            {
                dataTable.Clear();
            }
            Int32 returnValue = this.Adapter.Fill(dataTable);
            return returnValue;
        }

        public virtual StorageDataSet.FollowMapDataTable GetDataBy(String query)
        {
            this.Adapter.SelectCommand = new SQLiteCommand(query, this.Connection);
            this.Adapter.SelectCommand.CommandType = CommandType.Text;
            StorageDataSet.FollowMapDataTable dataTable = new StorageDataSet.FollowMapDataTable();
            this.Adapter.Fill(dataTable);
            return dataTable;
        }
    }

    partial class PostsTableAdapter
    {
        private readonly String _connectionString;

        public PostsTableAdapter(String connectionString)
        {
            this._connectionString = connectionString;
            this.ClearBeforeFill = true;
        }

        public virtual Int32 FillBy(StorageDataSet.PostsDataTable dataTable, String query)
        {
            this.Adapter.SelectCommand = new SQLiteCommand(query, this.Connection);
            this.Adapter.SelectCommand.CommandType = CommandType.Text;

            if (this.ClearBeforeFill)
            {
                dataTable.Clear();
            }
            Int32 returnValue = this.Adapter.Fill(dataTable);
            return returnValue;
        }

        public virtual StorageDataSet.PostsDataTable GetDataBy(String query)
        {
            this.Adapter.SelectCommand = new SQLiteCommand(query, this.Connection);
            this.Adapter.SelectCommand.CommandType = CommandType.Text;
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
            this._connectionString = connectionString;
            this.ClearBeforeFill = true;
        }

        public virtual Int32 FillBy(StorageDataSet.ReplyMapDataTable dataTable, String query)
        {
            this.Adapter.SelectCommand = new SQLiteCommand(query, this.Connection);
            this.Adapter.SelectCommand.CommandType = CommandType.Text;

            if (this.ClearBeforeFill)
            {
                dataTable.Clear();
            }
            Int32 returnValue = this.Adapter.Fill(dataTable);
            return returnValue;
        }

        public virtual StorageDataSet.ReplyMapDataTable GetDataBy(String query)
        {
            this.Adapter.SelectCommand = new SQLiteCommand(query, this.Connection);
            this.Adapter.SelectCommand.CommandType = CommandType.Text;
            StorageDataSet.ReplyMapDataTable dataTable = new StorageDataSet.ReplyMapDataTable();
            this.Adapter.Fill(dataTable);
            return dataTable;
        }
    }

    partial class TagMapTableAdapter
    {
        private readonly String _connectionString;

        public TagMapTableAdapter(String connectionString)
        {
            this._connectionString = connectionString;
            this.ClearBeforeFill = true;
        }

        public virtual Int32 FillBy(StorageDataSet.TagMapDataTable dataTable, String query)
        {
            this.Adapter.SelectCommand = new SQLiteCommand(query, this.Connection);
            this.Adapter.SelectCommand.CommandType = CommandType.Text;

            if (this.ClearBeforeFill)
            {
                dataTable.Clear();
            }
            Int32 returnValue = this.Adapter.Fill(dataTable);
            return returnValue;
        }

        public virtual StorageDataSet.TagMapDataTable GetDataBy(String query)
        {
            this.Adapter.SelectCommand = new SQLiteCommand(query, this.Connection);
            this.Adapter.SelectCommand.CommandType = CommandType.Text;
            StorageDataSet.TagMapDataTable dataTable = new StorageDataSet.TagMapDataTable();
            this.Adapter.Fill(dataTable);
            return dataTable;
        }
    }
}
