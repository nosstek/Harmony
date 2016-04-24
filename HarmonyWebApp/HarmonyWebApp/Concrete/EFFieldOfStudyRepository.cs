using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyWebApp.Abstract;
using HarmonyWebApp.Entities;
using HarmonyWebApp.Models;

namespace HarmonyWebApp.Concrete
{
    public class EFFieldOfStudyRepository : IFieldOfStudyRepository
    {
        public IEnumerable<FieldOfStudy> FieldsOfStudy
        {
            get
            {
                using (var context = new ApplicationDbContext())
                {
                    return context.FieldsOfStudy.AsNoTracking().ToList();
                }
            }
        }



    }
}