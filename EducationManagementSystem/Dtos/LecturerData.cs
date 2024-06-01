using Ganss.Excel;

namespace EducationManagementSystem.Controllers.Dtos;

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