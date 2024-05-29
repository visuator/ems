using Microsoft.EntityFrameworkCore;

namespace EducationManagementSystem.Domain;

[Owned]
public class LessonFlow
{
    public string Theme { get; set; } = default!;
    public string Resources { get; set; } = default!;
}