using Insight.Database;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.SqlServer
{
    public abstract class SqlServerRepositoryBase
    {
        private ConnectionStringSettings _connectionStringSettings;

        public string ConnectionString
        {
            get { return _connectionStringSettings.ConnectionString; }
            set
            {
                if (value == null)
                    return;

                _connectionStringSettings = new ConnectionStringSettings("_connection", value);
            }
        }

        protected TReturn Exec<TReturn>(Func<IDbConnection, TReturn> exec)
        {

            var connection = _connectionStringSettings.Connection();

            try
            {
                connection.Open();
                return exec(connection);
            }
            finally { connection.Close(); }
        }

        protected void Exec(Action<IDbConnection> exec)
        {

            var connection = _connectionStringSettings.Connection();

            try
            {
                connection.Open();
                exec(connection);
            }
            finally { connection.Close(); }
        }

        public SqlServerRepositoryBase(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
