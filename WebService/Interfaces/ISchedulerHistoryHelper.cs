using WebService.Models;
using WebService.Services;

namespace WebService.Interfaces
{
    public interface ISchedulerHistoryHelper
    {

        public bool HasWorkedOn(Task task, User user);

        public void AddToHistory(TaskWrapper taskWrapper);
    }
}