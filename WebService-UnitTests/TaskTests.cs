using WebService;
using WebService.Models;
using Xunit;

namespace WebService_UnitTests
{
    public class TaskTests
    {
        [Fact]
        public void Assign_User_Assigns_User()
        {
            User user = new User("Username",  "Password", 0);
            Task testTask = new Task(false);
            testTask.SetAllocatedTo(user);
            string allocatedTo = testTask.AllocatedTo;
            Assert.Equal(allocatedTo, testTask.AllocatedTo);
        }

        [Fact]
        public void UnAssignUser_UnAssigns_User()
        {
            User user = new User("Username",  "Password", 0);
            Task testTask = new Task(false);
            testTask.SetAllocatedTo(user);
            testTask.UnAllocate();
            Assert.Null(testTask.AllocatedTo);
        }
    }
}