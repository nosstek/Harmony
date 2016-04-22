using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HarmonyWebApp.Entities
{
    [Table("Activities")]
    public class Activity
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        [StringLength(8)]
        [Required]
        public string Code { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public int Every_x_Days { get; set; }

        [Required]
        public bool FreeWeekends { get; set; }

        [StringLength(50)]
        public string Instructor { get; set; }

        [Required]
        [StringLength(50)]
        public string Place { get; set; }

        [Required]
        [StringLength(1)]
        public string CourseForm { get; set; }

        [Required]
        public int NumberOfSeats { get; set; }

        [Required]
        public int SeatsOccupied { get; set; }

        [Required]
        public int Ects { get; set; }

        public int DepartmentId { get; set; }

        [Required]
        public int FieldOfStudyId { get; set; }

        public ICollection<UserWithActivities> UsersWithActivities { get; set; }
    }
}