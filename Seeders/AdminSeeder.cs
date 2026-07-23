using Microsoft.AspNetCore.Identity;
using SeamsApp.Data;
using SeamsApp.Models;

namespace SeamsApp.Seeders
{
    public static class AdminSeeder
    {
        public static void SeedAdmin(IServiceProvider serviceProvider)
        {
            var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SeamsDbContext>(); 
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();

            if (!context.Users.Any(u => u.Role == "Admin"))
            {
                var adminUser = new User
                {
                    Email = "admin@seams.local",
                    Role = "Admin",
                    IsActive = 1,
                    CreatedAt = DateTime.UtcNow
                };

                adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin123");

                context.Users.Add(adminUser);
                context.SaveChanges();

                var admin = new Admin
                {
                    UserId = adminUser.UserId,
                    CreatedAt = DateTime.UtcNow
                };

                context.Admins.Add(admin);
                context.SaveChanges();
            }
        }
    }
}
