using System;
using System.Timers;

namespace WebService.Services
{
    public class Automator
    {
        private Timer _aTimer;
        private Scheduler _scheduler;

        public Automator(int intervalsInMinutes, Scheduler scheduler)
        {
            _scheduler = scheduler;
            SetTimer(intervalsInMinutes);
        }

        private void SetTimer(int intervalsInMinutes)
        {
            // Create a timer with a 2 min interval.
            _aTimer = new Timer(1000 * 60 * intervalsInMinutes);
            // Hook up the Elapsed event for the timer. 
            _aTimer.Elapsed += (s, e) => _scheduler.FreeTasksNoLongerWorkedOn();
            _aTimer.AutoReset = true;
            _aTimer.Enabled = true;
        }
    }
}