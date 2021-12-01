using System;
using System.Collections.Generic;
using System.Threading;
using WebService.Interfaces;
using WebService.Models;
using Timer = System.Timers.Timer;

namespace WebService.Services
{
    public class Automator
    {
        private ISchedulerWorkedOnHelper _schedulerWorkedOnHelper;
        private IContributionPointCalculator _cp;
        private readonly IScheduler _scheduler;
        private readonly Thread _cleanInactiveUsers;
        private readonly Thread _contributionPointsManager;
        private readonly Thread _addBatchesToScheduler;
        private int _intervalInMinutes;

        public Automator(int intervalInMinutes, ISchedulerWorkedOnHelper schedulerWorkedOnHelper,
            IContributionPointCalculator cp,IScheduler scheduler)
        {
            _intervalInMinutes = intervalInMinutes;

            _schedulerWorkedOnHelper = schedulerWorkedOnHelper;
            _cp = cp;
            _scheduler = scheduler;

            _cleanInactiveUsers = new Thread(CleanInactiveUsers);
            _contributionPointsManager = new Thread(ContributionPointsHandler);
            _addBatchesToScheduler = new Thread(AddBatchesToScheduler);
            _addBatchesToScheduler.Start();
            _cleanInactiveUsers.Start();
            _contributionPointsManager.Start();
        }

        private void CleanInactiveUsers()
        {
        
            Timer aTimer = new Timer(1000 * 60 * _intervalInMinutes);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += (s, e) => _schedulerWorkedOnHelper.CleanInactiveUsers();
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void AddBatchesToScheduler()
        {
            Timer aTimer = new Timer(1000 * 60 * _intervalInMinutes);
            aTimer.Elapsed += (s, e) => FetchAndAddBatches();
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
        private void ContributionPointsHandler()
        {
      
            Timer aTimer = new Timer(1000 * 60 * _intervalInMinutes);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += (s, e) => _cp.CalculateContributionPoints();
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void FetchAndAddBatches()
        {
            List<Batch> batches = new List<Batch>();
            _scheduler.AddBatches(batches);
            
        }
    }
}