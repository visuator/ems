namespace EducationManagementSystem.Controllers.Dtos;

public class GroupInfoDto
{
    public string Name { get; set; } = default!;
    public List<MarkDto> Marks { get; set; } = default!;
}