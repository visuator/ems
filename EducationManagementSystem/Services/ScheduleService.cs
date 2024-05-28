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
}