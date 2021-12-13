using System;
using WebService.Helper;
using WebService.Interfaces;
using WebService.Models;
using WebService.Services;
using WebService.Services.Repositories;
using Xunit;

namespace WebService_UnitTests
{
    public class ContributionPointCalculatorTests
    {
        [Fact]
        public void CalculateContributionPoints_Calculates_Points_For_Active_Jobs()
        {
            // The setup is quite large due to dependencies.
            User user = new User("Username", "password", 0);
            Task task = new Task();
            TaskWrapper taskWrapper = new TaskWrapper(task);
            ISchedulerWorkedOnHelper workedOnHelper = new SchedulerWorkedOnHelper();
            workedOnHelper.AddToWorkedOn(taskWrapper, user);
            taskWrapper.AssignedAt = DateTime.Now.Subtract(new TimeSpan(0, 15, 0));
            workedOnHelper.UpdateLastPing("Username", DateTime.Now.Subtract(new TimeSpan(0, 6, 1)));
            IContributionPointCalculator calculator = new ContributionPointCalculator(workedOnHelper);
            int oldContributionPoints = taskWrapper.user.ContributionPoints;
            // act
            calculator.CalculateContributionPoints();
            int newContributionPoints = taskWrapper.user.ContributionPoints;
            Assert.True(oldContributionPoints < newContributionPoints);
        }

        [Fact]
        public void CalculateContributionPoints_Does_Not_Calculate_Points_For_Jobs_Which_Are_Not_Old_Enough()
        {
            // The setup is quite large due to dependencies.
            User user = new User("Username", "password", 0);
            Task task = new Task();
            TaskWrapper taskWrapper = new TaskWrapper(task);
            ISchedulerWorkedOnHelper workedOnHelper = new SchedulerWorkedOnHelper();
            workedOnHelper.AddToWorkedOn(taskWrapper, user);
            workedOnHelper.UpdateLastPing("Username", DateTime.Now.Subtract(new TimeSpan(0, 1, 0)));
            IContributionPointCalculator calculator = new ContributionPointCalculator(workedOnHelper);
            int oldContributionPoints = taskWrapper.user.ContributionPoints;
            // act
            calculator.CalculateContributionPoints();
            int newContributionPoints = taskWrapper.user.ContributionPoints;
            Assert.True(oldContributionPoints == newContributionPoints);
        }
    }
}