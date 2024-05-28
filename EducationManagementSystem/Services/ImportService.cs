using Ganss.Excel;

namespace EducationManagementSystem.Services;

public class StudentData
{
    [Column("Имя")]
    public string FirstName { get; set; } = default!;
    [Column("Фамилия")]
    public string LastName { get; set; } = default!;
    [Column("Отчество")]
    public string? MiddleName { get; set; } = default!;
    [Column("Номер телефона")]
    public string Phone { get; set; } = default!;
    [Column("Электронная почта")]
    public string Email { get; set; } = default!;
    [Column("Название группы")]
    public string GroupName { set; get; } = default!;
}
public class OrganizationData
{
    public class ClassroomData
    {
        public const string SheetName = "Аудитории";
        [Column("Название")]
        public string Name { get; set; } = default!;
    }
    public class GroupData
    {
        public const string SheetName = "Группы";
        [Column("Название")]
        public string Name { get; set; } = default!;
    }
    public class LecturerData
    {
        public const string SheetName = "Преподаватели";
        [Column("Имя")]
        public string FirstName { get; set; } = default!;
        [Column("Фамилия")]
        public string LastName { get; set; } = default!;
        [Column("Отчество")]
        public string? MiddleName { get; set; }
        [Column("Номер телефона")]
        public string Phone { get; set; } = default!;
        [Column("Электронная почта")]
        public string Email { get; set; } = default!;
    }
    public class SubjectData
    {
        public const string SheetName = "Предметы";
        [Column("Название")]
        public string Name { get; set; } = default!;
    }
    public class PeriodData
    {
        public const string SheetName = "Периоды проведения занятий";
        [Column("Начало")]
        public DateTime StartAt { get; set; }
        [Column("Конец")]
        public DateTime EndAt { get; set; }
    }
    public List<GroupData> Groups { get; set; } = default!;
    public List<SubjectData> Subjects { get; set; } = default!;
    public List<ClassroomData> Classrooms { get; set; } = default!;
    public List<PeriodData> Periods { get; set; } = default!;
    public List<LecturerData> Lecturers { get; set; } = default!;
}
public class TemplateData
{
    [Column("Квартал")]
    public string QuarterName { get; set; } = default!;
    [Column("День недели")]
    public string DayOfWeekName { get; set; } = default!;
    [Column("Название группы")]
    public string GroupName { get; set; } = default!;
    [Column("Период")]
    public string PeriodName { get; set; } = default!;
    [Column("ФИО преподавателя")]
    public string LecturerName { get; set; } = default!;
    [Column("Предмет")]
    public string SubjectName { get; set; } = default!;
    [Column("Аудитория")]
    public string ClassroomName { get; set; } = default!;
}
public class ImportService
{
    public List<StudentData> ParseStudents(Stream stream) => new ExcelMapper(stream).Fetch<StudentData>().ToList();
    public OrganizationData ParseOrganization(Stream stream)
    {
        var mapper = new ExcelMapper(stream);
        var groups = mapper.Fetch<OrganizationData.GroupData>(OrganizationData.GroupData.SheetName).ToList();
        var subjects = mapper.Fetch<OrganizationData.SubjectData>(OrganizationData.SubjectData.SheetName).ToList();
        var classrooms = mapper.Fetch<OrganizationData.ClassroomData>(OrganizationData.ClassroomData.SheetName).ToList();
        var periods = mapper.Fetch<OrganizationData.PeriodData>(OrganizationData.PeriodData.SheetName).ToList();
        var lecturers = mapper.Fetch<OrganizationData.LecturerData>(OrganizationData.LecturerData.SheetName).ToList();
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