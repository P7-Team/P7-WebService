using System;
using System.Linq;
using WebService.Helper;
using WebService.Interfaces;
using WebService.Models;

namespace WebService.Services
{
    public class ContributionPointCalculator : IContributionPointCalculator
    {
        private const int ContributionUpdateTime = 5;
        private const int ContributionPointsPerMinute = 50;
        private readonly ISchedulerWorkedOnHelper _schedulerWorkedOn;

        public ContributionPointCalculator(ISchedulerWorkedOnHelper schedulerWorkedOnHelper)
        {
            _schedulerWorkedOn = schedulerWorkedOnHelper;
        }

        public void CalculateContributionPoints()
        {
            _schedulerWorkedOn.WorkedOnElementsLock.EnterReadLock();
            try
            {
                foreach (TaskWrapper tWrapper in _schedulerWorkedOn.CurrentlyWorkedOn())
                {
                    DateTime time = DateTime.Now.Subtract(new TimeSpan(0, ContributionUpdateTime, 0));
                    if (tWrapper.AssignedAt < time && tWrapper.LastPing <= time)
                    {
                        tWrapper.user.ContributionPoints = tWrapper.user.ContributionPoints +=
                            ContributionPointsPerMinute * ContributionUpdateTime;
                    }
                }
            }
            finally
            {
                _schedulerWorkedOn.WorkedOnElementsLock.ExitReadLock();
            }
        }
    }
}