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
        public DbSet<User> Users { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
    }
}
