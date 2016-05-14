using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using ODataDemo.Models;

namespace OdataDemo.DataAccess
{
    public class OdataDemoDbContext : DbContext
    {
        public OdataDemoDbContext()
            :base("OdataDemoConnection")
        {
            Database.SetInitializer(new OdataDemoDBInitializer());
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           modelBuilder.Entity<Person>().HasMany(p => p.Friends).WithMany().Map(
               m =>
               {
                   m.ToTable("Friends");
                   m.MapLeftKey("User1ID");
                   m.MapRightKey("User2ID");

               }
            );
            modelBuilder.Entity<Person>().HasMany(p => p.Devices).WithRequired(d => d.Person).WillCascadeOnDelete(true);
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Device> Devices { get; set; }
    }
}
