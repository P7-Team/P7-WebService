using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using WebService.Interfaces;
using WebService.Models;

namespace WebService.Services
{
    public class Scheduler : IScheduler
    {
        // TODO Implement functionality such that the user can only be assigned to a specific task once.
        private readonly List<Batch> _batches;

        private readonly ISchedulerWorkedOnHelper _schedulerWoh;
        private readonly ISchedulerHistoryHelper _historyHelper;

        public Scheduler(ISchedulerWorkedOnHelper schedulerWoh, ISchedulerHistoryHelper historyHelper)
        {
            _schedulerWoh = schedulerWoh;
            _historyHelper = historyHelper;
            _batches = new List<Batch>();
        }

        public Task AllocateTask(User user)
        {
            foreach (Batch currentBatch in _batches)
            {
                for (int i = 0; i < currentBatch.TasksCount(); i++)
                {
                    Task tempTask = currentBatch.GetTask(i);

                    if (_schedulerWoh.IsWorkedOn(tempTask) || _historyHelper.HasWorkedOn(tempTask, user)) continue;
                    TaskWrapper tw = new TaskWrapper(tempTask)
                    {
                        AssignedAt = DateTime.Now
                    };
                    _schedulerWoh.AddToWorkedOn(tw, user);
                    return tempTask;
                }
            }

            return null;
        }

        public void AddBatch(Batch batch)
        {
            _batches.Add(batch);
        }

        public void RemoveCompletedTask(Task task)
        {
            RemoveCompletedTask(task.Id, task.Number, task.SubNumber);
        }

        public void RemoveCompletedTask(long id, int number, int subNumber)
        {
            List<Batch> batchesToRemove = new List<Batch>();
            foreach (Batch batch in _batches.Where(batch => batch.Id == id))
            {
                batch.RemoveTask(number, subNumber);
                if (batch.TasksCount() == 0) batchesToRemove.Add(batch);
            }

            _historyHelper.AddToHistory(_schedulerWoh.PopTaskWrapper(id, number, subNumber));

            foreach (var batch in batchesToRemove)
            {
                _batches.Remove(batch);
            }
        }
    }
}