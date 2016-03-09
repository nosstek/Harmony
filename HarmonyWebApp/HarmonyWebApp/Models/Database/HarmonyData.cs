namespace HarmonyWebApp.Models.Database
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class HarmonyData : DbContext
    {
        public HarmonyData()
            : base("name=HarmonyData")
        {
        }

        public virtual DbSet<Activity> Activity { get; set; }
        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<User_with_activities> User_with_activities { get; set; }
        public virtual DbSet<User_with_groups> User_with_groups { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activity>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Activity>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<Activity>()
                .Property(e => e.start_date)
                .HasPrecision(0);

            modelBuilder.Entity<Activity>()
                .Property(e => e.end_date)
                .HasPrecision(0);

            modelBuilder.Entity<Group>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.email)
                .IsUnicode(false);
        }
    }
}
