using EducationManagementSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EducationManagementSystem.Infrastructure;

public class TimestampInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is not null) 
            Mark(eventData.Context);
        return base.SavingChanges(eventData, result);
    }
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null) 
            Mark(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
    private static void Mark(DbContext dbContext)
    {
        foreach (var entry in dbContext.ChangeTracker.Entries<BaseEntity>())
        {
            if(entry.State == EntityState.Added)
                entry.Entity.CreatedAt = DateTime.UtcNow;
            entry.Entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}