namespace EducationManagementSystem.Domain;

public class TemplateGroup
{
    public Guid TemplateId { get; set; }
    public Template Template { get; set; } = default!;
    public Guid GroupId { get; set; }
    public Group Group { get; set; } = default!;
    public Guid PeriodId { get; set; }
    public Period Period { get; set; } = default!;
    public Guid LecturerId { get; set; }
    public Lecturer Lecturer { get; set; } = default!;
    public Guid SubjectId { get; set; }
    public Subject Subject { get; set; } = default!;
    public Guid ClassroomId { get; set; }
    public Classroom Classroom { get; set; } = default!;
}