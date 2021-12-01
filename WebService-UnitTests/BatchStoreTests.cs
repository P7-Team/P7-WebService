using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using WebService.Interfaces;
using WebService.Models;
using WebService.Services;
using WebService.Services.Stores;
using WebService.Services.Repositories;
using System.IO;
using System.Linq;

namespace WebService_UnitTests
{
    class MockBatchRepository : IRepository<Batch, int>
    {
        private int _batchId;

        public MockBatchRepository(int batchId)
        {
            _batchId = batchId;
        }

        public Batch CalledWithBatch { get; private set; }

        public int Create(Batch item)
        {
            CalledWithBatch = item;
            return _batchId;
        }

        public void Delete(int identifier)
        {
            throw new NotImplementedException();
        }

        public Batch Read(int identifier)
        {
            throw new NotImplementedException();
        }

        public void Update(Batch item)
        {
            throw new NotImplementedException();
        }
    }

    class MockFileStore : IFileStore
    {
        public SourceFile CalledWithSourceFile { get; private set; }
        public IEnumerable<BatchFile> CalledWithInputFiles { get; private set; }

        public string Directory { get; }

        public void StoreFile(SourceFile sourceFile)
        {
            CalledWithSourceFile = sourceFile;
        }

        public void StoreFile(Result resultFile)
        {
            throw new NotImplementedException();
        }

        public void StoreFiles(IEnumerable<BatchFile> batchFiles)
        {
            CalledWithInputFiles = batchFiles;
        }

        public Stream FetchFile(BatchFile file)
        {
            throw new NotImplementedException();
        }

        public bool DeleteFile(BatchFile file)
        {
            throw new NotImplementedException();
        }

        public bool DeleteFile(SourceFile sourceFile)
        {
            throw new NotImplementedException();
        }

        public bool DeleteFile(Result resultFile)
        {
            throw new NotImplementedException();
        }
    }

    class MockTaskStore : IStore<Task>
    {
        public List<Task> CalledWithTasks { get; private set; }

        public MockTaskStore()
        {
            CalledWithTasks = new List<Task>();
        }

        public void Store(Task item)
        {
            CalledWithTasks.Add(item);
        }
    }

    public class BatchStoreTests
    {
        private MockBatchRepository batchRepository;
        private MockFileStore fileStore;
        private MockTaskStore taskStore;

        private BatchStore CreateBatchStore(int batchId)
        {
            batchRepository = new MockBatchRepository(batchId);
            fileStore = new MockFileStore();
            taskStore = new MockTaskStore();

            return new BatchStore(batchRepository, fileStore, taskStore);
        }

        [Fact]
        public void Store_GivenBatch_SavesBatchUsingGivenRepository()
        {
            Batch testBatch = new Batch();
            BatchStore store = CreateBatchStore(1);
            store.Store(testBatch);

            Assert.Equal(testBatch, batchRepository.CalledWithBatch);
        }

        [Fact]
        public void Store_GivenBatch_AssignsIdToBatch()
        {
            Batch testBatch = new Batch();
            BatchStore store = CreateBatchStore(2);
            store.Store(testBatch);

            Assert.Equal(2, testBatch.Id);
        }

        [Fact]
        public void Store_GivenBatchWithSourceFile_SavesSourceFiles()
        {
            Batch testBatch = new Batch();
            BatchStore store = CreateBatchStore(3);
            SourceFile source = new SourceFile("", Encoding.UTF8.EncodingName, new MemoryStream(), testBatch, "language") { Filename = "testFile" };
            testBatch.SourceFile = source;

            store.Store(testBatch);

            Assert.Equal(source, fileStore.CalledWithSourceFile);
        }

        [Fact]
        public void Store_GivenBatchWithSourceFile_SetsFileBatchId()
        {
            Batch testBatch = new Batch();
            BatchStore store = CreateBatchStore(4);
            SourceFile source = new SourceFile("", Encoding.UTF8.EncodingName, new MemoryStream(), testBatch, "language") { Filename = "testFile" };
            testBatch.SourceFile = source;

            store.Store(testBatch);

            Assert.Equal(4, testBatch.SourceFile.BatchId);
        }

        [Fact]
        public void Store_GivenBatchWithInputFiles_SavesInputFiles()
        {
            Batch testBatch = new Batch();
            BatchStore store = CreateBatchStore(5);
            List<BatchFile> inputFiles = new List<BatchFile>()
            {
                new BatchFile("input1", Encoding.UTF8.EncodingName, new MemoryStream(), testBatch),
                new BatchFile("input2", Encoding.UTF8.EncodingName, new MemoryStream(), testBatch),
            };
            testBatch.InputFiles = inputFiles;

            store.Store(testBatch);

            Assert.Equal(inputFiles, fileStore.CalledWithInputFiles);
        }

        [Fact]
        public void Store_GivenBatchWithInputFiles_SetsFilesBatchId()
        {
            Batch testBatch = new Batch();
            BatchStore store = CreateBatchStore(6);
            List<BatchFile> inputFiles = new List<BatchFile>()
            {
                new BatchFile("input1", Encoding.UTF8.EncodingName, new MemoryStream(), testBatch),
                new BatchFile("input2", Encoding.UTF8.EncodingName, new MemoryStream(), testBatch),
            };
            testBatch.InputFiles = inputFiles;

            store.Store(testBatch);

            Assert.Equal(6, testBatch.InputFiles[0].BatchId);
            Assert.Equal(6, testBatch.InputFiles[1].BatchId);
        }

        [Fact]
        public void Store_GivenBatchWithInputsAndSource_CreatesAndStoresTasks()
        {
            Batch testBatch = new Batch();
            BatchStore store = CreateBatchStore(7);
            List<BatchFile> inputFiles = new List<BatchFile>()
            {
                new BatchFile("input1", Encoding.UTF8.EncodingName, new MemoryStream(), testBatch),
                new BatchFile("input2", Encoding.UTF8.EncodingName, new MemoryStream(), testBatch),
            };
            testBatch.InputFiles = inputFiles;

            SourceFile source = new SourceFile("", Encoding.UTF8.EncodingName, new MemoryStream(), testBatch, "language") { Filename = "testFile" };
            testBatch.SourceFile = source;

            store.Store(testBatch);

            // All tasks reference testBatch and have batch source
            Assert.True(taskStore.CalledWithTasks.TrueForAll(task => task.Id == testBatch.Id));
            Assert.True(taskStore.CalledWithTasks.TrueForAll(task => task.Executable == testBatch.SourceFile));
            // There is a task containing each inputfile
            Assert.Contains(taskStore.CalledWithTasks, task => task.Input == inputFiles[0]);
            Assert.Contains(taskStore.CalledWithTasks, task => task.Input == inputFiles[1]);
        }
    }
}
