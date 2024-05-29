using EducationManagementSystem.Domain;
using EducationManagementSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EducationManagementSystem.Services;

public class QrCodeSessionDto
{
    
}
public class GpsPointDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
public class MarkService(AppDbContext dbContext)
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
        var startAt = requestedAt;
        var expiresAt = requestedAt.AddMinutes(5);
        var session = new GpsMarkSession()
        {
            LessonId = lessonId,
            Source = new()
            {
                Longitude = dto.Longitude,
                Latitude = dto.Latitude
            },
            StartAt = startAt,
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
    public async Task CreateQrCodeSession(Guid lessonId, Guid studentId, CancellationToken token = default)
    {
    }
}