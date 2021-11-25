using System;
using System.IO;
using System.Text;
using Dapper;
using IntegrationTests.Fixture;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using NuGet.Frameworks;
using SqlKata.Compilers;
using SqlKata.Execution;
using WebService;
using WebService.Models;
using WebService.Services;
using WebService.Services.Repositories;
using Xunit;

namespace IntegrationTests
{
    public class BatchFileRepositoryTests : IClassFixture<DatabaseFixture>
    {
        // change to skip = null, in order to run integration tests
        // Check https://josephwoodward.co.uk/2019/01/skipping-xunit-tests-based-on-runtime-conditions for conditional skip
        //const string skip = "Integration Test, should not be run along with unit tests";
        const string skip = null;
        private BatchFileRepository _batchFileRepository;
        private BatchRepository _batchRepository;
        private UserRepository _userRepository;
        
        public BatchFileRepositoryTests(DatabaseFixture fixture)
        {
            fixture.Clean(new string[] {"File", "Batch", "Users"});
            _batchFileRepository = new BatchFileRepository(fixture.Db);
            _batchRepository = new BatchRepository(fixture.Db);
            _userRepository = new UserRepository(fixture.Db);
        }

        [Fact(Skip = skip)]
        public void Create_GivenBatchFile_InsertsBatchFileIntoDB()
        {
            // Setup
            const string testPath = "/user/";
            const string testFilename = "epicfile";
            const string testUsername = "usr";
            const string testPassword = "pass";

            string testUser = CreateAndInsertTestUser(testUsername, testPassword);
            Batch testBatch = CreateAndInsertTestBatch(testUser);

            BatchFile file = new BatchFile(".txt", Encoding.UTF8.BodyName, new MemoryStream(), testBatch);
            file.WithPath(testPath);
            file.WithFileName(testFilename);

            // Act
            _batchFileRepository.Create(file);
            BatchFile actual = _batchFileRepository.Read(file.GetIdentifier());
            
            // Assert
            Assert.Equal(testPath, actual.Path);
            Assert.Equal(testFilename, actual.Filename);
            Assert.Equal(Encoding.UTF8.BodyName, actual.Encoding);
            Assert.Equal(file.BatchId, actual.BatchId);
            
            // Cleanup
            _batchRepository.Delete(file.BatchId);
            _userRepository.Delete(testUsername);
            _batchFileRepository.Delete(actual.GetIdentifier());
        }

        [Fact(Skip = skip)]
        public void Read_GivenIdentifierForABatchFile_ReturnsTheBatchFile()
        {
            // Setup
            const string testPath = "/user/";
            const string testFilename = "epicfile";
            const string testUsername = "usr";
            const string testPassword = "pass";

            string testUser = CreateAndInsertTestUser(testUsername, testPassword);
            Batch testBatch = CreateAndInsertTestBatch(testUser);

            BatchFile file = new BatchFile(".txt", Encoding.UTF8.BodyName, new MemoryStream(), testBatch);
            file.WithPath(testPath);
            file.WithFileName(testFilename);

            // Act
            _batchFileRepository.Create(file);
            BatchFile actual = _batchFileRepository.Read(file.GetIdentifier());
            
            // Assert
            Assert.Equal(testPath, actual.Path);
            Assert.Equal(testFilename, actual.Filename);
            Assert.Equal(Encoding.UTF8.BodyName, actual.Encoding);
            Assert.Equal(testBatch.Id, actual.BatchId);
            
            // Cleanup
            _batchRepository.Delete(testBatch.Id);
            _userRepository.Delete(testUsername);
            _batchFileRepository.Delete(actual.GetIdentifier());
        }

        [Fact(Skip = skip)]
        public void Update_GivenANewEncodingAndBatchId_Read_ReturnsUpdatedBatchFile()
        {
            // Setup
            const string testPath = "/user/";
            const string testFilename = "epicfile";
            const string testUsername = "usr";
            const string testPassword = "pass";

            string testUser = CreateAndInsertTestUser(testUsername, testPassword);
            Batch testBatch1 = CreateAndInsertTestBatch(testUser);
            Batch testBatch2 = CreateAndInsertTestBatch(testUser);

            string originalEncoding = Encoding.UTF8.BodyName;
            string updatedEncoding = Encoding.ASCII.BodyName;

            BatchFile file = new BatchFile(".txt", originalEncoding, new MemoryStream(), testBatch1);
            file.WithPath(testPath);
            file.WithFileName(testFilename);
            _batchFileRepository.Create(file);

            BatchFile updated = new BatchFile(".txt", updatedEncoding, new MemoryStream(), testBatch2);
            updated.WithPath(testPath).WithFileName(testFilename);
            
            // Act
            _batchFileRepository.Update(updated);
            BatchFile actual = _batchFileRepository.Read(file.GetIdentifier());
            
            
            // Assert
            Assert.NotEqual(file.Encoding, actual.Encoding);
            Assert.NotEqual(file.BatchId, actual.BatchId);
            Assert.Equal(updated.BatchId, actual.BatchId);
            Assert.Equal(updated.Encoding, actual.Encoding);
            
            // Cleanup
            _batchRepository.Delete(testBatch1.Id);
            _batchRepository.Delete(testBatch2.Id);
            _userRepository.Delete(testUser);
            _batchFileRepository.Delete(updated.GetIdentifier());
        }

        [Fact(Skip = skip)]
        public void Delete_GivenBatchFileIdentifier_DeletesBatchFileFromDB()
        {
            // Setup
            const string testPath = "/user/";
            const string testFilename = "epicfile";
            const string testUsername = "usr";
            const string testPassword = "pass";

            string testUser = CreateAndInsertTestUser(testUsername, testPassword);
            Batch testBatch = CreateAndInsertTestBatch(testUser);

            BatchFile file = new BatchFile(".txt", Encoding.UTF8.BodyName, new MemoryStream(), testBatch);
            file.WithPath(testPath);
            file.WithFileName(testFilename);

            // Act
            _batchFileRepository.Delete(file.GetIdentifier());
            
            // Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                _batchFileRepository.Read(file.GetIdentifier());
            });
            
            // Cleanup
            _batchRepository.Delete(testBatch.Id);
            _userRepository.Delete(testUser);
        }

        private string CreateAndInsertTestUser(string username, string password)
        {
            User user = new User(username, password);
            return _userRepository.Create(user);
        }

        private Batch CreateAndInsertTestBatch(string owner)
        {
            Batch batch = new Batch(9, owner);
            batch.Id = _batchRepository.Create(batch);
            return batch;
        }
        
    }
}