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
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [StringLength(8)]
        [Required]
        [Display(Name = "Kod")]
        public string Code { get; set; }

        [StringLength(255)]
        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Rozpoczęcie zajęć")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Zakończenie zajęć")]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Częstość (Co ile dni)")]
        public int Every_x_Days { get; set; }

        [Required]
        [Display(Name = "Studia stacjonarne")]
        public bool FreeWeekends { get; set; }

        [StringLength(50)]
        [Display(Name = "Prowadzący")]
        public string Instructor { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Miejsce")]
        public string Place { get; set; }

        [Required]
        [StringLength(1)]
        [Display(Name = "Forma kursu")]
        public string CourseForm { get; set; }

        [Required]
        [Display(Name = "Ilość miejsc")]
        public int NumberOfSeats { get; set; }

        [Required]
        [Display(Name = "Ilość zajętych miejsc")]
        public int SeatsOccupied { get; set; }

        [Required]
        [Display(Name = "Ilość pktów Ects")]
        public int Ects { get; set; }

        [Display(Name = "Id Wydziału")]
        public int DepartmentId { get; set; }

        [Required]
        [Display(Name = "Id Kierunku")]
        public int FieldOfStudyId { get; set; }

        public ICollection<UserWithActivities> UsersWithActivities { get; set; }
    }
}