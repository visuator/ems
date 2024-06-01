using EducationManagementSystem.Controllers.Dtos;
using EducationManagementSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EducationManagementSystem.Services;

public class GroupService(AppDbContext dbContext)
{
    public async Task<GroupInfoDto> GetInfo(Guid groupId, CancellationToken token = default)
    {
        var group = await dbContext.Groups
            .Where(x => x.Id == groupId)
            .SingleAsync(token);
        var students = await dbContext.Students
            .Where(x => x.GroupId == groupId)
            .ToListAsync(token);
        var marks = await dbContext.Marks
            .Where(x => students
                .Select(s => s.Id)
                .Contains(x.StudentId))
            .ToListAsync(token);
        var marksDto = marks.Select(x => new MarkDto()
        {
            Id = x.Id,
            Passed = x.Passed,
            StudentId = x.StudentId
        }).ToList();
        marksDto.AddRange(students
            .Where(x => marks.All(m => m.StudentId != x.Id))
            .Select(x => new MarkDto()
            {
                StudentId = x.Id,
                Passed = false
            }));
        return new GroupInfoDto()
        {
            Name = group.Name,
            Marks = marksDto
        };
    }
}