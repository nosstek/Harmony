using HarmonyWebApp.Entities;
using System.Collections.Generic;

namespace HarmonyWebApp.Abstract
{
    public interface IActivityRepository
    {
        IEnumerable<Activity> Activities { get; }
        IEnumerable<UserWithActivities> UsersWithActivities { get; }

        Activity DeleteActivity(int activityId);
        void DeleteUserWithActivity(UserWithActivities userWithActivity);
        Activity GetActivityById(int activityId);
        void SaveActivity(Activity activity);
        void SaveUpdatedActivity(Activity activity);
        void SaveUserWithActivity(string userId, int activityId);
    }
}