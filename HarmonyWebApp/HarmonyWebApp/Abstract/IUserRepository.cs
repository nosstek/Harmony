using System.Collections.Generic;
using HarmonyWebApp.Models;
using Microsoft.AspNet.Identity;

namespace HarmonyWebApp.Abstract
{
    public interface IUserRepository
    {
        ApplicationUser GetUserInfo(string userId);
        UserInfoViewModel GetUserViewInfo(string userId);
        IEnumerable<IdentityViewModel> IdentitiesViewInfo { get; }
        IEnumerable<ApplicationUser>  ApplicationUsers { get; }
        void SaveUserData(IdentityViewModel identityViewModel);

    }
}
