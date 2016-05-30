﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HarmonyWebApp.Models
{
    public class UserInfoViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }

        public bool Student { get; set; }

        public bool FullTimeStudies { get; set; }

        public string DepartmentName { get; set; }

        public string FieldOfStudyName { get; set; }
    }
}