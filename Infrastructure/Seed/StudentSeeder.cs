using Domain.Entities;
using Infrastructure.Auth;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructure.Persistence.Seeders;

public static class StudentSeeder
{
    public static async Task SeedTestStudentAsync(AppDbContext db)
    {
        var email = "student@example.com";
        var exists = await db.Users.AnyAsync(u => u.Email == email);

        if (exists) return;

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = PasswordHasher.Hash("123456"),
            Role = UserRole.Student
        };

        var student = new Student
        {
            Id = Guid.NewGuid(),
            FirstName = "Duygu",
            LastName = "Bereket",
            BirthDate = new DateTime(2001, 7, 1),
            UserId = user.Id
        };

        db.Users.Add(user);
        db.Students.Add(student);

        await db.SaveChangesAsync();
    }
}
