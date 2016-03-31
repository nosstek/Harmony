using System;
using System.Collections.Generic;
using HarmonyWebApp.Abstract;
using HarmonyWebApp.Entities;
using HarmonyWebApp.Models;
using System.Linq;

namespace HarmonyWebApp.Concrete
{
    public class EFActivityRepository : IActivityRepository
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        public IEnumerable<Activity> Activities
        {
            get
            {
                return context.Activities;
            }
        }

        public Activity DeleteActivity(int activityId)
        {
            var dbEntry = context.Activities.Single(m => m.Id == activityId);

            if (dbEntry != null)
            {
                context.Activities.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        public void SaveProduct(Activity activity)
        {
            if (activity.Id == 0)
            {
                var dbActivity = context.Activities.ToList();

                if (dbActivity.Count == 0)
                {
                    activity.Id = 1;
                }
                else
                {
                    activity.Id = dbActivity.Max(x => x.Id) + 1;

                }

                context.Activities.Add(activity);
            }
            else
            {
                var dbEntry = context.Activities.Single(m => m.Id == activity.Id);

                if (dbEntry != null)
                {
                    dbEntry.Name = activity.Name;
                    dbEntry.Code = activity.Code;
                    dbEntry.Description = activity.Description;
                    dbEntry.StartDate = activity.StartDate;
                    dbEntry.EndDate = activity.EndDate;
                    dbEntry.Every_x_Days = activity.Every_x_Days;
                    dbEntry.FreeWeekends = activity.FreeWeekends;

                }
            }
            context.SaveChanges();
        }
    }
}