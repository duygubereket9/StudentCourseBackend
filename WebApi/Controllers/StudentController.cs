using Application.Students;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Infrastructure.Auth;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class StudentController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var students = await db.Students
            .AsNoTracking()
            .Select(s => new
            {
                s.Id,
                s.FirstName,
                s.LastName,
                s.BirthDate
            })
            .ToListAsync();

        return Ok(students);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var student = await db.Students.FindAsync(id);
        if (student is null) return NotFound();

        return Ok(student);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStudent(CreateStudentDto dto)
    {
        // 1. User nesnesi oluştur
        var user = new User
        {
            Email = dto.Email,
            PasswordHash = PasswordHasher.Hash(dto.Password),
            Role = UserRole.Student // Otomatik olarak Student rolü atanıyor
        };

        await db.Users.AddAsync(user);
        await db.SaveChangesAsync(); // ID üretmek için önce kaydediyoruz

        // 2. Student nesnesi oluştur (user.Id ile eşleştirerek)
        var student = new Student
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            BirthDate = dto.BirthDate,
            UserId = user.Id
        };

        await db.Students.AddAsync(student);
        await db.SaveChangesAsync();

        return Ok(new
        {
            user.Id,
            student.FirstName,
            student.LastName,
            user.Email,
            Role = user.Role.ToString()
        });
    }



    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateStudentDto dto)
    {
        var student = await db.Students.FindAsync(id);
        if (student is null) return NotFound();

        student.FirstName = dto.FirstName;
        student.LastName = dto.LastName;
        student.BirthDate = dto.BirthDate;

        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var student = await db.Students.FindAsync(id);
        if (student is null) return NotFound();

        db.Students.Remove(student);
        await db.SaveChangesAsync();

        return NoContent();
    }
}
