using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using userGroup_Management.Entities;

namespace userGroup_Management.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("ApplicationConnectionString")
        {
            //our strategy for db initializer
            Database.SetInitializer<ApplicationDbContext>(new CreateDatabaseIfNotExists<ApplicationDbContext>());
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<GroupRelation> GroupsRelation { get; set; }
    }
}