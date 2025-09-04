using Application.Students;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public async Task<IActionResult> Create(CreateStudentDto dto)
    {
        var student = new Student
        {
            Id = Guid.NewGuid(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            BirthDate = dto.BirthDate
        };

        db.Students.Add(student);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = student.Id }, student);
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
