namespace EducationManagementSystem.Controllers.Dtos;

public class OrganizationData
{
    public List<GroupData> Groups { get; set; } = default!;
    public List<SubjectData> Subjects { get; set; } = default!;
    public List<ClassroomData> Classrooms { get; set; } = default!;
    public List<PeriodData> Periods { get; set; } = default!;
    public List<LecturerData> Lecturers { get; set; } = default!;
}