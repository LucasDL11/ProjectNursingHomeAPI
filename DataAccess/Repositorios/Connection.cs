using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;
namespace RepositoriosADO
{
    public class Connection : IDbConnection
    {
        SqlConnection conn = null;

        // Lucas 

        private string connectionString = "";


        public string ConnectionString { get => connectionString; set => connectionString = value; }

        public int ConnectionTimeout => conn.CommandTimeout;
        public Connection()
        {
            conn = new SqlConnection(connectionString);
        }

        public string Database => conn.Database;

        public ConnectionState State => conn.State;



        public IDbTransaction BeginTransaction()
        {
            return conn.BeginTransaction();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            throw new NotImplementedException();
        }

        public void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            conn.Close();
        }

        public IDbCommand CreateCommand()
        {
            return conn.CreateCommand();
        }

        public void Dispose()
        {
            conn.Dispose();
        }

        public void Open()
        {
            conn.Open();
        }
    }
}