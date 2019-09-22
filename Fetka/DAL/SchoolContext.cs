using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Fetka.DAL
{
    public class SchoolContext : System.Data.Entity.DbContext
    {
        public SchoolContext() : base ("DefaultConnection")
        {

        }

        public DbSet<Models.Student> Students { get; set; }
        public DbSet<Models.Teacher> Teachers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}
