namespace EducationManagementSystem.Controllers.Dtos;

public class ReplaceLessonDto
{
    public Guid LecturerId { get; set; }
    public Guid SubjectId { get; set; }
    public Guid ClassroomId { get; set; }
}