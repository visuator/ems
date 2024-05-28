namespace EducationManagementSystem.Domain;

public class Student : BaseEntity
{
    public Guid? UserId { get; set; }
    public Person Person { get; set; } = default!;
    public Contact Contact { get; set; } = default!;
    public Guid GroupId { get; set; }
    public Group Group { get; set; } = default!;
}