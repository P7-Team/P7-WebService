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
        private const int InRangeNumber = 0;
        private const int OutOfRangeNumber = 20;
        [Fact]
        public void GetNextTask_Returns_Task()
        {
            IScheduler scheduler = new Scheduler();
            User user = new User("Username", "Password", 0);
            Task task = new Task(true);
            Batch testBatch = new Batch(0);
            // Act
            testBatch.AddTask(task);
            scheduler.AddBatch(testBatch);
            Assert.IsType<Task>(scheduler.AllocateTask(user));
        }


        [Fact]
        public void GetNextTask_User_Already_Assigned_Returns_Null()
        {
            IScheduler scheduler = new Scheduler();
            User user = new User("Username", "Password", 0);
            Task task = new Task(true);
            task.SetAllocatedTo(user);
            Batch testBatch = new Batch(0);
            testBatch.AddTask(task);
            scheduler.AddBatch(testBatch);
            // Assert
            Assert.Null(scheduler.AllocateTask(user));
        }

        [Fact]
        public void GetNextTask_Assigns_User()
        {
            IScheduler scheduler = new Scheduler();
            User user = new User("Username", "Password", 0);
            User anotherUser = new User("AnotherUser", "Password", 0);
            Task task = new Task(true);
            Task taskOne = new Task(true);
            Batch testBatch = new Batch(0);
            testBatch.AddTask(task);
            testBatch.AddTask(taskOne);
            scheduler.AddBatch(testBatch);
            scheduler.AllocateTask(user);
            // Assert
            Assert.Equal(1, scheduler.AllocateTask(anotherUser).Number);
        }

        [Fact]
        public void GetNextTask_Another_User_Already_Assigned_Returns_Null()
        {
            IScheduler scheduler = new Scheduler();
            User user = new User("Username", "Password", 0);
            User anotherUser = new User("AnotherUser", "Password", 0);
            Task task = new Task(true);
            task.SetAllocatedTo(anotherUser);
            Batch testBatch = new Batch(0);
            scheduler.AddBatch(testBatch);
            // Assert
            Assert.Null(scheduler.AllocateTask(user));
        }

        [Fact]
        public void Remove_Completed_Task_Task_Provided()
        {
            IScheduler scheduler = new Scheduler();
            User user = new User("Username", "Password", 0);
            Task task = new Task(true);

            Batch testBatch = new Batch(0);
            testBatch.AddTask(task);
            scheduler.AddBatch(testBatch);
            // Act
            testBatch.RemoveTask(task);
            // Assert
            Assert.Null(scheduler.AllocateTask(user));
        }

        [Fact]
        public void Remove_Completed_Task_ID_Number_SubNumber_Provided()
        {
            IScheduler scheduler = new Scheduler();
            User user = new User("Username", "Password", 0);
            Task task = new Task(true);

            Batch testBatch = new Batch(0);
            testBatch.AddTask(task);
            scheduler.AddBatch(testBatch);
            // Act
            scheduler.RemoveCompletedTask(testBatch.Id, InRangeNumber, InRangeNumber);
            // Assert
            Assert.Null(scheduler.AllocateTask(user));
        }

        [Fact]
        public void Remove_Completed_Task_Incorrect_ID_Number_SubNumber_Provided()
        {
            IScheduler scheduler = new Scheduler();
            User user = new User("Username", "Password", 0);
            Task task = new Task(true);

            Batch testBatch = new Batch(0);
            testBatch.AddTask(task);
            scheduler.AddBatch(testBatch);
            // Act
            // AS there are not 20 tasks, trying to remove number 20 will not modify the queue.
            scheduler.RemoveCompletedTask(testBatch.Id, OutOfRangeNumber, OutOfRangeNumber);
            // Assert
            Assert.IsType<Task>(scheduler.AllocateTask(user));
        }

        [Fact]
        public void Remove_Completed_Task_Remove_Batch_On_Emptying_Batch()
        {
            IScheduler scheduler = new Scheduler();
            User user = new User("Username", "Password", 0);
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
            scheduler.RemoveCompletedTask(testBatch.Id, InRangeNumber,InRangeNumber );
            // Assert
            Assert.Equal(testBatchTwo.Id, scheduler.AllocateTask(user).Id);
        }

        [Fact]
        public void Remove_Completed_Task_Only_Remove_Task_If_More_Tasks_Are_In_Batch()
        {
            IScheduler scheduler = new Scheduler();
            User user = new User("Username", "Password", 0);
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
            Assert.Equal(testBatch.Id, scheduler.AllocateTask(user).Id);
        }

        [Fact]
        public void UnAssignUserFromTasks_UnAssignsUser()
        {
            IScheduler scheduler = new Scheduler();
            User user = new User("Username", "Password", 0);
            Task task = new Task(true);
            Batch testBatch = new Batch(0);
            testBatch.AddTask(task);
            scheduler.AddBatch(testBatch);
            scheduler.AllocateTask(user);
            // Using the fact that objects are pass by reference.
            scheduler.UnAssignUserFromTask(user, testBatch.Id, task.Number, task.SubNumber);

            Assert.Null(task.AllocatedTo);
        }

        [Fact]
        public void GetTaskAndAssignUser_Assigns_User()
        {
            IScheduler scheduler = new Scheduler();
            User user = new User("Username", "Password", 0);
            Task task = new Task(true);
            Batch testBatch = new Batch(0);
            testBatch.AddTask(task);
            scheduler.AddBatch(testBatch);
            scheduler.AllocateTask(user);
            // Using the fact that objects are pass by reference.

            Assert.Equal(user.Username, task.AllocatedTo);
        }

        [Fact]
        public void PingScheduler_Ping_Set()
        {
            IScheduler scheduler = new Scheduler();
            User user = new User("Username", "Password", 0);
            Task task = new Task(true);
            Batch testBatch = new Batch(0);
            // Act
            testBatch.AddTask(task);
            scheduler.AddBatch(testBatch);
            scheduler.AllocateTask(user);
            scheduler.PingScheduler(user, DateTime.Now);

            Assert.IsType<DateTime>(scheduler.GetLastPing(user));
        }

        [Fact]
        public void PingScheduler_Ping_Updated()
        {
            IScheduler scheduler = new Scheduler();
            User user = new User("Username", "Password", 0);
            Task task = new Task(true);
            Batch testBatch = new Batch(0);
            // Act
            testBatch.AddTask(task);
            scheduler.AddBatch(testBatch);
            scheduler.AllocateTask(user);
            scheduler.PingScheduler(user, DateTime.Now.Subtract(new TimeSpan(2, 0, 2, 0)));
            DateTime? lastPing = scheduler.GetLastPing(user);
            scheduler.PingScheduler(user, DateTime.Now);

            Assert.True(lastPing < scheduler.GetLastPing(user));
        }

        [Fact]
        public void FreeTasksNoLongerWorkedOn_UnAssigns_InActive_User()
        {
            IScheduler scheduler = new Scheduler();
            User user = new User("Username", "Password", 0);
            Task task = new Task(true);
            Batch testBatch = new Batch(0);
            // Act
            testBatch.AddTask(task);
            scheduler.AddBatch(testBatch);
            scheduler.AllocateTask(user);
            scheduler.PingScheduler(user, DateTime.Now.Subtract(new TimeSpan(0, 0, 5, 2)));
            scheduler.FreeTasksNoLongerWorkedOn();
            Assert.Null(task.AllocatedTo);
        }

        [Fact]
        public void FreeTasksNoLongerWorkedOn_Does_Not_UnAssign_Active_User()
        {
            IScheduler scheduler = new Scheduler();
            User user = new User("Username", "Password", 0);
            Task task = new Task(true);
            Batch testBatch = new Batch(0);
            // Act
            testBatch.AddTask(task);
            scheduler.AddBatch(testBatch);
            scheduler.AllocateTask(user);
            scheduler.PingScheduler(user, DateTime.Now.Subtract(new TimeSpan(0, 0, 2, 0)));
            scheduler.FreeTasksNoLongerWorkedOn();
            Assert.Equal(user.Username,task.AllocatedTo);
        }
    }
}
