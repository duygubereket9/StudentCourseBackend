using Application.Enrollments;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Student")]
public class EnrollmentController(AppDbContext db) : ControllerBase
{
    // Derslere kaydol
    [HttpPost]
    public async Task<IActionResult> Enroll(EnrollRequestDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userId, out var guid))
            return Unauthorized();

        // JWT'den gelen User.Id → ona bağlı Student ID'yi bul
        var student = await db.Students
            .FirstOrDefaultAsync(s => s.UserId == guid);

        if (student is null)
            return BadRequest("Öğrenci kaydı bulunamadı.");

        var exists = await db.Enrollments.AnyAsync(e =>
            e.StudentId == student.Id && e.CourseId == dto.CourseId);

        if (exists)
            return BadRequest("Bu derse zaten kayıtlısınız.");

        var enrollment = new Enrollment
        {
            StudentId = student.Id,
            CourseId = dto.CourseId,
            EnrolledAt = DateTime.UtcNow
        };

        db.Enrollments.Add(enrollment);
        await db.SaveChangesAsync();

        return Ok("Derse kayıt başarılı.");
    }

    // Dersten kaydını sil
    [HttpDelete]
    public async Task<IActionResult> Unenroll([FromQuery] Guid courseId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userId, out var guid))
            return Unauthorized();

        var student = await db.Students
            .FirstOrDefaultAsync(s => s.UserId == guid);

        if (student is null)
            return BadRequest("Öğrenci bulunamadı.");

        var enrollment = await db.Enrollments
            .FirstOrDefaultAsync(e => e.StudentId == student.Id && e.CourseId == courseId);

        if (enrollment is null)
            return NotFound("Kayıt bulunamadı.");

        db.Enrollments.Remove(enrollment);
        await db.SaveChangesAsync();

        return Ok("Kayıt silindi.");
    }

    // Kendi kayıtlı olduğu dersleri listele
    [HttpGet("my")]
    public async Task<IActionResult> GetMyEnrollments()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userId, out var guid))
            return Unauthorized();

        var student = await db.Students
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Course)
            .FirstOrDefaultAsync(s => s.UserId == guid);

        if (student is null)
            return BadRequest("Öğrenci bulunamadı.");

        var result = student.Enrollments.Select(e => new
        {
            e.CourseId,
            e.Course.Name,
            e.Course.Description,
            e.EnrolledAt
        });

        return Ok(result);
    }
}
