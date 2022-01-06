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
    public class ResultRepositoryTests : IClassFixture<DatabaseFixture>
    {
        // change to skip = null, in order to run integration tests
        // Check https://josephwoodward.co.uk/2019/01/skipping-xunit-tests-based-on-runtime-conditions for conditional skip
        const string skip = "Integration Test, should not be run along with unit tests";
        //const string skip = null;
        
        private readonly UserRepository _userRepository;
        private readonly BatchRepository _batchRepository;
        private readonly BatchFileRepository _batchFileRepository;
        private readonly ResultRepository _resultRepository;
        private readonly TaskRepository _taskRepository;
        
        public ResultRepositoryTests(DatabaseFixture fixture)
        {
            fixture.Clean(new []{"Users", "Batch", "Result", "File", "Task"});
            _userRepository = new UserRepository(fixture.Db);
            _batchRepository = new BatchRepository(fixture.Db);
            _batchFileRepository = new BatchFileRepository(fixture.Db);
            _resultRepository = new ResultRepository(fixture.Db);
            _taskRepository = new TaskRepository(fixture.ConnectionFactory);
        }

        [Fact(Skip = skip)]
        public void Create_GivenResultFile_BatchFileExists_InsertResultFileInDb()
        {
            // Setup
            string testUser = CreateAndInsertTestUser("TestUser", "password");
            Batch testBatch = CreateAndInsertTestBatch(testUser);
            const int taskNumber = 1;
            const int taskSubnumber = 1;
            Task testTask = CreateAndInsertTask(testBatch, taskNumber, taskSubnumber);
            const string testPath = "path";
            const string testFilename = "testName";
            BatchFile testBatchFile = CreateAndInsertTestBatchFile(testPath, testFilename, testBatch);
            Result expected = CreateResultFromBatchFile(testBatchFile, testBatch, true, testTask);

            // Act
            _resultRepository.Create(expected);
            
            // Assert
            Result actual = _resultRepository.Read(expected.GetIdentifier());
            Assert.Equal(expected.Path, actual.Path);
            Assert.Equal(expected.BatchId, actual.BatchId);
            Assert.Equal(expected.Filename, actual.Filename);
            Assert.Equal(expected.Encoding, actual.Encoding);
            Assert.Equal(expected.Verified, actual.Verified);
            
            // Cleanup
            _resultRepository.Delete(actual.GetIdentifier());
            _batchFileRepository.Delete(actual.GetIdentifier());
            _taskRepository.Delete(testTask.GetIdentifier());
            _batchRepository.Delete(testBatch.Id);
            _userRepository.Delete(testUser);
        }
        
        [Fact(Skip = skip)]
        public void Read_GivenResultFile_BatchFileExists_InsertResultFileInDb()
        {
            // Setup
            string testUser = CreateAndInsertTestUser("TestUser", "password");
            Batch testBatch = CreateAndInsertTestBatch(testUser);
            const int taskNumber = 1;
            const int taskSubnumber = 1;
            Task testTask = CreateAndInsertTask(testBatch, taskNumber, taskSubnumber);
            const string testPath = "path";
            const string testFilename = "testName";
            BatchFile testBatchFile = CreateAndInsertTestBatchFile(testPath, testFilename, testBatch);
            Result expected = CreateResultFromBatchFile(testBatchFile, testBatch, true, testTask);

            // Act
            _resultRepository.Create(expected);
            
            // Assert
            Result actual = _resultRepository.Read(expected.GetIdentifier());
            Assert.Equal(expected.Path, actual.Path);
            Assert.Equal(expected.Filename, actual.Filename);
            Assert.Equal(expected.Encoding, actual.Encoding);
            Assert.Equal(expected.Verified, actual.Verified);
            
            // Cleanup
            _resultRepository.Delete(actual.GetIdentifier());
            _batchFileRepository.Delete(actual.GetIdentifier());
            _taskRepository.Delete(testTask.GetIdentifier());
            _batchRepository.Delete(testBatch.Id);
            _userRepository.Delete(testUser);
        }
        
        
        
        
        
        [Fact(Skip = skip)]
        public void Delete_GivenResultFile_BatchFileExists_InsertResultFileInDb()
        {
            // Setup
            string testUser = CreateAndInsertTestUser("TestUser", "password");
            Batch testBatch = CreateAndInsertTestBatch(testUser);
            const int taskNumber = 1;
            const int taskSubnumber = 1;
            Task testTask = CreateAndInsertTask(testBatch, taskNumber, taskSubnumber);
            const string testPath = "path";
            const string testFilename = "testName";
            BatchFile testBatchFile = CreateAndInsertTestBatchFile(testPath, testFilename, testBatch);
            Result expected = CreateResultFromBatchFile(testBatchFile, testBatch, true, testTask);
            _resultRepository.Create(expected);
            
            // Act
            _resultRepository.Delete(expected.GetIdentifier());
            
            // Assert
            Assert.Null(_resultRepository.Read(expected.GetIdentifier()));

            // Cleanup
            _batchFileRepository.Delete(expected.GetIdentifier());
            _taskRepository.Delete(testTask.GetIdentifier());
            _batchRepository.Delete(testBatch.Id);
            _userRepository.Delete(testUser);
        }

        [Fact(Skip = skip)]
        public void Update_GivenAChangedResult_ResultExistsInDb_ResultInDbIsUpdated()
        {
            // Setup
            string testUser = CreateAndInsertTestUser("TestUser", "password");
            Batch testBatch = CreateAndInsertTestBatch(testUser);
            const int taskNumber = 1;
            const int taskSubnumber = 1;
            Task testTask = CreateAndInsertTask(testBatch, taskNumber, taskSubnumber);
            const string testPath = "path";
            const string testFilename = "testName";
            BatchFile testBatchFile = CreateAndInsertTestBatchFile(testPath, testFilename, testBatch);
            Result original = CreateResultFromBatchFile(testBatchFile, testBatch, true, testTask);
            _resultRepository.Create(original);

            Result updated = CreateResultFromBatchFile(testBatchFile, testBatch, false, testTask);
            
            // Act
            _resultRepository.Update(updated);
            Result actual = _resultRepository.Read(updated.GetIdentifier());
            
            // Assert
            Assert.Equal(updated.Path, actual.Path);
            Assert.Equal(updated.Filename, actual.Filename);
            Assert.Equal(updated.Encoding, actual.Encoding);
            Assert.Equal(updated.TaskId, actual.TaskId);
            Assert.Equal(updated.TaskNumber, actual.TaskNumber);
            Assert.Equal(updated.TaskSubnumber, actual.TaskSubnumber);
            Assert.Equal(updated.BatchId, actual.BatchId);
            Assert.Equal(updated.Verified, actual.Verified);
            Assert.NotEqual(original.Verified, actual.Verified);
            
            // Cleanup
            _resultRepository.Delete(original.GetIdentifier());
            _batchFileRepository.Delete(original.GetIdentifier());
            _taskRepository.Delete(testTask.GetIdentifier());
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

        private BatchFile CreateAndInsertTestBatchFile(string path, string filename, Batch batch)
        {
            BatchFile file = new BatchFile(".txt", Encoding.UTF8.BodyName, new MemoryStream(), batch);
            file.WithPath(path).WithFileName(filename);
            _batchFileRepository.Create(file);
            return file;
        }

        private Result CreateResultFromBatchFile(BatchFile batchFile, Batch batch, bool verified, Task task)
        {
            Result result = new Result(batchFile.OriginalExtension, batchFile.Encoding, batchFile.Data, batch, verified, task);
            result.WithPath(batchFile.Path).WithFileName(batchFile.Filename);
            return result;
        }

        private Task CreateAndInsertTask(Batch batch, int taskNumber, int taskSubnumber)
        {
            Task task = new Task(batch.Id, taskNumber, taskSubnumber);
            _taskRepository.Create(task);
            
            return task;
        }
    }
}