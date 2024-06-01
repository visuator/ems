using Ganss.Excel;

namespace EducationManagementSystem.Controllers.Dtos;

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