using HarmonyWebApp.Entities;
using System.Collections.Generic;

namespace HarmonyWebApp.Abstract
{
    public interface IActivityRepository
    {
        IEnumerable<Activity> Activities { get; }
        IEnumerable<UserWithActivities> UsersWithActivities { get; }

        void SaveProduct(Activity activity);
        Activity DeleteActivity(int activityId);
    }
}