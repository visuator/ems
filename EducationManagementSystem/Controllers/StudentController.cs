using EducationManagementSystem.Controllers.Dtos;
using EducationManagementSystem.Domain;
using EducationManagementSystem.Infrastructure;
using EducationManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationManagementSystem.Controllers;

[ApiController]
[Route("students")]
public class StudentController(AppDbContext dbContext, MarkService markService, LessonService lessonService) : ControllerBase
{
    [Authorize(Roles = "student")]
    [HttpPost("qr")]
    public async Task<IActionResult> MarkViaQrCode(QrCodeContentDto dto, CancellationToken token = default)
    {
        await markService.MarkViaQr(RequestedAt, dto, token);
        return Ok();
    }
    [Authorize(Roles = "student")]
    [HttpPost("gps")]
    public async Task<IActionResult> MarkViaGps(GpsPointDto dto, CancellationToken token = default)
    {
        var lesson = await GetCurrentLesson();
        dto.PersonId = Current;
        await markService.MarkViaGps(RequestedAt, lesson.Id, dto, token);
        return Ok();
    }
    [Authorize(Roles = "student")]
    [HttpPost("debts")]
    public async Task<IActionResult> GetDebts(CancellationToken token = default)
    {
        return Ok(await lessonService.GetDebts(Current, token));
    }
    private Guid Current => Guid.Parse(HttpContext.User.Claims.Single(x => x.Type == "sub").Value);
    private DateTime RequestedAt => HttpContext.Items["requestedAt"] is DateTime dt ? dt : throw new Exception();
    private async Task<Lesson> GetCurrentLesson() => await dbContext.Lessons
        .Where(x => RequestedAt >= x.StartAt && RequestedAt <= x.EndAt)
        .SingleAsync();
}