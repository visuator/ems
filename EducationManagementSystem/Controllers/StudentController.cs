using EducationManagementSystem.Domain;
using EducationManagementSystem.Infrastructure;
using EducationManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationManagementSystem.Controllers;

[ApiController]
[Route("students")]
public class StudentController(AppDbContext dbContext, MarkService markService) : ControllerBase
{
    public class GpsPointDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
    [Authorize(Roles = "student")]
    [HttpPost("qr")]
    public async Task<IActionResult> MarkViaQrCode(CancellationToken token = default)
    {
        return Ok();
    }
    [Authorize(Roles = "student")]
    [HttpPost("gps")]
    public async Task<IActionResult> MarkViaGps(GpsPointDto dto, CancellationToken token = default)
    {
        var lesson = await GetCurrentLesson();
        await markService.MarkViaGps(RequestedAt, lesson.Id, new()
        {
            Longitude = dto.Longitude,
            Latitude = dto.Latitude
        }, token);
        return Ok();
    }
    [Authorize(Roles = "student")]
    [HttpGet("schedule")]
    public async Task<IActionResult> GetLastScheduleInfo(CancellationToken token = default)
    {
        return Ok();
    }
    [Authorize(Roles = "student")]
    [HttpPost("debts")]
    public async Task<IActionResult> EliminateDebt(CancellationToken token = default)
    {
        return Ok();
    }
    private DateTime RequestedAt => HttpContext.Items["requestedAt"] is DateTime dt ? dt : throw new Exception();
    private async Task<Lesson> GetCurrentLesson() => await dbContext.Lessons
        .Where(x => RequestedAt >= x.StartAt && RequestedAt <= x.EndAt)
        .SingleAsync();
}