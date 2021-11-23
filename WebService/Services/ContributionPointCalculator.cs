using System;
using WebService.Interfaces;

namespace WebService.Services
{
    public static class ContributionPointCalculator
    {
        // TODO Find ud af hvorvidt heartbeat skal udvides til at indeholde den task der arbejdes på.
        // Find ud af hvordan findes ud af hvornår et element blev startet med at arbejde på.
        // Information fra scheduler.
        //      Hvordan instansieres denne.
        //      Hvordan begræneses workloaden på scheduleren?
        private static TokenStore _tokenStore = new TokenStore();
        public static void UpdateContributionPoints(string token)
        {
            string userToUpdate = _tokenStore.Fetch(token);
            if (string.IsNullOrEmpty(userToUpdate)) return;
            // TODO insert fetch from database once it is completed.
            User user = new User(userToUpdate, 1, "")
            {
                ContributionPoints = 5
            };
        }
    }
}