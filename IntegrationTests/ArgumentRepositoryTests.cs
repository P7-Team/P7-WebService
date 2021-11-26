using IntegrationTests.Fixture;
using System;
using System.Collections.Generic;
using System.Text;
using WebService.Models;
using WebService.Services;
using WebService.Services.Repositories;
using Xunit;

namespace IntegrationTests
{
    public class ArgumentRepositoryTests : IClassFixture<DatabaseFixture>
    {
        // change to skip = null, in order to run integration tests
        // Check https://josephwoodward.co.uk/2019/01/skipping-xunit-tests-based-on-runtime-conditions for conditional skip
        const string skip = "Integration Test, should not be run along with unit tests";
        //const string skip = null;
        ArgumentRepository repository;

        public ArgumentRepositoryTests(DatabaseFixture fixture)
        {
            fixture.Clean(new string[] { "Argument", "Batch", "File", "Users", "Source" });
            repository = new ArgumentRepository(fixture.Db);

            UserRepository userRepository = new UserRepository(fixture.Db);
            userRepository.Create(new WebService.User("testUser", "testPassword"));
            BatchRepository batchRepository = new BatchRepository(fixture.Db);
            int batchId = batchRepository.Create(new Batch() { OwnerUsername = "testUser" });
            BatchFileRepository fileRepository = new BatchFileRepository(fixture.Db);
            fileRepository.Create(new BatchFile("testPath", "testFile", "", batchId));
            SourceFileRepository sourceFileRepository = new SourceFileRepository(fixture.Db);
            sourceFileRepository.Create(new SourceFile("", "", null, new Batch() { Id = batchId }, "") { Path = "testPath", Filename = "testFile" });
        }

        [Fact(Skip = skip)]
        public void Repository_GivenArgument_InsertedArgumentCanBeRead()
        {
            Argument arg = new Argument("testPath", "testFile", 1, "--Verbose");
            repository.Create(arg);

            Argument result = repository.Read(arg.GetIdentifier());

            Assert.Equal("testPath", result.Path);
            Assert.Equal("testFile", result.Filename);
            Assert.Equal(1, result.Number);
            Assert.Equal("--Verbose", result.Value);
        }

        [Fact(Skip = skip)]
        public void Update_GivenValidArgument_UpdatesArgumentInDB()
        {
            Argument oldArg = new Argument("testPath", "testFile", 1, "--Verbose");
            repository.Create(oldArg);

            Argument newArg = new Argument("testPath", "testFile", 1, "-Pedantic");
            repository.Update(newArg);

            Argument result = repository.Read(oldArg.GetIdentifier());

            Assert.Equal("-Pedantic", result.Value);
        }

        [Fact(Skip = skip)]
        public void Delete_GivenKeyForExistingArgument_RemovesArgumentFromDB()
        {
            Argument arg = new Argument("testPath", "testFile", 1, "--Verbose");
            
            repository.Create(arg);
            Argument resultBeforeDelete = repository.Read(arg.GetIdentifier());
            Assert.Equal("--Verbose", resultBeforeDelete.Value);

            repository.Delete(arg.GetIdentifier());

            Assert.Throws<InvalidOperationException>(() => { repository.Read(arg.GetIdentifier()); });
        }
    }
}
