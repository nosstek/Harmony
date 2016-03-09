using HarmonyWebApp.Models.Database;
using HarmonyWebApp.Models.UserLogin;
using System.Web.Mvc;

namespace HarmonyWebApp.Controllers
{
    public class HomeController : Controller
    {

        /*
        private HarmonyData _context;

        public HomeController(HarmonyData context)
        {
            _context = context;
        }*/



        [HttpGet]
        public ActionResult Login()
        {



            return View();


        }

        
        [HttpPost]
        public ActionResult Login(UserLogin userlogin)
        {



            if (ModelState.IsValid)
            {
                return View("Index", userlogin);
            }
            else
            {
                return View();
            }

        }




        public ActionResult Index()
        {

            //HarmonyData db = new HarmonyData();
            //var uzytkownik=db.User.ToList();

            return View();
        }
    }
}