using HarmonyWebApp.Abstract;
using HarmonyWebApp.Entities;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using HarmonyWebApp.Models;
using System;
using System.IO;
using System.Web;
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
            var userId = User.Identity.GetUserId();
            var userInfo = _userRepository.GetUserViewInfo(userId);
            return View(userInfo);
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
        public ActionResult CoursesPage()
        {
            return View();
        }

        public ActionResult CoursesList(int? page, string searchString, string searchBy)
        {
            //var result = _repository.Activities;
            var result = _repository.ActivitiesViewInfo;

            if (searchBy == "Code")
            {
                if (!String.IsNullOrEmpty(searchString))
                {
                    result = result.Where(s => s.Code.Contains(searchString));
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(searchString))
                {
                    result = result.Where(s => s.Name.Contains(searchString));
                }
            }

            return PartialView(result.ToList().ToPagedList(page ?? 1, 10));
        }


        public ActionResult UsersData()
        {

            var UsersData = _userRepository.UsersInfoViewModels();

            return View(UsersData);
        }


        [HttpGet]
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
            var activity = _repository.Activities.Single(x => x.Id == usersActivitiesViewModel.CourseId);
            var userWithActivity = _repository.UsersWithActivities.Single(x => x.ActivityId == usersActivitiesViewModel.CourseId && x.UserId == usersActivitiesViewModel.UserId);

            _repository.DeleteUserWithActivity(userWithActivity);
            activity.SeatsOccupied -= 1;
            _repository.SaveUpdatedActivity(activity);

            TempData["message"] = $"Wypisano użytkownika {usersActivitiesViewModel.FirstName} {usersActivitiesViewModel.LastName} z kursu {activity.Name}";
            
            return RedirectToAction("EditUsersActivities", "Admin", new { id=usersActivitiesViewModel.UserId});

        }



        [HttpGet]
        public ActionResult EditUser(string id)
        {

            var fieldOfStudyList = from e in _fieldOfStudyRepository.FieldsOfStudy.ToList()
                                   join d in _departmentRepository.Departments.ToList()
                                       on e.DepartmentId equals d.Id
                                   select new
                                   {
                                       FieldOfStudyId = e.Id,
                                       InfoToDisplay = $"{d.DepartmentName}, Kierunek: {e.FieldOfStudyName}"
                                   };


            IdentityViewModel identityViewModel = _userRepository.IdentitiesViewInfo.FirstOrDefault(u => u.Id == id);

            identityViewModel.DepartmentId = _fieldOfStudyRepository.GetDepartmentId(identityViewModel.FieldOfStudyId);

            identityViewModel.CoursesList = new SelectList(fieldOfStudyList, "FieldOfStudyId", "InfoToDisplay");

            return View(identityViewModel);
        }


        [HttpPost]
        public ActionResult EditUser(IdentityViewModel identityViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = identityViewModel.Id;
                var fieldOfStudyOld = _userRepository.ApplicationUsers.Single(u => u.Id == userId).FieldOfStudyId;

                if (identityViewModel.FieldOfStudyId != fieldOfStudyOld)
                {
                    _repository.DeleteUserFromAllActivities(userId);

                }
               
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

        [HttpGet]
        public ActionResult PictureUpload()
        {
            return View("PictureUploadView");
        }



        [HttpPost]
        public ActionResult PictureUpload(IdentityViewModel model)
        {


            var userId = User.Identity.GetUserId();
            model = _userRepository.GetIdentityViewModel(userId);

            HttpPostedFileBase file;
            if (Request.Files.Count > 0)
            {
                file = Request.Files[0];

                if (file != null && file.ContentLength > 0 && ModelState.IsValid)
                {
                    using (Stream stream = file.InputStream)
                    {
                        MemoryStream memoryStream = stream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            stream.CopyTo(memoryStream);
                        }

                        model.ImageBytes = memoryStream.ToArray();
                        memoryStream.Dispose();
                    }


                    _userRepository.SaveUserData(model);


                }
            }
            else
            {
                ModelState.AddModelError("Files", "Files Are empty");
                return View("PictureUploadView", model);
            }

            return View("PictureUploadView");
        }

    }
}