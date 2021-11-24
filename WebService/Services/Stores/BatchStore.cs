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

        /// <summary>
        /// Store the details of a batch and associated files in the database, and save the files in the file system
        /// </summary>
        /// <param name="batch">Batch that should be stored</param>
        public void Store(Batch batch)
        {
            // Store batch in DB and add batchId to files
            int batchId = _batchRepository.Create(batch);
            AddBatchReferenceToFiles(batch, batchId);

            // store source and input files
            _fileStore.StoreFile(batch.SourceFile);
            _fileStore.StoreFiles(batch.InputFiles);
        }

        private void AddBatchReferenceToFiles(Batch batch, int batchId)
        {
            // Assign batchId property of source and input files
            if(batch.SourceFile != null)
                batch.SourceFile.BatchId = batchId;
            if(batch.InputFiles != null)
                batch.InputFiles.ForEach(file => file.BatchId = batchId);
        }
    }
}
