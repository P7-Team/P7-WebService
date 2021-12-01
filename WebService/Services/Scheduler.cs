using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using WebService.Interfaces;
using WebService.Models;

namespace WebService.Services
{
    public class Scheduler : IScheduler
    {
        // TODO Implement functionality such that the user can only be assigned to a specific task once.
        private readonly List<Batch> _batches;
        private ReaderWriterLockSlim BatchesLock { get; }

        private readonly ISchedulerWorkedOnHelper _schedulerWoh;
        private readonly ISchedulerHistoryHelper _historyHelper;

        public Scheduler(ISchedulerWorkedOnHelper schedulerWoh, ISchedulerHistoryHelper historyHelper)
        {
            BatchesLock = new ReaderWriterLockSlim();
            _schedulerWoh = schedulerWoh;
            _historyHelper = historyHelper;
            _batches = new List<Batch>();
        }

        public Task AllocateTask(User user)
        {
            BatchesLock.EnterWriteLock();
            try
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
                        BatchesLock.ExitWriteLock();
                        return tempTask;
                    }
                }
            }
            finally
            {
                BatchesLock.ExitWriteLock();
            }


            return null;
        }

        public void AddBatch(Batch batch)
        {
            BatchesLock.EnterWriteLock();
            try
            {
                if (!_batches.Contains(batch))
                {
                    _batches.Add(batch);
                }
            }
            finally
            {
                BatchesLock.ExitWriteLock();
            }
        }

        public void AddBatches(List<Batch> batches)
        {
            foreach (Batch batch in batches)
            {
                AddBatch(batch);
            }
        }

        public void RemoveCompletedTask(Task task)
        {
            BatchesLock.EnterWriteLock();
            try
            {
                RemoveCompletedTask(task.Id, task.Number, task.SubNumber);
            }
            finally

            {
                BatchesLock.ExitWriteLock();
            }
        }

        public void RemoveCompletedTask(long id, int number, int subNumber)
        {
            BatchesLock.EnterReadLock();
            List<Batch> batchesToRemove = new List<Batch>();
            try
            {
                foreach (Batch batch in _batches.Where(batch => batch.Id == id))
                {
                    batch.RemoveTask(number, subNumber);
                    if (batch.TasksCount() == 0) batchesToRemove.Add(batch);
                }
            }
            finally
            {
                BatchesLock.ExitReadLock();
            }

            _historyHelper.AddToHistory(_schedulerWoh.PopTaskWrapper(id, number, subNumber));
            BatchesLock.EnterWriteLock();
            try
            {
                foreach (var batch in batchesToRemove)
                {
                    _batches.Remove(batch);
                }
            }
            finally
            {
                BatchesLock.ExitWriteLock();
            }
        }
    }
}