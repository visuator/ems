using Ganss.Excel;

namespace EducationManagementSystem.Controllers.Dtos;

public class ClassroomData
{
    public const string SheetName = "Аудитории";
    [Column("Название")]
    public string Name { get; set; } = default!;
}