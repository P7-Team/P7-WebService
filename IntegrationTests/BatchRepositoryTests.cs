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
        //const string skip = "Integration Test, should not be run along with unit tests";
        const string skip = null;
        BatchRepository repository;

        public BatchRepositoryTests(DatabaseFixture fixture)
        {
            fixture.Clean(new string[]{"Batch"});
            repository = new BatchRepository(fixture.Db);
            
            UserRepository _userRepository = new UserRepository(fixture.Db);
            _userRepository.Create(new WebService.User("batchTestUser", "test"));
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
