using System;
using System.Data.SQLite;
using System.Data;

namespace XSpect.MetaTweet.SQLiteStorageDataSetTableAdapters
{
    partial class AccountsTableAdapter
    {
        private readonly String _connectionString;

        public AccountsTableAdapter(String connectionString)
        {
            this._connectionString = connectionString;
            this.ClearBeforeFill = true;
        }

        public virtual Int32 FillBy(SQLiteStorageDataSet.AccountsDataTable dataTable, String query)
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

        public virtual SQLiteStorageDataSet.AccountsDataTable GetDataBy(String query)
        {
            this.Adapter.SelectCommand = new SQLiteCommand(query, this.Connection);
            this.Adapter.SelectCommand.CommandType = CommandType.Text;
            SQLiteStorageDataSet.AccountsDataTable dataTable = new SQLiteStorageDataSet.AccountsDataTable();
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

        public virtual Int32 FillBy(SQLiteStorageDataSet.ActivitiesDataTable dataTable, String query)
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

        public virtual SQLiteStorageDataSet.ActivitiesDataTable GetDataBy(String query)
        {
            this.Adapter.SelectCommand = new SQLiteCommand(query, this.Connection);
            this.Adapter.SelectCommand.CommandType = CommandType.Text;
            SQLiteStorageDataSet.ActivitiesDataTable dataTable = new SQLiteStorageDataSet.ActivitiesDataTable();
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

        public virtual Int32 FillBy(SQLiteStorageDataSet.FollowMapDataTable dataTable, String query)
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

        public virtual SQLiteStorageDataSet.FollowMapDataTable GetDataBy(String query)
        {
            this.Adapter.SelectCommand = new SQLiteCommand(query, this.Connection);
            this.Adapter.SelectCommand.CommandType = CommandType.Text;
            SQLiteStorageDataSet.FollowMapDataTable dataTable = new SQLiteStorageDataSet.FollowMapDataTable();
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

        public virtual Int32 FillBy(SQLiteStorageDataSet.PostsDataTable dataTable, String query)
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

        public virtual SQLiteStorageDataSet.PostsDataTable GetDataBy(String query)
        {
            this.Adapter.SelectCommand = new SQLiteCommand(query, this.Connection);
            this.Adapter.SelectCommand.CommandType = CommandType.Text;
            SQLiteStorageDataSet.PostsDataTable dataTable = new SQLiteStorageDataSet.PostsDataTable();
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

        public virtual Int32 FillBy(SQLiteStorageDataSet.ReplyMapDataTable dataTable, String query)
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

        public virtual SQLiteStorageDataSet.ReplyMapDataTable GetDataBy(String query)
        {
            this.Adapter.SelectCommand = new SQLiteCommand(query, this.Connection);
            this.Adapter.SelectCommand.CommandType = CommandType.Text;
            SQLiteStorageDataSet.ReplyMapDataTable dataTable = new SQLiteStorageDataSet.ReplyMapDataTable();
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

        public virtual Int32 FillBy(SQLiteStorageDataSet.TagMapDataTable dataTable, String query)
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

        public virtual SQLiteStorageDataSet.TagMapDataTable GetDataBy(String query)
        {
            this.Adapter.SelectCommand = new SQLiteCommand(query, this.Connection);
            this.Adapter.SelectCommand.CommandType = CommandType.Text;
            SQLiteStorageDataSet.TagMapDataTable dataTable = new SQLiteStorageDataSet.TagMapDataTable();
            this.Adapter.Fill(dataTable);
            return dataTable;
        }
    }
}