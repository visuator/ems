namespace EducationManagementSystem.Domain;

public class Lecturer : BaseEntity
{
    public Guid? UserId { get; set; }
    public Person Person { get; set; } = default!;
    public Contact Contact { get; set; } = default!;
}