using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HarmonyWebApp.Entities
{
    [Table("FieldsOfStudy")]
    public class FieldOfStudy
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FieldOfStudyName { get; set; }

        //Foreign key for Department
        [Required]
        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        //public ICollection<UserWithFieldsOfStudy> UsersWithFieldsOfStudy { get; set; }
    }
}