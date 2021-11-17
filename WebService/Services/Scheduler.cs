using System;
using System.Collections.Generic;
using System.Linq;
using WebService.Interfaces;
using WebService.Models;

namespace WebService.Services
{
    class TaskWrapper
    {
        public int TaskNumber;
        public long Id;
        public DateTime LastPing;
        public string Assignee;
        public bool isDone;

        public TaskWrapper(Task task)
        {
            TaskNumber = task.Number;
            Id = task.Id;
            isDone = false;
            LastPing = DateTime.Now;
            Assignee = task.AllocatedTo;
        }
    }

    public class SchedulerController : IScheduler
    {
        // TODO Implement functionality such that the user can only be assigned to a specific task once.
        private List<Batch> _batches;

        private List<TaskWrapper> _assignedUsers;

        public SchedulerController()
        {
            _assignedUsers = new List<TaskWrapper>();
            _batches = new List<Batch>();
        }

        public Task GetTaskAndAssignUser(User user)
        {
            foreach (Batch currentBatch in _batches)
            {
                for (int i = 0; i < currentBatch.TasksCount(); i++)
                {
                    Task tempTask = currentBatch.GetTask(i);

                    if (tempTask.AllocatedTo != null) continue;

                    if (_assignedUsers.Any(x => x.Assignee == user.Username
                                                && x.Id == currentBatch.Id
                                                && x.TaskNumber == tempTask.Number)) continue;
                    tempTask.SetAllocatedTo(user);
                    TaskWrapper tw = new TaskWrapper(tempTask);
                    _assignedUsers.Add(tw);
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

            foreach (var currentTask in _assignedUsers.Where(x => x.Id == id && x.TaskNumber == number))
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
            foreach (TaskWrapper taskWrapper in _assignedUsers.Where(x => x.Assignee == user.Username && !x.isDone))
            {
                taskWrapper.LastPing = dateTime;
            }
        }

        public DateTime? GetLastPing(User user)
        {
            foreach (TaskWrapper taskWrapper in _assignedUsers.Where(x => x.Assignee == user.Username && !x.isDone))
            {
                return taskWrapper.LastPing;
            }

            return null;
        }
    }
}