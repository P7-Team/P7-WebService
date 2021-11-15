using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using SqlKata.Compilers;
using SqlKata.Execution;
using WebService.Interfaces;
using WebService.Models;
using WebService.Services;
using Task = WebService.Models.Task;

namespace WebService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices((hostBuilderContext, serviceCollection) =>
                {
                    // This makes the content of application.json available
                    var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();
                    
                    // Setup a context for the TaskController
                    serviceCollection.AddSingleton<ITaskContext, TaskContext>(sp =>
                    {
                        return new TaskContext();
                    });

                    // This provides a IDBConnectionFactory that can be used to create connections
                    // to the db.
                    serviceCollection.AddSingleton<IDBConnectionFactory, DBConnectionFactory>(sp =>
                    {
                        // Get connection string from application.json
                        string connectionString = config.GetSection("ConnectionString").Value;
                        return new DBConnectionFactory(connectionString);
                    });

                    // Create a QueryFactory that can be used in repositories to create queries using SqlKata
                    serviceCollection.AddScoped<QueryFactory>(sp =>
                    {
                        // Get connection string from application.json
                        string connectionString = config.GetSection("ConnectionString").Value;
                        var connection = new MySqlConnection(connectionString);

                        var compiler = new MySqlCompiler();

                        return new QueryFactory(connection, compiler);
                    });

                    // Inject UserRepository when IRepository<User, int> or UserRepository is required
                    serviceCollection.AddScoped<IRepository<User, int>, UserRepository>();
                    serviceCollection.AddScoped<UserRepository>();
                });
    }
}
