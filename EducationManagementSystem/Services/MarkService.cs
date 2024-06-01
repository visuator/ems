using System.Security.Cryptography;
using System.Text;
using EducationManagementSystem.Controllers.Dtos;
using EducationManagementSystem.Domain;
using EducationManagementSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EducationManagementSystem.Services;

public class MarkService(AppDbContext dbContext, IConfiguration configuration)
{
    public async Task Revoke(Guid markId, CancellationToken token = default)
    {
        var mark = await dbContext.Marks
            .Where(x => x.Id == markId)
            .SingleAsync(token);
        mark.Passed = false;
        await dbContext.SaveChangesAsync(token);
    }
    public async Task<(Guid Id, DateTime ExpiresAt)> CreateGpsSession(DateTime requestedAt, Guid lessonId, GpsPointDto dto, CancellationToken token = default)
    {
        var expiresAt = requestedAt.Add(configuration.GetSection("MarkSettings:GpsSessionExpiration").Get<TimeSpan>());
        var session = new GpsMarkSession()
        {
            LessonId = lessonId,
            Source = new()
            {
                Longitude = dto.Longitude,
                Latitude = dto.Latitude,
                PersonId = dto.PersonId
            },
            StartAt = requestedAt,
            EndAt = expiresAt
        };
        await dbContext.MarkSessions.AddAsync(session, token);
        await dbContext.SaveChangesAsync(token);
        return (session.Id, expiresAt);
    }
    public async Task MarkViaGps(DateTime requestedAt, Guid lessonId, GpsPointDto dto, CancellationToken token = default)
    {
        var session = await dbContext.MarkSessions
            .OfType<GpsMarkSession>()
            .Include(x => x.Points)
            .Where(x => x.LessonId == lessonId)
            .SingleAsync(token);
        if (requestedAt > session.EndAt)
            throw new Exception();
        session.Points.Add(new()
        {
            Longitude = dto.Longitude,
            Latitude = dto.Latitude
        });
        await dbContext.SaveChangesAsync(token);
    }
    public async Task<Guid> CreateQrCodeSession(DateTime requestedAt, Guid lessonId, Guid studentId, CancellationToken token = default)
    {
        var offset = configuration.GetSection("MarkSettings:QrCodeSessionExpiration").Get<TimeSpan>();
        var expiresAt = requestedAt.Add(offset);
        var session = new QrCodeMarkSession()
        {
            LessonId = lessonId,
            StudentId = studentId,
            StartAt = requestedAt,
            EndAt = expiresAt,
            QrCodes = []
        };
        var count = configuration.GetSection("MarkSettings:QrCodeCount").Get<int>();
        for (var i = 0; i < count; i++)
        {
            session.QrCodes.Add(new()
            {
                Content = RandomHash(),
                ExpiresAt = requestedAt.AddSeconds(offset.Seconds / (double)count * i)
            });
        }
        await dbContext.SaveChangesAsync(token);
        return session.Id;
    }
    public async Task MarkViaQr(DateTime requestedAt, QrCodeContentDto dto, CancellationToken token = default)
    {
        var session = await dbContext.MarkSessions
            .Where(x => x.Id == dto.SessionId)
            .OfType<QrCodeMarkSession>()
            .Include(x => x.QrCodes)
            .SingleAsync(token);
        if (session.Completed)
            throw new Exception();
        var current = session.QrCodes.Single(x => x.Content == dto.Content);
        if (requestedAt > current.ExpiresAt)
            throw new Exception();
        session.Completed = true;
        await dbContext.Marks.AddAsync(new()
        {
            StudentId = session.StudentId,
            LessonId = session.LessonId,
            Passed = true
        }, token);
        await dbContext.SaveChangesAsync(token);
    }
    private static string RandomHash()
    {
        Span<byte> hashBytes = stackalloc byte[32];
        RandomNumberGenerator.Fill(hashBytes);
        return Encoding.UTF8.GetString(hashBytes);
    }
    public async Task<QrCodeContentDto> GetNextQrCode(Guid sessionId, CancellationToken token)
    {
        var session = await dbContext.MarkSessions
            .Where(x => x.Id == sessionId)
            .OfType<QrCodeMarkSession>()
            .Include(x => x.QrCodes)
            .SingleAsync(token);
        var final = session.Index < session.QrCodes.Count;
        var dto = new QrCodeContentDto()
        {
            SessionId = sessionId,
            Content = final ? null : session.QrCodes[session.Index].Content,
            Final = final,
        };
        session.Index++;
        await dbContext.SaveChangesAsync(token);
        return dto;
    }
}