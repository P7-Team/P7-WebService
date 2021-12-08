using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Services
{
    public class DBConnectionFactory : IDBConnectionFactory
    {
        private readonly string _connectionString;

        public DBConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public QueryFactory CreateQueryFactory(MySqlConnection connection)
        {
            var compiler = new MySqlCompiler();

            return new QueryFactory(connection, compiler);
        }

        MySqlConnection IDBConnectionFactory.GetConnection()
        {

            return new MySqlConnection(_connectionString);
        }
    }
}
