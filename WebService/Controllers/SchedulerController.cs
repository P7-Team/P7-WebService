using System;
using System.Collections.Generic;
using System.Linq;
using WebService.Interfaces;
using WebService.Models;

namespace WebService.Controllers
{
    public class SchedulerController : IScheduler
    {
        private List<Batch> Batches;

        public SchedulerController()
        {
            Batches = new List<Batch>();
        }

        public Task GetNextTask(User user)
        {
            foreach (Batch currentBatch in Batches)
            {
                for (int i = 0; i < currentBatch.TasksCount(); i++)
                {
                    Task tempTask = currentBatch.GetTask(i);
                    if (tempTask.AllocatedTo == null)
                    {
                        tempTask.SetAllocatedTo(user);
                        return tempTask;
                    }
                }
            }

            return null;
        }

        public void AddBatch(Batch batch)
        {
            Batches.Add(batch);
        }

        public void RemoveCompletedTask(Task task)
        {
            RemoveCompletedTask(task.Id, task.Number, task.SubNumber);
        }

        public void RemoveCompletedTask(long id, int number, int subNumber)
        {
            List<Batch> batchesToRemove = new List<Batch>();
            foreach (Batch batch in Batches.Where(batch => batch.Id == id))
            {
                batch.RemoveTask(number, subNumber);
                if (batch.TasksCount() == 0) batchesToRemove.Add(batch);
            }

            foreach (var batch in batchesToRemove)
            {
                Batches.Remove(batch);
            }
        }
    }
}