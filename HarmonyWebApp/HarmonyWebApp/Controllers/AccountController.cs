using HarmonyWebApp.Abstract;
using HarmonyWebApp.Models.Database;
using System.Linq;
using System.Web.Mvc;

namespace HarmonyWebApp.Controllers
{
    public class AccountController : Controller
    {
        private IActivityRepository _activityRepo;

        public AccountController(IActivityRepository activityRepo)
        {
            _activityRepo = activityRepo;
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User account)
        {
            if(ModelState.IsValid)
            {
                using (HarmonyData db = new HarmonyData())
                {
                    db.User.Add(account);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = "Użytkownik " + account.name + " pomyślnie utworzony.";
            }
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            using (HarmonyData db = new HarmonyData())
            {
                var usr = db.User.Where(u => u.name == user.name && u.password == user.password).FirstOrDefault();

                if(usr != null)
                {
                    Session["UserId"] = usr.id.ToString();
                    Session["UserName"] = usr.name.ToString();
                    return RedirectToAction("LoggedIn");
                }
                else
                {
                    ModelState.AddModelError("", "Podano błedny login lub hasło.");
                }
            }
            return View();
        }

        public ActionResult LoggedIn()
        {
            if(Session["UserId"] != null)
            {
                var activityList = _activityRepo.Activities.ToList();

                return View(activityList);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        // Panel administracyjny - dodać

    }
}