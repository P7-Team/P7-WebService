using System;
using System.Collections.Generic;
using WebService;
using WebService.Helper;
using WebService.Interfaces;
using WebService.Models;
using WebService.Services;
using WebService.Services.Repositories;
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
            IScheduler scheduler = new Scheduler(new SchedulerWorkedOnHelper(),new SchedulerHistoryHelper(), new MockTaskRepository());
            User user = new User("Username", "Password", 0);
            Task task = new Task(true);
            Batch testBatch = new Batch(0);
            // Act
            testBatch.AddTask(task);
            scheduler.AddBatch(testBatch);
            Assert.IsType<Task>(scheduler.AllocateTask(user));
        }

        [Fact]
        public void GetNextTask_Assigns_User()
        {
            IScheduler scheduler = new Scheduler(new SchedulerWorkedOnHelper(),new SchedulerHistoryHelper(), new MockTaskRepository());
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
            IScheduler scheduler = new Scheduler(new SchedulerWorkedOnHelper(),new SchedulerHistoryHelper(), new MockTaskRepository());
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
            IScheduler scheduler = new Scheduler(new SchedulerWorkedOnHelper(),new SchedulerHistoryHelper(), new MockTaskRepository());
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
            IScheduler scheduler = new Scheduler(new SchedulerWorkedOnHelper(),new SchedulerHistoryHelper(), new MockTaskRepository());
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
            IScheduler scheduler = new Scheduler(new SchedulerWorkedOnHelper(),new SchedulerHistoryHelper(), new MockTaskRepository());
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
            IScheduler scheduler = new Scheduler(new SchedulerWorkedOnHelper(),new SchedulerHistoryHelper(), new MockTaskRepository());
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
            IScheduler scheduler = new Scheduler(new SchedulerWorkedOnHelper(),new SchedulerHistoryHelper(), new MockTaskRepository());
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
            IScheduler scheduler = new Scheduler(new SchedulerWorkedOnHelper(),new SchedulerHistoryHelper(), new MockTaskRepository());
            User user = new User("Username",  "Password", 0);
            Task task = new Task(true);
            Batch testBatch = new Batch(0);
            testBatch.AddTask(task);
            scheduler.AddBatch(testBatch);
            scheduler.AllocateTask(user);
            // Using the fact that objects are pass by reference.

            Assert.Equal(user.Username, task.AllocatedTo);
        }

        [Fact]
        public void AddBatch_BatchWithAlreadyAllocatedTasks_ConsistentWithTaskRepo()
        {
            // Setup
            User user = new User("Me", "password");
            Batch batch = new Batch(1, user.Username);
            Task previouslyAllocated = new Task(1, 0, 0);
            previouslyAllocated.SetAllocatedTo(user);
            Task unallocatedTask = new Task(1, 1, 0);
            batch.Tasks.Add(previouslyAllocated);
            batch.Tasks.Add(unallocatedTask);

            ISchedulerWorkedOnHelper woh = new SchedulerWorkedOnHelper();
            IScheduler scheduler = new Scheduler(woh,new SchedulerHistoryHelper(), new MockTaskRepository());
            
            // Act
            scheduler.AddBatch(batch);
            
            // Assert
            Assert.True(woh.IsWorkedOn(previouslyAllocated));
            Assert.False(woh.IsWorkedOn(unallocatedTask));
        }

        [Fact]
        public void AllocateTask_UpdateTaskStateInDb()
        {
            // Setup
            User user = new User("Me", "password");
            User otherUser = new User("Not Me", "drowssap");
            
            Batch batch = new Batch(1, user.Username);
            
            Task previouslyAllocated = new Task(1, 0, 0);
            previouslyAllocated.SetAllocatedTo(user);
            
            Task unallocatedTask = new Task(1, 1, 0);

            batch.Tasks.Add(previouslyAllocated);
            batch.Tasks.Add(unallocatedTask);

            IRepository<Task, (int, int, int)> mockRepo = new MockTaskRepository();
            ISchedulerWorkedOnHelper woh = new SchedulerWorkedOnHelper();
            IScheduler scheduler = new Scheduler(woh,new SchedulerHistoryHelper(), mockRepo);
            
            scheduler.AddBatch(batch);
            mockRepo.Create(previouslyAllocated);
            mockRepo.Create(unallocatedTask);

            // Act
            Task result = scheduler.AllocateTask(otherUser);
            Task dbTask = mockRepo.Read(result.GetIdentifier());

            // Assert
            Assert.NotNull(dbTask);
            Assert.Equal(result.AllocatedTo, dbTask.AllocatedTo);
        }
        
        private class MockTaskRepository : IRepository<Task, (int, int, int)>
        {
            private readonly Dictionary<(int, int, int), Task> _tasks = new Dictionary<(int, int, int), Task>();

            public (int, int, int) Create(Task item)
            {
                _tasks.Add((item.Id, item.Number, item.SubNumber), item);
                return (item.Id, item.Number, item.SubNumber);
            }

            public Task Read((int, int, int) identifier)
            {
                if (_tasks.ContainsKey(identifier))
                {
                    return _tasks[identifier];
                }

                return null;
            }

            public void Update(Task item)
            {
                (int, int, int) id = (item.Id, item.Number, item.SubNumber);
                if (_tasks.ContainsKey(id))
                    _tasks[id] = item;
            }

            public void Delete((int, int, int) identifier)
            {
                throw new NotImplementedException();
            }
        }
    }
}