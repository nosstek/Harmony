using System;
using System.Collections.Generic;
using System.Data.Entity;
using HarmonyWebApp.Abstract;
using HarmonyWebApp.Entities;
using HarmonyWebApp.Models;
using System.Linq;

namespace HarmonyWebApp.Concrete
{
    public class EFActivityRepository : IActivityRepository
    {
        public IEnumerable<Activity> Activities
        {
            get
            {
                using (var context = new ApplicationDbContext())
                {
                    return context.Activities.AsNoTracking().ToList();
                }
            }
        }

        public IEnumerable<UserWithActivities> UsersWithActivities
        {
            get
            {
                using (var context = new ApplicationDbContext())
                {
                    return context.UsersWithActivities.AsNoTracking().ToList();
                }
            }
        }

        public Activity DeleteActivity(int activityId)
        {

            using (var context = new ApplicationDbContext())
            {
                var dbEntry = context.Activities.Single(m => m.Id == activityId);

                if (dbEntry != null)
                {
                    context.Activities.Remove(dbEntry);
                    context.SaveChanges();
                }
                return dbEntry;
            } 
        }

        public void DeleteUserWithActivity(UserWithActivities userWithActivity)
        {
            using (var context = new ApplicationDbContext())
            {
                context.UsersWithActivities.Attach(userWithActivity);
                context.UsersWithActivities.Remove(userWithActivity);
                context.SaveChanges();
            }
        }


        public Activity GetActivityById(int activityId)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Activities.AsNoTracking().Single(x => x.Id == activityId);
            }
        }

        public void SaveActivity(Activity activity)
        {
            using (var context = new ApplicationDbContext())
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
                        dbEntry.CourseForm = activity.CourseForm;
                        dbEntry.Instructor = activity.Instructor;
                        dbEntry.Place = activity.Place;
                        dbEntry.NumberOfSeats = activity.NumberOfSeats;
                        dbEntry.SeatsOccupied = activity.SeatsOccupied;
                        dbEntry.Ects = activity.Ects;
                        dbEntry.DepartmentId = activity.DepartmentId;
                        dbEntry.FieldOfStudyId = activity.FieldOfStudyId;

                    }
                }
                context.SaveChanges();
            }     
        }

        public void SaveUpdatedActivity(Activity activity)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(activity).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void SaveUserWithActivity(string userId, int activityId)
        {
            using (var context = new ApplicationDbContext())
            {
                context.UsersWithActivities.Add(new UserWithActivities()
                {
                    UserId = userId,
                    ActivityId = activityId
                });

                context.SaveChanges();
            }
        }


    }
}