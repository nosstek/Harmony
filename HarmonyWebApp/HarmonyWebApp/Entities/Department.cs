using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HarmonyWebApp.Entities
{
    [Table("Departments")]
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string DepartmentName { get; set; }

        public ICollection<FieldOfStudy> FieldsOfStudy { get; set; }
    }
}