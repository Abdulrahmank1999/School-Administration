using Microsoft.EntityFrameworkCore;
using School_Administration.Models;
using System;

namespace School_Administration.Data
{
    public class SchoolDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                                    .HasData(
                                              new Role
                                              {
                                                 RoleId=1,
                                                 RoleName="Admin"
                                              },
                                              new Role
                                              {
                                                  RoleId = 2,
                                                  RoleName = "User"
                                              }
                                           );
        }

    }
}
