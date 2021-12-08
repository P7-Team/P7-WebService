using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Services
{
    public interface IDBConnectionFactory
    {
        public MySqlConnection GetConnection();

        public QueryFactory CreateQueryFactory(MySqlConnection connection);
    }
}
