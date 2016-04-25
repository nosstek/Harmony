using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HarmonyWebApp.Entities
{
    [Table("UsersWithFieldsOfStudy")]
    public class UserWithFieldsOfStudy
    {
        [Key]
        public int Id { get; set; }

        [StringLength(128)]
        [Required]
        public string UserId { get; set; }

        //Foreign key for FieldOfStudy
        [Required]
        public int FieldOfStudyId { get; set; }

        public FieldOfStudy FieldOfStudy { get; set; }
    }
}