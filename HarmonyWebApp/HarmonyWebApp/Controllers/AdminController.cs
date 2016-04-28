﻿using HarmonyWebApp.Abstract;
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
        private readonly IActivityRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IFieldOfStudyRepository _fieldOfStudyRepository;

        public AdminController(IActivityRepository repositry, IUserRepository userRepository,IDepartmentRepository departmentRepository, IFieldOfStudyRepository fieldOfStudyRepository)

        {
            _repository = repositry;
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
            _fieldOfStudyRepository = fieldOfStudyRepository;

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


        public ActionResult UsersData()
        {

            var UsersData = _userRepository.ApplicationUsers;
            return View(UsersData);
        }


        
        public ActionResult EditUsersActivities(string id)
        {

            var result = from e in _repository.UsersWithActivities.Where(x => x.UserId == id).ToList()
                join f in _userRepository.ApplicationUsers.ToList()
                    on e.UserId equals f.Id
                join d in _repository.Activities.ToList()
                    on e.ActivityId equals d.Id
               join g in _departmentRepository.Departments.ToList()
                     on d.DepartmentId equals g.Id
                join h in _fieldOfStudyRepository.FieldsOfStudy.ToList()
                     on d.FieldOfStudyId equals h.Id

                select new UsersActivitiesViewModel()
                {
                    UserId = f.Id,
                    FirstName = f.FirstName,
                    LastName = f.LastName,
                    CourseId = d.Id,
                    CourseCode = d.Code,
                    CourseName = d.Name,
                    NumberOfSeats = d.NumberOfSeats,
                    SeatsOccupied = d.SeatsOccupied,
                    CourseForm = d.CourseForm,
                    FieldName = h.FieldOfStudyName,
                    DepartmentName = g.DepartmentName
                };

            return View(result);

        }



        [HttpPost]
        public ActionResult DeleteUserFromActivity(UsersActivitiesViewModel usersActivitiesViewModel)
        {

            //    _repository.DeleteUserWithActivity(userWithActivity);
            //       activity.SeatsOccupied -= 1;
            //          _repository.SaveUpdatedActivity(activity);






            return RedirectToAction("EditUsersActivities","Admin", usersActivitiesViewModel.UserId);

        }



        [HttpGet]
        public ActionResult EditUser(string id)
        {
            IdentityViewModel identityViewModel = _userRepository.IdentitiesViewInfo.FirstOrDefault(u => u.Id == id);
           
            return View(identityViewModel);
        }


        [HttpPost]
        public ActionResult EditUser(IdentityViewModel identityViewModel)
        {
            if (ModelState.IsValid)
            {
                _userRepository.SaveUserData(identityViewModel);
                TempData["message"] = string.Format("Zapisano {0} {1} ", identityViewModel.FirstName, identityViewModel.LastName);
                return RedirectToAction("UsersData");
            }
            else
            {
                // błąd 
                return View(identityViewModel);
            }
        }

    
    }
}