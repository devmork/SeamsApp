using SeamsApp.Models;

namespace SeamsApp.Data.Seeders
{
    public static class AdminSeeder
    {
        public static void SeedAdmin(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<SeamsDbContext>();

            //if (!context.Users.Any(u => u.Role == "Admin"))
            //{
            //    var adminUser = new User
            //    {
            //        Username = "admin",
            //        PasswordHash = new PasswordHasher<User>().HashPassword(null, "admin123"),
            //        Role = "Admin"
            //    };
            //    context.Users.Add(adminUser);
            //    context.SaveChanges();
            //}

            context.SaveChanges();
        }
    }
}
