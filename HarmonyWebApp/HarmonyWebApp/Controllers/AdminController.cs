using HarmonyWebApp.Abstract;
using HarmonyWebApp.Models.Database;
using HarmonyWebApp.Models.UserAccount;
using System.Linq;
using System.Web.Mvc;

namespace HarmonyWebApp.Controllers
{
    public class AdminController : Controller
    {
        private IActivityRepository _repository;

        public AdminController(IActivityRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            return View(_repository.Activities);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLoginInfo user)
        {
            if (ModelState.IsValid)
            {
                using (HarmonyData db = new HarmonyData())
                {
                    var usr = db.User.Where(u => u.name == user.Login && u.password == user.Password).FirstOrDefault();

                    if (usr != null)
                    {
                        var usrGroup = db.User_with_groups.Where(u => u.User_id == usr.id).FirstOrDefault();

                        if (usrGroup.Group_id == 2)
                        {
                            Session["UserId"] = usr.id.ToString();
                            Session["UserName"] = usr.name.ToString();
                            return RedirectToAction("Index");
                        }
                        else if (usrGroup.Group_id == 1)
                        {
                            ModelState.AddModelError("", "Podany użytkownik nie posiada uprawnień administratora.");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Podano błedny login lub hasło.");
                        }
                    }                
                }
            }

            return View();
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            Activity activity = _repository.Activities.FirstOrDefault(p => p.id == id);

            return View(activity);
        }

        [HttpPost]
        public ActionResult Edit(Activity activity)
        {
            if(ModelState.IsValid)
            {
                _repository.SaveProduct(activity);
                TempData["message"] = string.Format("Zapisano {0} ", activity.name);
                return RedirectToAction("Index");
            }
            else
            {
                // błąd w wartościach danych
                return View(activity);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var deletedActivity = _repository.DeleteActivity(id);

            if(deletedActivity != null)
            {
                TempData["message"] = string.Format("Usunięto {0}", deletedActivity.name);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View("Edit", new Activity());
        }

    }
}