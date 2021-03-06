﻿using HarmonyWebApp.Abstract;
using HarmonyWebApp.Entities;
using HarmonyWebApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using PagedList;
using PagedList.Mvc;

namespace HarmonyWebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IActivityRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IFieldOfStudyRepository _fieldOfStudyRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public HomeController(IActivityRepository repository, IUserRepository userRepository, IFieldOfStudyRepository fieldOfStudyRepository, IDepartmentRepository departmentRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
            _fieldOfStudyRepository = fieldOfStudyRepository;
            _departmentRepository = departmentRepository;
        }

        // GET: Home
        public ActionResult Index()
        {

            var userId = User.Identity.GetUserId();
            var userInfo = _userRepository.GetUserViewInfo(userId);

            return View(userInfo);
        }

        // GET: Zapisy na zajęcia
        public ActionResult ActivityJoin()
        {
            var activityList =
                _repository.Activities
                .ToList()
                .Select(s => new
                {
                    ActivityId = s.Id,
                    OtherInfo = $"{s.Code} - {s.Name} - ({s.SeatsOccupied}/{s.NumberOfSeats})"
                });

            ViewBag.ActivityData = new SelectList(activityList, "ActivityId", "OtherInfo");

            return View();
        }

        // POST: Zapisy na zajęcia
        [HttpPost]
        public ActionResult ActivityJoin(string ActivityData)
        {
            if (!ActivityData.IsNullOrWhiteSpace())
            {
                var userId = User.Identity.GetUserId();
                var activityId = int.Parse(ActivityData);

                var checkExistingUserWithActivity =
                    _repository.UsersWithActivities.Any(x => x.ActivityId == activityId && x.UserId == userId);

                if (checkExistingUserWithActivity == false)
                {
                    var activity = _repository.GetActivityById(activityId);
                    int maxSeats = activity.NumberOfSeats;
                    int currentSeats = activity.SeatsOccupied;
                    var userInfo = _userRepository.GetUserInfo(userId);
                    var userDepartmentId = _fieldOfStudyRepository.GetDepartmentId(userInfo.FieldOfStudyId);

                    if (currentSeats == maxSeats)
                    {
                        TempData["brak_ miejsc"] = string.Format("Brak miejsc w wybranej grupie zajęciowej");
                        return RedirectToAction("ActivityJoin", "Home");
                    }

                    if ((activity.FieldOfStudyId != userInfo.FieldOfStudyId) && (activity.FieldOfStudyId != 0))
                    {
                        TempData["niepoprawny_kierunek"] =
                            string.Format("Nie możesz się zapisać na kurs przeznaczony dla innego kierunku");
                        return RedirectToAction("ActivityJoin", "Home");
                    }

                    if ((activity.DepartmentId != userDepartmentId) && (activity.DepartmentId != 0))
                    {
                        TempData["niepoprawny_wydzial"] =
                            string.Format("Nie możesz się zapisać na kurs przeznaczony dla innego wydziału");
                        return RedirectToAction("ActivityJoin", "Home");
                    }
                    
                    activity.SeatsOccupied += 1;
                    _repository.SaveUpdatedActivity(activity);
                    _repository.SaveUserWithActivity(userId, activityId);

                    TempData["message"] = string.Format("Zapisano pomyślnie do wybranej grupy zajęciowej");
                }
                else
                {
                    TempData["message2"] = string.Format("Nie można zapisać się ponownie do tej samej grupy");
                }

                return RedirectToAction("ActivityJoin", "Home");
            }
            else
            {
                TempData["message2"] = string.Format("Nie ma takiej grupy");
                return RedirectToAction("ActivityJoin", "Home");
            }
        }

        // POST: Zapisy na zajęcia - kod kursu
        [HttpPost]
        public ActionResult ActivityJoinByCode(string activityCode)
        {
            if (activityCode != null)
            {
                    var userId = User.Identity.GetUserId();

                    var checkExistingActivity = _repository.Activities.Any(x => x.Code == activityCode);

                    if (checkExistingActivity != false)
                    {
                        var activity = _repository.Activities.Single(x => x.Code == activityCode);
                        var checkExistingUserWithActivity =
                            _repository.UsersWithActivities.Any(x => x.ActivityId == activity.Id && x.UserId == userId);

                        if (checkExistingUserWithActivity == false)
                        {
                            var activityId = activity.Id;
                            int maxSeats = activity.NumberOfSeats;
                            int currentSeats = activity.SeatsOccupied;
                            var userInfo = _userRepository.GetUserInfo(userId);
                            var userDepartmentId = _fieldOfStudyRepository.GetDepartmentId(userInfo.FieldOfStudyId);

                        if (currentSeats == maxSeats)
                            {
                                TempData["brak_ miejsc"] = string.Format("Brak miejsc w wybranej grupie zajęciowej");
                                return RedirectToAction("ActivityJoin", "Home");
                            }

                            if ((activity.FieldOfStudyId != userInfo.FieldOfStudyId) && (activity.FieldOfStudyId != 0))
                            {
                                TempData["niepoprawny_kierunek"] =
                                    string.Format("Nie możesz się zapisać na kurs przeznaczony dla innego kierunku");
                                return RedirectToAction("ActivityJoin", "Home");
                            }

                            if ((activity.DepartmentId != userDepartmentId) && (activity.DepartmentId != 0))
                            {
                                TempData["niepoprawny_wydzial"] =
                                    string.Format("Nie możesz się zapisać na kurs przeznaczony dla innego wydziału");
                                return RedirectToAction("ActivityJoin", "Home");
                            }

                            activity.SeatsOccupied += 1;
                            _repository.SaveUpdatedActivity(activity);
                            _repository.SaveUserWithActivity(userId, activityId);

                        TempData["message"] = string.Format("Zapisano pomyślnie do wybranej grupy zajęciowej");
                        }
                       else
                        {
                            TempData["message2"] = string.Format("Nie można zapisać się ponownie do tej samej grupy");
                        }
                    }
                    else
                    {
                         TempData["message2"] = string.Format("Nie ma takiej grupy");
                    }
                    return RedirectToAction("ActivityJoin", "Home");
            }
            else
            {
                TempData["message2"] = string.Format("Nie ma takiej grupy");
                return RedirectToAction("ActivityJoin", "Home");
            }
        }

        // GET: Kursy użytkownika
        public ActionResult UserCourses(int? page)
        {
            var userId = User.Identity.GetUserId();
            var result = from e in _repository.UsersWithActivities.ToList()
                         join d in _repository.Activities.ToList()
                         on e.ActivityId equals d.Id
                         where e.UserId == userId
                         select d;

            return View(result.ToList().ToPagedList(page ?? 1, 10));
        }

        // POST: Wypisanie użytkownika z kursu
        [HttpPost]
        public ActionResult UnsubscribeCourse(int id)
        {
            var userId = User.Identity.GetUserId();
            var activity = _repository.Activities.Single(x => x.Id == id);
            var checkExistingUserWithActivity = _repository.UsersWithActivities.First(x => x.ActivityId == id && x.UserId == userId);

            if (checkExistingUserWithActivity != null)
            {
                var userWithActivity = _repository.UsersWithActivities.Single(x => x.ActivityId == id && x.UserId == userId);
                _repository.DeleteUserWithActivity(userWithActivity);

                activity.SeatsOccupied -= 1;
                _repository.SaveUpdatedActivity(activity);
                TempData["message"] = $"Wypisano z {activity.Name}";
            }

            return RedirectToAction("UserCourses");
        }

        // GET: Przeglądanie kursów
        public ActionResult OverviewCourses(string searchString, string searchBy, string sort, int? page)
        {
            var movies = _repository.ActivitiesViewInfo;

            if (searchBy == "Code")
            {
                if (!String.IsNullOrEmpty(searchString))
                {
                    movies = movies.Where(s => s.Code == searchString);
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(searchString))
                {
                    movies = movies.Where(s => s.Name.StartsWith(searchString));
                }
            }        
         
            // Sortowanie
            ViewBag.SortByCode = sort == "CodeDesc" ? "CodeAsc" : "CodeDesc";
            ViewBag.SortByName = sort == "NameDesc" ? "NameAsc" : "NameDesc";
            ViewBag.SortByDescription = sort == "DescriptionDesc" ? "DescriptionAsc" : "DescriptionDesc";
            ViewBag.SortByStart = sort == "StartDesc" ? "StartAsc" : "StartDesc";
            ViewBag.SortByEnd = sort == "EndDesc" ? "EndAsc" : "EndDesc";

            switch (sort)
            {
                case "CodeDesc":
                    movies = movies.OrderByDescending(x => x.Code);
                    break;
                case "CodeAsc":
                    movies = movies.OrderBy(x => x.Code);
                    break;
                case "NameDesc":
                    movies = movies.OrderByDescending(x => x.Name);
                    break;
                case "NameAsc":
                    movies = movies.OrderBy(x => x.Name);
                    break;
                case "DescriptionDesc":
                    movies = movies.OrderByDescending(x => x.Description);
                    break;
                case "DescriptionAsc":
                    movies = movies.OrderBy(x => x.Description);
                    break;
                case "StartDesc":
                    movies = movies.OrderByDescending(x => x.StartDate);
                    break;
                case "StartAsc":
                    movies = movies.OrderBy(x => x.StartDate);
                    break;
                case "EndDesc":
                    movies = movies.OrderByDescending(x => x.EndDate);
                    break;
                case "EndAsc":
                    movies = movies.OrderBy(x => x.EndDate);
                    break;
            }

            return View(movies.ToList().ToPagedList(page ?? 1, 10));
        }
    }
}

