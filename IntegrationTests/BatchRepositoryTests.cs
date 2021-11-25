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
            _userRepository.Create(new WebService.User("batchTestUser1", "test"));
            _userRepository.Create(new WebService.User("batchTestUser2", "test"));
        }

        public void Dispose()
        {
            _userRepository.Delete("batchTestUser1");
            _userRepository.Delete("batchTestUser2");
        }

        public QueryFactory Db { get; private set; }
    }

    public class BatchRepositoryTests : IClassFixture<BatchDatabaseFixture>
    {
        // change to skip = null, in order to run integration tests
        // Check https://josephwoodward.co.uk/2019/01/skipping-xunit-tests-based-on-runtime-conditions for conditional skip
        const string skip = "Integration Test, should not be run along with unit tests";
        //const string skip = null;
        BatchRepository repository;

        public BatchRepositoryTests(BatchDatabaseFixture fixture)
        {
            repository = new BatchRepository(fixture.Db);
        }

        [Fact(Skip = skip)]
        public void Repository_GivenBatch_InsertedBatchCanBeRead()
        {
            Batch batch = new Batch() { OwnerUsername = "batchTestUser1" };
            int id = repository.Create(batch);

            Batch result = repository.Read(id);

            Assert.Equal("batchTestUser1", result.OwnerUsername);
        }

        [Fact(Skip = skip)]
        public void Update_GivenValidBatch_UpdatedBatchInDB()
        {
            Batch oldBatch = new Batch() { OwnerUsername = "batchTestUser1" };
            int id = repository.Create(oldBatch);

            Batch newBatch = new Batch() { Id = id, OwnerUsername = "batchTestUser2" };
            repository.Update(newBatch);

            Batch result = repository.Read(id);

            Assert.Equal("batchTestUser2", result.OwnerUsername);
        }

        [Fact(Skip = skip)]
        public void Delete_GivenIdForExistingBatch_RemovesBatchFromDB()
        {
            Batch batch = new Batch() { OwnerUsername = "batchTestUser1" };
            // Insert user to be deleted, and ensure that it is in the DB
            int id = repository.Create(batch);
            Batch resultBeforeDelete = repository.Read(id);
            Assert.Equal("batchTestUser1", resultBeforeDelete.OwnerUsername);

            repository.Delete(id);

            Assert.Throws<InvalidOperationException>(() => { repository.Read(id); });
        }
    }
}
