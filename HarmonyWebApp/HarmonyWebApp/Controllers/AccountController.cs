﻿using HarmonyWebApp.Abstract;
using HarmonyWebApp.Models.Database;
using HarmonyWebApp.Models.UserAccount;
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
            if(ModelState.IsValid)
            {
                using (HarmonyData db = new HarmonyData())
                {
                    var usr = db.User.Where(u => u.name == account.Login && u.email == account.Email).FirstOrDefault();
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
                        ModelState.AddModelError("", "Użytkownik o podanym loginie / adresie e-mail już istnieje");
                        return View();
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
                            return RedirectToAction("LoggedIn");
                        }
                        else if (usrGroup.Group_id == 2)
                        {
                            ModelState.AddModelError("", "Proszę zalogować się w panelu dla administratorów");
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
    }

}