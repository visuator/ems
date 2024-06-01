using EducationManagementSystem.Controllers.Dtos;
using EducationManagementSystem.Domain;
using EducationManagementSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EducationManagementSystem.Services;

public class ScheduleService(AppDbContext dbContext)
{
    public async Task Publish(Guid version, CancellationToken token = default)
    {
        var templates = await dbContext.Templates
            .Where(x => x.Version == version)
            .ToListAsync(cancellationToken: token);
        foreach (var template in templates)
        {
            template.Published = true;
        }
        await dbContext.SaveChangesAsync(token);
    }
    public async Task Replace(Guid lessonId, ReplaceLessonDto dto, CancellationToken token = default)
    {
        var source = await dbContext.Lessons
            .Where(x => x.Id == lessonId)
            .SingleAsync(cancellationToken: token);
        var lesson = new Lesson()
        {
            GroupId = source.GroupId,
            ClassroomId = dto.ClassroomId,
            SubjectId = dto.SubjectId,
            LecturerId = dto.LecturerId
        };
        await dbContext.Lessons.AddAsync(lesson, token);
        await dbContext.SaveChangesAsync(token);
    }
    public async Task<List<LessonInfoDto>> GetLatest(DateTime requestedAt, CancellationToken token = default)
    {
        var lessons = await dbContext.Lessons
            .Where(x => requestedAt.Date == x.StartAt.Date)
            .GroupBy(x => x.StartAt)
            .Select(x => x
                .OrderByDescending(c => c.CreatedAt)
                .First())
            .Select(x => new LessonInfoDto()
            {
                ClassroomId = x.ClassroomId,
                SubjectId = x.SubjectId,
                GroupId = x.GroupId,
                LecturerId = x.LecturerId,
                StartAt = x.StartAt,
                EndAt = x.EndAt,
                Flow = x.Flow == null
                    ? null
                    : new LessonFlowDto()
                    {
                        Theme = x.Flow.Theme,
                        Resources = x.Flow.Resources
                    }
            })
            .ToListAsync(token);
        return lessons;
    }
}