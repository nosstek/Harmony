using System.Linq;
using HarmonyWebApp.Abstract;
using HarmonyWebApp.Models;
using System.Collections.Generic;
using System.Data.Entity;

namespace HarmonyWebApp.Concrete
{
    public class EFUserRepository : IUserRepository
    {
        public ApplicationUser GetUserInfo(string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Users.Single(u => u.Id == userId);

            }
        }

        public UserInfoViewModel GetUserViewInfo(string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                var userInfo = from e in context.Users.Where(x => x.Id == userId).ToList()
                    join f in context.FieldsOfStudy.ToList()
                        on e.FieldOfStudyId equals f.Id
                    join d in context.Departments.ToList()
                        on f.DepartmentId equals d.Id
                    select new UserInfoViewModel()
                    {
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        Address = e.Address,
                        PostalCode = e.PostalCode,
                        City = e.City,
                        Student = e.Student,
                        FullTimeStudies = e.FullTimeStudies,
                        DepartmentName = d.DepartmentName,
                        FieldOfStudyName = f.FieldOfStudyName
                    };
                return userInfo.SingleOrDefault();
            }
        }







        public IEnumerable<IdentityViewModel> IdentitiesViewInfo
        {
            get
            {
                using (var context = new ApplicationDbContext())
                {

                    var identitiesInfo = from e in context.Users.ToList()
                        select new IdentityViewModel()
                        {
                            Id = e.Id,
                            FirstName = e.FirstName,
                            LastName = e.LastName,
                            Address = e.Address,
                            PostalCode = e.PostalCode,
                            City = e.City,
                            Student = e.Student,
                            FullTimeStudies = e.FullTimeStudies,
                            FieldOfStudyId = e.FieldOfStudyId
                        };

                    return identitiesInfo.ToList();
                }
            }
        }



        public IEnumerable<ApplicationUser> ApplicationUsers
        {
            get
            {
                using (var context = new ApplicationDbContext())
                {
                    return context.Users.ToList();
                }
            }
        }


        public void SaveUserData(IdentityViewModel identityViewModel)
        {
            using (var context = new ApplicationDbContext())
            {

                var dbEntry = context.Users.Single(m => m.Id == identityViewModel.Id);

                if (dbEntry != null)
                {

                    dbEntry.FirstName = identityViewModel.FirstName;
                    dbEntry.LastName = identityViewModel.LastName;
                    dbEntry.Address = identityViewModel.Address;
                    dbEntry.PostalCode = identityViewModel.PostalCode;
                    dbEntry.City = identityViewModel.City;
                    dbEntry.FullTimeStudies = identityViewModel.FullTimeStudies;
                    dbEntry.FieldOfStudyId = identityViewModel.FieldOfStudyId;

                }

                context.SaveChanges();
            }
        }
    }

}
