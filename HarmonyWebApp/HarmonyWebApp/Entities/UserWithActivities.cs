using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HarmonyWebApp.Entities
{
    [Table("UsersWithActivities")]
    public class UserWithActivities
    {
        [Key]
        public int Id { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        public int ActivityId { get; set; }
    }
}