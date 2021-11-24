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

    public class BatchStoreTests
    {
        [Fact]
        public void Store_GivenBatch_SavesBatchUsingGivenRepository()
        {
            Batch testBatch = new Batch();
            MockBatchRepository batchRep = new MockBatchRepository(1);
            MockFileStore fileStore = new MockFileStore();
            BatchStore store = new BatchStore(batchRep, fileStore);
            store.Store(testBatch);

            Assert.Equal(testBatch, batchRep.CalledWithBatch);
        }

        [Fact]
        public void Store_GivenBatch_AssignsIdToBatch()
        {
            Batch testBatch = new Batch();
            MockBatchRepository batchRep = new MockBatchRepository(2);
            MockFileStore fileStore = new MockFileStore();
            BatchStore store = new BatchStore(batchRep, fileStore);
            store.Store(testBatch);

            Assert.Equal(2, testBatch.Id);
        }

        [Fact]
        public void Store_GivenBatchWithSourceFile_SavesSourceFiles()
        {
            Batch testBatch = new Batch();
            MockBatchRepository batchRep = new MockBatchRepository(3);
            MockFileStore fileStore = new MockFileStore();
            BatchStore store = new BatchStore(batchRep, fileStore);
            SourceFile source = new SourceFile("", Encoding.UTF8.EncodingName, new MemoryStream(), testBatch, "language") { Filename = "testFile" };
            testBatch.SourceFile = source;

            store.Store(testBatch);

            Assert.Equal(source, fileStore.CalledWithSourceFile);
        }

        [Fact]
        public void Store_GivenBatchWithSourceFile_SetsFileBatchId()
        {
            Batch testBatch = new Batch();
            MockBatchRepository batchRep = new MockBatchRepository(4);
            MockFileStore fileStore = new MockFileStore();
            BatchStore store = new BatchStore(batchRep, fileStore);
            SourceFile source = new SourceFile("", Encoding.UTF8.EncodingName, new MemoryStream(), testBatch, "language") { Filename = "testFile" };
            testBatch.SourceFile = source;

            store.Store(testBatch);

            Assert.Equal(4, testBatch.SourceFile.BatchId);
        }

        [Fact]
        public void Store_GivenBatchWithInputFiles_SavesInputFiles()
        {
            Batch testBatch = new Batch();
            MockBatchRepository batchRep = new MockBatchRepository(5);
            MockFileStore fileStore = new MockFileStore();
            BatchStore store = new BatchStore(batchRep, fileStore);
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
            MockBatchRepository batchRep = new MockBatchRepository(6);
            MockFileStore fileStore = new MockFileStore();
            BatchStore store = new BatchStore(batchRep, fileStore);
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
    }
}
