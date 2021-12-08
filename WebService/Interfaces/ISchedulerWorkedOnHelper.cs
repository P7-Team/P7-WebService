using System;
using System.Collections.Generic;
using System.Threading;
using WebService.Models;
using WebService.Services;

namespace WebService.Interfaces
{
    public interface ISchedulerWorkedOnHelper
    {
        public ReaderWriterLockSlim WorkedOnElementsLock { get; }
        public bool IsWorkedOnBy(Task task, User user);

        public bool IsWorkedOn(Task task);

        public void AddToWorkedOn(TaskWrapper taskWrapper, User user);

        public void UpdateLastPing(string providerId, DateTime dateTime);

        public TaskWrapper GetCurrentlyWorkedOn(User user);

        public void CleanInactiveUsers();

        public TaskWrapper PopTaskWrapper(long id, int number, int subNumber);

        public List<TaskWrapper> CurrentlyWorkedOn();
    }
}