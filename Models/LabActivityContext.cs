using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LaboratoryActivityAPI.Models;

namespace LaboratoryActivityAPI.Models
{
    public class LabActivityContext : IdentityDbContext
    {
        public LabActivityContext(DbContextOptions<LabActivityContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<StudentModel> Student { get; set; }

        public DbSet<GroupModel> Group { get; set; }

        public DbSet<LabModel> Lab { get; set; }

        public DbSet<AttendanceModel> Attendance { get; set; }

        public DbSet<StateModel> State { get; set; }

        public DbSet<AssignmentModel> Assignment { get; set; }

        public DbSet<SubmissionModel> Submission { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /* User */
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(c => c.Student)
                .WithOne(d => d.User);

            /* Student */
            modelBuilder.Entity<StudentModel>()
                .HasMany(c => c.Attendances)
                .WithOne(d => d.Student);
            modelBuilder.Entity<StudentModel>()
                .HasMany(c => c.Submissions)
                .WithOne(d => d.Student);

            /* Group */
            modelBuilder.Entity<GroupModel>()
                .HasMany(c => c.Students)
                .WithOne(d => d.Group);
            modelBuilder.Entity<GroupModel>()
                .HasMany(c => c.Labs)
                .WithOne(d => d.Group);

            /* Lab */
            modelBuilder.Entity<LabModel>()
                .HasMany(c => c.Attendances)
                .WithOne(d => d.Lab);
            modelBuilder.Entity<LabModel>()
                .HasOne(c => c.Assignment)
                .WithOne(d => d.Lab);

            /* State */
            modelBuilder.Entity<StateModel>()
                .HasMany(c => c.Attendances)
                .WithOne(d => d.State);
            base.OnModelCreating(modelBuilder);

            /* Assignment */
            modelBuilder.Entity<AssignmentModel>()
                .HasMany(c => c.Submissions)
                .WithOne(d => d.Assignment);
            base.OnModelCreating(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
