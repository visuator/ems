using Ganss.Excel;

namespace EducationManagementSystem.Services;

public class StudentData
{
    [Column("Имя")]
    public string FirstName { get; set; }
    [Column("Фамилия")]
    public string LastName { get; set; }
    [Column("Отчество")]
    public string? MiddleName { get; set; }
    [Column("Номер телефона")]
    public string Phone { get; set; }
    [Column("Электронная почта")]
    public string Email { get; set; }
    [Column("Название группы")]
    public string GroupName { set; get; }
}
public class OrganizationData
{
    public class ClassroomData
    {
        public const string SheetName = "Аудитории";
        [Column("Тип аудитории")]
        public string Size { get; set; }
        [Column("Название аудитории")]
        public string Name { get; set; } = default!;
    }
    public class GroupData
    {
        public const string SheetName = "Группы";
        [Column("Название группы")]
        public string Name { get; set; }
    }
    public class LecturerData
    {
        public const string SheetName = "Преподаватели";
        [Column("Имя")]
        public string FirstName { get; set; }
        [Column("Фамилия")]
        public string LastName { get; set; }
        [Column("Отчество")]
        public string? MiddleName { get; set; }
        [Column("Номер телефона")]
        public string Phone { get; set; }
        [Column("Электронная почта")]
        public string Email { get; set; }
    }
    public class SubjectData
    {
        public const string SheetName = "Предметы";
        [Column("Название предмета")]
        public string Name { get; set; }
    }
    public class PeriodData
    {
        public const string SheetName = "Периоды проведения занятий";
        [Column("Начало")]
        public TimeSpan StartAt { get; set; }
        [Column("Конец")]
        public TimeSpan EndAt { get; set; }
    }
    public class QuarterData
    {
        public const string SheetName = "Кварталы";
        [Column("Название квартала")]
        public string Name { get; set; }
    }
    public List<GroupData> Groups { get; set; }
    public List<SubjectData> Subjects { get; set; }
    public List<ClassroomData> Classrooms { get; set; }
    public List<PeriodData> Periods { get; set; }
    public List<QuarterData> Quarters { get; set; }
    public List<LecturerData> Lecturers { get; set; }
}
public class TemplateData
{
    [Column("Квартал")]
    public string QuarterName { get; set; }
    [Column("День недели")]
    public string DayOfWeek { get; set; }
    [Column("Название группы")]
    public string GroupName { get; set; }
    [Column("Период")]
    public string PeriodName { get; set; }
    [Column("ФИО преподавателя")]
    public string LecturerName { get; set; }
    [Column("Предмет")]
    public string SubjectName { get; set; }
    [Column("Аудитория")]
    public string ClassroomName { get; set; }
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
        var quarters = mapper.Fetch<OrganizationData.QuarterData>(OrganizationData.QuarterData.SheetName).ToList();
        var lecturers = mapper.Fetch<OrganizationData.LecturerData>(OrganizationData.LecturerData.SheetName).ToList();
        return new()
        {
            Groups = groups,
            Subjects = subjects,
            Classrooms = classrooms,
            Periods = periods,
            Quarters = quarters,
            Lecturers = lecturers
        };
    }
    public List<TemplateData> ParseSchedule(Stream stream) => new ExcelMapper(stream).Fetch<TemplateData>().ToList();
}