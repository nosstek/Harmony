using System.ComponentModel.DataAnnotations;

namespace HarmonyWebApp.Models.UserAccount
{
    public class UserRegisterInfo
    {
        //public int UserID { get; set; }

        //[Required(ErrorMessage = "Pole Imię jest wymagane")]
        //public string FirstName { get; set; }

        //[Required(ErrorMessage = "Pole Nazwisko jest wymagane")]
        //public string LastName { get; set; }

        [Required(ErrorMessage = "Pole Login jest wymagane")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Pole Email jest wymagane")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Pole Nazwa użytkownika jest wymagane")]
        //public string UserName { get; set; }

        [Required(ErrorMessage = "Pole Hasło jest wymagane")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //[Compare("Password", ErrorMessage ="Proszę potwierdzić swoje hasło")]
        //[DataType(DataType.Password)]
        //public string ConfirmPassword { get; set; }
    }
}