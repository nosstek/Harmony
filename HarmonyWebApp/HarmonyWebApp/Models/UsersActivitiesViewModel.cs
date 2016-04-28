using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace HarmonyWebApp.Models
{
    public class UsersActivitiesViewModel
    {

        public string UserId { get; set; }
        public string FirstName  { get; set; }

        public string LastName { get; set; }
        public int CourseId  { get; set; }

        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public int NumberOfSeats { get; set; }

        public int SeatsOccupied { get; set; }

        public string CourseForm { get; set; }

        public string FieldName { get; set; }

        public string DepartmentName { get; set; }

    }
}