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
        public ActionResult Index(int? page)
        {

            var result = _repository.Activities;
            return View(result.ToList().ToPagedList(page ?? 1, 10));
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
                _repository.SaveProduct(activity);
                TempData["message"] = string.Format("Zapisano {0} ", activity.Name);
                return RedirectToAction("Index");
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
            return RedirectToAction("Index");
        }

        // GET: Tworzenie
        public ActionResult Create()
        {
            return View("Edit", new Activity());
        }
    }
}