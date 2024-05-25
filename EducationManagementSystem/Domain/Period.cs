namespace EducationManagementSystem.Domain;

public class Period : BaseEntity
{
    public TimeSpan StartAt { get; set; }
    public TimeSpan EndAt { get; set; }
}