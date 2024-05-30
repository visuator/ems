using EducationManagementSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EducationManagementSystem.Services;

public class LessonService(AppDbContext dbContext)
{
    public class SetFlowDto
    {
        public string Theme { get; set; } = default!;
        public string Resources { get; set; } = default!;
    }
    public class DebtDto
    {
        public Guid LessonId { get; set; }
    }
    public async Task EliminateDebt(Guid lessonId, Guid studentId, CancellationToken token = default)
    {
        await dbContext.Marks.AddAsync(new()
        {
            LessonId = lessonId,
            StudentId = studentId,
            Passed = true
        }, token);
        await dbContext.SaveChangesAsync(token);
    }
    public async Task<List<DebtDto>> GetDebts(Guid studentId, CancellationToken token = default) =>
        await dbContext.Lessons
            .Include(x => x.Marks)
            .Where(x => x.Marks.All(m => m.StudentId != studentId))
            .Select(x => new DebtDto()
            {
                LessonId = x.Id
            })
            .ToListAsync(token);
    public async Task SetFlow(Guid lessonId, SetFlowDto dto, CancellationToken token = default)
    {
        var lesson = await dbContext.Lessons
            .Where(x => x.Id == lessonId)
            .SingleAsync(cancellationToken: token);
        lesson.Flow = new()
        {
            Theme = dto.Theme,
            Resources = dto.Resources
        };
        await dbContext.SaveChangesAsync(token);
    }
}