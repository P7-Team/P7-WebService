using System;
using System.Collections.Generic;
using System.Linq;
using WebService.Models;
using WebService.Services;
using WebService.Interfaces;

namespace WebService.Services.Stores
{
    public class BatchStore : IStore<Batch>
    {
        private readonly IRepository<Batch, int> _batchRepository;
        private readonly IFileStore _fileStore;
        private readonly IStore<Task> _taskStore;

        public BatchStore(IRepository<Batch, int> batchRepository, IFileStore fileStore, IStore<Task> taskStore)
        {
            _batchRepository = batchRepository;
            _fileStore = fileStore;
            _taskStore = taskStore;
        }

        /// <summary>
        /// Store the details of a batch and associated files in the database, and save the files in the file system.
        /// Also handles creating tasks for each inputfile
        /// </summary>
        /// <param name="batch">Batch that should be stored</param>
        public void Store(Batch batch)
        {
            // Store batch in DB and assign resulting id
            int batchId = _batchRepository.Create(batch);
            batch.Id = batchId;

            // Add batchId to files
            AddBatchReferenceToFiles(batch, batchId);

            // store source and input files
            _fileStore.StoreFile(batch.SourceFile);
            _fileStore.StoreFiles(batch.InputFiles);

            // Create and store tasks
            CreateTasks(batch);
            batch.Tasks.ForEach(task => _taskStore.Store(task));
        }

        private void CreateTasks(Batch batch)
        {
            if(batch.InputFiles != null)
                foreach (BatchFile file in batch.InputFiles)
                    batch.AddTask(new Task() { Input = file, Executable = batch.SourceFile });
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
