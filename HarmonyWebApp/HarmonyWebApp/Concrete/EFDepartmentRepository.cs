using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyWebApp.Abstract;
using HarmonyWebApp.Entities;
using HarmonyWebApp.Models;

namespace HarmonyWebApp.Concrete
{
    public class EFDepartmentRepository : IDepartmentRepository
    {
        public IEnumerable<Department> Departments
        {
            get
            {
                using (var context = new ApplicationDbContext())
                {
                    return context.Departments.AsNoTracking().ToList();
                }
            }
        }
    }
}