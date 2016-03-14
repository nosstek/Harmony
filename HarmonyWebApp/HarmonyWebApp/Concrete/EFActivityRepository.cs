using System;
using System.Collections.Generic;
using HarmonyWebApp.Abstract;
using HarmonyWebApp.Models.Database;
using System.Linq;
using System.Data.Entity;

namespace HarmonyWebApp.Concrete
{
    public class EFActivityRepository : IActivityRepository
    {
        private HarmonyData context = new HarmonyData();

        public IEnumerable<Activity> Activities
        {
            get
            {
                return context.Activity;
            }
        }

        public Activity DeleteActivity(int activityId)
        {
            var dbEntry = context.Activity.Single(m => m.id == activityId);

            if (dbEntry != null)
            {
                context.Activity.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        public void SaveProduct(Activity activity)
        {
            if (activity.id == 0)
            {
                var dbActivity = context.Activity.ToList();
                activity.id = dbActivity.Max(x => x.id) + 1;

                context.Activity.Add(activity);
            }
            else
            {
                var dbEntry = context.Activity.Single(m => m.id == activity.id);

                if (dbEntry != null)
                {
                    dbEntry.name = activity.name;
                    dbEntry.description = activity.description;
                    dbEntry.start_date = activity.start_date;
                    dbEntry.end_date = activity.end_date;
                    dbEntry.every_x_days = activity.every_x_days;
                    dbEntry.free_weekends = activity.free_weekends;

                }
            }
            context.SaveChanges();
        }


    }
}