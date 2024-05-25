namespace EducationManagementSystem.Domain;

public class Classroom : BaseEntity
{
    public ClassroomSize Size { get; set; }
    public string Name { get; set; } = default!;
}