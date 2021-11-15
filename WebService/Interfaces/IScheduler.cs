using WebService.Models;

namespace WebService.Interfaces
{
    public interface IScheduler
    {
        public Task GetNextTask(User user);

        public void AddBatch(Batch batch);

        public void RemoveCompletedTask(Task task);

        public void RemoveCompletedTask(long id, int number, int subNumber);
    }
}