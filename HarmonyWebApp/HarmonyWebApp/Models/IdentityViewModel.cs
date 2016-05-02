using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HarmonyWebApp.Models
{
    public class IdentityViewModel
    {
    
        public string Id { get; set; }

        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Display(Name = "Adres")]
        public string Address { get; set; }
        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }

        [Display(Name = "Miasto")]
        public string City { get; set; }

        [Display(Name = "Student")]
        public bool Student { get; set; }

        [Display(Name = "Studia stacjonarne")]
        public bool FullTimeStudies { get; set; }

        public int FieldOfStudyId { get; set; }

        public int DepartmentId { get; set; }

        public IEnumerable<SelectListItem> CoursesList { get; set; }

    }
}