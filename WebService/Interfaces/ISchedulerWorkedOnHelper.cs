using System;
using WebService.Models;
using WebService.Services;

namespace WebService.Interfaces
{
    public interface ISchedulerWorkedOnHelper
    {
        public bool IsWorkedOnBy(Task task, User user);

        public bool IsWorkedOn(Task task);

        public void AddToWorkedOn(TaskWrapper taskWrapper, User user);

        public void MarkCurrentlyWorkingOnAsDone(long id, int number, int subNumber);

        public void UpdateLastPing(User user, DateTime dateTime);

        public TaskWrapper GetCurrentlyWorkedOn(User user);

        public void CleanInactiveUsers();

        public TaskWrapper PopTaskWrapper(long id, int number, int subNumber);
    }
}