using Microsoft.EntityFrameworkCore;

namespace EducationManagementSystem.Domain;

[Owned]
public class Person
{
    public Person()
    {
        FullName = LastName + ' ' + FirstName + (MiddleName is null ? string.Empty : ' ' + MiddleName);
    }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? MiddleName { get; set; }
    public string FullName { get; }
}