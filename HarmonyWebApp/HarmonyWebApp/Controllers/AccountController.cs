using HarmonyWebApp.Abstract;
using HarmonyWebApp.Models.Database;
using HarmonyWebApp.Models.UserAccount;
using System;
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
        public ActionResult Register(UserRegisterInfo account)
        {
            if (ModelState.IsValid)
            {
                using (HarmonyData db = new HarmonyData())
                {
                    var usr = db.User.Where(u => u.name == account.Login || u.email == account.Email).FirstOrDefault();
                    var dataUser = db.User.ToList();
                    var dataGroup = db.User_with_groups.ToList();
                    int maxIdUser = dataUser.Max(x => x.id);
                    int maxIdGroup = dataGroup.Max(x => x.id);

                    if (usr == null)
                    {
                        User userDb = new User();
                        User_with_groups userDbGroups = new User_with_groups();

                        userDb.id = maxIdUser + 1;
                        userDb.name = account.Login;
                        userDb.password = account.Password;
                        userDb.email = account.Email;

                        userDbGroups.id = maxIdGroup + 1;
                        userDbGroups.User_id = userDb.id;
                        userDbGroups.Group_id = 1; // domyślnie - zwykły użytkownik

                        db.User.Add(userDb);
                        db.User_with_groups.Add(userDbGroups);

                        db.SaveChanges();

                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        var login_exist = db.User.Where(u => u.name == account.Login).FirstOrDefault();

                        if (login_exist == null)
                        {
                            ModelState.AddModelError("", "Użytkownik o podanym adresie e-mail już istnieje w bazie. Proszę użyć innego.");
                            return View();
                        }
                        else
                        {
                            ModelState.AddModelError("", "Użytkownik o podanym loginie już istnieje. Proszę użyć innego.");
                            return View();
                        }
                    }

                }
            }
            else
            {
                return View();
            }
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

                        if (usrGroup.Group_id == 1)
                        {
                            Session["UserId"] = usr.id.ToString();
                            Session["UserName"] = usr.name.ToString();
                            Session["UserGroup"] = usrGroup.Group_id.ToString();
                            return RedirectToAction("LoggedIn");
                        }
                        else if (usrGroup.Group_id == 2)
                        {
                            ModelState.AddModelError("", "Proszę zalogować się w panelu dla administratorów");
                        }

                    }

                    else
                    {
                        var login_exist = db.User.Where(u => u.name == user.Login).FirstOrDefault();

                        if (login_exist != null)
                        {
                            ModelState.AddModelError("", "Podano błędne hasło.");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Podano błędny login.");
                        }
                    }

                }
            }

            return View();
        }

        public ActionResult LoggedIn()
        {
            if (Session["UserId"] != null)
            {
                var activityList = _activityRepo.Activities.ToList();

                return View(activityList);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        public ActionResult ActivityJoin()
        {
            ViewBag.ActivityData = new SelectList(_activityRepo.Activities, "id", "name");

            return View();
        }


        [HttpPost]
        public ActionResult ActivityJoin(string ActivityData)
        {
            if(ActivityData != null)
            {
                using (HarmonyData db = new HarmonyData())
                {
                    User_with_activities userWithActiv = new User_with_activities();

                    var dataUserActivity = db.User_with_activities.ToList();
                    int maxIdUserWithActivity;

                    if (dataUserActivity.Count == 0)
                    {
                        maxIdUserWithActivity = 1;
                    }
                    else
                    {
                        maxIdUserWithActivity = dataUserActivity.Max(x => x.id) + 1;
                    }

                    userWithActiv.id = maxIdUserWithActivity;
                    userWithActiv.User_id = int.Parse(Session["UserId"].ToString());
                    userWithActiv.Activity_id = int.Parse(ActivityData);

                    db.User_with_activities.Add(userWithActiv);
                    db.SaveChanges();

                    return RedirectToAction("LoggedIn", "Account");
                }
            }
            else
            {
                ModelState.AddModelError("", "Nie ma takiej grupy");
                return View();
            }
        }


    }
}