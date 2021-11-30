using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WebService.Interfaces;
using WebService.Models;
using WebService.Services;

namespace WebService.Helper
{
    public class SchedulerHistoryHelper : ISchedulerHistoryHelper
    {
        private readonly List<TaskWrapper> _history;
        private readonly ReaderWriterLockSlim _historyLock;

        public SchedulerHistoryHelper()
        {
            _historyLock = new ReaderWriterLockSlim();
            _history = new List<TaskWrapper>();
        }

        /// <summary>
        /// Returns true if a user have worked on a task with the same Id and number
        /// </summary>
        /// <param name="task"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool HasWorkedOn(Task task, User user)
        {
            _historyLock.EnterReadLock();
            try
            {
                bool status = _history.Any(x => task.Id == x.Task.Id &&
                                            x.Task.Number == task.Number &&
                                            x.user != null && x.user.Equals(user));
                return status;
            }
            finally
            {
                _historyLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Adds taskWrapper to history.
        /// </summary>
        /// <param name="taskWrapper">Is required to have the user defined.</param>
        public void AddToHistory(TaskWrapper taskWrapper)
        {
            _historyLock.EnterWriteLock();
            try
            {
                if (taskWrapper != null) _history.Add(taskWrapper);
            }
            finally
            {
                _historyLock.ExitWriteLock();
            }
        }
    }
}