using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using WebService.Interfaces;
using WebService.Models;
using WebService.Services;
using WebService.Services.Stores;
using WebService.Services.Repositories;

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

    public class BatchStoreTests
    {
        [Fact]
        public void Store_GivenBatch_SavesBatchUsingGivenRepository()
        {
            Batch testBatch = new Batch(182);
            MockBatchRepository rep = new MockBatchRepository();
            BatchStore store = new BatchStore(rep);
            store.Store(testBatch);

            Assert.Equal(testBatch, rep.CalledWithBatch);
        }

        [Fact]
        public void Store_Given()
        {

        }
    }
}
