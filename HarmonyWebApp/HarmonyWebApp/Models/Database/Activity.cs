namespace HarmonyWebApp.Models.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("Activity")]
    public partial class Activity
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [HiddenInput(DisplayValue =false)]
        public int id { get; set; }

        [Column(Order = 1)]
        [StringLength(255)]
        [Display(Name = "Nazwa")]
        public string name { get; set; }

        [StringLength(255)]
        [DataType(DataType.MultilineText), Display(Name="Opis")]
        public string description { get; set; }

        [Column(Order = 2, TypeName = "datetime2")]
        [Display(Name = "Start")]
        public DateTime start_date { get; set; }

        [Column(Order = 3, TypeName = "datetime2")]
        [Display(Name = "Koniec")]
        public DateTime end_date { get; set; }

        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Co ile dni")]
        public int every_x_days { get; set; }

        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Wolne weekendy")]
        public short free_weekends { get; set; }
    }
}
