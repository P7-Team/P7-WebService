using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebService.Models;
using WebService.Services;
using WebService.Interfaces;

namespace WebService.Services.Stores
{
    public class BatchStore
    {
        private readonly IRepository<Batch, int> _batchRepository;
        private readonly IFileStore _fileStore;

        public BatchStore(IRepository<Batch, int> batchRepository, IFileStore fileStore)
        {
            _batchRepository = batchRepository;
            _fileStore = fileStore;
        }

        public void Store(Batch batch)
        {
            _batchRepository.Create(batch);
            _fileStore.StoreSourceFile(batch.SourceFile);
            _fileStore.StoreInputFiles(batch.InputFiles);
        }
    }
}
