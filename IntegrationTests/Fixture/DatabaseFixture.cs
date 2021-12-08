using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace IntegrationTests.Fixture
{
    public class DatabaseFixture : IDisposable
    {
        private MySqlConnection _connection;
        public DatabaseFixture()
        {
            Configure();
            
            _connection = new MySqlConnection(ConnString);

            var compiler = new MySqlCompiler();

            Db = new QueryFactory(_connection, compiler);

            // Remove all existing data in the table
            //connection.Execute(@"SET FOREIGN_KEY_CHECKS = 0;
            //                    TRUNCATE TABLE Users;
            //                    SET FOREIGN_KEY_CHECKS = 1; ");
        }

        public void Dispose()
        {
            Clean(new string[] {"Users", "Batch", "Task", "Result", "Source", "File", "Runs", "Argument"});
        }

        private void Configure()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            ConnString = configuration.GetSection("TestDBConnectionString").Value;
        }

        public void Clean(IEnumerable<string> truncateTableNames)
        {
            if (truncateTableNames.Count() < 1)
                return;
            
            string truncateString = @"SET FOREIGN_KEY_CHECKS = 0;";
            foreach (string table in truncateTableNames)
            {
                truncateString += "TRUNCATE TABLE " + table + ";";
            }

            truncateString += "SET FOREIGN_KEY_CHECKS = 1;";
            _connection.Execute(truncateString);
        }

        public QueryFactory Db { get; private set; }
        public string ConnString { get; private set; }
    }
}