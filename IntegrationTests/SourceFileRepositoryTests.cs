using System;
using System.IO;
using System.Text;
using IntegrationTests.Fixture;
using WebService;
using WebService.Models;
using WebService.Services;
using WebService.Services.Repositories;
using Xunit;

namespace IntegrationTests
{
    public class SourceFileRepositoryTests : IClassFixture<DatabaseFixture>
    {
        // change to skip = null, in order to run integration tests
        // Check https://josephwoodward.co.uk/2019/01/skipping-xunit-tests-based-on-runtime-conditions for conditional skip
        //const string skip = "Integration Test, should not be run along with unit tests";
        const string skip = null;

        private readonly SourceFileRepository _sourceFileRepository;
        private readonly BatchFileRepository _batchFileRepository;
        private readonly BatchRepository _batchRepository;
        private readonly UserRepository _userRepository;
        
        public SourceFileRepositoryTests(DatabaseFixture fixture)
        {
            fixture.Clean(new string[]{"Source", "File", "Batch", "Users"});
            
            _sourceFileRepository = new SourceFileRepository(fixture.Db);
            _batchFileRepository = new BatchFileRepository(fixture.Db);
            _batchRepository = new BatchRepository(fixture.Db);
            _userRepository = new UserRepository(fixture.Db);
        }

        [Fact(Skip = skip)]
        public void Create_BatchFileExists_GivenSourceFile_InsertsSourceFileIntoDB()
        {
            // Setup
            string testUser = CreateAndInsertTestUser("TestUser", "password");
            Batch testBatch = CreateAndInsertTestBatch(testUser);
            const string testPath = "path";
            const string testFilename = "testName";
            BatchFile testBatchFile = CreateAndInsertTestBatchFile(testPath, testFilename, testBatch);

            SourceFile sourceFile = CreateSourceFileFromBatchFile(testBatchFile, testBatch, "Haskell");
            
            // Act
            _sourceFileRepository.Create(sourceFile);
            
            // Assert
            SourceFile actual = _sourceFileRepository.Read(sourceFile.GetIdentifier());
            Assert.Equal(sourceFile.Path, actual.Path);
            Assert.Equal(sourceFile.BatchId, actual.BatchId);
            Assert.Equal(sourceFile.Filename, actual.Filename);
            Assert.Equal(sourceFile.Encoding, actual.Encoding);
            Assert.Equal(sourceFile.Language, actual.Language);
            
            
            // Cleanup
            _batchRepository.Delete(testBatch.Id);
            _userRepository.Delete(testUser);
            _sourceFileRepository.Delete(actual.GetIdentifier());
            _batchFileRepository.Delete(actual.GetIdentifier());
        }
        
        [Fact(Skip = skip)]
        public void Read_BatchFileExists_GivenSourceFile_InsertsSourceFileIntoDB()
        {
            // Setup
            string testUser = CreateAndInsertTestUser("TestUser", "password");
            Batch testBatch = CreateAndInsertTestBatch(testUser);
            const string testPath = "path";
            const string testFilename = "testName";
            BatchFile testBatchFile = CreateAndInsertTestBatchFile(testPath, testFilename, testBatch);

            SourceFile sourceFile = CreateSourceFileFromBatchFile(testBatchFile, testBatch, "Haskell");
            
            // Act
            _sourceFileRepository.Create(sourceFile);
            
            // Assert
            SourceFile actual = _sourceFileRepository.Read(sourceFile.GetIdentifier());
            Assert.Equal(sourceFile.Path, actual.Path);
            Assert.Equal(sourceFile.Filename, actual.Filename);
            Assert.Equal(sourceFile.Encoding, actual.Encoding);
            Assert.Equal(sourceFile.Language, actual.Language);
            
            
            // Cleanup
            _batchRepository.Delete(testBatch.Id);
            _userRepository.Delete(testUser);
            _sourceFileRepository.Delete(actual.GetIdentifier());
            _batchFileRepository.Delete(actual.GetIdentifier());
        }
        
        [Fact(Skip = skip)]
        public void Read_BatchFileDoesNotExist_GivenSourceFile_ThrowsError()
        {
            // Setup
            const string testPath = "path";
            const string testFilename = "testName";
            const int testBatchId = 1;

            SourceFile sourceFile = new SourceFile(".txt", Encoding.UTF8.BodyName, new MemoryStream(),
                new Batch(testBatchId), "Haskell");
            sourceFile.WithPath(testPath).WithFileName(testFilename);
            
            // Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                _sourceFileRepository.Read(sourceFile.GetIdentifier());
            });
        }

        [Fact(Skip = skip)]
        public void Update_SourceFileExists_GivenSourceFile_ValuesAreUpdated()
        {
            // Setup
            string testUser = CreateAndInsertTestUser("TestUser", "password");
            Batch testBatch = CreateAndInsertTestBatch(testUser);
            const string testPath = "path";
            const string testFilename = "testName";
            const string firstLanguage = "Datalog";
            const string secondLanguage = "Prolog";
            
            BatchFile testBatchFile = CreateAndInsertTestBatchFile(testPath, testFilename, testBatch);

            SourceFile sourceFile = CreateSourceFileFromBatchFile(testBatchFile, testBatch, firstLanguage);
            _sourceFileRepository.Create(sourceFile);

            SourceFile updated = CreateSourceFileFromBatchFile(testBatchFile, testBatch, secondLanguage);
            
            // Act
            _sourceFileRepository.Update(updated);
            
            // Assert
            SourceFile actual = _sourceFileRepository.Read(updated.GetIdentifier());
            Assert.NotEqual(sourceFile.Language, actual.Language);
            Assert.Equal(updated.Language, actual.Language);
            
            // Cleanup
            _batchRepository.Delete(testBatch.Id);
            _userRepository.Delete(testUser);
            _sourceFileRepository.Delete(actual.GetIdentifier());
            _batchFileRepository.Delete(actual.GetIdentifier());

        }

        [Fact(Skip = skip)]
        public void Delete_SourceFileExists_SourceFileIsDeletedFromDb()
        {
            // Setup
            string testUser = CreateAndInsertTestUser("TestUser", "password");
            Batch testBatch = CreateAndInsertTestBatch(testUser);
            const string testPath = "path";
            const string testFilename = "testName";
            const string firstLanguage = "Datalog";
            const string secondLanguage = "Prolog";
            
            BatchFile testBatchFile = CreateAndInsertTestBatchFile(testPath, testFilename, testBatch);

            SourceFile sourceFile = CreateSourceFileFromBatchFile(testBatchFile, testBatch, firstLanguage);
            _sourceFileRepository.Create(sourceFile);

            // Act
            _sourceFileRepository.Delete(sourceFile.GetIdentifier());
            
            // Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                _sourceFileRepository.Read(sourceFile.GetIdentifier());
            });

            // Cleanup
            _batchRepository.Delete(testBatch.Id);
            _userRepository.Delete(testUser);
            _batchFileRepository.Delete(testBatchFile.GetIdentifier());
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

        private BatchFile CreateAndInsertTestBatchFile(string path, string filename, Batch batch)
        {
            BatchFile file = new BatchFile(".txt", Encoding.UTF8.BodyName, new MemoryStream(), batch);
            file.WithPath(path).WithFileName(filename);
            _batchFileRepository.Create(file);
            return file;
        }

        private SourceFile CreateSourceFileFromBatchFile(BatchFile batchFile, Batch batch, string language)
        {
            SourceFile sourceFile = new SourceFile(batchFile.OriginalExtension, batchFile.Encoding, batchFile.Data, batch, language);
            sourceFile.WithPath(batchFile.Path).WithFileName(batchFile.Filename);
            return sourceFile;
        }
    }
}