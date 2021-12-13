using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WebService.Interfaces;
using WebService.Models;
using WebService.Services;
using WebService.Services.Repositories;

namespace WebService.Helper
{
    public class SchedulerWorkedOnHelper : ISchedulerWorkedOnHelper
    {
        public ReaderWriterLockSlim WorkedOnElementsLock { get; }
        private readonly List<TaskWrapper> _workedOnElements;
        private const int CleanUpTimeHours = 0;
        private const int CleanUpTimeMinutes = 0;
        private const int CleanUpTimeSeconds = 10;

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
            try
            {
                return _workedOnElements.Any(x => x.Task.AllocatedTo == user.Username
                                                     && x.Task.Id == task.Id
                                                     && x.Task.Number == task.Number);
            }
            finally
            {
                WorkedOnElementsLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Checks whether or not a task is being worked on.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public bool IsWorkedOn(Task task)
        {
            WorkedOnElementsLock.EnterReadLock();
            try
            {
                return _workedOnElements.Any(x => x.Task.Id == task.Id
                                                     && x.Task.Number == task.Number);
            }
            finally
            {
                WorkedOnElementsLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Adds a taskWrapper to the list of elements currently being worked on.
        /// </summary>
        /// <param name="taskWrapper"></param>
        /// <param name="user"></param>
        public void AddToWorkedOn(TaskWrapper taskWrapper, User user)
        {
            if (IsWorkedOn(taskWrapper.Task))
                return;
            else
                WorkedOnElementsLock.EnterWriteLock();

            try
            {
                taskWrapper.user = user;
                taskWrapper.Task.SetAllocatedTo(user);
                taskWrapper.AssignedAt = DateTime.Now;
                _workedOnElements.Add(taskWrapper);
            }
            finally
            {
                WorkedOnElementsLock.ExitWriteLock();
            }
        }
        

        /// <summary>
        /// Returns the TaskWrapper object which is currently being worked on by a specific user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public TaskWrapper GetCurrentlyWorkedOn(User user)
        {
            WorkedOnElementsLock.EnterReadLock();
            try
            {
                return _workedOnElements.Find(x => x.user.Equals(user));
            }
            finally
            {
                WorkedOnElementsLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Cleans any TaskWrappers which have not been pinged for the defined amount of time.
        /// The time is defined within the class.
        /// </summary>
        public List<TaskWrapper> CleanInactiveUsers()
        {
            
            WorkedOnElementsLock.EnterWriteLock();
            try
            {
                List<TaskWrapper> inactiveElements = _workedOnElements.FindAll(x => x.LastPing <
                DateTime.Now.Subtract(new TimeSpan(CleanUpTimeHours, CleanUpTimeMinutes, CleanUpTimeSeconds)));
                foreach (TaskWrapper inactiveElement in inactiveElements)
                {
                    inactiveElement.Task.UnAllocate();
                    _workedOnElements.Remove(inactiveElement);
                }

                return inactiveElements;
            }
            finally
            {
                WorkedOnElementsLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Pops a taskWrapper from the list, and returns this.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="number"></param>
        /// <param name="subNumber"></param>
        /// <returns></returns>
        public TaskWrapper PopTaskWrapper(long id, int number, int subNumber)
        {
            WorkedOnElementsLock.EnterWriteLock();
            try
            {
                TaskWrapper taskWrapper = _workedOnElements.Find(x => x.Task.Id == id &&
                                                                  x.Task.Number == number &&
                                                                  x.Task.SubNumber == subNumber);
                _workedOnElements.Remove(taskWrapper);
                return taskWrapper;
            }
            finally
            {
                WorkedOnElementsLock.ExitWriteLock();
            }
        }

        public List<TaskWrapper> CurrentlyWorkedOn()
        {
            return _workedOnElements;
        }

        /// <summary>
        /// Updates the lastPing for a specific user, with a timestamp.
        /// </summary>
        /// <param name="providerId"></param>
        /// <param name="dateTime"></param>
        public void UpdateLastPing(string providerId, DateTime dateTime)
        {
            WorkedOnElementsLock.EnterWriteLock();
            try
            {
                foreach (TaskWrapper taskWrapper in _workedOnElements.Where(
                x => x.Task.AllocatedTo == providerId))
                {
                    taskWrapper.LastPing = new DateTime(dateTime.Ticks);
                }
            }
            finally
            {
                WorkedOnElementsLock.ExitWriteLock();
            }
        }
    }
}