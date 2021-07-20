using LocalCinema.Data.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LocalCinema.Data.Repository
{
    public class DbConnectionContext : IDbConnectionContext
    {
        
        private IDbConnection _currentConnection;

        protected string ConnectionStrings { get; }

        public void Dispose()
        {
            this.Dispose();
        }

        public virtual IDbConnection GetOpenConnection(string connectionString = null)
        {
            if (_currentConnection == null)
            {
                var conn = new MySqlConnection(connectionString);
                conn.Open();

                _currentConnection = conn;
            }

            return _currentConnection;
        }

    }
}
