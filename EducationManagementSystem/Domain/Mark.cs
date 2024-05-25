namespace EducationManagementSystem.Domain;

public class Mark : BaseEntity
{
    public Guid LessonId { get; set; }
    public Lesson Lesson { get; set; } = default!;
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = default!;
    public MarkStatus Status { get; set; }
}