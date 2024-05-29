using EducationManagementSystem.Domain;
using EducationManagementSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EducationManagementSystem.Services;

public class ScheduleService(AppDbContext dbContext)
{
    public class ReplaceDto
    {
        public Guid LecturerId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid ClassroomId { get; set; }
    }
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
    public async Task Replace(Guid lessonId, ReplaceDto dto, CancellationToken token = default)
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
}