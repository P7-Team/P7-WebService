using Dapper;
using MySql.Data.MySqlClient;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Text;
using IntegrationTests.Fixture;
using WebService.Models;
using WebService.Services;
using WebService.Services.Repositories;
using Xunit;

namespace IntegrationTests
{
    public class BatchRepositoryTests : IClassFixture<DatabaseFixture>
    {
        // change to skip = null, in order to run integration tests
        // Check https://josephwoodward.co.uk/2019/01/skipping-xunit-tests-based-on-runtime-conditions for conditional skip
        const string skip = "Integration Test, should not be run along with unit tests";
        //const string skip = null;
        BatchRepository repository;

        public BatchRepositoryTests(DatabaseFixture fixture)
        {
            fixture.Clean(new string[]{"Batch", "Users"});
            repository = new BatchRepository(fixture.Db);
            
            UserRepository _userRepository = new UserRepository(fixture.Db);
            _userRepository.Create(new User("batchTestUser1", "test"));
            _userRepository.Create(new User("batchTestUser2", "test"));
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
        public void Update_GivenValidBatch_UpdatesBatchInDB()
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
            
            int id = repository.Create(batch);
            Batch resultBeforeDelete = repository.Read(id);
            Assert.Equal("batchTestUser1", resultBeforeDelete.OwnerUsername);

            repository.Delete(id);

            Assert.Throws<InvalidOperationException>(() => { repository.Read(id); });
        }
    }
}
