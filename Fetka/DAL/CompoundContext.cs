using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Fetka.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Fetka.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Fetka.Commons;

namespace Fetka.DAL
{
    public class CompoundContext : System.Data.Entity.DbContext
    {
        public CompoundContext() : base("RequestConnection")
        {
            Database.SetInitializer(new MyDbContextInitializer());
            this.Configuration.LazyLoadingEnabled = true;
        }

        public DbSet<Compound> Compounds { get; set; }
        public DbSet<YearOrder> YearOrders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Database.SetInitializer(new MyDbContextInitializer());
            //Database.SetInitializer(new DropCreateDatabaseAlways<CompoundContext>());
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CompoundContext>());
            Database.SetInitializer(new CreateDatabaseIfNotExists<CompoundContext>());

            base.OnModelCreating(modelBuilder);
        }

    }

    public class MyDbContextInitializer : CreateDatabaseIfNotExists<CompoundContext>
    {
        protected override void Seed(CompoundContext dbContext)
        {
            // seed data

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            roleManager.Create(new IdentityRole("Admin"));
            roleManager.Create(new IdentityRole("User"));
            var admin = new ApplicationUser { UserName = "admin", Login = "admin", FirstName = "Mariusz", LastName = "Gwiazdecki" };
            string pwdAdmin = "Admin!1";

            var user = new ApplicationUser { UserName = "user", Login = "user", FirstName = "Grzegorz", LastName = "Brzęczyszczykiewicz" };
            string pwdUser = "User!1";

            userManager.Create(admin, pwdAdmin);
            userManager.Create(user, pwdUser);

            userManager.AddToRole(admin.Id, "Admin");
            userManager.AddToRole(user.Id, "User");

            base.Seed(dbContext);
        }
    }
}
