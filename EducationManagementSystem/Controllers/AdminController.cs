using EducationManagementSystem.Controllers.Dtos;
using EducationManagementSystem.Domain;
using EducationManagementSystem.Infrastructure;
using EducationManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationManagementSystem.Controllers;

[ApiController]
[Route("admin")]
public class AdminController(AppDbContext dbContext, KeycloakService keycloakService, ImportService importService, ScheduleService scheduleService, GroupService groupService, MarkService markService) : ControllerBase
{
    [Authorize("admin")]
    [HttpPost("schedule")]
    public async Task<IActionResult> PublishSchedule([FromBody] PublishScheduleDto dto, CancellationToken token = default)
    {
        await scheduleService.Publish(dto.VersionId, token);
        return Ok();
    }
    [Authorize("admin")]
    [HttpPost("replace/{lessonId:guid}")]
    public async Task<IActionResult> ReplaceLesson(Guid lessonId, ReplaceLessonDto dto,
        CancellationToken token = default)
    {
        await scheduleService.Replace(lessonId, new()
        {
            ClassroomId = dto.ClassroomId,
            SubjectId = dto.SubjectId,
            LecturerId = dto.LecturerId
        }, token);
        return Ok();
    }
    [Authorize("admin")]
    [HttpGet("groups/{groupId:guid}")]
    public async Task<IActionResult> GetGroupInfo(Guid groupId, CancellationToken token = default)
    {
        return Ok(await groupService.GetInfo(groupId, token));
    }
    [Authorize("admin")]
    [HttpDelete("marks/{markId:guid}")]
    public async Task<IActionResult> RevokeMark(Guid markId, CancellationToken token = default)
    {
        await markService.Revoke(markId, token);
        return Ok();
    }
    [Authorize(Roles = "admin")]
    [HttpPost("import")]
    public async Task<IActionResult> Import(IFormFile file, ImportType importType, CancellationToken token = default)
    {
        await using var stream = file.OpenReadStream();
        switch (importType)
        {
            case ImportType.Organization:
                var organizationData = importService.ParseOrganization(stream);
                await SaveOrganization(organizationData, token);
                break;
            case ImportType.Schedule:
                var schedule = importService.ParseSchedule(stream);
                await SaveSchedule(schedule, token);
                break;
            case ImportType.Students:
                var students = importService.ParseStudents(stream);
                await SaveStudents(students, token);
                break;
        }
        return Ok();
    }
    private async Task SaveOrganization(OrganizationData organizationData, CancellationToken token = default)
    {
        await dbContext.Classrooms.AddRangeAsync(organizationData.Classrooms.Select(x => new Classroom()
        {
            Name = x.Name
        }), token);
        await dbContext.Groups.AddRangeAsync(organizationData.Groups.Select(x => new Group()
        {
            Name = x.Name
        }), token);
        await dbContext.Periods.AddRangeAsync(organizationData.Periods.Select(x => new Period()
        {
            StartAt = x.StartAt.TimeOfDay,
            EndAt = x.EndAt.TimeOfDay
        }), token);
        await dbContext.Subjects.AddRangeAsync(organizationData.Subjects.Select(x => new Subject()
        {
            Name = x.Name
        }), token);
        var lecturers = organizationData.Lecturers.Select(x => new Lecturer()
        {
            Person = new Person()
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                MiddleName = x.MiddleName
            },
            Contact = new Contact()
            {
                Email = x.Email,
                Phone = x.Phone
            }
        }).ToList();
        await dbContext.Lecturers.AddRangeAsync(lecturers, token);
        foreach (var lecturer in lecturers)
        {
            lecturer.UserId = await keycloakService.Register(new RegisterDto()
            {
                Email = lecturer.Contact.Email
            });
        }
        await dbContext.SaveChangesAsync(token);
    }
    private static readonly Dictionary<string, Quarter> QuarterMapper = new()
    {
        ["1-я неделя"] = Quarter.First,
        ["2-я неделя"] = Quarter.Second
    };
    private static readonly Dictionary<string, DayOfWeek> DayOfWeekMapper = new()
    {
        ["Понедельник"] = DayOfWeek.Monday,
        ["Вторник"] = DayOfWeek.Tuesday,
        ["Среда"] = DayOfWeek.Wednesday,
        ["Четверг"] = DayOfWeek.Thursday,
        ["Пятница"] = DayOfWeek.Friday,
        ["Суббота"] = DayOfWeek.Saturday,
        ["Воскресенье"] = DayOfWeek.Sunday,
    };
    private async Task SaveSchedule(List<TemplateData> templates, CancellationToken token = default)
    {
        foreach (var template in templates)
        {
            var version = Guid.NewGuid();
            var classroom = await dbContext.Classrooms
                .Where(x => x.Name == template.ClassroomName)
                .SingleAsync(token);
            var group = await dbContext.Groups
                .Where(x => x.Name == template.GroupName)
                .SingleAsync(token);
            var subject = await dbContext.Subjects
                .Where(x => x.Name == template.SubjectName)
                .SingleAsync(token);
            var period = await dbContext.Periods
                .Where(x => x.Name == template.PeriodName)
                .SingleAsync(token);
            var lecturer = await dbContext.Lecturers
                .Where(x => x.Person.FullName == template.LecturerName)
                .SingleAsync(token);
            await dbContext.Templates.AddAsync(new()
            {
                Version = version,
                ClassroomId = classroom.Id,
                Quarter = QuarterMapper[template.QuarterName],
                DayOfWeek = DayOfWeekMapper[template.DayOfWeekName],
                GroupId = group.Id,
                LecturerId = lecturer.Id,
                PeriodId = period.Id,
                SubjectId = subject.Id
            }, token);
        }
        await dbContext.SaveChangesAsync(token);
    }
    private async Task SaveStudents(List<StudentData> students, CancellationToken token = default)
    {
        foreach (var student in students)
        {
            var group = await dbContext.Groups
                .Where(x => x.Name == student.GroupName)
                .SingleAsync(token);
            var dbStudent = new Student()
            {
                GroupId = group.Id,
                Person = new Person()
                {
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    MiddleName = student.MiddleName
                },
                Contact = new Contact()
                {
                    Email = student.Email,
                    Phone = student.Phone
                }
            };
            await dbContext.Students.AddAsync(dbStudent, token);
            await keycloakService.Register(new RegisterDto()
            {
                Email = dbStudent.Contact.Email
            });
        }
    }
}