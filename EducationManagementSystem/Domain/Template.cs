namespace EducationManagementSystem.Domain;

public class Template : BaseEntity
{
    public Guid Version { get; set; }
    public Quarter Quarter { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public bool Published { get; set; }
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