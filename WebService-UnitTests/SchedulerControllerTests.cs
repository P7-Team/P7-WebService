using System;
using WebService;
using WebService.Interfaces;
using WebService.Models;
using WebService.Services;
using Xunit;

namespace WebService_UnitTests
{
    public class SchedulerControllerTests
    {
        [Fact]
        public void GetNextTask_Returns_Task()
        {
            IScheduler scheduler = new SchedulerController();
            User user = new User("Username", 0, "Password");
            Task task = new Task(true);
            Batch testBatch = new Batch(0);
            // Act
            testBatch.AddTask(task);
            scheduler.AddBatch(testBatch);
            Assert.IsType<Task>(scheduler.GetTaskAndAssignUser(user));
        }


        [Fact]
        public void GetNextTask_User_Already_Assigned_Returns_Null()
        {
            IScheduler scheduler = new SchedulerController();
            User user = new User("Username", 0, "Password");
            Task task = new Task(true);
            task.SetAllocatedTo(user);
            Batch testBatch = new Batch(0);
            testBatch.AddTask(task);
            scheduler.AddBatch(testBatch);
            // Assert
            Assert.Null(scheduler.GetTaskAndAssignUser(user));
        }

        [Fact]
        public void GetNextTask_Assigns_User()
        {
            IScheduler scheduler = new SchedulerController();
            User user = new User("Username", 0, "Password");
            User anotherUser = new User("AnotherUser", 0, "Password");
            Task task = new Task(true);
            Task taskOne = new Task(true);
            Batch testBatch = new Batch(0);
            testBatch.AddTask(task);
            testBatch.AddTask(taskOne);
            scheduler.AddBatch(testBatch);
            scheduler.GetTaskAndAssignUser(user);
            // Assert
            Assert.Equal(1, scheduler.GetTaskAndAssignUser(anotherUser).Number);
        }

        [Fact]
        public void GetNextTask_Another_User_Already_Assigned_Returns_Null()
        {
            IScheduler scheduler = new SchedulerController();
            User user = new User("Username", 0, "Password");
            User anotherUser = new User("AnotherUser", 0, "Password");
            Task task = new Task(true);
            task.SetAllocatedTo(anotherUser);
            Batch testBatch = new Batch(0);
            scheduler.AddBatch(testBatch);
            // Assert
            Assert.Null(scheduler.GetTaskAndAssignUser(user));
        }

        [Fact]
        public void Remove_Completed_Task_Task_Provided()
        {
            IScheduler scheduler = new SchedulerController();
            User user = new User("Username", 0, "Password");
            Task task = new Task(true);

            Batch testBatch = new Batch(0);
            testBatch.AddTask(task);
            scheduler.AddBatch(testBatch);
            // Act
            testBatch.RemoveTask(task);
            // Assert
            Assert.Null(scheduler.GetTaskAndAssignUser(user));
        }

        [Fact]
        public void Remove_Completed_Task_ID_Number_SubNumber_Provided()
        {
            IScheduler scheduler = new SchedulerController();
            User user = new User("Username", 0, "Password");
            Task task = new Task(true);

            Batch testBatch = new Batch(0);
            testBatch.AddTask(task);
            scheduler.AddBatch(testBatch);
            // Act
            scheduler.RemoveCompletedTask(0, 0, 0);
            // Assert
            Assert.Null(scheduler.GetTaskAndAssignUser(user));
        }

        [Fact]
        public void Remove_Completed_Task_Incorrect_ID_Number_SubNumber_Provided()
        {
            IScheduler scheduler = new SchedulerController();
            User user = new User("Username", 0, "Password");
            Task task = new Task(true);

            Batch testBatch = new Batch(0);
            testBatch.AddTask(task);
            scheduler.AddBatch(testBatch);
            // Act
            // AS there are not 20 tasks, trying to remove number 20 will not modify the queue.
            scheduler.RemoveCompletedTask(0, 20, 20);
            // Assert
            Assert.IsType<Task>(scheduler.GetTaskAndAssignUser(user));
        }

        [Fact]
        public void Remove_Completed_Task_Remove_Batch_On_Emptying_Batch()
        {
            IScheduler scheduler = new SchedulerController();
            User user = new User("Username", 0, "Password");
            Task task = new Task(true);
            Task taskOne = new Task(true);

            Batch testBatch = new Batch(0);
            Batch testBatchTwo = new Batch(1);
            testBatch.AddTask(task);
            testBatchTwo.AddTask(taskOne);
            scheduler.AddBatch(testBatch);
            scheduler.AddBatch(testBatchTwo);
            // Act
            // AS there are not 20 tasks, trying to remove number 20 will not modify the queue.
            scheduler.RemoveCompletedTask(0, 0, 0);
            // Assert
            Assert.Equal(testBatchTwo.Id, scheduler.GetTaskAndAssignUser(user).Id);
        }

        [Fact]
        public void Remove_Completed_Task_Only_Remove_Task_If_More_Tasks_Are_In_Batch()
        {
            IScheduler scheduler = new SchedulerController();
            User user = new User("Username", 0, "Password");
            Task task = new Task(true);
            Task taskOne = new Task(true);
            Task taskTwo = new Task(true);

            Batch testBatch = new Batch(0);
            Batch testBatchTwo = new Batch(1);
            testBatch.AddTask(task);
            testBatch.AddTask(taskOne);
            testBatchTwo.AddTask(taskTwo);
            scheduler.AddBatch(testBatch);
            scheduler.AddBatch(testBatchTwo);
            // Act
            // AS there are not 20 tasks, trying to remove number 20 will not modify the queue.
            scheduler.RemoveCompletedTask(0, 0, 0);
            // Assert
            Assert.Equal(testBatch.Id, scheduler.GetTaskAndAssignUser(user).Id);
        }

        [Fact]
        public void UnAssignUserFromTasks_UnAssignsUser()
        {
            IScheduler scheduler = new SchedulerController();
            User user = new User("Username", 0, "Password");
            Task task = new Task(true);
            Batch testBatch = new Batch(0);
            testBatch.AddTask(task);
            scheduler.AddBatch(testBatch);
            scheduler.GetTaskAndAssignUser(user);
            // Using the fact that objects are pass by reference.
            scheduler.UnAssignUserFromTask(user, testBatch.Id, task.Number, task.SubNumber);

            Assert.Null(task.AllocatedTo);
        }

        [Fact]
        public void GetTaskAndAssignUser_Assigns_User()
        {
            IScheduler scheduler = new SchedulerController();
            User user = new User("Username", 0, "Password");
            Task task = new Task(true);
            Batch testBatch = new Batch(0);
            testBatch.AddTask(task);
            scheduler.AddBatch(testBatch);
            scheduler.GetTaskAndAssignUser(user);
            // Using the fact that objects are pass by reference.

            Assert.Equal(user.Username, task.AllocatedTo);
        }

        [Fact]
        public void PingScheduler_Ping_Set()
        {
            IScheduler scheduler = new SchedulerController();
            User user = new User("Username", 0, "Password");
            Task task = new Task(true);
            Batch testBatch = new Batch(0);
            // Act
            testBatch.AddTask(task);
            scheduler.AddBatch(testBatch);
            scheduler.GetTaskAndAssignUser(user);
            scheduler.PingScheduler(user,DateTime.Now);

            Assert.IsType<DateTime>(scheduler.GetLastPing(user));
        }

        [Fact]
        public void PingScheduler_Ping_Updated()
        {
            IScheduler scheduler = new SchedulerController();
            User user = new User("Username", 0, "Password");
            Task task = new Task(true);
            Batch testBatch = new Batch(0);
            // Act
            testBatch.AddTask(task);
            scheduler.AddBatch(testBatch);
            scheduler.GetTaskAndAssignUser(user);
            scheduler.PingScheduler(user,DateTime.Now.Subtract(new TimeSpan(2,0,2,0)));
            DateTime? lastPing = scheduler.GetLastPing(user);
            scheduler.PingScheduler(user,DateTime.Now);
            
            Assert.True(lastPing < scheduler.GetLastPing(user));
        }
    }
}