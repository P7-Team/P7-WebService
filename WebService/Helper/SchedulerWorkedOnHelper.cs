using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WebService.Interfaces;
using WebService.Models;
using WebService.Services;

namespace WebService.Helper
{
    public class SchedulerWorkedOnHelper : ISchedulerWorkedOnHelper
    {
        private ReaderWriterLockSlim WorkedOnElementsLock;
        private static List<TaskWrapper> _workedOnElements;
        private const int CleanUpTimeHours = 0;
        private const int CleanUpTimeMinutes = 5;
        private const int CleanUpTimeSeconds = 0;

        public SchedulerWorkedOnHelper()
        {
            WorkedOnElementsLock = new ReaderWriterLockSlim();
            _workedOnElements = new List<TaskWrapper>();
        }

        /// <summary>
        /// Checks whether or not a specific task is being worked on by a user.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool IsWorkedOnBy(Task task, User user)
        {
            WorkedOnElementsLock.EnterReadLock();
            bool status = _workedOnElements.Any(x => x.Task.AllocatedTo == user.Username
                                                     && x.Task.Id == task.Id
                                                     && x.Task.Number == task.Number);
            WorkedOnElementsLock.ExitReadLock();
            return status;
        }

        /// <summary>
        /// Adds a taskWrapper to the list of elements currently being worked on.
        /// </summary>
        /// <param name="taskWrapper"></param>
        /// <param name="user"></param>
        public void AddToWorkedOn(TaskWrapper taskWrapper, User user)
        {
            WorkedOnElementsLock.EnterWriteLock();
            if (taskWrapper.Task.AllocatedTo != null)
            {
                WorkedOnElementsLock.ExitWriteLock();
                return;
            }

            taskWrapper.user = user;
            taskWrapper.Task.SetAllocatedTo(user);
            taskWrapper.AssignedAt = DateTime.Now;
            _workedOnElements.Add(taskWrapper);
            WorkedOnElementsLock.ExitWriteLock();
        }

        // TODO Overvej om denne skal rykkes til SchedulerHistory
        /// <summary>
        /// Marks specific task as done.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="number"></param>
        /// <param name="subNumber"></param>
        public void MarkCurrentlyWorkingOnAsDone(long id, int number, int subNumber)
        {
            WorkedOnElementsLock.EnterWriteLock();
            foreach (var currentTask in _workedOnElements.Where(x => x.Task.Id == id &&
                                                                     x.Task.Number == number &&
                                                                     x.Task.SubNumber == subNumber))
            {
                currentTask.isDone = true;
            }

            WorkedOnElementsLock.ExitWriteLock();
        }

        /// <summary>
        /// Returns the TaskWrapper object which is currently being worked on by a specific user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public TaskWrapper GetCurrentlyWorkedOn(User user)
        {
            WorkedOnElementsLock.EnterReadLock();
            TaskWrapper currentElement = _workedOnElements.Find(x => !x.isDone &&
                                                                     x.user.Equals(user));
            WorkedOnElementsLock.ExitReadLock();
            return currentElement;
        }

        /// <summary>
        /// Cleans any TaskWrappers which have not been pinged for the defined amount of time.
        /// The time is defined within the class.
        /// </summary>
        public void CleanInactiveUsers()
        {
            WorkedOnElementsLock.EnterWriteLock();
            _workedOnElements.RemoveAll(x =>
                !x.isDone && x.LastPing <
                DateTime.Now.Subtract(new TimeSpan(CleanUpTimeHours, CleanUpTimeMinutes, CleanUpTimeSeconds)));

            WorkedOnElementsLock.ExitWriteLock();
        }

        /// <summary>
        /// Updates the lastPing for a specific user, with a timestamp.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="dateTime"></param>
        public void UpdateLastPing(User user, DateTime dateTime)
        {
            WorkedOnElementsLock.EnterWriteLock();
            foreach (TaskWrapper taskWrapper in _workedOnElements.Where(
                x => x.Task.AllocatedTo == user.Username && !x.isDone))
            {
                taskWrapper.LastPing = new DateTime(dateTime.Ticks);
            }

            WorkedOnElementsLock.ExitWriteLock();
        }
    }
}