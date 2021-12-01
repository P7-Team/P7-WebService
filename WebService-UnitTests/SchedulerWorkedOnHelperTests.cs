using System;
using WebService;
using WebService.Helper;
using WebService.Interfaces;
using WebService.Models;
using WebService.Services;
using Xunit;

namespace WebService_UnitTests
{
    public class SchedulerWorkedOnHelperTests
    {
        private const long UnknownID = 500;
        private const int UnknownNumber = 500;
        private const int UnknownSubNumber = 500;

        [Fact]
        public void UpdateLastPing_Ping_Set()
        {
            ISchedulerWorkedOnHelper sh = new SchedulerWorkedOnHelper();
            User user = new User("Username",  "Password", 0);
            Task task = new Task(true);
            // Act
            TaskWrapper taskWrapper = new TaskWrapper(task);
            sh.AddToWorkedOn(taskWrapper, user);
            DateTime dateTime = DateTime.Now;
            sh.UpdateLastPing(user, dateTime);

            Assert.IsType<DateTime>(sh.GetCurrentlyWorkedOn(user).LastPing);
            Assert.Equal(dateTime, sh.GetCurrentlyWorkedOn(user).LastPing);
        }

        [Fact]
        public void UpdateLastPing_Ping_Updated()
        {
            ISchedulerWorkedOnHelper sh = new SchedulerWorkedOnHelper();
            User user = new User("Username",  "Password", 0);
            Task task = new Task(true);
            Batch testBatch = new Batch(0);
            // Act
            testBatch.AddTask(task);
            TaskWrapper taskWrapper = new TaskWrapper(task);
            sh.AddToWorkedOn(taskWrapper, user);

            sh.UpdateLastPing(user, DateTime.Now.Subtract(new TimeSpan(2, 0, 20, 0)));
            DateTime lastPing = sh.GetCurrentlyWorkedOn(user).LastPing;
            sh.UpdateLastPing(user, DateTime.Now);
            DateTime currentPing = sh.GetCurrentlyWorkedOn(user).LastPing;
            Assert.True(lastPing < currentPing);
        }

        [Fact]
        public void CleanInactiveUsers_UnAssigns_InActive_User()
        {
            ISchedulerWorkedOnHelper sh = new SchedulerWorkedOnHelper();
            User user = new User("Username",  "Password", 0);
            Task task = new Task(true);
            Batch testBatch = new Batch(0);
            // Act
            testBatch.AddTask(task);
            TaskWrapper taskWrapper = new TaskWrapper(task);
            sh.AddToWorkedOn(taskWrapper, user);
            sh.UpdateLastPing(user, DateTime.Now.Subtract(new TimeSpan(0, 0, 5, 2)));
            sh.CleanInactiveUsers();
            Assert.Null(sh.GetCurrentlyWorkedOn(user));
        }

        [Fact]
        public void CleanInactiveUsers_Does_Not_UnAssign_Active_User()
        {
            ISchedulerWorkedOnHelper sh = new SchedulerWorkedOnHelper();
            User user = new User("Username",  "Password", 0);
            Task task = new Task(true);
            // Act
            TaskWrapper taskWrapper = new TaskWrapper(task);
            sh.AddToWorkedOn(taskWrapper, user);
            sh.UpdateLastPing(user, DateTime.Now.Subtract(new TimeSpan(0, 0, 2, 2)));
            Assert.Equal(user.Username, sh.GetCurrentlyWorkedOn(user).user.Username);
        }

        [Fact]
        public void IsWorkedOn_User_Assigned_Returns_True()
        {
            ISchedulerWorkedOnHelper sh = new SchedulerWorkedOnHelper();
            User user = new User("Username",  "Password", 0);
            Task task = new Task(true);
            // Act
            TaskWrapper tw = new TaskWrapper(task);
            sh.AddToWorkedOn(tw, user);
            Assert.True(sh.IsWorkedOnBy(task, user));
        }

        [Fact]
        public void IsWorkedOn_User_Not_Assigned_Returns_False()
        {
            ISchedulerWorkedOnHelper sh = new SchedulerWorkedOnHelper();
            User user = new User("Username",  "Password", 0);
            User user1 = new User("AnotherUserName",  "Password", 9001);
            Task task = new Task(true);
            // Act
            TaskWrapper tw = new TaskWrapper(task);
            sh.AddToWorkedOn(tw, user1);
            Assert.False(sh.IsWorkedOnBy(task, user));
        }

        [Fact]
        public void AddWorkedOn_User_Assigned()
        {
            ISchedulerWorkedOnHelper sh = new SchedulerWorkedOnHelper();
            User user = new User("Username",  "Password", 0);
            Task task = new Task(true);
            TaskWrapper tw = new TaskWrapper(task);

            //Act
            sh.AddToWorkedOn(tw, user);
            Assert.Equal(tw, sh.GetCurrentlyWorkedOn(user));
        }

        [Fact]
        public void AddWorkedOn_AnotherUser_Assigned_User_Not_Assigned()
        {
            ISchedulerWorkedOnHelper sh = new SchedulerWorkedOnHelper();
            User user = new User("Username",  "Password", 0);
            User user2 = new User("AnotherUserName",  "Password", 9001);
            Task task = new Task(true);
            TaskWrapper tw = new TaskWrapper(task);

            //Act
            sh.AddToWorkedOn(tw, user2);
            sh.AddToWorkedOn(tw, user);

            // Assert
            Assert.NotEqual(tw, sh.GetCurrentlyWorkedOn(user));
        }

        [Fact]
        public void AddWorkedOn_AnotherUser_Assigned_AnotherUser_Stays_Assigned()
        {
            ISchedulerWorkedOnHelper sh = new SchedulerWorkedOnHelper();
            User user = new User("Username",  "Password", 0);
            User user2 = new User("AnotherUserName",  "Password", 9001);
            Task task = new Task(true);
            TaskWrapper tw = new TaskWrapper(task);

            //Act
            sh.AddToWorkedOn(tw, user2);
            sh.AddToWorkedOn(tw, user);

            // Assert
            Assert.Equal(tw.user, sh.GetCurrentlyWorkedOn(user2).user);
        }

        [Fact]
        public void GetCurrentlyWorkedOn_User_Assigned_Returns_Correct_TaskWrapper()
        {
            ISchedulerWorkedOnHelper sh = new SchedulerWorkedOnHelper();
            User user = new User("Username",  "Password", 0);
            Task task = new Task(true);
            TaskWrapper tw = new TaskWrapper(task);
            //Act
            sh.AddToWorkedOn(tw, user);
            Assert.Equal(tw, sh.GetCurrentlyWorkedOn(user));
        }

        [Fact]
        public void GetCurrentlyWorkedOn_User_Not_Assigned_Returns_Null()
        {
            ISchedulerWorkedOnHelper sh = new SchedulerWorkedOnHelper();
            User user = new User("Username",  "Password", 0);

            Assert.Null(sh.GetCurrentlyWorkedOn(user));
        }

        [Fact]
        public void PopTaskWrapper_Returns_TaskWrapper()
        {
            ISchedulerWorkedOnHelper sh = new SchedulerWorkedOnHelper();
            User user = new User("Username",  "Password", 0);
            Task task = new Task(true);
            TaskWrapper tw = new TaskWrapper(task);

            //Act
            sh.AddToWorkedOn(tw, user);

            Assert.IsType<TaskWrapper>(sh.PopTaskWrapper(tw.Task.Id, tw.Task.Number, tw.Task.SubNumber));
        }

        [Fact]
        public void PopTaskWrapper_Returns_Correct_TaskWrapper()
        {
            ISchedulerWorkedOnHelper sh = new SchedulerWorkedOnHelper();
            User user = new User("Username",  "Password", 0);
            Task task = new Task(true);
            TaskWrapper tw = new TaskWrapper(task);

            //Act
            sh.AddToWorkedOn(tw, user);

            Assert.Equal(tw, sh.PopTaskWrapper(tw.Task.Id, tw.Task.Number, tw.Task.SubNumber));
        }

        [Fact]
        public void PopTaskWrapper_Returns_Correct_Null_When_Not_Exists()
        {
            ISchedulerWorkedOnHelper sh = new SchedulerWorkedOnHelper();
            User user = new User("Username",  "Password", 0);
            Task task = new Task(true);
            TaskWrapper tw = new TaskWrapper(task);

            //Act
            sh.AddToWorkedOn(tw, user);

            Assert.Null(sh.PopTaskWrapper(UnknownID, UnknownNumber, UnknownSubNumber));
        }
    }
}