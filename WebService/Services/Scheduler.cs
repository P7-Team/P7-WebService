using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using WebService.Interfaces;
using WebService.Models;

namespace WebService.Services
{
    class TaskWrapper
    {
        public DateTime LastPing;
        public bool isDone;
        public User user;
        public Task Task;

        public TaskWrapper(Task task, User user)
        {
            this.user = user;
            isDone = false;
            Task = task;
            LastPing = DateTime.Now;
        }
    }

    public class Scheduler : IScheduler
    {
        // TODO Implement functionality such that the user can only be assigned to a specific task once.
        private readonly List<Batch> _batches;

        private readonly List<TaskWrapper> _taskWrappers;

        public Scheduler()
        {
            _taskWrappers = new List<TaskWrapper>();
            _batches = new List<Batch>();
        }

        public Task AllocateTask(User user)
        {
            foreach (Batch currentBatch in _batches)
            {
                for (int i = 0; i < currentBatch.TasksCount(); i++)
                {
                    Task tempTask = currentBatch.GetTask(i);

                    if (tempTask.AllocatedTo != null) continue;

                    if (_taskWrappers.Any(x => x.Task.AllocatedTo == user.Username
                                                && x.Task.Id == currentBatch.Id
                                                && x.Task.Number == tempTask.Number)) continue;
                    tempTask.SetAllocatedTo(user);
                    TaskWrapper tw = new TaskWrapper(tempTask, user);
                    _taskWrappers.Add(tw);
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

            foreach (var currentTask in _taskWrappers.Where(x => x.Task.Id == id && x.Task.Number == number))
            {
                currentTask.isDone = true;
            }

            foreach (var batch in batchesToRemove)
            {
                _batches.Remove(batch);
            }
        }

        public void UnAssignUserFromTask(User user, long id, int number, int subNumber)
        {
            foreach (Batch batch in _batches.Where(batch => batch.Id == id))
            {
                batch.GetTask(number, subNumber)?.UnAllocateFrom(user);
            }
        }

        public void PingScheduler(User user, DateTime dateTime)
        {
            // TODO Implement timestamp on a task for when a user last pinged the server.
            foreach (TaskWrapper taskWrapper in _taskWrappers.Where(x => x.Task.AllocatedTo == user.Username && !x.isDone))
            {
                taskWrapper.LastPing = dateTime;
            }
        }

        public DateTime? GetLastPing(User user)
        {
            foreach (TaskWrapper taskWrapper in _taskWrappers.Where(x => x.Task.AllocatedTo == user.Username && !x.isDone))
            {
                return taskWrapper.LastPing;
            }

            return null;
        }

        public void FreeTasksNoLongerWorkedOn()
        {
            foreach (var tw in _taskWrappers.Where(x=> !x.isDone && x.LastPing < DateTime.Now.Subtract(new TimeSpan(0, 5, 0))))
            {
                UnAssignUserFromTask(tw.user, tw.Task.Id, tw.Task.Number, tw.Task.SubNumber);
            }
        }
        
    }
}