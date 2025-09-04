using Domain.Entities;
using Infrastructure.Auth;
using Infrastructure.Persistence;

public static class UserSeeder
{
    public static async Task SeedAdminAsync(AppDbContext db)
    {
        if (!db.Users.Any())
        {
            var admin = new User
            {
                Id = Guid.NewGuid(),
                Email = "admin@example.com",
                PasswordHash = PasswordHasher.Hash("admin123"),
                Role = UserRole.Admin
            };
            db.Users.Add(admin);
            await db.SaveChangesAsync();
        }
    }
}