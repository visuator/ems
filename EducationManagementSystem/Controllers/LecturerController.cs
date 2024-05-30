using System.Text.Json;
using EducationManagementSystem.Domain;
using EducationManagementSystem.Infrastructure;
using EducationManagementSystem.Jobs;
using EducationManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using Quartz;

namespace EducationManagementSystem.Controllers;

[ApiController]
[Route("lecturer")]
public class LecturerController(AppDbContext dbContext, ISchedulerFactory schedulerFactory, LessonService lessonService, MarkService markService) : ControllerBase
{
    public class FlowDto
    {
        public string Theme { get; set; } = default!;
        public string Resources { get; set; } = default!;
    }
    public class EliminateDebtDto
    {
        public Guid StudentId { get; set; }
    }
    public class GpsPointDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
    public class GpsSessionDto
    {
        public DateTime ExpiresAt { get; set; }
    }
    public class QrCodeSessionDto
    {
        public Guid Id { get; set; }
    }
    public class QrCodeDto
    {
        public byte[] Image { get; set; }
    }
    [Authorize(Roles = "lecturer")]
    [HttpDelete("debts/{lessonId:guid}")]
    public async Task<IActionResult> EliminateDebt(Guid lessonId, [FromBody] EliminateDebtDto dto, CancellationToken token = default)
    {
        await lessonService.EliminateDebt(lessonId, dto.StudentId, token);
        return Ok();
    }
    [Authorize(Roles = "lecturer")]
    [HttpPost("flow/{lessonId:guid}")]
    public async Task<IActionResult> SetFlow(Guid lessonId, FlowDto dto, CancellationToken token = default)
    {
        await lessonService.SetFlow(lessonId, new()
        {
            Theme = dto.Theme,
            Resources = dto.Resources
        }, token);
        return Ok();
    }
    [Authorize(Roles = "lecturer")]
    [HttpPost("gps")]
    public async Task<IActionResult> CreateMarkSessionViaGps([FromBody] GpsPointDto dto, CancellationToken token = default)
    {
        var lesson = await GetCurrentLesson();
        await AssertGpsSessionCreatesOnce(lesson.Id, token);
        var (id, expiresAt) = await markService.CreateGpsSession(RequestedAt, lesson.Id, new()
        {
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            PersonId = Current
        }, token);
        var scheduler = await schedulerFactory.GetScheduler(token);
        var job = JobBuilder.Create<CreateMarksViaGpsJob>()
            .WithIdentity($"{id}")
            .UsingJobData("gpsSessionId", id)
            .Build();
        var trigger = TriggerBuilder.Create()
            .StartAt(expiresAt)
            .Build();
        await scheduler.ScheduleJob(job, trigger, token);
        return Ok(new GpsSessionDto()
        {
            ExpiresAt = expiresAt
        });
    }
    [Authorize(Roles = "lecturer")]
    [HttpPost("qr/{studentId:guid}")]
    public async Task<IActionResult> CreateMarkSessionViaQrCode(Guid studentId, CancellationToken token = default)
    {
        var lesson = await GetCurrentLesson();
        await AssertQrCodeSessionCreatesOnceForStudent(lesson.Id, studentId, token);
        var id = await markService.CreateQrCodeSession(RequestedAt, lesson.Id, studentId, token);
        return Ok(new QrCodeSessionDto()
        {
            Id = id
        });
    }
    [Authorize(Roles = "lecturer")]
    [HttpGet("qr/{sessionId:guid}")]
    public async Task<IActionResult> GetNextQrCode(Guid sessionId, CancellationToken token = default)
    {
        var qrCode = await markService.GetNextQrCode(sessionId, token);
        var image = new PngByteQRCode(QRCodeGenerator.GenerateQrCode(JsonSerializer.Serialize(qrCode),
            QRCodeGenerator.ECCLevel.Q)).GetGraphic(128);
        return Ok(new QrCodeDto()
        {
            Image = image
        });
    }
    private async Task<Lesson> GetCurrentLesson() => await dbContext.Lessons
        .Where(x => RequestedAt >= x.StartAt && RequestedAt <= x.EndAt)
        .SingleAsync();
    private async Task AssertQrCodeSessionCreatesOnceForStudent(Guid lessonId, Guid studentId, CancellationToken token = default)
    {
        var count = await dbContext.Lessons
            .Include(x => x.Sessions)
            .Where(x => x.Id == lessonId)
            .Select(x => x.Sessions)
            .OfType<QrCodeMarkSession>()
            .Where(x => x.StudentId == studentId)
            .CountAsync(token);
        if (count > 1)
            throw new Exception();
    }
    private async Task AssertGpsSessionCreatesOnce(Guid lessonId, CancellationToken token = default)
    {
        var count = await dbContext.Lessons
            .Include(x => x.Sessions)
            .Where(x => x.Id == lessonId)
            .Select(x => x.Sessions)
            .OfType<GpsMarkSession>()
            .CountAsync(token);
        if (count > 0)
            throw new Exception();
    }
    private Guid Current => Guid.Parse(HttpContext.User.Claims.Single(x => x.Type == "sub").Value);
    private DateTime RequestedAt => HttpContext.Items["requestedAt"] is DateTime dt ? dt : throw new Exception();
}