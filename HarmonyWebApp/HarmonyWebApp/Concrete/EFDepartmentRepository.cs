using System;
using System.Collections.Generic;
using HarmonyWebApp.Abstract;
using HarmonyWebApp.Entities;
using HarmonyWebApp.Models;

namespace HarmonyWebApp.Concrete
{
    public class EFDepartmentRepository : IDepartmentRepository
    {

        private ApplicationDbContext context = new ApplicationDbContext();

        public IEnumerable<Department> Departments
        {
            get
            {
                return context.Departments;
            }
        }
    }
}