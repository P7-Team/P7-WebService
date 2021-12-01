using System;
using WebService;
using WebService.Helper;
using WebService.Interfaces;
using WebService.Models;
using WebService.Services;
using Xunit;

namespace WebService_UnitTests
{
    public class SchedulerTests
    {
        private const int InRangeNumber = 0;
        private const int OutOfRangeNumber = 20;

        [Fact]
        public void GetNextTask_Returns_Task()
        {
            IScheduler scheduler = new Scheduler(new SchedulerWorkedOnHelper(),new SchedulerHistoryHelper());
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
            IScheduler scheduler = new Scheduler(new SchedulerWorkedOnHelper(),new SchedulerHistoryHelper());
            User user = new User("Username", "Password", 0);
            Task task = new Task(true);
            Batch testBatch = new Batch(0);
            testBatch.AddTask(task);
            scheduler.AddBatch(testBatch);
            scheduler.AllocateTask(user);
            // Assert
            Assert.Null(scheduler.AllocateTask(user));
        }

        [Fact]
        public void GetNextTask_Assigns_User()
        {
            IScheduler scheduler = new Scheduler(new SchedulerWorkedOnHelper(),new SchedulerHistoryHelper());
            User user = new User("Username",  "Password", 0);
            User anotherUser = new User("AnotherUser",  "Password", 0);
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
            IScheduler scheduler = new Scheduler(new SchedulerWorkedOnHelper(),new SchedulerHistoryHelper());
            User user = new User("Username",  "Password", 0);
            User anotherUser = new User("AnotherUser",  "Password", 0);
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
            IScheduler scheduler = new Scheduler(new SchedulerWorkedOnHelper(),new SchedulerHistoryHelper());
            User user = new User("Username",  "Password", 0);
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
            IScheduler scheduler = new Scheduler(new SchedulerWorkedOnHelper(),new SchedulerHistoryHelper());
            User user = new User("Username",  "Password", 0);
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
            IScheduler scheduler = new Scheduler(new SchedulerWorkedOnHelper(),new SchedulerHistoryHelper());
            User user = new User("Username",  "Password", 0);
            Task task = new Task(true);
            Batch testBatch = new Batch(0);
            scheduler.AllocateTask(user);
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
            IScheduler scheduler = new Scheduler(new SchedulerWorkedOnHelper(),new SchedulerHistoryHelper());
            User user = new User("Username",  "Password", 0);
            Task task = new Task(true);
            Task taskOne = new Task(true);
            scheduler.AllocateTask(user);
            Batch testBatch = new Batch(0);
            Batch testBatchTwo = new Batch(1);
            testBatch.AddTask(task);
            testBatchTwo.AddTask(taskOne);
            scheduler.AddBatch(testBatch);
            scheduler.AddBatch(testBatchTwo);
            // Act
            scheduler.RemoveCompletedTask(testBatch.Id, InRangeNumber, InRangeNumber);
            // Assert
            Assert.Equal(testBatchTwo.Id, scheduler.AllocateTask(user).Id);
        }

        [Fact]
        public void Remove_Completed_Task_Only_Remove_Task_If_More_Tasks_Are_In_Batch()
        {
            IScheduler scheduler = new Scheduler(new SchedulerWorkedOnHelper(),new SchedulerHistoryHelper());
            User user = new User("Username",  "Password", 0);
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

            scheduler.RemoveCompletedTask(0, 0, 0);
            // Assert
            Assert.Equal(testBatch.Id, scheduler.AllocateTask(user).Id);
        }

        [Fact]
        public void GetTaskAndAssignUser_Assigns_User()
        {
            IScheduler scheduler = new Scheduler(new SchedulerWorkedOnHelper(),new SchedulerHistoryHelper());
            User user = new User("Username",  "Password", 0);
            Task task = new Task(true);
            Batch testBatch = new Batch(0);
            testBatch.AddTask(task);
            scheduler.AddBatch(testBatch);
            scheduler.AllocateTask(user);
            // Using the fact that objects are pass by reference.

            Assert.Equal(user.Username, task.AllocatedTo);
        }
    }
}