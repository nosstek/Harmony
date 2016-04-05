namespace HarmonyWebApp.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HarmonyWebApp.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(HarmonyWebApp.Models.ApplicationDbContext context)
        {

            // Dodawanie nowych kursów do bazy
            if (context.Activities.ToList().Count == 0)
            {
                context.Activities.Add(new Entities.Activity()
                {
                    Name = "Fizyka 1.1A",
                    Code = "E01-01a",
                    Description = "Kurs przeznaczony dla studentów I stopnia",
                    StartDate = new DateTime(2016, 2, 15, 9, 15, 0),
                    EndDate = new DateTime(2016, 6, 15, 9, 15, 0),
                    Every_x_Days = 7,
                    FreeWeekends = true
                });

                context.Activities.Add(new Entities.Activity()
                {
                    Name = "Analiza matematyczna 1.2",
                    Code = "E01-02a",
                    Description = "Kurs przeznaczony dla studentów I stopnia",
                    StartDate = new DateTime(2016, 2, 15, 11, 15, 0),
                    EndDate = new DateTime(2016, 6, 15, 11, 15, 0),
                    Every_x_Days = 7,
                    FreeWeekends = true
                });
            }

            // Dodawanie nowego u¿ytkownika
            if (!context.Users.Any(u => u.UserName == "marcin91@z.pl"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "marcin91@z.pl" };

                manager.Create(user, "MyPassword1");
            }

            // Dodawanie u¿ytkownika z uprawnieniami administratora
            if (!context.Users.Any(u => u.UserName == "adam92@xyz.com"))
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);

                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = new ApplicationUser { UserName = "adam92@xyz.com" };

                userManager.Create(user, "MyAdminPass1");
                roleManager.Create(new IdentityRole { Name = "admin" });

                userManager.AddToRole(user.Id, "admin");
            }

        }
    }
}

