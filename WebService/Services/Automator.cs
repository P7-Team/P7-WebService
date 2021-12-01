using System;
using System.Threading;
using WebService.Interfaces;
using Timer = System.Timers.Timer;

namespace WebService.Services
{
    public class Automator
    {
        private ISchedulerWorkedOnHelper _schedulerWorkedOnHelper;
        private IContributionPointCalculator _cp;
        private Thread _cleanInactiveUsers;
        private Thread _contributionPointsManager;
        private int _intervalInMinutes;

        public Automator(int intervalInMinutes, ISchedulerWorkedOnHelper schedulerWorkedOnHelper,
            IContributionPointCalculator cp)
        {
            _intervalInMinutes = intervalInMinutes;

            _schedulerWorkedOnHelper = schedulerWorkedOnHelper;
            _cp = cp;

            _cleanInactiveUsers = new Thread(CleanInactiveUsers);
            _contributionPointsManager = new Thread(ContributionPointsHandler);
            _cleanInactiveUsers.Start();
            _contributionPointsManager.Start();
        }

        private void CleanInactiveUsers()
        {
            // Create a timer with a 2 min interval.
            Timer aTimer = new Timer(1000 * 60 * _intervalInMinutes);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += (s, e) => _schedulerWorkedOnHelper.CleanInactiveUsers();
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void ContributionPointsHandler()
        {
            // Create a timer with a 2 min interval.
            Timer aTimer = new Timer(1000 * 60 * _intervalInMinutes);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += (s, e) => _cp.CalculateContributionPoints();
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
    }
}