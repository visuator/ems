namespace EducationManagementSystem.Domain;

public class Period : BaseEntity
{
    public string Name { get; set; } = default!;
    public TimeSpan StartAt { get; set; }
    public TimeSpan EndAt { get; set; }
}