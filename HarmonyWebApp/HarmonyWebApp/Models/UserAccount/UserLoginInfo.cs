using System.ComponentModel.DataAnnotations;

namespace HarmonyWebApp.Models.UserAccount
{
    public class UserLoginInfo
    {
        [Required(ErrorMessage = "Proszę podać swój login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Proszę podać swoje hasło")]
        public string Password { get; set; }
    }
}