using WebService;
using WebService.Helper;
using WebService.Interfaces;
using WebService.Models;
using WebService.Services;
using Xunit;

namespace WebService_UnitTests
{
    public class SchedulerHistoryHelperTester
    {
        [Fact]
        public void History_Helper_Class_Test()
        {
            ISchedulerHistoryHelper historyHelper = new SchedulerHistoryHelper();
            Task task = new Task();
            User user = new User("Username", 0, "Password");
            TaskWrapper tw = new TaskWrapper(task)
            {
                user = user
            };
            historyHelper.AddToHistory(tw);

            Assert.True(historyHelper.HasWorkedOn(task, user));
        }
    }
}