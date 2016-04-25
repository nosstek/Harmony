using HarmonyWebApp.Abstract;
using HarmonyWebApp.Entities;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using HarmonyWebApp.Models;
using System;
using Microsoft.AspNet.Identity;

namespace HarmonyWebApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private IActivityRepository _repository;

        public AdminController(IActivityRepository repositry)
        {
            _repository = repositry;
        }

        // GET: Strona główna
        public ActionResult Index()
        {
            return View();
        }

        // GET: Edycja
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Activity activity = _repository.Activities.FirstOrDefault(p => p.Id == id);

            return View(activity);
        }

        // POST: Edycja
        [HttpPost]
        public ActionResult Edit(Activity activity)
        {
            if (ModelState.IsValid)
            {
                _repository.SaveActivity(activity);
                TempData["message"] = string.Format("Zapisano {0} ", activity.Name);
                return RedirectToAction("CoursesPage");
            }
            else
            {
                // błąd w wartościach danych
                return View(activity);
            }
        }

        // POST: Usuwanie
        public ActionResult Delete(int id)
        {
            var deletedActivity = _repository.DeleteActivity(id);

            if (deletedActivity != null)
            {
                TempData["message"] = string.Format("Usunięto {0}", deletedActivity.Name);
            }
            return RedirectToAction("CoursesPage");
        }

        // GET: Tworzenie
        public ActionResult Create()
        {
            return View("Edit", new Activity());
        }

        // GET: Strona z listą kursów
        public ActionResult CoursesPage(int? page, string searchString, string searchBy)
        {
            var result = _repository.Activities;

            if (searchBy == "Code")
            {
                if (!String.IsNullOrEmpty(searchString))
                {
                    result = result.Where(s => s.Code == searchString);
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(searchString))
                {
                    result = result.Where(s => s.Name.StartsWith(searchString));
                }
            }

            return View(result.ToList().ToPagedList(page ?? 1, 10));
        }
    }
}