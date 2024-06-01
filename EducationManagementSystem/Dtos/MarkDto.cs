namespace EducationManagementSystem.Controllers.Dtos;

public class MarkDto
{
    public Guid? Id { get; set; }
    public Guid StudentId { get; set; }
    public bool Passed { get; set; }
}