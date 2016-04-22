using System.Collections.Generic;
using HarmonyWebApp.Abstract;
using HarmonyWebApp.Entities;
using HarmonyWebApp.Models;

namespace HarmonyWebApp.Concrete
{
    public class EFFieldOfStudyRepository : IFieldOfStudyRepository
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        public IEnumerable<FieldOfStudy> FieldsOfStudy
        {
            get
            {
                return context.FieldsOfStudy;
                
            }
        }
    }
}