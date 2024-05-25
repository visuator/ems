namespace EducationManagementSystem.Domain;

public class Template : BaseEntity
{
    public Guid Version { get; set; }
    public Quarter Quarter { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public List<TemplateGroup> Groups { get; set; } = default!;
}