using System;
using System.Data.SQLite;
using System.Data;

namespace XSpect.MetaTweet.StorageDataSetTableAdapters
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
