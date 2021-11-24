using System.Collections;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebService.Interfaces;
using WebService.Services.Repositories;

namespace WebService.Models
{
    public class TaskContext : List<Task>, ITaskContext
    {
        private readonly IFileStore _fileStore;
        private readonly BatchRepository _batchRepository;

        public TaskContext(IFileStore fileStore, BatchRepository batchRepository)
        {
            _fileStore = fileStore;
            _batchRepository = batchRepository;
        }

        public void SaveResult(Result result)
        {
            _fileStore.StoreFile(result);
        }

        public Batch GetBatch(int batchId)
        {
            return _batchRepository.Read(batchId);
        }
    }
}