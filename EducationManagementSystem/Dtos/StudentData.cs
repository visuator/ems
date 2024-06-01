using Ganss.Excel;

namespace EducationManagementSystem.Controllers.Dtos;

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