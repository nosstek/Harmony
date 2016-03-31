using HarmonyWebApp.Abstract;
using HarmonyWebApp.Entities;
using HarmonyWebApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Web.Mvc;

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
            if(ActivityData != null)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {

                    var userId = User.Identity.GetUserId();
                    var activityId = int.Parse(ActivityData);

                    db.UsersWithActivities.Add(new UserWithActivities() { UserId = userId, ActivityId = activityId });
                    db.SaveChanges();
                }

                    return RedirectToAction("ActivityJoin", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Nie ma takiej grupy");
                return View();
            }
            
        }



    }
}