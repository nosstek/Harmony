using HarmonyWebApp.Abstract;
using HarmonyWebApp.Entities;
using HarmonyWebApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace HarmonyWebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IActivityRepository _repository;

        public HomeController(IActivityRepository repository)
        {
            _repository = repository;
        }

        // GET: Home
        public ActionResult Index()
        {
            var activitiesList = _repository.Activities;

            return View(activitiesList);
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
                    OtherInfo = string.Format("{0} -- {1}", s.Code, s.Name)
                });

            ViewBag.ActivityData = new SelectList(activityList, "ActivityId", "OtherInfo");

            return View();
        }

        // POST: Zapisy na zajęcia
        [HttpPost]
        public ActionResult ActivityJoin(string ActivityData)
        {
            if (ActivityData != null)
            { 
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var userId = User.Identity.GetUserId();
                    var activityId = int.Parse(ActivityData);

                    var disableJoin = _repository.UsersWithActivities.Any(x => x.ActivityId == activityId && x.UserId == userId);

                    if (disableJoin == false)
                    {
                        db.UsersWithActivities.Add(new UserWithActivities() { UserId = userId, ActivityId = activityId });
                        db.SaveChanges();
                        TempData["message"] = string.Format("Zapisano pomyślnie do wybranej grupy zajęciowej");
                    }
                    else
                    {
                        TempData["message2"] = string.Format("Nie można zapisać się ponownie do tej samej grupy");
                    }
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
        public ActionResult ActivityJoinByCode(string searchString)
        {
            if (searchString != null)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var userId = User.Identity.GetUserId();
                    var activityCode = searchString;

                    var result = _repository.Activities.Any(x => x.Code == activityCode);

                    if (result != false)
                    {
                        var checkActivityName = _repository.Activities.Where(x => x.Code == activityCode).Single();
                        var disableJoin = _repository.UsersWithActivities.Any(x => x.ActivityId == checkActivityName.Id && x.UserId == userId);

                        if (disableJoin == false)
                        {

                            var activity = _repository.Activities.Where(x => x.Code == activityCode).Single();
                            var activityId = activity.Id;

                            db.UsersWithActivities.Add(new UserWithActivities() { UserId = userId, ActivityId = activityId });
                            db.SaveChanges();
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
            ApplicationDbContext db = new ApplicationDbContext();

            var userId = User.Identity.GetUserId();

            var result = from e in db.UsersWithActivities.ToList()
                         join d in db.Activities.ToList()
                         on e.ActivityId equals d.Id
                         where e.UserId == userId
                         select d;

            return View(result.ToList().ToPagedList(page ?? 1, 10));
        }

        // POST: Wypisanie użytkownika z kursu
        [HttpPost]
        public ActionResult UnsubscribeCourse(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var userId = User.Identity.GetUserId();

                var userWithActivity = db.UsersWithActivities.Where(x => x.ActivityId == id && x.UserId == userId).First();

                var activityDetail = _repository.Activities.Where(x => x.Id == id).Single();

                if (userWithActivity != null)
                {
                    db.UsersWithActivities.Remove(userWithActivity);
                    db.SaveChanges();
                    TempData["message"] = string.Format("Wypisano z {0}", activityDetail.Name);
                }
            }

            return RedirectToAction("UserCourses");
        }

        // GET: Przeglądanie kursów
        public ActionResult OverviewCourses(string searchString, string searchBy, string sort, int? page)
        {
            var movies = _repository.Activities;

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

