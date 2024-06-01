using Ganss.Excel;

namespace EducationManagementSystem.Controllers.Dtos;

public class SubjectData
{
    public const string SheetName = "Предметы";
    [Column("Название")]
    public string Name { get; set; } = default!;
}