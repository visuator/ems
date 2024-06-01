using Ganss.Excel;

namespace EducationManagementSystem.Controllers.Dtos;

public class PeriodData
{
    public const string SheetName = "Периоды проведения занятий";
    [Column("Начало")]
    public DateTime StartAt { get; set; }
    [Column("Конец")]
    public DateTime EndAt { get; set; }
}