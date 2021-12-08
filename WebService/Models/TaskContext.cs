using System;
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
        private readonly TaskRepository _taskRepository;

        public TaskContext(IFileStore fileStore, BatchRepository batchRepository, TaskRepository taskRepository)
        {
            _fileStore = fileStore;
            _batchRepository = batchRepository;
            _taskRepository = taskRepository;
        }

        public void SaveResult(Result result)
        {
            _fileStore.StoreFile(result);
        }

        public Batch GetBatch(int batchId)
        {
            return _batchRepository.Read(batchId);
        }

        public void UpdateCompletedTask(int id, int number, int subnumber)
        {
            Task task = _taskRepository.Read((id, number, subnumber));
            task.FinishedOn = DateTime.Now;
            _taskRepository.Update(task);
        }
    }
}