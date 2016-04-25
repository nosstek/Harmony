using HarmonyWebApp.Models;

namespace HarmonyWebApp.Abstract
{
    public interface IUserRepository
    {
        ApplicationUser GetUserInfo(string userId);
        UserInfoViewModel GetUserViewInfo(string userId);
    }
}
