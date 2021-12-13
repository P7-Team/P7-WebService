using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using WebService.Interfaces;
using WebService.Models;
using WebService.Services.Repositories;

namespace WebService.Services
{
    public class Scheduler : IScheduler
    {
        private readonly List<Batch> _batches;
        private ReaderWriterLockSlim BatchesLock { get; }

        private readonly ISchedulerWorkedOnHelper _schedulerWoh;
        private readonly ISchedulerHistoryHelper _historyHelper;
        private readonly IRepository<Task, (int, int, int)> _taskRepository;

        public Scheduler(ISchedulerWorkedOnHelper schedulerWoh, ISchedulerHistoryHelper historyHelper,
            IDBConnectionFactory connectionFactory)
        {
            BatchesLock = new ReaderWriterLockSlim();
            _schedulerWoh = schedulerWoh;
            _historyHelper = historyHelper;
            _batches = new List<Batch>();

            var conn = connectionFactory.GetConnection();

            var db = connectionFactory.CreateQueryFactory(conn);
            _taskRepository = new TaskRepository(db);
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

                        if (_schedulerWoh.IsWorkedOn(tempTask) && !_schedulerWoh.IsWorkedOnBy(tempTask, user) ||
                            _historyHelper.HasWorkedOn(tempTask, user)) continue;
                        TaskWrapper tw = new TaskWrapper(tempTask)
                        {
                            AssignedAt = DateTime.Now
                        };
                        _schedulerWoh.AddToWorkedOn(tw, user);
                        _taskRepository.Update(tw.Task);
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
                    foreach (Task task in batch.Tasks)
                    {
                        if (task.AllocatedTo != null)
                        {
                            _schedulerWoh.AddToWorkedOn(new TaskWrapper(task), new User(task.AllocatedTo));
                        }
                    }
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
            RemoveCompletedTask(task.Id, task.Number, task.SubNumber);
        }

        public void RemoveInactiveUsers()
        {
            List<TaskWrapper> itemsToPersist = _schedulerWoh.CleanInactiveUsers();
            foreach (TaskWrapper taskWrapper in itemsToPersist)
            {
                _taskRepository.Update(taskWrapper.Task);
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