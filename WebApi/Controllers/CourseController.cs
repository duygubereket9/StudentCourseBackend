using Application.Courses;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class CourseController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var courses = await db.Courses
            .AsNoTracking()
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.Description
            })
            .ToListAsync();

        return Ok(courses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var course = await db.Courses.FindAsync(id);
        if (course is null) return NotFound();

        return Ok(course);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCourseDto dto)
    {
        var exists = await db.Courses.AnyAsync(c => c.Name == dto.Name);
        if (exists)
            return BadRequest("Bu isimde bir ders zaten var.");

        var course = new Course
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description
        };

        db.Courses.Add(course);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = course.Id }, course);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateCourseDto dto)
    {
        var course = await db.Courses.FindAsync(id);
        if (course is null) return NotFound();

        course.Name = dto.Name;
        course.Description = dto.Description;

        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var course = await db.Courses.FindAsync(id);
        if (course is null) return NotFound();

        db.Courses.Remove(course);
        await db.SaveChangesAsync();

        return NoContent();
    }
}
