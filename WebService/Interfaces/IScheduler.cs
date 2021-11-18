using System;
using WebService.Models;

namespace WebService.Interfaces
{
    public interface IScheduler
    {
        public Task AllocateTask(User user);

        public void AddBatch(Batch batch);

        public void RemoveCompletedTask(Task task);

        public void RemoveCompletedTask(long id, int number, int subNumber);

        public void UnAssignUserFromTask(User user,long id, int number, int subNumber);

        public void PingScheduler(User user,DateTime time);

        public DateTime? GetLastPing(User user);

        public void FreeTasksNoLongerWorkedOn();
    }
}