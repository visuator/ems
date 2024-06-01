using Ganss.Excel;

namespace EducationManagementSystem.Controllers.Dtos;

public class GroupData
{
    public const string SheetName = "Группы";
    [Column("Название")]
    public string Name { get; set; } = default!;
}