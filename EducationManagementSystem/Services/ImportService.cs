using EducationManagementSystem.Controllers.Dtos;
using Ganss.Excel;

namespace EducationManagementSystem.Services;

public class ImportService
{
    public List<StudentData> ParseStudents(Stream stream) => new ExcelMapper(stream).Fetch<StudentData>().ToList();
    public OrganizationData ParseOrganization(Stream stream)
    {
        var mapper = new ExcelMapper(stream);
        var groups = mapper.Fetch<GroupData>(GroupData.SheetName).ToList();
        var subjects = mapper.Fetch<SubjectData>(SubjectData.SheetName).ToList();
        var classrooms = mapper.Fetch<ClassroomData>(ClassroomData.SheetName).ToList();
        var periods = mapper.Fetch<PeriodData>(PeriodData.SheetName).ToList();
        var lecturers = mapper.Fetch<LecturerData>(LecturerData.SheetName).ToList();
        return new()
        {
            Groups = groups,
            Subjects = subjects,
            Classrooms = classrooms,
            Periods = periods,
            Lecturers = lecturers
        };
    }
    public List<TemplateData> ParseSchedule(Stream stream) => new ExcelMapper(stream).Fetch<TemplateData>().ToList();
}