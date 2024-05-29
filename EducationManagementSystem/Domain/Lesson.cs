namespace EducationManagementSystem.Domain;

public class Lesson : BaseEntity
{
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public Guid LecturerId { get; set; }
    public Lecturer Lecturer { get; set; } = default!;
    public Guid SubjectId { get; set; }
    public Subject Subject { get; set; } = default!;
    public Guid ClassroomId { get; set; }
    public Classroom Classroom { get; set; } = default!;
    public Guid GroupId { get; set; }
    public Group Group { get; set; } = default!;
    public LessonFlow Flow { get; set; } = default!;
    public List<MarkSession> Sessions { get; set; } = default!;
}