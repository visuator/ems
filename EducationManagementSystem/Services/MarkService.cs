using EducationManagementSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EducationManagementSystem.Services;

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
}