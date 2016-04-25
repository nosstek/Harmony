using System.Linq;
using HarmonyWebApp.Abstract;
using HarmonyWebApp.Models;

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

    }
}