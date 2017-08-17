namespace ShoppingCartNew.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using ShoppingCartNew.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using ShoppingCartNew.Models.Code_First;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
            new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            var userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(context));
            if (!context.Users.Any(u => u.Email == "rchapman@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "rchapman@coderfoundry.com",
                    Email = "rchapman@coderfoundry.com",
                    FirstName = "Ryan",
                    LastName = "Chapman",
                }, "Password1!");
            }
            var userId = userManager.FindByEmail("rchapman@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Admin");

            //if (!context.Items.Any())
            //{
            //    context.Items.AddOrUpdate(x => x.Id, 
            //        new Item()
            //        {
            //            Name = "watch1",
            //            Description = "some description",
            //            Created = DateTime.Now,
            //            Price = 100,
            //            MediaURL = "~/images/g1.jpg",
            //            Updated = null
            //        },
            //        new Item()
            //        {
            //            Name = "watch2",
            //            Description = "some more description",
            //            Created = DateTime.Now,
            //            Price = (decimal)80.95,
            //            MediaURL = "~/images/g2.jpg",
            //            Updated = null
            //        }
            //    );
            //}
        }
    }
}

