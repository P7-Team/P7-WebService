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
        public Batch CalledWithBatch { get; private set; }

        public void Create(Batch item)
        {
            CalledWithBatch = item;
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
        public void StoreInputFiles(IEnumerable<BatchFile> inputFiles)
        {
            CalledWithInputFiles = inputFiles;
        }

        public void StoreSourceFile(SourceFile sourceFile)
        {
            CalledWithSourceFile = sourceFile;
        }
    }

    public class BatchStoreTests
    {
        [Fact]
        public void Store_GivenBatch_SavesBatchUsingGivenRepository()
        {
            Batch testBatch = new Batch(1);
            MockBatchRepository batchRep = new MockBatchRepository();
            MockFileStore fileStore = new MockFileStore();
            BatchStore store = new BatchStore(batchRep, fileStore);
            store.Store(testBatch);

            Assert.Equal(testBatch, batchRep.CalledWithBatch);
        }

        [Fact]
        public void Store_GivenBatchWithSourceFile_SavesSourceFiles()
        {
            Batch testBatch = new Batch(2);
            MockBatchRepository batchRep = new MockBatchRepository();
            MockFileStore fileStore = new MockFileStore();
            BatchStore store = new BatchStore(batchRep, fileStore);
            SourceFile source = new SourceFile(new MemoryStream()) { Filename = "testFile" };
            testBatch.SourceFile = source;

            store.Store(testBatch);

            Assert.Equal(source, fileStore.CalledWithSourceFile);
        }

        [Fact]
        public void Store_GivenBatchWithInputFiles_SavesInputFiles()
        {
            Batch testBatch = new Batch(2);
            MockBatchRepository batchRep = new MockBatchRepository();
            MockFileStore fileStore = new MockFileStore();
            BatchStore store = new BatchStore(batchRep, fileStore);
            List<BatchFile> inputFiles = new List<BatchFile>()
            {
                new BatchFile("input1", new MemoryStream()),
                new BatchFile("input2", new MemoryStream()),
            };
            testBatch.InputFiles = inputFiles;

            store.Store(testBatch);

            Assert.Equal(inputFiles, fileStore.CalledWithInputFiles);
        }
    }
}
