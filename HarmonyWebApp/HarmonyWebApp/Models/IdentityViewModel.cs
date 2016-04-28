using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HarmonyWebApp.Models
{
    public class IdentityViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public bool Student { get; set; }
        public bool FullTimeStudies { get; set; }
        public int FieldOfStudyId { get; set; }

    }
}