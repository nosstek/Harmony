using System;

namespace HarmonyWebApp.Models
{
    public class ActivityViewModel
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Instructor { get; set; }

        public string Place { get; set; }

        public string CourseForm { get; set; }

        public int NumberOfSeats { get; set; }

        public int SeatsOccupied { get; set; }

        public int Ects { get; set; }

        public string DepartmentName { get; set; }

        public string FieldOfStudyName { get; set; }
    }
}