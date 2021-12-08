using System;
using System.Collections.Generic;
using WebService.Models;

namespace WebService.Interfaces
{
    public interface IScheduler
    {
        public Task AllocateTask(User user);

        public void AddBatch(Batch batch);
        public void AddBatches(List<Batch> batches);

        public void RemoveCompletedTask(Task task);

        public void RemoveCompletedTask(long id, int number, int subNumber);
        
    }
}