using System;
using System.Timers;
using WebService.Interfaces;

namespace WebService.Services
{
    public class Automator
    {
        private Timer _aTimer;
        private IScheduler _scheduler;
        private ISchedulerWorkedOnHelper _schedulerWorkedOnHelper;

        public Automator(int intervalsInMinutes, IScheduler scheduler,ISchedulerWorkedOnHelper schedulerWorkedOnHelper)
        {
            _scheduler = scheduler;
            _schedulerWorkedOnHelper = schedulerWorkedOnHelper;
            SetTimer(intervalsInMinutes);
        }

        private void SetTimer(int intervalsInMinutes)
        {
            // Create a timer with a 2 min interval.
            _aTimer = new Timer(1000 * 60 * intervalsInMinutes);
            // Hook up the Elapsed event for the timer. 
            _aTimer.Elapsed += (s, e) => _schedulerWorkedOnHelper.CleanInactiveUsers();
            _aTimer.AutoReset = true;
            _aTimer.Enabled = true;
        }
    }
}