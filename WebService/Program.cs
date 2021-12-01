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
using WebService.Helper;
using WebService.Interfaces;
using WebService.Models;
using WebService.Services;
using WebService.Services.Repositories;
using WebService.Services.Stores;
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

                    // This provides a IDBConnectionFactory that can be used to create connections
                    // to the db.
                    serviceCollection.AddSingleton<IDBConnectionFactory, DBConnectionFactory>(sp =>
                    {
                        // Get connection string from application.json
                        string connectionString = config.GetSection("ConnectionString").Value;
                        return new DBConnectionFactory(connectionString);
                    });

                    // Create a QueryFactory that can be used in repositories to create queries using SqlKata
                    serviceCollection.AddSingleton<QueryFactory>(sp =>
                    {
                        // Get connection string from application.json
                        string connectionString = config.GetSection("ConnectionString").Value;
                        var connection = new MySqlConnection(connectionString);

                        var compiler = new MySqlCompiler();

                        return new QueryFactory(connection, compiler);
                    });

                    // Inject UserRepository when IRepository<User, int> or UserRepository is required
                    serviceCollection.AddScoped<IRepository<User, string>, UserRepository>();
                    serviceCollection.AddScoped<UserRepository>();
                    serviceCollection.AddScoped<IRepository<Batch, int>, BatchRepository>();
                    serviceCollection.AddScoped<BatchRepository>();
                    serviceCollection.AddScoped<BatchFileRepository>();
                    serviceCollection.AddScoped<SourceFileRepository>();
                    serviceCollection.AddScoped<IRepository<Task, (int, int, int)>, TaskRepository>();
                    serviceCollection.AddScoped<TaskRepository>();
                    serviceCollection.AddScoped<ResultRepository>();
                    serviceCollection.AddScoped<FileSaver>();
                    serviceCollection.AddScoped<FileFetcher>();
                    serviceCollection.AddScoped<FileDeleter>();
                    serviceCollection.AddScoped<IRepository<Run, (int, int, int, string, string)>, RunRepository>();
                    serviceCollection.AddScoped<RunRepository>();

                    serviceCollection.AddScoped<IFileStore, FileStore>(sp =>
                    {
                        string fileDir = ConfigurationHelper.ReadOSFileDirFromConfiguration(config);
                        return new FileStore(sp.GetService<BatchFileRepository>(), sp.GetService<ResultRepository>(), sp.GetService<SourceFileRepository>(), sp.GetService<FileSaver>(), sp.GetService<FileFetcher>(), sp.GetService<FileDeleter>(), fileDir);
                    });

                    serviceCollection.AddScoped<TaskStore>();
                    serviceCollection.AddScoped<IStore<Task>, TaskStore>();
                    serviceCollection.AddScoped<BatchStore>();
                    serviceCollection.AddScoped<IStore<Batch>, BatchStore>();

                    // Setup a context for the TaskController
                    serviceCollection.AddSingleton<ITaskContext, TaskContext>(sp =>
                    {
                        return new TaskContext(sp.GetService<FileStore>(), sp.GetService<BatchRepository>());
                    });

                    serviceCollection.AddSingleton<ISchedulerHistoryHelper, SchedulerHistoryHelper>();
                    serviceCollection.AddSingleton<ISchedulerWorkedOnHelper, SchedulerWorkedOnHelper>();
                    serviceCollection.AddSingleton<IContributionPointCalculator, ContributionPointCalculator>();
                    serviceCollection.AddSingleton<Scheduler>();

                    serviceCollection.AddSingleton<Automator>(sp =>
                    {
                        // Get connection string from application.json
                        int automaterInterval = int.Parse(config.GetSection("AutomaterInterval").Value);
                        return new Automator(automaterInterval, sp.GetRequiredService<ISchedulerWorkedOnHelper>(), sp.GetRequiredService<IContributionPointCalculator>());
                    });
                });
    }
}
