using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LaboratoryActivityAPI.Models;

namespace LaboratoryActivityAPI.Models
{
    public class AuthenticationContext : IdentityDbContext
    {
        public AuthenticationContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        //public DbSet<ApplicationUserModel> ApplicationUserModel { get; set; }

        public DbSet<StudentModel> Student { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(c => c.Student)
                .WithOne(d => d.User);
            base.OnModelCreating(modelBuilder);
        }
    }
}
