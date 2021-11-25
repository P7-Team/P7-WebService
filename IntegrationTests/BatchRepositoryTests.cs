using Dapper;
using MySql.Data.MySqlClient;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Text;
using WebService.Models;
using WebService.Services;
using WebService.Services.Repositories;
using Xunit;

namespace IntegrationTests
{
    public class BatchDatabaseFixture : IDisposable
    {
        UserRepository _userRepository;
        public BatchDatabaseFixture()
        {
            var connection = new MySqlConnection("server=164.90.236.116;uid=Tester;pwd=Finlux12345;database=TestDB");

            var compiler = new MySqlCompiler();

            Db = new QueryFactory(connection, compiler);

            // Remove all existing data in the table
            connection.Execute(@"SET FOREIGN_KEY_CHECKS = 0;
                                TRUNCATE TABLE Batch;
                                SET FOREIGN_KEY_CHECKS = 1; ");

            _userRepository = new UserRepository(Db);
            _userRepository.Create(new WebService.User("batchTestUser", "test"));
        }

        public void Dispose()
        {
            _userRepository.Delete("batchTestUser");
        }

        public QueryFactory Db { get; private set; }
    }

    public class BatchRepositoryTests : IClassFixture<BatchDatabaseFixture>
    {
        // change to skip = null, in order to run integration tests
        // Check https://josephwoodward.co.uk/2019/01/skipping-xunit-tests-based-on-runtime-conditions for conditional skip
        //const string skip = "Integration Test, should not be run along with unit tests";
        const string skip = null;
        BatchRepository repository;

        public BatchRepositoryTests(BatchDatabaseFixture fixture)
        {
            repository = new BatchRepository(fixture.Db);
        }

        [Fact(Skip = skip)]
        public void Repository_GivenBatch_InsertedBatchCanBeRead()
        {
            Batch batch = new Batch() { OwnerUsername = "batchTestUser" };
            int id = repository.Create(batch);

            Batch result = repository.Read(id);

            Assert.Equal("batchTestUser", result.OwnerUsername);
        }
    }
}
