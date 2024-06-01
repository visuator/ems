namespace EducationManagementSystem.Controllers.Dtos;

public class LessonInfoDto
{
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public Guid LecturerId { get; set; }
    public Guid SubjectId { get; set; }
    public Guid ClassroomId { get; set; }
    public Guid GroupId { get; set; }
    public LessonFlowDto? Flow { get; set; }
}