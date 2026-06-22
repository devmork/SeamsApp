using Microsoft.EntityFrameworkCore;
using SeamsApp.Models;
using SeamsApp.Models.Base;
using System;

namespace SeamsApp.Data
{
    public class SeamsDbContext : DbContext
    {
        public SeamsDbContext(DbContextOptions<SeamsDbContext> options) : base(options)
        {
        }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Officer> Officers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentApplication> StudentApplications { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
