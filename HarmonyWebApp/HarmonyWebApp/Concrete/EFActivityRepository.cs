using System;
using System.Collections.Generic;
using System.Data.Entity;
using HarmonyWebApp.Abstract;
using HarmonyWebApp.Entities;
using HarmonyWebApp.Models;
using System.Linq;
using AutoMapper;

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

        public IEnumerable<ActivityViewModel> ActivitiesViewInfo
        {
            get
            {
                using (var context = new ApplicationDbContext())
                {
                    var activitiesInfo = from e in context.Activities.ToList()
                        join f in context.FieldsOfStudy.ToList()
                            on e.FieldOfStudyId equals f.Id into eGroup
                        from f in eGroup.DefaultIfEmpty()
                        join d in context.Departments.ToList()
                            on e.DepartmentId equals d.Id into eGroup2
                        from d in eGroup2.DefaultIfEmpty()
                        select new ActivityViewModel()
                        {
                            Id = e.Id,
                            Name = e.Name,
                            Code = e.Code,
                            Description = e.Description,
                            StartDate = e.StartDate,
                            EndDate = e.EndDate,
                            Every_x_Days = e.Every_x_Days,
                            FreeWeekends = e.FreeWeekends,
                            Instructor = e.Instructor,
                            Place = e.Place,
                            CourseForm = e.CourseForm,
                            NumberOfSeats = e.NumberOfSeats,
                            SeatsOccupied = e.SeatsOccupied,
                            Ects = e.Ects,
                            DepartmentName = d == null ? "Wszystkie wydziały" : d.DepartmentName,
                            FieldOfStudyName = f == null ? "Wszystkie kierunki" : f.FieldOfStudyName
                        };

                    return activitiesInfo.ToList();
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


        public void DeleteUserFromAllActivities(string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                var userActivities = context.UsersWithActivities.Where(u => u.UserId == userId).ToList();

              
                foreach (var item in userActivities)
                {
                    var activity = Activities.Single(x => x.Id == item.ActivityId);
                    activity.SeatsOccupied -= 1;
                    SaveUpdatedActivity(activity);

                    context.UsersWithActivities.Attach(item);
                    context.UsersWithActivities.Remove(item);

                }
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